using System;
using Microsoft.VisualStudio.Debugger.Interop;
using static Microsoft.VisualStudio.VSConstants;
using System.Runtime.InteropServices;

namespace VSMonoSDB.Debugging
{
    [ComVisible(true)]
    [Guid(DebuggerGuids.ProgramProviderString)]
    public class MonoProgramProvider : IDebugProgramProvider2
    {
        public int GetProviderProcessData(uint Flags, IDebugDefaultPort2 pPort, AD_PROCESS_ID ProcessId, CONST_GUID_ARRAY EngineFilter, PROVIDER_PROCESS_DATA[] pProcess)
        {
            return S_FALSE;
        }

        public int GetProviderProgramNode(uint Flags, IDebugDefaultPort2 pPort, AD_PROCESS_ID ProcessId, ref Guid guidEngine, ulong programId, out IDebugProgramNode2 ppProgramNode)
        {
            ppProgramNode = null;
            return S_FALSE;
        }

        public int SetLocale(ushort wLangID)
        {
            return S_OK;
        }

        public int WatchForProviderEvents(uint Flags, IDebugDefaultPort2 pPort, AD_PROCESS_ID ProcessId, CONST_GUID_ARRAY EngineFilter, ref Guid guidLaunchingEngine, IDebugPortNotify2 pEventCallback)
        {
            return S_OK;
        }
    }
}
