namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Protocol;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;

    /// <summary>
    /// Сервис документа "Протокол"
    /// </summary>
    public class DocumentProtocolService : DocumentWithParentService<Protocol, DocumentProtocolGet, DocumentProtocolCreate, DocumentProtocolUpdate, BaseDocQueryParams>,
        IDocumentProtocolService
    {
        #region DependencyInjection
        private readonly IDomainService<TatProtocol> protocolDomain;
        private readonly IDomainService<DocumentGjiPdfSignInfo> documentGjiPdfSignInfoDomain;

        ///<inheritdoc cref="DocumentProtocolService"/>
        public DocumentProtocolService(IDomainService<TatProtocol> protocolDomain, IDomainService<DocumentGjiPdfSignInfo> documentGjiPdfSignInfoDomain)
        {
            this.protocolDomain = protocolDomain;
            this.documentGjiPdfSignInfoDomain = documentGjiPdfSignInfoDomain;
        }
        #endregion

        /// <summary>
        /// Словарь с этапами нарушений
        /// </summary>
        private Dictionary<long, List<InspectionGjiViol>> inspectionGjiViolDict = new Dictionary<long, List<InspectionGjiViol>>();

        /// <summary>
        /// Перенос информации для <see cref="Protocol"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TViolation">Тип нарушений</typeparam>
        /// <typeparam name="TFileInfo">Тип файлов-приложений</typeparam>
        protected TransferValues<TModel, TatProtocol> ProtocolTransfer<TModel, TViolation, TFileInfo>()
            where TModel : BaseDocumentProtocol<TViolation, TFileInfo> =>
            (TModel model, ref TatProtocol protocol, object mainEntity) =>
            {
                protocol.Contragent = model.OrganizationId.HasValue
                    ? new Contragent { Id = model.OrganizationId.Value } : null;
                protocol.DocumentDate = model.DocumentDate;
                protocol.FormatPlace = model.TimeCompilation;
                protocol.DocumentPlace = model.DocumentPlace.GetFiasAddress(this.Container, protocol.DocumentPlace);
                protocol.NotifDeliveredThroughOffice = model.SignOffice.Value;
                protocol.FormatDate = model.DateNotification;
                protocol.NotifNumber = model.NumberRegistration;
                protocol.Executant = model.ExecutantId.HasValue
                    ? new ExecutantDocGji { Id = model.ExecutantId.Value } : null;
                protocol.PhysicalPerson = model.Individual;
                protocol.PhysicalPersonInfo = model.Requisites;
                protocol.ProceedingsPlace = model.PlaceReview;
            };

        /// <summary>
        /// Перенос информации для <see cref="ProtocolViolation"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        protected TransferValues<TModel, ProtocolViolation> ViolationTransfer<TModel>()
            where TModel : BaseProtocolViolation =>
            (TModel model, ref ProtocolViolation protocolViolation, object mainEntity) =>
            {
                var violationId = (long)model.ViolationId;

                if (protocolViolation.Id == 0 || protocolViolation.InspectionViolation.IsNull())
                {
                    this.EntityRefCheck(mainEntity);
                    var protocol = mainEntity as Protocol;

                    protocolViolation.Document = protocol;

                    try
                    {
                        protocolViolation.InspectionViolation = this.inspectionGjiViolDict.Get(violationId,
                            defaultValue: new List<InspectionGjiViol>()).Single();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new ApiServiceException("При определении этапа нарушения возникла ошибка");
                    }
                }

                protocolViolation.InspectionViolation.Violation = new ViolationGji { Id = violationId };
                protocolViolation.InspectionViolation.DateFactRemoval = model.DateViolation;
            };

        /// <summary>
        /// Перенос информации для <see cref="ProtocolArticleLaw"/>
        /// </summary>
        protected TransferValues<long, ProtocolArticleLaw> ArticleLawTransfer =>
            (long model, ref ProtocolArticleLaw protocolArticleLaw, object mainEntity) =>
            {
                if (protocolArticleLaw.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    protocolArticleLaw.Protocol = mainEntity as Protocol;
                }

                protocolArticleLaw.ArticleLaw = new ArticleLawGji { Id = model };
            };

        /// <inheritdoc />
        protected override IEnumerable<DocumentProtocolGet> GetDocumentList(long? documentId = null, BaseDocQueryParams queryParams = null, params long[] parentDocumentIds)
        {
            var protocolArticleLawDomain = this.Container.ResolveDomain<ProtocolArticleLaw>();
            var protocolViolationDomain = this.Container.ResolveDomain<ProtocolViolation>();
            var protocolAnnexDomain = this.Container.ResolveDomain<ProtocolAnnex>();

            var protocolService = this.Container.Resolve<IProtocolService>();

            using (this.Container.Using(
                protocolArticleLawDomain,
                protocolViolationDomain,
                protocolAnnexDomain,
                protocolService))
            {
                var parentDocTypes = new[]
                {
                    TypeDocumentGji.ActCheck,
                    TypeDocumentGji.Prescription
                };

                var formattedDocumentBaseDict = new Dictionary<long, string>();

                var childrenDocumentsInfoDict = this.DocumentGjiChildrenDomain.GetAll()
                    .Where(x => parentDocTypes.Contains(x.Parent.TypeDocumentGji))
                    .Where(x => !x.Children.State.FinalState &&
                        x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .WhereIfElse(!documentId.HasValue,
                        x => parentDocumentIds.Contains(x.Parent.Id),
                        x => x.Children.Id == documentId)
                    .Select(x => new
                    {
                        ParentId = x.Parent.Id,
                        ParentTypeDocumentGji = x.Parent.TypeDocumentGji,
                        ParentDocumentDate = x.Parent.DocumentDate,
                        ParentDocumentNumber = x.Parent.DocumentNumber,
                        ChildrenId = x.Children.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ChildrenId)
                    .ToDictionary(x => x.Key,
                        y =>
                        {
                            var first = y.First();
                            if (!formattedDocumentBaseDict.TryGetValue(first.ParentId, out var documentBase))
                            {
                                documentBase = protocolService.GetFormattedDocumentBase(new ProtocolParentDocInfo
                                {
                                    TypeDocumentGji = first.ParentTypeDocumentGji,
                                    DocumentDate = first.ParentDocumentDate,
                                    DocumentNumber = first.ParentDocumentNumber
                                });

                                formattedDocumentBaseDict.Add(first.ParentId, documentBase);
                            }

                            return new
                            {
                                first.ParentId,
                                DocumentBase = documentBase
                            };
                        });

                var protocolIds = childrenDocumentsInfoDict.Keys.ToArray();

                if (!protocolIds.Any())
                    return null;

                var inspectorsDict = this.DocumentGjiInspectorDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.DocumentGji.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DocumentGji.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Inspector.Id).ToArray());

                var protocolArticleLawsDict = protocolArticleLawDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.Protocol.Id))
                    .Where(x => x.ArticleLaw != null)
                    .AsEnumerable()
                    .GroupBy(x => x.Protocol.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(x => x.ArticleLaw?.Id).FirstOrDefault());

                var violationsDict = protocolViolationDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.Document.Id))
                    .Where(x => x.InspectionViolation != null && x.InspectionViolation.Violation != null)
                    .AsEnumerable()
                    .GroupBy(x => x.Document.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(x => new ProtocolViolationGet
                        {
                            Id = x.Id,
                            ViolationId = x.InspectionViolation.Violation.Id,
                            DateViolation = x.InspectionViolation.DateFactRemoval,
                            LawId = protocolArticleLawsDict.Get(x.Document.Id),
                            AddressId = x.InspectionViolation.RealityObject?.Id
                        }));

                var filesInfoDict = protocolAnnexDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.Protocol.Id))
                    .Where(x => x.File != null)
                    .AsEnumerable()
                    .GroupBy(x => x.Protocol.Id)
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
                    .Where(x => protocolIds.Contains(x.DocumentGji.Id))
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

                return this.protocolDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.Id))
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var childrenDocumentInfo = childrenDocumentsInfoDict.Get(x.Id);

                        return new DocumentProtocolGet
                        {
                            Id = x.Id,
                            ParentDocumentId = childrenDocumentInfo.ParentId,
                            InspectionId = x.Inspection?.Id ?? 0,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate,
                            TimeCompilation = x.FormatPlace,
                            DocumentPlace = x.DocumentPlace?.CopyIdenticalProperties<FiasAddress>(),
                            DocumentBase = childrenDocumentInfo.DocumentBase,
                            InspectorIds = inspectorsDict.Get(x.Id),
                            SignOffice = x.NotifDeliveredThroughOffice,
                            DateNotification = x.FormatDate,
                            NumberRegistration = x.NotifNumber,
                            ExecutantId = x.Executant?.Id,
                            OrganizationId = x.Contragent?.Id,
                            Individual = x.PhysicalPerson,
                            Requisites = x.PhysicalPersonInfo,
                            PlaceReview = x.ProceedingsPlace,
                            Violations = violationsDict.Get(x.Id),
                            Files = filesInfoDict.Get(x.Id),
                            SignedFiles = signedFileDict.Get(x.Id),
                            AddressId = violationsDict.Get(x.Id)?.FirstOrDefault()?.AddressId
                        };
                    })
                    .Where(x => x.ParentDocumentId > 0)
                    .Where(x => x.InspectionId > 0)
                    .ToArray();
            }
        }

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(DocumentProtocolCreate createDocument)
        {
            this.AnnexEntityRefCheck<ProtocolAnnex, FileInfoCreate>(createDocument.Files);

            var parentDocTypes = new[]
            {
                TypeDocumentGji.ActCheck,
                TypeDocumentGji.Prescription
            };

            var parentDocument = this.DocumentGjiDomain.Get(createDocument.ParentDocumentId);

            if (parentDocument.IsNull() ||
                parentDocument.Inspection.IsNull() ||
                !parentDocTypes.Contains(parentDocument.TypeDocumentGji))
                return null;

            var protocol = this.CreateEntity(
                createDocument,
                this.ProtocolTransfer<DocumentProtocolCreate, ProtocolViolationCreate, FileInfoCreate>(),
                order: this.MainProcessOrder);

            protocol.TypeDocumentGji = TypeDocumentGji.Protocol;
            protocol.Inspection = parentDocument.Inspection;
            protocol.Stage = this.GetDocumentInspectionGjiStage(parentDocument, TypeStage.Protocol);

            this.CreateDocumentGjiChildren(parentDocument, protocol);
            this.CreateInspectors(createDocument.InspectorIds, protocol);

            if (createDocument.Violations.IsNotNull() &&
                createDocument.Violations.Any())
            {
                var inspectionGjiViolStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();

                using (this.Container.Using(inspectionGjiViolStageDomain))
                {
                    var violationIds = createDocument.Violations
                        .Select(x => x.ViolationId);

                    this.inspectionGjiViolDict = inspectionGjiViolStageDomain.GetAll()
                        .Where(x => x.Document.Id == parentDocument.Id)
                        .Where(x => x.InspectionViolation != null && x.InspectionViolation.Violation != null &&
                            violationIds.Contains(x.InspectionViolation.Violation.Id))
                        .AsEnumerable()
                        .GroupBy(x => x.InspectionViolation.Violation)
                        .ToDictionary(x => x.Key.Id,
                            y => y.Select(x => x.InspectionViolation).ToList());
                }
            }

            this.CreateEntities(createDocument.Violations, this.ViolationTransfer<ProtocolViolationCreate>(), protocol);
            this.UpdateProtocolArticleLaws(createDocument.Violations, protocol);

            var annexEntities = this.CreateAnnexEntities<FileInfoCreate, ProtocolAnnex>(createDocument.Files, nameof(ProtocolAnnex.Protocol), protocol);
            annexEntities.Values.ForEach(x => x.SendFileToErknm = YesNoNotSet.NotSet);

            return protocol;
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long documentId, DocumentProtocolUpdate updateDocument)
        {
            var protocol = this.protocolDomain.Get(documentId);

            if (protocol.IsNull())
                throw new ApiServiceException("Не найден документ для обновления");

            this.AnnexEntityRefCheck<ProtocolAnnex, FileInfoUpdate>(updateDocument.Files, x => x.Protocol.Id == documentId);

            this.UpdateEntity(updateDocument, protocol,
                this.ProtocolTransfer<DocumentProtocolUpdate, ProtocolViolationUpdate, FileInfoUpdate>());

            this.UpdateInspectors(updateDocument.InspectorIds, protocol);

            this.UpdateNestedEntities(
                updateDocument.Violations,
                x => x.Document.Id == protocol.Id,
                this.ViolationTransfer<ProtocolViolationUpdate>(),
                protocol);

            this.UpdateProtocolArticleLaws(updateDocument.Violations, protocol);

            this.UpdateAnnexEntities<FileInfoUpdate, ProtocolAnnex>(
                updateDocument.Files,
                x => x.Protocol.Id == protocol.Id,
                nameof(ProtocolAnnex.Protocol),
                protocol);

            return protocol.Id;
        }

        /// <summary>
        /// Обновить статьи закона документа "Протокол"
        /// </summary>
        /// <param name="violations">Модели с информацией о выявленных нарушениях</param>
        /// <param name="protocol">Документ "Протокол"</param>
        /// <typeparam name="TViolation">Тип модели с информацией о выявленном нарушении</typeparam>
        private void UpdateProtocolArticleLaws<TViolation>(IEnumerable<TViolation> violations, Protocol protocol)
            where TViolation : BaseProtocolViolation =>
            this.CreateOrUpdateNestedEntities(
                violations.Where(x => x.LawId.HasValue)
                    .Distinct(x => x.LawId)
                    .Select(x => x.LawId.Value),
                x => x.Protocol.Id == protocol.Id,
                this.ArticleLawTransfer,
                protocol);
    }
}