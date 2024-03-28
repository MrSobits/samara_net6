namespace Bars.Gkh.Domain.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Microsoft.Extensions.Logging;

    using NHibernate;

    /// <summary>
    ///     InMemory кэш сущностей.
    ///     Строится при первом запросе списка сущностей.
    ///     Также содержит набор выражений для построения словарей сущностей.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public class DtoEntityCache<T, TDto> : BaseEntityCache<TDto>
        where T : class, IEntity where TDto : class
    {
        private readonly List<Func<IRepository<T>, IQueryable<TDto>>> queryBuilders;

        /// <summary>
        ///     Создает новый экземпляр
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="logCacheInvalidation"></param>
        public DtoEntityCache(
            Func<IRepository<T>, IQueryable<TDto>> queryBuilder,
            bool logCacheInvalidation = false)
            : base(logCacheInvalidation)
        {
            this.EntityName = typeof(TDto).GetTypeUid();
            this.queryBuilders = new List<Func<IRepository<T>, IQueryable<TDto>>>();
            if (queryBuilder != null)
            {
                this.queryBuilders.Add(queryBuilder);
            }
        }

        /// <summary>
        ///     Установка нового построителся запроса
        /// </summary>
        public DtoEntityCache<T, TDto> SetQueryBuilder(Func<IRepository<T>, IQueryable<TDto>> queryBuilder)
        {
            if (queryBuilder != null)
            {
                this.queryBuilders.Add(queryBuilder);
            }
            return this;
        }

        /// <summary>
        ///     Обновление кэша
        ///     <param name="force">Принудительное обновление</param>
        /// </summary>
        public override void Update(bool force = false)
        {
            if (this.WasUpdateInCurrentSession)
            {
                return;
            }

            if (this.Items.IsNotEmpty() && this.ExpireAt > DateTime.Now && !force)
            {
                return;
            }

            if (this.LogCacheInvalidation)
            {
                this.Log.LogDebug("Обновление кэша сущностей {0}...", this.EntityName);
            }

            var sw = Stopwatch.StartNew();

            var container = ApplicationContext.Current.Container;
            var repo = container.ResolveRepository<T>();

            using (container.Using(repo))
            {
                this.Items.Clear();
                this.Dictionaries.Clear();

                foreach (var queryBuilder in this.queryBuilders)
                {
                    this.AddEntity(this.BuildFetchQuery(queryBuilder(repo)).ToArray());
                }

                if (this.LogCacheInvalidation)
                {
                    this.Log.LogDebug("Обновление кэша завершено за {0}...", sw.Elapsed);
                }

                sw.Reset();

                this.WasUpdateInCurrentSession = true;
            }
        }

        /// <summary>
        /// Обновление кеша с StatelessSession
        /// </summary>
        /// <param name="ss"></param>
        public override void UpdateStateless(IStatelessSession ss)
        {
            if (this.WasUpdateInCurrentSession)
            {
                return;
            }

            if (this.Items.IsNotEmpty() && this.ExpireAt > DateTime.Now)
            {
                return;
            }

            if (this.LogCacheInvalidation)
            {
                this.Log.LogDebug("Обновление кэша сущностей {0}...", this.EntityName);
            }

            var sw = Stopwatch.StartNew();

            var repo = new StatelessNhRepository<T>(ss);

            this.Items.Clear();
            this.Dictionaries.Clear();

            foreach (var queryBuilder in this.queryBuilders)
            {
                this.AddEntity(this.BuildFetchQuery(queryBuilder(repo)).ToArray());
            }

            if (this.LogCacheInvalidation)
            {
                this.Log.LogDebug("Обновление кэша завершено за {0}...", sw.Elapsed);
            }

            sw.Reset();

            this.WasUpdateInCurrentSession = true;
        }
    }
}