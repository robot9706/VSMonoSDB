using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSMonoSDB.Tools;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.Net;

namespace VSMonoSDB
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class MainPackage : Package, IMessageLogger
    {
        public const string PackageGuidString = "67654b5e-5daa-480f-947c-39d43820a23f";

		public static DebugInfo DebugTarget; //Hackish way lol, TODO: Find a better way to pass data to the DE

		public SolutionTools SolutionTools;
		public MonoTools MonoTools;

		public Settings Settings
		{
			get;
			private set;
		}

        public MainPackage()
        {
        }

        protected override void Initialize()
        {
            SolutionTools = new SolutionTools(this);
			MonoTools = new MonoTools(this);

			InitMenu();
			InitSettings();
			
			base.Initialize();
        }

		#region Settings
		private WritableSettingsStore GetWritableSettingsStore()
        {
			ShellSettingsManager shellSettingsManager = new ShellSettingsManager(this);
            return shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
        }

		private void InitSettings()
		{
			Settings = new Settings(GetWritableSettingsStore());
		}
		#endregion

		#region Menu commands
		public readonly Guid MenuCommandSet = new Guid("ddfe7fa8-8bad-46b8-b97c-8dbbc000c7fd");

		public const int DebugLocalhostCommandID = 0x0100;
		public const int SettingsCommandID = 0x0101;
		public const int DebugRemoteCommandID = 0x0102;

		private void InitMenu()
		{
			OleMenuCommandService commandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService == null)
				return;

			//Debug localhost menu
			{
				CommandID menuCommandID = new CommandID(MenuCommandSet, DebugLocalhostCommandID);
				MenuCommand menuItem = new MenuCommand(MenuItem_DebugLocalhost, menuCommandID);
				commandService.AddCommand(menuItem);
			}

			//Debug remote menu
			{
				CommandID menuCommandID = new CommandID(MenuCommandSet, DebugRemoteCommandID);
				MenuCommand menuItem = new MenuCommand(MenuItem_DebugRemote, menuCommandID);
				commandService.AddCommand(menuItem);
			}

			//Settings menu
			{
				CommandID menuCommandID = new CommandID(MenuCommandSet, SettingsCommandID);
				MenuCommand menuItem = new MenuCommand(MenuItem_Settings, menuCommandID);
				commandService.AddCommand(menuItem);
			}
		}

		private void MenuItem_DebugLocalhost(object sender, EventArgs e)
		{
			if (!CheckMonoSettings())
			{
				return;
			}

			SolutionTools.DebugStartupProject(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Settings.DebugPort));
		}

		private void MenuItem_DebugRemote(object sender, EventArgs e)
		{
			if (!CheckMonoSettings())
			{
				return;
			}

			using (ConnectForm cf = new VSMonoSDB.ConnectForm(this))
			{
				if (cf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					SolutionTools.DebugStartupProject(cf.DebugTarget);
				}
			}
		}

		private void MenuItem_Settings(object sender, EventArgs e)
		{
			ShowSettings();
		}
		#endregion

		#region Tools
		private void ShowSettings()
		{
			SettingsForm settings = new SettingsForm(this);
			settings.ShowDialog();
		}

		private bool CheckMonoSettings()
		{
			if (!MonoTools.Validate())
			{
				if (VsShellUtilities.PromptYesNo("The Mono path setting is invalid, do you want to update it now?", "Unable to find Mono", OLEMSGICON.OLEMSGICON_WARNING, FindService<IVsUIShell>()))
				{
					ShowSettings();

					if (!MonoTools.Validate())
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}

			return true;
		}

		public T FindService<T>()
		{
			return (T)GetService(typeof(T));
		}
		#endregion

		#region IMessageLogger
		public void ErrorMessage(string error, string title)
        {
            VsShellUtilities.ShowMessageBox(
               this,
               error,
               title,
               OLEMSGICON.OLEMSGICON_CRITICAL,
               OLEMSGBUTTON.OLEMSGBUTTON_OK,
               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public void InfoMessage(string info, string title)
        {
            VsShellUtilities.ShowMessageBox(
                this,
                info,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
		#endregion
	}
}
