namespace Bars.Gkh.RegOperator.Caching.Proxy
{
    using System;

    using Bars.B4.Modules.Caching.Interfaces;
    using Bars.Gkh.Config.Impl.Internal.Proxy;

    using Castle.DynamicProxy;

    internal static class CacheEntityProxyGenerator
    {
        private static readonly ProxyGenerator ProxyGenerator;
        private static readonly ProxyGenerationOptions Options;

        static CacheEntityProxyGenerator()
        {
            CacheEntityProxyGenerator.ProxyGenerator = new ProxyGenerator();
            CacheEntityProxyGenerator.Options = new ProxyGenerationOptions(new GetSetPropertyProxyHook());
        }

        public static T Generate<T>(T innerObject, IAppCache cache, bool loaded)
            where T : class
        {
            return (T)CacheEntityProxyGenerator.Generate(typeof(T), innerObject, cache, loaded);
        }

        public static object Generate(Type type, object innerObject, IAppCache cache, bool loaded)
        {
            return CacheEntityProxyGenerator.ProxyGenerator
                .CreateClassProxyWithTarget(
                    type,
                    innerObject,
                    CacheEntityProxyGenerator.Options,
                    UnProxyEntityFromCacheInterceptor.Create(type, cache, loaded));
        }
    }
}