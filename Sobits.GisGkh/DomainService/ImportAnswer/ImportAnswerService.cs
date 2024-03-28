using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
using Bars.GkhGji.DomainService;
using Bars.GkhGji.DomainService.GisGkhRegional;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;
//using CryptoPro.Sharpei;
using GisGkhLibrary.AppealsServiceAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
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
    public class ImportAnswerService : IImportAnswerService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }
        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        private IFileService _fileService;
        private IGisGkhRegionalService _gisRegionalService;

        #endregion

        #region Constructors

        public ImportAnswerService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService FileService, IGisGkhRegionalService gisRegionalService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _fileService = FileService;
            _gisRegionalService = gisRegionalService;
        }

        #endregion

        #region Public methods

        public void SaveAppealRequest(GisGkhRequests req, AppealCits appeal, Inspector inspector)
        {
            var gisNum = AppealCitsSourceDomain.GetAll().Where(x => x.AppealCits == appeal).FirstOrDefault()?.RevenueSourceNumber;
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Формирование запроса на назначение проверяющего и контрольного срока обращения {appeal.NumberGji} от " +
                    $"{(appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : "-")} (ГИС ЖКХ {gisNum})\r\n";
                appeal.GisGkhTransportGuid = Guid.NewGuid().ToString();
                AppealCitsDomain.Update(appeal);
                importAnswerRequestAppealAction appealAction = new importAnswerRequestAppealAction
                {
                    TransportGUID = appeal.GisGkhTransportGuid,
                    ItemElementName = ItemChoiceType2.AppealGUID,
                    Item = appeal.GisGkhGuid,
                    Performer = new importAnswerRequestAppealActionPerformer
                    {
                        AnswererGUID = inspector.GisGkhGuid
                    }
                };
                if (appeal.CheckTime.HasValue)
                {
                    IGisGkhRegionalService gisGkhRegionalService = Container.Resolve<IGisGkhRegionalService>();
                    try
                    {
                        appealAction.Performer.DateOfAppointment = gisGkhRegionalService.GetDateOfAppointment(appeal);
                        appealAction.Performer.DateOfAppointmentSpecified = true;
                    }
                    catch
                    {
                        // нет контрольного срока
                    }
                    finally
                    {
                        Container.Release(gisGkhRegionalService);
                    }
                }
                var request = AppealsServiceAsync.importAnswerReq(new importAnswerRequestAppealAction[] { appealAction });
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                req.Answer = "Запрос на назначение проверяющего и контрольного срока сформирован";
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void SaveAnswerRequest(GisGkhRequests req, AppealCits appeal, AppealAnswerType closeType, string OrgPPAGUID)
        {
            var gisNum = AppealCitsSourceDomain.GetAll().Where(x => x.AppealCits == appeal).FirstOrDefault()?.RevenueSourceNumber;
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Формирование запроса на выгрузку в ГИС ЖКХ ответа по обращению {appeal.NumberGji} от " +
                    $"{(appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : "-")} (ГИС ЖКХ {gisNum})\r\n";
                appeal.GisGkhTransportGuid = Guid.NewGuid().ToString();
                AppealCitsDomain.Update(appeal);
                object request = null;
                XmlNsPrefixer prefixer = new XmlNsPrefixer();
                XmlDocument document = null;

                var answers = _gisRegionalService.GetAppCitAnswersForGisGkh(appeal, closeType);
                switch (closeType)
                {
                    case AppealAnswerType.Answer:
                    case AppealAnswerType.RedirectAndAnswer:
                        List<importAnswerRequestAnswerActionAnswer> gisAnswers = new List<importAnswerRequestAnswerActionAnswer>();
                        foreach (var answer in answers)
                        {
                            answer.GisGkhTransportGuid = Guid.NewGuid().ToString();
                            AppealCitsAnswerDomain.Update(answer);
                            importAnswerRequestAnswerActionAnswerLoadAnswer loadAnswer = new importAnswerRequestAnswerActionAnswerLoadAnswer
                            {
                                AnswerText = answer.Description
                            };
                            // Приложение к ответу
                            if (answer.File != null)
                            {
                                if (string.IsNullOrEmpty(answer.GisGkhAttachmentGuid))
                                {
                                    var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, answer.File, OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        answer.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                        AppealCitsAnswerDomain.Update(answer);
                                    }
                                    else
                                    {
                                        throw new Exception(uploadResult.Message);
                                    }
                                }
                                loadAnswer.Attachment = new AttachmentType[]
                                {
                                    new AttachmentType
                                    {
                                        Attachment = new Attachment
                                        {
                                            AttachmentGUID = answer.GisGkhAttachmentGuid
                                        },
                                        Description = answer.File.FullName,
                                        Name = answer.File.FullName,
                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(answer.File))
                                    }
                                };
                            }
                            importAnswerRequestAnswerActionAnswer gisAnswer = new importAnswerRequestAnswerActionAnswer
                            {
                                TransportGUID = answer.GisGkhTransportGuid,
                                AnswererGUID = answer.Executor.GisGkhGuid,
                                ItemsElementName = new ItemsChoiceType6[]
                                {
                                    ItemsChoiceType6.LoadAnswer,
                                    //ItemsChoiceType6.SendAnswer
                                },
                                Items = new object[]
                                {
                                    loadAnswer,
                                    //false // отправлять ответ или нет
                                }
                            };
                            gisAnswers.Add(gisAnswer);
                        }
                        importAnswerRequestAnswerAction answerAction = new importAnswerRequestAnswerAction
                        {
                            TransportGUID = appeal.GisGkhTransportGuid,
                            ItemElementName = ItemChoiceType1.AppealGUID,
                            Item = appeal.GisGkhGuid,
                            Answer = gisAnswers.ToArray(),
                        };
                        request = AppealsServiceAsync.importAnswerReq(null, new importAnswerRequestAnswerAction[] { answerAction });
                        req.Answer = $"*{appeal.NumberGji} (ГИС ЖКХ {gisNum})* - запрос на выгрузку ответа сформирован";
                        log += $"{DateTime.Now} Запрос на выгрузку ответа ответа сформирован\r\n";
                        break;
                    case AppealAnswerType.NotNeedAnswer:
                        importAnswerRequestAppealAction appealActionNotNeed = new importAnswerRequestAppealAction
                        {
                            TransportGUID = appeal.GisGkhTransportGuid,
                            ItemElementName = ItemChoiceType2.AppealGUID,
                            Item = appeal.GisGkhGuid,
                            AnswerIsNotRequired = new importAnswerRequestAppealActionAnswerIsNotRequired
                            {
                                Cause = answers[0].Description,
                                AnswererGUID = answers[0].Executor.GisGkhGuid
                            }
                        };
                        request = AppealsServiceAsync.importAnswerReq(new importAnswerRequestAppealAction[] { appealActionNotNeed });
                        req.Answer = $"*{appeal.NumberGji} (ГИС ЖКХ {gisNum})* - запрос при отсутствии необходимости ответа сформирован";
                        log += $"{DateTime.Now} Запрос при отсутствии необходимости ответа сформирован\r\n";
                        break;
                    case AppealAnswerType.Redirect:
                        importAnswerRequestAppealAction appealActionRedirect = new importAnswerRequestAppealAction
                        {
                            TransportGUID = appeal.GisGkhTransportGuid,
                            ItemElementName = ItemChoiceType2.AppealGUID,
                            Item = appeal.GisGkhGuid,
                            Executed = new importAnswerRequestAppealActionExecuted
                            {
                                AnswererGUID = answers[0].Executor.GisGkhGuid
                            }
                        };
                        break;
                    case AppealAnswerType.NotSet:
                        log += $"{DateTime.Now} Неизвестный тип ответа. Запрос не сформирован\r\n";
                        break;
                }
                document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message + " " + e.StackTrace);
            }
        }

        public void SaveRollOverRequest(GisGkhRequests req, AppealCits appeal, string OrgPPAGUID)
        {
            var gisNum = AppealCitsSourceDomain.GetAll().Where(x => x.AppealCits == appeal).FirstOrDefault()?.RevenueSourceNumber;
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Формирование запроса на продление срока рассмотрения обращения {appeal.NumberGji} от " +
                    $"{(appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : "-")} (ГИС ЖКХ {gisNum})\r\n";
                appeal.GisGkhTransportGuid = Guid.NewGuid().ToString();
                AppealCitsDomain.Update(appeal);

                var rollOverAnswers = _gisRegionalService.GetAppCitRollOverAnswersForGisGkh(appeal);
                if (rollOverAnswers[0].Executor == null)
                {
                    log += $"{DateTime.Now} Не указан исполнитель ответа. Запрос не сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Не указан исполнитель ответа");
                }
                if (string.IsNullOrEmpty(rollOverAnswers[0].Executor.GisGkhGuid))
                {
                    log += $"{DateTime.Now} У исполнителя ответа отсутствует идентификатор ГИС ЖКХ. Запрос не сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("У исполнителя ответа отсутствует идентификатор ГИС ЖКХ");
                }
                if (!appeal.ExtensTime.HasValue)
                {
                    log += $"{DateTime.Now} Не указан продлённый срок рассмотрения обращения. Запрос не сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Не указан продлённый срок рассмотрения обращения");
                }
                if (appeal.ExtensTime.Value.Date > DateTime.Now.AddDays(30))
                {
                    log += $"{DateTime.Now} Продлённый срок не может быть позднее, чем текущий день + 30 календарных дней\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Продлённый срок не может быть позднее, чем текущий день + 30 календарных дней");
                }
                importAnswerRequestAppealAction appealActionRollOver = new importAnswerRequestAppealAction
                {
                    TransportGUID = appeal.GisGkhTransportGuid,
                    ItemElementName = ItemChoiceType2.AppealGUID,
                    Item = appeal.GisGkhGuid,
                    RollOverAppeal = new importAnswerRequestAppealActionRollOverAppeal
                    {
                        AnswererGUID = rollOverAnswers[0].Executor.GisGkhGuid,
                        Comment = rollOverAnswers[0].Description,
                        ExecutionDate = appeal.ExtensTime.Value
                    }
                };
                // Приложение к продлению
                if (rollOverAnswers[0].File != null)
                {
                    if (string.IsNullOrEmpty(rollOverAnswers[0].GisGkhAttachmentGuid))
                    {
                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, rollOverAnswers[0].File, OrgPPAGUID);

                        if (uploadResult.Success)
                        {
                            rollOverAnswers[0].GisGkhAttachmentGuid = uploadResult.FileGuid;
                            AppealCitsAnswerDomain.Update(rollOverAnswers[0]);
                        }
                        else
                        {
                            throw new Exception(uploadResult.Message);
                        }
                    }
                    appealActionRollOver.RollOverAppeal.Attachment = new AttachmentType[]
                    {
                        new AttachmentType
                        {
                            Attachment = new Attachment
                            {
                                AttachmentGUID = rollOverAnswers[0].GisGkhAttachmentGuid
                            },
                            Description = rollOverAnswers[0].File.FullName,
                            Name = rollOverAnswers[0].File.FullName,
                            AttachmentHASH = GetGhostHash(_fileManager.GetFile(rollOverAnswers[0].File))
                        }
                    };
                }
                var request = AppealsServiceAsync.importAnswerReq(new importAnswerRequestAppealAction[] { appealActionRollOver });
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.Answer = $"*{appeal.NumberGji} (ГИС ЖКХ {gisNum})* - запрос на продление рассмотрения сформирован";
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос на продление срока рассмотрения обращения сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void SaveRedirectRequest(GisGkhRequests req, AppealCits appeal, string OrgPPAGUID)
        {
            var gisNum = AppealCitsSourceDomain.GetAll().Where(x => x.AppealCits == appeal).FirstOrDefault()?.RevenueSourceNumber;
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Формирование запроса на переадресацию обращения {appeal.NumberGji} от " +
                    $"{(appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : "-")} (ГИС ЖКХ {gisNum})\r\n";
                appeal.GisGkhTransportGuid = Guid.NewGuid().ToString();
                AppealCitsDomain.Update(appeal);

                var redirectAnswers = _gisRegionalService.GetAppCitRedirectAnswersForGisGkh(appeal);
                if (redirectAnswers[0].Executor == null)
                {
                    log += $"{DateTime.Now} Не указан исполнитель ответа с переадресацией. Запрос не сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Не указан исполнитель ответа с переадресацией");
                }
                if (string.IsNullOrEmpty(redirectAnswers[0].Executor.GisGkhGuid))
                {
                    log += $"{DateTime.Now} У исполнителя ответа с переадресацией отсутствует идентификатор ГИС ЖКХ. Запрос не сформирован\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("У исполнителя ответа с переадресацией отсутствует идентификатор ГИС ЖКХ");
                }
                if (redirectAnswers[0].RedirectContragent == null)
                {
                    log += $"{DateTime.Now} При перенаправлении обращение из ГИС ЖКХ обязательно указание контрагента \"Кому перенаправлено\" внутри ответа. Запрос не сформирован.\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("При перенаправлении обращение из ГИС ЖКХ обязательно указание контрагента \"Кому перенаправлено\" внутри ответа");
                }
                else if (string.IsNullOrEmpty(redirectAnswers[0].RedirectContragent.GisGkhGUID))
                {
                    IExportOrgRegistryService exportOrgRegistryService = Container.Resolve<IExportOrgRegistryService>();
                    try
                    {
                        log += $"{DateTime.Now} У организации, которой перенаправляется обращение, отсутствует идентификатор. Формируем запрос на получение информации об организации\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                        exportOrgRegistryService.SaveRequest(null, new List<long> { redirectAnswers[0].RedirectContragent.Id });
                        //throw new Exception("У организации, которой перенаправляется обращение, отсутствует идентификатор. Создан запрос на получение информации об организации");
                    }
                    catch (Exception e)
                    {
                        log += $"{DateTime.Now} Не удалось создать запрос на получение информации об организации\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                        //throw new Exception("У организации, которой перенаправляется обращение, отсутствует идентификатор. Не удалось создать запрос на получение информации об организации");
                    }
                    finally
                    {
                        Container.Release(exportOrgRegistryService);
                        throw new Exception("У организации, которой перенаправляется обращение, отсутствует идентификатор");
                    }
                }
                importAnswerRequestAppealAction appealActionRedirect = new importAnswerRequestAppealAction
                {
                    TransportGUID = appeal.GisGkhTransportGuid,
                    ItemElementName = ItemChoiceType2.AppealGUID,
                    Item = appeal.GisGkhGuid,
                    RedirectAppeal = new importAnswerRequestAppealActionRedirectAppeal[]
                    {
                        new importAnswerRequestAppealActionRedirectAppeal
                        {
                            AnswererGUID = redirectAnswers[0].Executor.GisGkhGuid,
                            Comment = redirectAnswers[0].Description,
                            Receiver = new RedirectAppealTypeReceiver
                            {
                                orgRootEntityGUID = redirectAnswers[0].RedirectContragent.GisGkhGUID
                            }
                        }
                    }
                };
                // Приложение к редиректу
                if (redirectAnswers[0].File != null)
                {
                    if (string.IsNullOrEmpty(redirectAnswers[0].GisGkhAttachmentGuid))
                    {
                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, redirectAnswers[0].File, OrgPPAGUID);

                        if (uploadResult.Success)
                        {
                            redirectAnswers[0].GisGkhAttachmentGuid = uploadResult.FileGuid;
                            AppealCitsAnswerDomain.Update(redirectAnswers[0]);
                        }
                        else
                        {
                            throw new Exception(uploadResult.Message);
                        }
                    }
                    appealActionRedirect.RedirectAppeal[0].Attachment = new AttachmentType[]
                    {
                        new AttachmentType
                        {
                            Attachment = new Attachment
                            {
                                AttachmentGUID = redirectAnswers[0].GisGkhAttachmentGuid
                            },
                            Description = redirectAnswers[0].File.FullName,
                            Name = redirectAnswers[0].File.FullName,
                            AttachmentHASH = GetGhostHash(_fileManager.GetFile(redirectAnswers[0].File))
                        }
                    };
                }
                var request = AppealsServiceAsync.importAnswerReq(new importAnswerRequestAppealAction[] { appealActionRedirect });
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.Answer = $"*{appeal.NumberGji} (ГИС ЖКХ {gisNum})* - запрос на перенаправление обращения сформирован";
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос на перенаправление обращения сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message + " " + e.StackTrace);
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
                var response = AppealsServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer += ". Ответ получен";
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
                            req.Answer += ". Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            SaveLog(ref req, ref log);
                        }
                        else
                        {
                            req.Answer += $". Задача на обработку ответа exportAccountData поставлена в очередь с id {taskInfo.TaskId}";
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer += ". Запрос ещё в очереди";
                    log += $"{DateTime.Now} Запрос ещё в очереди\r\n";
                    SaveLog(ref req, ref log);
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer += $". {e.Message}";
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
                            else if (responseItem is CommonResultType)
                            {
                                var gisResult = (CommonResultType)responseItem;
                                AppealCits appCit = AppealCitsDomain.GetAll()
                                        .Where(x => x.GisGkhGuid == gisResult.GUID).FirstOrDefault();
                                if (appCit != null)
                                {
                                    appCit.GisGkhTransportGuid = null;
                                    AppealCitsDomain.Update(appCit);
                                    log += $"{DateTime.Now} Запрос по обращению {appCit.NumberGji} от " +
                                        $"{(appCit.DateFrom.HasValue ? appCit.DateFrom.Value.ToShortDateString() : "-")} успешно выполнен\r\n";
                                }
                                else
                                {
                                    AppealCitsAnswer appCitAnswer = AppealCitsAnswerDomain.GetAll()
                                        .Where(x => x.GisGkhTransportGuid == gisResult.TransportGUID).FirstOrDefault();
                                    if (appCitAnswer != null)
                                    {
                                        appCitAnswer.GisGkhGuid = gisResult.GUID;
                                        appCitAnswer.GisGkhTransportGuid = null;
                                        AppealCitsAnswerDomain.Update(appCitAnswer);
                                        log += $"{DateTime.Now} Запрос по обращению {appCitAnswer.AppealCits.NumberGji} от " +
                                            $"{(appCitAnswer.AppealCits.DateFrom.HasValue ? appCitAnswer.AppealCits.DateFrom.Value.ToShortDateString() : "-")} успешно выполнен\r\n";
                                    }
                                }
                                req.Answer += ". Данные из ГИС ЖКХ обработаны";
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
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
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