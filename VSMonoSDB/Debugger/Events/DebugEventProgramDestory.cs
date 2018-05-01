using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventProgramDestory : AsyncEvent, IDebugProgramDestroyEvent2
    {
        public static Guid ID = new Guid("E147E9E3-6440-4073-A7B7-A65592C714B5");

        private uint _exitCode;

        public DebugEventProgramDestory(uint exitCode)
        {
            _exitCode = exitCode;
        }

        public int GetExitCode(out uint pdwExit)
        {
            pdwExit = _exitCode;

            return VSConstants.S_OK;
        }
    }
}
