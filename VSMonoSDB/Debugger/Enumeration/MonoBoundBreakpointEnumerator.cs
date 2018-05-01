using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Enumerators
{
    public class MonoBoundBreakpointEnumerator : BaseEnumerator<IDebugBoundBreakpoint2, IEnumDebugBoundBreakpoints2>, IEnumDebugBoundBreakpoints2
    {
        public MonoBoundBreakpointEnumerator(IDebugBoundBreakpoint2[] bps) : base(bps)
        {
        }
    }
}
