namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActActionIsolated;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Сервис документа "Акт КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    public class DocumentActActionIsolatedService
        : BaseActCheckDocumentService<ActActionIsolated, DocumentActActionIsolatedGet, DocumentActActionIsolatedCreate, DocumentActActionIsolatedUpdate, BaseDocQueryParams>,
        IDocumentActActionIsolatedService
    {
        #region Constructor
        /// <inheritdoc />
        public DocumentActActionIsolatedService(IDomainService<ActCheckAnnex> actCheckAnnexDomain,
            IDomainService<ActCheckWitness> actCheckWitnessDomain,
            IDomainService<ActCheckProvidedDoc> actCheckProvidedDocDomain,
            IDomainService<ActCheckRealityObject> actCheckRealityObjectDomain,
            IDomainService<ActCheckInspectedPart> actCheckInspectedPartDomain,
            IDomainService<ActCheckViolation> actCheckViolationDomain,
            IDomainService<ActCheckAction> actCheckActionDomain,
            IDomainService<ActCheckDefinition> actCheckDefinitionDomain,
            IDomainService<RealityObject> realityObjectDomain,
            IDomainService<DocumentGjiPdfSignInfo> docGjiPdfSignInfoDomain)
            : base(actCheckAnnexDomain, actCheckWitnessDomain, actCheckProvidedDocDomain, actCheckRealityObjectDomain, actCheckInspectedPartDomain, 
                actCheckViolationDomain, actCheckActionDomain, actCheckDefinitionDomain, realityObjectDomain, docGjiPdfSignInfoDomain)
        {
        }
        #endregion
        
        /// <inheritdoc />
        protected override IEnumerable<DocumentActActionIsolatedGet> GetDocumentList(long? documentId = null, BaseDocQueryParams queryParams = null, params long[] parentDocumentIds) =>
            this.CommonGetDocumentList<DocumentActActionIsolatedGet>(
                TypeDocumentGji.ActActionIsolated,
                TypeDocumentGji.TaskActionIsolated,
                new[] { TypeDocumentGji.MotivatedPresentation },
                documentId,
                parentDocumentIds);

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(DocumentActActionIsolatedCreate createDocument)
        {
            this.AnnexEntityRefCheck<ActCheckAnnex, FileInfoCreate>(createDocument.Files);

            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();

            using (this.Container.Using(taskActionIsolatedDomain))
            {
                var parentDocument = taskActionIsolatedDomain.Get(createDocument.ParentDocumentId);

                if (parentDocument.IsNull() || parentDocument.Inspection.IsNull())
                    return null;

                var actActionIsolated = this.CreateEntity(
                    createDocument,
                    this.ActCheckTransfer<DocumentActActionIsolatedCreate, ActActionIsolated, ActCheckWitnessCreate, ActCheckEventResultCreate,
                        ActCheckProvidedDocumentCreate, ActCheckInspectedPartCreate, FileInfoCreate, ActCheckViolationCreate>(),
                    order: this.MainProcessOrder);
                
                actActionIsolated.Inspection = parentDocument.Inspection;
                actActionIsolated.TypeActCheck = TypeActCheckGji.ActActionIsolated;
                actActionIsolated.TypeDocumentGji = TypeDocumentGji.ActActionIsolated;
                actActionIsolated.DocumentNum = parentDocument.Inspection.InspectionNum;
                actActionIsolated.Stage = this.GetDocumentInspectionGjiStage(parentDocument, TypeStage.ActActionIsolated);

                this.CreateDocumentGjiChildren(parentDocument, actActionIsolated);
                this.CreateInspectors(createDocument.InspectorIds, actActionIsolated);

                this.CreateEntities(createDocument.Witnesses, this.WitnessTransfer<ActCheckWitnessCreate>(), actActionIsolated);
                this.CreateEntities(createDocument.EventResults, this.EventResultTransfer<ActCheckEventResultCreate, ActCheckViolationCreate>(), actActionIsolated);

                this.CreateAnnexEntities<FileInfoCreate, ActCheckAnnex>(createDocument.Files, nameof(ActCheckAnnex.ActCheck), actActionIsolated);

                return actActionIsolated;
            }
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long documentId, DocumentActActionIsolatedUpdate updateDocument)
        {
            var actActionIsolatedDomain = this.Container.ResolveDomain<ActActionIsolated>();

            using (this.Container.Using(actActionIsolatedDomain))
            {
                var actActionIsolated = actActionIsolatedDomain.Get(documentId);

                if (actActionIsolated.IsNull())
                    throw new ApiServiceException("Не найден документ для обновления");

                this.AnnexEntityRefCheck<ActCheckAnnex, FileInfoUpdate>(updateDocument.Files, x => x.ActCheck.Id == documentId);

                this.UpdateEntity(updateDocument, actActionIsolated,
                    this.ActCheckTransfer<DocumentActActionIsolatedUpdate, ActActionIsolated, ActCheckWitnessUpdate, ActCheckEventResultUpdate,
                        ActCheckProvidedDocumentUpdate, ActCheckInspectedPartUpdate, FileInfoUpdate, ActCheckViolationUpdate>());

                this.UpdateInspectors(updateDocument.InspectorIds, actActionIsolated);

                this.UpdateNestedEntities(
                    updateDocument.Witnesses,
                    x => x.ActCheck.Id == actActionIsolated.Id,
                    this.WitnessTransfer<ActCheckWitnessUpdate>(),
                    actActionIsolated);

                this.UpdateNestedEntities(
                    updateDocument.EventResults,
                    x => x.ActCheck.Id == actActionIsolated.Id,
                    this.EventResultTransfer<ActCheckEventResultUpdate, ActCheckViolationUpdate>(),
                    actActionIsolated);

                this.UpdateAnnexEntities<FileInfoUpdate, ActCheckAnnex>(
                    updateDocument.Files,
                    x => x.ActCheck.Id == actActionIsolated.Id,
                    nameof(ActCheckAnnex.ActCheck),
                    actActionIsolated);

                return actActionIsolated.Id;
            }
        }
    }
}