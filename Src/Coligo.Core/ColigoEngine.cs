using System.Threading;

namespace Coligo.Core
{
    public static class ColigoEngine
    {
        static bool _initialized;
        private static SynchronizationContext _syncContext;

        /// <summary>
        /// 
        /// </summary>
        public static IIocContainer Container { get; set; }

        /// <summary>
        /// Returns the captured SynchronisationContext which was capetured when Initialize(...) was called.
        /// </summary>
        public static SynchronizationContext SyncContext
        {
            get
            {
                return _syncContext;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize(IIocContainer container)
        {
            if (_initialized)
            {
                //@todo: log this!
                return;
            }

            _syncContext = SynchronizationContext.Current;

            Container = container;

            if (Container == null)
            {
                //@todo: log this!

                //@todo: throw something here, we must have a container...

            }

            // initialize the container...
            Container.Initialize();
        }

    }


}
