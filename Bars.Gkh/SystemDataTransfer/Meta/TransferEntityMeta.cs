namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;

    using Newtonsoft.Json;

    using NHibernate;
    using NHibernate.Metadata;
    using NHibernate.Persister.Entity;

    /// <summary>
    /// Класс описания класса
    /// </summary>
    public class TransferEntityMeta<TEntity> : ITransferEntityMeta where TEntity : class, IEntity
    {
        private readonly HashSet<PropertyInfo> properties = new HashSet<PropertyInfo>();

        public HashSet<PropertyInfo> IgnoreProperties { get; } = new HashSet<PropertyInfo>();

        private TypeMetaContainer Container => TypeMetaContainer.Instance;

        /// <inheritdoc />
        public string Description { get; set; }

        public Expression<Func<TEntity, bool>> FilterExpression { get; set; }

        /// <inheritdoc />
        object ITransferEntityMeta.Serialize(object obj)
        {
            return this.Serializer!= null ? this.Serializer.Serialize(obj) : obj;
        }

        /// <inheritdoc />
        public IImportEntity GetImportMeta(FileProcessingHelper fileProcessingHelper)
        {
            return new ImportEntity<TEntity>(this, fileProcessingHelper);
        }

        /// <inheritdoc />
        public bool HasCustomSerializer => this.Serializer.IsNotNull();

        /// <inheritdoc />
        public Type Type => typeof(TEntity);

        /// <inheritdoc />
        public Type BaseType { get; set; }

        /// <inheritdoc />
        public bool IsBase { get; set; }

        /// <inheritdoc />
        public IEnumerable<PropertyInfo> KeyProperties => this.properties;

        /// <inheritdoc />
        IEnumerable<PropertyInfo> ITransferEntityMeta.IgnoreProperties => this.IgnoreProperties;

        public void AddProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertySelector)
        {
            this.properties.Add((PropertyInfo)((MemberExpression)propertySelector.Body).Member);
        }

        /// <inheritdoc />
        public IQueryable<IEntity> Filter(IQueryable<IEntity> query)
        {
            var genericQuery = (IQueryable<TEntity>)query;

            return this.FilterExpression.IsNotNull()
                ? genericQuery.Where(this.FilterExpression)
                : genericQuery;
        }

        /// <inheritdoc />
        public bool IsPartially { get; set; }

        public bool CreateNew { get; set; }

        public IDataSerializer<TEntity> Serializer { get; set; }

        /// <inheritdoc />
        public List<Dependency> GetDependencies()
        {
            var props = this.Type.GetProperties().Where(x => typeof(IEntity).IsAssignableFrom(x.PropertyType)).ToList();

            if (!props.Any())
            {
                return new List<Dependency>();
            }

            var list = new List<Dependency>();
            var visitedTypes = new HashSet<string>();

            this.CollectDependencies(props, list, visitedTypes);

            return list;
        }

        /// <inheritdoc />
        public PropertyInfo[] GetExportableProperties(Type finalType = null)
        {
            var classMeta = this.GetClassMetaData();

            var type = finalType ?? this.Type;
            return type
                .GetProperties()
                .Where(x => this.KeyProperties.Any(y => y.Name == x.Name) || !x.IsDefined(typeof(JsonIgnoreAttribute), true))
                .Where(x => this.IgnoreProperties.All(y => y.Name != x.Name))
                .WhereIf(type.Is<IHaveExportId>(), x => x.Name != "ExportId")               // ExportId во внешней системе свои
                .Where(x => classMeta.PropertyNames.Contains(x.Name) || x.Name == "Id")     // Выгружаем только свойства, которые имеют маппинг
                .ToArray();
        }

        /// <inheritdoc />
        public TypeMetadata GetTypeMetadata()
        {
            return this.Container.GetMeta(this.Type);
        }

        /// <summary>
        /// Получение метаинформации NHibernate
        /// </summary>
        /// <returns>NHibernate IClassMetadata</returns>
        public IClassMetadata GetClassMetaData()
        {
            return this.Container.GetMeta(this.Type).PersistentMetadata;
        }

        /// <summary>
        /// Получение метаинформации NHibernate
        /// </summary>
        /// <param name="type">Тип хранимой сущности</param>
        /// <returns>NHibernate IClassMetadata</returns>
        private IClassMetadata GetClassMetaData(Type type)
        {
            return this.Container.GetMeta(type).PersistentMetadata;
        }

        /// <summary>
        /// Получение метаинформации для типа
        /// </summary>
        /// <param name="tableName">Название таблицы в БД</param>
        /// <returns>TypeMetadata</returns>
        public TypeMetadata GetTypeMetadata(string tableName)
        {
            return this.Container.GetMeta(tableName);
        }

        IDataSerializer ITransferEntityMeta.Serializer => this.Serializer;

        /// <summary>
        /// Сбор ссылок на зависимости
        /// </summary>
        /// <param name="type">Тип хранимой сущности</param>
        /// <param name="props">Список свойств сущности, которые явядяются ссылками на другие сущности</param>
        /// <param name="list">Список, заполняемый при обходе зависимостей</param>
        /// <param name="visitedTypes">Список посещенных зависимостей</param>
        private void CollectDependencies(IEnumerable<PropertyInfo> props, List<Dependency> list,
            HashSet<string> visitedTypes)
        {
            var topTypeMeta = this.GetClassMetaData(this.Type) as AbstractEntityPersister;
            if (topTypeMeta.IsNull())
            {
                return;
            }

            foreach (var propertyInfo in props)
            {
                if (!visitedTypes.Contains(propertyInfo.PropertyType.FullName) && topTypeMeta.PropertyNames.Contains(propertyInfo.Name))
                {
                    var tmpType = propertyInfo.PropertyType;

                    visitedTypes.Add(tmpType.FullName);

                    var meta = this.GetClassMetaData(tmpType) as AbstractEntityPersister;
                    var joinedSubclassMeta = meta as JoinedSubclassEntityPersister;
                    if (meta != null)
                    {
                        list.Add(new Dependency
                        {
                            Type = tmpType,
                            Name = propertyInfo.Name,
                            IsParent = this.Type == propertyInfo.PropertyType,
                            IsRoot = joinedSubclassMeta?.HasSubclasses ?? false
                        });
                    }
                }
            }

            var xParam = Expression.Parameter(typeof(TEntity), "x");
            var selctExpression = (this.Serializer as AbstractDataSerializer<TEntity>)?.GetEntitySelectExpression(xParam);
            if (selctExpression.IsNotNull())
            {
                var returnType = selctExpression.Type;
                if (returnType.Is<IEntity>())
                {
                    list.Add(new Dependency
                    {
                        Name = "ComplexKey",
                        Type = returnType,
                        IsParent = false
                    });
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Serializer?.Dispose();
        }
    }

    internal class TypeMetaContainer
    {
        public static TypeMetaContainer Instance = new TypeMetaContainer();

        private ISessionProvider provider = ApplicationContext.Current.Container.Resolve<ISessionProvider>();
        private IDictionary<Type, TypeMetadata> metaDictionary;

        private TypeMetaContainer()
        {
            this.metaDictionary = new Dictionary<Type, TypeMetadata>();
        }

        public TypeMetadata GetMeta(Type type)
        {
            var meta = this.metaDictionary.Get(type);
            if (meta.IsNull())
            {
                var typeMeta = this.provider.GetCurrentSession().SessionFactory.GetClassMetadata(type) as AbstractEntityPersister;
                var props = type.GetProperties().Where(x => !x.IsDefined(typeof(JsonIgnoreAttribute), true) && x.CanWrite);

                meta = new TypeMetadata
                {
                    Type = type,
                    PersistentMetadata = typeMeta,
                    TableName = typeMeta.TableName
                };

                foreach (var propertyInfo in props)
                {
                    string columnName;
                    try
                    {
                        columnName = typeMeta.GetPropertyColumnNames(propertyInfo.Name).First();
                    }
                    catch (HibernateException)
                    {
                        continue;
                    }

                    meta.Columns.Add(columnName, propertyInfo);
                }

                this.metaDictionary.Add(type, meta);
            }

            return meta;
        }

        public TypeMetadata GetMeta(string tableName)
        {
            var meta = this.metaDictionary.Values.FirstOrDefault(x => x.TableName == tableName);
            if (meta.IsNotNull())
            {
                return meta;
            }

            var persister = this.provider.GetCurrentSession()
                .SessionFactory.GetAllClassMetadata()
                .FirstOrDefault(x => ((AbstractEntityPersister)x.Value).TableName == tableName)
                .Return(x => x.Value);

            var type = persister.MappedClass;

            return this.GetMeta(type);
        }
    }

    /// <summary>
    /// Класс описания класса
    /// </summary>
    public interface ITransferEntityMeta : IDisposable
    {
        /// <summary>
        /// Тип
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Базовый тип
        /// </summary>
        Type BaseType { get; }

        /// <summary>
        /// Тип является базовым (не экспортируется, нужен только для работы со ссылками в кэше)
        /// </summary>
        bool IsBase { get; }

        /// <summary>
        /// Селекторы ключевых свойств
        /// </summary>
        IEnumerable<PropertyInfo> KeyProperties { get; }

        /// <summary>
        /// Игнорируемые свойства
        /// </summary>
        IEnumerable<PropertyInfo> IgnoreProperties { get; }

        /// <summary>
        /// Метод фильтрации выгружаемых сущностей
        /// </summary>
        /// <param name="query">Входной запрос при получении идентификаторов</param>
        IQueryable<IEntity> Filter(IQueryable<IEntity> query);

        /// <summary>
        /// Признак частично выгружаемой сущности
        /// </summary>
        bool IsPartially { get; }

        /// <summary>
        /// Всегда создавать сущность (сущность не требует сопоставления)
        /// </summary>
        bool CreateNew { get; }

        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Метод сериализации сущности
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns>Сохраняемые данные</returns>
        object Serialize(object obj);

        /// <summary>
        /// Вернуть импортируемую сущность
        /// </summary>
        /// <param name="fileProcessingHelper">Помощник обработки импорта дополнительных файлов</param>
        IImportEntity GetImportMeta(FileProcessingHelper fileProcessingHelper);

        /// <summary>
        /// Тип имеет переопределенный сериализатор
        /// </summary>
        bool HasCustomSerializer { get; }

        /// <summary>
        /// Сериализатор
        /// </summary>
        IDataSerializer Serializer { get; }

        /// <summary>
        /// Получение метаинформации NHibernate
        /// </summary>
        /// <returns>NHibernate IClassMetadata</returns>
        IClassMetadata GetClassMetaData();

        /// <summary>
        /// Получение метаинформации TypeMetadata
        /// </summary>
        TypeMetadata GetTypeMetadata();

        /// <summary>
        /// Сформировать список зависимостей
        /// </summary>
        List<Dependency> GetDependencies();

        /// <summary>
        /// Вернуть список экспортируемых свойств
        /// </summary>
        /// <param name="finalType">Конечный тип, на случай, если в результате сериализации изменился тип сущности</param>
        PropertyInfo[] GetExportableProperties(Type finalType = null);
    }
}