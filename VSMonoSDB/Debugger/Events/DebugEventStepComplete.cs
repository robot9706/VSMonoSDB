using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventStepComplete : StopEvent, IDebugStepCompleteEvent2
    {
        public static Guid ID = new Guid("0f7f24c1-74d9-4ea6-a3ea-7edb2d81441d");
    }
}
