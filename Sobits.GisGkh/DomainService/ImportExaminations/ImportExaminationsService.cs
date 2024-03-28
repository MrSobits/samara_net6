using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.FileManager;
using Bars.Gkh.Utils;
using Bars.GkhCalendar.Entities;
using Bars.GkhCalendar.Enums;
using Bars.GkhGji.DomainService.GisGkhRegional;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;
using Castle.Windsor;
//using CryptoPro.Sharpei;
using GisGkhLibrary.InspectionServiceAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ImportExaminationsService : IImportExaminationsService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }
        public IDomainService<NsiItem> NsiItemDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<ProdCalendar> ProdCalendarDomain { get; set; }
        public IDomainService<DisposalSurveyObjective> DisposalSurveyObjectiveDomain { get; set; }
        public IDomainService<DisposalSurveyPurpose> DisposalSurveyPurposeDomain { get; set; }
        public IDomainService<DisposalVerificationSubject> DisposalVerificationSubjectDomain { get; set; }
        public IDomainService<DecisionVerificationSubject> DecisionVerificationSubjectDomain { get; set; }
        public IDomainService<DecisionControlMeasures> DecisionControlMeasuresDomain { get; set; }
        public IDomainService<DecisionInspectionReason> DecisionInspectionReasonDomain { get; set; }
        public IDomainService<ActCheckRealityObject> ActCheckRealityObjectDomain { get; set; }
        public IDomainService<ActCheckViolation> ActCheckViolationDomain { get; set; }
        public IDomainService<ActRemovalViolation> ActRemovalViolationDomain { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }
        public IDomainService<ActCheckWitness> ActCheckWitnessDomain { get; set; }
        public IDomainService<ChelyabinskDocumentLongText> LongTextService { get; set; }
        public IDomainService<ActRemovalWitness> ActRemovalWitnessDomain { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }

        public IRepository<DocumentGji> DocumentGjiRepo { get; set; }
        public IRepository<ActCheckAnnex> ActCheckAnnexRepo { get; set; }
        public IRepository<ActCheck> ActCheckRepo { get; set; }
        public IRepository<ChelyabinskActRemoval> ActRemovalRepo { get; set; }
        public IRepository<DisposalAnnex> DisposalAnnexRepo { get; set; }
        public IRepository<DecisionAnnex> DecisionAnnexRepo { get; set; }
        public IRepository<Disposal> DisposalRepo { get; set; }
        public IRepository<Decision> DecisionRepo { get; set; }
        public IRepository<Prescription> PrescriptionRepo { get; set; }
        public IRepository<Protocol> ProtocolRepo { get; set; }
        public IRepository<PrescriptionAnnex> PrescriptionAnnexRepo { get; set; }
        public IRepository<ProtocolAnnex> ProtocolAnnexRepo { get; set; }
        public IRepository<ActCheckPeriod> ActCheckPeriodRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        private IFileService _fileService;

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }
        private IExportOrgRegistryService _ExportOrgRegistryService;
        private IGisGkhRegionalService _GisGkhRegionalService;

        #endregion

        #region Constructors

        public ImportExaminationsService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService FileService, IExportOrgRegistryService expServ)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _fileService = FileService;
            _ExportOrgRegistryService = expServ;
        }

        #endregion

        #region Public methods

        // Запрос на выгрузку проверок по датам
        public void SaveRequest(GisGkhRequests req, string[] reqParams, string OrgPPAGUID)
        {
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                DateTime from = DateTime.Parse(reqParams[0]);
                DateTime to = DateTime.Parse(reqParams[1]);

                log += $"{DateTime.Now} Формирование запроса на выгрузку проверок в ГИС ЖКХ по датам: с {from.ToShortDateString()} по {to.ToShortDateString()}\r\n";
                // Все проверки по датам, независимо от наличия ГУИДов и т.д.
                List<string> disposalIds = DisposalDomain.GetAll()
                     .Where(x => x.DocumentDate >= from && x.DocumentDate <= to).Select(x => x.Id.ToString()).ToList();

                log += $"{DateTime.Now} Формирование запроса по списку постановлений.\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);

                SaveCurrentRequest(req, disposalIds.ToArray(), OrgPPAGUID);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        // Запрос на выгрузку проверок по id
        public void SaveCurrentRequest(GisGkhRequests req, string[] reqParams, string OrgPPAGUID)
        {
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                List<string> disposalIds = reqParams.ToList();
                Dictionary<KindKNDGJI, NsiItem> KindKNDDict = new Dictionary<KindKNDGJI, NsiItem>
                {
                    { KindKNDGJI.HousingSupervision, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "65" && x.GisGkhItemCode == "1").FirstOrDefault() },
                    { KindKNDGJI.LicenseControl, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "65" && x.GisGkhItemCode == "2").FirstOrDefault() }
                };

                Dictionary<TypeCheck, NsiItem> TypeCheckDict = new Dictionary<TypeCheck, NsiItem>
                {
                    { TypeCheck.InspectionSurvey, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { TypeCheck.Monitoring, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "1").FirstOrDefault() },
                    { TypeCheck.NotPlannedDocumentation, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "1").FirstOrDefault() },
                    { TypeCheck.NotPlannedDocumentationExit, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "3").FirstOrDefault() },
                    { TypeCheck.NotPlannedExit, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { TypeCheck.PlannedDocumentation, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "1").FirstOrDefault() },
                    { TypeCheck.PlannedDocumentationExit, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "3").FirstOrDefault() },
                    { TypeCheck.PlannedExit, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { TypeCheck.VisualSurvey, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { TypeCheck.InspectVisit, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "71" && x.GisGkhItemCode == "2").FirstOrDefault() }
                };
                List<importExaminationsRequestImportExamination> examinations = new List<importExaminationsRequestImportExamination>();

                foreach (var disposalId in disposalIds)
                {
                    nsiRef[] predmet = getNSIObject(Convert.ToInt64(disposalId));
                    Disposal disposal = DisposalRepo.Get(long.Parse(disposalId));
                    if (disposal != null)
                    {
                        log += $"{DateTime.Now} Проверка {disposal.DocumentNumber} от {(disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToShortDateString() : "-")}\r\n";
                        // если конечный статус
                        if (disposal.State.FinalState)
                        {
                            try
                            {
                                InspectionGji inspection = disposal.Inspection;
                                if ((inspection != null
                                    && disposal.KindCheck.Code != TypeCheck.VisualSurvey
                                    && disposal.KindCheck.Code != TypeCheck.InspectionSurvey)
                                    && !(disposal.TypeDisposal == TypeDisposalGji.DocumentGji && GetPrescriptionGuid(disposal) == null))
                                {
                                    // проверки целиком
                                    if (disposal.GisGkhGuid == null || disposal.GisGkhGuid == "")
                                    {
                                        log += $"Распоряжение не сопоставлено с ГИС ЖКХ, выгружаем все документы по проверке\r\n";
                                        if (1 == 2)
                                        {
                                            //log += $"Запрос по данному распоряжению уже был отправлен, проверку не выгружаем\r\n";
                                            //continue;
                                        }
                                        else
                                        {
                                            CitizenInfoType citizen = null;
                                            bool exportable = true;
                                            if (inspection.PersonInspection == PersonInspection.PhysPerson)
                                            {
                                                string pp = inspection.PhysicalPerson.Replace("  ", " ");
                                                if (pp.Split(' ').Length == 3)
                                                {
                                                    citizen = new CitizenInfoType();
                                                    citizen.FirstName = pp.Split(' ')[1];
                                                    citizen.LastName = pp.Split(' ')[0];
                                                    citizen.MiddleName = pp.Split(' ')[2];

                                                }
                                                else
                                                {
                                                    log += $"{DateTime.Now} Субъект проверки - физическое лицо, не указаны Фамилия Имя и Отчество. Проверка не будет выгружена\r\n";
                                                    exportable = false;
                                                    continue;
                                                }

                                            }
                                            else if (string.IsNullOrEmpty(inspection.Contragent.GisGkhGUID))
                                            {
                                                // организация не сопоставлена
                                                try
                                                {
                                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { inspection.Contragent.Id });
                                                    log += $"{DateTime.Now} Организация - исполнитель не сопоставлена с ГИС ЖКХ. Создан запрос получения информации об организации. Проверка не будет выгружена\r\n";
                                                    exportable = false;
                                                    continue;
                                                }
                                                catch (Exception e)
                                                {
                                                    log += $"{DateTime.Now} Организация - исполнитель не сопоставлена с ГИС ЖКХ. Ошибка при создании запроса получения информации об организации {e.Message}. Проверка не будет выгружена\r\n";
                                                    exportable = false;
                                                    continue;
                                                }
                                            }
                                            if (exportable)
                                            {
                                                disposal.GisGkhTransportGuid = Guid.NewGuid().ToString();

                                                //KindKND
                                                bool licenseControl = false;
                                                if (disposal.KindKNDGJI == KindKNDGJI.LicenseControl)
                                                {
                                                    licenseControl = true;
                                                }
                                                else if (disposal.KindKNDGJI == KindKNDGJI.Both)
                                                {
                                                    log += $"Смешанный контроль - проверку не выгружаем\r\n";
                                                    continue;
                                                }
                                                else if (disposal.KindKNDGJI == KindKNDGJI.NotSet)
                                                {
                                                    log += $"Не указан вид контроля - проверку не выгружаем\r\n";
                                                    continue;
                                                }
                                                NsiItem KindKNDItem = KindKNDDict[KindKNDGJI.HousingSupervision];
                                                log += $"Жилищный надзор\r\n";
                                                if (licenseControl)
                                                {
                                                    KindKNDItem = KindKNDDict[KindKNDGJI.LicenseControl];
                                                    log += $"Лицензионный контроль\r\n";
                                                }
                                                nsiRef OversightActivitiesRef = new nsiRef
                                                {
                                                    Code = KindKNDItem.GisGkhItemCode,
                                                    GUID = KindKNDItem.GisGkhGUID
                                                };

                                                //TypeCheck
                                                NsiItem TypeCheckItem = TypeCheckDict[disposal.KindCheck.Code];
                                                nsiRef ExaminationForm = new nsiRef
                                                {
                                                    Code = TypeCheckItem.GisGkhItemCode,
                                                    GUID = TypeCheckItem.GisGkhGUID
                                                };

                                                // акт в конечном статусе
                                                DocumentGji docAct = DocumentGjiChildrenDomain.GetAll()
                                                                .Where(x => x.Parent == disposal)
                                                                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                                                                || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval
                                                                //|| x.Children.TypeDocumentGji == TypeDocumentGji.ActSurvey
                                                                )
                                                                .Where(x => x.Children.State.FinalState)
                                                                    .Select(x => x.Children).FirstOrDefault();
                                                // результат проверки
                                                ResultsInfoType ResultInfo = docAct != null ? GetResultInfo(disposal, OrgPPAGUID) : null;
                                                if (ResultInfo != null)
                                                {
                                                    if (docAct.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                                    {
                                                        log += $"Результат проверки - акт проверки {docAct.DocumentNumber} от {(docAct.DocumentDate.HasValue ? docAct.DocumentDate.ToDateString() : "-")}\r\n";
                                                    }
                                                    else if (docAct.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                                    {
                                                        log += $"Результат проверки - акт проверки предписнаия {docAct.DocumentNumber} от {(docAct.DocumentDate.HasValue ? docAct.DocumentDate.ToDateString() : "-")}\r\n";
                                                    }
                                                }

                                                // Приложения к проверке
                                                var disposalAnnexes = DisposalAnnexRepo.GetAll()
                                                    .Where(x => x.Disposal == disposal)
                                                    .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
                                                List<AttachmentType> Attachments = new List<AttachmentType>();
                                                foreach (var annex in disposalAnnexes)
                                                {
                                                    if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                                                    {
                                                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                                                        if (uploadResult.Success)
                                                        {
                                                            annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                                            DisposalAnnexRepo.Update(annex);
                                                        }
                                                        else
                                                        {
                                                            throw new Exception(uploadResult.Message);
                                                        }
                                                    }
                                                    Attachments.Add(new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = annex.GisGkhAttachmentGuid
                                                        },
                                                        Description = annex.File.FullName,
                                                        Name = annex.File.FullName,
                                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(annex.File))
                                                    });
                                                }
                                                // Предписания в конечном статусе
                                                List<DocumentGji> docPrescriptions = DocumentGjiChildrenDomain.GetAll()
                                                   .Where(x => x.Parent == docAct)
                                                   .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                                   .Where(x => x.Children.State.FinalState)
                                                   .Select(x => x.Children)
                                                   .ToList();

                                                List<importExaminationsRequestImportExaminationImportPrecept> ImportPreceptList = GetPrecepts(docPrescriptions, OrgPPAGUID, ref log);
                                                // Протоколы в конечном статусе
                                                List<DocumentGji> docProtocols = DocumentGjiChildrenDomain.GetAll()
                                                   .Where(x => docPrescriptions.Contains(x.Parent) || x.Parent == docAct)
                                                   .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                                   .Where(x => x.Children.State.FinalState)
                                                   .Select(x => x.Children)
                                                   .ToList();
                                                List<importExaminationsRequestImportExaminationImportOffence> ImportOffenceList = GetOffences(docProtocols, OrgPPAGUID, ref log);

                                                // Плановая или внеплановая проверка
                                                ExaminationTypeExaminationOverviewExaminationTypeType ExaminationTypeType = null;
                                                switch (disposal.KindCheck.Code)
                                                {
                                                    case TypeCheck.PlannedDocumentation:
                                                    case TypeCheck.PlannedDocumentationExit:
                                                    case TypeCheck.PlannedExit:
                                                        ScheduledExaminationSubjectInfoType SubjectSheduled = new ScheduledExaminationSubjectInfoType();
                                                        // Плановая - ИП
                                                        if (inspection.Contragent.OrganizationForm.OkopfCode[0] == '5')
                                                        {
                                                            log += $"Плановая проверка ИП\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);

                                                            SubjectSheduled = new ScheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new ScheduledExaminationSubjectInfoTypeIndividual
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        // Плановая - ЮЛ
                                                        else
                                                        {
                                                            log += $"Плановая проверка ЮЛ\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);
                                                            SubjectSheduled = new ScheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new ScheduledExaminationSubjectInfoTypeOrganization
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        ExaminationTypeType = new ExaminationTypeExaminationOverviewExaminationTypeType
                                                        {
                                                            Item = new ExaminationTypeExaminationOverviewExaminationTypeTypeScheduled
                                                            {
                                                                Subject = SubjectSheduled
                                                            }
                                                        };
                                                        break;
                                                    default:
                                                        UnscheduledExaminationSubjectInfoType SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType();
                                                        // Внеплановая - ИП
                                                        if (inspection.PersonInspection == PersonInspection.PhysPerson)
                                                        {
                                                            log += $"Внеплановая проверка физлица\r\n";
                                                            SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new CitizenInfoType
                                                                {
                                                                    FirstName = citizen.FirstName,
                                                                    LastName = citizen.LastName,
                                                                    MiddleName = citizen.MiddleName
                                                                }
                                                            };
                                                        }
                                                        else if (inspection.Contragent.OrganizationForm.OkopfCode[0] == '5')
                                                        {
                                                            log += $"Внеплановая проверка ИП\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);
                                                            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                                                            {
                                                                if (!string.IsNullOrEmpty(inspection.Contragent.FactAddress))
                                                                {
                                                                    placearress = inspection.Contragent.FactAddress;
                                                                }
                                                            }
                                                            SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new UnscheduledExaminationSubjectInfoTypeIndividual
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        // Внеплановая - ЮЛ
                                                        else
                                                        {
                                                            log += $"Внеплановая проверка ЮЛ\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);
                                                            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                                                            {
                                                                if (!string.IsNullOrEmpty(inspection.Contragent.FactAddress))
                                                                {
                                                                    placearress = inspection.Contragent.FactAddress;
                                                                }
                                                            }
                                                            SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new UnscheduledExaminationSubjectInfoTypeOrganization
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        ExaminationTypeType = new ExaminationTypeExaminationOverviewExaminationTypeType
                                                        {
                                                            Item = new ExaminationTypeExaminationOverviewExaminationTypeTypeUnscheduled
                                                            {
                                                                Subject = SubjectUnsheduled
                                                            }
                                                        };
                                                        break;
                                                }

                                                // Инспекторы
                                                var inspectors = DocumentGjiInspectorDomain.GetAll()
                                                    .Where(x => x.DocumentGji == disposal)
                                                    .Select(x => new
                                                    {
                                                        x.Inspector.Id,
                                                        x.Inspector.Fio,
                                                        x.Inspector.Position
                                                    }).ToList();
                                                var inspectorFIO = "";
                                                var inspectorPos = "";
                                                foreach (var inspector in inspectors)
                                                {
                                                    if (!string.IsNullOrEmpty(inspectorFIO) && !string.IsNullOrEmpty(inspectorPos))
                                                    {
                                                        inspectorFIO += ", ";
                                                        inspectorPos += ", ";
                                                    }
                                                    inspectorFIO += inspector.Fio;
                                                    inspectorPos += inspector.Position;
                                                    try
                                                    {
                                                        inspectorPos += " " + ZonalInspectionInspectorDomain.GetAll().FirstOrDefault(x => x.Inspector.Id == inspector.Id).ZonalInspection.NameGenetive;
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                                //log += $"Инспекторы: {inspectorFIO}\r\n";
                                                //log += $"Должности инспекторов: {inspectorPos}\r\n";
                                                // ExaminationInfo
                                                ExaminationTypeExaminationInfo ExaminationInfo = new ExaminationTypeExaminationInfo
                                                {
                                                    Tasks = GetTasks(disposal, ref log),
                                                    Objective = GetObjectives(disposal, ref log),
                                                    From = disposal.DateStart.Value,
                                                    Object = predmet
                                                };
                                                if (disposal.DateEnd != null)
                                                {
                                                    ExaminationInfo.To = disposal.DateEnd.Value;
                                                    ExaminationInfo.ToSpecified = true;
                                                    ExaminationInfo.Duration = new ExaminationTypeExaminationInfoDuration
                                                    {
                                                        WorkDays = GetDisposalDuration(disposal),
                                                        WorkDaysSpecified = true
                                                    };
                                                };
                                                // Участие прокуратуры
                                                if (disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                                                {
                                                    if (disposal.TypeAgreementResult != TypeAgreementResult.NotSet &&
                                                       (!string.IsNullOrEmpty(disposal.DocumentNumberWithResultAgreement) && disposal.DocumentDateWithResultAgreement.HasValue))
                                                    {
                                                        if (disposal.TypeAgreementResult == TypeAgreementResult.Agreed)
                                                        {
                                                            ExaminationInfo.ProsecutorAgreementInformation = new ProsecutorAgreementInformationType
                                                            {
                                                                ItemsElementName = new ItemsChoiceType7[]
                                                                {
                                                                ItemsChoiceType7.Agreed,
                                                                ItemsChoiceType7.OrderNumber,
                                                                ItemsChoiceType7.OrderDate
                                                                },
                                                                Items = new object[]
                                                                {
                                                                true,
                                                                disposal.DocumentNumberWithResultAgreement,
                                                                disposal.DocumentDateWithResultAgreement.Value
                                                                }
                                                            };
                                                        }
                                                        else if (disposal.TypeAgreementResult == TypeAgreementResult.NotAgreed)
                                                        {
                                                            ExaminationInfo.ProsecutorAgreementInformation = new ProsecutorAgreementInformationType
                                                            {
                                                                ItemsElementName = new ItemsChoiceType7[]
                                                                {
                                                                ItemsChoiceType7.Rejected,
                                                                ItemsChoiceType7.OrderNumber,
                                                                ItemsChoiceType7.OrderDate
                                                                },
                                                                Items = new object[]
                                                                {
                                                                true,
                                                                disposal.DocumentNumberWithResultAgreement,
                                                                disposal.DocumentDateWithResultAgreement.Value
                                                                }
                                                            };
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ExaminationInfo.ProsecutorAgreementInformation = new ProsecutorAgreementInformationType
                                                        {
                                                            ItemsElementName = new ItemsChoiceType7[]
                                                            {
                                                            ItemsChoiceType7.NoAgreementInformation
                                                            },
                                                            Items = new object[]
                                                            {
                                                            true
                                                            }
                                                        };
                                                    }
                                                }
                                                // Если проверка не предписания (первичная)
                                                if (disposal.TypeDisposal == TypeDisposalGji.Base)
                                                {
                                                    switch (inspection.TypeBase) // смотрим что явилось основанием
                                                    {
                                                        // Обращение граждан
                                                        case TypeBase.CitizenStatement:
                                                            switch (inspection.TypeJurPerson)
                                                            {
                                                                // регОп
                                                                case TypeJurPerson.RegOp:
                                                                    break;
                                                                // остальные
                                                                default:
                                                                    // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                                    NsiItem BaseItemAppeal = licenseControl ? NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "26").FirstOrDefault() :
                                                                                                               NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "36").FirstOrDefault();
                                                                    nsiRef BaseAppeal = new nsiRef
                                                                    {
                                                                        Code = BaseItemAppeal.GisGkhItemCode,
                                                                        GUID = BaseItemAppeal.GisGkhGUID
                                                                    };
                                                                    ExaminationInfo.Base = BaseAppeal;
                                                                    break;
                                                            }
                                                            break;
                                                        case TypeBase.PlanAction:
                                                            break;
                                                        case TypeBase.PlanJuridicalPerson:
                                                            break;
                                                        // Проверка соискателя лицензии
                                                        case TypeBase.LicenseApplicants:
                                                            // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                            NsiItem BaseItemNewLic = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "16").FirstOrDefault();
                                                            nsiRef BaseNewLic = new nsiRef
                                                            {
                                                                Code = BaseItemNewLic.GisGkhItemCode,
                                                                GUID = BaseItemNewLic.GisGkhGUID
                                                            };
                                                            ExaminationInfo.Base = BaseNewLic;
                                                            break;
                                                        // Проверка при переоформлении лицензии
                                                        case TypeBase.LicenseReissuance:
                                                            // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                            NsiItem BaseItemLicReissuance = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "16").FirstOrDefault();
                                                            nsiRef BaseLicReissuance = new nsiRef
                                                            {
                                                                Code = BaseItemLicReissuance.GisGkhItemCode,
                                                                GUID = BaseItemLicReissuance.GisGkhGUID
                                                            };
                                                            ExaminationInfo.Base = BaseLicReissuance;
                                                            break;
                                                    }
                                                }
                                                // Если проверка на основании предписания
                                                else if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
                                                {
                                                    var prescriptionGuid = GetPrescriptionGuid(disposal);
                                                    // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                    NsiItem BaseItem = licenseControl ? NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "9").FirstOrDefault() :
                                                                                        NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "4").FirstOrDefault();
                                                    nsiRef Base = new nsiRef
                                                    {
                                                        Code = BaseItem.GisGkhItemCode,
                                                        GUID = BaseItem.GisGkhGUID
                                                    };
                                                    ExaminationInfo.Base = Base;
                                                    log += $"Основание проведения проверки: {Base.Name}\r\n";
                                                    ExaminationInfo.BasedOnPrecept = new ExaminationTypeExaminationInfoBasedOnPrecept
                                                    {
                                                        PreceptGuid = prescriptionGuid
                                                    };
                                                }

                                                // Мероприятия
                                                List<ExaminationEventType> Events = GetEvents(disposal);

                                                List<ItemsChoiceType10> ItemsElementName = new List<ItemsChoiceType10>
                                            {
                                                ItemsChoiceType10.Examination
                                            };
                                                ExaminationTypeNotificationInfo notif = null;
                                                if (disposal.NcDate.HasValue)
                                                {
                                                    notif = new ExaminationTypeNotificationInfo
                                                    {
                                                        ItemsElementName = new ItemsChoiceType6[]
                                                        {
                                                ItemsChoiceType6.NotificationDate,
                                                ItemsChoiceType6.NotificationMethod
                                                        },
                                                        Items = new object[]
                                                        {
                                                disposal.NcDate.Value,
                                                "Уведомлён по почте, электронной почте или нарочно"
                                                        }
                                                    };
                                                }
                                                else
                                                {
                                                    notif = new ExaminationTypeNotificationInfo
                                                    {
                                                        ItemsElementName = new ItemsChoiceType6[]
                                                       {
                                                        ItemsChoiceType6.NotRequired
                                                       },
                                                        Items = new object[]
                                                        {
                                                        true
                                                        }
                                                    };
                                                }
                                                var ExaminationType = new ExaminationType
                                                {
                                                    ExaminationOverview = new ExaminationTypeExaminationOverview
                                                    {
                                                        OversightActivitiesRef = OversightActivitiesRef,
                                                        ExaminationForm = ExaminationForm,
                                                        ExaminationTypeType = ExaminationTypeType,
                                                        OrderNumber = disposal.DocumentNumber,
                                                        OrderDate = disposal.DocumentDate.Value,
                                                        OrderDateSpecified = true,
                                                        ItemsElementName = new ItemsChoiceType5[]
                                                        {
                                                ItemsChoiceType5.ShouldNotBeRegistered
                                                        },
                                                        Items = new object[]
                                                        {
                                                true
                                                        }
                                                    },
                                                    RegulatoryAuthorityInformation = new ExaminationTypeRegulatoryAuthorityInformation
                                                    {
                                                        FunctionRegistryNumber = _container.Resolve<IGisGkhRegionalService>().GetFunctionRegistryNumber(licenseControl),
                                                        AuthorizedPersons = new ExaminationTypeRegulatoryAuthorityInformationAuthorizedPersons
                                                        {
                                                            FullName = inspectorFIO,
                                                            Position = inspectorPos
                                                        }
                                                    },

                                                    NotificationInfo = notif,
                                                    ExaminationInfo = ExaminationInfo,
                                                    ExecutingInfo = new ExaminationTypeExecutingInfo
                                                    {
                                                        Event = Events.ToArray()
                                                    },
                                                    Attachments = Attachments.ToArray()
                                                };

                                                if (ResultInfo != null)
                                                {
                                                    ExaminationType.ResultsInfo = ResultInfo;
                                                }

                                                List<object> Items = new List<object>
                                            {
                                                ExaminationType
                                            };

                                                foreach (var ImportPrecept in ImportPreceptList)
                                                {
                                                    ItemsElementName.Add(ItemsChoiceType10.ImportPrecept);
                                                    Items.Add(ImportPrecept);
                                                }

                                                foreach (var ImportOffence in ImportOffenceList)
                                                {
                                                    ItemsElementName.Add(ItemsChoiceType10.ImportOffence);
                                                    Items.Add(ImportOffence);
                                                }

                                                var examination = new importExaminationsRequestImportExamination
                                                {
                                                    TransportGUID = disposal.GisGkhTransportGuid,
                                                    ItemsElementName = ItemsElementName.ToArray(),
                                                    Items = Items.ToArray()
                                                };

                                                DisposalRepo.Update(disposal);
                                                examinations.Add(examination);
                                            }
                                        }
                                    }
                                    // если есть Гуид проверки
                                    else
                                    {
                                        disposal.GisGkhTransportGuid = Guid.NewGuid().ToString();
                                        // акт в конечном статусе
                                        DocumentGji docAct = DocumentGjiChildrenDomain.GetAll()
                                                        .Where(x => x.Parent == disposal)
                                                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                                                        )
                                                        .Where(x => x.Children.State.FinalState)
                                                            .Select(x => x.Children).FirstOrDefault();
                                        // результат проверки без гуида
                                        ResultsInfoType ResultInfo = null;
                                        if (docAct != null && string.IsNullOrEmpty(docAct.GisGkhGuid))
                                        {
                                            if (string.IsNullOrEmpty(docAct.GisGkhGuid))
                                            {
                                                ResultInfo = GetResultInfo(disposal, OrgPPAGUID);
                                            }
                                        }

                                        // акты устранения в конечном статусе
                                        List<DocumentGji> docActRemovals = DocumentGjiChildrenDomain.GetAll()
                                            .Where(x => x.Parent == docAct)
                                            .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                            .Where(x => x.Children.State.FinalState)
                                            .Select(x => x.Children).ToList();
                                        // Предписания все в конечном статусе
                                        List<DocumentGji> docPrescriptionsAll = DocumentGjiChildrenDomain.GetAll()
                                           .Where(x => x.Parent == docAct || docActRemovals.Contains(x.Parent))
                                           .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                           .Where(x => x.Children.State.FinalState)
                                           .Select(x => x.Children)
                                           .ToList();
                                        // Предписания в конечном статусе без ГУИДа и транспорт ГУИДа
                                        List<DocumentGji> docPrescriptions = docPrescriptionsAll
                                           .Where(x => (x.GisGkhGuid == null || x.GisGkhGuid == "") && (x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == ""))
                                           .ToList();

                                        List<importExaminationsRequestImportExaminationImportPrecept> ImportPreceptList = GetPrecepts(docPrescriptions, OrgPPAGUID, ref log);
                                        // Протоколы в конечном статусе без ГУИДа и транспорт ГУИДа
                                        List<DocumentGji> docProtocols = new List<DocumentGji>();
                                        foreach (var docPrescription in docPrescriptionsAll)
                                        {
                                            List<DocumentGji> docProtocolsPart = DocumentGjiChildrenDomain.GetAll()
                                           .Where(x => x.Parent == docPrescription || x.Parent == docAct || docActRemovals.Contains(x.Parent))
                                           .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                           .Where(x => x.Children.State.FinalState)
                                           .Where(x => (x.Children.GisGkhGuid == null || x.Children.GisGkhGuid == "") && (x.Children.GisGkhTransportGuid == null || x.Children.GisGkhTransportGuid == ""))
                                           .Select(x => x.Children)
                                           .ToList();
                                            docProtocols.AddRange(docProtocolsPart);
                                        }
                                        List<importExaminationsRequestImportExaminationImportOffence> ImportOffenceList = GetOffences(docProtocols, OrgPPAGUID, ref log);

                                        // Если есть результат, предписание или протокол без ГУИДА, собираем запрос по этой проверке
                                        if (ResultInfo != null || ImportPreceptList.Count() > 0 || ImportOffenceList.Count() > 0)
                                        {
                                            List<ItemsChoiceType10> ItemsElementName = new List<ItemsChoiceType10>();
                                            List<object> Items = new List<object>();
                                            if (ResultInfo != null)
                                            {
                                                ItemsElementName.Add(ItemsChoiceType10.ResultsInfo);
                                                Items.Add(ResultInfo);
                                            }
                                            // Причина изменения - "Получение новых сведений" 
                                            NsiItem ChangeReasonItem = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "271" && x.GisGkhItemCode == "4").FirstOrDefault();
                                            nsiRef ChangeReason = new nsiRef
                                            {
                                                Code = ChangeReasonItem.GisGkhItemCode,
                                                GUID = ChangeReasonItem.GisGkhGUID
                                            };
                                            ItemsElementName.Add(ItemsChoiceType10.ExaminationChangeInfo);
                                            Items.Add(new ExaminationChangeInfoType
                                            {
                                                ChangeReason = ChangeReason,
                                                ChangeDate = DateTime.Now
                                            });
                                            foreach (var ImportPrecept in ImportPreceptList)
                                            {
                                                ItemsElementName.Add(ItemsChoiceType10.ImportPrecept);
                                                Items.Add(ImportPrecept);
                                            }

                                            foreach (var ImportOffence in ImportOffenceList)
                                            {
                                                ItemsElementName.Add(ItemsChoiceType10.ImportOffence);
                                                Items.Add(ImportOffence);
                                            }
                                            var examination = new importExaminationsRequestImportExamination
                                            {
                                                TransportGUID = disposal.GisGkhTransportGuid,
                                                ExaminationGuid = disposal.GisGkhGuid,
                                                ItemsElementName = ItemsElementName.ToArray(),
                                                Items = Items.ToArray()
                                            };
                                            if (examination.ItemsElementName.Count() > 0)
                                            {
                                                examinations.Add(examination);
                                                DisposalRepo.Update(disposal);
                                            }
                                        }
                                        else
                                        {
                                            // Отправлять нечего
                                            disposal.GisGkhTransportGuid = null;
                                            DisposalRepo.Update(disposal);
                                        }
                                    }
                                }
                                else
                                {
                                    // none inspection in disposal или проверка инспекционная / визуальная
                                }
                            }
                            catch (Exception e)
                            {
                                disposal.GisGkhTransportGuid = null;
                                throw new Exception("Ошибка: " + e.Message);
                            }
                        }
                        else
                        {
                            log += $"{DateTime.Now} Проверка не в конечном статусе\r\n";
                            // disposal не в finalState
                        }
                    }
                    else
                    {
                        Decision decision = DecisionRepo.Get(long.Parse(disposalId));
                        log += $"{DateTime.Now} Проверка {decision.DocumentNumber} от {(decision.DocumentDate.HasValue ? decision.DocumentDate.Value.ToShortDateString() : "-")}\r\n";
                        // если конечный статус
                        if (decision.State.FinalState)
                        {
                            try
                            {
                                InspectionGji inspection = decision.Inspection;
                                if ((inspection != null
                                    && decision.KindCheck.Code != TypeCheck.VisualSurvey
                                    && decision.KindCheck.Code != TypeCheck.InspectionSurvey)
                                    && !(decision.TypeDisposal == TypeDisposalGji.DocumentGji && GetPrescriptionGuidForDecision(decision) == null))
                                {
                                    // проверки целиком
                                    if (decision.GisGkhGuid == null || decision.GisGkhGuid == "")
                                    {
                                        log += $"Распоряжение не сопоставлено с ГИС ЖКХ, выгружаем все документы по проверке\r\n";
                                        if (1 == 2)
                                        {
                                            //log += $"Запрос по данному распоряжению уже был отправлен, проверку не выгружаем\r\n";
                                            //continue;
                                        }
                                        else
                                        {
                                            CitizenInfoType citizen = null;
                                            bool exportable = true;
                                            if (inspection.PersonInspection == PersonInspection.PhysPerson)
                                            {
                                                string pp = inspection.PhysicalPerson.Replace("  ", " ");
                                                if (pp.Split(' ').Length == 3)
                                                {
                                                    citizen = new CitizenInfoType();
                                                    citizen.FirstName = pp.Split(' ')[1];
                                                    citizen.LastName = pp.Split(' ')[0];
                                                    citizen.MiddleName = pp.Split(' ')[2];

                                                }
                                                else
                                                {
                                                    log += $"{DateTime.Now} Субъект проверки - физическое лицо, не указаны Фамилия Имя и Отчество. Проверка не будет выгружена\r\n";
                                                    exportable = false;
                                                    continue;
                                                }

                                            }
                                            else if (string.IsNullOrEmpty(inspection.Contragent.GisGkhGUID))
                                            {
                                                // организация не сопоставлена
                                                try
                                                {
                                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { inspection.Contragent.Id });
                                                    log += $"{DateTime.Now} Организация - исполнитель не сопоставлена с ГИС ЖКХ. Создан запрос получения информации об организации. Проверка не будет выгружена\r\n";
                                                    exportable = false;
                                                    continue;
                                                }
                                                catch (Exception e)
                                                {
                                                    log += $"{DateTime.Now} Организация - исполнитель не сопоставлена с ГИС ЖКХ. Ошибка при создании запроса получения информации об организации {e.Message}. Проверка не будет выгружена\r\n";
                                                    exportable = false;
                                                    continue;
                                                }
                                            }
                                            if (exportable)
                                            {
                                                decision.GisGkhTransportGuid = Guid.NewGuid().ToString();

                                                //KindKND
                                                bool licenseControl = false;
                                                if (decision.KindKNDGJI == KindKNDGJI.LicenseControl)
                                                {
                                                    licenseControl = true;
                                                }
                                                else if (decision.KindKNDGJI == KindKNDGJI.Both)
                                                {
                                                    log += $"Смешанный контроль - проверку не выгружаем\r\n";
                                                    continue;
                                                }
                                                else if (decision.KindKNDGJI == KindKNDGJI.NotSet)
                                                {
                                                    log += $"Не указан вид контроля - проверку не выгружаем\r\n";
                                                    continue;
                                                }
                                                NsiItem KindKNDItem = KindKNDDict[KindKNDGJI.HousingSupervision];
                                                log += $"Жилищный надзор\r\n";
                                                if (licenseControl)
                                                {
                                                    KindKNDItem = KindKNDDict[KindKNDGJI.LicenseControl];
                                                    log += $"Лицензионный контроль\r\n";
                                                }
                                                nsiRef OversightActivitiesRef = new nsiRef
                                                {
                                                    Code = KindKNDItem.GisGkhItemCode,
                                                    GUID = KindKNDItem.GisGkhGUID
                                                };

                                                //TypeCheck
                                                NsiItem TypeCheckItem = TypeCheckDict[decision.KindCheck.Code];
                                                nsiRef ExaminationForm = new nsiRef
                                                {
                                                    Code = TypeCheckItem.GisGkhItemCode,
                                                    GUID = TypeCheckItem.GisGkhGUID
                                                };

                                                // акт в конечном статусе
                                                DocumentGji docAct = DocumentGjiChildrenDomain.GetAll()
                                                                .Where(x => x.Parent == decision)
                                                                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                                                                || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval
                                                                )
                                                                .Where(x => x.Children.State.FinalState)
                                                                    .Select(x => x.Children).FirstOrDefault();
                                                // результат проверки
                                                ResultsInfoType ResultInfo = docAct != null ? GetResultInfoForDecision(decision, OrgPPAGUID) : null;
                                                if (ResultInfo != null)
                                                {
                                                    if (docAct.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                                    {
                                                        log += $"Результат проверки - акт проверки {docAct.DocumentNumber} от {(docAct.DocumentDate.HasValue ? docAct.DocumentDate.ToDateString() : "-")}\r\n";
                                                    }
                                                    else if (docAct.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                                    {
                                                        log += $"Результат проверки - акт проверки предписнаия {docAct.DocumentNumber} от {(docAct.DocumentDate.HasValue ? docAct.DocumentDate.ToDateString() : "-")}\r\n";
                                                    }
                                                }

                                                // Приложения к проверке
                                                var decisionAnnexes = DecisionAnnexRepo.GetAll()
                                                    .Where(x => x.Decision == decision)
                                                    .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
                                                List<AttachmentType> Attachments = new List<AttachmentType>();
                                                foreach (var annex in decisionAnnexes)
                                                {
                                                    if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                                                    {
                                                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                                                        if (uploadResult.Success)
                                                        {
                                                            annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                                            DecisionAnnexRepo.Update(annex);
                                                        }
                                                        else
                                                        {
                                                            throw new Exception(uploadResult.Message);
                                                        }
                                                    }
                                                    Attachments.Add(new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = annex.GisGkhAttachmentGuid
                                                        },
                                                        Description = annex.File.FullName,
                                                        Name = annex.File.FullName,
                                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(annex.File))
                                                    });
                                                }
                                                // Предписания в конечном статусе
                                                List<DocumentGji> docPrescriptions = DocumentGjiChildrenDomain.GetAll()
                                                   .Where(x => x.Parent == docAct)
                                                   .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                                   .Where(x => x.Children.State.FinalState)
                                                   .Select(x => x.Children)
                                                   .ToList();

                                                List<importExaminationsRequestImportExaminationImportPrecept> ImportPreceptList = GetPrecepts(docPrescriptions, OrgPPAGUID, ref log);
                                                // Протоколы в конечном статусе
                                                List<DocumentGji> docProtocols = DocumentGjiChildrenDomain.GetAll()
                                                   .Where(x => docPrescriptions.Contains(x.Parent) || x.Parent == docAct)
                                                   .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                                   .Where(x => x.Children.State.FinalState)
                                                   .Select(x => x.Children)
                                                   .ToList();
                                                List<importExaminationsRequestImportExaminationImportOffence> ImportOffenceList = GetOffences(docProtocols, OrgPPAGUID, ref log);

                                                // Плановая или внеплановая проверка
                                                ExaminationTypeExaminationOverviewExaminationTypeType ExaminationTypeType = null;
                                                switch (decision.KindCheck.Code)
                                                {
                                                    case TypeCheck.PlannedDocumentation:
                                                    case TypeCheck.PlannedDocumentationExit:
                                                    case TypeCheck.PlannedExit:
                                                        ScheduledExaminationSubjectInfoType SubjectSheduled = new ScheduledExaminationSubjectInfoType();
                                                        // Плановая - ИП
                                                        if (inspection.Contragent.OrganizationForm.OkopfCode[0] == '5')
                                                        {
                                                            log += $"Плановая проверка ИП\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);

                                                            SubjectSheduled = new ScheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new ScheduledExaminationSubjectInfoTypeIndividual
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        // Плановая - ЮЛ
                                                        else
                                                        {
                                                            log += $"Плановая проверка ЮЛ\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);
                                                            SubjectSheduled = new ScheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new ScheduledExaminationSubjectInfoTypeOrganization
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        ExaminationTypeType = new ExaminationTypeExaminationOverviewExaminationTypeType
                                                        {
                                                            Item = new ExaminationTypeExaminationOverviewExaminationTypeTypeScheduled
                                                            {
                                                                Subject = SubjectSheduled
                                                            }
                                                        };
                                                        break;
                                                    default:
                                                        UnscheduledExaminationSubjectInfoType SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType();
                                                        // Внеплановая - ИП
                                                        if (inspection.PersonInspection == PersonInspection.PhysPerson)
                                                        {
                                                            log += $"Внеплановая проверка физлица\r\n";
                                                            SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new CitizenInfoType
                                                                {
                                                                    FirstName = citizen.FirstName,
                                                                    LastName = citizen.LastName,
                                                                    MiddleName = citizen.MiddleName
                                                                }
                                                            };
                                                        }
                                                        else if (inspection.Contragent.OrganizationForm.OkopfCode[0] == '5')
                                                        {
                                                            log += $"Внеплановая проверка ИП\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);
                                                            if (decision.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                                                            {
                                                                if (!string.IsNullOrEmpty(inspection.Contragent.FactAddress))
                                                                {
                                                                    placearress = inspection.Contragent.FactAddress;
                                                                }
                                                            }
                                                            SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new UnscheduledExaminationSubjectInfoTypeIndividual
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        // Внеплановая - ЮЛ
                                                        else
                                                        {
                                                            log += $"Внеплановая проверка ЮЛ\r\n";
                                                            string placearress = _container.Resolve<IGisGkhRegionalService>().FindExaminationPlace(inspection);
                                                            if (decision.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit || decision.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                                                            {
                                                                if (!string.IsNullOrEmpty(inspection.Contragent.FactAddress))
                                                                {
                                                                    placearress = inspection.Contragent.FactAddress;
                                                                }
                                                            }
                                                            SubjectUnsheduled = new UnscheduledExaminationSubjectInfoType
                                                            {
                                                                Item = new UnscheduledExaminationSubjectInfoTypeOrganization
                                                                {
                                                                    orgRootEntityGUID = inspection.Contragent.GisGkhGUID,
                                                                    ActualActivityPlace = string.IsNullOrEmpty(placearress) ? inspection.Contragent.FactAddress : placearress,
                                                                    SmallBusiness = inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Micro
                                                                        || inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? true : false
                                                                }
                                                            };
                                                        }
                                                        ExaminationTypeType = new ExaminationTypeExaminationOverviewExaminationTypeType
                                                        {
                                                            Item = new ExaminationTypeExaminationOverviewExaminationTypeTypeUnscheduled
                                                            {
                                                                Subject = SubjectUnsheduled
                                                            }
                                                        };
                                                        break;
                                                }

                                                // Инспекторы
                                                var inspectors = DocumentGjiInspectorDomain.GetAll()
                                                    .Where(x => x.DocumentGji == decision)
                                                    .Select(x => new
                                                    {
                                                        x.Inspector.Id,
                                                        x.Inspector.Fio,
                                                        x.Inspector.Position
                                                    }).ToList();
                                                var inspectorFIO = "";
                                                var inspectorPos = "";
                                                foreach (var inspector in inspectors)
                                                {
                                                    if (!string.IsNullOrEmpty(inspectorFIO) && !string.IsNullOrEmpty(inspectorPos))
                                                    {
                                                        inspectorFIO += ", ";
                                                        inspectorPos += ", ";
                                                    }
                                                    inspectorFIO += inspector.Fio;
                                                    inspectorPos += inspector.Position;
                                                    try
                                                    {
                                                        inspectorPos += " " + ZonalInspectionInspectorDomain.GetAll().FirstOrDefault(x => x.Inspector.Id == inspector.Id).ZonalInspection.NameGenetive;
                                                    }
                                                    catch
                                                    {

                                                    }
                                                }
                                                // ExaminationInfo
                                                ExaminationTypeExaminationInfo ExaminationInfo = new ExaminationTypeExaminationInfo
                                                {
                                                    Tasks = GetTasksForDecision(decision),
                                                    Objective = GetObjectivesForDecision(decision),
                                                    From = decision.DateStart.Value,
                                                    Object = getNSIObjectDecision(Convert.ToInt64(disposalId))
                                                };
                                                if (decision.DateEnd != null)
                                                {
                                                    ExaminationInfo.To = decision.DateEnd.Value;
                                                    ExaminationInfo.ToSpecified = true;
                                                    ExaminationInfo.Duration = new ExaminationTypeExaminationInfoDuration
                                                    {
                                                        WorkDays = GetDecisionDuration(decision),
                                                        WorkDaysSpecified = true
                                                    };
                                                };
                                                // Участие прокуратуры
                                                if (decision.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                                                {
                                                    if (decision.TypeAgreementResult != TypeAgreementResult.NotSet &&
                                                       (!string.IsNullOrEmpty(decision.DocumentNumberWithResultAgreement) && decision.DocumentDateWithResultAgreement.HasValue))
                                                    {
                                                        if (decision.TypeAgreementResult == TypeAgreementResult.Agreed)
                                                        {
                                                            ExaminationInfo.ProsecutorAgreementInformation = new ProsecutorAgreementInformationType
                                                            {
                                                                ItemsElementName = new ItemsChoiceType7[]
                                                                {
                                                                ItemsChoiceType7.Agreed,
                                                                ItemsChoiceType7.OrderNumber,
                                                                ItemsChoiceType7.OrderDate
                                                                },
                                                                Items = new object[]
                                                                {
                                                                true,
                                                                decision.DocumentNumberWithResultAgreement,
                                                                decision.DocumentDateWithResultAgreement.Value
                                                                }
                                                            };
                                                        }
                                                        else if (decision.TypeAgreementResult == TypeAgreementResult.NotAgreed)
                                                        {
                                                            ExaminationInfo.ProsecutorAgreementInformation = new ProsecutorAgreementInformationType
                                                            {
                                                                ItemsElementName = new ItemsChoiceType7[]
                                                                {
                                                                ItemsChoiceType7.Rejected,
                                                                ItemsChoiceType7.OrderNumber,
                                                                ItemsChoiceType7.OrderDate
                                                                },
                                                                Items = new object[]
                                                                {
                                                                true,
                                                                decision.DocumentNumberWithResultAgreement,
                                                                decision.DocumentDateWithResultAgreement.Value
                                                                }
                                                            };
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ExaminationInfo.ProsecutorAgreementInformation = new ProsecutorAgreementInformationType
                                                        {
                                                            ItemsElementName = new ItemsChoiceType7[]
                                                            {
                                                            ItemsChoiceType7.NoAgreementInformation
                                                            },
                                                            Items = new object[]
                                                            {
                                                            true
                                                            }
                                                        };
                                                    }
                                                }
                                                // Если проверка не предписания (первичная)
                                                if (decision.TypeDisposal == TypeDisposalGji.Base)
                                                {
                                                    switch (inspection.TypeBase) // смотрим что явилось основанием
                                                    {
                                                        // Обращение граждан
                                                        case TypeBase.CitizenStatement:
                                                            switch (inspection.TypeJurPerson)
                                                            {
                                                                // регОп
                                                                case TypeJurPerson.RegOp:
                                                                    break;
                                                                // остальные
                                                                default:
                                                                    // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                                    NsiItem BaseItemAppeal = licenseControl ? NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "26").FirstOrDefault() :
                                                                                                               NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "36").FirstOrDefault();
                                                                    nsiRef BaseAppeal = new nsiRef
                                                                    {
                                                                        Code = BaseItemAppeal.GisGkhItemCode,
                                                                        GUID = BaseItemAppeal.GisGkhGUID
                                                                    };
                                                                    ExaminationInfo.Base = BaseAppeal;
                                                                    break;
                                                            }
                                                            break;
                                                        case TypeBase.PlanAction:
                                                            break;
                                                        case TypeBase.PlanJuridicalPerson:
                                                            break;
                                                        // Проверка соискателя лицензии
                                                        case TypeBase.LicenseApplicants:
                                                            // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                            NsiItem BaseItemNewLic = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "16").FirstOrDefault();
                                                            nsiRef BaseNewLic = new nsiRef
                                                            {
                                                                Code = BaseItemNewLic.GisGkhItemCode,
                                                                GUID = BaseItemNewLic.GisGkhGUID
                                                            };
                                                            ExaminationInfo.Base = BaseNewLic;
                                                            break;
                                                        // Проверка при переоформлении лицензии
                                                        case TypeBase.LicenseReissuance:
                                                            // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                            NsiItem BaseItemLicReissuance = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "16").FirstOrDefault();
                                                            nsiRef BaseLicReissuance = new nsiRef
                                                            {
                                                                Code = BaseItemLicReissuance.GisGkhItemCode,
                                                                GUID = BaseItemLicReissuance.GisGkhGUID
                                                            };
                                                            ExaminationInfo.Base = BaseLicReissuance;
                                                            break;
                                                    }
                                                }
                                                // Если проверка на основании предписания
                                                else if (decision.TypeDisposal == TypeDisposalGji.DocumentGji)
                                                {
                                                    var prescriptionGuid = GetPrescriptionGuidForDecision(decision);
                                                    // НСИ "Основание проведения проверки" (реестровый номер 68)
                                                    NsiItem BaseItem = licenseControl ? NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "9").FirstOrDefault() :
                                                                                        NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "68" && x.GisGkhItemCode == "4").FirstOrDefault();
                                                    nsiRef Base = new nsiRef
                                                    {
                                                        Code = BaseItem.GisGkhItemCode,
                                                        GUID = BaseItem.GisGkhGUID
                                                    };
                                                    ExaminationInfo.Base = Base;
                                                    log += $"Основание проведения проверки: {Base.Name}\r\n";
                                                    ExaminationInfo.BasedOnPrecept = new ExaminationTypeExaminationInfoBasedOnPrecept
                                                    {
                                                        PreceptGuid = prescriptionGuid
                                                    };
                                                }

                                                // Мероприятия
                                                List<ExaminationEventType> Events = GetEventsForDecision(decision);

                                                List<ItemsChoiceType10> ItemsElementName = new List<ItemsChoiceType10>
                                            {
                                                ItemsChoiceType10.Examination
                                            };
                                                ExaminationTypeNotificationInfo notif = null;
                                                if (decision.NcDate.HasValue)
                                                {
                                                    notif = new ExaminationTypeNotificationInfo
                                                    {
                                                        ItemsElementName = new ItemsChoiceType6[]
                                                        {
                                                ItemsChoiceType6.NotificationDate,
                                                ItemsChoiceType6.NotificationMethod
                                                        },
                                                        Items = new object[]
                                                        {
                                                decision.NcDate.Value,
                                                "Уведомлён по почте, электронной почте или нарочно"
                                                        }
                                                    };
                                                }
                                                else
                                                {
                                                    notif = new ExaminationTypeNotificationInfo
                                                    {
                                                        ItemsElementName = new ItemsChoiceType6[]
                                                       {
                                                        ItemsChoiceType6.NotRequired
                                                       },
                                                        Items = new object[]
                                                        {
                                                        true
                                                        }
                                                    };
                                                }
                                                var ExaminationType = new ExaminationType
                                                {
                                                    ExaminationOverview = new ExaminationTypeExaminationOverview
                                                    {
                                                        OversightActivitiesRef = OversightActivitiesRef,
                                                        ExaminationForm = ExaminationForm,
                                                        ExaminationTypeType = ExaminationTypeType,
                                                        OrderNumber = decision.DocumentNumber,
                                                        OrderDate = decision.DocumentDate.Value,
                                                        OrderDateSpecified = true,
                                                        ItemsElementName = new ItemsChoiceType5[]
                                                        {
                                                ItemsChoiceType5.ShouldNotBeRegistered
                                                        },
                                                        Items = new object[]
                                                        {
                                                true
                                                        }
                                                    },
                                                    RegulatoryAuthorityInformation = new ExaminationTypeRegulatoryAuthorityInformation
                                                    {
                                                        FunctionRegistryNumber = _container.Resolve<IGisGkhRegionalService>().GetFunctionRegistryNumber(licenseControl),
                                                        AuthorizedPersons = new ExaminationTypeRegulatoryAuthorityInformationAuthorizedPersons
                                                        {
                                                            FullName = inspectorFIO,
                                                            Position = inspectorPos
                                                        }
                                                    },

                                                    NotificationInfo = notif,
                                                    ExaminationInfo = ExaminationInfo,
                                                    ExecutingInfo = new ExaminationTypeExecutingInfo
                                                    {
                                                        Event = Events.ToArray()
                                                    },
                                                    Attachments = Attachments.ToArray()
                                                };

                                                if (ResultInfo != null)
                                                {
                                                    ExaminationType.ResultsInfo = ResultInfo;
                                                }

                                                List<object> Items = new List<object>
                                            {
                                                ExaminationType
                                            };

                                                foreach (var ImportPrecept in ImportPreceptList)
                                                {
                                                    ItemsElementName.Add(ItemsChoiceType10.ImportPrecept);
                                                    Items.Add(ImportPrecept);
                                                }

                                                foreach (var ImportOffence in ImportOffenceList)
                                                {
                                                    ItemsElementName.Add(ItemsChoiceType10.ImportOffence);
                                                    Items.Add(ImportOffence);
                                                }

                                                var examination = new importExaminationsRequestImportExamination
                                                {
                                                    TransportGUID = decision.GisGkhTransportGuid,
                                                    ItemsElementName = ItemsElementName.ToArray(),
                                                    Items = Items.ToArray()
                                                };

                                                DecisionRepo.Update(decision);
                                                examinations.Add(examination);
                                            }
                                        }
                                    }
                                    // если есть Гуид проверки
                                    else
                                    {
                                        decision.GisGkhTransportGuid = Guid.NewGuid().ToString();
                                        // акт в конечном статусе
                                        DocumentGji docAct = DocumentGjiChildrenDomain.GetAll()
                                                        .Where(x => x.Parent == decision)
                                                        .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                                                        )
                                                        .Where(x => x.Children.State.FinalState)
                                                            .Select(x => x.Children).FirstOrDefault();
                                        // результат проверки без гуида
                                        ResultsInfoType ResultInfo = null;
                                        if (docAct != null && string.IsNullOrEmpty(docAct.GisGkhGuid))
                                        {
                                            if (string.IsNullOrEmpty(docAct.GisGkhGuid))
                                            {
                                                ResultInfo = GetResultInfoForDecision(decision, OrgPPAGUID);
                                            }
                                        }

                                        // акты устранения в конечном статусе
                                        List<DocumentGji> docActRemovals = DocumentGjiChildrenDomain.GetAll()
                                            .Where(x => x.Parent == docAct)
                                            .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                            .Where(x => x.Children.State.FinalState)
                                            .Select(x => x.Children).ToList();
                                        // Предписания все в конечном статусе
                                        List<DocumentGji> docPrescriptionsAll = DocumentGjiChildrenDomain.GetAll()
                                           .Where(x => x.Parent == docAct || docActRemovals.Contains(x.Parent))
                                           .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                           .Where(x => x.Children.State.FinalState)
                                           .Select(x => x.Children)
                                           .ToList();
                                        // Предписания в конечном статусе без ГУИДа и транспорт ГУИДа
                                        List<DocumentGji> docPrescriptions = docPrescriptionsAll
                                           .Where(x => (x.GisGkhGuid == null || x.GisGkhGuid == "") && (x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == ""))
                                           .ToList();

                                        List<importExaminationsRequestImportExaminationImportPrecept> ImportPreceptList = GetPrecepts(docPrescriptions, OrgPPAGUID, ref log);
                                        // Протоколы в конечном статусе без ГУИДа и транспорт ГУИДа
                                        List<DocumentGji> docProtocols = new List<DocumentGji>();
                                        foreach (var docPrescription in docPrescriptionsAll)
                                        {
                                            List<DocumentGji> docProtocolsPart = DocumentGjiChildrenDomain.GetAll()
                                           .Where(x => x.Parent == docPrescription || x.Parent == docAct || docActRemovals.Contains(x.Parent))
                                           .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                           .Where(x => x.Children.State.FinalState)
                                           .Where(x => (x.Children.GisGkhGuid == null || x.Children.GisGkhGuid == "") && (x.Children.GisGkhTransportGuid == null || x.Children.GisGkhTransportGuid == ""))
                                           .Select(x => x.Children)
                                           .ToList();
                                            docProtocols.AddRange(docProtocolsPart);
                                        }
                                        List<importExaminationsRequestImportExaminationImportOffence> ImportOffenceList = GetOffences(docProtocols, OrgPPAGUID, ref log);

                                        // Если есть результат, предписание или протокол без ГУИДА, собираем запрос по этой проверке
                                        if (ResultInfo != null || ImportPreceptList.Count() > 0 || ImportOffenceList.Count() > 0)
                                        {
                                            List<ItemsChoiceType10> ItemsElementName = new List<ItemsChoiceType10>();
                                            List<object> Items = new List<object>();
                                            if (ResultInfo != null)
                                            {
                                                ItemsElementName.Add(ItemsChoiceType10.ResultsInfo);
                                                Items.Add(ResultInfo);
                                            }
                                            // Причина изменения - "Получение новых сведений" 
                                            NsiItem ChangeReasonItem = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "271" && x.GisGkhItemCode == "4").FirstOrDefault();
                                            nsiRef ChangeReason = new nsiRef
                                            {
                                                Code = ChangeReasonItem.GisGkhItemCode,
                                                GUID = ChangeReasonItem.GisGkhGUID
                                            };
                                            ItemsElementName.Add(ItemsChoiceType10.ExaminationChangeInfo);
                                            Items.Add(new ExaminationChangeInfoType
                                            {
                                                ChangeReason = ChangeReason,
                                                ChangeDate = DateTime.Now
                                            });
                                            foreach (var ImportPrecept in ImportPreceptList)
                                            {
                                                ItemsElementName.Add(ItemsChoiceType10.ImportPrecept);
                                                Items.Add(ImportPrecept);
                                            }

                                            foreach (var ImportOffence in ImportOffenceList)
                                            {
                                                ItemsElementName.Add(ItemsChoiceType10.ImportOffence);
                                                Items.Add(ImportOffence);
                                            }
                                            var examination = new importExaminationsRequestImportExamination
                                            {
                                                TransportGUID = decision.GisGkhTransportGuid,
                                                ExaminationGuid = decision.GisGkhGuid,
                                                ItemsElementName = ItemsElementName.ToArray(),
                                                Items = Items.ToArray()
                                            };
                                            if (examination.ItemsElementName.Count() > 0)
                                            {
                                                examinations.Add(examination);
                                                DecisionRepo.Update(decision);
                                            }
                                        }
                                        else
                                        {
                                            // Отправлять нечего
                                            decision.GisGkhTransportGuid = null;
                                            DecisionRepo.Update(decision);
                                        }
                                    }
                                }
                                else
                                {
                                    // none inspection in decision или проверка инспекционная / визуальная
                                }
                            }
                            catch (Exception e)
                            {
                                decision.GisGkhTransportGuid = null;
                                throw new Exception("Ошибка: " + e.Message);
                            }
                        }
                        else
                        {
                            log += $"{DateTime.Now} Проверка не в конечном статусе\r\n";
                        }
                    }
                }
                if (examinations.Count > 0)
                {
                    var request = InspectionServiceAsync.importExaminationsReq(examinations.ToArray());
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    req.Answer = "Запрос на выгрузку проверок сформирован";
                    log += $"{DateTime.Now} Запрос на выгрузку {examinations.Count} проверок сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                }
                else
                {
                    req.RequestState = GisGkhRequestState.NotFormed;
                    req.Answer = "Выбранные проверки не требуют отправки в ГИС ЖКХ";
                    log += $"{DateTime.Now} Нет подходящих проверок для выгрузки\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void CheckAnswer(GisGkhRequests req, string orgPPAGUID)
        {
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Запрос ответа\r\n";
                var response = InspectionServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer = "Ответ получен";
                    req.RequestState = GisGkhRequestState.ResponseReceived;
                    log += $"{DateTime.Now} Ответ из ГИС ЖКХ получен. Ставим заачу на обработку ответа\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = "Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            SaveLog(ref req, ref log);
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа importExamination поставлена в очередь с id {taskInfo.TaskId}";
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer = "Запрос ещё в очереди";
                    log += $"{DateTime.Now} Запрос ещё в очереди\r\n";
                    SaveLog(ref req, ref log);
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer = e.Message;
                GisGkhRequestsDomain.Update(req);
            }
        }

        public void ProcessAnswer(GisGkhRequests req)
        {
            string log = string.Empty;
            if (req.LogFile != null)
            {
                StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                log = reader.ReadToEnd();
                log += "\r\n";
            }
            log += $"{DateTime.Now} Обработка ответа\r\n";
            if (req.RequestState == GisGkhRequestState.ResponseReceived)
            {
                try
                {
                    var fileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(fileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        //return Encoding.UTF8.GetString(data);
                        var response = DeserializeData<getStateResult>(Encoding.UTF8.GetString(data));
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)responseItem;
                                req.Answer = $"{error.ErrorCode}:{error.Description}";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                            }
                            else if (responseItem is CommonResultType) // тут проверка, предписание или протокол
                            {
                                bool error = false;
                                foreach (var item in ((CommonResultType)responseItem).Items)
                                {
                                    if (item is CommonResultTypeError)
                                    {
                                        error = true;
                                        log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {((CommonResultTypeError)item).ErrorCode}:{((CommonResultTypeError)item).Description}\r\n" +
                                            $"Документ не выгружен\r\n";
                                    }
                                }
                                var disposal = DisposalRepo.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == ((CommonResultType)responseItem).TransportGUID).FirstOrDefault();
                                if (disposal != null)
                                {
                                    disposal.GisGkhGuid = ((CommonResultType)responseItem).GUID;
                                    disposal.GisGkhTransportGuid = null;
                                    DisposalRepo.Update(disposal);
                                    if (!error)
                                    {
                                        log += $"{DateTime.Now} Распоряжение {disposal.DocumentNumber} от " +
                                            $"{(disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToShortDateString() : "")} " +
                                            $"выгруженo в ГИС ЖКХ с GUID {disposal.GisGkhGuid}\r\n";
                                    }
                                    List<DocumentGji> docActs = DocumentGjiChildrenDomain.GetAll()
                                       .Where(x => x.Parent == disposal)
                                       .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                       .Where(x => x.Children.GisGkhTransportGuid == ((CommonResultType)responseItem).TransportGUID)
                                       .Select(x => x.Children).ToList(); // все актЧеки с транспорт гуидом
                                    foreach (var docAct in docActs)
                                    {
                                        docAct.GisGkhGuid = ((CommonResultType)responseItem).GUID;
                                        docAct.GisGkhTransportGuid = null;
                                        DocumentGjiRepo.Update(docAct);
                                        if (!error)
                                        {
                                            log += $"{DateTime.Now} Акт проверки {docAct.DocumentNumber} от " +
                                                $"{(docAct.DocumentDate.HasValue ? docAct.DocumentDate.Value.ToShortDateString() : "")} " +
                                                $"выгружен в ГИС ЖКХ с GUID {docAct.GisGkhGuid}\r\n";
                                        }
                                    }
                                }
                                else
                                {
                                    var prescription = PrescriptionRepo.GetAll()
                                        .Where(x => x.GisGkhTransportGuid == ((CommonResultType)responseItem).TransportGUID).FirstOrDefault();
                                    if (prescription != null)
                                    {
                                        prescription.GisGkhGuid = ((CommonResultType)responseItem).GUID;
                                        prescription.GisGkhTransportGuid = null;
                                        PrescriptionRepo.Update(prescription);
                                        if (!error)
                                        {
                                            log += $"{DateTime.Now} Предписание {prescription.DocumentNumber} от " +
                                                $"{(prescription.DocumentDate.HasValue ? prescription.DocumentDate.Value.ToShortDateString() : "")} " +
                                                $"выгружено в ГИС ЖКХ с GUID {prescription.GisGkhGuid}\r\n";
                                        }
                                    }
                                    else
                                    {
                                        var protocol = ProtocolRepo.GetAll()
                                            .Where(x => x.GisGkhTransportGuid == ((CommonResultType)responseItem).TransportGUID).FirstOrDefault();
                                        if (protocol != null)
                                        {
                                            protocol.GisGkhGuid = ((CommonResultType)responseItem).GUID;
                                            protocol.GisGkhTransportGuid = null;
                                            ProtocolRepo.Update(protocol);
                                            if (!error)
                                            {
                                                log += $"{DateTime.Now} Протокол {protocol.DocumentNumber} от " +
                                                    $"{(protocol.DocumentDate.HasValue ? protocol.DocumentDate.Value.ToShortDateString() : "")} " +
                                                    $"выгружен в ГИС ЖКХ с GUID {protocol.GisGkhGuid}\r\n";
                                            }
                                        }
                                    }
                                }
                                req.Answer = "Данные из ГИС ЖКХ обработаны";
                                req.RequestState = GisGkhRequestState.ResponseProcessed;
                            }
                        }
                        log += $"{DateTime.Now} Обработка ответа завершена\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                    }
                    else
                    {
                        throw new Exception("Не найден файл с ответом из ГИС ЖКХ");
                    }
                }
                catch (Exception e)
                {
                    req.RequestState = GisGkhRequestState.Error;
                    req.Answer = "Ошибка при обработке данных из ГИС ЖКХ";
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: " + e.Message);
                }
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Получить хэш файла по алгоритму ГОСТ Р 34.11-94
        /// </summary>
        public static string GetGhostHash(Stream content)
        {
            return "";
            /* using (var gost = Gost3411.Create())
             {
                 if (gost == null)
                 {
                     throw new ApplicationException("Не удалось получть хэш вложения по алгоритму ГОСТ-3411");
                 }
                 var hash = gost.ComputeHash(content);
                 var hex = new StringBuilder(hash.Length * 2);
                 foreach (var b in hash)
                 {
                     hex.AppendFormat("{0:x2}", b);
                 }
                 return hex.ToString();
             }*/
        }

        private nsiRef[] getNSIObject(long dispId)
        {
            List<nsiRef> nsiList = new List<nsiRef>();
            DisposalVerificationSubjectDomain.GetAll()
                .Where(x => x.Disposal.Id == dispId)
                .Select(x => x.SurveySubject).ToList()
                .ForEach(rec =>
                {
                    var nsiItem = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "69" && x.GisGkhItemCode == rec.GisGkhCode && x.IsActual == YesNo.Yes).FirstOrDefault();
                    if (nsiItem != null)
                    {
                        nsiList.Add(new nsiRef
                        {
                            Code = nsiItem.GisGkhItemCode,
                            GUID = nsiItem.GisGkhGUID,
                        });
                    }
                });
            return nsiList.ToArray();
        }

        private nsiRef[] getNSIObjectDecision(long dispId)
        {
            List<nsiRef> nsiList = new List<nsiRef>();
            DecisionVerificationSubjectDomain.GetAll()
                .Where(x => x.Decision.Id == dispId)
                .Select(x => x.SurveySubject).ToList()
                .ForEach(rec =>
                {
                    var nsiItem = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "69" && x.GisGkhItemCode == rec.GisGkhCode && x.IsActual == YesNo.Yes).FirstOrDefault();
                    if (nsiItem != null)
                    {
                        nsiList.Add(new nsiRef
                        {
                            Code = nsiItem.GisGkhItemCode,
                            GUID = nsiItem.GisGkhGUID,
                        });
                    }
                });
            return nsiList.ToArray();
        }

        /// <summary>
        /// Сериаилазация запроса
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <returns>Xml-документ</returns>
        protected XmlDocument SerializeRequest(object data)
        {
            var type = data.GetType();
            XmlDocument result;

            var attr = (XmlTypeAttribute)type.GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(type, attr.Namespace);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, data);

                    result = new XmlDocument();
                    result.LoadXml(stringWriter.ToString());
                }
            }

            //var prefixer = new XmlNsPrefixer();
            //prefixer.Process(result);

            return result;
        }

        protected TDataType DeserializeData<TDataType>(string data)
        {
            TDataType result;

            var attr = (XmlTypeAttribute)typeof(TDataType).GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(typeof(TDataType), attr.Namespace);

            using (var reader = XmlReader.Create(new StringReader(data)))
            {
                result = (TDataType)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        private Bars.B4.Modules.FileStorage.FileInfo SaveFile(byte[] data, string fileName)
        {
            if (data == null)
                return null;

            try
            {
                //сохраняем пакет
                return _fileManager.SaveFile(fileName, data);
            }
            catch (Exception eeeeeeee)
            {
                return null;
            }
        }

        private void SaveFile(GisGkhRequests req, GisGkhFileType fileType, XmlDocument data, string fileName)
        {
            if (data == null)
                throw new Exception("Пустой документ для сохранения");

            MemoryStream stream = new MemoryStream();
            data.PreserveWhitespace = true;
            data.Save(stream);
            try
            {
                //сохраняем
                GisGkhRequestsFileDomain.Save(new GisGkhRequestsFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    GisGkhRequests = req,
                    GisGkhFileType = fileType,
                    FileInfo = _fileManager.SaveFile(stream, fileName)
                });
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при сохранении файла: " + e.Message);
            }
        }

        private void SaveLog(ref GisGkhRequests req, ref string log)
        {
            if (req.LogFile != null)
            {
                var FileInfo = req.LogFile;
                req.LogFile = null;
                GisGkhRequestsDomain.Update(req);
                _fileManager.Delete(FileInfo);
                var fullPath = $"{((FileSystemFileManager)_fileManager).FilesDirectory.FullName}\\{FileInfo.ObjectCreateDate.Year}\\{FileInfo.ObjectCreateDate.Month}\\{FileInfo.Id}.{FileInfo.Extention}";
                //var fullPath = $"{FtpPath}\\{FileInfo.ObjectCreateDate.Year}\\{FileInfo.ObjectCreateDate.Month}\\{FileInfo.Id}.{FileInfo.Extention}";
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch
                {
                    log += $"{DateTime.Now} Не удалось удалить старый лог-файл\r\n";
                }
            }
            req.LogFile = _fileManager.SaveFile("log.txt", Encoding.UTF8.GetBytes(log));
            //GisGkhRequestsDomain.Update(req);
        }

        private string GetPrescriptionGuid(Disposal disposal)
        {
            if (disposal != null)
            {
                var parent = DocumentGjiChildrenDomain.GetAll()
                               .Where(x => x.Children == disposal)
                               .Select(x => new
                               {
                                   x.Parent
                               })
                               .FirstOrDefault();
                var prescription = PrescriptionDomain.Get(parent.Parent.Id);
                if (prescription != null)
                {
                    return prescription.GisGkhGuid;
                }
                else return null;
            }
            else return null;
        }

        private string GetPrescriptionGuidForDecision(Decision decision)
        {
            if (decision != null)
            {
                var parent = DocumentGjiChildrenDomain.GetAll()
                               .Where(x => x.Children == decision)
                               .Select(x => new
                               {
                                   x.Parent
                               })
                               .FirstOrDefault();
                var prescription = PrescriptionDomain.Get(parent.Parent.Id);
                if (prescription != null)
                {
                    return prescription.GisGkhGuid;
                }
                else return null;
            }
            else return null;
        }

        /// <summary>
        /// Задачи проверки
        /// </summary>
        private string GetTasks(Disposal disposal, ref string log)
        {
            string result = string.Empty;
            if (disposal != null)
            {
                List<string> tasks = DisposalSurveyObjectiveDomain.GetAll() // у нас задачи - это Objective
                            .Where(x => x.Disposal == disposal)
                            .Select(x => x.Description).Distinct().ToList();
                foreach (string task in tasks)
                {
                    if (result != string.Empty)
                    {
                        result += "; ";
                    }
                    result += task;
                }
                log += $"Задачи: {result}\r\n";
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Задачи проверки
        /// </summary>
        private string GetTasksForDecision(Decision decision)
        {
            string result = string.Empty;
            if (decision != null)
            {
                List<string> tasks = DecisionInspectionReasonDomain.GetAll()
                            .Where(x => x.Decision == decision)
                            .Select(x => x.InspectionReason.Name).Distinct().ToList();
                foreach (string task in tasks)
                {
                    if (result != string.Empty)
                    {
                        result += "; ";
                    }
                    result += task;
                }

                if (!string.IsNullOrEmpty(result))
                {
                    return "Проверка в связи с " + result;
                }
                else
                {
                    return decision.TypeBase.GetDisplayName();
                }
            }
            else return null;
        }

        /// <summary>
        /// Цели проверки
        /// </summary>
        private string GetObjectives(Disposal disposal, ref string log)
        {
            string result = string.Empty;
            if (disposal != null)
            {
                List<string> objectives = DisposalSurveyPurposeDomain.GetAll() // у нас цели - это Purpose
                            .Where(x => x.Disposal == disposal)
                            .Select(x => x.Description).Distinct().ToList();
                foreach (string objective in objectives)
                {
                    if (result != string.Empty)
                    {
                        result += "; ";
                    }
                    result += objective;
                }
                log += $"Цели: {result}\r\n";
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Цели проверки
        /// </summary>
        private string GetObjectivesForDecision(Decision decision)
        {
            string result = string.Empty;
            if (decision != null)
            {
                List<string> objectives = DecisionControlMeasuresDomain.GetAll()
                            .Where(x => x.Decision == decision)
                            .Select(x => x.ControlActivity.Name).Distinct().ToList();
                foreach (string objective in objectives)
                {
                    if (result != string.Empty)
                    {
                        result += "; ";
                    }
                    result += objective;
                }

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
                else
                {
                    return decision.TypeDisposal.GetDisplayName();
                }
            }
            else return null;
        }

        private List<ExaminationEventType> GetEvents(Disposal disposal)
        {
            var eventsStrs = _container.Resolve<IGisGkhRegionalService>().GetDisposalControlMeasures(disposal);
            List<ExaminationEventType> events = new List<ExaminationEventType>();
            int EventNo = 1;
            foreach (var eventsStr in eventsStrs)
            {

                events.Add(new ExaminationEventType
                {
                    Number = EventNo++.ToString(),
                    Description = eventsStr
                });
            }
            return events;
        }

        private List<ExaminationEventType> GetEventsForDecision(Decision decision)
        {
            var eventsStrs = _container.Resolve<IGisGkhRegionalService>().GetDecisionControlMeasures(decision);
            List<ExaminationEventType> events = new List<ExaminationEventType>();
            int EventNo = 1;
            foreach (var eventsStr in eventsStrs)
            {
                events.Add(new ExaminationEventType
                {
                    Number = EventNo++.ToString(),
                    Description = eventsStr
                });
            }
            return events;
        }

        private string FindPlace(InspectionGji inspection)
        {
            var place = "";
            if (inspection != null)
            {
                var ROs = InspectionGjiRealityObjectDomain.GetAll()
                                        .Where(x => x.Inspection == inspection).ToList();
                foreach (var RO in ROs)
                {
                    if (!RO.RealityObject.Address.Contains("Регистрация обращений"))
                    {
                        if (!string.IsNullOrEmpty(place))
                        {
                            place += ", ";
                        }
                        place += RO.RealityObject.Address;
                    }
                    // адрес - "Регистрация обращений"
                    else
                    {
                        if (inspection.Contragent.JuridicalAddress != null)
                        {
                            if (!string.IsNullOrEmpty(place))
                            {
                                place += ", ";
                            }
                            place += inspection.Contragent.JuridicalAddress;
                        }
                        // если у контрагента нет юридического адреса
                        else
                        {
                            Contragent insp = ContragentDomain.GetAll()
                                .Where(x => x.Inn == "6317038043").FirstOrDefault(); // ГЖИ Самарской области
                            if (insp != null && insp.JuridicalAddress != null)
                            {
                                if (!string.IsNullOrEmpty(place))
                                {
                                    place += ", ";
                                }
                                place += insp.JuridicalAddress;
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(place))
            {
                if (inspection.Contragent != null)
                {
                    place = inspection.Contragent.FactAddress;
                }
                else
                {
                    place = "г. Воронеж";
                }
            }
            return place;
        }

        private string FindARepresentativersFio(DocumentGji act)
        {
            if (act != null)
            {
                if (act.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    var witness = ActCheckWitnessDomain.GetAll().FirstOrDefault(x => x.ActCheck.Id == act.Id);
                    if (witness != null)
                    {
                        return witness.Fio;
                    }
                }
                else if (act.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                {
                    var witness = ActRemovalWitnessDomain.GetAll().FirstOrDefault(x => x.ActRemoval.Id == act.Id);
                    if (witness != null)
                    {
                        return witness.Position;
                    }
                }

             
            }
            return "не указан";
        }
        private string FindARepresentativersPosition(DocumentGji act)
        {
            if (act != null)
            {
                if (act.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    var witness = ActCheckWitnessDomain.GetAll().FirstOrDefault(x => x.ActCheck.Id == act.Id);
                    if (witness != null)
                    {
                        return witness.Position;
                    }
                }
                else if (act.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                {
                    var witness = ActRemovalWitnessDomain.GetAll().FirstOrDefault(x => x.ActRemoval.Id == act.Id);
                    if (witness != null)
                    {
                        return witness.Position;
                    }
                }


            }
            return "не указана";
        }
        private string FindActInspectors(DocumentGji act)
        {
            var inspectorsStr = "";
            if (act != null)
            {
                var inspectors = DocumentGjiInspectorDomain.GetAll()
                                    .Where(x => x.DocumentGji == act)
                                    .Select(x => new
                                    {
                                        x.Inspector.Fio,
                                        x.Inspector.Position
                                    }).ToList();
                foreach (var inspector in inspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorsStr))
                    {
                        inspectorsStr += "; ";
                    }
                    inspectorsStr += $"{inspector.Fio}";
                }
            }
            return inspectorsStr;
        }
        private string FindActInspectorsPositions(DocumentGji act)
        {
            var inspectorsStr = "";
            if (act != null)
            {
                var inspectors = DocumentGjiInspectorDomain.GetAll()
                                    .Where(x => x.DocumentGji == act)
                                    .Select(x => new
                                    {
                                        x.Inspector.Id,
                                        x.Inspector.Fio,
                                        x.Inspector.Position
                                    }).ToList();
                foreach (var inspector in inspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorsStr))
                    {
                        inspectorsStr += "; ";
                    }
                    inspectorsStr += $"{inspector.Position}";
                    try
                    {
                        inspectorsStr += $" {ZonalInspectionInspectorDomain.GetAll().FirstOrDefault(x => x.Inspector.Id == inspector.Id).ZonalInspection.NameGenetive}";
                    }
                    catch
                    {
                        
                    }
                }
            }
            return inspectorsStr;
        }
        //private int GetDisposalDuration(Disposal disposal, ActCheck actCheck)
        //{
        //    // продолжительность
        //    var calendarDays = this.Container.Resolve<IDomainService<Day>>().GetAll()
        //                            .Where(x => x.DayDate >= disposal.DateStart.Value.Date && x.DayDate <= disposal.DateEnd.Value.Date).ToList();
        //    int durationDays = 0;
        //    var convertedDate = actCheck.DocumentDate.Value.Subtract(disposal.DateStart.Value).Days + 1;
        //    for (int i = 0; i < convertedDate; i++)
        //    {
        //        var checkedDate = disposal.DateStart.Value.AddDays(i);
        //        // проверяем по производственному календарю
        //        if (calendarDays.Where(x => x.DayType == DayType.DayOff || x.DayType == DayType.Holiday).Select(x => x.DayDate.Date).Contains(checkedDate.Date))
        //        {
        //            continue;
        //        }
        //        //if (checkedDate.DayOfWeek == DayOfWeek.Saturday || checkedDate.DayOfWeek == DayOfWeek.Sunday)
        //        //{
        //        //    continue;
        //        //}
        //        durationDays++;
        //    }
        //    if (durationDays <= 0)
        //    {
        //        durationDays = 1;
        //    }
        //    return durationDays;
        //}

        private int GetDisposalDuration(Disposal disposal, DateTime? actDate)
        {
            // продолжительность
            //var calendarDays = this.Container.Resolve<IDomainService<Day>>().GetAll()
            //                        .Where(x => x.DayDate >= disposal.DateStart.Value.Date && x.DayDate <= disposal.DateEnd.Value.Date).ToList();
            //int durationDays = 0;
            //var convertedDate = actDate.Value.Subtract(disposal.DateStart.Value).Days + 1;
            //for (int i = 0; i < convertedDate; i++)
            //{
            //    var checkedDate = disposal.DateStart.Value.AddDays(i);
            //    // проверяем по производственному календарю
            //    if (calendarDays.Where(x => x.DayType == DayType.DayOff || x.DayType == DayType.Holiday).Select(x => x.DayDate.Date).Contains(checkedDate.Date))
            //    {
            //        continue;
            //    }
            //    //if (checkedDate.DayOfWeek == DayOfWeek.Saturday || checkedDate.DayOfWeek == DayOfWeek.Sunday)
            //    //{
            //    //    continue;
            //    //}
            //    durationDays++;
            //}
            //if (durationDays <= 0)
            //{
            //    durationDays = 1;
            //}
            //return durationDays;
            int DURATION_DAY = 0;
            if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
            {
                var prodCalendarContainer = ProdCalendarDomain.GetAll()
                  .Where(x => x.ProdDate >= disposal.DateStart.Value && x.ProdDate <= disposal.DateEnd.Value).Select(x => x.ProdDate).ToList();

                
                for (DateTime dt = disposal.DateStart.Value; dt <= disposal.DateEnd.Value; dt = dt.AddDays(1))
                {
                    if ((dt.DayOfWeek != DayOfWeek.Saturday) && (dt.DayOfWeek != DayOfWeek.Sunday))
                    {
                        if (!prodCalendarContainer.Contains(dt))
                            DURATION_DAY++;
                    }
                }
            }
            return DURATION_DAY;
        }

        private int GetDisposalDuration(Disposal disposal)
        {
            // продолжительность
            //var calendarDays = this.Container.Resolve<IDomainService<Day>>().GetAll()
            //                        .Where(x => x.DayDate >= disposal.DateStart.Value.Date && x.DayDate <= disposal.DateEnd.Value.Date).ToList();
            //int durationDays = 0;
            //var convertedDate = disposal.DateEnd.Value.Subtract(disposal.DateStart.Value).Days + 1;
            //for (int i = 0; i < convertedDate; i++)
            //{
            //    var checkedDate = disposal.DateStart.Value.AddDays(i);
            //    // проверяем по производственному календарю
            //    if (calendarDays.Where(x => x.DayType == DayType.DayOff || x.DayType == DayType.Holiday).Select(x => x.DayDate.Date).Contains(checkedDate.Date))
            //    {
            //        continue;
            //    }
            //    //if (checkedDate.DayOfWeek == DayOfWeek.Saturday || checkedDate.DayOfWeek == DayOfWeek.Sunday)
            //    //{
            //    //    continue;
            //    //}
            //    durationDays++;
            //}
            //if (durationDays <= 0)
            //{
            //    durationDays = 1;
            //}
            //return durationDays;
            int DURATION_DAY = 0;
            if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
            {
                var prodCalendarContainer = ProdCalendarDomain.GetAll()
                  .Where(x => x.ProdDate >= disposal.DateStart.Value && x.ProdDate <= disposal.DateEnd.Value).Select(x => x.ProdDate).ToList();


                for (DateTime dt = disposal.DateStart.Value; dt <= disposal.DateEnd.Value; dt = dt.AddDays(1))
                {
                    if ((dt.DayOfWeek != DayOfWeek.Saturday) && (dt.DayOfWeek != DayOfWeek.Sunday))
                    {
                        if (!prodCalendarContainer.Contains(dt))
                            DURATION_DAY++;
                    }
                }
            }
            return DURATION_DAY;
        }

        private int GetDecisionDuration(Decision decision, DateTime? actDate)
        {
            int DURATION_DAY = 0;
            if (decision.DateStart.HasValue && decision.DateEnd.HasValue)
            {
                var prodCalendarContainer = ProdCalendarDomain.GetAll()
                  .Where(x => x.ProdDate >= decision.DateStart.Value && x.ProdDate <= decision.DateEnd.Value).Select(x => x.ProdDate).ToList();


                for (DateTime dt = decision.DateStart.Value; dt <= decision.DateEnd.Value; dt = dt.AddDays(1))
                {
                    if ((dt.DayOfWeek != DayOfWeek.Saturday) && (dt.DayOfWeek != DayOfWeek.Sunday))
                    {
                        if (!prodCalendarContainer.Contains(dt))
                            DURATION_DAY++;
                    }
                }
            }
            return DURATION_DAY;
        }

        private int GetDecisionDuration(Decision decision)
        {
            int DURATION_DAY = 0;
            if (decision.DateStart.HasValue && decision.DateEnd.HasValue)
            {
                var prodCalendarContainer = ProdCalendarDomain.GetAll()
                  .Where(x => x.ProdDate >= decision.DateStart.Value && x.ProdDate <= decision.DateEnd.Value).Select(x => x.ProdDate).ToList();


                for (DateTime dt = decision.DateStart.Value; dt <= decision.DateEnd.Value; dt = dt.AddDays(1))
                {
                    if ((dt.DayOfWeek != DayOfWeek.Saturday) && (dt.DayOfWeek != DayOfWeek.Sunday))
                    {
                        if (!prodCalendarContainer.Contains(dt))
                            DURATION_DAY++;
                    }
                }
            }
            return DURATION_DAY;
        }

        private List<importExaminationsRequestImportExaminationImportPrecept> GetPrecepts(List<DocumentGji> docPrescriptions, string OrgPPAGUID, ref string log)
        {
            List<importExaminationsRequestImportExaminationImportPrecept> preceptList = new List<importExaminationsRequestImportExaminationImportPrecept>();
            List<Prescription> prescriptions = PrescriptionRepo.GetAll()
                .Where(x => docPrescriptions.Contains(x)).ToList();
            foreach (var prescription in prescriptions)
            {
                var prescriptionAnnexes = PrescriptionAnnexRepo.GetAll()
                       .Where(x => x.Prescription == prescription)
                        .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
                List<AttachmentType> AttachmentList = new List<AttachmentType>();
                if (prescriptionAnnexes.Count > 0)
                {
                    foreach (var annex in prescriptionAnnexes)
                    {
                        if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                        {
                            var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                            if (uploadResult.Success)
                            {
                                annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                PrescriptionAnnexRepo.Update(annex);
                            }
                            else
                            {
                                throw new Exception(uploadResult.Message);
                            }
                        }
                        AttachmentList.Add(new AttachmentType
                        {
                            Attachment = new Attachment
                            {
                                AttachmentGUID = annex.GisGkhAttachmentGuid
                            },
                            Description = annex.File.FullName,
                            Name = annex.File.FullName,
                            AttachmentHASH = GetGhostHash(_fileManager.GetFile(annex.File))
                        });
                    }
                }
                List<PrescriptionViol> prescriptionViols = PrescriptionViolDomain.GetAll()
                    .Where(x => x.Document == prescription).ToList();
                if (prescriptionViols.Count() > 0)
                {
                    bool Fulfiled = true;
                    DateTime FulfiledDate = DateTime.MinValue;
                    DateTime PlanDate = DateTime.MinValue;
                    foreach (var prescriptionViol in prescriptionViols)
                    {
                        if (prescriptionViol.DateFactRemoval == null)
                        {
                            Fulfiled = false;
                        }
                        else
                        {
                            if (prescriptionViol.DateFactRemoval.Value > FulfiledDate)
                            {
                                FulfiledDate = prescriptionViol.DateFactRemoval.Value;
                            }
                        }
                        if (prescriptionViol.DatePlanRemoval == null)
                        {
                            if (prescriptionViol.InspectionViolation.DatePlanRemoval != null && prescriptionViol.InspectionViolation.DatePlanRemoval.Value > PlanDate)
                            {
                                PlanDate = prescriptionViol.InspectionViolation.DatePlanRemoval.Value;
                            }
                        }
                        else
                        {
                            if (prescriptionViol.DatePlanRemoval.Value > PlanDate)
                            {
                                PlanDate = prescriptionViol.DatePlanRemoval.Value;
                            }
                        }
                    }
                    prescription.GisGkhTransportGuid = Guid.NewGuid().ToString();

                    var precept = new importExaminationsRequestImportExaminationImportPrecept
                    {
                        TransportGUID = prescription.GisGkhTransportGuid
                    };

                    if (Fulfiled)
                    {
                        precept.Item = new PreceptType
                        {
                            Number = prescription.DocumentNumber,
                            Date = prescription.DocumentDate.Value,
                            Deadline = PlanDate,
                            ActualFulfiledPreceptDate = FulfiledDate,
                            ActualFulfiledPreceptDateSpecified = true,
                            IsFulfiledPrecept = true,
                            IsFulfiledPreceptSpecified = true,
                            Items = AttachmentList.ToArray()
                        };
                    }
                    else
                    {
                        precept.Item = new PreceptType
                        {
                            Number = prescription.DocumentNumber,
                            Date = prescription.DocumentDate.Value,
                            Deadline = PlanDate,
                            IsFulfiledPrecept = false,
                            IsFulfiledPreceptSpecified = true,
                            Items = AttachmentList.ToArray()
                        };
                        log += $"Предписание {prescription.DocumentNumber} от {prescription.DocumentDate.Value.ToShortDateString()}\r\n";
                    }                  
                    PrescriptionRepo.Update(prescription);
                    preceptList.Add(precept);
                }
            }
            return preceptList;
        }

        private List<importExaminationsRequestImportExaminationImportOffence> GetOffences(List<DocumentGji> docProtocols, string OrgPPAGUID, ref string log)
        {
            List<importExaminationsRequestImportExaminationImportOffence> offenceList = new List<importExaminationsRequestImportExaminationImportOffence>();
            List<Protocol> protocols = ProtocolRepo.GetAll()
                .Where(x => docProtocols.Contains(x)).ToList();
            foreach (var protocol in protocols)
            {
                var protocolAnnexes = ProtocolAnnexRepo.GetAll()
                       .Where(x => x.Protocol == protocol)
                        .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
                List<AttachmentType> AttachmentList = new List<AttachmentType>();
                if (protocolAnnexes.Count > 0)
                {
                    foreach (var annex in protocolAnnexes)
                    {
                        if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                        {
                            var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                            if (uploadResult.Success)
                            {
                                annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                ProtocolAnnexRepo.Update(annex);
                            }
                            else
                            {
                                throw new Exception(uploadResult.Message);
                            }
                        }
                        AttachmentList.Add(new AttachmentType
                        {
                            Attachment = new Attachment
                            {
                                AttachmentGUID = annex.GisGkhAttachmentGuid
                            },
                            Description = annex.File.FullName,
                            Name = annex.File.FullName,
                            AttachmentHASH = GetGhostHash(_fileManager.GetFile(annex.File))
                        });
                    }
                }
                List<ProtocolViolation> protocolViols = ProtocolViolationDomain.GetAll()
                    .Where(x => x.Document == protocol).ToList();
                if (protocolViols.Count() > 0)
                {
                    bool Fulfiled = true;
                    DateTime FulfiledDate = DateTime.MinValue;
                    foreach (var protocolViol in protocolViols)
                    {
                        if (protocolViol.DateFactRemoval == null)
                        {
                            Fulfiled = false;
                        }
                        else
                        {
                            if (protocolViol.DateFactRemoval.Value > FulfiledDate)
                            {
                                FulfiledDate = protocolViol.DateFactRemoval.Value;
                            }
                        }
                    }
                    protocol.GisGkhTransportGuid = Guid.NewGuid().ToString();

                    var offence = new importExaminationsRequestImportExaminationImportOffence
                    {
                        TransportGUID = protocol.GisGkhTransportGuid,
                        ItemElementName = ItemChoiceType1.Offence
                    };
                    if (Fulfiled)
                    {
                        offence.Item = new OffenceType
                        {
                            Number = protocol.DocumentNumber,
                            Date = protocol.DocumentDate.Value,
                            ActualFulfiledOffenceDate = FulfiledDate,
                            ActualFulfiledOffenceDateSpecified = true,
                            IsFulfiledOffence = true,
                            IsFulfiledOffenceSpecified = true,
                            Items = AttachmentList.ToArray()
                        };
                        log += $"Протокол {protocol.DocumentNumber} от {protocol.DocumentDate.Value.ToShortDateString()}\r\n";
                    }
                    else
                    {
                        offence.Item = new OffenceType
                        {
                            Number = protocol.DocumentNumber,
                            Date = protocol.DocumentDate.Value,
                            IsFulfiledOffence = false,
                            IsFulfiledOffenceSpecified = true,
                            Items = AttachmentList.ToArray()
                        };
                    }                   
                    ProtocolRepo.Update(protocol);
                    offenceList.Add(offence);
                }
            }
            return offenceList;
        }

        private ResultsInfoType GetResultInfo(Disposal disposal, string OrgPPAGUID)
        {
            // акт
            DocumentGji docChildAct = DocumentGjiChildrenDomain.GetAll()
                            .Where(x => x.Parent.Id == disposal.Id)
                            .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                            || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval
                            )
                            .Select(x => x.Children).FirstOrDefault();

            // inspection
            InspectionGji inspection = disposal.Inspection;
            if (inspection != null && docChildAct != null)
            {
                // ActCheck
                if (docChildAct.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    try
                    {
                        ActCheck actCheck = ActCheckRepo.Load(docChildAct.Id);
                        actCheck.GisGkhTransportGuid = disposal.GisGkhTransportGuid;
                        var actCheckROs = ActCheckRealityObjectDomain.GetAll()
                            .Where(x => x.ActCheck == actCheck).ToList();
                        // лица, присутствовавшие
                        var witnesses = ActCheckWitnessDomain.GetAll()
                            .Where(x => x.ActCheck == actCheck).ToList();
                        // лица, присутствовавшие и ознакомленные
                        string witnessesStr = ""; // присутствовали
                        bool NotFamiliarize = true; // никто не ознакомлен
                        string FamiliarizedPerson = ""; // кто ознакомлен
                        foreach (var witness in witnesses)
                        {
                            if (!string.IsNullOrEmpty(witnessesStr))
                            {
                                witnessesStr += "; ";
                            }
                            witnessesStr += $"{witness.Fio}, {witness.Position}";
                            if (witness.IsFamiliar)
                            {
                                NotFamiliarize = false;
                                if (!string.IsNullOrEmpty(FamiliarizedPerson))
                                {
                                    FamiliarizedPerson += "; ";
                                }
                                FamiliarizedPerson += $"{witness.Fio}";
                            }
                        }
                        var actAnnexes = ActCheckAnnexRepo.GetAll()
                            .Where(x => x.ActCheck == docChildAct)
                             .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
                        if (actAnnexes.Count > 0)
                        {
                            List<AttachmentType> OtherDocumentList = new List<AttachmentType>();
                            foreach (var annex in actAnnexes)
                            {
                                if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                                {
                                    var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                        ActCheckAnnexRepo.Update(annex);
                                    }
                                    else
                                    {
                                        throw new Exception(uploadResult.Message);
                                    }
                                }
                                OtherDocumentList.Add(new AttachmentType
                                {
                                    Attachment = new Attachment
                                    {
                                        AttachmentGUID = annex.GisGkhAttachmentGuid
                                    },
                                    Description = annex.File.FullName,
                                    Name = annex.File.FullName,
                                    AttachmentHASH = GetGhostHash(_fileManager.GetFile(annex.File))
                                });
                            }
                            // место
                            string place = FindPlace(inspection);
                            // есть нарушения
                            bool hasViol = false;
                            foreach (var actCheckRO in actCheckROs)
                            {
                                if (actCheckRO.HaveViolation == YesNoNotSet.Yes)
                                {
                                    hasViol = true;
                                    break;
                                }
                            }
                            // ссылка на тип документа "Акт" в справочнике ГИС ЖКХ
                            NsiItem actGisDict = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "64" && x.GisGkhItemCode == "1").FirstOrDefault();
                            var actCheckPeriods = ActCheckPeriodRepo.GetAll()
                                .Where(x => x.ActCheck == actCheck).ToList();
                            var actCheckDays = 0;
                            var actCheckHours = 0;
                            if (!actCheckPeriods.Any())
                            {
                                actCheckDays = GetDisposalDuration(disposal, actCheck.DocumentDate);
                                actCheckHours = actCheckDays * 8;
                            }
                            else
                            {
                                actCheckDays = actCheckPeriods.Count();
                                DateTime tempHours = DateTime.MinValue;
                                foreach (var actCheckPeriod in actCheckPeriods)
                                {
                                    if (actCheckPeriod.DateEnd != null && actCheckPeriod.DateStart != null)
                                    {
                                        tempHours += (actCheckPeriod.DateEnd.Value - actCheckPeriod.DateStart.Value);
                                    }
                                }
                                actCheckHours = tempHours.Minute > 0 ? tempHours.Hour + 1 : tempHours.Hour;
                                if (actCheckHours == 0)
                                {
                                    actCheckHours = 1;
                                }
                            }
                            //var duration = GetDisposalDuration(disposal, actCheck);
                            nsiRef settlPlaceType = null;
                            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "44",
                                    GUID = "5a4e1bf8-a55a-4f9b-bef6-65915c74bfcf",
                                    Name = "Иное"
                                };
                                place = "г. Воронеж, ул. Кирова, д. 6А.";
                            }
                            else
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "2",
                                    GUID = "88f0cc33-4845-40aa-b318-348190ad8892",
                                    Name = "Место фактического осуществления деятельности"
                                };
                            }
                            DateTime? actDateTime = null;
                            if (docChildAct != null)
                            {
                                if (docChildAct.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                {
                                    var acrChect = ActCheckRepo.Get(docChildAct.Id);
                                    if (acrChect != null)
                                    {
                                        try
                                        {
                                            actDateTime = new DateTime(docChildAct.DocumentDate.Value.Year, docChildAct.DocumentDate.Value.Month, docChildAct.DocumentDate.Value.Day, acrChect.DocumentTime.Value.Hour, acrChect.DocumentTime.Value.Minute, acrChect.DocumentTime.Value.Second);//   acrChect.DocumentTime
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            if (!actDateTime.HasValue)
                            {
                                actDateTime = docChildAct.DocumentDate.Value;
                            }
                           
                            ResultsInfoType resultInfo = new ResultsInfoType
                            {
                                FinishedInfo = new ResultsInfoTypeFinishedInfo
                                {
                                    Result = new ExaminationResultType
                                    {
                                        InspectionPersonsPosition = FindActInspectorsPositions(docChildAct),
                                        RepresentativesRegionPersons = FindARepresentativersFio(docChildAct),
                                        RepresentativesRegionPersonsPosition = FindARepresentativersPosition(docChildAct),
                                        DocumentType = new nsiRef
                                        {
                                            Code = actGisDict.GisGkhItemCode,
                                            GUID = actGisDict.GisGkhGUID
                                        },
                                        Number = docChildAct.DocumentNumber,
                                        Date = actDateTime.Value,
                                        From = disposal.DateStart.Value.Hour == 0 ? disposal.DateStart.Value.AddHours(9) : disposal.DateStart.Value,
                                        To = actCheck.DocumentDate.Value,
                                        Duration = new ExaminationResultTypeDuration
                                        {
                                            Days = actCheckDays,
                                            DaysSpecified = true,
                                            Hours = actCheckHours,
                                            HoursSpecified = true
                                        },
                                        Place = place,
                                        InspectionPersons = FindActInspectors(docChildAct),
                                        SettlingDocumentPlaceType = settlPlaceType,
                                        SettlingDocumentPlace = place
                                    },
                                    Items = OtherDocumentList.ToArray()
                                },
                                FamiliarizationInfo = new ResultsInfoTypeFamiliarizationInfo
                                {

                                }
                            };

                            if (NotFamiliarize)
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.NotFamiliarize
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            true
                                };
                            }
                            else
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.FamiliarizationDate,
                            ItemsChoiceType9.FamiliarizedPerson,
                                    //ItemsChoiceType8.IsSigned
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            actCheck.DocumentDate,
                            FamiliarizedPerson,
                                    //true // ставим "подписано"
                                };
                            }
                            if (hasViol)
                            {
                                var existingLongText = this.LongTextService.GetAll().Where(x => x.DocumentGji == actCheck).FirstOrDefault();
                                var violationsAct = ActCheckViolationDomain.GetAll()
                                    .Where(x => x.Document.Id == actCheck.Id)
                                    .Select(x => x.InspectionViolation.Violation.NormativeDocNames).ToList();
                                string regulationOffencedLegalAct = string.Empty;
                                if (violationsAct.Count > 0)
                                {
                                    regulationOffencedLegalAct = violationsAct.AggregateWithSeparator(x => x, ";");
                                }
                                var violationsActNames = ActCheckViolationDomain.GetAll()
                                   .Where(x => x.Document.Id == actCheck.Id)
                                   .Select(x => x.InspectionViolation.Violation.Name).ToList();
                                string natureOffence = string.Empty;
                                if (violationsActNames.Count > 0)
                                {
                                    natureOffence = violationsActNames.AggregateWithSeparator(x => x, ";");
                                }
                                // есть нарушение
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasOffence,
                            ItemsChoiceType8.IdentifiedOffencesInfo
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                                    true,
                                    new ExaminationResultTypeIdentifiedOffencesInfo
                                    {
                                        RegulationOffencedLegalAct = regulationOffencedLegalAct,
                                        NatureOffence = natureOffence,
                                        PersonsOffenceList = existingLongText != null? existingLongText.PersonViolationInfo.GetString():"Нет сведений"
                                    }
                                };

  
                            }
                            else
                            {
                                // нет нарушения
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasNoOffence
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                            true
                                };
                            }
                            if (witnesses.Count == 0)
                            {
                                resultInfo.FinishedInfo.Result.AbsentRepresentatives = true;
                                resultInfo.FinishedInfo.Result.AbsentRepresentativesSpecified = true;
                            }
                            else
                            {
                                resultInfo.FinishedInfo.Result.RepresentativesRegionPersons = witnessesStr;
                            }
                            ActCheckRepo.Update(actCheck);
                            return resultInfo;
                        }
                        else
                        {
                            //docChildAct.GisGkhTransportGuid = null;
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        //docChildAct.GisGkhTransportGuid = null;
                        return null;
                    }
                }
                // ActRemoval
                else
                {
                    try
                    {
                        ChelyabinskActRemoval actRemoval = ActRemovalRepo.Load(docChildAct.Id);
                        actRemoval.GisGkhTransportGuid = disposal.GisGkhTransportGuid;
                        string witnessesStr = ""; // присутствовали
                        bool NotFamiliarize = true; // никто не ознакомлен
                        string FamiliarizedPerson = ""; // кто ознакомлен
                        int witnessCount = _GisGkhRegionalService.FindActRemovalWitness(actRemoval, out witnessesStr, out NotFamiliarize, out FamiliarizedPerson);
                        List<Bars.B4.Modules.FileStorage.FileInfo> files = new List<Bars.B4.Modules.FileStorage.FileInfo>();
                        List<string> gisGkhGuids = new List<string>();
                        List<long> ids = new List<long>();
                        //_container.Resolve<IGisGkhRegionalService>().FindActRemovalAnnexes(docChildAct, out ids, out files, out gisGkhGuids);
                        _GisGkhRegionalService.FindActRemovalAnnexes(docChildAct, out ids, out files, out gisGkhGuids);
                        //var actAnnexes = ActRemovalAnnexRepo.GetAll()
                        //    .Where(x => x.ActCheck == docChildAct).ToList();
                        if (files.Count > 0)
                        {
                            List<AttachmentType> OtherDocumentList = new List<AttachmentType>();
                            for (int i = 0; i < files.Count(); i++)
                            {
                                if (string.IsNullOrEmpty(gisGkhGuids[i]))
                                {
                                    var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, files[i], OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        _GisGkhRegionalService.SaveActRemovalAnnex(ids[i], uploadResult.FileGuid);
                                    }
                                    else
                                    {
                                        throw new Exception(uploadResult.Message);
                                    }
                                }
                                OtherDocumentList.Add(new AttachmentType
                                {
                                    Attachment = new Attachment
                                    {
                                        AttachmentGUID = gisGkhGuids[i]
                                    },
                                    Description = files[i].FullName,
                                    Name = files[i].FullName,
                                    AttachmentHASH = GetGhostHash(_fileManager.GetFile(files[i]))
                                });
                            }
                            // место
                            string place = FindPlace(inspection);
                            // есть нарушения
                            bool hasViol = false;
                            if (actRemoval.TypeRemoval == YesNoNotSet.No)
                            {
                                hasViol = true;
                            }
                            // ссылка на тип документа "Акт" в справочнике ГИС ЖКХ
                            NsiItem actGisDict = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "64" && x.GisGkhItemCode == "1").FirstOrDefault();
                            //var actCheckPeriods = ActRemovalPeriodRepo.GetAll()
                            //    .Where(x => x.ActRemoval == actRemoval).ToList();
                            List<DateTime?> ActDatesStart = new List<DateTime?>();
                            List<DateTime?> ActDatesEnd = new List<DateTime?>();
                            _GisGkhRegionalService.GetActRemovalPeriods(actRemoval, out ActDatesStart, out ActDatesEnd);
                            var actRemovalDays = 0;
                            var actRemovalHours = 0;
                            if (!ActDatesStart.Any())
                            {
                                actRemovalDays = GetDisposalDuration(disposal, actRemoval.DocumentDate);
                                actRemovalHours = actRemovalDays * 8;
                            }
                            else
                            {
                                actRemovalDays = ActDatesStart.Count();
                                DateTime tempHours = DateTime.MinValue;
                                for (int i = 0; i < ActDatesStart.Count; i++)
                                {
                                    if (ActDatesEnd[i] != null && ActDatesStart[i] != null)
                                    {
                                        tempHours += (ActDatesEnd[i].Value - ActDatesStart[i].Value);
                                    }
                                }
                                actRemovalHours = tempHours.Minute > 0 ? tempHours.Hour + 1 : tempHours.Hour;
                                if (actRemovalHours == 0)
                                {
                                    actRemovalHours = 1;
                                }
                            }
                            //var duration = GetDisposalDuration(disposal, actCheck);
                            nsiRef settlPlaceType = null;
                            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "44",
                                    GUID = "5a4e1bf8-a55a-4f9b-bef6-65915c74bfcf",
                                    Name = "Иное"
                                };
                                place = "г. Воронеж, ул. Кирова, д. 6А.";
                            }
                            else
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "2",
                                    GUID = "88f0cc33-4845-40aa-b318-348190ad8892",
                                    Name = "Место фактического осуществления деятельности"
                                };
                            }
                            DateTime? actDateTime = null;
                            if (docChildAct != null)
                            {
                                if (docChildAct.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                {
                                    var acrChect = ActRemovalRepo.Get(docChildAct.Id);
                                    if (acrChect != null)
                                    {
                                        try
                                        {
                                            actDateTime = new DateTime(docChildAct.DocumentDate.Value.Year, docChildAct.DocumentDate.Value.Month, docChildAct.DocumentDate.Value.Day, acrChect.DocumentTime.Value.Hour, acrChect.DocumentTime.Value.Minute, acrChect.DocumentTime.Value.Second);//   acrChect.DocumentTime
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            if (!actDateTime.HasValue)
                            {
                                actDateTime = docChildAct.DocumentDate.Value;
                            }
                            ResultsInfoType resultInfo = new ResultsInfoType
                            {
                                FinishedInfo = new ResultsInfoTypeFinishedInfo
                                {
                                    Result = new ExaminationResultType
                                    {
                                        DocumentType = new nsiRef
                                        {
                                            Code = actGisDict.GisGkhItemCode,
                                            GUID = actGisDict.GisGkhGUID
                                        },
                                        Number = docChildAct.DocumentNumber,
                                        Date = actDateTime.Value,
                                        From = disposal.DateStart.Value.Hour == 0 ? disposal.DateStart.Value.AddHours(9) : disposal.DateStart.Value,
                                        To = actRemoval.DocumentDate.Value,
                                        Duration = new ExaminationResultTypeDuration
                                        {
                                            Days = actRemovalDays,
                                            DaysSpecified = true,
                                            Hours = actRemovalHours,
                                            HoursSpecified = true
                                        },
                                        Place = place,
                                        InspectionPersons = FindActInspectors(docChildAct),
                                        SettlingDocumentPlaceType = settlPlaceType,
                                        SettlingDocumentPlace = place
                                    },
                                    Items = OtherDocumentList.ToArray()
                                },
                                FamiliarizationInfo = new ResultsInfoTypeFamiliarizationInfo
                                {

                                }
                            };

                            if (NotFamiliarize)
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.NotFamiliarize
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            true
                                };
                            }
                            else
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.FamiliarizationDate,
                            ItemsChoiceType9.FamiliarizedPerson,
                                    //ItemsChoiceType8.IsSigned
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            actRemoval.DocumentDate,
                            FamiliarizedPerson,
                                    //true // ставим "подписано"
                                };
                            }
                            if (hasViol)
                            {
                                var existingLongText = this.LongTextService.GetAll().Where(x => x.DocumentGji == actRemoval).FirstOrDefault();
                                var violationsAct = ActRemovalViolationDomain.GetAll()
                                    .Where(x => x.Document.Id == actRemoval.Id)
                                    .Select(x => x.InspectionViolation.Violation.NormativeDocNames).ToList();
                                string regulationOffencedLegalAct = string.Empty;
                                if (violationsAct.Count > 0)
                                {
                                    regulationOffencedLegalAct = violationsAct.AggregateWithSeparator(x => x, ";");
                                }
                                var violationsActNames = ActRemovalViolationDomain.GetAll()
                                   .Where(x => x.Document.Id == actRemoval.Id)
                                   .Select(x => x.InspectionViolation.Violation.Name).ToList();
                                string natureOffence = string.Empty;
                                if (violationsActNames.Count > 0)
                                {
                                    natureOffence = violationsActNames.AggregateWithSeparator(x => x, ";");
                                }
                                // есть нарушение
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasOffence,
                            ItemsChoiceType8.IdentifiedOffencesInfo
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                                    true,
                                    new ExaminationResultTypeIdentifiedOffencesInfo
                                    {
                                        RegulationOffencedLegalAct = regulationOffencedLegalAct,
                                        NatureOffence = natureOffence,
                                        PersonsOffenceList = existingLongText != null? existingLongText.PersonViolationInfo.GetString():"Нет сведений"
                                    }
                                };


                            }
                            //if (hasViol)
                            //{
                            //    // есть нарушение
                            //    resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType7[]
                            //    {
                            //ItemsChoiceType7.HasOffence
                            //    };
                            //    resultInfo.FinishedInfo.Result.Items = new object[]
                            //    {
                            //true
                            //    };
                            //}
                            else
                            {
                                // нет нарушения
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasNoOffence
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                            true
                                };
                            }
                            if (witnessCount == 0)
                            {
                                resultInfo.FinishedInfo.Result.AbsentRepresentatives = true;
                                resultInfo.FinishedInfo.Result.AbsentRepresentativesSpecified = true;
                            }
                            else
                            {
                                resultInfo.FinishedInfo.Result.RepresentativesRegionPersons = witnessesStr;
                            }
                            ActRemovalRepo.Update(actRemoval);
                            return resultInfo;
                        }
                        else
                        {
                            //docChildAct.GisGkhTransportGuid = null;
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        //docChildAct.GisGkhTransportGuid = null;
                        return null;
                    }
                }
                //return null;
            }
            else
            {
                //docChildAct.GisGkhTransportGuid = null;
                return null;
            }
        }

        private ResultsInfoType GetResultInfoForDecision(Decision decision, string OrgPPAGUID)
        {
            // акт
            DocumentGji docChildAct = DocumentGjiChildrenDomain.GetAll()
                            .Where(x => x.Parent.Id == decision.Id)
                            .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                            || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval
                            )
                            .Select(x => x.Children).FirstOrDefault();

            // inspection
            InspectionGji inspection = decision.Inspection;
            if (inspection != null && docChildAct != null)
            {
                // ActCheck
                if (docChildAct.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    try
                    {
                        ActCheck actCheck = ActCheckRepo.Load(docChildAct.Id);
                        actCheck.GisGkhTransportGuid = decision.GisGkhTransportGuid;
                        var actCheckROs = ActCheckRealityObjectDomain.GetAll()
                            .Where(x => x.ActCheck == actCheck).ToList();
                        // лица, присутствовавшие
                        var witnesses = ActCheckWitnessDomain.GetAll()
                            .Where(x => x.ActCheck == actCheck).ToList();
                        // лица, присутствовавшие и ознакомленные
                        string witnessesStr = ""; // присутствовали
                        bool NotFamiliarize = true; // никто не ознакомлен
                        string FamiliarizedPerson = ""; // кто ознакомлен
                        foreach (var witness in witnesses)
                        {
                            if (!string.IsNullOrEmpty(witnessesStr))
                            {
                                witnessesStr += "; ";
                            }
                            witnessesStr += $"{witness.Fio}, {witness.Position}";
                            if (witness.IsFamiliar)
                            {
                                NotFamiliarize = false;
                                if (!string.IsNullOrEmpty(FamiliarizedPerson))
                                {
                                    FamiliarizedPerson += "; ";
                                }
                                FamiliarizedPerson += $"{witness.Fio}";
                            }
                        }
                        var actAnnexes = ActCheckAnnexRepo.GetAll()
                            .Where(x => x.ActCheck == docChildAct)
                             .Where(x => x.File.Extention.ToLower() == "pdf").ToList();
                        if (actAnnexes.Count > 0)
                        {
                            List<AttachmentType> OtherDocumentList = new List<AttachmentType>();
                            foreach (var annex in actAnnexes)
                            {
                                if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                                {
                                    var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                        ActCheckAnnexRepo.Update(annex);
                                    }
                                    else
                                    {
                                        throw new Exception(uploadResult.Message);
                                    }
                                }
                                OtherDocumentList.Add(new AttachmentType
                                {
                                    Attachment = new Attachment
                                    {
                                        AttachmentGUID = annex.GisGkhAttachmentGuid
                                    },
                                    Description = annex.File.FullName,
                                    Name = annex.File.FullName,
                                    AttachmentHASH = GetGhostHash(_fileManager.GetFile(annex.File))
                                });
                            }
                            // место
                            string place = FindPlace(inspection);
                            // есть нарушения
                            bool hasViol = false;
                            foreach (var actCheckRO in actCheckROs)
                            {
                                if (actCheckRO.HaveViolation == YesNoNotSet.Yes)
                                {
                                    hasViol = true;
                                    break;
                                }
                            }
                            // ссылка на тип документа "Акт" в справочнике ГИС ЖКХ
                            NsiItem actGisDict = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "64" && x.GisGkhItemCode == "1").FirstOrDefault();
                            var actCheckPeriods = ActCheckPeriodRepo.GetAll()
                                .Where(x => x.ActCheck == actCheck).ToList();
                            var actCheckDays = 0;
                            var actCheckHours = 0;
                            if (!actCheckPeriods.Any())
                            {
                                actCheckDays = GetDecisionDuration(decision, actCheck.DocumentDate);
                                actCheckHours = actCheckDays * 8;
                            }
                            else
                            {
                                actCheckDays = actCheckPeriods.Count();
                                DateTime tempHours = DateTime.MinValue;
                                foreach (var actCheckPeriod in actCheckPeriods)
                                {
                                    if (actCheckPeriod.DateEnd != null && actCheckPeriod.DateStart != null)
                                    {
                                        tempHours += (actCheckPeriod.DateEnd.Value - actCheckPeriod.DateStart.Value);
                                    }
                                }
                                actCheckHours = tempHours.Minute > 0 ? tempHours.Hour + 1 : tempHours.Hour;
                                if (actCheckHours == 0)
                                {
                                    actCheckHours = 1;
                                }
                            }
                            //var duration = GetDisposalDuration(disposal, actCheck);
                            nsiRef settlPlaceType = null;
                            if (decision.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "44",
                                    GUID = "5a4e1bf8-a55a-4f9b-bef6-65915c74bfcf",
                                    Name = "Иное"
                                };
                                place = "г. Воронеж, ул. Кирова, д. 6А.";
                            }
                            else
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "2",
                                    GUID = "88f0cc33-4845-40aa-b318-348190ad8892",
                                    Name = "Место фактического осуществления деятельности"
                                };
                            }
                            DateTime? actDateTime = null;
                            if (docChildAct != null)
                            {
                                if (docChildAct.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                {
                                    var acrChect = ActCheckRepo.Get(docChildAct.Id);
                                    if (acrChect != null)
                                    {
                                        try
                                        {
                                            actDateTime = new DateTime(docChildAct.DocumentDate.Value.Year, docChildAct.DocumentDate.Value.Month, docChildAct.DocumentDate.Value.Day, acrChect.DocumentTime.Value.Hour, acrChect.DocumentTime.Value.Minute, acrChect.DocumentTime.Value.Second);//   acrChect.DocumentTime
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            if (!actDateTime.HasValue)
                            {
                                actDateTime = docChildAct.DocumentDate.Value;
                            }

                            ResultsInfoType resultInfo = new ResultsInfoType
                            {
                                FinishedInfo = new ResultsInfoTypeFinishedInfo
                                {
                                    Result = new ExaminationResultType
                                    {
                                        InspectionPersonsPosition = FindActInspectorsPositions(docChildAct),
                                        RepresentativesRegionPersons = FindARepresentativersFio(docChildAct),
                                        RepresentativesRegionPersonsPosition = FindARepresentativersPosition(docChildAct),
                                        DocumentType = new nsiRef
                                        {
                                            Code = actGisDict.GisGkhItemCode,
                                            GUID = actGisDict.GisGkhGUID
                                        },
                                        Number = docChildAct.DocumentNumber,
                                        Date = actDateTime.Value,
                                        From = decision.DateStart.Value.Hour == 0 ? decision.DateStart.Value.AddHours(9) : decision.DateStart.Value,
                                        To = actCheck.DocumentDate.Value,
                                        Duration = new ExaminationResultTypeDuration
                                        {
                                            Days = actCheckDays,
                                            DaysSpecified = true,
                                            Hours = actCheckHours,
                                            HoursSpecified = true
                                        },
                                        Place = place,
                                        InspectionPersons = FindActInspectors(docChildAct),
                                        SettlingDocumentPlaceType = settlPlaceType,
                                        SettlingDocumentPlace = place
                                    },
                                    Items = OtherDocumentList.ToArray()
                                },
                                FamiliarizationInfo = new ResultsInfoTypeFamiliarizationInfo
                                {

                                }
                            };

                            if (NotFamiliarize)
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.NotFamiliarize
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            true
                                };
                            }
                            else
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.FamiliarizationDate,
                            ItemsChoiceType9.FamiliarizedPerson,
                                    //ItemsChoiceType8.IsSigned
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            actCheck.DocumentDate,
                            FamiliarizedPerson,
                                    //true // ставим "подписано"
                                };
                            }
                            if (hasViol)
                            {
                                var existingLongText = this.LongTextService.GetAll().Where(x => x.DocumentGji == actCheck).FirstOrDefault();
                                var violationsAct = ActCheckViolationDomain.GetAll()
                                    .Where(x => x.Document.Id == actCheck.Id)
                                    .Select(x => x.InspectionViolation.Violation.NormativeDocNames).ToList();
                                string regulationOffencedLegalAct = string.Empty;
                                if (violationsAct.Count > 0)
                                {
                                    regulationOffencedLegalAct = violationsAct.AggregateWithSeparator(x => x, ";");
                                }
                                var violationsActNames = ActCheckViolationDomain.GetAll()
                                   .Where(x => x.Document.Id == actCheck.Id)
                                   .Select(x => x.InspectionViolation.Violation.Name).ToList();
                                string natureOffence = string.Empty;
                                if (violationsActNames.Count > 0)
                                {
                                    natureOffence = violationsActNames.AggregateWithSeparator(x => x, ";");
                                }
                                // есть нарушение
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasOffence,
                            ItemsChoiceType8.IdentifiedOffencesInfo
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                                    true,
                                    new ExaminationResultTypeIdentifiedOffencesInfo
                                    {
                                        RegulationOffencedLegalAct = regulationOffencedLegalAct,
                                        NatureOffence = natureOffence,
                                        PersonsOffenceList = existingLongText != null? existingLongText.PersonViolationInfo.GetString():"Нет сведений"
                                    }
                                };


                            }
                            else
                            {
                                // нет нарушения
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasNoOffence
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                            true
                                };
                            }
                            if (witnesses.Count == 0)
                            {
                                resultInfo.FinishedInfo.Result.AbsentRepresentatives = true;
                                resultInfo.FinishedInfo.Result.AbsentRepresentativesSpecified = true;
                            }
                            else
                            {
                                resultInfo.FinishedInfo.Result.RepresentativesRegionPersons = witnessesStr;
                            }
                            ActCheckRepo.Update(actCheck);
                            return resultInfo;
                        }
                        else
                        {
                            //docChildAct.GisGkhTransportGuid = null;
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        //docChildAct.GisGkhTransportGuid = null;
                        return null;
                    }
                }
                // ActRemoval
                else
                {
                    try
                    {
                        ChelyabinskActRemoval actRemoval = ActRemovalRepo.Load(docChildAct.Id);
                        actRemoval.GisGkhTransportGuid = decision.GisGkhTransportGuid;
                        string witnessesStr = ""; // присутствовали
                        bool NotFamiliarize = true; // никто не ознакомлен
                        string FamiliarizedPerson = ""; // кто ознакомлен
                        int witnessCount = _GisGkhRegionalService.FindActRemovalWitness(actRemoval, out witnessesStr, out NotFamiliarize, out FamiliarizedPerson);
                        List<Bars.B4.Modules.FileStorage.FileInfo> files = new List<Bars.B4.Modules.FileStorage.FileInfo>();
                        List<string> gisGkhGuids = new List<string>();
                        List<long> ids = new List<long>();
                        //_container.Resolve<IGisGkhRegionalService>().FindActRemovalAnnexes(docChildAct, out ids, out files, out gisGkhGuids);
                        _GisGkhRegionalService.FindActRemovalAnnexes(docChildAct, out ids, out files, out gisGkhGuids);
                        //var actAnnexes = ActRemovalAnnexRepo.GetAll()
                        //    .Where(x => x.ActCheck == docChildAct).ToList();
                        if (files.Count > 0)
                        {
                            List<AttachmentType> OtherDocumentList = new List<AttachmentType>();
                            for (int i = 0; i < files.Count(); i++)
                            {
                                if (string.IsNullOrEmpty(gisGkhGuids[i]))
                                {
                                    var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, files[i], OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        _GisGkhRegionalService.SaveActRemovalAnnex(ids[i], uploadResult.FileGuid);
                                    }
                                    else
                                    {
                                        throw new Exception(uploadResult.Message);
                                    }
                                }
                                OtherDocumentList.Add(new AttachmentType
                                {
                                    Attachment = new Attachment
                                    {
                                        AttachmentGUID = gisGkhGuids[i]
                                    },
                                    Description = files[i].FullName,
                                    Name = files[i].FullName,
                                    AttachmentHASH = GetGhostHash(_fileManager.GetFile(files[i]))
                                });
                            }
                            // место
                            string place = FindPlace(inspection);
                            // есть нарушения
                            bool hasViol = false;
                            if (actRemoval.TypeRemoval == YesNoNotSet.No)
                            {
                                hasViol = true;
                            }
                            // ссылка на тип документа "Акт" в справочнике ГИС ЖКХ
                            NsiItem actGisDict = NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "64" && x.GisGkhItemCode == "1").FirstOrDefault();
                            //var actCheckPeriods = ActRemovalPeriodRepo.GetAll()
                            //    .Where(x => x.ActRemoval == actRemoval).ToList();
                            List<DateTime?> ActDatesStart = new List<DateTime?>();
                            List<DateTime?> ActDatesEnd = new List<DateTime?>();
                            _GisGkhRegionalService.GetActRemovalPeriods(actRemoval, out ActDatesStart, out ActDatesEnd);
                            var actRemovalDays = 0;
                            var actRemovalHours = 0;
                            if (!ActDatesStart.Any())
                            {
                                actRemovalDays = GetDecisionDuration(decision, actRemoval.DocumentDate);
                                actRemovalHours = actRemovalDays * 8;
                            }
                            else
                            {
                                actRemovalDays = ActDatesStart.Count();
                                DateTime tempHours = DateTime.MinValue;
                                for (int i = 0; i < ActDatesStart.Count; i++)
                                {
                                    if (ActDatesEnd[i] != null && ActDatesStart[i] != null)
                                    {
                                        tempHours += (ActDatesEnd[i].Value - ActDatesStart[i].Value);
                                    }
                                }
                                actRemovalHours = tempHours.Minute > 0 ? tempHours.Hour + 1 : tempHours.Hour;
                                if (actRemovalHours == 0)
                                {
                                    actRemovalHours = 1;
                                }
                            }
                            //var duration = GetDisposalDuration(disposal, actCheck);
                            nsiRef settlPlaceType = null;
                            if (decision.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "44",
                                    GUID = "5a4e1bf8-a55a-4f9b-bef6-65915c74bfcf",
                                    Name = "Иное"
                                };
                                place = "г. Воронеж, ул. Кирова, д. 6А.";
                            }
                            else
                            {
                                settlPlaceType = new nsiRef
                                {
                                    Code = "2",
                                    GUID = "88f0cc33-4845-40aa-b318-348190ad8892",
                                    Name = "Место фактического осуществления деятельности"
                                };
                            }
                            DateTime? actDateTime = null;
                            if (docChildAct != null)
                            {
                                if (docChildAct.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                {
                                    var acrChect = ActRemovalRepo.Get(docChildAct.Id);
                                    if (acrChect != null)
                                    {
                                        try
                                        {
                                            actDateTime = new DateTime(docChildAct.DocumentDate.Value.Year, docChildAct.DocumentDate.Value.Month, docChildAct.DocumentDate.Value.Day, acrChect.DocumentTime.Value.Hour, acrChect.DocumentTime.Value.Minute, acrChect.DocumentTime.Value.Second);//   acrChect.DocumentTime
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                            if (!actDateTime.HasValue)
                            {
                                actDateTime = docChildAct.DocumentDate.Value;
                            }
                            ResultsInfoType resultInfo = new ResultsInfoType
                            {
                                FinishedInfo = new ResultsInfoTypeFinishedInfo
                                {
                                    Result = new ExaminationResultType
                                    {
                                        DocumentType = new nsiRef
                                        {
                                            Code = actGisDict.GisGkhItemCode,
                                            GUID = actGisDict.GisGkhGUID
                                        },
                                        Number = docChildAct.DocumentNumber,
                                        Date = actDateTime.Value,
                                        From = decision.DateStart.Value.Hour == 0 ? decision.DateStart.Value.AddHours(9) : decision.DateStart.Value,
                                        To = actRemoval.DocumentDate.Value,
                                        Duration = new ExaminationResultTypeDuration
                                        {
                                            Days = actRemovalDays,
                                            DaysSpecified = true,
                                            Hours = actRemovalHours,
                                            HoursSpecified = true
                                        },
                                        Place = place,
                                        InspectionPersons = FindActInspectors(docChildAct),
                                        SettlingDocumentPlaceType = settlPlaceType,
                                        SettlingDocumentPlace = place
                                    },
                                    Items = OtherDocumentList.ToArray()
                                },
                                FamiliarizationInfo = new ResultsInfoTypeFamiliarizationInfo
                                {

                                }
                            };

                            if (NotFamiliarize)
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.NotFamiliarize
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            true
                                };
                            }
                            else
                            {
                                resultInfo.FamiliarizationInfo.ItemsElementName = new ItemsChoiceType9[]
                                {
                            ItemsChoiceType9.FamiliarizationDate,
                            ItemsChoiceType9.FamiliarizedPerson,
                                    //ItemsChoiceType8.IsSigned
                                };

                                resultInfo.FamiliarizationInfo.Items = new object[]
                                {
                            actRemoval.DocumentDate,
                            FamiliarizedPerson,
                                    //true // ставим "подписано"
                                };
                            }
                            if (hasViol)
                            {
                                var existingLongText = this.LongTextService.GetAll().Where(x => x.DocumentGji == actRemoval).FirstOrDefault();
                                var violationsAct = ActRemovalViolationDomain.GetAll()
                                    .Where(x => x.Document.Id == actRemoval.Id)
                                    .Select(x => x.InspectionViolation.Violation.NormativeDocNames).ToList();
                                string regulationOffencedLegalAct = string.Empty;
                                if (violationsAct.Count > 0)
                                {
                                    regulationOffencedLegalAct = violationsAct.AggregateWithSeparator(x => x, ";");
                                }
                                var violationsActNames = ActRemovalViolationDomain.GetAll()
                                   .Where(x => x.Document.Id == actRemoval.Id)
                                   .Select(x => x.InspectionViolation.Violation.Name).ToList();
                                string natureOffence = string.Empty;
                                if (violationsActNames.Count > 0)
                                {
                                    natureOffence = violationsActNames.AggregateWithSeparator(x => x, ";");
                                }
                                // есть нарушение
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasOffence,
                            ItemsChoiceType8.IdentifiedOffencesInfo
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                                    true,
                                    new ExaminationResultTypeIdentifiedOffencesInfo
                                    {
                                        RegulationOffencedLegalAct = regulationOffencedLegalAct,
                                        NatureOffence = natureOffence,
                                        PersonsOffenceList = existingLongText != null? existingLongText.PersonViolationInfo.GetString():"Нет сведений"
                                    }
                                };


                            }
                            //if (hasViol)
                            //{
                            //    // есть нарушение
                            //    resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType7[]
                            //    {
                            //ItemsChoiceType7.HasOffence
                            //    };
                            //    resultInfo.FinishedInfo.Result.Items = new object[]
                            //    {
                            //true
                            //    };
                            //}
                            else
                            {
                                // нет нарушения
                                resultInfo.FinishedInfo.Result.ItemsElementName = new ItemsChoiceType8[]
                                {
                            ItemsChoiceType8.HasNoOffence
                                };
                                resultInfo.FinishedInfo.Result.Items = new object[]
                                {
                            true
                                };
                            }
                            if (witnessCount == 0)
                            {
                                resultInfo.FinishedInfo.Result.AbsentRepresentatives = true;
                                resultInfo.FinishedInfo.Result.AbsentRepresentativesSpecified = true;
                            }
                            else
                            {
                                resultInfo.FinishedInfo.Result.RepresentativesRegionPersons = witnessesStr;
                            }
                            ActRemovalRepo.Update(actRemoval);
                            return resultInfo;
                        }
                        else
                        {
                            //docChildAct.GisGkhTransportGuid = null;
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        //docChildAct.GisGkhTransportGuid = null;
                        return null;
                    }
                }
                //return null;
            }
            else
            {
                //docChildAct.GisGkhTransportGuid = null;
                return null;
            }
        }

        #endregion

    }
}
