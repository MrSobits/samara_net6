namespace Bars.Gkh.RegOperator.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4.Modules.Caching.Interfaces;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Caching.Proxy;

    using Castle.DynamicProxy;

    public class LazyLoadCacheClientDecorator<T> : ICacheClient<T> where T : class, new()
    {
        private readonly ICacheClient<T> innerClient;
        private readonly IAppCache cache;

        public LazyLoadCacheClientDecorator(IAppCache cache)
        {
            this.cache = cache;
            this.innerClient = this.cache.GetClient<T>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.innerClient.Dispose();
        }

        /// <inheritdoc />
        public T Get(object key)
        {
            var value = this.innerClient.Get(key);

            return value.IsNull() ? value : this.GetProxy(value);
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            this.innerClient.Add(this.UnProxy(item));
        }

        /// <inheritdoc />
        public void Add(T item, TimeSpan expiresIn)
        {
            this.innerClient.Add(this.UnProxy(item), expiresIn);
        }

        /// <inheritdoc />
        public Task AddAsync(T item)
        {
            return this.innerClient.AddAsync(this.UnProxy(item));
        }

        /// <inheritdoc />
        public Task AddAsync(T item, TimeSpan expiresIn)
        {
            return this.innerClient.AddAsync(this.UnProxy(item), expiresIn);
        }

        /// <inheritdoc />
        public void AddMany(IEnumerable<T> items, bool expandKeys = false)
        {
            this.innerClient.AddMany(items.Select(this.UnProxy), expandKeys);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll()
        {
            return this.innerClient.GetAll().Select(this.GetProxy);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll(IEnumerable<object> keys)
        {
            return this.innerClient.GetAll(keys).Select(this.GetProxy);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetAllKeys()
        {
            return this.innerClient.GetAllKeys();
        }

        /// <inheritdoc />
        public void Remove(T item)
        {
            this.innerClient.Remove(this.UnProxy(item));
        }

        /// <inheritdoc />
        public void RemoveAll(IEnumerable<T> items)
        {
            this.innerClient.RemoveAll(items.Select(this.UnProxy));
        }

        /// <inheritdoc />
        public void RemoveAll()
        {
            this.innerClient.RemoveAll();
        }

        private T GetProxy(T value)
        {
            return CacheEntityProxyGenerator.Generate(value, this.cache, true);
        }

        private T UnProxy(T value)
        {
            var proxyItem = value as IProxyTargetAccessor;
            if (value != null)
            {
                return (T)proxyItem.DynProxyGetTarget();
            }

            return value;
        }
    }
}