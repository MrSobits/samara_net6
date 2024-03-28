namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Extensions;

    /// <summary>
    /// Сервис "Протокола действия акта"
    /// </summary>
    public class DocumentActCheckActionService : BaseApiService<ActCheckAction, DocumentActCheckActionCreate, DocumentActCheckActionUpdate>,
        IDocumentActCheckActionService
    {
        #region DependencyInjection
        private readonly IDomainService<ActCheckAction> actCheckActionDomain;
        private readonly IDomainService<InstrExamAction> instrExamActionDomain;
        private readonly IDomainService<DocRequestAction> docRequestActionDomain;
        private readonly IDomainService<ExplanationAction> explanationActionDomain;
        private readonly IDomainService<ActCheckActionFile> actCheckActionFileDomain;
        private readonly IDomainService<ActCheckActionRemark> actCheckActionRemarkDomain;
        private readonly IDomainService<SurveyActionQuestion> surveyActionQuestionDomain;
        private readonly IDomainService<DocRequestActionRequestInfo> docRequestActionRequestInfoDomain;
        private readonly IDomainService<ActCheckActionViolation> actCheckActionViolationDomain;
        private readonly IDomainService<InstrExamActionNormativeDoc> instrExamActionNormativeDocDomain;
        private readonly IDomainService<ActCheckActionCarriedOutEvent> actCheckActionCarriedOutEventDomain;
        private readonly IDomainService<ActCheckActionInspector> actCheckActionInspectorDomain;

        public DocumentActCheckActionService(
            IDomainService<ActCheckAction> actCheckActionDomain,
            IDomainService<InstrExamAction> instrExamActionDomain,
            IDomainService<DocRequestAction> docRequestActionDomain,
            IDomainService<ExplanationAction> explanationActionDomain,
            IDomainService<ActCheckActionFile> actCheckActionFileDomain,
            IDomainService<ActCheckActionRemark> actCheckActionRemarkDomain,
            IDomainService<SurveyActionQuestion> surveyActionQuestionDomain,
            IDomainService<DocRequestActionRequestInfo> docRequestActionRequestInfoDomain,
            IDomainService<ActCheckActionViolation> actCheckActionViolationDomain,
            IDomainService<InstrExamActionNormativeDoc> instrExamActionNormativeDocDomain,
            IDomainService<ActCheckActionCarriedOutEvent> actCheckActionCarriedOutEventDomain,
            IDomainService<ActCheckActionInspector> actCheckActionInspectorDomain
        )
        {
            this.actCheckActionDomain = actCheckActionDomain;
            this.instrExamActionDomain = instrExamActionDomain;
            this.docRequestActionDomain = docRequestActionDomain;
            this.explanationActionDomain = explanationActionDomain;
            this.actCheckActionFileDomain = actCheckActionFileDomain;
            this.actCheckActionRemarkDomain = actCheckActionRemarkDomain;
            this.surveyActionQuestionDomain = surveyActionQuestionDomain;
            this.docRequestActionRequestInfoDomain = docRequestActionRequestInfoDomain;
            this.actCheckActionViolationDomain = actCheckActionViolationDomain;
            this.instrExamActionNormativeDocDomain = instrExamActionNormativeDocDomain;
            this.actCheckActionCarriedOutEventDomain = actCheckActionCarriedOutEventDomain;
            this.actCheckActionInspectorDomain = actCheckActionInspectorDomain;
        }
        #endregion

        /// <inheritdoc />
        public DocumentActCheckActionGet Get(long actionId)
            => this.GetActCheckActionList(actionId)?.FirstOrDefault();

        /// <inheritdoc />
        public IEnumerable<DocumentActCheckActionGet> GetList(long[] parentDocumentIds)
            => this.GetActCheckActionList(parentDocumentIds: parentDocumentIds);

        /// <summary>
        /// Получить список действия акта проверок
        /// </summary>
        /// <param name="actionId">Идентификатор действия акта проверки </param>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        /// <returns></returns>
        private IEnumerable<DocumentActCheckActionGet> GetActCheckActionList(long? actionId = null, params long[] parentDocumentIds)
        {
            var childrenDocumentsInfoDict = this.actCheckActionDomain.GetAll()
                .Where(x =>
                    x.ActCheck.TypeDocumentGji == TypeDocumentGji.ActCheck ||
                    x.ActCheck.TypeDocumentGji == TypeDocumentGji.ActActionIsolated)
                .WhereIfElse(!actionId.HasValue,
                    x => parentDocumentIds.Contains(x.ActCheck.Id),
                    x => x.Id == actionId)
                .Select(x => new
                {
                    ParentId = x.ActCheck.Id,
                    ActCheckActionId = x.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ActCheckActionId)
                .ToDictionary(x => x.Key, y => y.First().ParentId);

            var actCheckActionIds = childrenDocumentsInfoDict.Keys.ToArray();

            if (!actCheckActionIds.Any())
            {
                return null;
            }

            var inspectorsDict = this.actCheckActionInspectorDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.ActCheckAction.Id))
                .Where(x => x.Inspector != null)
                .AsEnumerable()
                .GroupBy(x => x.ActCheckAction.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Inspector.Id).ToArray());

            var actCheckActionViolationsDict = this.actCheckActionViolationDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.ActCheckAction.Id))
                .Where(x => x.Violation != null)
                .AsEnumerable()
                .GroupBy(x => x.ActCheckAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckActionViolationGet
                    {
                        Id = x.Id,
                        ViolationId = x.Violation.Id,
                        Explanation = x.ContrPersResponse
                    }));

            var filesInfoDict = this.actCheckActionFileDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.ActCheckAction.Id))
                .Where(x => x.File != null)
                .AsEnumerable()
                .GroupBy(x => x.ActCheckAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckActionFileInfoGet
                    {
                        Id = x.Id,
                        FileId = x.File.Id,
                        FileName = x.Name,
                        FileDescription = x.Description
                    }));

            var remarksDict = this.actCheckActionRemarkDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.ActCheckAction.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActCheckAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckActionRemarkGet
                    {
                        Id = x.Id,
                        Remark = x.Remark,
                        FullName = x.MemberFio
                    }));

            var questionsDict = this.surveyActionQuestionDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.SurveyAction.Id))
                .AsEnumerable()
                .GroupBy(x => x.SurveyAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new SurveyActionQuestionGet
                    {
                        Id = x.Id,
                        Answer = x.Answer,
                        Question = x.Question
                    }));

            var docRequestActionRequestInfoDict = this.docRequestActionRequestInfoDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.DocRequestAction.Id))
                .AsEnumerable()
                .GroupBy(x => x.DocRequestAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new DocRequestActionRequestInfoGet
                    {
                        Id = x.Id,
                        Description = x.RequestInfoType,
                        Name = x.Name
                    }));

            var instrExamActionNormativeDocsIds = this.instrExamActionNormativeDocDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.InstrExamAction.Id) && x.NormativeDoc != null)
                .AsEnumerable()
                .GroupBy(x => x.InstrExamAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => x.NormativeDoc.Id));

            var actCheckActionCarriedOutEventTypesDict = this.actCheckActionCarriedOutEventDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.ActCheckAction.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActCheckAction.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => x.EventType));

            var instrExamActions = this.instrExamActionDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.Id))
                .ToList();

            var docRequestActions = this.docRequestActionDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.Id))
                .ToList();

            var explanationActions = this.explanationActionDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.Id))
                .ToList();

            return this.actCheckActionDomain.GetAll()
                .Where(x => actCheckActionIds.Contains(x.Id))
                .AsEnumerable()
                .Select(x =>
                {
                    var instrExamAction = instrExamActions.SingleOrDefault(y => y.Id == x.Id);
                    var docRequestAction = docRequestActions.SingleOrDefault(y => y.Id == x.Id);
                    var explanationAction = explanationActions.SingleOrDefault(y => y.Id == x.Id);

                    return new DocumentActCheckActionGet
                    {
                        Id = x.Id,
                        ParentDocumentId = x.ActCheck.Id,
                        Violations = actCheckActionViolationsDict.Get(x.Id),
                        DocumentNumber = x.Number,
                        DocumentDate = x.Date,
                        StartDate = x.StartDate,
                        StartTime = x.StartTime,
                        EndDate = x.EndDate,
                        EndTime = x.EndTime,
                        InspectorIds = inspectorsDict.Get(x.Id),
                        DocumentPlace = x.CreationPlace?.CopyIdenticalProperties<FiasAddress>(),
                        Files = filesInfoDict.Get(x.Id),
                        Remarks = remarksDict.Get(x.Id),
                        Questions = questionsDict.Get(x.Id),
                        Regulations = instrExamActionNormativeDocsIds.Get(x.Id),
                        Location = x.ExecutionPlace?.CopyIdenticalProperties<FiasAddress>(),
                        DenialAccess = instrExamAction?.TerritoryAccessDenied,
                        Territory = instrExamAction?.Territory,
                        Room = instrExamAction?.Premise,
                        Email = docRequestAction?.ContrPersEmailAddress,
                        Deadline = docRequestAction?.DocProvidingPeriod,
                        Explanation = explanationAction?.Explanation,
                        EventType = actCheckActionViolationsDict.Any() ? actCheckActionCarriedOutEventTypesDict.Get(x.Id) : null,
                        AddressSubmissionDocuments = docRequestAction?.DocProvidingAddress?.CopyIdenticalProperties<FiasAddress>(),
                        FullNameRepresentative = x.RepresentFio,
                        NameControlledPerson = x.ContrPersFio,
                        PositionControlledPerson = x.ContrPersPost,
                        PowerAttorneyDate = x.RepresentProcurationIssuedOn,
                        PowerAttorneyNumber = x.RepresentProcurationNumber,
                        WorkControlledPerson = x.ContrPersWorkPlace,
                        ActionType = x.ActionType,
                        InformationRequested = docRequestActionRequestInfoDict.Get(x.Id)
                    };
                })
                .Where(x => x.ParentDocumentId > 0)
                .Where(x => x.InspectorIds != null && x.InspectorIds.Any())
                .Where(x =>
                    x.DocumentPlace == null || x.DocumentPlace != null &&
                    x.DocumentPlace.PlaceAddressName.IsNotEmpty() &&
                    x.DocumentPlace.PlaceGuidId.IsNotEmpty() &&
                    x.DocumentPlace.PlaceName.IsNotEmpty() &&
                    x.DocumentPlace.House.IsNotEmpty())
                .Where(x =>
                    x.Location == null || x.Location != null &&
                    x.Location.PlaceAddressName.IsNotEmpty() &&
                    x.Location.PlaceGuidId.IsNotEmpty() &&
                    x.Location.PlaceName.IsNotEmpty() &&
                    x.Location.House.IsNotEmpty())
                .Where(x =>
                    x.AddressSubmissionDocuments == null || x.AddressSubmissionDocuments != null &&
                    x.AddressSubmissionDocuments.PlaceAddressName.IsNotEmpty() &&
                    x.AddressSubmissionDocuments.PlaceGuidId.IsNotEmpty() &&
                    x.AddressSubmissionDocuments.PlaceName.IsNotEmpty() &&
                    x.AddressSubmissionDocuments.House.IsNotEmpty())
                .ToArray();
        }

        /// <summary>
        /// Перенос информации для <see cref="ActCheckAction"/>
        /// </summary> 
        private TransferValues<TModel, TActCheckAction> ActCheckActionTransfer<TModel, TActCheckAction, TActCheckActionViolation, TActCheckActionRemark,
            TActCheckActionFile, TSurveyActionQuestion, TDocRequestActionRequestInfo>()
            where TModel : BaseDocumentActCheckAction<TActCheckActionViolation, TActCheckActionRemark,
                TActCheckActionFile, TSurveyActionQuestion, TDocRequestActionRequestInfo>
            where TSurveyActionQuestion : BaseSurveyActionQuestion
            where TDocRequestActionRequestInfo : BaseDocRequestActionRequestInfo
            where TActCheckAction : ActCheckAction =>
            (TModel model, ref TActCheckAction actCheckAction, object mainEntity) =>
            {
                actCheckAction.Number = model.DocumentNumber;
                actCheckAction.Date = model.DocumentDate ?? DateTime.Now;
                actCheckAction.StartTime = model.StartTime;
                actCheckAction.EndTime = model.EndTime;
                actCheckAction.StartDate = model.StartDate ?? actCheckAction.StartDate;
                actCheckAction.EndDate = model.EndDate ?? actCheckAction.EndDate;
                actCheckAction.RepresentFio = model.FullNameRepresentative;
                actCheckAction.ContrPersFio = model.NameControlledPerson;
                actCheckAction.ContrPersPost = model.PositionControlledPerson;
                actCheckAction.RepresentProcurationIssuedOn = model.PowerAttorneyDate;
                actCheckAction.RepresentProcurationNumber = model.PowerAttorneyNumber;
                actCheckAction.ContrPersWorkPlace = model.WorkControlledPerson;
                actCheckAction.CreationPlace = model.DocumentPlace.GetFiasAddress(this.Container, actCheckAction.CreationPlace);
                actCheckAction.ExecutionPlace = model.Location?.GetFiasAddress(this.Container, actCheckAction.ExecutionPlace) ?? actCheckAction.ExecutionPlace;

                switch (actCheckAction.ActionType)
                {
                    case ActCheckActionType.Inspection:
                        var inspectionAction = actCheckAction as InspectionAction;

                        inspectionAction.HasRemark = model.Remarks.IsNotNull() && model.Remarks.Any()
                            ? HasValuesNotSet.Yes : HasValuesNotSet.No;
                        inspectionAction.HasViolation = model.Violations.IsNotNull() && model.Violations.Any()
                            ? YesNoNotSet.Yes : YesNoNotSet.No;
                        break;
                    case ActCheckActionType.GettingWrittenExplanations:
                        var explanationAction = actCheckAction as ExplanationAction;

                        explanationAction.Explanation = model.Explanation;
                        break;
                    case ActCheckActionType.Survey:
                        var surveyAction = actCheckAction as SurveyAction;

                        surveyAction.HasRemark = model.Remarks.IsNotNull() && model.Remarks.Any()
                            ? HasValuesNotSet.Yes : HasValuesNotSet.No;
                        surveyAction.ProtocolReaded = YesNoNotSet.NotSet;

                        this.CreateOrUpdateNestedEntities(model.Questions,
                            x => x.SurveyAction.Id == surveyAction.Id,
                            this.SurveyActionQuestionTransfer<TSurveyActionQuestion>(),
                            surveyAction);
                        break;
                    case ActCheckActionType.InstrumentalExamination:
                        var instrExamAction = actCheckAction as InstrExamAction;

                        instrExamAction.HasRemark = model.Remarks.IsNotNull() && model.Remarks.Any()
                            ? HasValuesNotSet.Yes : HasValuesNotSet.No;
                        instrExamAction.HasViolation = model.Violations.IsNotNull() && model.Violations.Any()
                            ? YesNoNotSet.Yes : YesNoNotSet.No;
                        instrExamAction.Territory = model.Territory;
                        instrExamAction.Premise = model.Room;
                        instrExamAction.TerritoryAccessDenied = model.DenialAccess ?? false;

                        this.CreateOrUpdateNestedEntities(model.Regulations,
                            x => x.InstrExamAction.Id == instrExamAction.Id,
                            this.InstrExamActionNormativeDocTransfer,
                            instrExamAction);
                        break;
                    case ActCheckActionType.RequestingDocuments:
                        var docRequestAction = actCheckAction as DocRequestAction;

                        docRequestAction.ContrPersEmailAddress = model.Email;
                        docRequestAction.DocProvidingPeriod = model.Deadline ?? 0;
                        docRequestAction.DocProvidingAddress = model.AddressSubmissionDocuments?.GetFiasAddress(this.Container, docRequestAction.DocProvidingAddress)
                            ?? docRequestAction.DocProvidingAddress;

                        this.CreateOrUpdateNestedEntities(model.InformationRequested,
                            x => x.DocRequestAction.Id == docRequestAction.Id,
                            this.DocRequestActionRequestInfoTransfer<TDocRequestActionRequestInfo>(),
                            docRequestAction);
                        break;
                    default:
                        throw new ApiServiceException("Указанный тип протокола действия не поддерживается");
                }
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckActionInspector"/>
        /// </summary>
        private TransferValues<long, ActCheckActionInspector> ActCheckActionInspectorTransfer =>
            (long inspectorId, ref ActCheckActionInspector documentGjiInspector, object mainEntity) =>
            {
                this.EntityRefCheck(mainEntity);
                documentGjiInspector.ActCheckAction = mainEntity as ActCheckAction;
                documentGjiInspector.Inspector = new Inspector { Id = inspectorId };
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckActionCarriedOutEvent"/>
        /// </summary>
        private TransferValues<ActCheckActionCarriedOutEventType, ActCheckActionCarriedOutEvent> ActCheckActionCarriedOutEventTransfer =>
            (ActCheckActionCarriedOutEventType model, ref ActCheckActionCarriedOutEvent documentGjiInspector, object mainEntity) =>
            {
                this.EntityRefCheck(mainEntity);
                documentGjiInspector.ActCheckAction = mainEntity as ActCheckAction;
                documentGjiInspector.EventType = model;
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckActionRemark"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, ActCheckActionRemark> ActCheckActionRemarkTransfer<TModel>()
            where TModel : BaseActCheckActionRemark =>
            (TModel model, ref ActCheckActionRemark actCheckActionRemark, object mainEntity) =>
            {
                if (actCheckActionRemark.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    actCheckActionRemark.ActCheckAction = mainEntity as ActCheckAction;
                }

                actCheckActionRemark.MemberFio = model.FullName;
                actCheckActionRemark.Remark = model.Remark;
            };

        /// <summary>
        /// Перенос информации для <see cref="SurveyActionQuestion"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, SurveyActionQuestion> SurveyActionQuestionTransfer<TModel>()
            where TModel : BaseSurveyActionQuestion =>
            (TModel model, ref SurveyActionQuestion surveyActionQuestion, object mainEntity) =>
            {
                if (surveyActionQuestion.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    surveyActionQuestion.SurveyAction = mainEntity as SurveyAction;
                }

                surveyActionQuestion.Answer = model.Answer;
                surveyActionQuestion.Question = model.Question;
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckActionViolation"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, ActCheckActionViolation> ActCheckActionViolationTransfer<TModel>()
            where TModel : BaseActCheckActionViolation =>
            (TModel model, ref ActCheckActionViolation actCheckActionViolation, object mainEntity) =>
            {
                if (actCheckActionViolation.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    actCheckActionViolation.ActCheckAction = mainEntity as ActCheckAction;
                }

                actCheckActionViolation.Violation = model.ViolationId != null
                    ? new ViolationGji { Id = model.ViolationId ?? 0 }
                    : null;
                actCheckActionViolation.ContrPersResponse = model.Explanation;
            };

        /// <summary>
        /// Перенос информации для <see cref="DocRequestActionRequestInfo"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, DocRequestActionRequestInfo> DocRequestActionRequestInfoTransfer<TModel>()
            where TModel : BaseDocRequestActionRequestInfo =>
            (TModel model, ref DocRequestActionRequestInfo docRequestActionRequestInfo, object mainEntity) =>
            {
                if (docRequestActionRequestInfo.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    docRequestActionRequestInfo.DocRequestAction = mainEntity as DocRequestAction;
                }

                docRequestActionRequestInfo.Name = model.Name;
                docRequestActionRequestInfo.RequestInfoType = model.Description;
            };

        /// <summary>
        /// Перенос информации для <see cref="InstrExamActionNormativeDoc"/>
        /// </summary>
        private TransferValues<long, InstrExamActionNormativeDoc> InstrExamActionNormativeDocTransfer =>
            (long normativeDocId, ref InstrExamActionNormativeDoc documentGjiInspector, object mainEntity) =>
            {
                this.EntityRefCheck(mainEntity);
                documentGjiInspector.InstrExamAction = mainEntity as InstrExamAction;
                documentGjiInspector.NormativeDoc = new NormativeDoc { Id = normativeDocId };
            };

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(DocumentActCheckActionCreate createActCheckAction)
        {
            var entityType = ((ActCheckActionType)createActCheckAction.ActionType).GetEntityClassType();

            if (entityType.IsNull())
                throw new ApiServiceException("Указанный тип протокола действия не поддерживается");

            var actCheckAction = this.CreateEntity(createActCheckAction,
                this.ActCheckActionTransfer<DocumentActCheckActionCreate, ActCheckAction, ActCheckActionViolationCreate, ActCheckActionRemarkCreate,
                    ActCheckActionFileInfoCreate, SurveyActionQuestionCreate, DocRequestActionRequestInfoCreate>(),
                order: this.MainProcessOrder,
                createEntityType: entityType);

            // Наличие акта проверки гарантировано required-атрибутом с проверкой
            actCheckAction.ActCheck = new ActCheck { Id = (long)createActCheckAction.ParentDocumentId };

            this.CreateEntities(createActCheckAction.InspectorIds, this.ActCheckActionInspectorTransfer, actCheckAction);
            this.CreateAnnexEntities<ActCheckActionFileInfoCreate, ActCheckActionFile>(createActCheckAction.Files,
                nameof(ActCheckActionFile.ActCheckAction),
                actCheckAction);
            this.CreateEntities(createActCheckAction.Remarks, this.ActCheckActionRemarkTransfer<ActCheckActionRemarkCreate>(), actCheckAction);
            this.CreateEntities(createActCheckAction.Violations, this.ActCheckActionViolationTransfer<ActCheckActionViolationCreate>(), actCheckAction);
            this.CreateEntities(createActCheckAction.EventType, this.ActCheckActionCarriedOutEventTransfer, actCheckAction);

            return actCheckAction;
        }

        /// <summary>
        /// Обновить действие акта
        /// </summary>
        protected override long UpdateEntity(long actionId, DocumentActCheckActionUpdate updateActCheckAction)
        {
            var actCheckAction = this.actCheckActionDomain.Get(actionId);

            if (actCheckAction.IsNull())
                throw new ApiServiceException("Не найден Протокол действия для обновления");

            var inheritActionType = actCheckAction.ActionType.GetEntityClassType();

            if (inheritActionType.IsNotNull())
            {
                // Экземпляр наследованной сущности приводится к базовой, чтобы прокинуть его в transfer
                actCheckAction = (ActCheckAction)this.GetInheritEntityInstance(actionId, inheritActionType);
            }

            this.UpdateEntity(updateActCheckAction, actCheckAction,
                this.ActCheckActionTransfer<DocumentActCheckActionUpdate, ActCheckAction, ActCheckActionViolationUpdate, ActCheckActionRemarkUpdate,
                    ActCheckActionFileInfoUpdate, SurveyActionQuestionUpdate, DocRequestActionRequestInfoUpdate>());

            this.UpdateNestedEntities(updateActCheckAction.InspectorIds,
                x => x.ActCheckAction.Id == actionId,
                this.ActCheckActionInspectorTransfer,
                actCheckAction);

            this.UpdateNestedEntities(updateActCheckAction.Violations,
                x => x.ActCheckAction.Id == actionId,
                this.ActCheckActionViolationTransfer<ActCheckActionViolationUpdate>(),
                actCheckAction);

            this.UpdateAnnexEntities<ActCheckActionFileInfoUpdate, ActCheckActionFile>(
                updateActCheckAction.Files,
                x => x.ActCheckAction.Id == actionId,
                nameof(ActCheckActionFile.ActCheckAction),
                actCheckAction);

            this.UpdateNestedEntities(updateActCheckAction.Remarks,
                x => x.ActCheckAction.Id == actionId,
                this.ActCheckActionRemarkTransfer<ActCheckActionRemarkUpdate>(),
                actCheckAction);

            this.UpdateNestedEntities(updateActCheckAction.EventType,
                x => x.ActCheckAction.Id == actionId,
                this.ActCheckActionCarriedOutEventTransfer,
                actCheckAction);

            return actCheckAction.Id;
        }
    }
}