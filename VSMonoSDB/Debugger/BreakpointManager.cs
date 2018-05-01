using Mono.Debugging.Client;
using System.Collections.Generic;

namespace VSMonoSDB.Debugging
{
    public class BreakpointManager
    {
        private Dictionary<BreakEvent, MonoPendingBreakpoint> _breakpoints;

        private MonoEngine _engine;

        public BreakpointManager(MonoEngine engine)
        {
            _breakpoints = new Dictionary<BreakEvent, MonoPendingBreakpoint>();

            _engine = engine;
        }

        public MonoPendingBreakpoint FindBreakpoint(BreakEvent bp)
        {
            if (_breakpoints.ContainsKey(bp))
                return _breakpoints[bp];

            return null;
        }

        public Breakpoint CreateBreakpoint(TextPositionInfo position, MonoPendingBreakpoint bp)
        {
            Breakpoint point = _engine.SoftDebugger.Breakpoints.Add(position.FilePath, position.StartLine, position.StartColumn);

            _breakpoints.Add(point, bp);

            return point;
        }

        public void RemoveBreakpoint(BreakEvent e)
        {
            _engine.SoftDebugger.Breakpoints.Remove(e);

            _breakpoints.Remove(e);
        }
    }
}
