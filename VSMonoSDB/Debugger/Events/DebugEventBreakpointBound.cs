using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventBreakpointBound : AsyncEvent, IDebugBreakpointBoundEvent2
    {
        public static Guid ID = new Guid("1dddb704-cf99-4b8a-b746-dabb01dd13a0");

        private MonoPendingBreakpoint _pending;

        public DebugEventBreakpointBound(MonoPendingBreakpoint pending)
        {
            _pending = pending;
        }

        public int EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            return _pending.EnumBoundBreakpoints(out ppEnum);
        }

        public int GetPendingBreakpoint(out IDebugPendingBreakpoint2 ppPendingBP)
        {
            ppPendingBP = _pending;

            return VSConstants.S_OK;
        }
    }
}
