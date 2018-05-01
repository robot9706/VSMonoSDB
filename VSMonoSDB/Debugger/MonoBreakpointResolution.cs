using Microsoft.VisualStudio.Debugger.Interop;
using System.Runtime.InteropServices;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoBreakpointResolution : IDebugBreakpointResolution2
    {
        private uint _address;
        private MonoDocumentContext _document;

        public MonoBreakpointResolution(uint address, MonoDocumentContext doc)
        {
            _address = address;
            _document = doc;
        }

        public int GetBreakpointType(out uint pBPType)
        {
            pBPType = (uint)enum_BP_TYPE.BPT_CODE;

            return S_OK;
        }

        public int GetResolutionInfo(uint dwFields, BP_RESOLUTION_INFO[] pBPResolutionInfo)
        {
            if ((dwFields & (uint)enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION) != 0)
            {
                BP_RESOLUTION_LOCATION location = new BP_RESOLUTION_LOCATION { bpType = (uint)enum_BP_TYPE.BPT_CODE };

                MonoMemoryContext codeContext = new MonoMemoryContext(_document, _address);
                location.unionmember1 = Marshal.GetComInterfaceForObject(codeContext, typeof(IDebugCodeContext2));
                pBPResolutionInfo[0].bpResLocation = location;
                pBPResolutionInfo[0].dwFields |= (uint)enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION;
            }

            return S_OK;
        }
    }
}
