using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Events
{
    public class SyncEvent : IDebugEvent2
    {
        public const uint Attributes = (uint)enum_EVENTATTRIBUTES.EVENT_SYNCHRONOUS;

        public int GetAttributes(out uint pdwAttrib)
        {
            pdwAttrib = Attributes;

            return VSConstants.S_OK;
        }
    }
}
