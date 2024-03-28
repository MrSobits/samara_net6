namespace Bars.Gkh.Domain.Cache
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;

    using Microsoft.Extensions.Logging;

    using NHibernate;
    using NHibernate.Linq;

    public abstract class BaseEntityCache<T> : IEntityCache<T>
        where T : class
    {
        protected readonly Dictionary<string, Dictionary<string, T>> Dictionaries =
            new Dictionary<string, Dictionary<string, T>>(StringComparer.Ordinal);

        protected readonly IList<T> Items = new List<T>(100);

        private readonly IList<EntityKeyBuilder<T>> keyBuilders = new List<EntityKeyBuilder<T>>();

        private readonly object syncRoot = new object();

        protected string EntityName;

        protected DateTime ExpireAt = DateTime.MaxValue;

        private TimeSpan livingTime = TimeSpan.Zero;

        private ILogger log;

        protected bool LogCacheInvalidation;

        protected bool WasUpdateInCurrentSession;

        protected BaseEntityCache(bool logCacheInvalidation)
        {
            this.LogCacheInvalidation = logCacheInvalidation;
        }

        /// <summary>
        ///     Логгер
        /// </summary>
        protected ILogger Log
        {
            get
            {
                return this.log ?? (this.log = ApplicationContext.Current.Container.Resolve<ILogger>());
            }
            set
            {
                this.log = value;
            }
        }

        /// <summary>
        ///     Добавление сущности в кэш.
        ///     Сущность будет добавлена во внутреннюю коллекцию элементов,
        ///     также для нее будут сформирвоаны элементы во всех внутренних словарях
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEntityCache<T> AddEntity(params T[] entities)
        {
            lock (this.syncRoot)
            {
                foreach (var entity in entities)
                {
                    this.Items.Add(entity);

                    this.UpdateDictionaryForRecord(entity);
                }
            }

            return this;
        }

        /// <summary>
        ///     Удаление сущности из всех словарей
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEntityCache<T> DeleteEntity(params T[] entities)
        {
            lock (this.syncRoot)
            {
                foreach (var entity in entities)
                {
                    this.DeleteEntityFromDictionaries(entity);
                }
            }

            return this;
        }

        /// <summary>
        ///     Получение записи из именованного словаря, построенного с помощью метода
        ///     <see>
        ///         <cref>MakeDictionary</cref>
        ///     </see>
        /// </summary>
        /// <param name="name">Наименование словаря</param>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public T GetByKey(string name, string key)
        {
            lock (this.syncRoot)
            {
                Dictionary<string, T> dictionary;
                T value;
                if (!this.Dictionaries.TryGetValue(name, out dictionary))
                {
                    return null;
                }
                return !dictionary.TryGetValue(key, out value) ? null : value;
            }
        }

        /// <summary>
        ///     Получение записи из именованного словаря #Default
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public T GetByKey(string key)
        {
            lock (this.syncRoot)
            {
                var list = new List<T>();
                //return GetByKey("#Default", key);
                foreach (var dictionary in this.Dictionaries.Values)
                {
                    T entry;
                    if (dictionary.TryGetValue(key, out entry))
                    {
                        list.Add(entry);
                        if ((entry as IEntity).Return(x => x.Id).CastAs<long>() > 0)
                        {
                            return entry;
                        }
                    }
                }

                return list.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Получение списка записей из именованного словаря по набору ключей
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T[] GetByKeys(string name, IEnumerable<string> keys)
        {
            Dictionary<string, T> dictionary;
            if (!this.Dictionaries.TryGetValue(name, out dictionary))
            {
                return new T[0];
            }
            var list = new ConcurrentBag<T>();

            keys.AsParallel().ForAll(
                key =>
                    {
                        T entry;
                        if (dictionary.TryGetValue(key, out entry) && entry.IsNotNull())
                        {
                            list.Add(entry);
                        }
                    });

            return list.ToArray();
        }

        /// <summary>
        ///     Получение общего списка зарегистрированных сущностей
        /// </summary>
        /// <returns></returns>
        public IList<T> GetEntities()
        {
            return this.Items;
        }

        /// <summary>
        ///     Регистрация словаря элементов для последующего поиска
        /// </summary>
        /// <param name="name">Имя словаря</param>
        /// <param name="keyComparer">Метод сравнения ключей</param>
        /// <param name="builder">Функция построения ключей</param>
        /// <param name="filter">Предикат, определяющий необходимость помещения элемента в словарь</param>
        /// <returns></returns>
        public IEntityCache<T> MakeDictionary(
            string name,
            StringComparer keyComparer,
            Func<T, string> builder,
            Predicate<T> filter = null)
        {
            ArgumentChecker.NotNull(name, "name");
            var item = this.keyBuilders.FirstOrDefault(x => x.Name == name);
            if (item.IsNotNull())
            {
                return this;
            }

            new EntityKeyBuilder<T>(name, keyComparer, builder, filter).AddTo(this.keyBuilders);
            if (this.Items.IsEmpty())
            {
                return this;
            }
            lock (this.syncRoot)
            {
                this.UpdateDictionary(name);
            }
            return this;
        }

        /// <summary>
        ///     Регистрация словаря элементов для последующего поиска
        /// </summary>
        /// <param name="name">Имя словаря</param>
        /// <param name="builder">Функция построения ключей</param>
        /// <param name="filter">Предикат, определяющий необходимость помещения элемента в словарь</param>
        /// <returns></returns>
        public IEntityCache<T> MakeDictionary(
            string name,
            Func<T, string> builder,
            Predicate<T> filter = null)
        {
            return this.MakeDictionary(name, StringComparer.Ordinal, builder, filter);
        }

        /// <summary>
        ///     Регистрация словаря элементов для последующего поиска. В качестве имени словаря будет использован ключ "#Default"
        /// </summary>
        /// <param name="builder">Функция построения ключей</param>
        /// <param name="filter">Предикат, определяющий необходимость помещения элемента в словарь</param>
        /// <returns></returns>
        public IEntityCache<T> MakeDictionary(Func<T, string> builder, Predicate<T> filter = null)
        {
            return this.MakeDictionary("#Default", builder, filter);
        }

        /// <summary>
        ///     Установить время жизни кэша.
        ///     Проверяется при попытке обновления кэша в импортерах.
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public IEntityCache<T> SetLivingTime(TimeSpan timespan)
        {
            if (timespan == this.livingTime)
            {
                return this;
            }
            this.livingTime = timespan;
            this.ExpireAt = this.ExpireAt.In(DateTime.MinValue, DateTime.MaxValue)
                            ? DateTime.Now.Add(this.livingTime)
                            : this.ExpireAt.Add(this.livingTime);

            if (this.LogCacheInvalidation)
            {
                this.Log.LogDebug("Время жизни кэша {0} установлено в {1}", this.EntityName, timespan);
            }

            return this;
        }

        public void UpdateDictionary(string name)
        {
            var keyBuilder = this.keyBuilders.FirstOrDefault(x => x.Name == name);
            if (keyBuilder.IsNull())
            {
                return;
            }

            Dictionary<string, T> dictionary;
            if (!this.Dictionaries.TryGetValue(keyBuilder.Name, out dictionary))
            {
                dictionary = new Dictionary<string, T>(this.Items.Count, keyBuilder.Comparer);
                this.Dictionaries[keyBuilder.Name] = dictionary;
            }

            dictionary.Clear();
            this.Items.ForEach(
                x =>
                    {
                        if (!keyBuilder.Filter(x))
                        {
                            return;
                        }
                        var key = keyBuilder.Builder(x);
                        if (key.IsEmpty())
                        {
                            return;
                        }
                        dictionary[key] = x;
                    });
        }

        /// <summary>
        ///     Обновление кэша сущностей.
        ///     Будут обновлены внутренние словари
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEntityCache<T> UpdateEntity(params T[] entities)
        {
            lock (this.syncRoot)
            {
                foreach (var entity in entities)
                {
                    this.UpdateDictionaryForRecord(entity);
                }
            }

            return this;
        }

        /// <summary>
        ///     Очистка кэша
        /// </summary>
        public void Clear()
        {
            lock (this.syncRoot)
            {
                this.Items.Clear();
                this.Dictionaries.Clear();
                this.WasUpdateInCurrentSession = false;
            }
        }

        /// <summary>
        ///     Обновление кэша
        ///     <param name="force">Принудительное обновление</param>
        /// </summary>
        public abstract void Update(bool force = false);

        /// <summary>
        /// Обновление кеша с StatelessSession
        /// </summary>
        /// <param name="ss"></param>
        public abstract void UpdateStateless(IStatelessSession ss);

        protected IQueryable<T> BuildFetchQuery(IQueryable<T> query)
        {
            return query;

            // Пока пишем вручную

            var entityProperties =
                typeof(T).GetProperties().Where(x => typeof(IEntity).IsAssignableFrom(x.PropertyType));

            var paramExpr = Expression.Parameter(typeof(T));

            foreach (var entityProperty in entityProperties)
            {
                var propExpr = Expression.Property(paramExpr, entityProperty);
                var lambda = Expression.Lambda(propExpr, paramExpr);

                var callExpr = Expression.Call(
                    typeof(EagerFetchingExtensionMethods),
                    "Fetch",
                    new[] { typeof(T), propExpr.Type },
                    Expression.Constant(query),
                    lambda);

                query = Expression.Lambda(callExpr).Compile().DynamicInvoke() as IQueryable<T>;
            }

            return query;
        }

        private void DeleteEntityFromDictionaries(T entity)
        {
            foreach (var keyBuilder in this.keyBuilders)
            {
                Dictionary<string, T> dictionary;
                if (!this.Dictionaries.TryGetValue(keyBuilder.Name, out dictionary))
                {
                    continue;
                }

                var key = keyBuilder.Builder(entity);
                // если ключ пуст или нет сущности в словаре, то пропускаем его
                if (key.IsEmpty() || !dictionary.ContainsKey(key))
                {
                    continue;
                }

                dictionary.Remove(key);
            }
        }

        private void UpdateDictionaryForRecord(T entity)
        {
            // обходим коллекцию построителей ключей
            foreach (var keyBuilder in this.keyBuilders)
            {
                if (!keyBuilder.Filter(entity))
                {
                    continue;
                }
                Dictionary<string, T> dictionary;
                if (!this.Dictionaries.TryGetValue(keyBuilder.Name, out dictionary))
                {
                    dictionary = new Dictionary<string, T>(this.Items.Count, keyBuilder.Comparer);
                    this.Dictionaries[keyBuilder.Name] = dictionary;
                }
                var key = keyBuilder.Builder(entity);
                // если ключ пуст, то пропускаем его
                if (key.IsEmpty())
                {
                    continue;
                }
                dictionary[key] = entity;
            }
        }
    }
}