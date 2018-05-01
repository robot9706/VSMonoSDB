using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using VSMonoSDB.Debugging.Events;

namespace VSMonoSDB.Debugging
{
    public class MonoEngineCallback
    {
        private MonoEngine _engine;
        private IDebugEventCallback2 _callback;

        public MonoEngineCallback(MonoEngine engine, IDebugEventCallback2 callback)
        {
            _engine = engine;
            _callback = callback;
        }

        public void OnProgramDestroyed(IDebugProgram2 program, uint exitCode)
        {
            _callback.Event(_engine, null, program, null, new DebugEventProgramDestory(exitCode), ref DebugEventProgramDestory.ID, DebugEventProgramDestory.Attributes);
        }

        public void OnEngineCreated()
        {
            _callback.Event(_engine, _engine.Process, _engine, null, new DebugEventEngineCreate(_engine), ref DebugEventEngineCreate.ID, DebugEventEngineCreate.Attributes);
        }

        public void OnProgramCreated()
        {
            _callback.Event(_engine, null, _engine, null, new DebugEventProgramCreate(), ref DebugEventProgramCreate.ID, DebugEventProgramCreate.Attributes);
        }

        public void OnThreadCreate(MonoThread thread)
        {
            _callback.Event(_engine, _engine.Process, _engine, thread, new DebugEventThreadCreate(), ref DebugEventThreadCreate.ID, DebugEventThreadCreate.Attributes);
        }

        public void OnThreadDestroy(MonoThread thread)
        {
            _callback.Event(_engine, _engine.Process, _engine, thread, new DebugEventThreadDestroy(), ref DebugEventThreadDestroy.ID, DebugEventThreadDestroy.Attributes);
        }

        public void OnBreakpointHit(MonoPendingBreakpoint bp, MonoThread thread)
        {
            _callback.Event(_engine, _engine.Process, _engine, thread, new DebugEventBreakpointHit(bp), ref DebugEventBreakpointHit.ID, DebugEventBreakpointHit.Attributes);
        }

        public void OnBreakpointBound(MonoPendingBreakpoint pending)
        {
            _callback.Event(_engine, _engine.Process, _engine, null, new DebugEventBreakpointBound(pending), ref DebugEventBreakpointBound.ID, DebugEventBreakpointBound.Attributes);
        }

        public void OnStepFinished(IDebugThread2 thread)
        {
            _callback.Event(_engine, _engine.Process, _engine, thread, new DebugEventStepComplete(), ref DebugEventStepComplete.ID, DebugEventStepComplete.Attributes);
        }
        
        public void OnExpressionEvaluated(MonoExpression expression, ObjectValue expValue, string expString, MonoThread thread)
        {
            _callback.Event(_engine, _engine.Process, _engine, thread, new DebugEventExpressionComplete(expression, expValue, expString), ref DebugEventExpressionComplete.ID, DebugEventExpressionComplete.Attributes);
        }

        public void OnExceptionThrown(ExceptionInfo info, MonoThread thread)
        {
            _callback.Event(_engine, _engine.Process, _engine, thread, new DebugEventException(_engine, info), ref DebugEventException.ID, DebugEventException.Attributes);
        }
    }
}
