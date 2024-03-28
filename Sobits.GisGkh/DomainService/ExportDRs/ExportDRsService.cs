using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.States;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Suggestion;
using Bars.Gkh.FileManager;
using Bars.Gkh.DomainService.GisGkhRegional;
using Bars.GkhGji.Entities;
using Castle.Windsor;
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
using GisGkhLibrary.DebtRequestAsync;
using Bars.B4.Application;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.Modules.ClaimWork.Entities;
using Bars.Gkh.RegOperator.Entities.Owner;
using GisGkhLibrary.Crypto;
using Period = GisGkhLibrary.DebtRequestAsync.Period;
    //using CryptoPro.Sharpei;

namespace Sobits.GisGkh.DomainService
{
    public class ExportDRsService : IExportDRsService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<ClaimWorkAccountDetail> ClaimWorkAccountDetailDomain { get; set; }
        public IDomainService<Lawsuit> LawsuitDomain { get; set; }//
        public IDomainService<LawsuitClwDocument> LawsuitClwDocumentDomain { get; set; }
        public IDomainService<LawsuitIndividualOwnerInfo> LawsuitOwnerInfoDomain { get; set; }
        public IDomainService<FlattenedClaimWork> FlattenedClaimWorkDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }      
        public IDomainService<GisGkhDownloads> GisGkhDownloadsDomain { get; set; }
        public IRepository<StringField> StringFieldRepo { get; set; }
        public IRepository<State> StateRepo { get; set; }
        public IDomainService<OrganizationForm> OrganizationFormDomain { get; set; }
        public IWindsorContainer Container { get; set; }

        private IFileService _fileService;

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportDRsService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService fileService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _fileService = fileService;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, DateTime? start, DateTime? end)
        {
            try
            {
                if (!start.HasValue || !end.HasValue)
                {
                    throw new Exception("Ошибка: Обязательные поля запроса (Дата с и/или дата по) не заполнены");
                }
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                bool isCr = false;
                IGisGkhRegionalService _gisGkhRegional = Container.Resolve<IGisGkhRegionalService>();
                try
                {
                    isCr = true;
                }
                finally
                {
                    Container.Release(_gisGkhRegional);
                }
                if (!isCr)
                {
                    log += $"{DateTime.Now} Потзователь системы не является сотрудником ФКР. Получение запросов задолженности ФКР доступно только сотрудникам ФКР\r\n";
                    req.RequestState = GisGkhRequestState.Error;
                    SaveLog(req, log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: Пользователь системы не является сотрудником ФКР");
                }
                log += $"{DateTime.Now} Формирование запроса на получение запросов задолженности из ГИС ЖКХ по датам: с {(start.HasValue ? start.Value.ToShortDateString() : "-")} " +
                    $"по { (end.HasValue ? end.Value.ToShortDateString() : "-")}\r\n";
                var request = DRsServiceAsync.exportDRsReq(start.Value, end.Value);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(req, log);
            //    GisGkhRequestsDomain.Update(req);
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
                var response = DRsServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
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
                    SaveLog(req, log);
                   // GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = "Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            SaveLog(req, log);
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа поставлена в очередь с id {taskInfo.TaskId}";
                            GisGkhRequestsDomain.Update(req);
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
                    SaveLog(req, log);
                }
                //GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer = e.Message;
                GisGkhRequestsDomain.Update(req);
            }
        }

        public void ProcessAnswer(GisGkhRequests req, bool isDebtRequest)
        {
            List<importDSRResponsesRequestAction> actionList = new List<importDSRResponsesRequestAction>();
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
                    var requestFileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.request)
                        .FirstOrDefault();
                    exportDSRsRequest requestSended = new exportDSRsRequest();
                    if (requestFileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(requestFileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        requestSended = DeserializeData<exportDSRsRequest>(Encoding.UTF8.GetString(data));
                    }
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
                            //exportDSRsResult
                            else if (isDebtRequest && responseItem is exportDSRsResultType)
                            {
                                var gisAppeal = (exportDSRsResultType)responseItem;
                                string nextSRGuid = string.Empty;
                                if (gisAppeal.pagedOutput != null)
                                {
                                    if (gisAppeal.pagedOutput.Item != null)
                                    {
                                        if (gisAppeal.pagedOutput.Item is bool)
                                        {

                                        }
                                        else
                                        {
                                            nextSRGuid = (string)gisAppeal.pagedOutput.Item;
                                            if (!string.IsNullOrEmpty(nextSRGuid))
                                            {
                                                GisGkhLibrary.DebtRequestAsync.Period period = new GisGkhLibrary.DebtRequestAsync.Period();
                                                try
                                                {
                                                    foreach (var reqItem in requestSended.Items.ToList())
                                                    {
                                                        if (reqItem is GisGkhLibrary.DebtRequestAsync.Period)
                                                        {
                                                            period = (GisGkhLibrary.DebtRequestAsync.Period)reqItem;
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                    
                                                }
                                                CreateNextPageRequest(nextSRGuid, period);
                                            }
                                        }
                                    }
                                }
                                if (gisAppeal.subrequestData != null && gisAppeal.subrequestData.Length > 0)
                                {
                                    int i = 0;
                                    foreach (var sData in gisAppeal.subrequestData.ToList())
                                    {
                                        //i++; //для теста
                                        //if (i > 6)
                                        //{
                                        //    break;
                                        //}
                                        log += $"{DateTime.Now} Запрос задолженности из ГИС ЖКХ {sData.requestInfo.requestNumber} от {sData.requestInfo.sentDate.ToShortDateString()}\r\n";
                                        string fiasHouse = sData.requestInfo.housingFundObject.fiasHouseGUID;
                                        var items = sData.requestInfo.housingFundObject.Items;
                                        var choises = sData.requestInfo.housingFundObject.ItemsElementName;
                                        string roomNumber = "";
                                        for (int k = 0;k< choises.Count();k++)
                                        {
                                            if (choises[k] == ItemsChoiceType6.addressDetails)
                                            {
                                                roomNumber = items[k];
                                            }
                                        }
                                        log += $"fiasHouse {fiasHouse} roomNumber {roomNumber}\r\n";
                                        //для теста
                                        //fiasHouse = "6a6314d2-cd09-4fbe-a8bc-cd0e908c9e72";
                                        //roomNumber = "26";
                                        if (roomNumber.Contains("кв."))
                                        {
                                            roomNumber = roomNumber.Replace("кв.", "");
                                            roomNumber = roomNumber.Trim();
                                        }    
                                        string srGuid = sData.subrequestGUID;
                                       
                                        var appSettings = ApplicationContext.Current.Configuration.AppSettings;
                                        var gispersonGuid = appSettings.GetAs<string>("DRsRequestPersonGuid");
                                        try
                                        {
                                            var room = RoomDomain.GetAll()
                                            .Where(x => x.RealityObject.FiasAddress.HouseGuid.ToString() == fiasHouse)
                                            .Where(x => x.RoomNum == roomNumber).FirstOrDefault();
                                            if (room != null)
                                            {
                                                var persAccs = BasePersonalAccountDomain.GetAll()
                                                    .Where(x => x.Room == room && x.State.StartState)
                                                    .ToList();
                                                bool isdebtor = false;
                                                //тестируем
                                                //DebtInfoTypePerson testperson = new DebtInfoTypePerson
                                                //{
                                                //    firstName = "Тест",
                                                //    lastName = "Тестов",
                                                //    middleName = "Тестович",
                                                //};
                                                //List<DebtInfoTypeDocument> testdocs = new List<DebtInfoTypeDocument>();
                                                //testdocs.Add(new DebtInfoTypeDocument
                                                //{
                                                //    type = new nsiRef
                                                //    {
                                                //        Code = "1",
                                                //        GUID = "237b587e-0b10-4702-b78c-8d641c3d4d45"
                                                //    }
                                                //});
                                                //string testcourtOrder = "Судебный приказ №12345 от 01.01.0000";
                                                //Bars.B4.Modules.FileStorage.FileInfo testfakeFile = _fileManager.SaveFile("CourtDecisionInfo.txt", Encoding.UTF8.GetBytes(testcourtOrder));
                                                //actionList.Add(CreateDebtAnswer(srGuid, gispersonGuid, testperson, testdocs, testfakeFile));

                                                //
                                                foreach (var acc in persAccs)
                                                {

                                                    var psumm = PersonalAccountPeriodSummaryDomain.GetAll()
                                                         .Where(x => !x.Period.IsClosed && x.PersonalAccount == acc).FirstOrDefault();
                                                    if (psumm != null)
                                                    {
                                                        var debt = psumm.BaseTariffDebt - psumm.TariffPayment;
                                                        if (debt > 1000)
                                                        {
                                                            var clwork = ClaimWorkAccountDetailDomain.GetAll()
                                                                .Where(x => x.PersonalAccount == acc).OrderByDescending(x => x.Id).FirstOrDefault()?.ClaimWork;
                                                            if (clwork != null)
                                                            {
                                                                var lawsuit = LawsuitDomain.GetAll()
                                                                    .Where(x => x.ClaimWork == clwork && x.DocumentType == Bars.Gkh.Modules.ClaimWork.Enums.ClaimWorkDocumentType.CourtOrderClaim).FirstOrDefault();

                                                                if (lawsuit != null)
                                                                {
                                                                    var lawsuitDoc = LawsuitClwDocumentDomain.GetAll()
                                                                        .FirstOrDefault(x => x.DocumentClw.Id == lawsuit.Id);
                                                                    var isk = LawsuitDomain.GetAll()
                                                                    .Where(x => x.ClaimWork == clwork && x.DocumentType == Bars.Gkh.Modules.ClaimWork.Enums.ClaimWorkDocumentType.Lawsuit).FirstOrDefault();
                                                                    if (isk == null && lawsuit.DocumentDate.HasValue && lawsuit.DocumentDate.Value > DateTime.Now.AddYears(-3))
                                                                    {
                                                                        if (lawsuit.ResultConsideration == Bars.Gkh.Modules.ClaimWork.Enums.LawsuitResultConsideration.CourtOrderIssued)
                                                                        {
                                                                            if (lawsuit.ConsiderationNumber != "" && lawsuit.ConsiderationDate.HasValue)
                                                                            {
                                                                                if (!lawsuit.IsDeterminationCancel)
                                                                                {
                                                                                    //ну чел точно должник
                                                                                    isdebtor = true;
                                                                                    var owner = LawsuitOwnerInfoDomain.GetAll()
                                                                               .Where(x => x.Lawsuit.Id == lawsuit.Id && !x.Underage).FirstOrDefault();
                                                                                    DebtInfoTypePerson person = new DebtInfoTypePerson
                                                                                    {
                                                                                        firstName = owner.FirstName,
                                                                                        lastName = owner.Surname,
                                                                                        middleName = owner.SecondName
                                                                                    };
                                                                                    List<DebtInfoTypeDocument> docs = new List<DebtInfoTypeDocument>();
                                                                                    docs.Add(new DebtInfoTypeDocument
                                                                                    {
                                                                                        type = new nsiRef
                                                                                        {
                                                                                            Code = "1",
                                                                                            GUID = "237b587e-0b10-4702-b78c-8d641c3d4d45"
                                                                                        }
                                                                                    });
                                                                                    string courtOrder = $"Судебный приказ №{lawsuit.ConsiderationNumber} от {lawsuit.ConsiderationDate.Value.ToString("dd.MM.yyyy")}";
                                                                                    Bars.B4.Modules.FileStorage.FileInfo fakeFile = _fileManager.SaveFile("CourtDecisionInfo.txt", Encoding.UTF8.GetBytes(courtOrder));
                                                                                    var existsAnswer = actionList.Where(x => x.subrequestGUID == srGuid).FirstOrDefault();
                                                                                    if (existsAnswer == null)
                                                                                    {
                                                                                        if (lawsuitDoc != null && lawsuitDoc.File != null)
                                                                                        {
                                                                                            actionList.Add(CreateDebtAnswer(srGuid, gispersonGuid, person, docs, lawsuitDoc.File, true));
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            actionList.Add(CreateDebtAnswer(srGuid, gispersonGuid, person, docs, fakeFile, true));
                                                                                        }
                                                                                        
                                                                                    }
                                                                                }
                                                                            }
                                                                            else if (lawsuitDoc != null)
                                                                            {
                                                                                isdebtor = true;
                                                                                try
                                                                                {
                                                                                    var owner = LawsuitOwnerInfoDomain.GetAll()
                                                                               .Where(x => x.Lawsuit.Id == lawsuit.Id && !x.Underage).FirstOrDefault();
                                                                                    DebtInfoTypePerson person = new DebtInfoTypePerson
                                                                                    {
                                                                                        firstName = owner.FirstName,
                                                                                        lastName = owner.Surname,
                                                                                        middleName = owner.SecondName
                                                                                    };
                                                                                    List<DebtInfoTypeDocument> docs = new List<DebtInfoTypeDocument>();
                                                                                    docs.Add(new DebtInfoTypeDocument
                                                                                    {
                                                                                        type = new nsiRef
                                                                                        {
                                                                                            Code = "1",
                                                                                            GUID = "237b587e-0b10-4702-b78c-8d641c3d4d45"
                                                                                        }
                                                                                    });
                                                                                    string courtOrder = $"Судебный приказ №{lawsuitDoc.DocNumber} от {lawsuitDoc.DocDate.Value.ToString("dd.MM.yyyy")}";
                                                                                    var existsAnswer = actionList.Where(x => x.subrequestGUID == srGuid).FirstOrDefault();
                                                                                    if (existsAnswer == null)
                                                                                    {
                                                                                        actionList.Add(CreateDebtAnswer(srGuid, gispersonGuid, person, docs, lawsuitDoc.File, false));
                                                                                    }
                                                                                }
                                                                                catch
                                                                                { }
                                                                            }

                                                                        }
                                                                        else if (lawsuitDoc != null)
                                                                        {
                                                                            isdebtor = true;
                                                                            try
                                                                            {
                                                                                var owner = LawsuitOwnerInfoDomain.GetAll()
                                                                           .Where(x => x.Lawsuit.Id == lawsuit.Id && !x.Underage).FirstOrDefault();
                                                                                DebtInfoTypePerson person = new DebtInfoTypePerson
                                                                                {
                                                                                    firstName = owner.FirstName,
                                                                                    lastName = owner.Surname,
                                                                                    middleName = owner.SecondName
                                                                                };
                                                                                List<DebtInfoTypeDocument> docs = new List<DebtInfoTypeDocument>();
                                                                                docs.Add(new DebtInfoTypeDocument
                                                                                {
                                                                                    type = new nsiRef
                                                                                    {
                                                                                        Code = "1",
                                                                                        GUID = "237b587e-0b10-4702-b78c-8d641c3d4d45"
                                                                                    }
                                                                                });
                                                                                string courtOrder = $"Судебный приказ №{lawsuitDoc.DocNumber} от {lawsuitDoc.DocDate.Value.ToString("dd.MM.yyyy")}";
                                                                                var existsAnswer = actionList.Where(x => x.subrequestGUID == srGuid).FirstOrDefault();
                                                                                if (existsAnswer == null)
                                                                                {
                                                                                    actionList.Add(CreateDebtAnswer(srGuid, gispersonGuid, person, docs, lawsuitDoc.File, false));
                                                                                }
                                                                            }
                                                                            catch
                                                                            { }
                                                                        }
                                                                        else
                                                                        {
                                                                            //проверяем дольщиков
                                                                            var owner = LawsuitOwnerInfoDomain.GetAll()
                                                                                .Where(x => x.Lawsuit.Id == lawsuit.Id && !x.Underage).FirstOrDefault();
                                                                            if (owner != null)
                                                                            {
                                                                                var flClw = FlattenedClaimWorkDomain.GetAll()
                                                                                    .Where(x => x.RloiId.HasValue && x.RloiId.Value == owner.Id).FirstOrDefault();
                                                                                if (flClw != null && flClw.ZVSPCourtDecision == Bars.Gkh.Modules.ClaimWork.Enums.ZVSPCourtDecision.CourtOrder)
                                                                                {
                                                                                    if (!flClw.CourtClaimCancellationDate.HasValue)
                                                                                    {
                                                                                        //этот должник дольщик
                                                                                        DebtInfoTypePerson person = new DebtInfoTypePerson
                                                                                        {
                                                                                            firstName = owner.FirstName,
                                                                                            lastName = owner.Surname,
                                                                                            middleName = owner.SecondName
                                                                                        };
                                                                                        List<DebtInfoTypeDocument> docs = new List<DebtInfoTypeDocument>();
                                                                                        docs.Add(new DebtInfoTypeDocument
                                                                                        {
                                                                                            type = new nsiRef
                                                                                            {
                                                                                                Code = "1",
                                                                                                GUID = "237b587e-0b10-4702-b78c-8d641c3d4d45"
                                                                                            }
                                                                                        });
                                                                                        string courtOrder = $"Судебный приказ №{flClw.CourtClaimNum} от {flClw.CourtClaimDate.Value.ToString("dd.MM.yyyy")}";
                                                                                        Bars.B4.Modules.FileStorage.FileInfo fakeFile = _fileManager.SaveFile("log.txt", Encoding.UTF8.GetBytes(courtOrder));
                                                                                        var existsAnswer = actionList.Where(x => x.subrequestGUID == srGuid).FirstOrDefault();
                                                                                        if (existsAnswer == null)
                                                                                        {
                                                                                            actionList.Add(CreateDebtAnswer(srGuid, gispersonGuid, person, docs, fakeFile, true));
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (!isdebtor)
                                                {
                                                    var existsAnswer = actionList.Where(x => x.subrequestGUID == srGuid).FirstOrDefault();
                                                    if (existsAnswer == null)
                                                    {
                                                        actionList.Add(CreateNoDebtAnswer(srGuid, gispersonGuid));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                actionList.Add(CreateNoDebtAnswer(srGuid, gispersonGuid));
                                            }
                                        }
                                        catch(Exception e)
                                        {
                                            log += $"{DateTime.Now} Запрос задолженности из ГИС ЖКХ {sData.requestInfo.requestNumber} от {sData.requestInfo.sentDate.ToShortDateString()}\r\n" + e.Message;
                                        }
                                        



                                    }

                                }
                               
                            }
                        }
                        try
                        {
                            CreateAnswerRequest(actionList);
                            req.Answer = "Данные из ГИС ЖКХ обработаны";
                        }
                        catch (Exception e)
                        {
                            log += $"{DateTime.Now} Ошибка сохранения запроса {e.Message} {e.StackTrace}\r\n";
                            req.Answer = "Данные из ГИС ЖКХ обработаны c ошибкой";
                        }
                        
                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                        log += $"{DateTime.Now} Обработка ответа завершена\r\n";
                        SaveLog(req, log);
                       
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
                    throw new Exception("Ошибка: " + e.Message + e.StackTrace);
                }
            }
        }

        #endregion

        #region Private methods

        private importDSRResponsesRequestAction CreateNoDebtAnswer(string subGuid, string personGuid)
        {
            var request = DRsServiceAsync.exportDRsReqNoDebtAnswer(subGuid, personGuid);
            //var prefixer = new XmlNsPrefixer();
            //XmlDocument document = SerializeRequest(request);
            //prefixer.Process(document);
            //GisGkhRequests req = new GisGkhRequests
            //{
            //    TypeRequest = GisGkhTypeRequest.importDebtRequests
            //};
            //GisGkhRequestsDomain.Save(req);
            //SaveFile(req, GisGkhFileType.request, document, "request.xml");
            //req.RequestState = GisGkhRequestState.Formed;
            //GisGkhRequestsDomain.Update(req);
            return request;
        }

        private void CreateAnswerRequest(List<importDSRResponsesRequestAction> actionList)
        {
            var request = DRsServiceAsync.exportDRsReqGetAction(actionList);
            var prefixer = new XmlNsPrefixer();
            XmlDocument document = SerializeRequest(request);
            prefixer.Process(document);
            GisGkhRequests req = new GisGkhRequests
            {
                TypeRequest = GisGkhTypeRequest.importDebtRequests,
                ObjectCreateDate = DateTime.Now,
                ObjectVersion = 0,
                ObjectEditDate = DateTime.Now
            };
            this.GisGkhRequestsDomain.Save(req);
            SaveFile(req, GisGkhFileType.request, document, "request.xml");
            req.RequestState = GisGkhRequestState.Formed;
            GisGkhRequestsDomain.Update(req);
        }
        private importDSRResponsesRequestAction CreateDebtAnswer(string subGuid, string personGuid, DebtInfoTypePerson personDebtor, List<DebtInfoTypeDocument> docs, Bars.B4.Modules.FileStorage.FileInfo fakeFile, bool reallyFake)
        {
            var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.debtreq, fakeFile, GetOrgPpaGuid());
            List<DebtInfoTypeDocument> docsAttmt = new List<DebtInfoTypeDocument>();
            if (uploadResult.Success)
            {
                foreach (var doc in docs)
                {
                    docsAttmt.Add(new DebtInfoTypeDocument
                    {
                        type = doc.type,
                        attachment = new AttachmentType
                        {
                            Attachment = new Attachment
                            {
                                AttachmentGUID = uploadResult.FileGuid
                            },
                            Name = fakeFile.FullName,
                            Description = "Информация по судебным актам",
                            AttachmentHASH = GetGhostHash(_fileManager.GetFile(fakeFile))
                        }
                    });
                }
            }
            try
            {
                //if(reallyFake)
                //_fileManager.Delete(fakeFile);
            }
            catch
            {
                
            }
            var request = DRsServiceAsync.exportDRsReqDebtAnswer(subGuid, personGuid, personDebtor, docsAttmt);
            //var prefixer = new XmlNsPrefixer();
            //XmlDocument document = SerializeRequest(request);
            //prefixer.Process(document);
            //GisGkhRequests req = new GisGkhRequests
            //{
            //    TypeRequest = GisGkhTypeRequest.importDebtRequests
            //};
            //GisGkhRequestsDomain.Save(req);
            //SaveFile(req, GisGkhFileType.request, document, "request.xml");
            //req.RequestState = GisGkhRequestState.Formed;
            //GisGkhRequestsDomain.Update(req);
            return request;
        }

        /// <summary>
        /// Получить хэш файла по алгоритму ГОСТ Р 34.11-94
        /// </summary>
        public static string GetGhostHash(Stream content)
        {
            return "";
            // TODO: 
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

        private string GetOrgPpaGuid()
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            return appSettings.GetAs<string>("FKROPGPPAGuid");
        }

        private void CreateNextPageRequest(string subGuid, Period period)
        {
            var request = DRsServiceAsync.exportDRsNextPageReq(subGuid, period);
            var prefixer = new XmlNsPrefixer();
            XmlDocument document = SerializeRequest(request);
            prefixer.Process(document);
            GisGkhRequests req = new GisGkhRequests
            {
                TypeRequest = GisGkhTypeRequest.exportDebtRequests
            };
            GisGkhRequestsDomain.Save(req);
            SaveFile(req, GisGkhFileType.request, document, "request.xml");
            req.RequestState = GisGkhRequestState.Formed;
            GisGkhRequestsDomain.Update(req);
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

        private void SaveLog(GisGkhRequests req, string log)
        {
            if (req == null || _fileManager == null || GisGkhRequestsDomain == null)
            {
                throw new Exception("req или _fileManager нулл ");
            }
            try
            {
                req.LogFile = _fileManager.SaveFile("log.txt", Encoding.UTF8.GetBytes(log));
              
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка сохранения файла" + log);
            }
            try
            {
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка сохранения ентити" + log);
            }
        }

        #endregion

    }
}