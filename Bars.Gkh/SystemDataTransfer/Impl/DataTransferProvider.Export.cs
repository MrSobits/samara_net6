namespace Bars.Gkh.SystemDataTransfer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.SystemDataTransfer.Meta;
    
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.SystemDataTransfer.Meta.JsonConverters;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;
    using Bars.Gkh.SystemDataTransfer.Utils;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using NHibernate;
    using NHibernate.Persister.Entity;
    using NHibernate.Proxy;
    
    using IQueryable = System.Linq.IQueryable;

    /// <summary>
    /// Провайдер переноса данных
    /// </summary>
    /// <remarks>
    /// Чтобы не было большого объема данных, делим интеграции над подынтеграции
    /// Строим дерево зависимостей
    /// На вершине у нас будут сущности, у которых больше всего зависимостей
    /// Внизу у нас самые простые сущности - справочники
    /// Тем самым мы грузим слоями снизу вверх
    /// Ещё стоит вопрос о количестве секций, которые мы отправляем одновременно, потому что на принимающей стороне нужно сократить объем кэшируемых данных,
    ///   но при этом не потерять в скорости интеграции
    /// </remarks>
    public partial class DataTransferProvider
    {
        public ILogger LogManager { get; set; }

        /// <inheritdoc />
        public Stream Export(IEnumerable<string> typeNames = null, bool exportDependencies = true)
        {
            var types = this.GetExportableTypes();
            typeNames = typeNames ?? new List<string>();

            var exportTypes = types.Values
                .Where(x => !x.IsBase)
                .Select(x => x.GetTypeMetadata())
                .WhereIf(typeNames.IsNotEmpty(), x => typeNames.Contains(x.Type.FullName))
                .Select(x => x.Type)
                .ToList();

            var list = new List<Entity>();

            this.BuildEntityHierarchy(list, exportTypes, types, null, false, exportDependencies);

            var collection = new MetaDescription
            {
                Entities = list
            };

            try
            {
                this.SortTypes(exportDependencies, collection.Entities);
            }
            catch (InvalidOperationException exception)
            {
                throw new ValidationException("Обнаружены циклические ссылки", exception);
            }

            var items = this.FillHierarchy(collection, types);

            var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var zipEntity = zipArchive.CreateEntry("_META.json");
                using (var sw = new StreamWriter(zipEntity.Open()))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(collection.Entities, Formatting.None, new TypeJsonConverter()));
                }

                foreach (var entity in collection.Entities)
                {
                    var section = items.Get(entity);
                    zipEntity = zipArchive.CreateEntry($"{entity.FullName}.json");

                    try
                    {
                        using (var stream = zipEntity.Open())
                        using (var fs = section.Data.Open(FileMode.Open))
                        {
                            fs.CopyTo(stream);
                        }
                    }
                    finally
                    {
                        section.Data.Delete();
                    }
                    

                    var meta = types.Get(entity.Type);

                    if (meta.IsNotNull() && meta.HasCustomSerializer && section.IsNotNull())
                    {
                        foreach (var item in section.Ids)
                        {
                            var fileName = meta.Serializer.GetFileName(item);
                            if (fileName.IsNotEmpty())
                            {
                                zipEntity = zipArchive.CreateEntry($"{entity.FullName}/{meta.Serializer.GetFileName(item)}");
                                using (var stream = zipEntity.Open())
                                {
                                    meta.Serializer.Flush(item, stream);
                                }
                            }
                        }
                    }
                }
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        private IDictionary<Entity, ExportData> FillHierarchy(
            MetaDescription collection, 
            IDictionary<Type, ITransferEntityMeta> typeInfoDict)
        {
            var entitiesToExport = collection.Entities.ToList();
            var itemsToExport = this.GetDataFromDb(typeInfoDict, entitiesToExport);

            return itemsToExport;
        }

        private Dictionary<Entity, ExportData> GetDataFromDb(
            IDictionary<Type, ITransferEntityMeta> typeInfoDict,
            IList<Entity> entitiesToExport)
        {
            // сначала обрабатываем сущности, которая ни от кого не зависит
            var processedEntities = new List<Entity>();
            var result = new Dictionary<Entity, ExportData>();

            try
            {
                foreach (var entity in entitiesToExport)
                {
#if DEBUG
                    this.LogManager.LogInformation($"Старт выгрузки секции {entity.Type.FullName}({processedEntities.Count}/{entitiesToExport.Count})");
                    var sw = Stopwatch.StartNew();
#endif
                    var items = this.GetItemsFromDb(typeInfoDict.Get(entity.Type));
#if DEBUG
                    this.LogManager.LogInformation($"Секция выгружена за {sw.Elapsed}");
#endif

                    processedEntities.Add(entity);
                    result.Add(entity, items);
                }
            }
            catch
            {
                // если упал экспорт из бд, то чистим за собой
                foreach (var item in result.Values)
                {
                    if (item.Data.Exists)
                    {
                        item.Data.Delete();
                    }
                }
            }
            

            return result;
        }

        /// <summary>
        /// Построение метаинформации по объектам для экспорта 
        /// </summary>
        /// <param name="list">Список сущностей для экспорта</param>
        /// <param name="types">Типы экспортируемых сущностей</param>
        /// <param name="transferEntityMetas"></param>
        /// <param name="visited">Список зависимотей текущей сущности. Нужен для определния циклических зависимостей</param>
        /// <param name="recursiveCall">Признак является ли вызов рекрсивным</param>
        /// <param name="exportDependencies"></param>
        private void BuildEntityHierarchy(
            List<Entity> list,
            IEnumerable<Type> types,
            IDictionary<Type, ITransferEntityMeta> transferEntityMetas,
            HashSet<string> visited,
            bool recursiveCall, 
            bool exportDependencies)
        {
            foreach (var type in types)
            {
                visited = recursiveCall ? visited : new HashSet<string>();

                var transferMeta = transferEntityMetas.Get(type);
                var typeMeta = transferMeta?.GetClassMetaData() as AbstractEntityPersister;

                if (typeMeta != null)
                {
                    if (visited.Any(x => x == typeMeta.TableName))
                    {
                        // мы уже обошли эту сущность
                        continue;
                    }

                    visited.Add(typeMeta.TableName);

                    if (list.All(x => x.FullName != typeMeta.TableName))
                    {
                        var entity = new Entity
                        {
                            FullName = typeMeta.TableName,
                            Dependencies = transferMeta.GetDependencies(),
                            Type = type
                        };

                        list.Add(entity);

                        if (exportDependencies)
                        {
                            var rootTypes = entity.Dependencies.Where(x => x.IsRoot).Select(x => x.Type).SelectMany(x => transferEntityMetas.Values.Where(y => y.BaseType == x)).Select(x => x.Type);
                            this.BuildEntityHierarchy(list, rootTypes, transferEntityMetas, visited, true, exportDependencies);

                            var nextEntities = entity.Dependencies.Where(x => !x.IsParent && !x.IsRoot).Select(x => x.Type);
                            this.BuildEntityHierarchy(list, nextEntities, transferEntityMetas, visited, true, exportDependencies);
                        }
                    }
                }
            }

            if (!recursiveCall)
            {
                this.ValidateEntities(list, this.GetExportableTypes());
            }
        }

        /// <summary>
        /// Получение списка значений из БД
        /// </summary>
        private ExportData GetItemsFromDb(ITransferEntityMeta meta)
        {
            var items = new ExportData(this.GetEnititiesCount(meta.Type));
            PropertyInfo[] props = null;

            var typeMetaData = meta.GetTypeMetadata();
            var fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            using (var fileStream = new FileStream(fileName, FileMode.Create))
            using (var textWriter = new StreamWriter(fileStream))
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                var stateless = this.SessionProvider.OpenStatelessSession();
                try
                {
                    IntegrationSessionAccessor.Session = stateless;

                    jsonWriter.WriteStartArray();
                    bool first = true;
                    foreach (var item in this.GetEnitities(stateless, typeMetaData.Type, meta.Serializer?.QueryModifier(), meta.Filter))
                    {
                        var entityData = item;

                        if (meta.IsNull() || props.IsNull() && !meta.HasCustomSerializer)
                        {
                            props = this.GetProperties(typeMetaData.Type, meta);
                        }

                        if (meta.IsNotNull() && meta.HasCustomSerializer)
                        {
                            entityData = meta.Serialize(item);

                            //состояние неверное, пропускаем сущность
                            if (entityData.IsNull())
                            {
                                continue;
                            }

                            // могли вернуть сразу сериализованный объект, здорово
                            if (entityData.Is<Item>())
                            {
                                if (!first)
                                {
                                    jsonWriter.WriteRaw(",");
                                }

                                items.Ids.Add(((Item)entityData).Id);
                                jsonWriter.WriteRaw(JsonConvert.SerializeObject(entityData));
                                first = false;

                                continue;
                            }

                            if (props.IsNull())
                            {
                                var type = entityData.GetType();
                                if (type.Is<INHibernateProxy>())
                                {
                                    type = type.BaseType;
                                }

                                props = this.GetProperties(type, meta);
                            }
                        }

                        var properties = new Dictionary<string, object>();
                        foreach (var propertyInfo in props)
                        {
                            var value = propertyInfo.GetValue(entityData, new object[0]);
                            properties[propertyInfo.Name] = (value as IEntity)?.Id ?? value;
                        }

                        entityData = new Item
                        {
                            Properties = properties
                        };

                        if (!first)
                        {
                            jsonWriter.WriteRaw(",");
                        }

                        items.Ids.Add(((Item)entityData).Id);
                        jsonWriter.WriteRaw(JsonConvert.SerializeObject(entityData));
                        first = false;
                    }

                    jsonWriter.WriteEndArray();
                    jsonWriter.Flush();

                    // скидываем данные
                    items.SetStream(fileStream);
                }
                finally
                {
                    IntegrationSessionAccessor.Session = null;
                    stateless?.Dispose();
                }
            }

            return items;
        }

        private IEnumerable GetEnitities(
            IStatelessSession statelessSession,
            Type type, 
            Func<IQueryable, IQueryable> queryModifier, 
            Func<IQueryable<IEntity>, IQueryable<IEntity>> filter)
        {
            var repoType = typeof(StatelessNhRepository<>).MakeGenericType(type);
            var repository = Activator.CreateInstance(repoType, statelessSession);

            var method = this.GetType().GetMethod("GetQuery", BindingFlags.Static | BindingFlags.NonPublic);
            var genericMethod = method.MakeGenericMethod(type);

            return (IEnumerable)genericMethod.Invoke(null, new[] { repository, queryModifier, filter });
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable GetQuery<T>(
            IRepository<T> repository, 
            Func<IQueryable, IQueryable> queryModifier,
            Func<IQueryable<IEntity>, IQueryable<IEntity>> filter) 
            where T : IEntity
        {
            var uniqueIds = filter((IQueryable<IEntity>)repository.GetAll()).Select(x => x.Id).ToArray();

            if (uniqueIds.Length > 200000)
            {
                foreach (var ids in uniqueIds.Section(5000))
                {
                    IQueryable query = repository.GetAll().Where(x => ids.Contains(x.Id));

                    if (queryModifier.IsNotNull())
                    {
                        query = queryModifier(query) ?? query;
                    }

                    foreach (var item in query)
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                IQueryable query = filter((IQueryable<IEntity>)repository.GetAll());

                if (queryModifier.IsNotNull())
                {
                    query = queryModifier(query) ?? query;
                }

                foreach (var item in query)
                {
                    yield return item;
                }
            }
            
        }

        private int GetEnititiesCount(Type type)
        {
            var repoType = typeof(IRepository<>).MakeGenericType(type);
            var repository = (IRepository)this.Container.Resolve(repoType);

            using (this.Container.Using(repository))
            {
                var method = this.GetType().GetMethod("GetCount", BindingFlags.Static | BindingFlags.NonPublic);
                var genericMethod = method.MakeGenericMethod(type);

                return (int)genericMethod.Invoke(null, new object[] { repository });
            }
        }

        // ReSharper disable once UnusedMember.Local
        private static int GetCount<T>(IRepository<T> repository) where T : IEntity
        {
            return repository.GetAll().Count();
        }


    }
}