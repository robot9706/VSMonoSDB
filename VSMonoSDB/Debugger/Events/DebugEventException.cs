using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using System;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventException : StopEvent, IDebugExceptionEvent2
    {
        public static Guid ID = new Guid("51A94113-8788-4A54-AE15-08B74FF922D0");

        private MonoEngine _engine;
        private ExceptionInfo _info;

        public DebugEventException(MonoEngine engine, ExceptionInfo info)
        {
            _engine = engine;
            _info = info;
        }

        public int GetException(EXCEPTION_INFO[] pExceptionInfo)
        {
            EXCEPTION_INFO info = new EXCEPTION_INFO();

            info.pProgram = _engine;
            info.bstrExceptionName = _info.Type;
            info.dwState = (uint)enum_EXCEPTION_STATE.EXCEPTION_STOP_FIRST_CHANCE;

            pExceptionInfo[0] = info;

            return S_OK;
        }

        public int GetExceptionDescription(out string pbstrDescription)
        {
            pbstrDescription = _info.Message;

            return S_OK;
        }

        public int CanPassToDebuggee()
        {
            return S_FALSE;
        }

        public int PassToDebuggee(int fPass)
        {
            fPass = S_FALSE;

            return S_OK;
        }
    }
}
