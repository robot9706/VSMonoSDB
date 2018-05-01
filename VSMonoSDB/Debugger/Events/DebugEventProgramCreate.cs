using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventProgramCreate : AsyncEvent, IDebugProgramCreateEvent2
    {
        public static Guid ID = new Guid("96CD11EE-ECD4-4E89-957E-B5D496FC4139");
    }
}
