using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventThreadCreate : AsyncEvent, IDebugThreadCreateEvent2
    {
        public static Guid ID = new Guid("2090CCFC-70C5-491D-A5E8-BAD2DD9EE3EA");
    }
}
