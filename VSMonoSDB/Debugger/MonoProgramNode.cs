using System;
using Microsoft.VisualStudio.Debugger.Interop;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoProgramNode : IDebugProgramNode2
    {
        private Guid _processID;

        public MonoProgramNode(Guid processID)
        {
            _processID = processID;
        }

        public int GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            pbstrEngine = DebuggerGuids.EngineName;
            pguidEngine = DebuggerGuids.EngineIdGuid;

            return S_OK;
        }

        public int GetHostName(uint dwHostNameType, out string pbstrHostName)
        {
            pbstrHostName = null;

            return S_OK;
        }

        public int GetHostPid(AD_PROCESS_ID[] pHostProcessId)
        {
            pHostProcessId[0].ProcessIdType = (uint)enum_AD_PROCESS_ID.AD_PROCESS_ID_GUID;
            pHostProcessId[0].guidProcessId = _processID;

            return S_OK;
        }

        public int GetProgramName(out string pbstrProgramName)
        {
            pbstrProgramName = null;

            return S_OK;
        }

        #region Deprecated
        public int Attach_V7(IDebugProgram2 pMDMProgram, IDebugEventCallback2 pCallback, uint dwReason)
        {
            return E_NOTIMPL;
        }

        public int DetachDebugger_V7()
        {
            return E_NOTIMPL;
        }

        public int GetHostMachineName_V7(out string pbstrHostMachineName)
        {
            pbstrHostMachineName = null;

            return E_NOTIMPL;
        }
        #endregion
    }
}
