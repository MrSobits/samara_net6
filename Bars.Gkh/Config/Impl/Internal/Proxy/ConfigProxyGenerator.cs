namespace Bars.Gkh.Config.Impl.Internal.Proxy
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.Config.Attributes;

    using Castle.DynamicProxy;

    /// <summary>
    ///     Генератор оберток вокруг экземпляров конфигурационных секций
    /// </summary>
    internal static class ConfigProxyGenerator
    {
        private static readonly object Sync = new object();

        private static readonly ProxyGenerator ProxyGenerator;

        private static readonly ProxyGenerationOptions Options;

        private static readonly IDictionary<string, object> ProxyCache;

        static ConfigProxyGenerator()
        {
            ProxyGenerator = new ProxyGenerator();
            Options = new ProxyGenerationOptions(new GetSetPropertyProxyHook());
            ProxyCache = new ConcurrentDictionary<string, object>();
        }

        public static object Generate(Type type, IDictionary<string, ValueHolder> valueHolders, string key = null)
        {
            key = key ?? type.GetAttribute<GkhConfigSectionAttribute>(true).Alias ?? type.Name;

            object proxy;

            if (!ProxyCache.TryGetValue(key, out proxy))
            {
                lock (Sync)
                {
                    if (!ProxyCache.TryGetValue(key, out proxy))
                    {
                        proxy = ProxyGenerator.CreateClassProxy(type, Options, new ConfigProxyInterceptor(valueHolders, key));
                        ProxyCache.Add(key, proxy);
                    }
                }
            }

            return proxy;
        }

        public static T Generate<T>(Dictionary<string, ValueHolder> valueHolders, string key = null)
            where T : class, IGkhConfigSection
        {
            return (T)Generate(typeof(T), valueHolders, key);
        }
    }
}