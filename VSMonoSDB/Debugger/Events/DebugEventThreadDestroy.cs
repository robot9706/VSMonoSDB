using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventThreadDestroy : AsyncEvent, IDebugThreadDestroyEvent2
    {
        public static Guid ID = new Guid("0f7f24c1-74d9-4ea6-a3ea-7edb2d81441d");

        public int GetExitCode(out uint pdwExit)
        {
            pdwExit = 0;

            return VSConstants.S_OK;
        }
    }
}
