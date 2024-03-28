namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    using NHibernate.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Базовый сервис для документов "Акт проверки" и его наследников
    /// </summary>
    /// <typeparam name="TServiceEntity">Сущность, над которой проводятся CRUD операции</typeparam>
    /// <typeparam name="TGetModel">Модель получения данных</typeparam>
    /// <typeparam name="TCreateModel">Модель создания данных</typeparam>
    /// <typeparam name="TUpdateModel">Модель обновления данных</typeparam>
    /// <typeparam name="TQueryModel">Модель запроса</typeparam>
    public abstract class BaseActCheckDocumentService<TServiceEntity, TGetModel, TCreateModel, TUpdateModel, TQueryModel>
        : DocumentWithParentService<TServiceEntity, TGetModel, TCreateModel, TUpdateModel, TQueryModel>
        where TServiceEntity : ActCheck
        where TGetModel : DocumentActCheckGet
        where TCreateModel : DocumentActCheckCreate
        where TUpdateModel : DocumentActCheckUpdate
        where TQueryModel : BaseDocQueryParams
    {
        #region DependencyInjection
        protected readonly IDomainService<ActCheckWitness> ActCheckWitnessDomain;
        protected readonly IDomainService<ActCheckInspectedPart> ActCheckInspectedPartDomain;
        
        private readonly IDomainService<ActCheckAnnex> actCheckAnnexDomain;
        private readonly IDomainService<ActCheckProvidedDoc> actCheckProvidedDocDomain;
        private readonly IDomainService<ActCheckRealityObject> actCheckRealityObjectDomain;
        private readonly IDomainService<ActCheckViolation> actCheckViolationDomain;
        private readonly IDomainService<ActCheckAction> actCheckActionDomain;
        private readonly IDomainService<ActCheckDefinition> actCheckDefinitionDomain;
        private readonly IDomainService<RealityObject> realityObjectDomain;
        private readonly IDomainService<DocumentGjiPdfSignInfo> docGjiPdfSignInfoDomain;

        protected BaseActCheckDocumentService(IDomainService<ActCheckAnnex> actCheckAnnexDomain,
            IDomainService<ActCheckWitness> actCheckWitnessDomain,
            IDomainService<ActCheckProvidedDoc> actCheckProvidedDocDomain,
            IDomainService<ActCheckRealityObject> actCheckRealityObjectDomain,
            IDomainService<ActCheckInspectedPart> actCheckInspectedPartDomain,
            IDomainService<ActCheckViolation> actCheckViolationDomain,
            IDomainService<ActCheckAction> actCheckActionDomain,
            IDomainService<ActCheckDefinition> actCheckDefinitionDomain,
            IDomainService<RealityObject> realityObjectDomain,
            IDomainService<DocumentGjiPdfSignInfo> docGjiPdfSignInfoDomain)
        { 
            this.actCheckAnnexDomain = actCheckAnnexDomain;
            this.ActCheckWitnessDomain = actCheckWitnessDomain;
            this.actCheckProvidedDocDomain = actCheckProvidedDocDomain;
            this.actCheckRealityObjectDomain = actCheckRealityObjectDomain;
            this.ActCheckInspectedPartDomain = actCheckInspectedPartDomain;
            this.actCheckViolationDomain = actCheckViolationDomain;
            this.actCheckActionDomain = actCheckActionDomain;
            this.actCheckDefinitionDomain = actCheckDefinitionDomain;
            this.realityObjectDomain = realityObjectDomain;
            this.docGjiPdfSignInfoDomain = docGjiPdfSignInfoDomain;
        }
        #endregion

        /// <summary>
        /// Общий метод получения списка документов "Акт проверки" и его наследников
        /// </summary>
        /// <typeparam name="TOut">Выходной тип</typeparam>
        /// <param name="typeDocument">Тип документа</param>
        /// <param name="parentTypeDocument">Тип родительского документа</param>
        /// <param name="childrenDocTypes">Типы дочерних документов</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="parentDocumentIds">Идентификатор родительского документа</param>
        /// <returns></returns>
        protected IEnumerable<TOut> CommonGetDocumentList<TOut>(TypeDocumentGji typeDocument, TypeDocumentGji parentTypeDocument,
            TypeDocumentGji[] childrenDocTypes, long? documentId = null, params long[] parentDocumentIds)
            where TOut : DocumentActCheckGet, new()
        {
            var outModel = new TOut();

            var childrenDocumentsInfoDict = this.DocumentGjiChildrenDomain.GetAll()
                .Where(x =>
                    x.Parent.TypeDocumentGji == parentTypeDocument &&
                    x.Children.TypeDocumentGji == typeDocument)
                .WhereIfElse(!documentId.HasValue,
                    x => parentDocumentIds.Contains(x.Parent.Id),
                    x => x.Children.Id == documentId)
                .Select(x => new
                {
                    ParentId = x.Parent.Id,
                    ChildrenId = x.Children.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.ChildrenId)
                .ToDictionary(x => x.Key, y => y.First().ParentId);

            var actCheckIds = childrenDocumentsInfoDict.Keys.ToArray();

            if (!actCheckIds.Any())
                return default;

            var actCheckChildrenDocTypesDict = this.DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == typeDocument)
                .Where(x => childrenDocTypes.Contains(x.Children.TypeDocumentGji))
                .Where(x => actCheckIds.Contains(x.Parent.Id))
                .Where(x => !x.Children.State.FinalState)
                .GroupBy(x => x.Parent.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => x.Children.TypeDocumentGji).Distinct().ToArray());

            var inspectorsDict = this.DocumentGjiInspectorDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.DocumentGji.Id))
                .Where(x => x.Inspector != null)
                .AsEnumerable()
                .GroupBy(x => x.DocumentGji.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Inspector.Id).ToArray());

            var witnessesDict = !Attribute.IsDefined(outModel.GetType().GetProperty(nameof(outModel.Witnesses)), typeof(JsonIgnoreAttribute))
                ? this.ActCheckWitnessDomain.GetAll()
                    .Where(x => actCheckIds.Contains(x.ActCheck.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.ActCheck.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(x => new ActCheckWitnessGet
                        {
                            Id = x.Id,
                            FullName = x.Fio,
                            Position = x.Position,
                            SignAcquainted = x.IsFamiliar
                        }))
                : new Dictionary<long, IEnumerable<ActCheckWitnessGet>>();

            var actCheckViolationsDict = this.actCheckViolationDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.Document.Id))
                .Where(x => x.InspectionViolation != null && x.InspectionViolation.Violation != null)
                .AsEnumerable()
                .GroupBy(x => x.ActObject.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckViolationGet
                    {
                        Id = x.Id,
                        ViolationId = x.InspectionViolation.Violation.Id,
                        TermElimination = x.DatePlanRemoval ?? x.InspectionViolation.DatePlanRemoval
                    }));

            var eventResultsDict = this.actCheckRealityObjectDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.ActCheck.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActCheck.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckEventResultGet
                    {
                        Id = x.Id,
                        AddressId = x.RealityObject?.Id,
                        Violations = actCheckViolationsDict.Get(x.Id),
                        DescriptionViolations = x.Description
                    }));

            var providedDocumentsDict = this.actCheckProvidedDocDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.ActCheck.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActCheck.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckProvidedDocumentGet
                    {
                        Id = x.Id,
                        ProvidedDocumentId = x.ProvidedDoc.Id,
                        ProvidedDocumentDate = x.DateProvided
                    }));

            var inspectedPartsDict = this.ActCheckInspectedPartDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.ActCheck.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActCheck.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new ActCheckInspectedPartGet
                    {
                        Id = x.Id,
                        InspectedPartId = x.InspectedPart.Id,
                        CharacterLocation = x.Character,
                        Description = x.Description
                    }));

            var filesInfoDict = this.actCheckAnnexDomain.GetAll()
                .Where(x => actCheckIds.Contains(x.ActCheck.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActCheck.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new FileInfoGet
                    {
                        Id = x.Id,
                        FileId = x.File?.Id,
                        FileName = x.Name,
                        FileDate = x.DocumentDate,
                        FileDescription = x.Description
                    }));

            var signedFileDict = docGjiPdfSignInfoDomain.GetAll()
                    .Where(x => actCheckIds.Contains(x.DocumentGji.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DocumentGji.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(x => new SignedFileInfo
                        {
                            Id = x.PdfSignInfo.Id,
                            FileId = x.PdfSignInfo.SignedPdf?.Id,
                            FileName = x.PdfSignInfo.SignedPdf?.Name,
                            FileDate = x.PdfSignInfo.ObjectCreateDate
                        }));

            var entityDomain = this.Container.ResolveDomain<TServiceEntity>();
            using (this.Container.Using(entityDomain))
            {
                return entityDomain.GetAll()
                    .Where(x => actCheckIds.Contains(x.Id))
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var actCheckChildrenDocTypes = actCheckChildrenDocTypesDict.Get(x.Id)?
                            .Select(y => (ActCheckChildrenDocumentType)y)
                            .ToArray();

                        return new TOut
                        {
                            Id = x.Id,
                            ParentDocumentId = childrenDocumentsInfoDict.Get(x.Id),
                            InspectionId = x.Inspection?.Id ?? 0,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate,
                            Time = x.DocumentTime?.TimeOfDay,
                            InspectorIds = inspectorsDict.Get(x.Id),
                            DocumentPlace = x.DocumentPlaceFias?.CopyIdenticalProperties<FiasAddress>(),
                            Square = x.Area,
                            Flat = x.Flat,
                            ExecutiveId = x.Signer?.Id,
                            Witnesses = witnessesDict.Get(x.Id),
                            AcquaintedStatus = x.AcquaintState,
                            AcquaintedDate = x.AcquaintedDate,
                            OfficialFullName = x.AcquaintedPerson,
                            OfficialPosition = x.AcquaintedPersonTitle,
                            EventResults = eventResultsDict.Get(x.Id),
                            ProvidedDocuments = providedDocumentsDict.Get(x.Id),
                            InspectedParts = inspectedPartsDict.Get(x.Id),
                            Files = filesInfoDict.Get(x.Id),
                            SignedFiles = signedFileDict.Get(x.Id),
                        RelatedDocuments = actCheckChildrenDocTypes?.Any() ?? false,
                        ChildrenDocumentTypes = actCheckChildrenDocTypes
                    };
                }).ToArray();
            }
        }

        /// <summary>
        /// Перенос информации для <see cref="ActCheckWitness"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        protected TransferValues<TModel, ActCheckWitness> WitnessTransfer<TModel>()
            where TModel : BaseActCheckWitness =>
            (TModel model, ref ActCheckWitness actCheckWitness, object mainEntity) =>
            {
                if (actCheckWitness.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    actCheckWitness.ActCheck = mainEntity as ActCheck;
                }

                actCheckWitness.Fio = model.FullName;
                actCheckWitness.Position = model.Position;
                actCheckWitness.IsFamiliar = model.SignAcquainted;
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckRealityObject"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TViolation">Тип модели нарушений</typeparam>
        protected TransferValues<TModel, ActCheckRealityObject> EventResultTransfer<TModel, TViolation>()
            where TModel : BaseActCheckEventResult<TViolation>
            where TViolation : BaseActCheckViolation =>
            (TModel model, ref ActCheckRealityObject actCheckRealityObject, object mainEntity) =>
            {
                if (actCheckRealityObject.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    actCheckRealityObject.ActCheck = mainEntity as ActCheck;
                }

                actCheckRealityObject.RealityObject = new RealityObject { Id = (long)model.AddressId };
                actCheckRealityObject.Description = model.DescriptionViolations;
                actCheckRealityObject.HaveViolation = model.Violations.IsNotEmpty() ? YesNoNotSet.Yes : YesNoNotSet.No;

                var actCheckRo = actCheckRealityObject;

                this.UpdateNestedEntities(model.Violations,
                    x => x.ActObject.Id == actCheckRo.Id,
                    this.ViolationTransfer<TViolation>(),
                    actCheckRo,
                    this.NextOrder + 1);
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckViolation"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        protected TransferValues<TModel, ActCheckViolation> ViolationTransfer<TModel>()
            where TModel : BaseActCheckViolation =>
            (TModel model, ref ActCheckViolation actCheckViolation, object mainEntity) =>
            {
                this.EntityRefCheck(mainEntity);
                var actCheckRealityObject = mainEntity as ActCheckRealityObject;

                if (actCheckViolation.Id == 0)
                {
                    actCheckViolation.Document = actCheckRealityObject.ActCheck;
                    actCheckViolation.ActObject = actCheckRealityObject;
                    actCheckViolation.TypeViolationStage = TypeViolationStage.Detection;
                }

                if (actCheckViolation.InspectionViolation.IsNull())
                {
                    var newInspectionViolation = new InspectionGjiViol
                    {
                        Inspection = actCheckRealityObject.ActCheck.Inspection,
                        RealityObject = actCheckRealityObject.RealityObject
                    };

                    this.AddEntityToSave(newInspectionViolation);

                    actCheckViolation.InspectionViolation = newInspectionViolation;
                }

                actCheckViolation.InspectionViolation.Violation = new ViolationGji { Id = (long)model.ViolationId };
                actCheckViolation.DatePlanRemoval = model.TermElimination;
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheck"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TWitness">Тип модели свидетелей (присутствующие при проверке)</typeparam>
        /// <typeparam name="TEventResult">Тип информации о выявленных нарушениях</typeparam>
        /// <typeparam name="TProvidedDocument">Тип модели предоставленных документов</typeparam>
        /// <typeparam name="TInspectedPart">Тип модели инспектируемых частей</typeparam>
        /// <typeparam name="TFileInfo">Тип модели файлов-вложений</typeparam>
        /// <typeparam name="TViolation">Тип модели нарушений</typeparam>
        /// <typeparam name="TActCheck">Тип модели, в которую будут перенесены данные</typeparam>
        protected TransferValues<TModel, TActCheck> ActCheckTransfer<TModel, TActCheck, TWitness, TEventResult,
            TProvidedDocument, TInspectedPart, TFileInfo, TViolation>()
            where TModel : BaseDocumentActCheck<TWitness, TEventResult, TProvidedDocument, TInspectedPart, TFileInfo>
            where TActCheck : ActCheck
            where TEventResult : BaseActCheckEventResult<TViolation> =>
            (TModel model, ref TActCheck actCheck, object mainEntity) =>
            {
                var roIds = model.EventResults.Select(y => y.AddressId);
                var realityObjectsArea = this.realityObjectDomain.GetAll()
                    .Where(x => roIds.Contains(x.Id))
                    .Sum(x => x.AreaMkd);

                actCheck.DocumentDate = model.DocumentDate;
                actCheck.DocumentTime = new DateTime() + model.Time;
                actCheck.DocumentPlaceFias = model.DocumentPlace.GetFiasAddress(this.Container, actCheck.DocumentPlaceFias);
                actCheck.Area = realityObjectsArea;
                actCheck.Flat = model.Flat;
                actCheck.Signer = model.ExecutiveId.HasValue ? new Inspector { Id = model.ExecutiveId.Value } : null;
                actCheck.AcquaintState = model.AcquaintedStatus;
                actCheck.AcquaintedDate = model.AcquaintedDate;
                actCheck.AcquaintedPerson = model.OfficialFullName;
                actCheck.AcquaintedPersonTitle = model.OfficialPosition;
            };
        
        /// <inheritdoc />
        protected override async Task BeforeDeleteAsync(long documentId)
        {
            var documentExists = await this.DocumentGjiDomain.GetAll()
                .AnyAsync(x => x.Id == documentId);

            if (!documentExists)
                throw new ApiServiceException("Не найден документ с переданным идентификатором");

            this.DeletingQueryQueue.Enqueue(this.actCheckActionDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
            this.DeletingQueryQueue.Enqueue(this.actCheckRealityObjectDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
            this.DeletingQueryQueue.Enqueue(this.actCheckDefinitionDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
            this.DeletingQueryQueue.Enqueue(this.actCheckAnnexDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
        }
    }
}