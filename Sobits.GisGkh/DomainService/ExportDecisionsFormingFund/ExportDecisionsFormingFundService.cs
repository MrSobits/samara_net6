using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.HcsCapitalRepairAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bars.Gkh.Decisions.Nso.Entities;
// TODO: CryptoPro заменить
    //using CryptoPro.Sharpei;
using Bars.Gkh.Decisions.Nso.Entities.Decisions;

namespace Sobits.GisGkh.DomainService
{
    public class ExportDecisionsFormingFundService : IExportDecisionsFormingFundService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }
        //public IDomainService<GisGkhDownloads> GisGkhDownloadsDomain { get; set; }
        public IRepository<BasePersonalAccount> BasePersonalAccountRepo { get; set; }
        public IRepository<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolRepo { get; set; }
        public IRepository<DecisionNotification> DecisionNotificationRepo { get; set; }
        public IRepository<GovDecision> GovDecisionRepo { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }
        public IExportOrgRegistryService _ExportOrgRegistryService;
        private IFileService _fileService;

        #endregion

        #region Constructors

        public ExportDecisionsFormingFundService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager,
            IExportOrgRegistryService ExportOrgRegistryService, IFileService FileService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _ExportOrgRegistryService = ExportOrgRegistryService;
            _fileService = FileService;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, List<long> roIds)
        {
            try
            {
                var houses = RealityObjectDomain.GetAll()
                    .Where(x => roIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.GisGkhGUID,
                        x.HouseGuid,
                        FiasHouseGuid = x.FiasAddress.HouseGuid.ToString()
                    }).ToList();
                List<string> guids = new List<string>();
                foreach (var house in houses)
                {
                    if (!string.IsNullOrEmpty(house.GisGkhGUID))
                    {
                        guids.Add(house.GisGkhGUID);
                    }
                    else if (!string.IsNullOrEmpty(house.HouseGuid))
                    {
                        guids.Add(house.HouseGuid);
                    }
                    else if (!string.IsNullOrEmpty(house.FiasHouseGuid))
                    {
                        guids.Add(house.FiasHouseGuid);
                    }
                }
                if (guids.Count > 0)
                {
                    var request = HcsCapitalRepairAsync.exportDecisionsFormingFundReq(guids);
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                }
                else
                {
                    req.Answer = "В запросе отсутствуют дома с идентификатором ФИАС / ГИС ЖКХ";
                    req.RequestState = GisGkhRequestState.Error;
                }
                GisGkhRequestsDomain.Update(req);
                //return true;
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
                var response = HcsCapitalRepairAsync.GetState(req.MessageGUID, orgPPAGUID);
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
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = "Сбой создания задачи обработки ответа";
                            GisGkhRequestsDomain.Update(req);
                            //req.RequestState = GisGkhRequestState.У; ("Сбой создания задачи");
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа exportDecisionsFormingFund поставлена в очередь с id {taskInfo.TaskId}";
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

        public void ProcessAnswer (GisGkhRequests req)
        {
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
                            }
                            else if (responseItem is exportDecisionType)
                            {
                                var guid = ((exportDecisionType)responseItem).FIASHouseGuid;
                                var house = RealityObjectDomain.GetAll()
                                    .Where(x => x.HouseGuid == guid || x.FiasAddress.HouseGuid.ToString() == guid).FirstOrDefault();
                                // Пришёл протокол решения собственников
                                if (((exportDecisionType)responseItem).Item is DecisionTypeProtocol)
                                {
                                    // Последний активный протокол по дому
                                    var ownerDecision = RealityObjectDecisionProtocolRepo.GetAll()
                                        .Where(x => x.RealityObject == house)
                                        .Where(x => x.State.FinalState)
                                        .OrderByDescending(x => x.ProtocolDate)
                                        .FirstOrDefault();
                                    if (ownerDecision != null)
                                    {
                                        var decNumNum = Array.IndexOf(((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).ItemsElementName, ItemsChoiceType3.Number);
                                        string decNum = (string)((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).Items[decNumNum];
                                        var decDateNum = Array.IndexOf(((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).ItemsElementName, ItemsChoiceType3.Date);
                                        DateTime decDate = (DateTime)((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).Items[decDateNum];
                                        //var decDateStart = ((exportDecisionType)responseItem).DateEffective;
                                        // Если есть протокол на доме, сопадающий по номеру и датам с пришедшим из ГИС
                                        if (ownerDecision.DocumentNum == decNum && ownerDecision.ProtocolDate == decDate) //&& ownerDecision.DateStart == decDateStart)
                                        {
                                            ownerDecision.GisGkhGuid = ((exportDecisionType)responseItem).DecisionGuid;
                                            List<AttachmentType> attachments = new List<AttachmentType>();
                                            var AttachInd = 0;
                                            foreach (var el in ((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).ItemsElementName)
                                            {
                                                if (el == ItemsChoiceType3.Attachment)
                                                {
                                                    attachments.Add((AttachmentType)((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).Items[AttachInd]);
                                                }
                                                AttachInd++;
                                            }
                                            //var decAttachNum = Array.IndexOf(((DecisionTypeProtocol)((exportDecisionType)responseItem).Item).ItemsElementName, ItemsChoiceType3.Attachment);
                                            //if (decAttachNum != -1)
                                            
                                            // Если у нас есть файл протокола
                                            if (ownerDecision.File != null)
                                            {
                                                var gostHash = GetGhostHash(_fileManager.GetFile(ownerDecision.File));
                                                foreach (var decAttach in attachments)
                                                {
                                                    // Хэш совпадает - значит файл тот же
                                                    if (gostHash == decAttach.AttachmentHASH)
                                                    {
                                                        ownerDecision.GisGkhAttachmentGuid = decAttach.Attachment.AttachmentGUID;
                                                    }
                                                    // Хэш не совпадает - файл другой
                                                    else { }
                                                }
                                            }
                                            // Если у нас нет фала протокола 
                                            else
                                            {
                                                // Если файлов несколько, ничего не остаётся, кроме как взять первый попавшийся
                                                var decAttach = attachments.FirstOrDefault();
                                                if (decAttach != null)
                                                {
                                                    ownerDecision.GisGkhAttachmentGuid = decAttach.Attachment.AttachmentGUID;
                                                    // Файлы пока не скачиваем
                                                    //var download = new GisGkhDownloads
                                                    //{
                                                    //    Guid = decAttach.Attachment.AttachmentGUID,
                                                    //    EntityT = ownerDecision.GetType(),
                                                    //    FileField = nameof(ownerDecision.File),
                                                    //    RecordId = ownerDecision.Id
                                                    //};
                                                    //GisGkhDownloadsDomain.Save(download);
                                                    //Operator thisOperator = UserManager.GetActiveOperator();
                                                    //if (thisOperator.GisGkhContragent != null)
                                                    //{
                                                    //    if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID != null) && (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID != ""))
                                                    //    {
                                                    //        var OrgPPAGUID = thisOperator.GisGkhContragent.GisGkhOrgPPAGUID;
                                                    //        var downloadResult = _fileService.DownloadFile(decAttach.Attachment.AttachmentGUID, OrgPPAGUID);

                                                    //        if (downloadResult.Success)
                                                    //        {
                                                    //            ownerDecision.File = downloadResult.File;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            // Не скачалось
                                                    //        }
                                                    //    }
                                                    //    // Нет Гуида контрагента - не судьба скачать файлик
                                                    //    // Но запрос на получение информации организации создадим
                                                    //    else
                                                    //    {
                                                    //        // Как минимум попробуем
                                                    //        try
                                                    //        {
                                                    //            _ExportOrgRegistryService.SaveRequest(null, thisOperator.GisGkhContragent.Id.ToString());
                                                    //        }
                                                    //        catch (Exception e)
                                                    //        {

                                                    //        }
                                                    //    }
                                                    //}
                                                    //// Нет ГИС ЖКХ контрагента у оператора - тем более не судьба
                                                    //else { }
                                                }
                                            }
                                            RealityObjectDecisionProtocolRepo.Update(ownerDecision);
                                        }
                                        var notification = DecisionNotificationRepo.GetAll()
                                            .Where(x => x.Protocol == ownerDecision).FirstOrDefault();
                                        // Если есть уведомление (спецсчёт)
                                        if (notification != null)
                                        {
                                            if (((exportDecisionType)responseItem).Item1 is DecisionTypeFormationFundInSpecialAccount)
                                            {
                                                var gisAcc = (DecisionTypeFormationFundInSpecialAccount)((exportDecisionType)responseItem).Item1;
                                                // Номер счёта совпал - значит оно
                                                if (notification.AccountNum == gisAcc.AccountNumber)
                                                {
                                                    List<AttachmentType> attachments = gisAcc.AccountOpeningDocument.ToList();
                                                    // Если есть файл справки по счёту
                                                    if (notification.BankDoc != null)
                                                    {
                                                        var gostHash1 = GetGhostHash(_fileManager.GetFile(notification.BankDoc));
                                                        foreach (var docAttach in attachments)
                                                        {
                                                            // Хэш совпадает - значит файл тот же
                                                            if (gostHash1 == docAttach.AttachmentHASH)
                                                            {
                                                                notification.GisGkhAttachmentGuid = docAttach.Attachment.AttachmentGUID;
                                                            }
                                                            // Хэш не совпадает - файл другой
                                                            else { }
                                                        }
                                                    }
                                                    // Если нету файла справки по счёту
                                                    else
                                                    {
                                                        // Если файлов несколько, ничего не остаётся, кроме как взять первый попавшийся
                                                        var docAttach = attachments.FirstOrDefault();
                                                        if (docAttach != null)
                                                        {
                                                            notification.GisGkhAttachmentGuid = docAttach.Attachment.AttachmentGUID;
                                                            // Файлы пока не скачиваем
                                                            //var download = new GisGkhDownloads
                                                            //{
                                                            //    Guid = docAttach.Attachment.AttachmentGUID,
                                                            //    EntityT = notification.GetType(),
                                                            //    FileField = nameof(notification.BankDoc),
                                                            //    RecordId = notification.Id
                                                            //};
                                                            //GisGkhDownloadsDomain.Save(download);
                                                            //Operator thisOperator = UserManager.GetActiveOperator();
                                                            //if (thisOperator.GisGkhContragent != null)
                                                            //{
                                                            //    if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID != null) && (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID != ""))
                                                            //    {
                                                            //        var OrgPPAGUID = thisOperator.GisGkhContragent.GisGkhOrgPPAGUID;
                                                            //        var downloadResult = _fileService.DownloadFile(docAttach.Attachment.AttachmentGUID, OrgPPAGUID);

                                                            //        if (downloadResult.Success)
                                                            //        {
                                                            //            notification.BankDoc = downloadResult.File;
                                                            //        }
                                                            //        else
                                                            //        {
                                                            //            // Не скачалось
                                                            //        }
                                                            //    }
                                                            //    // Нет Гуида контрагента - не судьба скачать файлик
                                                            //    // Но запрос на получение информации организации создадим
                                                            //    else
                                                            //    {
                                                            //        // Как минимум попробуем
                                                            //        try
                                                            //        {
                                                            //            _ExportOrgRegistryService.SaveRequest(null, thisOperator.GisGkhContragent.Id.ToString());
                                                            //        }
                                                            //        catch (Exception e)
                                                            //        {

                                                            //        }
                                                            //    }
                                                            //}
                                                            //// Нет ГИС ЖКХ контрагента у оператора - тем более не судьба
                                                            //else { }
                                                        }
                                                    }
                                                }
                                                DecisionNotificationRepo.Update(notification);
                                            }
                                        }
                                    }
                                }
                                // Пришло решение органа власти
                                else if (((exportDecisionType)responseItem).Item is DocumentDecisionType)
                                {
                                    var govDecision = GovDecisionRepo.GetAll()
                                        .Where(x => x.RealityObject == house)
                                        .Where(x => x.State.FinalState && x.FundFormationByRegop)
                                        .OrderByDescending(x => x.ProtocolDate)
                                        .FirstOrDefault();
                                    if (govDecision != null)
                                    {
                                        string decNum = ((DocumentDecisionType)((exportDecisionType)responseItem).Item).Number;
                                        DateTime decDate = ((DocumentDecisionType)((exportDecisionType)responseItem).Item).Date;
                                        if (govDecision.ProtocolNumber == decNum && govDecision.ProtocolDate == decDate) //&& ownerDecision.DateStart == decDateStart)
                                        {
                                            govDecision.GisGkhGuid = ((exportDecisionType)responseItem).DecisionGuid;
                                            List<AttachmentType> attachments = ((DocumentDecisionType)((exportDecisionType)responseItem).Item).Attachment.ToList();
                                            // Если у нас есть файл протокола
                                            if (govDecision.ProtocolFile != null)
                                            {
                                                var gostHash = GetGhostHash(_fileManager.GetFile(govDecision.ProtocolFile));
                                                foreach (var decAttach in attachments)
                                                {
                                                    // Хэш совпадает - значит файл тот же
                                                    if (gostHash == decAttach.AttachmentHASH)
                                                    {
                                                        govDecision.GisGkhAttachmentGuid = decAttach.Attachment.AttachmentGUID;
                                                    }
                                                    // Хэш не совпадает - файл другой
                                                    else { }
                                                }
                                            }
                                            // Если у нас нет фала протокола 
                                            else
                                            {
                                                // Если файлов несколько, ничего не остаётся, кроме как взять первый попавшийся
                                                var decAttach = attachments.FirstOrDefault();
                                                if (decAttach != null)
                                                {
                                                    govDecision.GisGkhAttachmentGuid = decAttach.Attachment.AttachmentGUID;
                                                    // Файлы пока не скачиваем
                                                    //var download = new GisGkhDownloads
                                                    //{
                                                    //    Guid = decAttach.Attachment.AttachmentGUID,
                                                    //    EntityT = govDecision.GetType(),
                                                    //    FileField = nameof(govDecision.ProtocolFile),
                                                    //    RecordId = govDecision.Id
                                                    //};
                                                    //GisGkhDownloadsDomain.Save(download);
                                                    //Operator thisOperator = UserManager.GetActiveOperator();
                                                    //if (thisOperator.GisGkhContragent != null)
                                                    //{
                                                    //    if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID != null) && (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID != ""))
                                                    //    {
                                                    //        var OrgPPAGUID = thisOperator.GisGkhContragent.GisGkhOrgPPAGUID;
                                                    //        var downloadResult = _fileService.DownloadFile(decAttach.Attachment.AttachmentGUID, OrgPPAGUID);

                                                    //        if (downloadResult.Success)
                                                    //        {
                                                    //            govDecision.ProtocolFile = downloadResult.File;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            // Не скачалось
                                                    //        }
                                                    //    }
                                                    //    // Нет Гуида контрагента - не судьба скачать файлик
                                                    //    // Но запрос на получение информации организации создадим
                                                    //    else
                                                    //    {
                                                    //        // Как минимум попробуем
                                                    //        try
                                                    //        {
                                                    //            _ExportOrgRegistryService.SaveRequest(null, thisOperator.GisGkhContragent.Id.ToString());
                                                    //        }
                                                    //        catch (Exception e)
                                                    //        {

                                                    //        }
                                                    //    }
                                                    //}
                                                    //// Нет ГИС ЖКХ контрагента у оператора - тем более не судьба
                                                    //else { }
                                                }
                                            }
                                            GovDecisionRepo.Update(govDecision);
                                        }
                                    }
                                }
                                req.Answer = "Данные из ГИС ЖКХ обработаны";
                                req.RequestState = GisGkhRequestState.ResponseProcessed;
                            }
                            GisGkhRequestsDomain.Update(req);
                        }
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
            // TODO: Найти замену
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
                return hex.ToString();*/
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

        #endregion

    }
}
