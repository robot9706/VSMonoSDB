using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using VSMonoSDB.Debugging;

namespace VSMonoSDB.Tools
{
    public class DebuggerInstaller
    {
        private const string ENGINE_PATH = @"AD7Metrics\Engine\";
        private const string CLSID_PATH = @"CLSID\";

        public static bool InstallDebugger(IMessageLogger msg)
        {
            try
            {
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(@"CLSID\{" + DebuggerGuids.EngineIdGuid + "}");

                if (regKey != null) //Check if the debugger is already installed
                    return true;

                string location = typeof(MonoEngine).Assembly.Location;

                //Register the debugger assembly
                string regasm = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe";
                if (!Environment.Is64BitOperatingSystem)
                    regasm = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe";

                var p = new ProcessStartInfo(regasm, location);
                p.Verb = "runas";
                p.RedirectStandardOutput = true;
                p.UseShellExecute = false;
                p.CreateNoWindow = true;

                Process proc = Process.Start(p);
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    throw new UnauthorizedAccessException();
                }

                //Add the regsitry stuff for VS
                using (RegistryKey config = VSRegistry.RegistryRoot(__VsLocalRegistryType.RegType_Configuration))
                {
                    RegisterDebugEngine(location, config);
                }

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                msg.ErrorMessage("Unable to install " + DebuggerGuids.EngineName + " - Please run Visual Studio as administrator.", "Debugger installation");
            }
            catch (Exception ex)
            {
                msg.ErrorMessage("An error ocurred while installing " + DebuggerGuids.EngineName + ": \"" + ex.Message + "\"", "Debugger installation");
            }

            return false;
        }

        private static void RegisterDebugEngine(string dllPath, RegistryKey rootKey)
        {
            using (RegistryKey engine = rootKey.OpenSubKey(ENGINE_PATH, true))
            {
                string engineGuid = DebuggerGuids.EngineIdGuid.ToString("B").ToUpper();
                using (RegistryKey engineKey = engine.CreateSubKey(engineGuid))
                {
                    engineKey.SetValue("CLSID", DebuggerGuids.EngineIdGuid.ToString("B").ToUpper());
                    engineKey.SetValue("ProgramProvider", DebuggerGuids.ProgramProviderGuid.ToString("B").ToUpper());
                    engineKey.SetValue("Attach", 1, RegistryValueKind.DWord);
                    engineKey.SetValue("AddressBP", 0, RegistryValueKind.DWord);
                    engineKey.SetValue("AutoSelectPriority", 4, RegistryValueKind.DWord);
                    engineKey.SetValue("CallstackBP", 1, RegistryValueKind.DWord);
                    engineKey.SetValue("Name", DebuggerGuids.EngineName);
                    engineKey.SetValue("PortSupplier", DebuggerGuids.ProgramProviderGuid.ToString("B").ToUpper());
                    engineKey.SetValue("AlwaysLoadLocal", 1, RegistryValueKind.DWord);
                }
            }

            using (RegistryKey clsid = rootKey.OpenSubKey(CLSID_PATH, true))
            {
                using (RegistryKey clsidKey = clsid.CreateSubKey(DebuggerGuids.EngineIdGuid.ToString("B").ToUpper()))
                {
                    clsidKey.SetValue("Assembly", Assembly.GetExecutingAssembly().GetName().Name);
                    clsidKey.SetValue("Class", typeof(MonoEngine).FullName);
                    clsidKey.SetValue("InprocServer32", @"c:\windows\system32\mscoree.dll");
                    clsidKey.SetValue("CodeBase", dllPath);
                }

                using (RegistryKey programProviderKey = clsid.CreateSubKey(DebuggerGuids.ProgramProviderGuid.ToString("B").ToUpper()))
                {
                    programProviderKey.SetValue("Assembly", Assembly.GetExecutingAssembly().GetName().Name);
                    programProviderKey.SetValue("Class", typeof(MonoProgramProvider).FullName);
                    programProviderKey.SetValue("InprocServer32", @"c:\windows\system32\mscoree.dll");
                    programProviderKey.SetValue("CodeBase", dllPath);
                }
            }
        }
    }
}
