namespace Bars.Gkh.SystemDataTransfer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Caching;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.SystemDataTransfer.Meta.JsonConverters;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public partial class DataTransferProvider
    {
        private FileProcessingHelper fileProcessingHelper;

        public IDataTransferCache DataTransferCache { get; set; }

        /// <inheritdoc />
        public event Action<string, bool> OnSectionImportDone = (message, success) => { };

        /// <inheritdoc />
        public void Import(Stream stream)
        {
            using (this.DataTransferCache)
            using (this.fileProcessingHelper = new FileProcessingHelper())
            {
                this.ImportInternal(stream);
            }
        }

        private void ImportInternal(Stream stream)
        {
            var types = this.GetExportableTypes();

            try
            {
                // считываем архив и складываем данные во временную папку
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    // сначала обрабатываем данные по таблицам
                    var meta = zipArchive.Entries.FirstOrDefault(x => x.Name.ToLower() == "_meta.json");
                    if (meta.IsNull())
                    {
                        throw new ValidationException("Отсутствует заголовочный файл");
                    }

                    var entityCollection = new MetaDescription
                    {
                        Entities = this.DeserializeJson<List<Entity>>(meta)
                    };

                    if (entityCollection.Entities.IsEmpty())
                    {
                        throw new ValidationException("Заголовочный файл пуст");
                    }

                    // получаем реализации импортов сущностей и готовим кэш к прогреву
                    var importEntities = this.PrepareEntities(entityCollection, types);

                    // прогреваем кэш
                    this.DataTransferCache.WarmCache();
                    
                    var edges = entityCollection.Entities.ToDictionary(x => x.Type.FullName,
                        x => x.Dependencies
                        .Where(y => !y.IsParent)
                        .Select(y => y.Type.FullName));

                    // когда грузим без зависимостей, то надо дополнить зависимыми
                    foreach (var type in importEntities.Keys.Where(x => !edges.Keys.Contains(x.FullName)))
                    {
                        edges[type.FullName] = new List<string>();
                    }

                    // сортируем сущности для правильного порядка импорта
                    var sorted = DataTransferProvider.TopologicalSort(edges).Keys.Reverse().ToList();

                    foreach (var entityTypeName in sorted)
                    {
                        var entity = entityCollection.Entities.FirstOrDefault(x => x.Type.FullName == entityTypeName);
                        if (entity.IsNull())
                        {
                            continue;
                        }

                        try
                        {
                            var zipEntry = zipArchive.GetEntry($"{entity.FullName}.json");
                            if (zipEntry.IsNull())
                            {
                                throw new ValidationException($"Отсутствует файл {entity.FullName}.json, однако он указан в заголовочном файле");
                            }

                            var items = this.DeserializeJsonLazy<Item>(zipEntry);

                            // если у секции есть файлы, то складываем их в отдельную папку, обработаем их позже
                            // ReSharper disable once AccessToDisposedClosure
                            foreach (var fileEntry in zipArchive.Entries.Where(x => x.FullName.StartsWith($"{entity.FullName}/")))
                            {
                                using (var fileStream = fileEntry.Open())
                                {
                                    this.fileProcessingHelper.AddFile(fileEntry.Name, fileStream);
                                }
                            }

                            var importer = importEntities.Get(entity.Type);
                            importer.ImportSection(this.DataTransferCache, entity, items);

                            this.OnSectionImportDone?.Invoke(entityTypeName, true);
                        }
                        catch
                        {
                            this.OnSectionImportDone?.Invoke(entityTypeName, false);
                            throw;
                        }
                    }
                }
            }
            finally
            {
                types.ForEach(x => x.Value.Dispose());
            }
        }

        private Dictionary<Type, IImportEntity> PrepareEntities(MetaDescription entityCollection, IDictionary<Type, ITransferEntityMeta> types)
        {
            var importEntities = new Dictionary<Type, IImportEntity>();
            foreach (var entityCollectionEntity in entityCollection.Entities)
            {
                var typeMeta = types.Get(entityCollectionEntity.Type);
                if (typeMeta.IsNull())
                {
                    throw new ValidationException($"Отсутствует мета-описание для типа {entityCollectionEntity.Type}");
                }

                importEntities[entityCollectionEntity.Type] = typeMeta.GetImportMeta(this.fileProcessingHelper);
                importEntities[entityCollectionEntity.Type].Init(this.DataTransferCache);

                if (typeMeta.BaseType.IsNotNull())
                {
                    IImportEntity baseImportEntity;

                    var baseMeta = types.Get(typeMeta.BaseType);
                    if (baseMeta.IsNull())
                    {
                        throw new ValidationException($"Отсутствует мета-описание для типа {entityCollectionEntity.Type}");
                    }

                    if (!importEntities.TryGetValue(typeMeta.BaseType, out baseImportEntity))
                    {
                        baseImportEntity = baseMeta.GetImportMeta(this.fileProcessingHelper);
                        importEntities[typeMeta.BaseType] = baseImportEntity;
                        baseImportEntity.Init(this.DataTransferCache);
                    }

                    baseImportEntity.AddInherit(this.DataTransferCache, typeMeta);
                }
            }

            // потом их зависимости
            foreach (var entityCollectionEntity in entityCollection.Entities)
            {
                var importEntity = importEntities.Get(entityCollectionEntity.Type);

                if (importEntity.IsNotNull())
                {
                    foreach (var dependency in entityCollectionEntity.Dependencies)
                    {
                        var typeMeta = types.Get(dependency.Type);
                        if (typeMeta.IsNull())
                        {
                            throw new ValidationException(
                                $"Отсутствует мета-описание для зависимости {entityCollectionEntity.Type} объекта {entityCollectionEntity.Type}");
                        }

                        if (!importEntities.ContainsKey(dependency.Type))
                        {
                            importEntities[dependency.Type] = typeMeta.GetImportMeta(this.fileProcessingHelper);
                            importEntities[dependency.Type].Init(this.DataTransferCache);
                        }

                        importEntity.AddDependency(this.DataTransferCache, dependency.Name, typeMeta);
                    }
                }
            }
            return importEntities;
        }

        private T DeserializeJson<T>(ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
			using (var streamReader = new StreamReader(stream))
			{
			    return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd(), new TypeJsonConverter());
			}
        }

        /// <summary>
        /// Отложенное чтение JSON - считывает элементы массива по 1 и десериализует в указанный тип
        /// </summary>
        private IEnumerable<T> DeserializeJsonLazy<T>(ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
			using (var streamReader = new StreamReader(stream))
			using (var jsonReader = new JsonTextReader(streamReader))
			{
			    while (jsonReader.Read())
			    {
			        if (jsonReader.TokenType == JsonToken.StartObject)
			        {
			            yield return JObject.Load(jsonReader).ToObject<T>();
			        }
                }
			}
        }

		/// <summary>
        /// Сортировка сущностей по их весу. Необходимо для правильного импорта данных
        /// </summary>
        private static IDictionary<string, int> TopologicalSort(IDictionary<string, IEnumerable<string>> edges)
        {
            var result = new Dictionary<string, int>();
            var inWeight = edges.Keys.ToDictionary(x => x, x => 0);
            foreach (KeyValuePair<string, IEnumerable<string>> edge in edges)
            {
                foreach (string inEdge in edge.Value)
                {
                    inWeight[inEdge] = inWeight[inEdge] + 1;
                }
            }
            int visitedCount = 0;

            var queue = new Queue<Tuple<string, int>>(inWeight.Where(x => x.Value == 0).Select(x => Tuple.Create(x.Key, 0)));
            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                result.Add(vertex.Item1, vertex.Item2);
                ++visitedCount;

                foreach (var inEdge in edges[vertex.Item1])
                {
                    inWeight[inEdge] = inWeight[inEdge] - 1;
                    if (inWeight[inEdge] == 0)
                    {
                        queue.Enqueue(Tuple.Create(inEdge, vertex.Item2 + 1));
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
}