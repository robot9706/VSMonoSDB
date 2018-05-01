using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using System;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventEngineCreate : AsyncEvent, IDebugEngineCreateEvent2
    {
        public static Guid ID = new Guid("FE5B734C-759D-4E59-AB04-F103343BDD06");

        private MonoEngine _engine;

        public DebugEventEngineCreate(MonoEngine engine)
        {
            _engine = engine;
        }

        public int GetEngine(out IDebugEngine2 pEngine)
        {
            pEngine = _engine;

            return VSConstants.S_OK;
        }
    }
}
