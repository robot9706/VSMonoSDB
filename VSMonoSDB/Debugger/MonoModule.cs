using Microsoft.VisualStudio.Debugger.Interop;
using System.IO;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoModule : IDebugModule3
    {
        private MonoEngine _engine;

        public MonoModule(MonoEngine engine)
        {
            _engine = engine;
        }

        public int GetInfo(uint dwFields, MODULE_INFO[] pinfo)
        {
            var info = new MODULE_INFO();

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_NAME) != 0)
            {
                info.m_bstrName = Path.GetFileName(_engine.SoftDebugger.Session.VirtualMachine.RootDomain.GetEntryAssembly().Location);
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_NAME;
            }

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_URL) != 0)
            {
                info.m_bstrUrl = _engine.SoftDebugger.Session.VirtualMachine.RootDomain.GetEntryAssembly().Location;
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_URL;
            }

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_LOADADDRESS) != 0)
            {
                info.m_addrLoadAddress = 0;
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_LOADADDRESS;
            }

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_PREFFEREDADDRESS) != 0)
            {
                info.m_addrPreferredLoadAddress = 0;
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_PREFFEREDADDRESS;
            }

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_SIZE) != 0)
            {
                info.m_dwSize = 0;
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_SIZE;
            }

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_LOADORDER) != 0)
            {
                info.m_dwLoadOrder = 0;
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_LOADORDER;
            }

            if ((dwFields & (uint)enum_MODULE_INFO_FIELDS.MIF_FLAGS) != 0)
            {
                info.m_dwModuleFlags = (uint)enum_MODULE_FLAGS.MODULE_FLAG_SYMBOLS;
                info.dwValidFields |= (uint)enum_MODULE_INFO_FIELDS.MIF_FLAGS;
            }

            pinfo[0] = info;

            return S_OK;
        }

        public int GetSymbolInfo(uint dwFields, MODULE_SYMBOL_SEARCH_INFO[] pinfo)
        {
            return S_OK;
        }

        public int IsUserCode(out int pfUser)
        {
            pfUser = 1;

            return S_OK;
        }

        public int LoadSymbols()
        {
            return E_NOTIMPL;
        }

        public int SetJustMyCodeState(int fIsUserCode)
        {
            return E_NOTIMPL;
        }

        #region Deprecated
        public int ReloadSymbols_Deprecated(string pszUrlToSymbols, out string pbstrDebugMessage)
        {
            pbstrDebugMessage = null;

            return E_NOTIMPL;
        }
        #endregion
    }
}
