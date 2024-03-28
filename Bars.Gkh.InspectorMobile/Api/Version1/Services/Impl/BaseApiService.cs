namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Base;

    using Castle.Windsor;

    using Newtonsoft.Json;

    using NHibernate.Linq;

    /// <summary>
    /// Базовый класс API-сервиса
    /// </summary>
    public class BaseApiService<TServiceEntity, TCreateModel, TUpdateModel> : IBaseApiService<TCreateModel, TUpdateModel>
        where TServiceEntity : PersistentObject
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Порядок обработки
        /// </summary>
        /// <remarks>
        /// Используется в качестве основы при определении порядка обработки сущности
        /// </remarks>
        protected readonly int MainProcessOrder = 100;

        /// <summary>
        /// Предыдущий порядок обработки 
        /// </summary>
        protected readonly int PreviousOrder;

        /// <summary>
        /// Следующий порядок обработки 
        /// </summary>
        protected readonly int NextOrder;

        /// <summary>
        /// Перечень сущностей, которые необходимо сохранить
        /// </summary>
        private readonly Dictionary<DictProcessKey, List<object>> SavingEntities = new Dictionary<DictProcessKey, List<object>>();

        /// <summary>
        /// Перечень сущностей, которые необходимо обновить
        /// </summary>
        private readonly Dictionary<DictProcessKey, List<object>> UpdatingEntities = new Dictionary<DictProcessKey, List<object>>();

        /// <summary>
        /// Перечень сущностей, которые необходимо удалить
        /// </summary>
        private readonly Dictionary<DictProcessKey, List<object>> DeletingEntities = new Dictionary<DictProcessKey, List<object>>();

        /// <summary>
        /// Очередь запросов выборки сущностей для удаления
        /// </summary>
        protected readonly Queue<IQueryable<IHaveId>> DeletingQueryQueue = new Queue<IQueryable<IHaveId>>();

        /// <summary>
        /// Конструктор
        /// </summary>
        protected BaseApiService()
        {
            this.PreviousOrder = this.MainProcessOrder - 1;
            this.NextOrder = this.MainProcessOrder + 1;
        }

        /// <inheritdoc />
        public virtual long? Create(TCreateModel createModel) =>
            ((PersistentObject)this.InTransaction(() => this.CreateEntity(createModel)))?.Id;

        /// <summary>
        /// Метод создания новой сущности
        /// </summary>
        /// <param name="createModel">Модель с данными для создания</param>
        /// <returns>Созданная сущность</returns>
        protected virtual PersistentObject CreateEntity(TCreateModel createModel) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public long Update(long entityId, TUpdateModel updateModel) =>
            (long)this.InTransaction(() => this.UpdateEntity(entityId, updateModel));

        /// <summary>
        /// Метод обновления сущности
        /// </summary>
        /// <param name="entityId">Идентификатор сущности</param>
        /// <param name="updateModel">Модель с данными для обновления</param>
        /// <returns>Обновленная сущность</returns>
        /// <exception cref="NotImplementedException">Отсутствие реализации</exception>
        protected virtual long UpdateEntity(long entityId, TUpdateModel updateModel) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public async Task DeleteAsync(long entityId) =>
            await this.InTransactionAsync(async () => await this.DeleteEntityAsync<TServiceEntity>(entityId));

        /// <summary>
        /// Метод удаления сущности
        /// </summary>
        /// <typeparam name="TEntity">Сущность для удаления</typeparam>
        /// <param name="entityId">Идентификатор сущности</param>
        private async Task DeleteEntityAsync<TEntity>(long entityId)
            where TEntity : PersistentObject
        {
            var task = this.BeforeDeleteAsync(entityId);

            if (task != null) await task;
                
            this.AddEntityToDelete<TEntity>(entityId);
        }

        /// <summary>
        /// Выполнить действия перед удалением
        /// </summary>
        protected virtual Task BeforeDeleteAsync(long entityId) => null;

        /// <summary>
        /// Выполнить внутри транзакции
        /// </summary>
        protected object InTransaction(Func<object> func)
        {
            using (var dataTransaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var result = func();
                    this.ApplyAllChanges();
                    dataTransaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    try
                    {
                        dataTransaction.Rollback();
                    }
                    catch (TransactionRollbackException transactionException)
                    {
                        throw new DataAccessException(transactionException.Message, e);
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException(string.Format("Произошла не известная ошибка при откате транзакции: \r\n" +
                                "Message: {0}; \r\nStackTrace:{1};",
                                ex.Message,
                                ex.StackTrace),
                            e);
                    }

                    throw;
                }
            }
        }
        
        /// <summary>
        /// Выполнить внутри транзакции
        /// </summary>
        protected async Task InTransactionAsync(Func<Task> func)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    await func();
                    await this.ApplyAllChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException transactionException)
                    {
                        throw new DataAccessException(transactionException.Message, e);
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException(string.Format("Произошла не известная ошибка при откате транзакции: \r\n" +
                                "Message: {0}; \r\nStackTrace:{1};",
                                ex.Message,
                                ex.StackTrace),
                            e);
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Применить все изменения
        /// </summary>
        protected void ApplyAllChanges()
        {
            // Проходимся по словарю с сущностями для сохранения
            this.SavingEntitiesDictProcessing();

            // Проходимся по словарю с сущностями для обновления
            this.UpdatingEntitiesDictProcessing();

            // Проходимся по словарю с сущностями для удаления
            this.DeletingEntitiesDictProcessing();
        }

        /// <summary>
        /// Применить все изменения
        /// </summary>
        protected async Task ApplyAllChangesAsync()
        {
            // Проходимся по очереди запросов с сущностями для удаления
            await this.ProcessQueueAsync();
            
            // Проходимся по словарю с сущностями для сохранения
            this.SavingEntitiesDictProcessing();

            // Проходимся по словарю с сущностями для обновления
            this.UpdatingEntitiesDictProcessing();

            // Проходимся по словарю с сущностями для удаления
            this.DeletingEntitiesDictProcessing();
        }

        /// <summary>
        /// Обработать словарь с сущностями для сохранения
        /// </summary>
        protected void SavingEntitiesDictProcessing() =>
            this.DictProcessing(this.SavingEntities, nameof(IDomainService.Save));

        /// <summary>
        /// Обработать словарь с сущностями для обновления
        /// </summary>
        protected void UpdatingEntitiesDictProcessing() =>
            this.DictProcessing(this.UpdatingEntities, nameof(IDomainService.Update));

        /// <summary>
        /// Обработать словарь с сущностями для удаления
        /// </summary>
        protected void DeletingEntitiesDictProcessing() =>
            this.DictProcessing(this.DeletingEntities, nameof(IDomainService.Delete));

        /// <summary>
        /// Обработать очередь запросов
        /// </summary>
        private async Task ProcessQueueAsync()
        {
            while (this.DeletingQueryQueue.Any())
            {
                await this.DeletingQueryQueue.Dequeue().DeleteAsync();
            }
        }

        /// <summary>
        /// Обработать словарь
        /// </summary>
        private void DictProcessing(Dictionary<DictProcessKey, List<object>> dict, string methodName)
        {
            foreach (var entitiesGroup in dict.OrderBy(x => x.Key.Order))
            {
                var serviceDomain = this.Container.Resolve(entitiesGroup.Key.EntityDomainServiceType);

                using (this.Container.Using(serviceDomain))
                {
                    var method = serviceDomain.GetType().GetMethods()
                        .FirstOrDefault(x => x.Name == methodName && x.ReturnType == typeof(void));
                    entitiesGroup.Value.ForEach(entity => method.Invoke(serviceDomain, new[] { entity }));
                }
            }

            dict.Clear();
        }

        /// <summary>
        /// Добавить сущность в словарь для сохранения
        /// </summary>
        protected void AddEntityToSave<TEntity>(TEntity entity, int? order = null)
            where TEntity : PersistentObject =>
            this.AddEntityToSave(typeof(TEntity), entity, order);

        /// <summary>
        /// Добавить сущность в словарь для сохранения
        /// </summary>
        private void AddEntityToSave<TEntity>(Type entityType, TEntity entity, int? order = null)
            where TEntity : PersistentObject =>
            this.AddEntityToDict(entityType, this.SavingEntities, entity, order);

        /// <summary>
        /// Добавить сущности в словарь для сохранения
        /// </summary>
        protected void AddEntitiesToSave<TEntity>(IEnumerable<TEntity> entities, int? order = null)
            where TEntity : PersistentObject =>
            this.AddEntitiesToDict<TEntity>(this.SavingEntities, entities, order);

        /// <summary>
        /// Добавить сущности в словарь для сохранения
        /// </summary>
        private void AddEntitiesToSave(Type entityType, IEnumerable<object> entities, int? order = null) =>
            this.AddEntitiesToDict(entityType, this.SavingEntities, entities, order);

        /// <summary>
        /// Добавить сущность в словарь для обновления
        /// </summary>
        protected void AddEntityToUpdate<TEntity>(TEntity entity)
            where TEntity : PersistentObject =>
            this.AddEntityToDict<TEntity>(this.UpdatingEntities, entity);

        /// <summary>
        /// Добавить сущности в словарь для обновления
        /// </summary>
        protected void AddEntitiesToUpdate<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : PersistentObject =>
            this.AddEntitiesToDict<TEntity>(this.UpdatingEntities, entities);

        /// <summary>
        /// Добавить сущность в словарь для удаления
        /// </summary>
        protected void AddEntityToDelete<TEntity>(long entityId)
            where TEntity : PersistentObject =>
            this.AddEntityToDict<TEntity>(this.DeletingEntities, entityId);

        /// <summary>
        /// Добавить сущность в словарь для удаления
        /// </summary>
        protected void AddEntityToDelete<TEntity>(TEntity entity)
            where TEntity : PersistentObject =>
            this.AddEntityToDict<TEntity>(this.DeletingEntities, entity.Id);

        /// <summary>
        /// Добавить сущности в словарь для удаления
        /// </summary>
        protected void AddEntitiesToDelete<TEntity>(IEnumerable<long> entityIds)
            where TEntity : PersistentObject =>
            this.AddEntitiesToDict<TEntity>(this.DeletingEntities, entityIds.Cast<object>());

        /// <summary>
        /// Добавить сущности в словарь для удаления
        /// </summary>
        protected void AddEntitiesToDelete<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : PersistentObject =>
            this.AddEntitiesToDict<TEntity>(this.DeletingEntities, entities.Select(x => x.Id as object).ToList());

        /// <summary>
        /// Добавить сущность в определенный словарь
        /// </summary>
        private void AddEntityToDict<TEntity>(IDictionary<DictProcessKey, List<object>> dict, object entity, int? order = null)
            where TEntity : PersistentObject =>
            this.AddEntityToDict(typeof(TEntity), dict, entity, order);

        /// <summary>
        /// Добавить сущность в определенный словарь
        /// </summary>
        private void AddEntityToDict(Type entityType, IDictionary<DictProcessKey, List<object>> dict, object entity, int? order = null) =>
            this.AddEntitiesToDict(entityType, dict, new[] { entity }, order);

        /// <summary>
        /// Добавить сущности в определенный словарь
        /// </summary>
        private void AddEntitiesToDict<TEntity>(
            IDictionary<DictProcessKey, List<object>> dict,
            IEnumerable<object> entities,
            int? order = null) =>
            this.AddEntitiesToDict(typeof(TEntity), dict, entities, order);

        /// <summary>
        /// Добавить сущности в определенный словарь
        /// </summary>
        private void AddEntitiesToDict(
            Type entityType,
            IDictionary<DictProcessKey, List<object>> dict,
            IEnumerable<object> entities,
            int? order = null)
        {
            var entitiesList = entities.ToList();

            if (!entitiesList.Any())
                return;

            var key = new DictProcessKey
            {
                EntityDomainServiceType = typeof(IDomainService<>).MakeGenericType(entityType),
                Order = order ?? this.NextOrder
            };

            if (dict.TryGetValue(key, out var addedEntitiesList))
            {
                addedEntitiesList.AddRange(entitiesList);
            }
            else
            {
                dict.Add(key, entitiesList);
            }
        }

        /// <summary>
        /// Перенос значений полей объекта
        /// </summary>
        /// <param name="model">Модель со значениями для переноса</param>
        /// <param name="entity">Объект, в который заносятся значения</param>
        /// <param name="mainEntity">Дополнительная главная сущность, используемая при переносе</param>
        /// <typeparam name="TModel">Тип модели, со значениями для переноса</typeparam>
        /// <typeparam name="TEntity">Тип объекта для переноса</typeparam>
        protected delegate void TransferValues<in TModel, TEntity>(TModel model, ref TEntity entity, object mainEntity = null);

        /// <summary>
        /// Проверить передачу ссылки на сущность
        /// </summary>
        /// <exception cref="ArgumentNullException">Если сущность не была передана</exception>
        protected void EntityRefCheck(object entity)
        {
            if (entity.IsNull())
                throw new ArgumentNullException(nameof(entity));
        }

        /// <summary>
        /// Проверить наличие уже имеющихся связей у вложений передаваемой сущности
        /// </summary>
        /// <typeparam name="TAnnexEntity">Сущность, содержащая вложения</typeparam>
        /// <typeparam name="TFileInfo">Тип вложений</typeparam>
        /// <param name="files">Коллекция вложений</param>
        /// <param name="condition">Дополнительное условие для проверки</param>
        protected void AnnexEntityRefCheck<TAnnexEntity, TFileInfo>(IEnumerable<TFileInfo> files, Func<TAnnexEntity, bool> condition = null)
            where TAnnexEntity : BaseEntity, IAnnexEntity
            where TFileInfo : BaseFileInfo
        {
            if (!files?.Any() ?? true) return;

            var inFileIds = files.Select(x => x.FileId).ToList();
            var annexDomain = this.Container.ResolveDomain<TAnnexEntity>();
            using (this.Container.Using(annexDomain))
            {
                var existFiles = annexDomain.GetAll()
                    .Where(x => inFileIds.Contains(x.File.Id))
                    .ToList();

                var validIds = existFiles
                    .WhereIf(condition != null, condition)
                    .Join(files,
                        x => new { x.Id, FileId = x.File.Id },
                        y => new { Id = y.Id ?? 0, FileId = y.FileId ?? 0 },
                        (x, y) => x.Id)
                    .ToList();

                var fileNames = existFiles
                    .Where(x => !validIds.Contains(x.Id))
                    .Select(x => $"\"{x.Name}\"")
                    .ToList();

                if (fileNames.Any())
                {
                    var msg = fileNames.Count == 1
                        ? $"Файл с наименованием: {fileNames.First()} связан"
                        : $"Файлы с наименованиями: {string.Join(", ", fileNames)} связаны";

                    throw new ApiServiceException($"{msg} с другой записью, просьба выбрать новый файл");
                }
            }
        }

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="model">Модель со значениями для переноса</param>
        /// <param name="entity"></param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемый объект</param>
        /// <typeparam name="TModel">Тип входных данных</typeparam>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        protected void UpdateEntity<TModel, TEntity>(
            TModel model,
            TEntity entity,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null)
            where TEntity : PersistentObject, new()
        {
            modify(model, ref entity, mainEntity);
            this.AddEntityToUpdate(entity);
        }

        /// <summary>
        /// Создать сущность
        /// </summary>
        /// <param name="model">Модель со значениями</param>
        /// <param name="modify">Изменение сущности значениями модели</param>
        /// <param name="mainEntity">Дополнительная главная сущность, используемая при переносе</param>
        /// <param name="order">Порядок выполнения при применении всех изменений</param>
        /// <param name="createEntityType">
        /// Тип создаваемого экземпляра сущности.
        /// Указывается для случаев, когда TransferValues написан для базовой сущности
        /// </param>
        /// <typeparam name="TModel">Тип модели со значениями</typeparam>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        protected TEntity CreateEntity<TModel, TEntity>(
            TModel model,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null,
            int? order = null,
            Type createEntityType = null)
            where TEntity : PersistentObject, new()
            => this.CreateEntities(model != null ? new[] { model } : null, modify, mainEntity, order, createEntityType).Values.First();

        /// <summary>
        /// Создать сущности
        /// </summary>
        /// <param name="models">Коллекция моделей со значениями для переноса</param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемые объекты</param>
        /// <param name="order">Порядок выполнения при применении всех изменений</param>
        /// <param name="createEntityType">
        /// Тип создаваемого экземпляра сущности.
        /// Указывается для случаев, когда TransferValues написан для базовой сущности
        /// </param>
        /// <typeparam name="TModel">Тип входных данных</typeparam>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        protected Dictionary<TModel, TEntity> CreateEntities<TModel, TEntity>(
            IEnumerable<TModel> models,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null,
            int? order = null,
            Type createEntityType = null)
            where TEntity : PersistentObject, new()
        {
            var result = new Dictionary<TModel, TEntity>();
            var itemsList = (models ?? new List<TModel>()).ToList();

            if (itemsList.Any())
            {
                var newEntities = itemsList
                    .Select(x =>
                    {
                        var newEntity = createEntityType.IsNull()
                            ? new TEntity()
                            : (TEntity)Activator.CreateInstance(createEntityType);

                        modify(x, ref newEntity, mainEntity);
                        result.Add(x, newEntity);
                        return newEntity;
                    })
                    .ToList();

                createEntityType = createEntityType ?? newEntities.First().GetType();

                this.AddEntitiesToSave(createEntityType, newEntities, order);
            }

            return result;
        }

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="model">Модель со значениями для переноса</param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемый объект</param>
        /// <typeparam name="TModel">Тип входных данных</typeparam>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        protected TEntity UpdateEntity<TModel, TEntity>(
            TModel model,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null)
            where TEntity : PersistentObject, new()
            where TModel : IEntityId =>
            this.UpdateEntities(model != null ? new[] { model } : null, modify, mainEntity).Values.First();

        /// <summary>
        /// Обновить сущности
        /// </summary>
        /// <param name="models">Модель со значениями для переноса</param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемый объект</param>
        /// <typeparam name="TModel">Тип входных данных</typeparam>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        protected Dictionary<TModel, TEntity> UpdateEntities<TModel, TEntity>(
            IEnumerable<TModel> models,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null)
            where TEntity : PersistentObject, new()
            where TModel : IEntityId
        {
            var result = new Dictionary<TModel, TEntity>();
            var modelsList = (models ?? new List<TModel>()).ToList();

            if (modelsList.Any())
            {
                var domainService = this.Container.Resolve<IDomainService<TEntity>>();
                using (this.Container.Using(domainService))
                {
                    modelsList.ForEach(x =>
                    {
                        var entity = domainService.Get(x.Id.Value);
                        modify(x, ref entity, mainEntity);
                        result.Add(x, entity);
                        this.AddEntityToUpdate(entity);
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Создать или обновить(создание/обновление/удаление) вложенные сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="models">Коллекция входных данных</param>
        /// <param name="condition">Условие для отбора списка сущностей из БД</param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемые объекты</param>
        /// <param name="order">Порядок выполнения при применении всех изменений</param>
        protected Dictionary<TModel, TEntity> CreateOrUpdateNestedEntities<TModel, TEntity>(
            IEnumerable<TModel> models,
            Expression<Func<TEntity, bool>> condition,
            TransferValues<TModel, TEntity> modify,
            PersistentObject mainEntity = null,
            int? order = null)
            where TEntity : PersistentObject, new()
        {
            return mainEntity.IsNotNull() && mainEntity.Id.IsDefault()
                ? this.CreateEntities(models, modify, mainEntity, order)
                : this.UpdateNestedEntities(models, condition, modify, mainEntity, order);
        }

        /// <summary>
        /// Обновить вложенную сущность
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="model">Модель с данными для переноса</param>
        /// <param name="condition">Условие для отбора списка сущностей из БД</param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемые объекты</param>
        /// <param name="order">Порядок выполнения при применении всех изменений</param>
        protected TEntity UpdateNestedEntity<TModel, TEntity>(
            TModel model,
            Expression<Func<TEntity, bool>> condition,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null,
            int? order = null)
            where TEntity : PersistentObject, new() =>
            this.UpdateNestedEntities(model != null ? new[] { model } : null, condition, modify, mainEntity, order).Values.FirstOrDefault();

        /// <summary>
        /// Обновить вложенные сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="models">Коллекция входных данных</param>
        /// <param name="condition">Условие для отбора списка сущностей из БД</param>
        /// <param name="modify">Изменение сущности входными данными</param>
        /// <param name="mainEntity">Главная сущность, к которой необходимо привязать создаваемые объекты</param>
        /// <param name="order">Порядок обновления при применении всех изменений</param>
        protected Dictionary<TModel, TEntity> UpdateNestedEntities<TModel, TEntity>(
            IEnumerable<TModel> models,
            Expression<Func<TEntity, bool>> condition,
            TransferValues<TModel, TEntity> modify,
            object mainEntity = null,
            int? order = null)
            where TEntity : PersistentObject, new()
        {
            var result = new Dictionary<TModel, TEntity>();

            var domainService = this.Container.Resolve<IDomainService<TEntity>>();
            using (this.Container.Using(domainService))
            {
                var dict = domainService.GetAll()
                    .Where(condition)
                    .ToDictionary(x => x.Id, y => y);

                var modelsList = (models ?? new List<TModel>()).ToList();

                if (modelsList.Any())
                {
                    var isAssignedNestedEntityId = typeof(INestedEntityId).IsAssignableFrom(typeof(TModel));

                    var updatedIds = new List<long>();

                    if (isAssignedNestedEntityId)
                    {
                        var updatedEntities = modelsList
                            .Cast<INestedEntityId>()
                            .Where(x => x.Id.HasValue && dict.Keys.Contains(x.Id.Value))
                            .Select(x =>
                            {
                                var entity = dict.Get(x.Id.Value);
                                modify((TModel)x, ref entity, mainEntity);
                                result.Add((TModel)x, entity);
                                updatedIds.Add(x.Id.Value);
                                return entity;
                            });

                        this.AddEntitiesToUpdate(updatedEntities);
                    }

                    var newEntities = modelsList
                        .WhereIf(isAssignedNestedEntityId, x => !((INestedEntityId)x).Id.HasValue)
                        .Select(x =>
                        {
                            var newEntity = new TEntity();
                            modify(x, ref newEntity, mainEntity);
                            result.Add(x, newEntity);
                            return newEntity;
                        });

                    this.AddEntitiesToSave(newEntities, order);

                    this.AddEntitiesToDelete<TEntity>(dict.Keys.Except(updatedIds).ToList());
                }
                else
                {
                    this.AddEntitiesToDelete<TEntity>(dict.Keys);
                }
            }

            return result;
        }

        /// <summary>
        /// Перенос информации для сущности,
        /// которая реализует интерфейс <see cref="IAnnexEntity"/>
        /// </summary>
        /// <param name="propertyWithMainEntity">Наименование свойства со ссылкой на главную сущность</param>
        /// <typeparam name="TModel">Тип входной модели</typeparam>
        /// <typeparam name="TAnnexEntity">Тип сущности реализующей <see cref="IAnnexEntity"/></typeparam>
        /// <exception cref="ArgumentNullException">Если передать в метод пустое наименование свойства со ссылкой на документ</exception>
        protected TransferValues<TModel, TAnnexEntity> FilesInfoTransfer<TModel, TAnnexEntity>(string propertyWithMainEntity)
            where TModel : BaseFileInfo
            where TAnnexEntity : PersistentObject, IAnnexEntity =>
            (TModel model, ref TAnnexEntity annex, object mainEntity) =>
            {
                if (annex.Id == 0)
                {
                    if (propertyWithMainEntity.IsEmpty())
                        throw new ArgumentNullException(nameof(propertyWithMainEntity));

                    this.EntityRefCheck(mainEntity);
                    var property = annex.GetType().GetProperty(propertyWithMainEntity);
                    property.SetValue(annex, mainEntity);
                }

                annex.File = new FileInfo { Id = (long)model.FileId };
                annex.Name = model.FileName;

                var fileDateIgnored = typeof(TModel).GetProperty(nameof(BaseFileInfo.FileDate))
                    .GetCustomAttributes(false).Any(x => x is JsonIgnoreAttribute);

                if (!fileDateIgnored)
                {
                    annex.DocumentDate = model.FileDate;
                }
                else
                {
                    annex.DocumentDate = annex.DocumentDate ?? DateTime.Now;
                }

                annex.Description = model.FileDescription;
            };

        /// <summary>
        /// Создать сущности-вложения для документа
        /// </summary>
        /// <param name="models">Коллекция моделей с данными для переноса</param>
        /// <param name="propertyWithMainEntity">Наименование поля со ссылкой на документ</param>
        /// <param name="mainObject">Главный объект, к которому прикрепляется вложения</param>
        /// <param name="order">Порядок обработки</param>
        /// <typeparam name="TModel">Тип входной модели</typeparam>
        /// <typeparam name="TAnnexEntity">Тип сущности реализующей <see cref="IAnnexEntity"/></typeparam>
        protected Dictionary<TModel, TAnnexEntity> CreateAnnexEntities<TModel, TAnnexEntity>(
            IEnumerable<TModel> models,
            string propertyWithMainEntity,
            PersistentObject mainObject,
            int? order = null)
            where TModel : BaseFileInfo
            where TAnnexEntity : PersistentObject, IAnnexEntity, new() =>
            this.CreateEntities(models, this.FilesInfoTransfer<TModel, TAnnexEntity>(propertyWithMainEntity), mainObject, order);

        /// <summary>
        /// Обновить сущность-вложение для документа
        /// </summary>
        /// <param name="model">Экземпляр модели с данными для переноса</param>
        /// <param name="condition">Expression для сопоставления</param>
        /// <param name="propertyWithMainEntity">Наименование поля со ссылкой на документ</param>
        /// <param name="document">Экземпляр документа, к которому прикрепляется вложение</param>
        /// <param name="order">Порядок обряботки</param>
        /// <typeparam name="TModel">Тип входной модели</typeparam>
        /// <typeparam name="TAnnexEntity">Тип сущности реализующей <see cref="IAnnexEntity"/></typeparam>
        /// <returns>Обновленная сущность-вложение для документа</returns>
        protected TAnnexEntity UpdateAnnexEntity<TModel, TAnnexEntity>(
            TModel model,
            Expression<Func<TAnnexEntity, bool>> condition,
            string propertyWithMainEntity,
            DocumentGji document,
            int? order = null)
            where TModel : BaseFileInfo
            where TAnnexEntity : PersistentObject, IAnnexEntity, new() =>
            this.UpdateNestedEntity(model, condition, this.FilesInfoTransfer<TModel, TAnnexEntity>(propertyWithMainEntity), document, order);

        /// <summary>
        /// Обновить сущности-вложения для документа
        /// </summary>
        /// <param name="models">Коллекция моделей с данными для переноса</param>
        /// <param name="condition">Expression для сопоставления</param>
        /// <param name="propertyWithMainEntity">Наименование поля со ссылкой на документ</param>
        /// <param name="entity">Сущность, к которой прикрепляются вложения</param>
        /// <param name="order">Порядок обработки</param>
        /// <typeparam name="TModel">Тип входной модели</typeparam>
        /// <typeparam name="TAnnexEntity">Тип сущности реализующей <see cref="IAnnexEntity"/></typeparam>
        /// <returns>Словарь: модель - обновленная(созданная) сущность-вложение для документа</returns>
        protected Dictionary<TModel, TAnnexEntity> UpdateAnnexEntities<TModel, TAnnexEntity>(
            IEnumerable<TModel> models,
            Expression<Func<TAnnexEntity, bool>> condition,
            string propertyWithMainEntity,
            PersistentObject entity,
            int? order = null)
            where TModel : BaseFileInfo
            where TAnnexEntity : PersistentObject, IAnnexEntity, new() =>
            this.UpdateNestedEntities(models, condition, this.FilesInfoTransfer<TModel, TAnnexEntity>(propertyWithMainEntity), entity, order);

        /// <summary>
        /// Получить экземпляр наследованной сущности
        /// </summary>
        /// <param name="entityId">Идентификатор сущности</param>
        /// <param name="inheritEntityType">Тип наследованной сущности</param>
        protected object GetInheritEntityInstance(long entityId, Type inheritEntityType)
        {
            var inheritEntityServiceType = typeof(IDomainService<>).MakeGenericType(inheritEntityType);
            var inheritEntityDomain = this.Container.Resolve(inheritEntityServiceType) as IDomainService;
            
            using(this.Container.Using(inheritEntityDomain))
            {
                return inheritEntityDomain.Get(entityId);
            }
        }

        /// <summary>
        /// Класс ключа для обрабатываемого словаря
        /// </summary>
        protected class DictProcessKey
        {
            /// <summary>
            /// Тип сущности
            /// </summary>
            public Type EntityDomainServiceType { get; set; }

            /// <summary>
            /// Порядок обработки
            /// </summary>
            public int Order { get; set; }
        }
    }
}