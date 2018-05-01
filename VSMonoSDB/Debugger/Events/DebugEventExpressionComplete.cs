using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using System;
using static Microsoft.VisualStudio.VSConstants;

namespace VSMonoSDB.Debugging.Events
{
    public class DebugEventExpressionComplete : AsyncEvent, IDebugExpressionEvaluationCompleteEvent2
    {
        public static Guid ID = new Guid("C0E13A85-238A-4800-8315-D947C960A843");

        private MonoExpression _evaluatedExpression;
        private ObjectValue _value;
        private string _expressionString;

        public DebugEventExpressionComplete(MonoExpression evalExp, ObjectValue value, string expression)
        {
            _evaluatedExpression = evalExp;
            _value = value;
            _expressionString = expression;
        }

        public int GetExpression(out IDebugExpression2 ppExpr)
        {
            ppExpr = _evaluatedExpression;
            return S_OK;
        }

        public int GetResult(out IDebugProperty2 ppResult)
        {
            ppResult = new MonoProperty(null, _value, _expressionString);

            return S_OK;
        }
    }
}
