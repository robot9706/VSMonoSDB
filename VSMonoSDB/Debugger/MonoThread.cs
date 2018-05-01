using System;
using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using static Microsoft.VisualStudio.VSConstants;
using VSMonoSDB.Debugging.Enumerators;

namespace VSMonoSDB.Debugging
{
    public class MonoThread : IDebugThread2
    {
        private MonoEngine _engine;
        private string _name;

        private ThreadInfo _monoThread;

        public long ID
        {
            get { return _monoThread.Id; }
        }

        public MonoThread(MonoEngine engine, string name, ThreadInfo monoThread)
        {
            _engine = engine;
            _name = name;

            _monoThread = monoThread;
        }

        public int EnumFrameInfo(uint dwFieldSpec, uint nRadix, out IEnumDebugFrameInfo2 ppEnum)
        {
            Backtrace trace = _engine.SoftDebugger.ActiveBacktrace; //_monoThread.Backtrace;

            FRAMEINFO[] frames = new FRAMEINFO[trace.FrameCount];

            for (int x = 0; x < trace.FrameCount; x++)
            {
                MonoStackFrame frame = new MonoStackFrame(_engine, this, trace.GetFrame(x));

                frames[x] = frame.GetFrameInfo(dwFieldSpec);
            }

            ppEnum = new MonoFrameInfoEnumerator(frames);

            return S_OK;
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = _name;

            return S_OK;
        }

        public int GetProgram(out IDebugProgram2 ppProgram)
        {
            ppProgram = _engine;

            return S_OK;
        }

        public int GetThreadId(out uint pdwThreadId)
        {
            pdwThreadId = (uint)_monoThread.Id;

            return S_OK;
        }

        public int GetThreadProperties(uint dwFields, THREADPROPERTIES[] ptp)
        {
            THREADPROPERTIES props = new THREADPROPERTIES();

            if ((dwFields & (uint)enum_THREADPROPERTY_FIELDS.TPF_ID) != 0)
            {
                props.dwThreadId = (uint)_monoThread.Id;
                props.dwFields |= (uint)enum_THREADPROPERTY_FIELDS.TPF_ID;
            }

            if ((dwFields & (uint)enum_THREADPROPERTY_FIELDS.TPF_SUSPENDCOUNT) != 0)
            {
                props.dwFields |= (uint)enum_THREADPROPERTY_FIELDS.TPF_SUSPENDCOUNT;
            }

            if ((dwFields & (uint)enum_THREADPROPERTY_FIELDS.TPF_STATE) != 0)
            {
                props.dwThreadState = (uint)enum_THREADSTATE.THREADSTATE_RUNNING;
                props.dwFields |= (uint)enum_THREADPROPERTY_FIELDS.TPF_STATE;
            }

            if ((dwFields & (uint)enum_THREADPROPERTY_FIELDS.TPF_PRIORITY) != 0)
            {
                props.bstrPriority = "Normal";
                props.dwFields |= (uint)enum_THREADPROPERTY_FIELDS.TPF_PRIORITY;
            }

            if ((dwFields & (uint)enum_THREADPROPERTY_FIELDS.TPF_NAME) != 0)
            {
                props.bstrName = _name;
                props.dwFields |= (uint)enum_THREADPROPERTY_FIELDS.TPF_NAME;
            }

            if ((dwFields & (uint)enum_THREADPROPERTY_FIELDS.TPF_LOCATION) != 0)
            {
                props.bstrLocation = _monoThread.Location;
                props.dwFields |= (uint)enum_THREADPROPERTY_FIELDS.TPF_LOCATION;
            }

            return S_OK;
        }

        public int Resume(out uint pdwSuspendCount)
        {
            pdwSuspendCount = 0;

            return E_NOTIMPL;
        }

        public int Suspend(out uint pdwSuspendCount)
        {
            pdwSuspendCount = 0;

            return E_NOTIMPL;
        }

        public int CanSetNextStatement(IDebugStackFrame2 pStackFrame, IDebugCodeContext2 pCodeContext)
        {
            return S_FALSE;
        }

        public int SetNextStatement(IDebugStackFrame2 pStackFrame, IDebugCodeContext2 pCodeContext)
        {
            return E_NOTIMPL;
        }

        public void SetActive()
        {
            _monoThread.SetActive();
        }

        #region Deprecated
        public int GetLogicalThread(IDebugStackFrame2 pStackFrame, out IDebugLogicalThread2 ppLogicalThread)
        {
            ppLogicalThread = null;

            return E_NOTIMPL;
        }

        public int SetThreadName(string pszName)
        {
            return E_NOTIMPL;
        }
        #endregion
    }
}
