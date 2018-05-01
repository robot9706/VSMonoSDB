using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Enumerators
{
    class MonoPropertyEnumerator : BaseEnumerator<DEBUG_PROPERTY_INFO, IEnumDebugPropertyInfo2>, IEnumDebugPropertyInfo2
    {
        public MonoPropertyEnumerator(DEBUG_PROPERTY_INFO[] properties) : base(properties)
        {
        }

        public int Next(uint celt, DEBUG_PROPERTY_INFO[] rgelt, out uint pceltFetched)
        {
            return NextOut(celt, rgelt, out pceltFetched);
        }
    }
}
