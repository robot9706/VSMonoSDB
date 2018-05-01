using Microsoft.VisualStudio.Debugger.Interop;
using System;
using System.IO;
using VSMonoSDB.Debugging.Enumerators;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoDocumentContext : IDebugDocumentContext2
    {
        private TextPositionInfo _info;
        private MonoMemoryContext _memory;

        public MonoDocumentContext(TextPositionInfo info, MonoMemoryContext memory)
        {
            _info = info;
            _memory = memory;
        }

        public int Compare(uint Compare, IDebugDocumentContext2[] rgpDocContextSet, uint dwDocContextSetLen, out uint pdwDocContext)
        {
            pdwDocContext = 0;

            return E_NOTIMPL;
        }

        public int EnumCodeContexts(out IEnumDebugCodeContexts2 ppEnumCodeCxts)
        {
            ppEnumCodeCxts = new MonoCodeContextEnumerator(new IDebugCodeContext2[] { _memory });

            return S_OK;
        }

        public int GetDocument(out IDebugDocument2 ppDocument)
        {
            ppDocument = null;

            return E_NOTIMPL;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pguidLanguage = new Guid("{694DD9B6-B865-4C5B-AD85-86356E9C88DC}"); //C# guid
            pbstrLanguage = "C#";
            return S_OK;
        }

        public int GetName(uint gnType, out string pbstrFileName)
        {
            enum_GETNAME_TYPE getType = (enum_GETNAME_TYPE)gnType;

            switch (getType)
            {
                case enum_GETNAME_TYPE.GN_TITLE:
                case enum_GETNAME_TYPE.GN_NAME:
                case enum_GETNAME_TYPE.GN_BASENAME:
                    pbstrFileName = Path.GetFileName(_info.FilePath);
                    break;
                case enum_GETNAME_TYPE.GN_FILENAME:
                    pbstrFileName = _info.FilePath;
                    break;
                default:
                    pbstrFileName = null;
                    return E_NOTIMPL;
            }

            return S_OK;
        }

        public int GetSourceRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            pBegPosition[0].dwLine = (uint)_info.StartLine;
            pBegPosition[0].dwColumn = (uint)_info.StartColumn;

            pEndPosition[0].dwLine = (uint)_info.EndLine;
            pEndPosition[0].dwColumn = (uint)_info.EndColumn;

            return S_OK;
        }

        public int GetStatementRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            pBegPosition[0].dwLine = (uint)_info.StartLine;
            pBegPosition[0].dwColumn = (uint)_info.StartColumn;
            
            pEndPosition[0].dwLine = (uint)_info.EndLine;
            pEndPosition[0].dwColumn = (uint)_info.EndColumn;

            return S_OK;
        }

        public int Seek(int nCount, out IDebugDocumentContext2 ppDocContext)
        {
            ppDocContext = null;

            return E_NOTIMPL;
        }
    }
}
