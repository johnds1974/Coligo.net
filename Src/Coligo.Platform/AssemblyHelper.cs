using System;
using System.Linq;
using System.Reflection;

namespace Coligo.Platform
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static Type FindType(string typename)
        {
            var assembly = FindAssembyForType(typename);
            if (assembly != null)
            {
#if WINDOWS_PHONE_APP
#else
                var type = assembly.GetTypes().First(t => t.FullName.EndsWith(typename));

                return type;
#endif
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static Assembly FindAssembyForType(string typename)
        {
            Assembly assem = null;

#if WINDOWS_PHONE_APP

            var type = Type.GetType(typename, false);
            if (type != null)
            {
                assem = type.GetTypeInfo().Assembly;
            }
#else
            assem = AppDomain.CurrentDomain.GetAssemblies()
                 .FirstOrDefault(
                     assembly => assembly.GetTypes().Any(type1 => type1.FullName.EndsWith(typename)));
#endif

            return assem;
        }

    }
}