namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Services.ServiceContracts;

    /// <summary>
    /// Сервис для работы с рефлексией
    /// </summary>
    public class ReflectionHelperService : IReflectionHelperService
    {
        /// <summary>
        /// Кэш методов
        /// </summary>
        private readonly Dictionary<string, MethodInfo> _methodCache = new Dictionary<string, MethodInfo>();
        
        /// <inheritdoc />
        public MethodInfo GetGenericMethod<T>(string name, [NotNull] params Type[] resultTypes)
        {
            MethodInfo method;
            
            if (_methodCache.ContainsKey(name))
            {
                method = this._methodCache[name];
            }
            else
            {
                method = typeof(T).GetMethod(name);
                _methodCache.Add(name, method);
            }
            
            return method.MakeGenericMethod(resultTypes);;
        }

        /// <inheritdoc />
        public MethodInfo GetGenericMethod(Type methodType, string name, BindingFlags bindingFlags, [NotNull] params Type[] resultTypes)
        {
            MethodInfo method;
            
            if (_methodCache.ContainsKey(name))
            {
                method = this._methodCache[name];
            }
            else
            {
                method = methodType.GetMethods(bindingFlags)
                    .First(x => x.Name == name);
                _methodCache.Add(name, method);
            }
            
            return method.MakeGenericMethod(resultTypes);
        }
    }
}