namespace Bars.Gkh.ImportExport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Castle.Windsor;
    using Ionic.Zlib;

    using Newtonsoft.Json;

    using NHibernate;
    using NHibernate.Metadata;
    using NHibernate.Persister.Entity;

    public class ImportExportProvider : IImportExportProvider
    {
        private readonly IWindsorContainer _container;

        private readonly IImportExportLogger _logger;

        private ISessionProvider _sessionProvider;

        public ISessionProvider SessionProvider
        {
            get { return _sessionProvider ?? (_sessionProvider = this._container.Resolve<ISessionProvider>()); }
        }

        private List<TypeMetadata> _typeMeta;

        public List<TypeMetadata> TypeMetadatas
        {
            get { return _typeMeta ?? (_typeMeta = new List<TypeMetadata>()); }
            set { _typeMeta = value; }
        }

        public ImportExportProvider(IWindsorContainer container, IImportExportLogger logger)
        {
            _container = container;
            _logger = logger;
        }

        #region Export

        /// <summary>
        /// Получение списка сущностей, которые можно экспортировать
        /// </summary>
        /// <returns>Список сущностей</returns>
        public Dictionary<string, string> GetEntityNames()
        {
            var dict = GetExportableTypes();

            var result = new Dictionary<string, string>();

            foreach (var typeDescr in dict)
            {
                var typeMeta = GetTypeMetadata(typeDescr.Key);
                result.Add(typeMeta.TableName, typeDescr.Value.Description);
            }

            return result;
        }

        /// <summary>
        /// Выгрузка сущностей из системы
        /// </summary>
        /// <param name="tableNames">Список таблиц, выбранных пользователем</param>
        /// <returns></returns>
        public MemoryStream Export(IEnumerable<string> tableNames = null)
        {
            try
            {
                _logger.Begin(ImportExportType.EXPORT);

                var list = new List<Entity>();

                tableNames = tableNames ?? new List<string>();

                var typesDict = this.GetExportableTypes();

                var types =
                    typesDict
                        .Keys.Select(GetTypeMetadata)
                        .Where(x => tableNames.Contains(x.TableName))
                        .Select(x => x.Type);

                BuildEntityHierarchy(list, types, null, false, typesDict);

                var collection = new EntityCollection()
                {
                    Entities = list
                };

                var refValue = collection.Entities.SelectMany(x => x.Items.SelectMany(y => y.Properties.Select(z => new
                {
                    z.Reference,
                    z.Value
                })));

                var dict = refValue.Where(x => !x.Reference.IsEmpty())
                    .GroupBy(x => x.Reference)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Value.ToInt()).ToList());

                collection.Entities.Reverse();

                /*
                 * Удаляем те сущности, на которые нет ссылок.
                 * Но оставляя те, которые выбрал пользователь, нетронутыми
                 */
                foreach (var entity in
                    collection.Entities.Where(x => !tableNames.Any() || !tableNames.Contains(x.FullName)).ToList())
                {
                    List<int> ids;
                    dict.TryGetValue(entity.FullName, out ids);

                    ids = ids ?? new List<int>();

                    var typeMeta = GetTypeMetadata(entity.FullName);

                    entity.Items.RemoveAll(
                        x =>
                            ids.Any()
                            && x.Properties.Any(
                                p =>
                                    p.PropertyName.ToUpperInvariant()
                                    == typeMeta.PersistentMetadata
                                        .IdentifierPropertyName.ToUpperInvariant()
                                    && !ids.Contains(p.Value.ToInt())));
                }

                collection.Entities.RemoveAll(x => !x.Items.Any());

                var dependencies = string.Join(", ",
                    collection.Entities.SelectMany(x => x.Dependencies.Select(y => y.FullName)).Distinct());

                if (dependencies.Any())
                {
                    _logger.Info(string.Format("Таблицы, выгружаемые помимо выбранных: {0}", dependencies));
                }

                var ser = new XmlSerializer(typeof (EntityCollection));

                using (var ms = new MemoryStream())
                {
                    using (var wr = XmlWriter.Create(ms,
                        new XmlWriterSettings()
                        {
                            NamespaceHandling = NamespaceHandling.OmitDuplicates
                        }))
                    {
                        ser.Serialize(wr, collection);
                    }

                    using (var result = new MemoryStream())
                    {
                        using (
                            var gzip = new GZipStream(result,
                                CompressionMode.Compress,
                                CompressionLevel.BestCompression,
                                true))
                        {
                            ms.Position = 0;
                            ms.CopyTo(gzip);
                        }

                        return new MemoryStream(result.ToArray());
                    }
                }
            }
            finally
            {
                if (_logger != null)
                {
                    _logger.Save();
                }
            }
        }

        /// <summary>
        /// Построение метаинформации по объектам для экспорта 
        /// </summary>
        /// <param name="list">Список сущностей для экспорта</param>
        /// <param name="types">Типы экспортируемых сущностей</param>
        /// <param name="visited">Список зависимотей текущей сущности. Нужен для определния циклических зависимостей</param>
        /// <param name="recursiveCall">Признак является ли вызов рекрсивным</param>
        private void BuildEntityHierarchy(List<Entity> list, IEnumerable<Type> types, HashSet<string> visited,
            bool recursiveCall, Dictionary<Type, EntityExportMeta> exportMeta)
        {
            foreach (var type in types)
            {
                visited = recursiveCall ? visited : new HashSet<string>();

                var typeMeta = GetClassMetaData(type) as AbstractEntityPersister;

                if (typeMeta != null)
                {
                    if (visited.Any(x => x == typeMeta.TableName))
                    {
                        var cycle = string.Join(" - ", visited);

                        var message = string.Format("Cyclic dependency exception occured for table {0}. Cycle: {1}",
                            typeMeta.TableName.ToUpper(),
                            cycle);

                        this._logger.Error(message);

                        throw new InvalidOperationException(message);
                    }

                    visited.Add(typeMeta.TableName);

                    if (list.All(x => x.FullName != typeMeta.TableName))
                    {
                        var entity = new Entity
                        {
                            FullName = typeMeta.TableName,
                            Dependencies = GetDependencies(type),
                            Items = this.GetItemsFromDb(type),
                            IsSystem = exportMeta.ContainsKey(type) && exportMeta[type].IsSystem
                        };

                        list.Add(entity);

                        var nextEntities = entity.Dependencies.Where(x => !x.IsParent)
                            .Select(x => x.Type);
                        this.BuildEntityHierarchy(list, nextEntities, visited, true, exportMeta);
                    }
                }
            }
        }

        /// <summary>
        /// Получение списка значений из БД
        /// </summary>
        /// <param name="type">Тип экспортируемой сущности</param>
        /// <returns>Список значений из БД</returns>
        /// TODO учитывать ссылки зависимых сущностей, для уменьшения количества получаемой информации из БД
        private List<Item> GetItemsFromDb(Type type)
        {
            var domainType = typeof (IDomainService<>).MakeGenericType(type);

            dynamic domain = this._container.Resolve(domainType);
            var data = Enumerable.ToList(domain.GetAll());

            var items = new List<Item>();

            var props = type.GetProperties().Where(x => !x.IsDefined(typeof (JsonIgnoreAttribute), true));
            var typeMeta = this.GetClassMetaData(type) as AbstractEntityPersister;
            foreach (var item in data)
            {
                var properties = new List<Property>();
                foreach (var propertyInfo in props)
                {
                    string columnName = string.Empty;

                    try
                    {
                        columnName = typeMeta.GetPropertyColumnNames(propertyInfo.Name).First();
                    }
                    catch (HibernateException)
                    {
                        continue;
                    }

                    var value = propertyInfo.GetValue(item, new object[0]);

                    if (value is IEntity)
                    {
                        var refMeta = this.GetClassMetaData(propertyInfo.PropertyType) as AbstractEntityPersister;
                        properties.Add(new Property
                        {
                            PropertyName = columnName,
                            Value = ((IEntity) value).Id,
                            Reference = refMeta.TableName
                        });

                        continue;
                    }

                    properties.Add(new Property
                    {
                        PropertyName = columnName,
                        Value = value
                    });
                }

                items.Add(new Item
                {
                    Properties = properties
                });
            }

            return items;
        }

        /// <summary>
        /// Получение прямых зависимостей сущности
        /// </summary>
        /// <param name="type">Тип хранимой сущности</param>
        /// <returns></returns>
        private List<Dependency> GetDependencies(Type type)
        {
            var props = type.GetProperties().Where(x => typeof (IEntity).IsAssignableFrom(x.PropertyType)).ToList();

            if (!props.Any())
            {
                return new List<Dependency>();
            }

            var list = new List<Dependency>();
            var visitedTypes = new HashSet<string>();

            this.CollectDependencies(type, props, list, visitedTypes);

            return list;
        }

        /// <summary>
        /// Сбор ссылок на зависимости
        /// </summary>
        /// <param name="type">Тип хранимой сущности</param>
        /// <param name="props">Список свойств сущности, которые явядяются ссылками на другие сущности</param>
        /// <param name="list">Список, заполняемый при обходе зависимостей</param>
        /// <param name="visitedTypes">Список посещенных зависимостей</param>
        private void CollectDependencies(Type type, IEnumerable<PropertyInfo> props, List<Dependency> list,
            HashSet<string> visitedTypes)
        {
            var topTypeMeta = GetClassMetaData(type) as AbstractEntityPersister;
            foreach (var propertyInfo in props)
            {
                if (!visitedTypes.Contains(propertyInfo.PropertyType.FullName))
                {
                    var tmpType = propertyInfo.PropertyType;

                    visitedTypes.Add(tmpType.FullName);

                    var meta = this.GetClassMetaData(tmpType) as AbstractEntityPersister;

                    if (meta != null &&
                        (topTypeMeta != null &&
                         !topTypeMeta.PropertyNullability[topTypeMeta.GetPropertyIndex(propertyInfo.Name)]))
                    {
                        list.Add(new Dependency
                        {
                            Type = tmpType,
                            FullName = meta.TableName,
                            IsParent = type == propertyInfo.PropertyType
                        });
                    }
                }
            }
        }

        #endregion Export

        #region Import

        /// <summary>
        /// Импорт данных в систему
        /// </summary>
        /// <param name="stream">Поток данных в формате GZip</param>
        public void Import(Stream stream)
        {
            Contract.Assert(stream != null, "Import stream cannot be null");

            _logger.Begin(ImportExportType.IMPORT);

            EntityCollection collection = null;
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
                {
                    try
                    {
                        gzip.CopyTo(ms);
                        ms.Position = 0;

                        var ser = new XmlSerializer(typeof (EntityCollection));

                        using (var xmlReader = XmlReader.Create(ms))
                        {
                            if (ser.CanDeserialize(xmlReader))
                            {
                                ms.Position = 0;
                                collection = ser.Deserialize(ms) as EntityCollection;
                            }
                            else
                            {
                                _logger.Error("Неверный или поврежденный файл!");
                            }
                        }
                    }
                    catch (ZlibException)
                    {
                        _logger.Error("Неверный или поврежденный файл!");
                        throw;
                    }
                    finally
                    {
                        _logger.Save();
                    }
                }
            }

            if (collection == null)
            {
                return;
            }

            var entities = collection.Entities;
            var exportMetaDict = GetExportableTypes();

            var edges = entities.ToDictionary(x => x.FullName,
                x => x.Dependencies.Where(y => !y.IsParent).Select(y => y.FullName));

            var sorted = TopologicalSort(edges).Reverse().ToList();
            var refDict = new Dictionary<string, Dictionary<int, object>>();
            try
            {
                var refValue = entities.SelectMany(x => x.Items.SelectMany(y => y.Properties.Select(z => new
                {
                    z.Reference,
                    z.Value
                })));

                var dict = refValue.Where(x => !x.Reference.IsEmpty())
                    .GroupBy(x => x.Reference)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Value.ToInt()).ToList());

                _container.UsingForResolved<IDataTransaction>((c, tr) =>
                {
                    try
                    {
                        var sortedCopy = new List<string>(sorted);
                        DeleteFromSystemTables(sortedCopy, edges, c);

                        foreach (var fullName in sorted)
                        {
                            var entity = entities.First(x => x.FullName == fullName);
                            var entityTypeMeta = GetTypeMetadata(fullName);
                            var exportMeta = exportMetaDict.ContainsKey(entityTypeMeta.Type)
                                ? exportMetaDict[entityTypeMeta.Type]
                                : null;

                            List<int> ids;
                            dict.TryGetValue(entity.FullName, out ids);

                            ids = ids ?? new List<int>();

                            var items =
                                entity.Items.Where(
                                    x =>
                                        !ids.Any()
                                        || x.Properties.Any(
                                            p =>
                                                p.PropertyName == "ID"
                                                && ids.Contains(p.Value.ToInt())))
                                    .ToList();

                            CreateItems(fullName, items, refDict);
                        }

                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        _logger.Error(e.ToString());
                        throw;
                    }
                });
            }
            finally
            {
                if (_logger != null)
                {
                    _logger.Save();
                }
            }
        }

        private void DeleteFromSystemTables(List<string> sorted, Dictionary<string, IEnumerable<string>> edges, IWindsorContainer container)
        {
            var exportMetaDict = GetExportableTypes();
            sorted.Reverse();

            var types = sorted.Select(GetTypeMetadata);

            var systemTypes =
                types.Where(x => exportMetaDict.ContainsKey(x.Type) && exportMetaDict[x.Type].IsSystem)
                    .Select(
                        x =>
                            new
                            {
                                x.Type,
                                Dependencies =
                                    edges.ContainsKey(x.TableName)
                                        ? edges[x.TableName].Select(y => GetTypeMetadata(y).Type)
                                        : new List<Type>()
                            }).ToList();

            var dependencies = systemTypes.SelectMany(x => x.Dependencies).Distinct();

            foreach (var systemType in systemTypes)
            {
                DeleteFromSystemTable(container, systemType.Type);
            }

            foreach (var dependency in dependencies)
            {
                DeleteFromSystemTable(container, dependency);
            }
        }

        private static void DeleteFromSystemTable(IWindsorContainer container, Type systemType)
        {
            /* По идее можно через сессию срубить, благо моно получить метаинформацию таблицы */
            var domainType = typeof (IDomainService<>).MakeGenericType(systemType);
            dynamic domain = container.Resolve(domainType);

            foreach (var item in Enumerable.ToList(domain.GetAll()))
            {
                domain.Delete(item.Id);
            }
        }

        /// <summary>
        /// Создание сущностей на основе метаинформации
        /// </summary>
        /// <param name="fullName">Наименование таблицы в БД</param>
        /// <param name="items">Список значений сущностей</param>
        /// <param name="refDict">Словарь ссылок на внешние таблицы, на которые ссылаются импортируемые таблицы</param>
        private void CreateItems(string fullName, List<Item> items, Dictionary<string, Dictionary<int, object>> refDict)
        {
            var entityTypeMeta = GetTypeMetadata(fullName);

            var domainType = typeof (IDomainService<>).MakeGenericType(entityTypeMeta.Type);
            var domain = this._container.Resolve(domainType);
            var saveMethod = domainType.GetMethod("Save", new[] {entityTypeMeta.Type});

            dynamic dynamicDomain = domain;
            if (Enumerable.Any(dynamicDomain.GetAll()))
            {
                var message = string.Format("Target table {0} already contains data! Terminating import.",
                    fullName.ToUpper());

                this._logger.Error(message);

                throw new InvalidOperationException(message);
            }

            foreach (var item in items)
            {
                var entity = Activator.CreateInstance(entityTypeMeta.Type);
                var idProperty =
                    item.Properties.FirstOrDefault(
                        x =>
                            x.PropertyName.ToUpperInvariant()
                            == entityTypeMeta.PersistentMetadata.IdentifierPropertyName
                                .ToUpperInvariant());

                /*
                 * Возьмем поля, кроме ID
                 */
                foreach (
                    var property in
                        item.Properties.Where(
                            x => x.PropertyName.ToUpperInvariant() != entityTypeMeta.PersistentMetadata.IdentifierPropertyName.ToUpperInvariant()).ToList())
                {
                    if (!entityTypeMeta.Columns.ContainsKey(property.PropertyName))
                    {
                        continue;
                    }

                    var propertyInfo = entityTypeMeta.Columns[property.PropertyName];

                    /*
                     * Если есть ссылка на другую сущность,
                     * то она уже была обработана 
                     * и должна лежать в словаре ссылок под своим старым id
                     */
                    if (!property.Reference.IsEmpty())
                    {
                        if (refDict.ContainsKey(property.Reference))
                        {
                            var persistentDict = refDict[property.Reference];

                            var refId = property.Value.ToInt();
                            if (persistentDict.ContainsKey(refId))
                            {
                                propertyInfo.SetValue(entity, persistentDict[refId], new object[0]);
                            }
                        }

                        continue;
                    }

                    propertyInfo.SetValue(entity, property.Value, new object[0]);
                }

                /*
                 * Добавим в словарь ссылок текущую сущность
                 * с ее старым id
                 */
                if (idProperty != null)
                {
                    if (refDict.ContainsKey(fullName))
                    {
                        if (refDict[fullName].ContainsKey(idProperty.Value.ToInt()))
                        {
                            refDict[fullName][idProperty.Value.ToInt()] = entity;
                        }
                        else
                        {
                            refDict[fullName].Add(idProperty.Value.ToInt(), entity);
                        }
                    }
                    else
                    {
                        refDict.Add(fullName,
                            new Dictionary<int, object>()
                            {
                                {idProperty.Value.ToInt(), entity}
                            });
                    }
                }

                if (saveMethod != null)
                {
                    saveMethod.Invoke(domain, new[] {entity});
                }
            }
        }

        #endregion Import

        /// <summary>
        /// Получение метаинформации NHibernate
        /// </summary>
        /// <param name="type">Тип хранимой сущности</param>
        /// <returns>NHibernate IClassMetadata</returns>
        private IClassMetadata GetClassMetaData(Type type)
        {
            return GetTypeMetadata(type).PersistentMetadata;
        }

        /// <summary>
        /// Получение метаинформации для типа
        /// </summary>
        /// <param name="tableName">Название таблицы в БД</param>
        /// <returns>TypeMetadata</returns>
        private TypeMetadata GetTypeMetadata(string tableName)
        {
            if (TypeMetadatas.Any(x => x.TableName == tableName))
            {
                return TypeMetadatas.First(x => x.TableName == tableName);
            }

            var persister =
                SessionProvider.GetCurrentSession()
                    .SessionFactory.GetAllClassMetadata()
                    .FirstOrDefault(x => ((AbstractEntityPersister) x.Value).TableName == tableName)
                    .Return(x => x.Value);

            var type = persister.MappedClass;

            return GetTypeMetadata(type);
        }

        /// <summary>
        /// Получение метаинформации для типа
        /// </summary>
        /// <param name="type">Тип хранимой сущности</param>
        /// <returns>TypeMetadata</returns>
        private TypeMetadata GetTypeMetadata(Type type)
        {
            if (TypeMetadatas.Any(x => x.Type == type))
            {
                return TypeMetadatas.First(x => x.Type == type);
            }
            else
            {
                var typeMeta =
                    SessionProvider.GetCurrentSession().SessionFactory.GetClassMetadata(type) as AbstractEntityPersister;
                var props = type.GetProperties().Where(x => !x.IsDefined(typeof (JsonIgnoreAttribute), true));

                var meta = new TypeMetadata
                {
                    Type = type,
                    PersistentMetadata = typeMeta,
                    TableName = typeMeta.TableName
                };

                foreach (var propertyInfo in props)
                {
                    string columnName = null;
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

                TypeMetadatas.Add(meta);

                return meta;
            }
        }

        /// <summary>
        /// Получение типов экспортируемых сущностей
        /// </summary>
        /// <returns></returns>
        private Dictionary<Type, EntityExportMeta> GetExportableTypes()
        {
            var exportContainer = new EntityExportContainer();
            this._container.ResolveAll<IEntityExportProvider>().ForEach(x => x.FillContainer(exportContainer));

            return exportContainer.Container;
        }

        private static IList<string> TopologicalSort(IDictionary<string, IEnumerable<string>> edges)
        {
            List<string> result = new List<string>();
            Dictionary<string, int> inWeight = edges.Keys.ToDictionary(x => x, x => 0);
            foreach (KeyValuePair<string, IEnumerable<string>> edge in edges)
            {
                foreach (string inEdge in edge.Value)
                {
                    inWeight[inEdge] = inWeight[inEdge] + 1;
                }
            }
            int visitedCount = 0;
            Queue<string> queue = new Queue<string>(inWeight.Where(x => x.Value == 0).Select(x => x.Key));
            while (queue.Count > 0)
            {
                string vertex = queue.Dequeue();
                result.Add(vertex);
                ++visitedCount;
                foreach (string inEdge in edges[vertex])
                {
                    inWeight[inEdge] = inWeight[inEdge] - 1;
                    if (inWeight[inEdge] == 0)
                    {
                        queue.Enqueue(inEdge);
                    }
                }
            }
            if (visitedCount < edges.Count)
            {
                throw new InvalidOperationException("Граф является циклическим");
            }
            return result;
        }
    }

    public class EntityCollection
    {
        [XmlElement(ElementName = "e")]
        public List<Entity> Entities { get; set; }
    }

    [Serializable]
    public class Entity
    {
        [XmlAttribute(AttributeName = "fn")]
        public string FullName { get; set; }

        [XmlAttribute(AttributeName = "is")]
        public bool IsSystem { get; set; }

        [XmlElement(ElementName = "d")]
        public List<Dependency> Dependencies { get; set; }

        [XmlElement(ElementName = "i")]
        public List<Item> Items { get; set; }

        public override string ToString()
        {
            return string.Format("FullName: {0}, DepCount: {1}, ItemsCount: {2}",
                FullName,
                Dependencies.Return(x => x.Count()),
                Items.Return(x => x.Count()));
        }
    }

    [Serializable]
    public class Item
    {
        [XmlElement(ElementName = "p")]
        public List<Property> Properties { get; set; }
    }

    [Serializable]
    public class Property
    {
        [XmlAttribute(AttributeName = "pn")]
        public string PropertyName { get; set; }

        [XmlElement(ElementName = "v")]
        public object Value { get; set; }

        [XmlAttribute(AttributeName = "r")]
        public string Reference { get; set; }

        public override string ToString()
        {
            return string.Format("Property: {0}, Value: {1}, Reference: {2}",
                PropertyName,
                Value.Return(x => x.ToString()),
                Reference);
        }
    }

    [Serializable]
    public class Dependency
    {
        [XmlIgnore]
        public Type Type { get; set; }

        [XmlAttribute(AttributeName = "dfn")]
        public string FullName { get; set; }

        [XmlAttribute(AttributeName = "ip")]
        public bool IsParent { get; set; }

        public override string ToString()
        {
            return string.Format("FullName: {0}, Type: {1}, IsParent: {2}", FullName, Type.Name, IsParent);
        }
    }

    public class TypeMetadata
    {
        public Type Type { get; set; }

        public IClassMetadata PersistentMetadata { get; set; }

        public string TableName { get; set; }

        public Dictionary<string, PropertyInfo> Columns { get; set; }

        public TypeMetadata()
        {
            Columns = new Dictionary<string, PropertyInfo>();
        }
    }
}