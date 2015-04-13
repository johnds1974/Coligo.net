using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coligo.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocContainer
    {
        /// <summary>
        /// 
        /// </summary>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        Type GetType(string typename);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        object GetInstance(string typename);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetInstance(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="atype"></param>
        /// <returns></returns>
        T GetInstance<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subType"></param>
        void AsNew<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="BT"></typeparam>
        /// <typeparam name="CT"></typeparam>
        void AsNew<BT, CT>()
            where BT : class
            where CT : BT;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subType"></param>
        void AsSingle<T>() where T : class;

        /// <summary>
        /// Registers a new type (ST) as the type to create when any BT is encountered.
        /// </summary>
        /// <typeparam name="BT"></typeparam>
        /// <typeparam name="ST"></typeparam>
        void AsSingle<BT, ST>()
            where BT : class
            where ST : BT;
    }

}
