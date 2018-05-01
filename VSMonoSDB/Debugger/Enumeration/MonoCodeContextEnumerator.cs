using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Enumerators
{
    public class MonoCodeContextEnumerator : BaseEnumerator<IDebugCodeContext2, IEnumDebugCodeContexts2>, IEnumDebugCodeContexts2
    {
        public MonoCodeContextEnumerator(IDebugCodeContext2[] contexts) : base(contexts)
        {
        }
    }
}
