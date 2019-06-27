using System.Diagnostics;
using System.IO;

namespace VSMonoSDB.Tools
{
    public class MonoTools
    {
		private MainPackage _package;

		private string ToolPDB2MDB
		{
			get
			{
				return Path.Combine(_package.Settings.MonoPath, "bin\\pdb2mdb.bat");
			}
		}

		public MonoTools(MainPackage package)
		{
			_package = package;
		}

		public bool Validate()
		{
			string monoPath = _package.Settings.MonoPath;

			if (string.IsNullOrEmpty(monoPath) || !Directory.Exists(monoPath))
			{
				return false;
			}

			return File.Exists(ToolPDB2MDB);
		}

        public bool UpdateMDB(string assembly)
        {
            string mdbPath = assembly + ".mdb";
            if (File.Exists(mdbPath))
            {
                FileInfo dllFile = new FileInfo(assembly);
                FileInfo mdbFile = new FileInfo(mdbPath);

                if (dllFile.LastWriteTime < mdbFile.LastWriteTime)
                    return true; //The MDB is up to date
            }

            ProcessStartInfo info = new ProcessStartInfo(ToolPDB2MDB, assembly);

            info.CreateNoWindow = true;
			info.UseShellExecute = true;

            Process proc = Process.Start(info);
            proc.WaitForExit();

			return (proc.ExitCode == 0);
        }
    }
}
