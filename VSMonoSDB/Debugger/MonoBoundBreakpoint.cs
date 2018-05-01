using Microsoft.VisualStudio.Debugger.Interop;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    class MonoBoundBreakpoint : IDebugBoundBreakpoint2
    {
        private MonoPendingBreakpoint _pendingBreakpoint;
        private MonoBreakpointResolution _resolution;

        public MonoBoundBreakpoint(MonoPendingBreakpoint pending, MonoBreakpointResolution res)
        {
            _pendingBreakpoint = pending;
            _resolution = res;
        }

        public int Delete()
        {
            return S_OK;
        }

        public int Enable(int fEnable)
        {
            return S_OK;
        }

        public int GetBreakpointResolution(out IDebugBreakpointResolution2 ppBPResolution)
        {
            ppBPResolution = _resolution;

            return S_OK;
        }

        public int GetHitCount(out uint pdwHitCount)
        {
            pdwHitCount = 0;

            return S_OK;
        }

        public int GetPendingBreakpoint(out IDebugPendingBreakpoint2 ppPendingBreakpoint)
        {
            ppPendingBreakpoint = _pendingBreakpoint;

            return S_OK;
        }

        public int GetState(out uint pState)
        { 
            pState = (uint)enum_BP_STATE.BPS_ENABLED;
            return S_OK;
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            return S_OK;
        }

        public int SetHitCount(uint dwHitCount)
        {
            return S_OK;
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            return S_OK;
        }
    }
}
