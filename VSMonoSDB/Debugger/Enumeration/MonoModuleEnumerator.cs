using Microsoft.VisualStudio.Debugger.Interop;

namespace VSMonoSDB.Debugging.Enumerators
{
    public class MonoModuleEnumerator : BaseEnumerator<IDebugModule2, IEnumDebugModules2>, IEnumDebugModules2
    {
        public MonoModuleEnumerator(IDebugModule2[] modules) : base(modules)
        {
        }
    }
}
