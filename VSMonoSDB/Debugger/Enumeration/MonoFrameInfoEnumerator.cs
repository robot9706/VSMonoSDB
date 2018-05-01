using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Enumerators
{
    public class MonoFrameInfoEnumerator : BaseEnumerator<FRAMEINFO, IEnumDebugFrameInfo2>, IEnumDebugFrameInfo2
    {
        public MonoFrameInfoEnumerator(FRAMEINFO[] frames) : base(frames)
        {
        }
    }
}
