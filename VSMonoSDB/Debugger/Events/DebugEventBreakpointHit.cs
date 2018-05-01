using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventBreakpointHit : StopEvent, IDebugBreakpointEvent2
    {
        public static Guid ID = new Guid("501C1E21-C557-48B8-BA30-A1EAB0BC4A74");

        private MonoPendingBreakpoint _breakpoint;

        public DebugEventBreakpointHit(MonoPendingBreakpoint breakpoint)
        {
            _breakpoint = breakpoint;
        }

        public int EnumBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            return _breakpoint.EnumBoundBreakpoints(out ppEnum);
        }
    }
}
