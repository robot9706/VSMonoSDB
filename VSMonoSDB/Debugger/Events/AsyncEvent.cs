using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Events
{
    public class AsyncEvent : IDebugEvent2
    {
        public const uint Attributes = (uint)enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS;

        public int GetAttributes(out uint eventAttributes)
        {
            eventAttributes = Attributes;

            return VSConstants.S_OK;
        }
    }
}
