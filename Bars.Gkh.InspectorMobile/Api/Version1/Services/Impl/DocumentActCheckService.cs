namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
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
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// Сервис документа "Акт проверки"
    /// </summary>
    public class DocumentActCheckService : BaseActCheckDocumentService<ActCheck, DocumentActCheckGet, DocumentActCheckCreate, DocumentActCheckUpdate, BaseDocQueryParams>,
        IDocumentActCheckService
    {
        #region DependencyInjection
        private readonly IDomainService<ActCheck> actCheckDomain;
        private readonly IDomainService<ActCheckProvidedDoc> actCheckProvidedDocDomain;

        public DocumentActCheckService(IDomainService<ActCheck> actCheckDomain,
            IDomainService<ActCheckAction> actCheckActionDomain,
            IDomainService<ActCheckWitness> actCheckWitnessDomain,
            IDomainService<ActCheckRealityObject> actCheckRealityObjectDomain,
            IDomainService<ActCheckProvidedDoc> actCheckProvidedDocDomain,
            IDomainService<ActCheckInspectedPart> actCheckInspectedPartDomain,
            IDomainService<ActCheckDefinition> actCheckDefinitionDomain,
            IDomainService<ActCheckAnnex> actCheckAnnexDomain,
            IDomainService<DocumentGjiPdfSignInfo> docGjiPdfSignInfoDomain,
            IDomainService<ActCheckViolation> actCheckViolation,
            IDomainService<RealityObject> roDomain)
            : base(actCheckAnnexDomain, actCheckWitnessDomain, actCheckProvidedDocDomain, actCheckRealityObjectDomain, 
                actCheckInspectedPartDomain, actCheckViolation, actCheckActionDomain, actCheckDefinitionDomain, roDomain, docGjiPdfSignInfoDomain)
        {
            this.actCheckDomain = actCheckDomain;
            this.actCheckProvidedDocDomain = actCheckProvidedDocDomain;
        }
        #endregion

        /// <summary>
        /// Перенос информации для <see cref="ActCheckProvidedDoc"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        protected TransferValues<TModel, ActCheckProvidedDoc> ProvidedDocumentTransfer<TModel>()
            where TModel : BaseActCheckProvidedDocument =>
            (TModel model, ref ActCheckProvidedDoc actCheckProvidedDoc, object mainEntity) =>
            {
                if (actCheckProvidedDoc.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    actCheckProvidedDoc.ActCheck = mainEntity as ActCheck;
                }

                actCheckProvidedDoc.ProvidedDoc = new ProvidedDocGji { Id = (long)model.ProvidedDocumentId };
                actCheckProvidedDoc.DateProvided = model.ProvidedDocumentDate;
            };

        /// <summary>
        /// Перенос информации для <see cref="ActCheckInspectedPart"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        protected TransferValues<TModel, ActCheckInspectedPart> InspectedPartTransfer<TModel>()
            where TModel : BaseActCheckInspectedPart =>
            (TModel model, ref ActCheckInspectedPart actCheckInspectedPart, object mainEntity) =>
            {
                if (actCheckInspectedPart.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    actCheckInspectedPart.ActCheck = mainEntity as ActCheck;
                }

                actCheckInspectedPart.InspectedPart = new InspectedPartGji { Id = (long)model.InspectedPartId };
                actCheckInspectedPart.Character = model.CharacterLocation;
                actCheckInspectedPart.Description = model.Description;
            };

        /// <inheritdoc />
        protected override IEnumerable<DocumentActCheckGet> GetDocumentList(long? documentId = null, BaseDocQueryParams queryParams = null, params long[] parentDocumentIds)
            =>
            this.CommonGetDocumentList<DocumentActCheckGet>(
                TypeDocumentGji.ActCheck,
                TypeDocumentGji.Decision,
                new[] { TypeDocumentGji.Prescription, TypeDocumentGji.Protocol },
                documentId,
                parentDocumentIds);

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(DocumentActCheckCreate createDocument)
        {
            this.AnnexEntityRefCheck<ActCheckAnnex, FileInfoCreate>(createDocument.Files);

            var decisionDomain = this.Container.ResolveDomain<Decision>();

            using (this.Container.Using(decisionDomain))
            {
                var parentDocument = decisionDomain.Get(createDocument.ParentDocumentId);

                if (parentDocument.IsNull() || parentDocument.Inspection.IsNull())
                    return null;

                TypeActCheckGji typeActCheckGji;
                switch (parentDocument.TypeDisposal)
                {
                    case TypeDisposalGji.Base:
                        typeActCheckGji = TypeActCheckGji.ActCheckIndividual;
                        break;
                    case TypeDisposalGji.DocumentGji:
                        typeActCheckGji = TypeActCheckGji.ActCheckDocumentGji;
                        break;
                    default:
                        return default;
                }

                var actCheck = this.CreateEntity(
                    createDocument,
                    this.ActCheckTransfer<DocumentActCheckCreate, ActCheck, ActCheckWitnessCreate, ActCheckEventResultCreate,
                        ActCheckProvidedDocumentCreate, ActCheckInspectedPartCreate, FileInfoCreate, ActCheckViolationCreate>(),
                    order: this.MainProcessOrder);

                actCheck.Inspection = parentDocument.Inspection;
                actCheck.TypeActCheck = typeActCheckGji;
                actCheck.TypeDocumentGji = TypeDocumentGji.ActCheck;
                actCheck.DocumentNum = parentDocument.Inspection.InspectionNum;
                actCheck.Stage = this.GetDocumentInspectionGjiStage(parentDocument, TypeStage.ActCheck);

                this.CreateDocumentGjiChildren(parentDocument, actCheck);
                this.CreateInspectors(createDocument.InspectorIds, actCheck);

                this.CreateEntities(createDocument.Witnesses, this.WitnessTransfer<ActCheckWitnessCreate>(), actCheck);
                this.CreateEntities(createDocument.EventResults, this.EventResultTransfer<ActCheckEventResultCreate, ActCheckViolationCreate>(), actCheck);
                this.CreateEntities(createDocument.ProvidedDocuments, this.ProvidedDocumentTransfer<ActCheckProvidedDocumentCreate>(), actCheck);
                this.CreateEntities(createDocument.InspectedParts, this.InspectedPartTransfer<ActCheckInspectedPartCreate>(), actCheck);

                var annexEntities = this.CreateAnnexEntities<FileInfoCreate, ActCheckAnnex>(createDocument.Files, nameof(ActCheckAnnex.ActCheck), actCheck);
                annexEntities.Values.ForEach(x => x.SendFileToErknm = YesNoNotSet.NotSet);

                var parentDocIds = this.DocumentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == parentDocument.Id &&
                        x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Select(x => x.Parent.Id)
                    .ToArray();

                // Если родительский документ сформирован на основе предписаний
                // тогда создаем акты устранения нарушений
                if (parentDocIds.Any())
                {
                    var inspectionGjiViolStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();

                    using (this.Container.Using(inspectionGjiViolStageDomain))
                    {
                        var violations = inspectionGjiViolStageDomain.GetAll()
                            .Where(x => parentDocIds.Contains(x.Document.Id))
                            .Where(x => x.InspectionViolation.DateFactRemoval == null)
                            .Where(x => x.InspectionViolation != null && x.InspectionViolation.RealityObject != null)
                            .Select(x => new
                            {
                                DocumentId = x.Document.Id,
                                InspectionViolationId = x.InspectionViolation.Id,
                                RealityObjectId = x.InspectionViolation.RealityObject.Id,
                                x.InspectionViolation.RealityObject.AreaMkd,
                                x.DatePlanRemoval
                            })
                            .ToList();

                        var realityObjectsInfoDict = violations
                            .GroupBy(x => x.DocumentId)
                            .ToDictionary(x => x.Key,
                                y => y.Select(x => new
                                    {
                                        x.RealityObjectId,
                                        x.AreaMkd
                                    })
                                    .Distinct()
                                    .Sum(x => x.AreaMkd.ToDecimal()));

                        if (realityObjectsInfoDict.Any())
                        {
                            actCheck.Area = realityObjectsInfoDict.Values.Sum();
                        }

                        var violationsInfoDict = violations
                            .GroupBy(x => x.DocumentId)
                            .ToDictionary(x => x.Key,
                                y => y.Select(x => new
                                {
                                    x.InspectionViolationId,
                                    x.DatePlanRemoval
                                }));

                        var removalStage = new InspectionGjiStage
                        {
                            Inspection = actCheck.Inspection,
                            TypeStage = TypeStage.ActRemoval,
                            Parent = this.GetDocumentParentStage(parentDocument),
                            Position = actCheck.Stage.Position + 1
                        };

                        this.AddEntityToSave(removalStage, this.PreviousOrder);

                        foreach (var parentDocId in parentDocIds)
                        {
                            var actRemoval = new ActRemoval
                            {
                                Inspection = new InspectionGji { Id = actCheck.Inspection.Id },
                                DocumentDate = actCheck.DocumentDate,
                                TypeDocumentGji = TypeDocumentGji.ActRemoval,
                                Stage = removalStage,
                                TypeRemoval = YesNoNotSet.No,
                                Area = realityObjectsInfoDict.Get(parentDocId)
                            };

                            this.AddEntityToSave(actRemoval, this.MainProcessOrder);

                            this.CreateDocumentGjiChildren(parentDocId, actRemoval);
                            this.CreateDocumentGjiChildren(actCheck, actRemoval);

                            this.CreateInspectors(createDocument.InspectorIds, actRemoval);

                            if (violationsInfoDict.TryGetValue(parentDocId, out var violationsInfo))
                            {
                                var actRemovalViolations = violationsInfo
                                    .Select(x => new ActRemovalViolation
                                    {
                                        Document = actRemoval,
                                        InspectionViolation = new InspectionGjiViol { Id = x.InspectionViolationId },
                                        TypeViolationStage = TypeViolationStage.Removal,
                                        DatePlanRemoval = x.DatePlanRemoval
                                    });

                                this.AddEntitiesToSave(actRemovalViolations);
                            }
                            
                            this.NumberRegistrationQueue.Enqueue(actRemoval);
                        }
                    }
                }

                return actCheck;
            }
        }

        /// <inheritdoc/>
        protected override long UpdateEntity(long documentId, DocumentActCheckUpdate updateDocument)
        {
            var actCheck = this.actCheckDomain.Get(documentId);

            if (actCheck.IsNull())
                throw new ApiServiceException("Не найден документ для обновления");

            this.AnnexEntityRefCheck<ActCheckAnnex, FileInfoUpdate>(updateDocument.Files, x => x.ActCheck.Id == documentId);

            this.UpdateEntity(updateDocument, actCheck,
                this.ActCheckTransfer<DocumentActCheckUpdate, ActCheck, ActCheckWitnessUpdate, ActCheckEventResultUpdate,
                    ActCheckProvidedDocumentUpdate, ActCheckInspectedPartUpdate, FileInfoUpdate, ActCheckViolationUpdate>());

            this.UpdateInspectors(updateDocument.InspectorIds, actCheck);

            this.UpdateNestedEntities(
                updateDocument.Witnesses,
                x => x.ActCheck.Id == actCheck.Id,
                this.WitnessTransfer<ActCheckWitnessUpdate>(),
                actCheck);

            this.UpdateNestedEntities(
                updateDocument.EventResults,
                x => x.ActCheck.Id == actCheck.Id,
                this.EventResultTransfer<ActCheckEventResultUpdate, ActCheckViolationUpdate>(),
                actCheck);

            this.UpdateNestedEntities(
                updateDocument.ProvidedDocuments,
                x => x.ActCheck.Id == actCheck.Id,
                this.ProvidedDocumentTransfer<ActCheckProvidedDocumentUpdate>(),
                actCheck);

            this.UpdateNestedEntities(
                updateDocument.InspectedParts,
                x => x.ActCheck.Id == actCheck.Id,
                this.InspectedPartTransfer<ActCheckInspectedPartUpdate>(),
                actCheck);

            this.UpdateAnnexEntities<FileInfoUpdate, ActCheckAnnex>(
                updateDocument.Files,
                x => x.ActCheck.Id == actCheck.Id,
                nameof(ActCheckAnnex.ActCheck),
                actCheck);

            return actCheck.Id;
        }
        
        /// <inheritdoc />
        protected override async Task BeforeDeleteAsync(long documentId)
        {
            await base.BeforeDeleteAsync(documentId);

            this.DeletingQueryQueue.Enqueue(this.ActCheckWitnessDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
            this.DeletingQueryQueue.Enqueue(this.actCheckProvidedDocDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
            this.DeletingQueryQueue.Enqueue(this.ActCheckInspectedPartDomain
                .GetAll()
                .Where(x => x.ActCheck.Id == documentId));
        }
    }
}