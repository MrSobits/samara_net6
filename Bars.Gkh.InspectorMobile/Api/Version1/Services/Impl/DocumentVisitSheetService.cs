namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.VisitSheet;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// API сервис для <see cref="VisitSheet"/>
    /// </summary>
    public class DocumentVisitSheetService : DocumentWithParentService<VisitSheet, DocumentVisitSheetGet, DocumentVisitSheetCreate,
        DocumentVisitSheetUpdate, BaseDocQueryParams>, IDocumentVisitSheetService
    {
        private readonly IDomainService<PreventiveActionTask> _preventiveActionTaskDomain;
        private readonly IDomainService<VisitSheet> _visitSheetDomain;
        private readonly IDomainService<VisitSheetInfo> _visitSheetInfoDomain;
        private readonly IDomainService<VisitSheetAnnex> _visitSheetAnnexDomain;
        private readonly IDomainService<DocumentGjiPdfSignInfo> _documentGjiPdfSignInfoDomain;

        /// /// <summary>
        /// API сервис для <see cref="VisitSheet"/>
        /// </summary>
        public DocumentVisitSheetService(
            IDomainService<PreventiveActionTask> preventiveActionTaskDomain,
            IDomainService<VisitSheet> visitSheetDomain,
            IDomainService<VisitSheetInfo> visitSheetInfoDomain,
            IDomainService<VisitSheetAnnex> visitSheetAnnexDomain,
            IDomainService<DocumentGjiChildren> documentLinkDomain,
            IDomainService<DocumentGjiPdfSignInfo> documentGjiPdfSignInfoDomain)
        {
            _preventiveActionTaskDomain = preventiveActionTaskDomain;
            _visitSheetDomain = visitSheetDomain;
            _visitSheetAnnexDomain = visitSheetAnnexDomain;
            _visitSheetInfoDomain = visitSheetInfoDomain;
            _documentGjiPdfSignInfoDomain = documentGjiPdfSignInfoDomain;
        }
        
        /// <inheritdoc />
        protected override IEnumerable<DocumentVisitSheetGet> GetDocumentList(long? documentId = null, BaseDocQueryParams queryParams = null, params long[] parentDocumentIds)
        {
            return _visitSheetDomain.GetAll()
                .Join(this.DocumentGjiChildrenDomain.GetAll(),
                    x => x.Id,
                    x => x.Children.Id,
                    (x, y) => new
                    {
                        VisitSheet = x,
                        Task = y.Parent
                    }
                )
                .Where(x => x.Task.TypeDocumentGji == TypeDocumentGji.PreventiveActionTask)
                .Where(x => x.VisitSheet.ExecutingInspector != null)
                .WhereIfElse(documentId != null, x => x.VisitSheet.Id == documentId,
                    x => parentDocumentIds.Contains(x.Task.Id))
                .SelectMany(x => _visitSheetInfoDomain.GetAll()
                        .Where(y => y.VisitSheet.Id == x.VisitSheet.Id).DefaultIfEmpty(),
                    (x, y) => new
                    {
                        ParentId = x.Task.Id,
                        x.VisitSheet,
                        ProvidedInfo = y != null
                            ? new VisitSheetInformationProvidedGet
                            {
                                Id = y.Id,
                                Information = y.Info,
                                Comment = y.Comment
                            } : null
                    }
                )
                .SelectMany(x => this._visitSheetAnnexDomain.GetAll()
                        .Where(y => y.VisitSheet.Id == x.VisitSheet.Id).DefaultIfEmpty(),
                    (x, y) => new
                    {
                        x.ParentId,
                        x.VisitSheet,
                        x.ProvidedInfo,
                        Annex = y != null
                            ? new FileInfoGet
                            {
                                Id = y.Id,
                                FileId = y.File.Id,
                                FileDate = y.DocumentDate,
                                FileDescription = y.Description,
                                FileName = y.Name
                            } : null
                    }
                )
                .SelectMany(x => this._documentGjiPdfSignInfoDomain.GetAll()
                        .Where(y => y.DocumentGji.Id == x.VisitSheet.Id).DefaultIfEmpty(),
                    (x, y) => new
                    {
                        x.ParentId,
                        x.VisitSheet,
                        x.ProvidedInfo,
                        x.Annex,
                        SignedFileInfo = y != null
                            ? new SignedFileInfo
                            {
                                Id = y.PdfSignInfo.Id,
                                FileId = y.PdfSignInfo.SignedPdf.Id,
                                FileName = y.PdfSignInfo.SignedPdf.Name,
                                FileDate = y.PdfSignInfo.ObjectCreateDate
                            } : null
                    })
                .AsEnumerable()
                .GroupBy(x => new { x.ParentId, x.VisitSheet },
                    (x, y) => new
                    {
                        x.ParentId,
                        x.VisitSheet,
                        InformationProvided = y
                            .Where(z => z.ProvidedInfo != null)
                            .DistinctBy(z => z.ProvidedInfo.Id)
                            .Select(z => z.ProvidedInfo),
                        Files = y
                            .Where(z => z.Annex != null)
                            .DistinctBy(z => z.Annex.Id)
                            .Select(z => z.Annex),
                        SignedFileInfo = y
                            .Where(z => z.SignedFileInfo != null)
                            .DistinctBy(z => z.SignedFileInfo.Id)
                            .Select(z => z.SignedFileInfo)
                    })
                .Select(x => new DocumentVisitSheetGet
                {
                    Id = x.VisitSheet.Id,
                    InspectionId = x.VisitSheet.Inspection.Id,
                    ParentDocumentId = x.ParentId,
                    DocumentNumber = x.VisitSheet.DocumentNumber,
                    DocumentDate = x.VisitSheet.DocumentDate,
                    StartDate = x.VisitSheet.VisitDateStart,
                    EndDate = x.VisitSheet.VisitDateEnd,
                    StartTime = x.VisitSheet.VisitTimeStart,
                    EndTime = x.VisitSheet.VisitTimeEnd,
                    InspectorId = x.VisitSheet.ExecutingInspector.Id,
                    SignReceipt = x.VisitSheet.HasCopy == YesNoNotSet.Yes,
                    InformationProvided = x.InformationProvided.ToArray(),
                    Files = x.Files.ToArray(),
                    SignedFiles = x.SignedFileInfo.ToArray()
                });
        }

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(DocumentVisitSheetCreate createDocument)
        {
            this.AnnexEntityRefCheck<VisitSheetAnnex, FileInfoCreate>(createDocument.Files);

            var parentDocument = _preventiveActionTaskDomain.Get(createDocument.ParentDocumentId);

            if (parentDocument.IsNull() || parentDocument.Inspection.IsNull())
                return null;

            var visitSheet = this.CreateEntity(createDocument,
                this.VisitSheetTransfer<DocumentVisitSheetCreate, VisitSheetInformationProvidedCreate, FileInfoCreate>(),
                order: this.MainProcessOrder);

            visitSheet.TypeDocumentGji = TypeDocumentGji.VisitSheet;
            visitSheet.Inspection = parentDocument.Inspection;
            visitSheet.Stage = this.GetDocumentInspectionGjiStage(parentDocument, TypeStage.VisitSheet);

            this.CreateDocumentGjiChildren(parentDocument, visitSheet);

            this.CreateEntities(createDocument.InformationProvided, this.VisitSheetInformationProvidedTransfer<VisitSheetInformationProvidedCreate>(), visitSheet);

            this.CreateAnnexEntities<FileInfoCreate, VisitSheetAnnex>(createDocument.Files, nameof(VisitSheetAnnex.VisitSheet), visitSheet);

            return visitSheet;
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long documentId, DocumentVisitSheetUpdate updateDocument)
        {
            var visitSheet = this._visitSheetDomain.Get(documentId);

            if (visitSheet.IsNull())
            {
                throw new ApiServiceException("Не найден документ для обновления");
            }

            this.AnnexEntityRefCheck<VisitSheetAnnex, FileInfoUpdate>(updateDocument.Files, x => x.VisitSheet.Id == documentId);

            this.UpdateEntity(updateDocument, visitSheet, this.VisitSheetTransfer<DocumentVisitSheetUpdate, VisitSheetInformationProvidedUpdate, FileInfoUpdate>());
            this.UpdateInspector(updateDocument.InspectorId.Value, visitSheet);
            this.UpdateNestedEntities(updateDocument.InformationProvided,
                x => x.VisitSheet.Id == visitSheet.Id,
                this.VisitSheetInformationProvidedTransfer<VisitSheetInformationProvidedUpdate>(),
                visitSheet);
            this.UpdateAnnexEntities<FileInfoUpdate, VisitSheetAnnex>(updateDocument.Files,
                x => x.VisitSheet.Id == visitSheet.Id,
                nameof(VisitSheetAnnex.VisitSheet),
                visitSheet);

            return visitSheet.Id;
        }

        /// <summary>
        /// Перенос информации для <see cref="VisitSheet"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TInformationProvided">Тип информации о предоставленных данных</typeparam>
        /// <typeparam name="TFileInfo">Тип файлов-приложений</typeparam>
        private TransferValues<TModel, VisitSheet> VisitSheetTransfer<TModel, TInformationProvided, TFileInfo>()
            where TModel : BaseDocumentVisitSheet<TInformationProvided, TFileInfo>
            where TInformationProvided : BaseVisitSheetInformationProvided
            where TFileInfo : BaseFileInfo =>
            (TModel model, ref VisitSheet visitSheet, object mainEntity) =>
            {
                visitSheet.DocumentDate = model.DocumentDate;
                visitSheet.VisitDateStart = model.StartDate;
                visitSheet.VisitDateEnd = model.EndDate;
                visitSheet.VisitTimeStart = model.StartTime;
                visitSheet.VisitTimeEnd = model.EndTime;
                visitSheet.ExecutingInspector = model.InspectorId.HasValue
                    ? new Inspector { Id = model.InspectorId.Value } : null;
                visitSheet.HasCopy = model.SignReceipt.HasValue && model.SignReceipt.Value
                    ? YesNoNotSet.Yes : YesNoNotSet.No;
            };

        /// <summary>
        /// Перенос информации для <see cref="VisitSheetInfo"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, VisitSheetInfo> VisitSheetInformationProvidedTransfer<TModel>()
            where TModel : BaseVisitSheetInformationProvided =>
            (TModel model, ref VisitSheetInfo visitSheetInfo, object mainEntity) =>
            {
                if (visitSheetInfo.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    visitSheetInfo.VisitSheet = mainEntity as VisitSheet;
                }

                visitSheetInfo.Info = model.Information;
                visitSheetInfo.Comment = model.Comment;
            };
    }
}