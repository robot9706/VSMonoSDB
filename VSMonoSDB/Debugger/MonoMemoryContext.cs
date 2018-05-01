using Microsoft.VisualStudio.Debugger.Interop;
using System;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoMemoryContext : IDebugCodeContext2
    {
        private MonoDocumentContext _document;
        private uint _address;
        
        public MonoMemoryContext(MonoDocumentContext doc, uint address)
        {
            _document = doc;
            _address = address;
        }

        public int Add(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = new MonoMemoryContext(_document, (uint)(_address + dwCount));

            return S_OK;
        }

        public int Subtract(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = new MonoMemoryContext(_document, (uint)(_address - dwCount));

            return S_OK;
        }

        public int Compare(uint Compare, IDebugMemoryContext2[] rgpMemoryContextSet, uint dwMemoryContextSetLen, out uint pdwMemoryContext)
        {
            pdwMemoryContext = uint.MaxValue;

            for (uint c = 0; c < dwMemoryContextSetLen; c++)
            {
                MonoMemoryContext compareTo = rgpMemoryContextSet[c] as MonoMemoryContext;
                if (compareTo == null)
                    continue;

                bool result;

                switch ((enum_CONTEXT_COMPARE)Compare)
                {
                    case enum_CONTEXT_COMPARE.CONTEXT_EQUAL:
                        result = _address == compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN:
                        result = _address < compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN:
                        result = _address > compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN_OR_EQUAL:
                        result = _address <= compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN_OR_EQUAL:
                        result = _address >= compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_SAME_SCOPE:
                    case enum_CONTEXT_COMPARE.CONTEXT_SAME_FUNCTION:
                        result = _address == compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_SAME_MODULE:
                        result = _address == compareTo._address;
                        break;

                    case enum_CONTEXT_COMPARE.CONTEXT_SAME_PROCESS:
                        result = true;
                        break;

                    default:
                        return E_NOTIMPL;
                }

                if (result)
                {
                    pdwMemoryContext = c;
                    return S_OK;
                }
            }

            return S_FALSE;
        }

        public int GetDocumentContext(out IDebugDocumentContext2 ppSrcCxt)
        {
            ppSrcCxt = _document;

            return S_OK;
        }

        public int GetInfo(uint dwFields, CONTEXT_INFO[] pinfo)
        {
            pinfo[0].dwFields = 0;

            if ((dwFields & (uint)enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS) != 0)
            {
                pinfo[0].bstrAddress = _address.ToString();
                pinfo[0].dwFields |= (uint)enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS;
            }

            return S_OK;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            if (_document != null)
            {
                return _document.GetLanguageInfo(ref pbstrLanguage, ref pguidLanguage);
            }

            return S_FALSE;
        }

        public int GetName(out string pbstrName)
        {
            if (_document != null)
            {
                return _document.GetName((uint)enum_GETNAME_TYPE.GN_FILENAME, out pbstrName);
            }

            pbstrName = null;
            return S_FALSE;
        }
    }
}
