namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Базовый класс сервиса документа, который
    /// формируется с привязкой к другому - родительскому
    /// </summary>
    /// <typeparam name="TServiceEntity">Сущность, над которой проводятся CRUD-операции</typeparam>
    /// <typeparam name="TGetModel">Модель получения данных</typeparam>
    /// <typeparam name="TCreateModel">Модель создания данных</typeparam>
    /// <typeparam name="TUpdateModel">Модель обновления данных</typeparam>
    /// <typeparam name="TQuery">Параметры запроса</typeparam>
    public abstract class DocumentWithParentService<TServiceEntity, TGetModel, TCreateModel, TUpdateModel, TQuery> :
        BaseApiService<TServiceEntity, TCreateModel, TUpdateModel>,
        IDocumentWithParentService<TGetModel, TCreateModel, TUpdateModel, TQuery>
        where TServiceEntity : PersistentObject
        where TGetModel : class
        where TCreateModel : class
        where TUpdateModel : class
        where TQuery : BaseDocQueryParams
    {
        /// <summary> Домен-сервис дла <see cref="DocumentGji"/> </summary>
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        /// <summary> Домен-сервис дла <see cref="DocumentGjiChildren"/> </summary>
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        /// <summary> Домен-сервис дла <see cref="DocumentGjiInspector"/> </summary>
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }

        /// <summary> Домен-сервис дла <see cref="InspectionGjiStage"/> </summary>
        public IDomainService<InspectionGjiStage> InspectionGjiStageDomain { get; set; }

        /// <summary>
        /// Провайдер статусов сущностей
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Домен-сервис статусов сущностей
        /// </summary>
        public IDomainService<State> StateDomain { get; set; }

        /// <inheritdoc />
        public TGetModel Get(long documentId)
            => this.GetDocumentList(documentId)?.FirstOrDefault();

        /// <inheritdoc />
        public async Task<TGetModel> GetAsync(long documentId) =>
            (await this.GetDocumentListAsync(documentId))?.FirstOrDefault();

        /// <inheritdoc />
        public IEnumerable<TGetModel> GetList(long[] parentDocumentIds) =>
            this.GetDocumentList(parentDocumentIds: parentDocumentIds);

        /// <inheritdoc />
        public async Task<IEnumerable<TGetModel>> GetListAsync(long[] parentDocumentIds) =>
            await this.GetDocumentListAsync(parentDocumentIds: parentDocumentIds);

        /// <inheritdoc />
        public IEnumerable<TGetModel> GetListByQuery(TQuery queryParams) =>
            this.GetDocumentList(queryParams: queryParams);

        /// <summary>
        /// Очередь документов на регистрацию номера
        /// </summary>
        protected readonly Queue<DocumentGji> NumberRegistrationQueue = new Queue<DocumentGji>();

        /// <summary>
        /// Получить список документов
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="queryParams">Параметры запроса</param>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        protected virtual IEnumerable<TGetModel> GetDocumentList(
            long? documentId = null,
            TQuery queryParams = null,
            params long[] parentDocumentIds) =>
            throw new NotImplementedException();

        /// <summary>
        /// Получить список документов
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="queryParams">Параметры запроса</param>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        protected virtual Task<IEnumerable<TGetModel>> GetDocumentListAsync(
            long? documentId = null,
            TQuery queryParams = null,
            params long[] parentDocumentIds) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public override long? Create(TCreateModel createDocument) => ((DocumentGji)this.InTransaction(() =>
        {
            var document = (DocumentGji)this.CreateEntity(createDocument);

            this.NumberRegistrationQueue.Enqueue(document);

            this.RegisterDocumentNumbers();

            return document;
        }))?.Id;

        /// <summary>
        /// Получить этап документа
        /// </summary>
        /// <param name="document">Документ ГЖИ</param>
        /// <param name="typeStage">Тип этапа проверки ГЖИ</param>
        /// <returns>Этап проверки документа ГЖИ</returns>
        protected InspectionGjiStage GetDocumentInspectionGjiStage(DocumentGji document, TypeStage typeStage)
        {
            var parentStage = this.GetDocumentParentStage(document);

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionGjiStageDomain.FirstOrDefault(x =>
                x.Parent.Id == parentStage.Id && x.TypeStage == typeStage);

            if (currentStage == null)
            {
                newStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = typeStage,
                    Parent = parentStage
                };

                var stageMaxPosition = this.InspectionGjiStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                newStage.Position = stageMaxPosition.Position + 1;

                this.AddEntityToSave(newStage, this.PreviousOrder);
            }

            return currentStage ?? newStage;
        }

        /// <summary>
        /// Получить родительский этап документа
        /// </summary>
        /// <param name="document">Документ ГЖИ</param>
        /// <returns>Этап проверки документа ГЖИ</returns>
        protected InspectionGjiStage GetDocumentParentStage(DocumentGji document)
        {
            var parentStage = document.Stage;
            if (parentStage?.Parent != null)
                parentStage = parentStage.Parent;

            return parentStage;
        }

        /// <summary>
        /// Создавть привязку документа
        /// </summary>
        /// <param name="parentDocId">Идентификатор родительского документа ГЖИ</param>
        /// <param name="children">Дочерний документ ГЖИ</param>
        protected void CreateDocumentGjiChildren(long parentDocId, DocumentGji children) =>
            this.CreateDocumentGjiChildren(new DocumentGji { Id = parentDocId }, children);

        /// <summary>
        /// Создавть привязку документа
        /// </summary>
        /// <param name="parent">Родительский документ ГЖИ</param>
        /// <param name="children">Дочерний документ ГЖИ</param>
        protected void CreateDocumentGjiChildren(DocumentGji parent, DocumentGji children) =>
            this.AddEntityToSave(new DocumentGjiChildren { Parent = parent, Children = children });

        /// <summary>
        /// Перенос информации для <see cref="DocumentGjiInspector"/>
        /// </summary>
        protected TransferValues<long, DocumentGjiInspector> InspectorTransfer =>
            (long model, ref DocumentGjiInspector documentGjiInspector, object mainEntity) =>
            {
                this.EntityRefCheck(mainEntity);
                documentGjiInspector.DocumentGji = mainEntity as DocumentGji;
                documentGjiInspector.Inspector = new Inspector { Id = model };
            };

        /// <summary>
        /// Создать инспектора для документа
        /// </summary>
        /// <param name="inspectorId">Идентификатор испектора</param>
        /// <param name="document">Документ ГЖИ</param>
        /// <param name="order">Порядок обработки</param>
        protected void CreateInspector(long inspectorId, DocumentGji document, int? order = null) =>
            this.CreateEntity(inspectorId, this.InspectorTransfer, document, order);

        /// <summary>
        /// Создать инспекторов для документа
        /// </summary>
        /// <param name="inspectorIds">Перечень идентификаторов инспекторов</param>
        /// <param name="document">Документ ГЖИ</param>
        /// <param name="order">Порядок обработки</param>
        protected void CreateInspectors(IEnumerable<long> inspectorIds, DocumentGji document, int? order = null) =>
            this.CreateEntities(inspectorIds, this.InspectorTransfer, document, order);

        /// <summary>
        /// Expression для выявления инспекторов указанного документа ГЖИ
        /// </summary>
        /// <param name="document">Документ ГЖИ, инспекторов которого нужно найти</param>
        private Expression<Func<DocumentGjiInspector, bool>> InspectorCondition(DocumentGji document) =>
            x => x.DocumentGji.Id == document.Id;

        /// <summary>
        /// Обновить инспектора документа ГЖИ
        /// </summary>
        /// <param name="inspectorId">Идентификатор инспектора</param>
        /// <param name="document">Документ ГЖИ</param>
        /// <param name="order">Порядок обряботки</param>
        /// <returns>Обновленная привязка инспектора к документу</returns>
        protected DocumentGjiInspector UpdateInspector(long inspectorId, DocumentGji document, int? order = null) =>
            this.UpdateNestedEntity(inspectorId, this.InspectorCondition(document), this.InspectorTransfer, document, order);

        /// <summary>
        /// Обновить инспекторов документа ГЖИ
        /// </summary>
        /// <param name="inspectorIds">Идентификаторы инспекторов</param>
        /// <param name="document">Документ ГЖИ</param>
        /// <param name="order">Порядок обработки</param>
        /// <returns>Словарь: идентификатор инспектора - обновленная(созданная) привязка к документу</returns>
        protected Dictionary<long, DocumentGjiInspector> UpdateInspectors(IEnumerable<long> inspectorIds, DocumentGji document, int? order = null) =>
            this.UpdateNestedEntities(inspectorIds, this.InspectorCondition(document), this.InspectorTransfer, document, order);

        /// <summary>
        /// Проставление номера документам из <see cref="NumberRegistrationQueue"/>
        /// </summary>
        private void RegisterDocumentNumbers()
        {
            this.ApplyAllChanges();

            while (this.NumberRegistrationQueue.Any())
            {
                var document = this.NumberRegistrationQueue.Dequeue();

                var typeId = this.StateProvider.GetStatefulEntityInfo(document.GetType()).TypeId;
                var state = this.StateDomain.GetAll().First(x => x.TypeId == typeId && x.Code.ToLower() == "присвоение номера");

                this.StateProvider.SetDefaultState(document);
                this.StateProvider.ChangeState(document.Id, typeId, state);
            }
        }
    }
}