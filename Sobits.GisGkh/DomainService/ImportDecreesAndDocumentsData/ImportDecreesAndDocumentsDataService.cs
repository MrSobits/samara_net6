using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.FileManager;
using Bars.GkhGji.DomainService;
using Bars.GkhGji.DomainService.GisGkhRegional;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;
//using CryptoPro.Sharpei;
using GisGkhLibrary.RapServiceAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using Sobits.GisGkh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ImportDecreesAndDocumentsDataService : IImportDecreesAndDocumentsDataService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<NsiItem> NsiItemDomain { get; set; }

        public IRepository<ResolutionAnnex> ResolutionAnnexRepo { get; set; }
        public IRepository<Resolution> ResolutionRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        private IFileService _fileService;

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        private IResolutionService _ResolutionService;
        private IExportOrgRegistryService _ExportOrgRegistryService;

        #endregion

        #region Constructors

        public ImportDecreesAndDocumentsDataService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService FileService,
            IResolutionService resolutionService, IExportOrgRegistryService exportOrgRegistryService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _fileService = FileService;
            _ResolutionService = resolutionService;
            _ExportOrgRegistryService = exportOrgRegistryService;
        }

        #endregion

        #region Public methods

        // Запрос на выгрузку постановлений по датам
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

                log += $"{DateTime.Now} Формирование запроса на выгрузку постановлений в ГИС ЖКХ по датам: с {from.ToShortDateString()} по {to.ToShortDateString()}\r\n";
                // Все постановления по датам, независимо от наличия ГУИДов и т.д.
                List<string> resolutionIds = ResolutionRepo.GetAll()
                     .Where(x => x.DocumentDate >= from && x.DocumentDate <= to).Select(x => x.Id.ToString()).ToList();
                log += $"{DateTime.Now} Формирование запроса по списку постановлений.\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);

                SaveCurrentRequest(req, resolutionIds.ToArray(), OrgPPAGUID);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        // Запрос на выгрузку постановлений по id
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
                List<string> resolutionIds = reqParams.ToList();
                Dictionary<bool, NsiItem> IsCancelDict = new Dictionary<bool, NsiItem>
                {
                    { false, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "227" && x.GisGkhItemCode == "1").FirstOrDefault() },
                    { true, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "227" && x.GisGkhItemCode == "2").FirstOrDefault() }
                };

                Dictionary<bool, NsiItem> DocumentKindDict = new Dictionary<bool, NsiItem>
                {
                    { false, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "226" && x.GisGkhItemCode == "8").FirstOrDefault() },
                    { true, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "226" && x.GisGkhItemCode == "9").FirstOrDefault() }
                };

                Dictionary<TypeInitiativeOrgGji, NsiItem> TakingDecisionAuthorityDict = new Dictionary<TypeInitiativeOrgGji, NsiItem>
                {
                    { TypeInitiativeOrgGji.Court, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "332" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { TypeInitiativeOrgGji.HousingInspection, NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "332" && x.GisGkhItemCode == "9").FirstOrDefault() }
                };

                Dictionary<string, NsiItem> PunishmentKindDict = new Dictionary<string, NsiItem>
                {
                    { "Административный штраф", NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "225" && x.GisGkhItemCode == "1").FirstOrDefault() },
                    { "Предупреждение", NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "225" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { "Устное замечание", NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "225" && x.GisGkhItemCode == "2").FirstOrDefault() },
                    { "Дисквалификация", NsiItemDomain.GetAll().Where(x => x.NsiList.GisGkhCode == "225" && x.GisGkhItemCode == "3").FirstOrDefault() }
                };
                List<ImportDecreesAndDocumentsRequestImportDecreesAndDocuments> decrees = new List<ImportDecreesAndDocumentsRequestImportDecreesAndDocuments>();

                foreach (var resolutionId in resolutionIds)
                {
                    Resolution resolution = ResolutionRepo.Load(long.Parse(resolutionId));
                    // если конечный статус
                    log += $"{DateTime.Now} Постановление {resolution.DocumentNumber} от {(resolution.DocumentDate.HasValue ? resolution.DocumentDate.Value.ToShortDateString() : "-")}\r\n";
                    if (resolution.State.FinalState)
                    {
                        try
                        {
                            // постановления целиком
                            if (string.IsNullOrEmpty(resolution.GisGkhGuid))
                            {

                                if (!string.IsNullOrEmpty(resolution.GisGkhTransportGuid))
                                {
                                    // todo: тут что-то, если уже есть транспорт Гуид
                                    log += $"{DateTime.Now} У постановления уже есть TransportGUID\r\n";
                                }
                                else
                                {
                                    var exType = _container.Resolve<IGisGkhRegionalService>().GetTypeExecutant(resolution);
                                    if (exType == TypeExecutantForGisGkh.NotSet)
                                    {
                                        log += $"{DateTime.Now} Неизвестен тип исполнителя. Постановление не будет выгружено\r\n";
                                        continue;
                                    }
                                    if (exType == TypeExecutantForGisGkh.DL && (string.IsNullOrEmpty(resolution.Position) || string.IsNullOrEmpty(resolution.FirstName) || string.IsNullOrEmpty(resolution.Surname)))
                                    {
                                        log += $"{DateTime.Now} Исполнитель - должностное лицо. Требуется указание должности и ФИО исполнителя. Постановление не будет выгружено\r\n";
                                        continue;
                                    }
                                    List<ArticleLawGji> artLaws = _container.Resolve<IGisGkhRegionalService>().GetResolutionArtLaw(resolution);
                                    if (artLaws.Count() == 0)
                                    {
                                        log += $"{DateTime.Now} Не указаны статьи закона в постановлении. Постановление не будет выгружено\r\n";
                                        continue;
                                    }
                                    var Offender = new DecreeInfoTypeOffender();
                                    // постановление физлицу
                                    if (resolution.Contragent == null)
                                    {
                                        var Person = new DecreeIndType
                                        {
                                            FirstName = resolution.FirstName,
                                            Surname = resolution.Surname
                                        };
                                        if (!string.IsNullOrEmpty(resolution.Patronymic))
                                        {
                                            Person.Patronymic = resolution.Patronymic;
                                        }
                                        Offender.Items = new object[]
                                        {
                                            Person
                                        };
                                    }
                                    // постановление юрлицу
                                    else
                                    {
                                        if (string.IsNullOrEmpty(resolution.Contragent.GisGkhGUID))
                                        {
                                            // организация не сопоставлена
                                            try
                                            {
                                                _ExportOrgRegistryService.SaveRequest(null, new List<long> { resolution.Contragent.Id });
                                                log += $"{DateTime.Now} Организация - исполнитель не сопоставлена с ГИС ЖКХ. Создан запрос получения информации об организации. Постановление не будет выгружено\r\n";
                                                continue;
                                            }
                                            catch (Exception e)
                                            {
                                                log += $"{DateTime.Now} Организация - исполнитель не сопоставлена с ГИС ЖКХ. Ошибка при создании запроса получения информации об организации {e.Message}. Постановление не будет выгружено\r\n";
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            List<object> objects = new List<object>
                                            {
                                                new RegOrgType
                                                {
                                                    orgRootEntityGUID = resolution.Contragent.GisGkhGUID
                                                }
                                            };
                                            // если заполнено должностное лицо
                                            if (!string.IsNullOrEmpty(resolution.FirstName) &&
                                                !string.IsNullOrEmpty(resolution.Surname) &&
                                                !string.IsNullOrEmpty(resolution.Position))
                                            {
                                                var Official = new DecreeInfoTypeOffenderOfficialName
                                                {
                                                    FirstName = resolution.FirstName,
                                                    Surname = resolution.Surname,
                                                    Position = resolution.Position
                                                };
                                                if (!string.IsNullOrEmpty(resolution.Patronymic))
                                                {
                                                    Official.Patronymic = resolution.Patronymic;
                                                }
                                                objects.Add(Official);
                                            }
                                            Offender.Items = objects.ToArray();
                                        }
                                    }

                                    // Приложения к постановлению
                                    var resolutionAnnexes = ResolutionAnnexRepo.GetAll()
                                        .Where(x => x.Resolution == resolution && x.File.Extention.ToLower() == "pdf").ToList();
                                    List<AttachmentType> Attachments = new List<AttachmentType>();
                                    foreach (var annex in resolutionAnnexes)
                                    {
                                        if (string.IsNullOrEmpty(annex.GisGkhAttachmentGuid))
                                        {
                                            var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, annex.File, OrgPPAGUID);

                                            if (uploadResult.Success)
                                            {
                                                annex.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                                ResolutionAnnexRepo.Update(annex);
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

                                    bool isCancel = resolution.Sanction.Name == "Прекращено" || resolution.Sanction.Name == "Устное замечание" ? true : false;

                                    var DecreeDocument = new ProceduralDocumentType
                                    {
                                        DocumentKind = new nsiRef
                                        {
                                            Code = DocumentKindDict[isCancel].GisGkhItemCode,
                                            GUID = DocumentKindDict[isCancel].GisGkhGUID
                                        },
                                        DocumentName = isCancel ? "Постановление о прекращении производства по делу об административном правонарушении" :
                                                                    "Постановление о назначении административного наказания",
                                        DocumentNumber = resolution.DocumentNumber,
                                        DocumentDate = resolution.DocumentDate.Value,
                                        Document = Attachments.ToArray()
                                    };

                                    List<nsiRef> artLawRefs = new List<nsiRef>();
                                    foreach (var artLaw in artLaws)
                                    {
                                        artLawRefs.Add(new nsiRef
                                        {
                                            Code = artLaw.GisGkhCode,
                                            GUID = artLaw.GisGkhGuid
                                        });
                                    }

                                    var DecreeItem = new ImportDecreesAndDocumentsRequestImportDecreesAndDocumentsDecree
                                    {
                                        DecreeInfo = new ImportDecreesAndDocumentsRequestImportDecreesAndDocumentsDecreeDecreeInfo
                                        {
                                            Offender = Offender,
                                            ReviewResult = new DecreeInfoTypeReviewResult
                                            {
                                                ResultKind = new nsiRef
                                                {
                                                    Code = IsCancelDict[isCancel].GisGkhItemCode,
                                                    GUID = IsCancelDict[isCancel].GisGkhGUID
                                                },
                                                DecreeDocument = DecreeDocument,
                                                TakingDecisionAuthority = new nsiRef
                                                {
                                                    Code = TakingDecisionAuthorityDict[resolution.TypeInitiativeOrg].GisGkhItemCode,
                                                    GUID = TakingDecisionAuthorityDict[resolution.TypeInitiativeOrg].GisGkhGUID
                                                },
                                                TakingDecisionAuthorityName = resolution.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ?
                                                    _ResolutionService.GetTakingDecisionAuthorityName() :
                                                    (resolution.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ?
                                                    (resolution.JudicalOffice != null ? resolution.JudicalOffice.Name : "Суд") :
                                                    "-"),
                                                PositionOfAcceptedPerson = resolution.TypeInitiativeOrg == TypeInitiativeOrgGji.Court ?
                                                    "Судья" : (resolution.Official != null ? resolution.Official.Position : "-"),
                                                ViolationObject = artLawRefs.ToArray(),
                                                ProceedingDate = resolution.DocumentDate.Value,
                                                ViolationShortDescription = resolution.Violation
                                            }
                                        }
                                    };
                                    if (!isCancel)
                                    {
                                        DecreeItem.DecreeInfo.ReviewResult.PunishmentKind = new nsiRef[]
                                        {
                                            new nsiRef
                                            {
                                                Code = PunishmentKindDict[resolution.Sanction.Name].GisGkhItemCode,
                                                GUID = PunishmentKindDict[resolution.Sanction.Name].GisGkhGUID
                                            }
                                        };
                                        if (DecreeItem.DecreeInfo.ReviewResult.PunishmentKind.Where(x => x.Code == "1").Any() &&
                                            resolution.PenaltyAmount != null)
                                        {
                                            DecreeItem.DecreeInfo.ReviewResult.Fine = resolution.PenaltyAmount.Value.ToMagic(2);
                                            DecreeItem.DecreeInfo.ReviewResult.FineSpecified = true;
                                        }
                                        else if (DecreeItem.DecreeInfo.ReviewResult.PunishmentKind.Where(x => x.Code == "3").Any())
                                        {
                                            DecreeItem.DecreeInfo.ReviewResult.DisqualificationPeriod = new DecreeInfoTypeReviewResultDisqualificationPeriod
                                            {
                                                Item = 1,
                                                ItemElementName = ItemChoiceType3.Years
                                            };
                                        }
                                    }
                                    resolution.GisGkhTransportGuid = Guid.NewGuid().ToString();
                                    ImportDecreesAndDocumentsRequestImportDecreesAndDocuments decree = new ImportDecreesAndDocumentsRequestImportDecreesAndDocuments
                                    {
                                        Items = new object[]
                                        {
                                            DecreeItem
                                        },
                                        TransportGuid = resolution.GisGkhTransportGuid
                                    };
                                    ResolutionRepo.Update(resolution);
                                    decrees.Add(decree);
                                    log += "добавлено в запрос\r\n";
                                }
                            }
                            else
                            {
                                //уже есть гуид
                                log += $"{DateTime.Now} У постановления уже есть GUID\r\n";
                            }
                        }
                        catch (Exception e)
                        {
                            //disposal.GisGkhTransportGuid = null;
                            throw new Exception("Ошибка: " + e.Message);
                        }
                    }
                    else
                    {
                        // resolution не в finalState
                        log += $"{DateTime.Now} Статус постановления не конечный\r\n";
                    }
                }
                if (decrees.Count > 0)
                {
                    var request = RapServiceAsync.importDecreesAndDocumentsDataReq(decrees.ToArray());
                    var prefixer = new GisGkhLibrary.Utils.XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    req.Answer = "Запрос на выгрузку постановлений сформирован";
                    log += $"{DateTime.Now} Запрос на выгрузку {decrees.Count} постановлений сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                }
                else
                {
                    req.RequestState = GisGkhRequestState.NotFormed;
                    req.Answer = "Выбранные постановления не требуют отправки в ГИС ЖКХ";
                    log += $"{DateTime.Now} Нет подходящих постановлений для выгрузки\r\n";
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
                var response = RapServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
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
                            req.Answer = $"Задача на обработку ответа importDecreesAndDocumentsData поставлена в очередь с id {taskInfo.TaskId}";
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
                            else if (responseItem is CommonResultType) // тут постановление
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
                                var resolution = ResolutionRepo.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == ((CommonResultType)responseItem).TransportGUID).FirstOrDefault();
                                if (resolution != null)
                                {
                                    resolution.GisGkhGuid = ((CommonResultType)responseItem).GUID;
                                    resolution.GisGkhTransportGuid = null;
                                    ResolutionRepo.Update(resolution);
                                    if (!error)
                                    {
                                        log += $"{DateTime.Now} Постановление {resolution.DocumentNumber} от " +
                                            $"{(resolution.DocumentDate.HasValue ? resolution.DocumentDate.Value.ToShortDateString() : "")} " +
                                            $"выгруженo в ГИС ЖКХ с GUID {resolution.GisGkhGuid}\r\n";
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

            // TODO: Заменить Криптопро
            /*using (var gost = Gost3411.Create())
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

        #endregion

    }
}
