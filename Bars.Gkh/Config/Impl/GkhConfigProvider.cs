namespace Bars.Gkh.Config.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Impl.Internal;
    using Bars.Gkh.Config.Impl.Internal.Proxy;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    ///     Провайдер конфигурации
    /// </summary>
    public class GkhConfigProvider : IGkhConfigProvider
    {
        private const string ConfigPrefix = "GkhConfig";
        private const string CacheLifeTimeKey = "CacheLifeTime";

        private static IDictionary<string, PropertyMetadata> map;
        private static IDictionary<string, ValueHolder> valueHolders;
        private static ISet<string> mappedKeys;
        private static bool mappingCompleted;
        private static DateTime lastInvalidation;
        private static readonly object Sync = new object();
        private readonly int cacheLifeTime;


        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">Контейнер Castle Windsor</param>
        /// <param name="storageBackend">Хранилище настроек</param>
        public GkhConfigProvider(IWindsorContainer container, IGkhConfigStorageBackend storageBackend)
        {
            this.StorageBackend = storageBackend;
            this.Container = container;

            GkhConfigProvider.map = new ConcurrentDictionary<string, PropertyMetadata>();
            GkhConfigProvider.mappedKeys = new HashSet<string>();

            this.cacheLifeTime = ApplicationContext.Current.Configuration
                .AppSettings.GetAs($"{GkhConfigProvider.ConfigPrefix}.{GkhConfigProvider.CacheLifeTimeKey}", 5);
        }

        private IWindsorContainer Container { get; set; }
        private IGkhConfigStorageBackend StorageBackend { get; set; }

        /// <summary>
        ///     Возвращает полную карту конфигурации
        /// </summary>
        public IDictionary<string, PropertyMetadata> Map
        {
            get
            {
                return GkhConfigProvider.map;
            }
        }

        /// <summary>
        /// Значения
        /// </summary>
        public IDictionary<string, ValueHolder> ValueHolders
        {
            get
            {
                return GkhConfigProvider.valueHolders;
            }
        }
        
        /// <summary>
        ///     Выполняет сопоставление загруженной конфигурации с зарегистрированными
        ///     конфигурационными секциями
        /// </summary>
        public virtual void CompleteMapping()
        {
            var sections = this.Container.ResolveAll<IGkhConfigSection>();
            foreach (var section in sections)
            {
                var type = section.GetType();
                var key = this.GetSectionAlias(type);
                if (!GkhConfigProvider.mappedKeys.Contains(key))
                {
                    this.MapClass(type);
                }

                this.Container.Release(section);
            }

            GkhConfigProvider.valueHolders.Where(x => !GkhConfigProvider.map.ContainsKey(x.Key))
                        .Select(x => x.Key)
                        .ToArray()
                        .ForEach(x => GkhConfigProvider.valueHolders.Remove(x));

            GkhConfigProvider.mappingCompleted = true;
        }

        /// <summary>
        ///     Возвращает корневую секцию
        /// </summary>
        /// <typeparam name="T">Тип корневой секции</typeparam>
        /// <returns>Экземпляр класса T</returns>
        public virtual T Get<T>() where T : class, IGkhConfigSection
        {
            var key = this.GetSectionAlias(typeof(T));
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception(
                    "Секция конфигурации обязательно должна иметь аттрибут GkhConfigSectionAttribute с корректно заданым псевдонимом");
            }

            return this.GetByKey<T>(key);
        }

        /// <summary>
        ///     Возвращает корневую секцию
        /// </summary>
        /// <param name="type">
        ///     Тип секции. Должен реализовывать IGkhConfigSection
        /// </param>
        /// <returns>
        ///     Экземпляр класса type
        /// </returns>
        public virtual object Get(Type type)
        {
            if (!typeof(IGkhConfigSection).IsAssignableFrom(type))
            {
                throw new Exception("Секция конфигурации должна реализовывать интерфейс IGkhConfigSection");
            }

            var key = this.GetSectionAlias(type);
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception(
                    "Секция конфигурации обязательно должна иметь аттрибут GkhConfigSectionAttribute с корректно заданым псевдонимом");
            }

            return this.GetByKey(key, type.GetDefaultValue(), type);
        }

        /// <summary>
        ///     Возвращает значение конфигурации по указанному полному ключу
        /// </summary>
        /// <param name="key">Полный ключ</param>
        /// <param name="defaultValue">Стандартное значение на случай, если ключ не задан</param>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <returns>Значение конфигурации по указанному полному ключу</returns>
        public virtual T GetByKey<T>(string key, T defaultValue = default(T))
        {
            return (T)this.GetByKey(key, defaultValue, typeof(T));
        }

        /// <summary>
        ///     Возвращает значение конфигурации по указанному полному ключу
        /// </summary>
        /// <param name="key">
        ///     Полный ключ
        /// </param>
        /// <param name="defaultValue">
        ///     Стандартное значение на случай, если ключ не задан
        /// </param>
        /// <param name="type">
        ///     Тип возвращаемого значения.
        ///     Если null — попробует определить по маппингу
        ///     Если определить не удастся — вернет текстовое представление исходого JSON
        /// </param>
        /// <returns>
        ///     Значение конфигурации по указанному полному ключу
        /// </returns>
        public virtual object GetByKey(string key, object defaultValue, Type type = null)
        {
            if ((DateTime.Now - GkhConfigProvider.lastInvalidation).Minutes >= this.cacheLifeTime)
            {
                lock (GkhConfigProvider.Sync)
                {
                    if ((DateTime.Now - GkhConfigProvider.lastInvalidation).Minutes >= this.cacheLifeTime)
                    {
                        this.InvalidateCache();
                    }
                }
            }

            if (typeof(IGkhConfigSection).IsAssignableFrom(type))
            {
                if (!GkhConfigProvider.mappedKeys.Contains(key))
                {
                    this.MapClass(type, key);
                }

                return ConfigProxyGenerator.Generate(type, GkhConfigProvider.valueHolders, key);
            }

            ValueHolder holder;
            if (!GkhConfigProvider.valueHolders.TryGetValue(key, out holder))
            {
                return defaultValue;
            }

            type = type ?? holder.Type;
            if (type == null)
            {
                return holder.Value;
            }

            if (holder.Value == null)
            {
                return type.GetDefaultValue();
            }

            if (type.IsInstanceOfType(holder.Value))
            {
                return holder.Value;
            }

            object value;
            if (ValueHolder.TryConvert(holder.Value, type, out value))
            {
                return value;
            }

            throw new InvalidCastException(string.Format("Не удалось привести значение поля {0} к типу {1}", key, type.Name));
        }

        /// <summary>
        ///     Перезагружает сохраненные настройки из бэкенда
        /// </summary>
        protected virtual void InvalidateCache()
        {
            var configs = this.StorageBackend.GetConfig();
            foreach (var config in configs)
            {
                ValueHolder holder;
                if (GkhConfigProvider.valueHolders.TryGetValue(config.Key, out holder))
                {
                    holder.SetValue(config.Value.Value, true);
                }
            }

            GkhConfigProvider.lastInvalidation = DateTime.Now;
        }

        /// <summary>
        ///     Загружает конфигурацию
        /// </summary>
        public virtual void LoadConfiguration()
        {
            GkhConfigProvider.mappingCompleted = false;
            GkhConfigProvider.valueHolders = new ConcurrentDictionary<string, ValueHolder>(this.StorageBackend.GetConfig());

            GkhConfigProvider.lastInvalidation = DateTime.Now;
        }

        /// <summary>
        ///     Сохраняет внесенные изменения
        /// </summary>
        public virtual Exception SaveChanges()
        {
            if (!GkhConfigProvider.mappingCompleted)
            {
                throw new Exception("Для сохранения необходимо сначала завершить инициализацию");
            }

            Exception exception = null;
            try
            {
                this.StorageBackend.UpdateConfig(
                    GkhConfigProvider.valueHolders.Where(x => x.Value.IsModified).ToDictionary(x => x.Key, x => x.Value));
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            GkhConfigProvider.valueHolders.Select(x => x.Value).Where(x => x.IsModified).ForEach(
                x =>
                {
                    if (exception == null)
                    {
                        x.IsModified = false;
                    }
                    else
                    {
                        x.Revert();
                    }
                });

            return exception;
        }
        
        /// <summary>
        ///     Определяет алиас секции
        /// </summary>
        /// <param name="type">Тип секции</param>
        /// <returns>Алиас</returns>
        protected virtual string GetSectionAlias(Type type)
        {
            return type.GetAttribute<GkhConfigSectionAttribute>(true).Return(x => x.Alias);
        }

        /// <summary>
        ///     Строит карту секции конфигурации
        /// </summary>
        /// <typeparam name="T">Тип секции</typeparam>
        /// <param name="root">Ключ родительского элемента</param>
        protected virtual void MapClass<T>(string root = null) where T : class, IGkhConfigSection
        {
            this.MapClass(typeof(T), root);
        }

        /// <summary>
        ///     Строит карту секции конфигурации
        /// </summary>
        /// <param name="type">Тип секции. Должен наследоваться от IGkhConfigSection</param>
        /// <param name="root">Ключ родительского элемента</param>
        protected virtual void MapClass(Type type, string root = null)
        {
            lock (GkhConfigProvider.Sync)
            {
                var key = root ?? this.GetSectionAlias(type);
                if (GkhConfigProvider.mappedKeys.Contains(key))
                {
                    return;
                }

                var args = type.HasAttribute<GkhConfigSectionAttribute>(true)
                               ? ClassIndexer.IndexRootSection(type)
                               : ClassIndexer.IndexSubsection(type, null, root);
                var order = 0;

                foreach (var arg in args)
                {
                    var meta = new PropertyMetadata(arg) { Order = order++ };
                    GkhConfigProvider.map.Add(arg.Key, meta);

                    ValueHolder holder;
                    if (GkhConfigProvider.valueHolders.TryGetValue(arg.Key, out holder))
                    {
                        holder.Attach(meta);
                    }
                    else
                    {
                        GkhConfigProvider.valueHolders.Add(arg.Key, new ValueHolder(meta.DefaultValue, meta));
                    }
                }

                GkhConfigProvider.mappedKeys.Add(key);
            }
        }
    }
}