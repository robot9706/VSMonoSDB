using static Microsoft.VisualStudio.VSConstants;
using Microsoft.VisualStudio.Debugger.Interop;
using System;
using Mono.Debugging.Client;
using VSMonoSDB.Debugging.Enumerators;
using System.Runtime.InteropServices;

namespace VSMonoSDB.Debugging
{
    public class MonoPendingBreakpoint : IDebugPendingBreakpoint2
    {
        private MonoEngine _engine;

        private MonoBoundBreakpoint _boundBreakpoint;

        private IDebugBreakpointRequest2 _request;
        private BP_REQUEST_INFO _requestInfo;

        private TextPositionInfo _position;

        private bool _deleted;
        private bool _enabled;

        private Breakpoint _monoBreakpoint;

        public MonoPendingBreakpoint(MonoEngine engine, IDebugBreakpointRequest2 req)
        {
            _engine = engine;

            _request = req;

            BP_REQUEST_INFO[] inf = new BP_REQUEST_INFO[1];
            req.GetRequestInfo((uint)enum_BPREQI_FIELDS.BPREQI_ALLFIELDS, inf);
            _requestInfo = inf[0];
        }

        /// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int Bind()
        {
            _position = new TextPositionInfo(_requestInfo);
			_position.FixPosition();

            _monoBreakpoint = _engine.BreakpointManager.CreateBreakpoint(_position, this);
            SetCondition(_requestInfo.bpCondition);
            SetPassCount(_requestInfo.bpPassCount);

            if (!_enabled)
            {
                _monoBreakpoint.Enabled = true;
                _enabled = true;
            }

            MonoDocumentContext docCtx = new MonoDocumentContext(_position, new Debugging.MonoMemoryContext(null, 0));
            _boundBreakpoint = new MonoBoundBreakpoint(this, new MonoBreakpointResolution(0, docCtx));

            _engine.Callback.OnBreakpointBound(this);

            return S_OK;
        }

        public int CanBind(out IEnumDebugErrorBreakpoints2 ppErrorEnum)
        {
            ppErrorEnum = null;

            if (_requestInfo.bpLocation.bpLocationType == (uint)enum_BP_LOCATION_TYPE.BPLT_CODE_FILE_LINE)
                return S_OK;

            return S_FALSE;
        }

        public int Delete()
        {
            if (_deleted)
                return S_OK;

            _deleted = true;

            _engine.BreakpointManager.RemoveBreakpoint(_monoBreakpoint);

            _boundBreakpoint.Delete();

            return S_OK;
        }

        public int Enable(int fEnable)
        {
            _enabled = (fEnable > 0);

            if (_monoBreakpoint != null)
                _monoBreakpoint.Enabled = _enabled;

            if (_boundBreakpoint != null)
                _boundBreakpoint.Enable(fEnable);

            return S_OK;
        }

        public int EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            ppEnum = new MonoBoundBreakpointEnumerator(new IDebugBoundBreakpoint2[] { _boundBreakpoint });

            return S_OK;
        }

        public int EnumErrorBreakpoints(uint bpErrorType, out IEnumDebugErrorBreakpoints2 ppEnum)
        {
            ppEnum = null;

            return S_OK;
        }

        public int GetBreakpointRequest(out IDebugBreakpointRequest2 ppBPRequest)
        {
            ppBPRequest = _request;

            return S_OK;
        }

        public int GetState(PENDING_BP_STATE_INFO[] pState)
        {
            if (_deleted)
                pState[0].state = (uint)enum_BP_STATE.BPS_DELETED;
            else if (_enabled)
                pState[0].state = (uint)enum_BP_STATE.BPS_ENABLED;
            else
                pState[0].state = (uint)enum_BP_STATE.BPS_DISABLED;

            return S_OK;
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            _monoBreakpoint.ConditionExpression = bpCondition.bstrCondition;
            _monoBreakpoint.BreakIfConditionChanges = (bpCondition.styleCondition == (uint)enum_BP_COND_STYLE.BP_COND_WHEN_CHANGED);

            return S_OK;
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            _monoBreakpoint.HitCount = (int)bpPassCount.dwPassCount;

            switch(bpPassCount.stylePassCount)
            {
                case (uint)enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_EQUAL:
                    _monoBreakpoint.HitCountMode = HitCountMode.EqualTo;
                    break;
                case (uint)enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_EQUAL_OR_GREATER:
                    _monoBreakpoint.HitCountMode = HitCountMode.GreaterThanOrEqualTo;
                    break;
                case (uint)enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_MOD:
                    _monoBreakpoint.HitCountMode = HitCountMode.MultipleOf;
                    break;
                case (uint)enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_NONE:
                    _monoBreakpoint.HitCountMode = HitCountMode.None;
                    break;
            }

            return S_OK;
        }

        public int Virtualize(int fVirtualize)
        {
            return S_OK;
        }
    }
}
