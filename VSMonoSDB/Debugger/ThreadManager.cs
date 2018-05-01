using Mono.Debugging.Client;
using System.Collections.Generic;
using System.Linq;
using VSMonoSDB.Debugging.Enumerators;

namespace VSMonoSDB.Debugging
{
    public class ThreadManager
    {
        private Dictionary<long, MonoThread> _threads;

        private MonoEngine _engine;

        public MonoThread this[ThreadInfo info]
        {
            get
            {
                if (_threads.ContainsKey(info.Id))
                    return _threads[info.Id];

                return null;
            }
        }

        public ThreadManager(MonoEngine engine)
        {
            _threads = new Dictionary<long, MonoThread>();

            _engine = engine;
        }

        public MonoThread AddThread(ThreadInfo info, string name = null)
        {
            if (_threads.ContainsKey(info.Id))
                return _threads[info.Id];

            MonoThread thread = new Debugging.MonoThread(_engine, (string.IsNullOrEmpty(name) ? info.Name : name), info);

            _threads.Add(info.Id, thread);

            return thread;
        }

        public MonoThread Remove(long id)
        {
            if (!_threads.ContainsKey(id))
                return null;

            MonoThread thread = _threads[id];

            _threads.Remove(id);

            return thread;
        }

        public MonoThreadEnumerator GetInteropEnumerator()
        {
            return new MonoThreadEnumerator(_threads.Values.ToArray());
        }
    }
}
