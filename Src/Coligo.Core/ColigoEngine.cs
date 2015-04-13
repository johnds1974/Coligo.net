using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coligo.Core
{
    public static class ColigoEngine
    {
        static bool _initialized;

        /// <summary>
        /// 
        /// </summary>
        public static IIocContainer Container { get; set; }

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
