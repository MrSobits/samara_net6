namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using NHibernate.Linq;

    /// <summary>
    /// API сервис для <see cref="Prescription"/>
    /// </summary>
    public class DocumentPrescriptionService : DocumentWithParentService<Prescription, DocumentPrescriptionGet, DocumentPrescriptionCreate, DocumentPrescriptionUpdate, BaseDocQueryParams>, IDocumentPrescriptionService
    {
        /// <summary>
        /// Словарь плановых дат устарнения нарушений из акта проверки
        /// </summary>
        private Dictionary<string, DateTime?> _datePlanRemovalDict;

        /// <summary>
        /// Идентификатор дома решения
        /// </summary>
        private long _prescriptionAddressId;

        /// <summary>
        /// Идентификатор акта проверки
        /// </summary>
        private long _actCheckId;
        
        #region DomainServices
        private readonly IDomainService<PrescriptionViol> _prescriptionViolDomain;
        private readonly IDomainService<PrescriptionCloseDoc> _prescriptionCloseDocDomain;
        private readonly IDomainService<PrescriptionAnnex> _prescriptionAnnexDomain;
        private readonly IDomainService<DocumentGjiChildren> _documentChildrenDomain;
        private readonly IDomainService<Prescription> _prescriptionDomain;
        private readonly IDomainService<ActCheck> _actCheckDomain;
        private readonly IDomainService<ActCheckViolation> _actCheckViolDomain;
        private IDomainService<DocumentGjiPdfSignInfo> _documentGjiPdfSignInfoDomain;
        #endregion

        /// <inheritdoc cref="DocumentPrescriptionService" />
        public DocumentPrescriptionService( 
            IDomainService<PrescriptionViol> prescriptionViolDomain,
            IDomainService<PrescriptionCloseDoc> prescriptionCloseDocDomain, 
            IDomainService<PrescriptionAnnex> prescriptionAnnexDomain,
            IDomainService<DocumentGjiChildren> documentChildrenDomain,
            IDomainService<Prescription> prescriptionDomain,
            IDomainService<ActCheck> actCheckDomain,
            IDomainService<ActCheckViolation> actCheckViolDomain,
            IDomainService<DocumentGjiPdfSignInfo> documentGjiPdfSignInfoDomain)
        {
            _prescriptionViolDomain = prescriptionViolDomain;
            _prescriptionCloseDocDomain = prescriptionCloseDocDomain;
            _prescriptionAnnexDomain = prescriptionAnnexDomain;
            _documentChildrenDomain = documentChildrenDomain;
            _prescriptionDomain = prescriptionDomain;
            _actCheckDomain = actCheckDomain;
            _actCheckViolDomain = actCheckViolDomain;
            _documentGjiPdfSignInfoDomain = documentGjiPdfSignInfoDomain;
        }

        /// <inheritdoc />
        // ToDo: Убрать после перехода на асинхронные методы 
        protected override IEnumerable<DocumentPrescriptionGet> GetDocumentList(long? documentId = null, BaseDocQueryParams queryParams = null, params long[] parentDocumentIds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override async Task<IEnumerable<DocumentPrescriptionGet>> GetDocumentListAsync(long? documentId = null, BaseDocQueryParams queryParams = null, params long[] parentDocumentIds)
        {
            var inspectionViolDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();
            var documentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();

            if (documentId == null && parentDocumentIds == null)
            {
                return null;
            }

            using (this.Container.Using(inspectionViolDomain, documentInspectorDomain))
            {
                var dataList = await _prescriptionDomain.GetAll()
                    .Join(_documentChildrenDomain.GetAll(),
                        prescription => prescription.Id,
                        link => link.Children.Id,
                        (prescription, link) => new
                        {
                            Prescription = prescription,
                            Act = link.Parent
                        })
                    .Where(x => x.Act.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => !x.Prescription.State.FinalState)
                    .Join(_actCheckDomain.GetAll(),
                        x => x.Act.Id,
                        act => act.Id,
                        (x, act) => new
                        {
                            x.Prescription,
                            Act = new
                            {
                                act.DocumentNumber,
                                act.DocumentDate,
                                act.Id,
                                DocumentPlace = act.DocumentPlaceFias != null
                                    ? new FiasAddress
                                    {
                                        PlaceAddressName = act.DocumentPlaceFias.AddressName,
                                        PlaceGuidId = act.DocumentPlaceFias.PlaceGuidId,
                                        PlaceName = act.DocumentPlaceFias.PlaceName,
                                        StreetCode = act.DocumentPlaceFias.StreetCode,
                                        StreetGuidId = act.DocumentPlaceFias.StreetGuidId,
                                        StreetName = act.DocumentPlaceFias.StreetName,
                                        HouseGuid = act.DocumentPlaceFias.HouseGuid,
                                        House = act.DocumentPlaceFias.House,
                                        Housing = act.DocumentPlaceFias.Housing,
                                        PostCode = act.DocumentPlaceFias.PostCode,
                                        PlaceCode = act.DocumentPlaceFias.PlaceCode
                                    }
                                    : null
                            }
                        })
                    .SelectMany(x => inspectionViolDomain.GetAll()
                            .Join(_prescriptionViolDomain.GetAll(),
                                  inspViol => inspViol.Id,
                                  presViol => presViol.Id,
                                  (inspViol, presViol) => new
                                  {
                                      Viol = inspViol,
                                      presViol.Action
                                  })
                            .Where(y => y.Viol.Document.Id == x.Prescription.Id).DefaultIfEmpty(),
                        (x, y) => new
                        {
                            x.Prescription,
                            x.Act,
                            Viol = y.Viol != null
                                ? new PrescriptionViolationGet
                                {
                                    Id = y.Viol.Id,
                                    ViolationId = y.Viol.InspectionViolation.Violation.Id,
                                    TermElimination = y.Viol.DatePlanRemoval ?? y.Viol.InspectionViolation.DatePlanRemoval,
                                    DateFactRemoval = y.Viol.DateFactRemoval ?? y.Viol.InspectionViolation.DateFactRemoval,
                                    Amount = y.Viol.SumAmountWorkRemoval,
                                    Event = y.Action
                                }
                                : null,
                            AddressId = y.Viol != null
                                ? y.Viol.InspectionViolation.RealityObject.Id
                                : (long?)null
                        })
                    .SelectMany(x => _prescriptionAnnexDomain.GetAll()
                            .Where(annex => annex.Prescription.Id == x.Prescription.Id).DefaultIfEmpty(),
                        (x, annex) => new
                        {
                            x.Act,
                            x.Prescription,
                            x.Viol,
                            x.AddressId,
                            Annex = annex != null
                                ? new PrescriptionAnnexGet
                                {
                                    Id = annex.Id,
                                    FileId = annex.File.Id,
                                    FileName = annex.Name,
                                    FileDate = annex.DocumentDate,
                                    FileDescription = annex.Description
                                }
                                : null
                        })
                    .SelectMany(x => _prescriptionCloseDocDomain.GetAll()
                            .Where(closeDoc => closeDoc.Prescription.Id == x.Prescription.Id).DefaultIfEmpty(),
                        (x, closeDoc) => new
                        {
                            x.Act,
                            x.Prescription,
                            x.Viol,
                            x.AddressId,
                            x.Annex,
                            CloseDoc = closeDoc != null
                                ? new PrescriptionClosingDocumentGet
                                {
                                    Id = closeDoc.Id,
                                    FileId = closeDoc.File.Id,
                                    FileName = closeDoc.Name,
                                    FileDate = closeDoc.Date,
                                    FileType = closeDoc.DocType
                                }
                                : null
                        })
                    .SelectMany(x => documentInspectorDomain.GetAll()
                            .Where(docInsp => docInsp.DocumentGji.Id == x.Prescription.Id).DefaultIfEmpty(),
                        (x, docInsp) => new
                        {
                            x.Act,
                            x.Prescription,
                            x.Viol,
                            x.AddressId,

                            x.Annex,
                            x.CloseDoc,
                            InspectorId = docInsp != null ? docInsp.Inspector.Id : (long?)null
                        })
                    .SelectMany(x => _documentChildrenDomain.GetAll()
                            .Where(link => link.Parent.Id == x.Prescription.Id).DefaultIfEmpty(),
                        (x, link) => new
                        {
                            x.Act,
                            x.Prescription,
                            x.Viol,
                            x.AddressId,
                            x.Annex,
                            x.CloseDoc,
                            x.InspectorId,
                            RelatedDoc = link != null && !link.Children.State.FinalState
                        })
                    .SelectMany(x => _documentGjiPdfSignInfoDomain.GetAll()
                        .Where(pdfSign => pdfSign.DocumentGji.Id == x.Prescription.Id),
                    (x, pdfSign) => new
                    {
                        x.Act,
                        x.Prescription,
                        x.Viol,
                        x.AddressId,
                        x.Annex,
                        x.CloseDoc,
                        x.InspectorId,
                        x.RelatedDoc,
                        SignedFile = pdfSign != null
                            ? new SignedFileInfo
                            {
                                Id = pdfSign.PdfSignInfo.Id,
                                FileId = pdfSign.PdfSignInfo.SignedPdf.Id,
                                FileName = pdfSign.PdfSignInfo.SignedPdf.Name,
                                FileDate = pdfSign.PdfSignInfo.ObjectCreateDate
                            }
                            : null
                    })
                    .WhereIf(documentId.HasValue, x => x.Prescription.Id == documentId)
                    .WhereIf(parentDocumentIds?.Any() ?? false, x => parentDocumentIds.Contains(x.Act.Id))
                    .ToListAsync();
                    
                    return dataList.GroupBy(x => x.Prescription.Id,
                        (x, y) => new
                        {
                            Prescription = y.Select(z => z.Prescription).First(),
                            Act = y.Select(z => z.Act).First(),
                            AddressId = y.Select(z => z.AddressId).First(),
                            Violations = y.Where(z => z.Viol != null).DistinctBy(z => z.Viol.Id).Select(z => z.Viol),
                            Annexes = y.Where(z => z.Annex != null).DistinctBy(z => z.Annex.Id).Select(z => z.Annex),
                            CloseDocs = y.Where(z => z.CloseDoc != null).DistinctBy(z => z.CloseDoc.Id).Select(z => z.CloseDoc),
                            y.First().InspectorId,
                            RelatedDocs = y.First().RelatedDoc,
                            SignedFiles = y.Where(z => z.SignedFile != null).DistinctBy(z => z.SignedFile.Id).Select(z => z.SignedFile)
                        })
                    .Select(x => new DocumentPrescriptionGet
                    {
                        Id = x.Prescription.Id,
                        ParentDocumentId = x.Act.Id,
                        InspectionId = x.Prescription.Inspection.Id,
                        DocumentNumber = x.Prescription.DocumentNumber,
                        DocumentDate = x.Prescription.DocumentDate,
                        AddressId = x.AddressId,
                        DocumentBase = $"Акт проверки №{x.Act.DocumentNumber} от {x.Act.DocumentDate}",
                        InspectorId = x.InspectorId,
                        Note = x.Prescription.Description,
                        ExecutantId = x.Prescription.Executant?.Id,
                        OrganizationId = x.Prescription.Contragent?.Id,
                        Individual = x.Prescription.PhysicalPerson,
                        Requisites = x.Prescription.PhysicalPersonInfo,
                        PrescriptionClosed = x.Prescription.Closed,
                        Cause = x.Prescription.CloseReason,
                        ClosingNote = x.Prescription.CloseNote,
                        RelatedDocuments = x.RelatedDocs,
                        DocumentPlace = x.Act.DocumentPlace,
                        Violations = x.Violations.ToArray(),
                        PrescriptionClosingDocuments = x.CloseDocs.ToArray(),
                        Files = x.Annexes.ToArray(),
                        SignedFiles = x.SignedFiles
                    });
            }
        }

        /// <inheritdoc />
        protected override PersistentObject CreateEntity(DocumentPrescriptionCreate createDocument)
        {
            this.AnnexEntityRefCheck<PrescriptionAnnex, FileInfoCreate>(createDocument.Files);

            var actCheck = _actCheckDomain.Get(createDocument.ParentDocumentId);
            var prescription = this.CreateEntity(
                createDocument,
                this.PrescriptionTransfer<DocumentPrescriptionCreate, PrescriptionClosingDocumentCreate, PrescriptionViolationCreate, PrescriptionAnnexCreate>(),
                order: this.MainProcessOrder);
            
            prescription.Inspection = actCheck.Inspection;
            prescription.TypeDocumentGji = TypeDocumentGji.Prescription;
            prescription.Stage = this.GetDocumentInspectionGjiStage(actCheck, TypeStage.Prescription);
            
            this.CreateDocumentGjiChildren(actCheck, prescription);
            this.CreateInspector(createDocument.InspectorId.Value, prescription);

            this._actCheckId = actCheck.Id;
            this._prescriptionAddressId = createDocument.AddressId.Value;
            this.FillDatePlanRemovalDict(actCheck.Id);

            this.CreateEntities(createDocument.Violations, ViolationTransfer<PrescriptionViolationCreate>(), prescription);
            this.CreateEntities(createDocument.PrescriptionClosingDocuments, ClosingDocumentTransfer<PrescriptionClosingDocumentCreate>(), prescription);
            this.CreateAnnexEntities<PrescriptionAnnexCreate, PrescriptionAnnex>(createDocument.Files, nameof(PrescriptionAnnex.Prescription), prescription);

            return prescription;
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long documentId, DocumentPrescriptionUpdate documentData)
        {
            var prescription = _prescriptionDomain.Get(documentId);

            if (prescription.IsNull())
                throw new ApiServiceException("Не найден документ для обновления");

            this.AnnexEntityRefCheck<PrescriptionAnnex, FileInfoUpdate>(documentData.Files, x => x.Prescription.Id == documentId);

            this._actCheckId = this._documentChildrenDomain.GetAll().Where(x => x.Children.Id == documentId)
                .Select(x => x.Parent.Id)
                .First();
            this.FillDatePlanRemovalDict(_actCheckId);
            this._prescriptionAddressId = this._prescriptionViolDomain.GetAll()
                .Where(x => x.Document.Id == documentId)
                .Select(x => x.InspectionViolation.RealityObject.Id)
                .First();

            this.UpdateEntity(documentData, prescription,
                this.PrescriptionTransfer<DocumentPrescriptionUpdate, PrescriptionClosingDocumentUpdate, PrescriptionViolationUpdate,
                    FileInfoUpdate>());

            this.UpdateInspector(documentData.InspectorId.Value, prescription);

            this.UpdateNestedEntities(
                documentData.Violations,
                x => x.Document.Id == documentId,
                this.ViolationTransfer<PrescriptionViolationUpdate>(),
                prescription);
                
            this.UpdateNestedEntities(
                documentData.PrescriptionClosingDocuments,
                x => x.Prescription.Id == documentId,
                this.ClosingDocumentTransfer<PrescriptionClosingDocumentUpdate>(),
                prescription);
            
            this.UpdateAnnexEntities<FileInfoUpdate, PrescriptionAnnex>(
                documentData.Files,
                x => x.Prescription.Id == documentId,
                nameof(PrescriptionAnnex.Prescription),
                prescription);

            return documentId;
        }

        /// <summary>
        /// Перенос информации для <see cref="Prescription"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TClosingDoc">Тип документа по закрытию предписания</typeparam>
        /// <typeparam name="TViol">Тип документа по нарушению предписания</typeparam>
        /// <typeparam name="TFileInfo">Информация о файле</typeparam>
        private TransferValues<TModel, Prescription> PrescriptionTransfer<TModel, TClosingDoc, TViol, TFileInfo>() 
            where TModel : BaseDocumentPrescription<TClosingDoc, TViol, TFileInfo> =>
            (TModel model, ref Prescription prescription, object mainEntity) =>
            {

                prescription.DocumentDate = model.DocumentDate;
                prescription.Description = model.Note;
                prescription.Executant = model.ExecutantId.HasValue
                    ? new ExecutantDocGji { Id = model.ExecutantId.Value }
                    : null;
                prescription.Contragent = model.OrganizationId.HasValue
                    ? new Contragent { Id = model.OrganizationId.Value }
                    : null;
                prescription.PhysicalPerson = model.Individual;
                prescription.PhysicalPersonInfo = model.Requisites;
                prescription.Closed = model.PrescriptionClosed.Value;
                prescription.CloseReason = model.Cause;
                prescription.CloseNote = model.ClosingNote;
            };

        
        /// <summary>
        /// Перенос информации для <see cref="PrescriptionViol"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, PrescriptionViol> ViolationTransfer<TModel>()
            where TModel : BasePrescriptionViolation =>
            (TModel model, ref PrescriptionViol prescriptionViol, object mainEntity) =>
            {
                var datePlanRemoval = this._datePlanRemovalDict.Get($"{_prescriptionAddressId}_{model.ViolationId.Value}");

                if (!datePlanRemoval.HasValue)
                {
                    throw new ApiServiceException("Не удалось сопоставить плановую дату устранения " +
                        $"из родительского документа \"Акт проверки\" с Id: {_actCheckId}, " +
                        $"по нарушению с Id: {model.ViolationId}, " +
                        $"в доме с Id: {_prescriptionAddressId}.\n" +
                        $"Либо данное поле не заполнено");
                }
                
                if (prescriptionViol.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    prescriptionViol.Document = mainEntity as DocumentGji;
                }
                
                prescriptionViol.InspectionViolation = prescriptionViol.InspectionViolation ?? new InspectionGjiViol();
                prescriptionViol.InspectionViolation.Action = model.Event;
                prescriptionViol.InspectionViolation.DateFactRemoval = model.DateFactRemoval;
                prescriptionViol.InspectionViolation.DatePlanRemoval = datePlanRemoval;
                prescriptionViol.InspectionViolation.SumAmountWorkRemoval = model.Amount;
                prescriptionViol.InspectionViolation.Violation = new ViolationGji { Id = model.ViolationId.Value };

                if (prescriptionViol.InspectionViolation.Id == 0)
                {
                    prescriptionViol.InspectionViolation.Inspection = (mainEntity as DocumentGji).Inspection;
                    prescriptionViol.InspectionViolation.RealityObject = new RealityObject { Id = _prescriptionAddressId };
                    AddEntityToSave(prescriptionViol.InspectionViolation);
                }
                
                prescriptionViol.Action = model.Event;
                prescriptionViol.DateFactRemoval = model.DateFactRemoval;
                prescriptionViol.DatePlanRemoval = datePlanRemoval;
                prescriptionViol.SumAmountWorkRemoval = model.Amount;
            };

        /// <summary>
        /// Перенос информации для <see cref="PrescriptionCloseDoc"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, PrescriptionCloseDoc> ClosingDocumentTransfer<TModel>()
            where TModel : BasePrescriptionClosinDocument =>
            (TModel model, ref PrescriptionCloseDoc prescriptionCloseDoc, object mainEntity) =>
            {
                if (prescriptionCloseDoc.Id == 0)
                {
                    this.EntityRefCheck(mainEntity);
                    prescriptionCloseDoc.Prescription = mainEntity as Prescription;
                }

                prescriptionCloseDoc.File = new FileInfo { Id = model.FileId };
                prescriptionCloseDoc.Name = model.FileName;
                prescriptionCloseDoc.Date = model.FileDate.Value;
                prescriptionCloseDoc.DocType = model.FileType.Value;
            };

        /// <summary>
        /// Заполнение словаря дат планового устранения нарушений на основе акта проверки
        /// </summary>
        /// <param name="actCheckId">Илентификатор акта проверки</param>
        private void FillDatePlanRemovalDict(long actCheckId)
        {
            this._datePlanRemovalDict = _actCheckViolDomain
                .GetAll()
                .Where(x => x.Document.Id == actCheckId)
                .Where(x => x.DatePlanRemoval != null || x.InspectionViolation.DatePlanRemoval != null)
                .AsEnumerable()
                .GroupBy(x => new
                {
                    RoId = x.ActObject.RealityObject.Id,
                    ViolId = x.InspectionViolation.Violation.Id
                })
                .ToDictionary(x => $"{x.Key.RoId}_{x.Key.ViolId}",
                    x => x.Select(y => y.DatePlanRemoval ?? y.InspectionViolation.DatePlanRemoval).First());
        }
    }
}