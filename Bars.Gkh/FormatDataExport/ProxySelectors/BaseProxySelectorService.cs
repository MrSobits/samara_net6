namespace Bars.Gkh.FormatDataExport.ProxySelectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Базовый сервис получения прокси-объекта
    /// </summary>
    /// <typeparam name="T">Прокси-сущность</typeparam>
    public abstract class BaseProxySelectorService<T> : IProxySelectorService<T>
        where T : IHaveId
    {
        protected readonly ICollection<T> emptyCollection = new List<T>();

        private IDictionary<long, T> proxyListCache = null;
        private ICollection<T> additionalProxyListCache = null;

        protected long[] AdditionalIds = new long[0];

        private readonly object cacheLocker = new object();
        private readonly object additionalCacheLocker = new object();

        /// <inheritdoc />
        public DynamicDictionary SelectParams { get; } = new DynamicDictionary();

        /// <inheritdoc />
        public IDictionary<long, T> ProxyListCache
        {
            get
            {
                lock (this.cacheLocker)
                {
                    return this.proxyListCache ?? this.GetProxyListInternal();
                }
            }
        }

        /// <inheritdoc />
        public ICollection<T> ExtProxyListCache => this.ProxyListCache.Values
            .Union(this.GetAdditionalCacheInternal(), this.Comparer).ToList();

        /// <inheritdoc />
        public ICollection<long> ProxyListIdCache => this.ProxyListCache.Keys;

        /// <inheritdoc />
        public IDictionary<long, T> GetProxyList()
        {
            return this.GetCache();
        }

        private IDictionary<long, T> GetProxyListInternal()
        {
            this.LogManager.LogDebug($"Инициализация кэша для {typeof(T).Name}");
            this.InitCache();

            return this.proxyListCache;
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (this.cacheLocker)
            {
                this.proxyListCache?.Clear();
                this.proxyListCache = null;
                this.additionalProxyListCache?.Clear();
                this.additionalProxyListCache = null;
            }
        }

        protected virtual bool CanGetFullData()
        {
            var entityCode = typeof(T).Name.Replace("Proxy", string.Empty).ToUpper();
            return this.ProxySelectorFactory.SelectedEntityCodes.Contains(entityCode);
        }

        private void InitCache()
        {
            var cache = this.CanGetFullData() ? this.GetCache() : new Dictionary<long, T>();
            this.AddAdditionalIds(cache.Values);
            Interlocked.Exchange(ref this.proxyListCache, cache);
        }

        private void AddAdditionalIds(ICollection<T> data)
        {
            var type = typeof(T);
            type.GetProperties()
                .Where(x => x.CustomAttributes.Any(z => z.AttributeType == typeof(ProxyIdAttribute)))
                .ForEach(x =>
                {
                    var attribute = (ProxyIdAttribute)Attribute.GetCustomAttribute(x, typeof(ProxyIdAttribute));
                    var proxyTypeIds = this.ProxySelectorFactory.AdditionalProxyIds
                        .GetOrAdd(attribute.ProxyType, new HashSet<long>());

                    this.LogManager.LogDebug($"Инициализация дополнительных данных для {attribute.ProxyType.Name} инициатор {type.Name}.{x.Name}");

                    foreach (var proxy in data)
                    {
                        var id = x.GetValue(proxy) as long?;
                        if (id.HasValue)
                        {
                            proxyTypeIds.Add(id.Value);
                        }
                    }
                });
        }

        /// <summary>
        /// Получить кэш объекта
        /// </summary>
        protected abstract IDictionary<long, T> GetCache();

        /// <summary>
        /// Получить дополнительные прокси-сущности
        /// </summary>
        protected virtual ICollection<T> GetAdditionalCache()
        {
            return this.emptyCollection;
        }

        private ICollection<T> GetAdditionalCacheInternal()
        {
            var additionalIdArray = this.ProxySelectorFactory
                .AdditionalProxyIds
                .GetOrAdd(typeof(T), new HashSet<long>())
                .ToArray();

            if (additionalIdArray.Any())
            {
                lock (this.additionalCacheLocker)
                {
                    if (this.additionalProxyListCache?.Count != additionalIdArray.Length)
                    {
                        const int take = 20000;
                        if (additionalIdArray.Length < take)
                        {
                            this.AdditionalIds = additionalIdArray;
                            var cache = this.GetAdditionalCache();
                            this.AddAdditionalIds(cache);
                            Interlocked.Exchange(ref this.additionalProxyListCache, cache);
                        }
                        else
                        {
                            var result = new List<T>(additionalIdArray.Length);
                            for (int skip = 0; skip < additionalIdArray.Length; skip += take)
                            {
                                this.AdditionalIds = additionalIdArray.Skip(skip).Take(take).ToArray();
                                var additionalData = this.GetAdditionalCache();
                                this.AddAdditionalIds(additionalData);
                                result.AddRange(additionalData);
                            }

                            Interlocked.Exchange(ref this.additionalProxyListCache, result);
                        }
                    }
                }
            }

            return this.additionalProxyListCache ?? this.emptyCollection;
        }

        /// <summary>
        /// Компаратор сущностей по идентификатору
        /// </summary>
        protected readonly IEqualityComparer<T> Comparer = EntityEqComparer.ById<T>();

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public ILogger LogManager { get; set; }

        /// <summary>
        /// Конвертер данных приведения к формату экспорта
        /// </summary>
        public IExportFormatConverter Converter { get; set; }

        /// <inheritdoc />
        public IProxySelectorFactory ProxySelectorFactory => this.InitService<IProxySelectorFactory>("ProxySelectorFactory");

        /// <inheritdoc />
        public IFormatDataExportFilterService FilterService => this.InitService<IFormatDataExportFilterService>("FormatDataExportFilterService");

        /// <inheritdoc />
        public void Dispose()
        {
            this.Clear();
        }

        protected Operator Operator { get; set; }

        protected IQueryable<TEntity> FilterByEditDate<TEntity>(IQueryable<TEntity> query)
            where TEntity : BaseEntity
        {
            return query.WhereIf(this.UseIncremental, x => this.StartEditDate < x.ObjectEditDate && x.ObjectEditDate < this.EndEditDate);
        }

        protected bool UseIncremental => this.SelectParams.GetAs("UseIncremental", false);
        protected bool WithoutAttachment => this.SelectParams.GetAs("WithoutAttachment", false);
        protected DateTime StartEditDate => this.SelectParams.GetAs("StartEditDate", DateTime.MinValue);
        protected DateTime EndEditDate => this.SelectParams.GetAs("EndEditDate", DateTime.Today.AddDays(1));

        /// <summary>
        /// Исключает 0
        /// </summary>
        protected int? NonZero(int? value)
        {
            return value != 0 ? value : default(int?);
        }

        /// <summary>
        /// Исключает 0
        /// </summary>
        protected decimal? NonZero(decimal? value)
        {
            return value != decimal.Zero ? value : default(decimal?);
        }

        private TService InitService<TService>(string serviceName)
        {
            var service = this.SelectParams.GetAs<TService>(serviceName);
            if (service == null)
            {
                throw new TypeInitializationException(this.GetType().FullName, new Exception($"Не удалось инициализировать {typeof(TService).Name}"));
            }
            return service;
        }
    }
}