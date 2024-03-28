namespace Bars.Gkh.RegOperator.Caching
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    using Microsoft.Extensions.Logging;
    using Castle.Windsor;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.Enums;
    using Bars.B4.Modules.Caching.Interfaces;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Utils;

    using Castle.MicroKernel.Lifestyle;

    /// <summary>
    /// Отличается от дефолтной стратегии тем, что пробрасывает исключение инвалидации наверх - не использовать ее при инвалидации на старте приложения
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class CustomPersistentInvalidationStrategy<TValue, TKey> : IInvalidationStrategy
        where TValue : class, IEntity, new()
    {
        private readonly int bulkSize;
        private readonly IWindsorContainer container;
        private readonly ICacheClient<TValue> cache;
        private readonly ILogger logger;
        private readonly Expression<Func<TValue, bool>> filter;

        public CustomPersistentInvalidationStrategy(IWindsorContainer container, IAppCache appCache, Expression<Func<TValue, bool>> filter = null, int bulkSize = 1000)
        {
            this.container = container;
            this.bulkSize = bulkSize;
            this.filter = filter;
            this.cache = appCache.GetInvalidationClient<TValue>();
            this.logger = container.Resolve<ILogger>();
        }

        public bool Invalidate()
        {
            using (this.container.BeginScope())
            {
                var sw = Stopwatch.StartNew();
                var name = typeof(TValue).Name;
                var fName = typeof(TValue).FullName;

                this.logger.LogInformation("Кэш:({0}) Прогрев кэша для {1}".FormatUsing(name, fName));
                var domain = this.container.ResolveRepository<TValue>();
                var sessions = this.container.Resolve<ISessionProvider>();

                try
                {
                    var cachedKeys = this.cache.GetAllKeys().ToList().Select(x => x.To<TKey>()).ToList();

                    var persistentKeys =
                        domain.GetAll()
                            .WhereIf(this.filter != null, this.filter)
                            .OrderBy(x => x.Id)
                            .Select(x => x.Id)
                            .ToList()
                            .Cast<TKey>()
                            .ToList();

                    var cacheExceptDb = cachedKeys.Except(persistentKeys).ToList();
                    var dbExceptCache = persistentKeys.Except(cachedKeys.ToList());

                    // 1) Delete all excess _cache
                    this.cache.RemoveAll(this.cache.GetAll(cacheExceptDb.Cast<object>()));

                    // 2) Get new data from db and put to _cache
                    var objectKeys = dbExceptCache.ToList();

                    this.logger.LogInformation("Кэш:({0}) К заполнению {1}".FormatUsing(name, objectKeys.Count));
                    var counter = 0;
                    foreach (var partKeys in objectKeys.Split(this.bulkSize))
                    {
                        var fromDb = domain.GetAll().Where(x => partKeys.Contains((TKey)x.Id)).ToList();
                        this.cache.AddMany(fromDb, true);

                        counter += partKeys.Count();

                        this.logger.LogInformation("Кэш:({0}) Заполнено {1} из {2}".FormatUsing(name, counter, objectKeys.Count));

                        if (counter % 10000 == 0)
                        {
                            sessions.CloseCurrentSession();
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Кэш:({0}). Ошибка при обновлении кэша.", name);
                    throw;
                }
                finally
                {
                    sw.Stop();
                    this.logger.LogInformation("Кэш:({0}) Кэш прогрет за {1}".FormatUsing(name, sw.Elapsed));
                    this.container.Release(domain);

                    sessions.CloseCurrentSession();
                    this.container.Release(sessions);
                }
            }
        }

        public bool CanInvalidate(object item)
        {
            return item.Is<TValue>();
        }

        public void InvalidateEntry(object item, EntryUpdateType operation)
        {
            var typed = item as TValue;
            if (typed == null)
                return;

            switch (operation)
            {
                case EntryUpdateType.Create:
                case EntryUpdateType.Update:
                    this.cache.Add(typed);
                    break;
                case EntryUpdateType.Delete:
                    this.cache.Remove(typed);
                    break;
            }
        }
    }
}
