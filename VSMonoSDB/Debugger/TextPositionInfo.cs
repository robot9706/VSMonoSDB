using Microsoft.VisualStudio.Debugger.Interop;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace VSMonoSDB.Debugging
{
    public class TextPositionInfo
    {
        public string FilePath
        {
            get;
            private set;
        }

        public int StartLine
        {
            get;
            private set;
        }

        public int StartColumn
        {
            get;
            private set;
        }

        public int EndLine
        {
            get;
            private set;
        }

        public int EndColumn
        {
            get;
            private set;
        }

        public TextPositionInfo(string file, int line, int column)
        {
            FilePath = file;
            EndLine = StartLine = line;
            StartColumn = column;

            EndColumn = int.MaxValue;
        }

        public TextPositionInfo(IDebugDocumentPosition2 docPosition)
        {
            Create(docPosition);
        }

        public TextPositionInfo(BP_REQUEST_INFO reqInfo)
        {
            IDebugDocumentPosition2 docPosition = (IDebugDocumentPosition2)Marshal.GetObjectForIUnknown(reqInfo.bpLocation.unionmember2);
            Create(docPosition);
            Marshal.ReleaseComObject(docPosition);
        }

        private void Create(IDebugDocumentPosition2 docPosition)
        {
            string path;
            TEXT_POSITION[] tstart = new TEXT_POSITION[1];
            TEXT_POSITION[] tend = new TEXT_POSITION[1];

            docPosition.GetFileName(out path);
            docPosition.GetRange(tstart, tend);

            FilePath = path;
            StartLine = (int)tstart[0].dwLine;
            StartColumn = (int)tstart[0].dwColumn;
            EndLine = (int)tend[0].dwLine;
            EndColumn = (int)tend[0].dwColumn;
        }

		public void FixPosition()
		{
			if (!File.Exists(FilePath))
				return;

			string line = File.ReadLines(FilePath).Skip(StartLine).Take(1).First();

			line = line.Trim();

			if (line.EndsWith("{"))
			{
				StartLine++;
				EndLine++;
			}
			else if (line.EndsWith("}"))
			{
				StartLine--;
				EndLine--;
			}
		}
    }
}
