

namespace Bars.Gkh.RegOperator.Caching.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.Interfaces;
    using Bars.B4.Utils;

    using Castle.DynamicProxy;

    public static class UnProxyEntityFromCacheInterceptor
    {
        public static IInterceptor Create(Type type, IAppCache cache, bool loaded)
        {
            return (IInterceptor)Activator.CreateInstance(typeof(UnProxyEntityFromCacheInterceptor<>).MakeGenericType(type), cache, loaded);
        }
    }

    /// <summary>
    /// Интерцептор перехватывающий доступ к get-терам свойств
    /// </summary>
    public class UnProxyEntityFromCacheInterceptor<T> : IInterceptor
        where T : class, new()
    {
        private readonly IAppCache cache;
        private bool loaded;
        private readonly IDictionary<string, object> proxyHolder;
        private readonly IDictionary<string, PropertyInfo> proxiableProperties;

        public UnProxyEntityFromCacheInterceptor(IAppCache cache, bool loaded)
        {
            this.cache = cache;
            this.loaded = loaded;
            this.proxyHolder = new Dictionary<string, object>();
            this.proxiableProperties = typeof(T).GetProperties()
                .Where(x => x.PropertyType.Is<IEntity>())
                .Where(x => this.cache.IsCached(x.PropertyType))
                .ToDictionary(x => x.Name);
        }

        

        /// <inheritdoc />
        public void Intercept(IInvocation invocation)
        {
            var propertyName = this.GetPropertyName(invocation);
            if (propertyName != "Id")
            {
                this.EnsureLoaded(invocation);
            }

            if (invocation.Method.Name.StartsWith("get_", StringComparison.Ordinal))
            {
                this.ProcessGetMethod(invocation);
            }
            else
            {
                this.ProcessSetMethod(invocation);
            }
        }

        private void ProcessGetMethod(IInvocation invocation)
        {
            var propertyName = this.GetPropertyName(invocation);

            if (this.proxiableProperties.ContainsKey(propertyName))
            {
                invocation.Proceed();
                invocation.ReturnValue = this.GetPropertyProxy(propertyName, invocation.ReturnValue);
            }
            else
            {
               invocation.Proceed();
            }
        }

        private string GetPropertyName(IInvocation invocation)
        {
            return invocation.Method.Name.Substring(4);
        }

        private void ProcessSetMethod(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4);
            if (this.proxiableProperties.ContainsKey(propertyName))
            {
                this.proxyHolder[propertyName] = invocation.Arguments[0];
            }

            invocation.Proceed();
        }

        private object GetPropertyProxy(string propertyName, object propertyObject)
        {
            var propertyProxy = this.proxyHolder.Get(propertyName);
            if (propertyProxy.IsNull())
            {
                propertyProxy = CacheEntityProxyGenerator.Generate(propertyObject, this.cache, false);

                this.proxyHolder[propertyName] = propertyProxy;
            }

            return propertyProxy;
        }

        private void EnsureLoaded(IInvocation invocation)
        {
            if (this.loaded)
            {
                return;
            }

            var entityId = ((IEntity)invocation.InvocationTarget).Id;
            var proxyTarget = (IChangeProxyTarget)invocation.Proxy;

            var entityFromCache = this.cache.GetClient<T>().Get(entityId);
            proxyTarget.ChangeInvocationTarget(entityFromCache);

            this.loaded = true;
        }
    }
}