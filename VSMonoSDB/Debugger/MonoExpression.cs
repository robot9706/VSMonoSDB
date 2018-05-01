using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;

using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging
{
    public class MonoExpression : IDebugExpression2
    {
        private MonoEngine _engine;
        private MonoThread _thread;
        private ObjectValue _value;
        private string _expression;
        private CancellationTokenSource _cancellationToken;

        public MonoExpression(MonoEngine engine, MonoThread thread, ObjectValue value, string expression)
        {
            _engine = engine;
            _thread = thread;
            _value = value;
            _expression = expression;
        }

        public int EvaluateAsync(uint dwFlags, IDebugEventCallback2 pExprCallback)
        {
            _cancellationToken = new CancellationTokenSource();
            Task.Run(() =>
            {
                IDebugProperty2 result;
                EvaluateSync(dwFlags, uint.MaxValue, null, out result);

                _engine.Callback.OnExpressionEvaluated(this, _value, _expression, _thread);
            }, _cancellationToken.Token);

            return S_OK;
        }

        public int EvaluateSync(uint dwFlags, uint dwTimeout, IDebugEventCallback2 pExprCallback, out IDebugProperty2 ppResult)
        {
            ppResult = new MonoProperty(null, _value, _expression);

            return S_OK;
        }

        public int Abort()
        {
            if (_cancellationToken != null)
            {
                _cancellationToken.Cancel();
                _cancellationToken = null;
                return S_OK;
            }
            return S_FALSE;
        }
    }
}
