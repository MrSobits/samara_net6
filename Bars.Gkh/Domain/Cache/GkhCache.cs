namespace Bars.Gkh.Domain.Cache
{
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Статический кэш ЕБИРЯ
    /// </summary>
    public class GkhCache : IDisposable
    {
        private readonly Dictionary<string, IEntityCache> cacheList;

        [ThreadStatic]
        private static IStatelessSession statelessSession;

        public static IStatelessSession StatelessSession => GkhCache.statelessSession;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public GkhCache()
        {
            this.cacheList = new Dictionary<string, IEntityCache>(StringComparer.Ordinal);
            this.IsActive = false;
        }

        /// <summary>
        /// Кэш используется
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Очистка кэша
        /// </summary>
        public void Dispose()
        {
            this.Clear();
        }

        /// <summary>
        /// Очистка кэша
        /// </summary>
        public void Clear()
        {
            this.cacheList.Values.ForEach(x => x.Clear());
            this.cacheList.Clear();
            GC.Collect();
        }

        /// <summary>
        /// Получение кэша сущности
        /// </summary>
        /// <typeparam name="T">Тип кэшируемой сущности</typeparam>
        /// <returns>Кэш сущностей</returns>
        /// <exception cref="InvalidOperationException">Коллекция сущностей отсутсвует</exception>
        public IEntityCache<T> GetCache<T>() where T : class
        {
            var uid = typeof(T).GetTypeUid();
            IEntityCache cache;
            if (!this.cacheList.TryGetValue(uid, out cache))
            {
                throw new InvalidOperationException("Коллекция сущностей {0} отсутствует в кэше".FormatUsing(uid));
            }

            return cache.CastAs<IEntityCache<T>>();
        }

        /// <summary>
        /// Получение списка сущностей из кэша
        /// </summary>
        /// <typeparam name="T">Тип кэшируемой сущности</typeparam>
        /// <returns>Список сущностей</returns>
        public IList<T> GetEntities<T>() where T : class
        {
            return this.GetCache<T>().GetEntities();
        }

        /// <summary>
        /// Регистрация кэша для сущности
        /// </summary>
        /// <typeparam name="T">Тип кэшируемой сущности</typeparam>
        /// <typeparam name="TDto">Объект DTO</typeparam>
        /// <returns>Кэш DTO-объектов сущностей</returns>
        public DtoEntityCache<T, TDto> RegisterDto<T, TDto>(
            Func<IRepository<T>, IQueryable<TDto>> queryBuilder = null,
            bool logCacheInvalidation = false) 
            where T : class, IEntity 
            where TDto : class
        {
            var uid = typeof(TDto).GetTypeUid();
            IEntityCache cache;
            if (!this.cacheList.TryGetValue(uid, out cache))
            {
                cache = new DtoEntityCache<T, TDto>(queryBuilder, logCacheInvalidation);
                this.cacheList[uid] = cache;
            }

            return cache.CastAs<DtoEntityCache<T, TDto>>();
        }

        /// <summary>
        /// Регистрация кэша для сущности
        /// </summary>
        /// <typeparam name="T">Тип кэшируемой сущности</typeparam>
        /// <returns>Кэш сущностей</returns>
        public EntityCache<T> RegisterEntity<T>(
            Func<IRepository<T>, IQueryable<T>> queryBuilder = null,
            bool logCacheInvalidation = false) where T : class, IEntity
        {
            var uid = typeof(T).GetTypeUid();
            IEntityCache cache;
            if (!this.cacheList.TryGetValue(uid, out cache))
            {
                cache = new EntityCache<T>(queryBuilder, logCacheInvalidation);
                this.cacheList[uid] = cache;
            }

            return cache.CastAs<EntityCache<T>>();
        }

        /// <summary>
        /// Попытка получить кэш сущности
        /// </summary>
        /// <typeparam name="T">Тип кэшируемой сущности</typeparam>
        /// <param name="cache">Кэш сущностей</param>
        /// <returns>Кэш найден</returns>
        public bool TryGetCache<T>(out IEntityCache<T> cache) where T : class
        {
            var uid = typeof(T).GetTypeUid();

            cache = null;

            IEntityCache icache;
            if (this.cacheList.TryGetValue(uid, out icache))
            {
                cache = icache.CastAs<IEntityCache<T>>();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Обновление кэша
        /// </summary>
        public void Update()
        {
            foreach (var cachedValue in this.cacheList)
            {
                cachedValue.Value.Update();
            }
        }

        /// <summary>
        /// Обновление кэша
        /// </summary>
        public void UpdateStateless(IStatelessSession session)
        {
            foreach (var cachedValue in this.cacheList)
            {
                cachedValue.Value.UpdateStateless(session);
            }
        }

        /// <summary>
        /// Обновление кэша паралельно (попробуем паралельно получать, если лажи не будет оставим)
        /// </summary>
        public void UpdateStateless(ISessionProvider sessionProvider)
        {
            this.cacheList.AsParallel()
                .ForAll(x =>
                {
                    lock (sessionProvider)
                    {
                        GkhCache.statelessSession = sessionProvider.OpenStatelessSession();
                    }

                    using (GkhCache.statelessSession)
                    {
                        x.Value.UpdateStateless(GkhCache.statelessSession);
                    }

                    GkhCache.statelessSession = null;
                });
        }
    }
}