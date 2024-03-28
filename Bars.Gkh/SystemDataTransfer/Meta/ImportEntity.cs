namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.SystemDataTransfer.Caching;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;
    using Bars.Gkh.SystemDataTransfer.Utils;

    using Fasterflect;

    using Newtonsoft.Json.Linq;

    using NHibernate;

    /// <summary>
    /// Generic-реализация импортируемой сущности
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class ImportEntity<TEntity> : IImportEntity where TEntity : class, IEntity
    {
        private readonly TransferEntityMeta<TEntity> meta;
        private readonly FileProcessingHelper fileProcessingHelper;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="meta">Мета описание</param>
        /// <param name="fileProcessingHelper">Помощник для работы с файлами</param>
        public ImportEntity(TransferEntityMeta<TEntity> meta, FileProcessingHelper fileProcessingHelper)
        {
            this.meta = meta;
            this.fileProcessingHelper = fileProcessingHelper;
        }

        /// <inheritdoc />
        public void Init(IDataTransferCache cache)
        {
            cache.RegisterEntityCacheMap<TEntity>(this.meta);
        }

        /// <inheritdoc />
        public void AddDependency(IDataTransferCache cache, string propertyName, ITransferEntityMeta depencyMeta)
        {
            cache.RegisterDependecy<TEntity>(propertyName, depencyMeta);
        }

        /// <inheritdoc />
        public void AddInherit(IDataTransferCache cache, ITransferEntityMeta inheritMeta)
        {
            cache.AddInheritance<TEntity>(inheritMeta);
        }

        /// <inheritdoc />
        public void ImportSection(IDataTransferCache cache, Entity entity, IEnumerable<Item> items)
        {
            var sessionProvider = ApplicationContext.Current.Container.Resolve<ISessionProvider>();

            try
            {
                using (sessionProvider)
                {
                    var session = sessionProvider.OpenStatelessSession();
                    IntegrationSessionAccessor.Session = session;
                    session.SetBatchSize(1000);

                    using (var transaction = session.BeginTransaction())
                    {
                        // сущности, которые "отложены" на конец импорта
                        // пример необходимости: Контрагенты, которые могут иметь ссылку на ParentContragent,
                        // который ещё не загружен
                        var deferredEntities = new List<Item>();

                        foreach (var section in items.Section(10000))
                        {

                            foreach (var item in section)
                            {
                                this.ProcessItem(cache, item, deferredEntities, session);
                            }
                        }

                        while (deferredEntities.Count > 0)
                        {
                            foreach (var section in deferredEntities.Section(10000))
                            {
                                foreach (var item in section)
                                {
                                    // удаляем из списка, пытаемся импортировать
                                    deferredEntities.Remove(item);
                                    this.ProcessItem(cache, item, deferredEntities, session);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
            finally
            {
                IntegrationSessionAccessor.Session = null;
            }
            
        }

        private void ProcessItem(IDataTransferCache cache, Item item, List<Item> deferredEntities, IStatelessSession session)
        {
            object entityId;

            if (!cache.TryGetId<TEntity>(item.Properties, out entityId))
            {
                entityId = 0L;
            }

            var saveParams = new DynamicDictionary();
            saveParams.Apply(item.Properties);
            saveParams["Id"] = entityId;

            if (typeof(TEntity).Is<IImportableEntity>())
            {
                saveParams["ImportEntityId"] = item.Id;
            }

            var saveEntity = (TEntity)Activator.CreateInstance(typeof(TEntity), true);

            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (saveParams.ContainsKey(property.Name) && property.CanWrite)
                {
                    var value = saveParams[property.Name];

                    if (value.IsNotNull())
                    {
                        if (property.PropertyType.Is<IEntity>())
                        {
                            object subEntityId;
                            if (!cache.TryGetDependencyId(property.PropertyType, value, out subEntityId))
                            {
                                if (property.PropertyType == typeof(TEntity))
                                {
                                    deferredEntities.Add(item);
                                    continue;
                                }

                                throw new ValidationException("Не удалось сопоставить сущность по внешнему идентификатору");
                            }

                            value = Activator.CreateInstance(property.PropertyType, true);
                            ((IEntity)value).Id = subEntityId;
                        }
                        else if (value.Is<JToken>())
                        {
                            value = ((JToken)value).ToObject(property.PropertyType);
                        }
                        else if (property.PropertyType != value.GetType())
                        {
                            value = ConvertHelper.ConvertTo(value, property.PropertyType);
                        }
                    }

                    if (value.IsNull() && property.PropertyType.IsValueType)
                    {
                        value = Activator.CreateInstance(property.PropertyType);
                    }

                    saveEntity.SetPropertyValue(property.Name, value);
                }
            }

            // если сущность новая, то её необходимо добавить в кэш после сохранения
            var addToCache = saveEntity.Id.Equals(0L);

            if (this.meta.HasCustomSerializer)
            {
                var fileName = this.meta.Serializer.GetFileName(item.Id);
                var memoryStream = fileName.IsNotEmpty()
                    ? new MemoryStream(this.fileProcessingHelper.GetFile(fileName))
                    : new MemoryStream();

                using (memoryStream)
                {
                    saveEntity = (TEntity)this.meta.Serializer.Deserializer(saveEntity, item, memoryStream);
                }
            }
            else
            {
                this.SaveOrUpdate(session, saveEntity);
            }

            if (addToCache)
            {
                cache.AddEntity(saveEntity, item.Properties);
            }
        }

        private void SaveOrUpdate(IStatelessSession session, IEntity entity)
        {
            var persistentObject = entity as PersistentObject;
            if (persistentObject == null)
            {
                return;
            }

            var baseEntity = persistentObject as BaseEntity;
            if (baseEntity != null)
            {
                baseEntity.ObjectEditDate = DateTime.Now;
            }

            if (persistentObject.Id == 0)
            {
                if (baseEntity != null)
                {
                    baseEntity.ObjectCreateDate = DateTime.Now;
                }

                session.Insert(persistentObject);
            }
            else
            {
                if (baseEntity != null)
                {
                    baseEntity.ObjectVersion += 1;
                }

                session.Update(persistentObject);
            }
        }
    }

    /// <summary>
    /// Импортируемая сущность
    /// </summary>
    public interface IImportEntity
    {
        /// <summary>
        /// Инициализировать кэш для импортируемой сущности
        /// </summary>
        /// <param name="cache">Кэш импорта</param>
        void Init(IDataTransferCache cache);

        /// <summary>
        /// Добавить зависимую связь
        /// </summary>
        /// <param name="cache">Кэш</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="depencyMeta">Мета-описание зависимой сущности</param>
        void AddDependency(IDataTransferCache cache, string propertyName, ITransferEntityMeta depencyMeta);

        /// <summary>
        /// Импортировать секцию
        /// </summary>
        /// <param name="cache">Кэш</param>
        /// <param name="session">Сессия</param>
        /// <param name="entity">Сущность</param>
        /// <param name="items">Значения сущности</param>
        /// <returns></returns>
        void ImportSection(IDataTransferCache cache, Entity entity, IEnumerable<Item> items);

        /// <summary>
        /// Добавить наследника
        /// </summary>
        void AddInherit(IDataTransferCache cache, ITransferEntityMeta inheritMeta);
    }
}