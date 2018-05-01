using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VSMonoSDB.Debugging;

namespace VSMonoSDB.Tools
{
    public class SolutionTools
    {
        private DTE _dte;
        private MainPackage _package;

        public SolutionTools(MainPackage package)
        {
            _package = package;
			_dte = package.FindService<DTE>();
		}

        public Project GetStartupProject()
        {
            //Get the first startup project
            SolutionBuild2 buildInfo = (SolutionBuild2)_dte.Solution.SolutionBuild;

            if (buildInfo.StartupProjects == null)
                return null;

            string firstStartupProject = ((Array)buildInfo.StartupProjects).Cast<string>().First();

            //Find the actual project
            Projects projectList = _dte.Solution.Projects;
            IEnumerator list = projectList.GetEnumerator();

            while (list.MoveNext())
            {
                Project proj = list.Current as Project;
                if (proj == null)
                    continue;

                if (proj.Kind == ProjectKinds.vsProjectKindSolutionFolder) //Check if the project is a solution folder, not an actual project
                {
                    Project find = EnumerateSubprojects(proj, firstStartupProject);
                    if (find != null)
                        return find;
                }
                else
                {
                    if (proj.UniqueName == firstStartupProject)
                        return proj;
                }
            }

            return null;
        }

        private Project EnumerateSubprojects(Project solutionFolder, string findName)
        {
            for (int x = 1; x <= solutionFolder.ProjectItems.Count; x++)
            {
                Project subProject = solutionFolder.ProjectItems.Item(x).SubProject;
                if (subProject == null)
                    continue;

                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    Project find = EnumerateSubprojects(subProject, findName);
                    if (find != null)
                        return find;
                }
                else
                {
                    if (subProject.UniqueName == findName)
                        return subProject;
                }
            }

            return null;
        }

        public string GetOutputFolder(Project project)
        {
            string buildPath = project.Properties.Item("FullPath").Value.ToString();
            string outputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();

            return Path.Combine(buildPath, outputPath);
        }

        public string GetOutputFile(Project project)
        {
            string buildPath = project.Properties.Item("FullPath").Value.ToString();
            string outputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputFile = project.Properties.Item("OutputFileName").Value.ToString();

            return Path.Combine(buildPath, outputPath, outputFile);
        }

        public void StartDebugging(Project project, IMessageLogger msg)
        {
            string file = GetOutputFile(project);
            string folder = GetOutputFolder(project);

            ServiceProvider serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)_dte);

            IntPtr debugInfo = GetDebugInfo(string.Empty, file, folder);
            try
            {
                IVsDebugger debugger = (IVsDebugger)serviceProvider.GetService((typeof(SVsShellDebugger)));
                int launchResult = debugger.LaunchDebugTargets(1, debugInfo);

                Marshal.ThrowExceptionForHR(launchResult);
            }
            catch (Exception ex)
            {
                IVsUIShell uiShell = (IVsUIShell)serviceProvider.GetService(typeof(SVsUIShell));

                string message;
                uiShell.GetErrorInfo(out message);

                if (!string.IsNullOrEmpty(message))
                {
                    msg.ErrorMessage("Unable to start debugger: \"" + message + "\"", "Debugger");
                }
                else if (!string.IsNullOrEmpty(ex.Message))
                {
                    msg.ErrorMessage("Unable to start debugger: \"" + ex.Message + "\"", "Debugger");
                }
            }
            finally
            {
                if (debugInfo != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(debugInfo);
            }
        }

        private IntPtr GetDebugInfo(string args, string targetFile, string outputDirectory)
        {
			VsDebugTargetInfo info = new VsDebugTargetInfo();

            info.cbSize = (uint)Marshal.SizeOf(info);

            info.dlo = DEBUG_LAUNCH_OPERATION.DLO_CreateProcess; //Set "clsidCustom" to tell VS which debugger we want
            info.clsidCustom = DebuggerGuids.EngineIdGuid;

            info.bstrExe = targetFile;
            info.bstrCurDir = outputDirectory;
            info.bstrArg = args;
            info.bstrRemoteMachine = null;
            info.grfLaunch = (uint)__VSDBGLAUNCHFLAGS.DBGLAUNCH_StopDebuggingOnEnd;
			info.fSendStdoutToOutputWindow = 1;
            info.grfLaunch = 0;

            IntPtr pInfo = Marshal.AllocCoTaskMem((int)info.cbSize);
            Marshal.StructureToPtr(info, pInfo, false);
            return pInfo;
        }

        public void DebugStartupProject(IPEndPoint target)
        {
            Project startup = GetStartupProject();
            if (startup == null)
            {
                _package.InfoMessage("No startup project selected.", "Startup");
                return;
            }

            if (startup.CodeModel.Language != CodeModelLanguageConstants.vsCMLanguageCSharp) //Is the project C#?
            {
                _package.InfoMessage("The startup project is not a C# project.", "Startup");
                return;
            }

            string file = GetOutputFile(startup);
            if (!File.Exists(file)) //Make sure the project is built
            {
                _package.InfoMessage("The output file is not found, please build your project!", "Startup");
                return;
            }

            //Prepare the MDB (just to make sure)
            _package.MonoTools.UpdateMDB(file);

            //Try to install the debugger
            if (!DebuggerInstaller.InstallDebugger(_package))
                return;

			//Set the info for the debugger
			MainPackage.DebugTarget = new DebugInfo()
			{
				Target = target
			};

            //Start the debugger
            StartDebugging(startup, _package);
        }
    }
}
