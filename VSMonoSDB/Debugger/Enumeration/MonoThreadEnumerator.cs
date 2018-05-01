using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Enumerators
{
    public class MonoThreadEnumerator : BaseEnumerator<IDebugThread2, IEnumDebugThreads2>, IEnumDebugThreads2
    {
        public MonoThreadEnumerator(IDebugThread2[] threads) : base(threads)
        {
        }
    }
}
