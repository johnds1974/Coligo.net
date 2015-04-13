using Coligo.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coligo.Platform.Container
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultContainer : IIocContainer
    {
        /// <summary>
        /// 
        /// </summary>
        private enum InstanceType
        {
            AsSingle,
            AsNew
        };

        /// <summary>
        /// 
        /// </summary>
        private struct TypeInfoMap
        {
            public Type SourceType { get; set; }
            public Type TargetType { get; set; }
            public InstanceType InstanceType { get; set; }
        }

        IList<TypeInfoMap> _registeredTypes;

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            _registeredTypes = new List<TypeInfoMap>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public Type GetType(string typename)
        {
            if (!string.IsNullOrEmpty(typename))
            {
                TypeInfoMap? tim = _registeredTypes.FirstOrDefault(ti => ti.TargetType.Name.EndsWith(typename));

                Debug.WriteLine(" ===> DefaultContainer.GetType('{0}') {1}", typename, tim.HasValue ? "FOUND IT!" : "NOT REGISTERED!");

                return tim.HasValue ? tim.Value.TargetType : null;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subType"></param>
        public void AsNew<T>() where T : class
        {
            _registeredTypes.Add(new TypeInfoMap
            {
                SourceType = typeof(T),
                TargetType = typeof(T),
                InstanceType = InstanceType.AsNew
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="BT"></typeparam>
        /// <typeparam name="CT"></typeparam>
        public void AsNew<BT, CT>()
            where BT : class
            where CT : BT
        {
            _registeredTypes.Add(new TypeInfoMap
            {
                SourceType = typeof(BT),
                TargetType = typeof(CT),
                InstanceType = InstanceType.AsNew
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AsSingle<T>() where T : class
        {
            _registeredTypes.Add(new TypeInfoMap
            {
                SourceType = typeof(T),
                TargetType = typeof(T),
                InstanceType = InstanceType.AsSingle
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="BT"></typeparam>
        /// <typeparam name="ST"></typeparam>
        public void AsSingle<BT, ST>()
            where BT : class
            where ST : BT
        {
            _registeredTypes.Add(new TypeInfoMap
            {
                SourceType = typeof(BT),
                TargetType = typeof(ST),
                InstanceType = InstanceType.AsSingle
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atype"></param>
        /// <returns></returns>
        public object GetInstance(string typename)
        {
            var type = GetType(typename);

            return GetInstance(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetInstance(Type type)
        {
            object instance = null;

            if (type != null && IsRegistered(type))
            {
                Debug.WriteLine(" ===> DefaultContainer.GetInstance({0})...", type.Name);

                // Get the Target Type of the registered type...
                var targetType = _registeredTypes.First(rt => rt.SourceType.Equals(type)).TargetType;

                object[] paramInstances = null;
#if WINDOWS_PHONE_APP
                TypeInfo ti = type.GetTypeInfo();

                var ctors = ti.DeclaredConstructors.ToList();

                foreach (ConstructorInfo ci in ctors)
                {
                    var ps = ci.GetParameters().OrderBy(pi => pi.Position).ToList();

                    // Check if ALL of the parameters of this constructor are types that we can actually create (ie, they have been registered)...
                    if (ps.All(pi => IsRegistered(pi.ParameterType)))
                    {
                        // Ok, now create an array of type instances, to pass into the activator...
                        paramInstances = ps.Select(pi => GetInstance(pi.ParameterType)).ToArray();

//                        instance = ci.Invoke(paramInstances);
                        break;
                    }

                    Debug.WriteLine(" ===> Constructor ({0})", string.Join(",", ci.GetParameters().Select(c => c.ToString())));
                }
#else
                var ctors = type.GetConstructors();
#endif
                try
                {
                    if (paramInstances == null)
                    {
                        // So now we have a 'Type' instance, we can create an instance of it...
                        instance = Activator.CreateInstance(targetType);
                    }
                    else
                    {
                        // So now we have a 'Type' instance, we can create an instance of it...
                        instance = Activator.CreateInstance(targetType, paramInstances);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(" ===> DefaultContainer.GetInstance({0}) ERROR: {1}", type.Name, ex.Message);
                }
            }

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="atype"></param>
        /// <returns></returns>
        public T GetInstance<T>() where T : class
        {
            // So now we have a 'Type' instance, we can create an instance of it...
            var instance = GetInstance(typeof(T));

            return (T)instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsRegistered(Type type)
        {
            if (type != null)
            {
                return _registeredTypes.Any(rt => rt.SourceType.Equals(type));
            }

            return false;
        }
    }
}
