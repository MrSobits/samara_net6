namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.MotivationPresentation;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Сервис для МП документа "Мотивированное представление"
    /// </summary>
    public class DocMotivatedPresentationService : DocumentWithParentService<MotivatedPresentation, MotivationPresentationGet,
        MotivationPresentationCreate, MotivationPresentationUpdate, BaseDocQueryParams>,
        IDocMotivatedPresentationService
    {
        private readonly IDomainService<MotivatedPresentation> motivationPresentationDomain;
        private readonly IDomainService<MotivatedPresentationRealityObject> motivatedPresentationRealityObjectDomain;
        private readonly IDomainService<MotivatedPresentationAnnex> motivatedPresentationAnnexDomain;
        private readonly IDomainService<MotivatedPresentationViolation> motivatedPresentationViolationDomain;
        private readonly IDomainService<DocumentGjiPdfSignInfo> documentGjiPdfSignInfoDomain;

        /// <inheritdoc cref="DocMotivatedPresentationService" />
        public DocMotivatedPresentationService(
            IDomainService<MotivatedPresentation> motivationPresentationDomain,
            IDomainService<MotivatedPresentationRealityObject> motivatedPresentationRealityObjectDomain,
            IDomainService<MotivatedPresentationAnnex> motivatedPresentationAnnexDomain,
            IDomainService<MotivatedPresentationViolation> motivatedPresentationViolationDomain,
            IDomainService<DocumentGjiPdfSignInfo> documentGjiPdfSignInfoDomain)
        {
            this.motivationPresentationDomain = motivationPresentationDomain;
            this.motivatedPresentationRealityObjectDomain = motivatedPresentationRealityObjectDomain;
            this.motivatedPresentationAnnexDomain = motivatedPresentationAnnexDomain;
            this.motivatedPresentationViolationDomain = motivatedPresentationViolationDomain;
            this.documentGjiPdfSignInfoDomain = documentGjiPdfSignInfoDomain;
        }

        /// <inheritdoc />
        protected override IEnumerable<MotivationPresentationGet> GetDocumentList(long? documentId = null, BaseDocQueryParams queryParams = null, long[] parentDocumentIds = null)
        {
            var childrenDocumentsInfoDict = this.DocumentGjiChildrenDomain.GetAll()
                .Where(x =>
                    x.Parent.TypeDocumentGji == TypeDocumentGji.ActActionIsolated &&
                    x.Children.TypeDocumentGji == TypeDocumentGji.MotivatedPresentation)
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

            var motivationPresentationIds = childrenDocumentsInfoDict.Keys.ToArray();

            if (!motivationPresentationIds.Any())
                return default;

            var inspectorsDict = this.DocumentGjiInspectorDomain.GetAll()
                .Where(x => motivationPresentationIds.Contains(x.DocumentGji.Id))
                .Where(x => x.Inspector != null)
                .AsEnumerable()
                .GroupBy(x => x.DocumentGji.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Inspector.Id).ToArray());

            var motivationPresentationViolationsDict = this.motivatedPresentationViolationDomain.GetAll()
                .Where(x => motivationPresentationIds.Contains(x.MotivatedPresentationRealityObject.MotivatedPresentation.Id))
                .Where(x => x.Violation != null)
                .AsEnumerable()
                .GroupBy(x => x.MotivatedPresentationRealityObject.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => x.Violation.Id));

            var eventResultsDict = this.motivatedPresentationRealityObjectDomain.GetAll()
                .Where(x => motivationPresentationIds.Contains(x.MotivatedPresentation.Id))
                .Where(x => x.RealityObject != null)
                .AsEnumerable()
                .GroupBy(x => x.MotivatedPresentation.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new MotivationPresentationEventResultGet
                    {
                        Id = x.Id,
                        AddressId = x.RealityObject.Id,
                        Violations = motivationPresentationViolationsDict.Get(x.Id),
                    }));

            var filesInfoDict = this.motivatedPresentationAnnexDomain.GetAll()
                .Where(x => motivationPresentationIds.Contains(x.MotivatedPresentation.Id))
                .Where(x => x.File != null)
                .AsEnumerable()
                .GroupBy(x => x.MotivatedPresentation.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new FileInfoGet
                    {
                        Id = x.Id,
                        FileId = x.File.Id,
                        FileName = x.Name,
                        FileDate = x.DocumentDate,
                        FileDescription = x.Description
                    }));

            var signedFileDict = documentGjiPdfSignInfoDomain.GetAll()
                .Where(x => motivationPresentationIds.Contains(x.DocumentGji.Id))
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

            return this.motivationPresentationDomain.GetAll()
                .Where(x => motivationPresentationIds.Contains(x.Id))
                .AsEnumerable()
                .Select(x => new MotivationPresentationGet
                {
                    Id = x.Id,
                    ParentDocumentId = childrenDocumentsInfoDict.Get(x.Id),
                    DocumentNumber = x.DocumentNumber,
                    DocumentDate = x.DocumentDate,
                    DocumentPlace = x.CreationPlace?.CopyIdenticalProperties<FiasAddress>(),
                    ExecutiveId = x.IssuedMotivatedPresentation?.Id,
                    ResponsibleExecutionId = x.ResponsibleExecution?.Id,
                    InspectorIds = inspectorsDict.Get(x.Id),
                    EventResults = eventResultsDict.Get(x.Id),
                    Files = filesInfoDict.Get(x.Id),
                    SignedFiles = signedFileDict.Get(x.Id)
                })
                .ToArray();
        }

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(MotivationPresentationCreate createDocument)
        {
            var actActionIsolatedDomain = this.Container.Resolve<IDomainService<ActActionIsolated>>();
            using (this.Container.Using(actActionIsolatedDomain))
            {
                this.AnnexEntityRefCheck<MotivatedPresentationAnnex, FileInfoCreate>(createDocument.Files);

                var parentAct = actActionIsolatedDomain.Get(createDocument.ParentDocumentId);

                var motivationPresentation = this.CreateEntity(
                    createDocument,
                    this.MotivatedPresentationTransfer<MotivationPresentationCreate, MotivationPresentationEventResultCreate, FileInfoCreate>(),
                    order: this.MainProcessOrder);

                motivationPresentation.Inspection = parentAct.Inspection;
                motivationPresentation.TypeDocumentGji = TypeDocumentGji.MotivatedPresentation;
                motivationPresentation.Stage = this.GetDocumentInspectionGjiStage(parentAct, TypeStage.MotivatedPresentation);

                this.CreateDocumentGjiChildren(parentAct, motivationPresentation);

                this.CreateInspectors(createDocument.InspectorIds, motivationPresentation);
                this.CreateEntities(createDocument.EventResults, this.EventResultTransfer<MotivationPresentationEventResultCreate>(), motivationPresentation);

                this.CreateAnnexEntities<FileInfoCreate, MotivatedPresentationAnnex>(createDocument.Files,
                    nameof(MotivatedPresentationAnnex.MotivatedPresentation),
                    motivationPresentation);

                return motivationPresentation;
            }
        }

        /// <summary>
        /// Перенос информации для <see cref="BaseMotivationPresentationEventResult"/>
        /// </summary>
        /// <typeparam name="TEventResult">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TEventResult, MotivatedPresentationRealityObject> EventResultTransfer<TEventResult>()
            where TEventResult : BaseMotivationPresentationEventResult =>
            (TEventResult model, ref MotivatedPresentationRealityObject motivatedPresentationRealityObject, object mainEntity) =>
            {
                var motivationPresentation = (MotivatedPresentation)mainEntity;
                var motivatedPresentationRo = motivatedPresentationRealityObject;
                
                motivatedPresentationRealityObject.RealityObject = new RealityObject { Id = model.AddressId.Value };
                motivatedPresentationRealityObject.MotivatedPresentation = motivationPresentation;

                this.UpdateNestedEntities(model.Violations,
                    x => x.MotivatedPresentationRealityObject.Id == motivatedPresentationRo.Id,
                    this.ViolationTransfer(),
                    motivatedPresentationRo);
            };

        /// <summary>
        /// Перенос информации для <see cref="MotivatedPresentationViolation"/>
        /// </summary>
        private TransferValues<long, MotivatedPresentationViolation> ViolationTransfer() =>
            (long model, ref MotivatedPresentationViolation motivatedPresentationViolation, object mainEntity) =>
            {
                if (motivatedPresentationViolation.Id == default)
                {
                    this.EntityRefCheck(mainEntity);
                    motivatedPresentationViolation.MotivatedPresentationRealityObject = (MotivatedPresentationRealityObject)mainEntity;
                }

                motivatedPresentationViolation.Violation = new ViolationGji { Id = model };
            };

        /// <summary>
        /// Перенос информации для <see cref="MotivatedPresentation"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TEventResult">Тип модели результатов проведения проверки</typeparam>
        /// <typeparam name="TFileInfo">Модель "Информация о файле"</typeparam>
        private TransferValues<TModel, MotivatedPresentation> MotivatedPresentationTransfer<TModel, TEventResult, TFileInfo>()
            where TModel : BaseMotivationPresentation<TEventResult, TFileInfo>
            where TEventResult : BaseMotivationPresentationEventResult
            where TFileInfo : BaseFileInfo =>
            (TModel model, ref MotivatedPresentation motivationPresentation, object mainEntity) =>
            {
                motivationPresentation.DocumentDate = model.DocumentDate;
                motivationPresentation.CreationPlace = model.DocumentPlace.GetFiasAddress(this.Container);
                motivationPresentation.IssuedMotivatedPresentation = model.ExecutiveId.HasValue
                    ? new Inspector { Id = model.ExecutiveId.Value }
                    : null;
                motivationPresentation.ResponsibleExecution = model.ResponsibleExecutionId.HasValue
                    ? new Inspector { Id = model.ResponsibleExecutionId.Value }
                    : null;
            };

        /// <inheritdoc />
        protected override long UpdateEntity(long documentId, MotivationPresentationUpdate updateDocument)
        {
            var motivationPresentation = this.motivationPresentationDomain.Get(documentId);

            if (motivationPresentation.IsNull())
                throw new ApiServiceException("Не найден документ для обновления");

            this.AnnexEntityRefCheck<MotivatedPresentationAnnex, FileInfoUpdate>(updateDocument.Files, x => x.MotivatedPresentation.Id == documentId);

            this.UpdateEntity(updateDocument,
                motivationPresentation,
                this.MotivatedPresentationTransfer<MotivationPresentationUpdate, MotivationPresentationEventResultUpdate, FileInfoUpdate>());

            this.UpdateInspectors(updateDocument.InspectorIds, motivationPresentation);

            this.UpdateNestedEntities(
                updateDocument.EventResults,
                x => x.MotivatedPresentation.Id == motivationPresentation.Id,
                this.EventResultTransfer<MotivationPresentationEventResultUpdate>(),
                motivationPresentation);

            this.UpdateAnnexEntities<FileInfoUpdate, MotivatedPresentationAnnex>(updateDocument.Files,
                x => x.MotivatedPresentation.Id == motivationPresentation.Id,
                nameof(MotivatedPresentationAnnex.MotivatedPresentation),
                motivationPresentation);

            return motivationPresentation.Id;
        }
    }
}