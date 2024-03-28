using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.States;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.DomainService.GisGkhRegional;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
using Bars.GkhGji.Entities;
using Castle.Windsor;
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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ExportAppealService : IExportAppealService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        //public IRepository<RealityObject> RealityObjectRepo { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }

        public IDomainService<AppealCitsAttachment> AppealCitsAttachmentDomain { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        public IDomainService<AnswerContentGji> AnswerContentGjiDomain { get; set; }

        public IDomainService<GisGkhDownloads> GisGkhDownloadsDomain { get; set; }

        public IRepository<RevenueSourceGji> RevenueSourceRepo { get; set; }

        public IRepository<RevenueFormGji> RevenueFormRepo { get; set; }

        public IRepository<KindStatementGji> KindStatementRepo { get; set; }

        public IRepository<StringField> StringFieldRepo { get; set; }

        public IRepository<State> StateRepo { get; set; }

        public IDomainService<OrganizationForm> OrganizationFormDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportAppealService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, DateTime? start, DateTime? end)
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
                bool isGji = false;
                IGisGkhRegionalService _gisGkhRegional = Container.Resolve<IGisGkhRegionalService>();
                try
                {
                    isGji = _gisGkhRegional.UserIsGji();
                }
                finally
                {
                    Container.Release(_gisGkhRegional);
                }
                if (!isGji)
                {
                    log += $"{DateTime.Now} Пользователь системы не является сотрудником ГЖИ. Получение обращений ГЖИ доступно только сотрудникам ГЖИ\r\n";
                    req.RequestState = GisGkhRequestState.Error;
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: Пользователь системы не является сотрудником ГЖИ");
                }
                log += $"{DateTime.Now} Формирование запроса на получение обращений из ГИС ЖКХ по датам: с {(start.HasValue ? start.Value.ToShortDateString() : "-")} " +
                    $"по { (end.HasValue ? end.Value.ToShortDateString() : "-")}\r\n";
                var request = AppealsServiceAsync.exportAppealReq(start, end);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
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
                log += $"{DateTime.Now} Запрос ответа из ГИС ЖКХ\r\n";
                var response = AppealsServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
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
                    log += $"{DateTime.Now} Ответ из ГИС ЖКХ получен. Ставим задачу на обработку ответа\r\n";
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
                            req.Answer = $"Задача на обработку ответа exportAccountData поставлена в очередь с id {taskInfo.TaskId}";
                            //GisGkhRequestsDomain.Update(req);
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
                            else if (responseItem is exportAppealResultType)
                            {
                                var gisAppeal = (exportAppealResultType)responseItem;
                                log += $"{DateTime.Now} Обращение из ГИС ЖКХ {gisAppeal.AppealNumber} от {gisAppeal.AppealCreateDate.ToShortDateString()}\r\n";
                                if (gisAppeal.AppealStatus == AppealStatusType.Send)
                                {
                                    log += $"{DateTime.Now} Статус в ГИС ЖКХ - \"Получено\"\r\n";
                                    AppealCits appCit = AppealCitsDomain.GetAll()
                                        .Where(x => x.GisGkhGuid == gisAppeal.AppealGUID).FirstOrDefault();
                                    if (appCit == null)
                                    {
                                        AppealCitsSource appCitSrc = AppealCitsSourceDomain.GetAll()
                                            .Where(x => x.RevenueSourceNumber == gisAppeal.AppealNumber).FirstOrDefault();
                                        if (appCitSrc != null)
                                        {
                                            appCit = appCitSrc.AppealCits;
                                            appCit.GisGkhGuid = gisAppeal.AppealGUID;
                                            appCit.GisGkhParentGuid = gisAppeal.ParentAppealGUID;
                                            AppealCitsDomain.Update(appCit);
                                            log += $"{DateTime.Now} Сопоставлено с обращением {appCit.NumberGji} от " +
                                                $"{(appCit.DateFrom.HasValue ? appCit.DateFrom.Value.ToShortDateString() : "-")} по номеру источника поступления\r\n";
                                        }
                                        else
                                        {
                                            string apptext = string.Empty;
                                            try
                                            {
                                                apptext = Regex.Replace(gisAppeal.AppealText, "<[^>]+>", string.Empty);
                                            }
                                            catch(Exception e)
                                            {
                                                apptext = e.Message;
                                            }
                                            var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                                            .Where(x => x.ProdDate >= gisAppeal.AppealCreateDate && x.ProdDate <= gisAppeal.AppealCreateDate.AddDays(38)).Select(x => x.ProdDate).ToList();

                                            var newControlDate = gisAppeal.AppealCreateDate.AddDays(27);
                                            if (prodCalendarContainer.Contains(newControlDate))
                                            {
                                                for (int i = 0; i <= prodCalendarContainer.Count; i++)
                                                {
                                                    if (prodCalendarContainer.Contains(newControlDate))
                                                    {
                                                        newControlDate = newControlDate.AddDays(-1);
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }

                                            if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                                            {
                                                newControlDate = newControlDate.AddDays(-1);
                                            }
                                            else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                                            {
                                                newControlDate = newControlDate.AddDays(-2);
                                            }
                                            if (prodCalendarContainer.Contains(newControlDate))
                                            {
                                                for (int i = 0; i <= prodCalendarContainer.Count; i++)
                                                {
                                                    if (prodCalendarContainer.Contains(newControlDate))
                                                    {
                                                        newControlDate = newControlDate.AddDays(-1);
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                    
                                            appCit = new AppealCits
                                            {
                                                GisGkhGuid = gisAppeal.AppealGUID,
                                                GisGkhParentGuid = gisAppeal.ParentAppealGUID,
                                                DateFrom = DateTime.Now,
                                                CheckTime = newControlDate,
                                                Description = !string.IsNullOrEmpty(apptext)? apptext: gisAppeal.AppealText,
                                                KindStatement = KindStatementRepo.GetAll().Where(x => x.Name == "ГИС ЖКХ").FirstOrDefault(),
                                                GisWork = true,
                                                Comment = GetTopic(gisAppeal.Item),
                                                SSTUExportState = Bars.GkhGji.Enums.SSTUExportState.NotExported,
                                                QuestionStatus = Bars.GkhGji.Enums.QuestionStatus.InWork,
                                                State = StateRepo.GetAll().Where(x => x.TypeId == "gji_appeal_citizens" && x.Code == "Получено из ГИС ЖКХ").FirstOrDefault()
                                            };
                                            for (int i = 0; i < gisAppeal.ApplicantInfo.Items.Count(); i++)
                                            {
                                                switch (gisAppeal.ApplicantInfo.ItemsElementName[i])
                                                {
                                                    case ItemsChoiceType3.Person:
                                                        appCit.Correspondent = $"{((ApplicantTypePerson)gisAppeal.ApplicantInfo.Items[i]).Surname} {((ApplicantTypePerson)gisAppeal.ApplicantInfo.Items[i]).FirstName} {((ApplicantTypePerson)gisAppeal.ApplicantInfo.Items[i]).Patronymic}".Trim();
                                                        appCit.TypeCorrespondent = Bars.GkhGji.Enums.TypeCorrespondent.CitizenHe;
                                                        break;
                                                    case ItemsChoiceType3.Org:
                                                        var contragent = ContragentDomain.GetAll()
                                                            .Where(x => x.GisGkhGUID == ((RegOrgType)gisAppeal.ApplicantInfo.Items[i]).orgRootEntityGUID).FirstOrDefault();
                                                        if (contragent == null)
                                                        {
                                                            appCit.GisGkhContragentGuid = ((RegOrgType)gisAppeal.ApplicantInfo.Items[i]).orgRootEntityGUID;
                                                            //contragent = new Contragent
                                                            //{
                                                            //    GisGkhGUID = ((RegOrgType)gisAppeal.ApplicantInfo.Items[i]).orgRootEntityGUID,
                                                            //    Name = "Название организации ещё не получено из ГИС ЖКХ",
                                                            //    ContragentState = Bars.Gkh.Enums.ContragentState.Active,
                                                            //    IsSite = false,
                                                            //    IsSpecialAccount = false,
                                                            //    TypeEntrepreneurship = Bars.Gkh.Enums.TypeEntrepreneurship.NotSet,
                                                            //    ActivityGroundsTermination = Bars.Gkh.Enums.GroundsTermination.NotSet,
                                                            //    OrganizationForm = OrganizationFormDomain.GetAll().Where(x => x.Code == "65").FirstOrDefault()
                                                            //};
                                                            //ContragentDomain.Save(contragent);

                                                            // todo создать запрос в ГИС ЖКХ
                                                            //_container.Resolve<IExportOrgRegistryService>().SaveRequest()
                                                        }
                                                        else
                                                        {
                                                            appCit.ContragentCorrespondent = contragent;
                                                        }
                                                        appCit.TypeCorrespondent = Bars.GkhGji.Enums.TypeCorrespondent.LegalEntity;
                                                        break;
                                                    case ItemsChoiceType3.Email:
                                                        appCit.Email = (string)gisAppeal.ApplicantInfo.Items[i];
                                                        break;
                                                    case ItemsChoiceType3.PhoneNumber:
                                                        appCit.Phone = (string)gisAppeal.ApplicantInfo.Items[i];
                                                        break;
                                                    case ItemsChoiceType3.PostAddress:
                                                        appCit.CorrespondentAddress = (string)gisAppeal.ApplicantInfo.Items[i];
                                                        break;
                                                    case ItemsChoiceType3.ApartmentNumber:
                                                        appCit.FlatNum = (string)gisAppeal.ApplicantInfo.Items[i];
                                                        break;
                                                    case ItemsChoiceType3.HCS:
                                                        appCit.Correspondent = "Система ГИС ЖКХ";
                                                        break;
                                                }
                                            }
                                            AppealCitsDomain.Save(appCit);
                                            AppealCitsSource appCitSource = new AppealCitsSource
                                            {
                                                AppealCits = appCit,
                                                RevenueSourceNumber = gisAppeal.AppealNumber,
                                                RevenueSource = RevenueSourceRepo.GetAll().Where(x => x.Name == "ГИС ЖКХ").FirstOrDefault(),
                                              //  RevenueDate = gisAppeal.AppealCreateDate,
                                                RevenueForm = RevenueFormRepo.GetAll().Where(x => x.Name == "ГИС ЖКХ").FirstOrDefault(),
                                                SSTUDate = gisAppeal.AppealCreateDate
                                            };
                                            AppealCitsSourceDomain.Save(appCitSource);
                                            if (gisAppeal.FIASHouseGuid != null)
                                            {
                                                var RealityObject = RealityObjectDomain.GetAll().Where(x => x.HouseGuid == gisAppeal.FIASHouseGuid || (x.FiasAddress.HouseGuid != null && x.FiasAddress.HouseGuid.ToString() == gisAppeal.FIASHouseGuid)).FirstOrDefault();
                                                if (RealityObject != null)
                                                {
                                                    AppealCitsRealityObject appCitRO = new AppealCitsRealityObject
                                                    {
                                                        AppealCits = appCit,
                                                        RealityObject = RealityObject
                                                    };
                                                    AppealCitsRealityObjectDomain.Save(appCitRO);
                                                }
                                            }
                                            string text = "";
                                            if (gisAppeal.Item is nsiRef)
                                            {
                                                string TopicGroup = StringFieldRepo.GetAll()
                                                    .Where(x => x.NsiItem.NsiList.GisGkhCode == "220")
                                                    .Where(x => x.NsiItem.GisGkhItemCode == ((nsiRef)gisAppeal.Item).Code)
                                                    .Where(y => y.Name == "Наименование группы тем")
                                                    .First()?.Value;
                                                string Topic = StringFieldRepo.GetAll()
                                                    .Where(x => x.NsiItem.NsiList.GisGkhCode == "220")
                                                    .Where(x => x.NsiItem.GisGkhItemCode == ((nsiRef)gisAppeal.Item).Code)
                                                    .Where(y => y.Name == "Наименование темы")
                                                    .First()?.Value;
                                                text = $"Тема обращения: {Topic}\r\nГруппа тем: {TopicGroup}\r\n";
                                            }
                                            else if (gisAppeal.Item is string)
                                            {
                                                text = $"Тема обращения: {(string)gisAppeal.Item}\r\n";
                                            }
                                            text += $"Текст обращения:\r\n{gisAppeal.AppealText}";
                                            AppealCitsAttachment appCitTextAttachment = new AppealCitsAttachment
                                            {
                                                AppealCits = appCit,
                                                Name = "Текст обращения",
                                                Description = "Текст обращения, полученного из ГИС ЖКХ, с указанием темы обращения",
                                                FileInfo = _fileManager.SaveFile("Текст обращения.txt", Encoding.UTF8.GetBytes(text))
                                            };
                                            AppealCitsAttachmentDomain.Save(appCitTextAttachment);
                                            if (gisAppeal.Attachment != null)
                                            {
                                                foreach (var attachment in gisAppeal.Attachment)
                                                {
                                                    AppealCitsAttachment appCitAttachment = new AppealCitsAttachment
                                                    {
                                                        AppealCits = appCit,
                                                        Name = attachment.Name ?? "Название отсутствует",
                                                        Description = attachment.Description,
                                                        GisGkhGuid = attachment.Attachment.AttachmentGUID,
                                                        Hash = attachment.AttachmentHASH
                                                    };
                                                    AppealCitsAttachmentDomain.Save(appCitAttachment);
                                                    var download = new GisGkhDownloads
                                                    {
                                                        Guid = appCitAttachment.GisGkhGuid,
                                                        EntityT = "AppealCitsAttachment",
                                                        FileField = nameof(appCitAttachment.FileInfo),
                                                        RecordId = appCitAttachment.Id,
                                                        orgPpaGuid = req.Operator.GisGkhContragent.GisGkhOrgPPAGUID
                                                    };
                                                    GisGkhDownloadsDomain.Save(download);
                                                }
                                            }
                                            log += $"{DateTime.Now} Обращение создано в системе\r\n";
                                        }
                                    }
                                    else
                                    {
                                        log += $"{DateTime.Now} Сопоставленное обращение уже есть в системе: {appCit.NumberGji} от " +
                                            $"{(appCit.DateFrom.HasValue ? appCit.DateFrom.Value.ToShortDateString() : "-")}\r\n";
                                        // если есть обращение в системе и мы продолжаем работать с ним в ГИС
                                        if (appCit.GisWork == true)
                                        {
                                            List<string> answerAttachmentGuids = AppealCitsAnswerDomain.GetAll()
                                                .Where(x => x.AppealCits == appCit).Select(x => x.GisGkhAttachmentGuid).ToList();
                                            if (gisAppeal.AppealHistory != null)
                                            {
                                                foreach (var gisHistory in gisAppeal.AppealHistory)
                                                {
                                                    if (gisHistory.Item is exportAppealResultTypeAppealHistoryAnswer)
                                                    {
                                                        foreach (var attachment in ((exportAppealResultTypeAppealHistoryAnswer)gisHistory.Item).Attachment)
                                                        {
                                                            if (!answerAttachmentGuids.Contains(attachment.Attachment.AttachmentGUID))
                                                            {
                                                                //нашли в ответах из ГИСа файл, не выгруженный из системы - дальше интеграцией по этому обращению не работаем
                                                                appCit.GisWork = false;
                                                                AppealCitsDomain.Update(appCit);
                                                                log += $"{DateTime.Now} В ГИС ЖКХ к ответу приложен файл, отсутствующий в системе - дальше интеграцией по этому обращению не работаем\r\n";
                                                            }
                                                        }
                                                    }
                                                    else if (gisHistory.Item is exportAppealResultTypeAppealHistoryAssesment)
                                                    {

                                                    }
                                                    else if (gisHistory.Item is exportAppealResultTypeAppealHistoryIsNotRequired)
                                                    {
                                                        //если не требует ответа проставлено в ГИС ЖКХ, то у нас не будет ответа с содержанием "Не требует ответа"
                                                        if (!AppealCitsAnswerDomain.GetAll()
                                                            .Where(x => x.AppealCits == appCit)
                                                            .Where(x => x.AnswerContent.Name == "Не требует ответа").Any())
                                                        {
                                                            //дальше интеграцией по этому обращению не работаем
                                                            appCit.GisWork = false;
                                                            AppealCitsDomain.Update(appCit);
                                                            log += $"{DateTime.Now} В ГИС ЖКХ указано, что ответ не требуется, а в системе не указано - дальше интеграцией по этому обращению не работаем\r\n";
                                                        }
                                                    }
                                                    else if (gisHistory.Item is exportAppealResultTypeAppealHistoryRedirected)
                                                    {
                                                        foreach (var attachment in ((exportAppealResultTypeAppealHistoryRedirected)gisHistory.Item).Attachment)
                                                        {
                                                            if (!answerAttachmentGuids.Contains(attachment.Attachment.AttachmentGUID))
                                                            {
                                                                //нашли в ответах из ГИСа файл, не выгруженный из системы - дальше интеграцией по этому обращению не работаем
                                                                appCit.GisWork = false;
                                                                AppealCitsDomain.Update(appCit);
                                                                log += $"{DateTime.Now} В ГИС ЖКХ обращение перенаправлено, а в системе нет ответа с перенаправлением - дальше интеграцией по этому обращению не работаем\r\n";
                                                            }
                                                        }
                                                    }
                                                    else if (gisHistory.Item is exportAppealResultTypeAppealHistoryRolledOver)
                                                    {
                                                        foreach (var attachment in ((exportAppealResultTypeAppealHistoryRolledOver)gisHistory.Item).Attachment)
                                                        {
                                                            if (!answerAttachmentGuids.Contains(attachment.Attachment.AttachmentGUID))
                                                            {
                                                                //нашли в ответах из ГИСа файл, не выгруженный из системы - дальше интеграцией по этому обращению не работаем
                                                                appCit.GisWork = false;
                                                                AppealCitsDomain.Update(appCit);
                                                                log += $"{DateTime.Now} В ГИС ЖКХ обращение продлено, а в системе нет ответа с продлением - дальше интеграцией по этому обращению не работаем\r\n";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            log += $"{DateTime.Now} У обращения не стоит флаг работы с ГИС ЖКХ\r\n";
                                        }
                                    }
                                }
                                else
                                {
                                    log += $"{DateTime.Now} Обращение не в статусе \"Получено\"\r\n";
                                }
                            }
                        }
                        req.Answer = "Данные из ГИС ЖКХ обработаны";
                        req.RequestState = GisGkhRequestState.ResponseProcessed;
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

        private string GetTopic(object obj)
        {            
            string res = string.Empty;
            try
            {
                if (obj != null && obj is nsiRef)
                {
                    var nsi = (nsiRef)obj;
                    res = nsi.Name;
                }
                else if (obj != null)
                {
                    res = obj.ToString();
                }               
            }
            catch 
            {
            }
            return res;

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

        //private Bars.B4.Modules.FileStorage.FileInfo SaveFile(byte[] data, string fileName)
        //{
        //    if (data == null)
        //        return null;

        //    try
        //    {
        //        //сохраняем пакет
        //        return _fileManager.SaveFile(fileName, data);
        //    }
        //    catch (Exception eeeeeeee)
        //    {
        //        return null;
        //    }
        //}

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