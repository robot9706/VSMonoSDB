using Mono.Debugging.Client;
using Mono.Debugging.Soft;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace VSMonoSDB.Debugging
{
    public class SoftDebugger
    {
        readonly object _lock = new object();

        private uint _stepKind;
        private uint _stepUnit;

        private DebuggerSessionOptions _options;

        private MonoEngine _debugEngine;

        private SoftDebuggerSession _session; 
        public SoftDebuggerSession Session
        {
            get { return _session; }
        }

        //Props
        private BreakpointStore _breakEvents;
        public BreakpointStore Breakpoints
        {
            get { return _breakEvents; }
        }

        public ThreadInfo ActiveThread
        {
            get
            {
                lock (_lock)
                    return _session == null ? null : _session.ActiveThread;
            }
        }

        public Backtrace ActiveBacktrace
        {
            get
            {
                var thr = ActiveThread;

                return thr == null ? null : thr.Backtrace;
            }
        }

        public StackFrame ActiveFrame
        {
            get
            {
                var bt = ActiveBacktrace;

                if (bt != null)
                    return bt.GetFrame(0);

                return null;
            }
        }

        public ExceptionInfo ActiveException
        {
            get
            {
                var bt = ActiveBacktrace;

                return bt == null ? null : bt.GetFrame(0).GetException();
            }
        }

        public SoftDebugger(MonoEngine engine)
        {
            _debugEngine = engine;

            _options = new DebuggerSessionOptions()
            {
                EvaluationOptions = EvaluationOptions.DefaultOptions
            };
            _options.EvaluationOptions.UseExternalTypeResolver = true;

            _session = new SoftDebuggerSession();
            _session.Breakpoints = (_breakEvents = new BreakpointStore());

            _session.TypeResolverHandler += event_TypeResolverHandler;
            _session.TargetReady += event_TargetReady;
            _session.TargetExited += event_TargetExited;
            _session.TargetThreadStarted += event_TargetThreadStarted;
            _session.TargetThreadStopped += event_TargetThreadStopped;
            _session.TargetHitBreakpoint += event_TargetHitBreakpoint;
            _session.TargetUnhandledException += event_TargetUnhandledException;
        }

		#region Debug session
		public void Connect(IPAddress targetIP, int targetPort)
        {
            lock (_lock)
            {
                var args = new SoftDebuggerConnectArgs(string.Empty, targetIP, targetPort)
                {
                    MaxConnectionAttempts = 1,
                    TimeBetweenConnectionAttempts = 500
                };
                _session.Run(new SoftDebuggerStartInfo(args), _options);
            }
        }

        public void Dispose()
        {
            if (_session == null)
                return;

            lock (_lock)
            {
                _session.Dispose();
                _session = null;
            }
        }

        public void Kill()
        {
			lock (_lock)
			{
				if (_session == null)
					return;

				if (!_session.IsRunning)
					_session.Continue();

				if (!_session.HasExited)
					_session.Exit();

				_session.Dispose();
				_session = null;
			}
		}

        public void Continue()
        {
            lock (_lock)
            {
                if (_session != null && !_session.IsRunning && !_session.HasExited)
                {
                    _session.Continue();
                }
            }
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (_session != null && _session.IsRunning)
                {
                    _session.Stop();
                }
            }
        }
        #endregion

        #region Stepping
        public void StepOverLine()
        {
            lock (_lock)
            {
                if (_session != null && !_session.IsRunning && !_session.HasExited)
                {
                    _session.NextLine();
                }
            }
        }

        public void StepOverInstruction()
        {
            lock (_lock)
            {
                if (_session != null && !_session.IsRunning && !_session.HasExited)
                {
                    _session.NextInstruction();
                }
            }
        }

        public void StepIntoLine()
        {
            lock (_lock)
            {
                if (_session != null && !_session.IsRunning && !_session.HasExited)
                {
                    _session.StepLine();
                }
            }
        }

        public void StepIntoInstruction()
        {
            lock (_lock)
            {
                if (_session != null && !_session.IsRunning && !_session.HasExited)
                {
                    _session.StepInstruction();
                }
            }
        }

        public void StepOutOfMethod()
        {
            lock (_lock)
            {
                if (_session != null && !_session.IsRunning && !_session.HasExited)
                {
                    _session.Finish();
                }
            }
        }

        public void HandleStepEvent(uint stepKind, uint stepUnit)
        {
            _stepKind = stepKind;
            _stepUnit = stepUnit;

            //Modify the target stopped event so we can handle it
            _session.TargetStopped += event_Stepping_TargetStopped;
        }

        private void event_Stepping_TargetStopped(object sender, TargetEventArgs e)
        {
            //Restore the target stopped event
            _session.TargetStopped -= event_Stepping_TargetStopped;
            
            //Skip code which doesn't have source code
            if (!_debugEngine.CurrentFrameContainsKnownCode(e.Thread))
            {
                if (_debugEngine.ThreadContainsKnownCode(e.Thread))
                {
                    _debugEngine.Step(_debugEngine.ThreadManager[e.Thread], _stepKind, _stepUnit);
                    return;
                }

                Continue();
                return;
            }

            _debugEngine.Callback.OnStepFinished(_debugEngine.ThreadManager[e.Thread]);
        }
        #endregion

        #region Event handlers
        private void event_TargetUnhandledException(object sender, TargetEventArgs e)
        {
            ExceptionInfo exception = e.Backtrace.GetFrame(0).GetException();

            _debugEngine.Callback.OnExceptionThrown(exception, _debugEngine.ThreadManager[e.Thread]);
        }

        private void event_TargetReady(object sender, TargetEventArgs e)
        {
            _debugEngine.ThreadManager.AddThread(e.Thread);
        }

        private string event_TypeResolverHandler(string identifier, SourceLocation location)
        {
            //From the original CLI SDB

            if (identifier == "__EXCEPTION_OBJECT__")
                return null;

            foreach (var loc in ActiveFrame.GetAllLocals())
                if (loc.Name == identifier)
                    return null;

            return identifier;
        }

        private void event_TargetThreadStarted(object sender, TargetEventArgs e)
        {
            MonoThread thread = _debugEngine.ThreadManager.AddThread(e.Thread);

            _debugEngine.Callback.OnThreadCreate(thread);
        }

        private void event_TargetThreadStopped(object sender, TargetEventArgs e)
        {
            MonoThread thread = _debugEngine.ThreadManager.Remove(e.Thread.Id);

            _debugEngine.Callback.OnThreadDestroy(thread);
        }

        private void event_TargetHitBreakpoint(object sender, TargetEventArgs e)
        {
            MonoPendingBreakpoint pendingBreakpoint = _debugEngine.BreakpointManager.FindBreakpoint(e.BreakEvent);

            _debugEngine.Callback.OnBreakpointHit(pendingBreakpoint, _debugEngine.ThreadManager[e.Thread]);
        }

        private void event_TargetExited(object sender, TargetEventArgs e)
        {
            _debugEngine.Callback.OnProgramDestroyed(_debugEngine, (uint?)e.ExitCode ?? 0);
        }
        #endregion
    }
}
