using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using VSMonoSDB.Debugging.Enumerators;
using VSMonoSDB.Debugging.Events;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    [ComVisible(true)]
    [Guid(DebuggerGuids.EngineIdGuidString)]
    public class MonoEngine : IDebugEngine2, IDebugProgram3, IDebugEngineLaunch2
    {
        private MonoEngineCallback _callback;
        public MonoEngineCallback Callback
        {
            get { return _callback; }
        }

        private MonoProcess _process;
        public MonoProcess Process
        {
            get { return _process; }
        }

        private SoftDebugger _softDebugger;
        public SoftDebugger SoftDebugger
        {
            get { return _softDebugger; }
        }

        private ThreadManager _threadManager;
        public ThreadManager ThreadManager
        {
            get { return _threadManager; }
        }

        private BreakpointManager _breakpointManager;
        public BreakpointManager BreakpointManager
        {
            get { return _breakpointManager; }
        }

        private MonoModule _mainModule;

        private Guid _programGuid;

        public MonoEngine()
        {
            _threadManager = new ThreadManager(this);
            _breakpointManager = new BreakpointManager(this);
        }

        #region IDebugEngine2
        public int Attach(IDebugProgram2[] rgpPrograms, IDebugProgramNode2[] rgpProgramNodes, uint celtPrograms, IDebugEventCallback2 pCallback, uint dwReason)
        {
			DebugInfo attachInfo = MainPackage.DebugTarget;

            rgpPrograms[0].GetProgramId(out _programGuid);

            _softDebugger = new SoftDebugger(this);
            _softDebugger.Connect(attachInfo.Target.Address, attachInfo.Target.Port);

            _mainModule = new MonoModule(this);

            _callback.OnEngineCreated();
            _callback.OnProgramCreated();

            return S_OK;
        }

        public int CauseBreak()
        {
            _softDebugger.Pause();

            return S_OK;
        }

        public int ContinueFromSynchronousEvent(IDebugEvent2 pEvent)
        {
            if (pEvent is DebugEventProgramDestory)
            {
                _softDebugger.Dispose();
            }

            return S_OK;
        }

        public int CreatePendingBreakpoint(IDebugBreakpointRequest2 pBPRequest, out IDebugPendingBreakpoint2 ppPendingBP)
        {
            ppPendingBP = new MonoPendingBreakpoint(this, pBPRequest);

            return S_OK;
        }

        public int DestroyProgram(IDebugProgram2 pProgram)
        {
            return E_NOTIMPL;
        }

        public int GetEngineId(out Guid pguidEngine)
        {
            pguidEngine = DebuggerGuids.EngineIdGuid;

            return S_OK;
        }

        public int RemoveAllSetExceptions(ref Guid guidType)
        {
            return S_OK;
        }

        public int RemoveSetException(EXCEPTION_INFO[] pException)
        {
            return S_OK;
        }

        public int SetException(EXCEPTION_INFO[] pException)
        {
            return S_OK;
        }

        public int SetLocale(ushort wLangID)
        {
            return S_OK;
        }

        public int SetMetric(string pszMetric, object varValue)
        {
            return S_OK;
        }

        public int SetRegistryRoot(string pszRegistryRoot)
        {
            return S_OK;
        }
        #endregion

        #region IDebugProgram3
        public int CanDetach()
        {
            return S_OK;
        }

        public int Detach()
        {
            _softDebugger.Kill();
            _softDebugger.Dispose();

            return S_OK;
        }

        public int Continue(IDebugThread2 pThread)
        {
            _softDebugger.Continue();

            return S_OK;
        }

        public int ExecuteOnThread(IDebugThread2 pThread)
        {
            MonoThread thread = (MonoThread)pThread;

            if (_softDebugger.ActiveThread != null && _softDebugger.ActiveThread.Id != thread.ID)
            {
                thread.SetActive();
            }

            _softDebugger.Continue();

            return S_OK;
        }

        public int EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            TextPositionInfo posInfo = new TextPositionInfo(pDocPos);

            MonoDocumentContext docCtx = new MonoDocumentContext(posInfo, null);
            MonoMemoryContext memoryCtx = new MonoMemoryContext(docCtx, 0);

            ppEnum = new MonoCodeContextEnumerator(new IDebugCodeContext2[] { memoryCtx });

            return S_OK;
        }

        public int EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            ppEnum = null;
            ppSafety = null;

            return E_NOTIMPL;
        }

        public int EnumModules(out IEnumDebugModules2 ppEnum)
        {
            ppEnum = new MonoModuleEnumerator(new IDebugModule2[] { _mainModule });

            return S_OK;
        }

        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            ppEnum = _threadManager.GetInteropEnumerator();

            return S_OK;
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            ppProperty = null;

            return E_NOTIMPL;
        }

        public int GetDisassemblyStream(uint dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
        {
            ppDisassemblyStream = null;

            return E_NOTIMPL;
        }

        public int GetENCUpdate(out object ppUpdate)
        {
            ppUpdate = null;

            return E_NOTIMPL;
        }

        public int GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            pbstrEngine = DebuggerGuids.EngineName;
            pguidEngine = DebuggerGuids.EngineIdGuid;

            return S_OK;
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            ppMemoryBytes = null;

            return E_NOTIMPL;
        }

        public int GetName(out string pbstrName)
        {
            pbstrName = null;

            return E_NOTIMPL;
        }

        public int GetProgramId(out Guid pguidProgramId)
        {
            pguidProgramId = _programGuid;

            return S_OK;
        }

        public int Step(IDebugThread2 pThread, uint sk, uint step)
        {
            enum_STEPKIND stepKind = (enum_STEPKIND)sk;
            enum_STEPUNIT stepUnit = (enum_STEPUNIT)step;

            if (stepKind == enum_STEPKIND.STEP_BACKWARDS)
                return S_FALSE;

            //Handle the stepping event
            _softDebugger.HandleStepEvent(sk, step);

            //Do step
            switch (stepKind)
            {
                case enum_STEPKIND.STEP_INTO:
                    if (stepUnit == enum_STEPUNIT.STEP_INSTRUCTION)
                    {
                        _softDebugger.StepIntoInstruction();
                    }
                    else
                    {
                        _softDebugger.StepIntoLine();
                    }
                    break;
                case enum_STEPKIND.STEP_OUT:
                    _softDebugger.StepOutOfMethod();
                    break;
                case enum_STEPKIND.STEP_OVER:
                    if (stepUnit == enum_STEPUNIT.STEP_INSTRUCTION)
                    {
                        _softDebugger.StepOverInstruction();
                    }
                    else
                    {
                        _softDebugger.StepOverLine();
                    }
                    break;
            }

            return S_OK;
        }

        public int Terminate()
        {
            _softDebugger.Kill();
            _softDebugger.Dispose();

            return S_OK;
        }

        public int WriteDump(uint DUMPTYPE, string pszDumpUrl)
        {
            return E_NOTIMPL;
        }
        #endregion

        #region IDebugEngineLaunch2
        public int CanTerminateProcess(IDebugProcess2 pProcess)
        {
            return S_OK;
        }

        public int LaunchSuspended(string pszServer, IDebugPort2 pPort, string pszExe, string pszArgs, string pszDir, string bstrEnv, string pszOptions, uint dwLaunchFlags, uint hStdInput, uint hStdOutput, uint hStdError, IDebugEventCallback2 pCallback, out IDebugProcess2 ppProcess)
        {
            _callback = new MonoEngineCallback(this, pCallback);

            ppProcess = _process = new MonoProcess(Guid.NewGuid(), pPort);
            
            return S_OK;
        }

        public int ResumeProcess(IDebugProcess2 pProcess)
        {
            IDebugPort2 port;
            Guid id;

            pProcess.GetPort(out port);
            pProcess.GetProcessId(out id);

            IDebugDefaultPort2 defaultPort = (IDebugDefaultPort2)port;

            IDebugPortNotify2 notify;
            defaultPort.GetPortNotify(out notify);

            return notify.AddProgramNode(new MonoProgramNode(id));
        }

        public int TerminateProcess(IDebugProcess2 pProcess)
        {
            _softDebugger.Kill();
            _softDebugger.Dispose();

            return S_OK;
        }
        #endregion

        #region Debugger code
        public bool CurrentFrameContainsKnownCode(ThreadInfo thread)
        {
            return (thread.Backtrace.FrameCount > 0 && File.Exists(thread.Backtrace.GetFrame(0).SourceLocation.FileName));
        }

        public bool ThreadContainsKnownCode(ThreadInfo thread)
        {
            if (thread.Backtrace.FrameCount == 0)
            {
                return false;
            }

            if (!CurrentFrameContainsKnownCode(thread))
            {
                for (int x = 1; x < thread.Backtrace.FrameCount; x++)
                {
                    if (File.Exists(thread.Backtrace.GetFrame(x).SourceLocation.FileName))
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }
        #endregion

        #region Deprecated
        public int EnumPrograms(out IEnumDebugPrograms2 ppEnum)
        {
            ppEnum = null;

            return E_NOTIMPL;
        }

        public int Attach(IDebugEventCallback2 pCallback)
        {
            return E_NOTIMPL;
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = null;

            return E_NOTIMPL;
        }

        public int Execute()
        {
            return E_NOTIMPL;
        }
        #endregion
    }
}
