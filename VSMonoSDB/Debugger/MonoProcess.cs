using Microsoft.VisualStudio.Debugger.Interop;
using System;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoProcess : IDebugProcess3
    {
        private Guid _processID;
        private IDebugPort2 _port;

        public MonoProcess(Guid processID, IDebugPort2 port)
        {
            _processID = processID;
            _port = port;
        }

        public int Attach(IDebugEventCallback2 pCallback, Guid[] rgguidSpecificEngines, uint celtSpecificEngines, int[] rghrEngineAttach)
        {
            return S_OK;
        }

        public int CanDetach()
        {
            return S_OK;
        }

        public int CauseBreak()
        {
            return S_OK;
        }

        public int Continue(IDebugThread2 pThread)
        {
            return S_OK;
        }

        public int Detach()
        {
            return S_OK;
        }

        public int DisableENC(EncUnavailableReason reason)
        {
            return S_OK;
        }

        public int EnumPrograms(out IEnumDebugPrograms2 ppEnum)
        {
            ppEnum = null;

            return E_NOTIMPL;
        }

        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            ppEnum = null;

            return E_NOTIMPL;
        }

        public int Execute(IDebugThread2 pThread)
        {
            return S_OK;
        }

        public int GetAttachedSessionName(out string pbstrSessionName)
        {
            pbstrSessionName = "Session";

            return S_OK;
        }

        public int GetDebugReason(out uint pReason)
        {
            pReason = 0;

            return S_OK;
        }

        public int GetENCAvailableState(EncUnavailableReason[] pReason)
        {
            return S_OK;
        }

        public int GetEngineFilter(GUID_ARRAY[] pEngineArray)
        {
            return S_OK;
        }

        public int GetHostingProcessLanguage(out Guid pguidLang)
        {
            pguidLang = Guid.Empty;

            return S_OK;
        }

        public int GetInfo(uint Fields, PROCESS_INFO[] pProcessInfo)
        {
            return S_OK;
        }

        public int GetName(uint gnType, out string pbstrName)
        {
            pbstrName = string.Empty;

            return S_OK;
        }

        public int GetPhysicalProcessId(AD_PROCESS_ID[] pProcessId)
        {
            pProcessId[0].ProcessIdType = (uint)enum_AD_PROCESS_ID.AD_PROCESS_ID_GUID;
            pProcessId[0].guidProcessId = _processID;

            return S_OK;
        }

        public int GetPort(out IDebugPort2 ppPort)
        {
            ppPort = _port;

            return S_OK;
        }

        public int GetProcessId(out Guid pguidProcessId)
        {
            pguidProcessId = _processID;

            return S_OK;
        }

        public int GetServer(out IDebugCoreServer2 ppServer)
        {
            ppServer = null;

            return S_OK;
        }

        public int SetHostingProcessLanguage(ref Guid guidLang)
        {
            return S_OK;
        }

        public int Step(IDebugThread2 pThread, uint sk, uint Step)
        {
            return S_OK;
        }

        public int Terminate()
        {
            return S_OK;
        }
    }
}
