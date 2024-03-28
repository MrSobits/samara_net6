using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
using Bars.Gkh.Overhaul.Entities;
using Castle.Windsor;
using GisGkhLibrary.RegOrgCommonAsync;
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
    public class ExportOrgRegistryService : IExportOrgRegistryService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IRepository<Contragent> ContragentRepo { get; set; }

        public IRepository<CreditOrg> CreditOrgRepo { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportOrgRegistryService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, CreditOrg creditOrg)
        {
            if (req == null)
            {
                req = new GisGkhRequests();
                req.TypeRequest = GisGkhTypeRequest.exportOrgRegistry;
                //req.ReqDate = DateTime.Now;
                req.RequestState = GisGkhRequestState.NotFormed;
                GisGkhRequestsDomain.Save(req);
            }
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Формирование запроса на получение информации о кредитной организации {creditOrg.Name} из ГИС ЖКХ\r\n";
                var request = RegOrgCommonAsync.exportOrgRegistryReq(new List<Tuple<Dictionary<ItemsChoiceType3, string>, bool?>>{
                    new Tuple<Dictionary<ItemsChoiceType3, string>, bool?>
                        (new Dictionary<ItemsChoiceType3, string>{
                                    { ItemsChoiceType3.OGRN, creditOrg.Ogrn.Trim() },
                                    { ItemsChoiceType3.KPP, creditOrg.Kpp.Trim() }
                            }, null
                        )
                    });
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
                //return true;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void SaveRequest(GisGkhRequests req, List<long> contragentIds)
        {
            if (req == null)
            {
                req = new GisGkhRequests();
                req.TypeRequest = GisGkhTypeRequest.exportOrgRegistry;
                //req.ReqDate = DateTime.Now;
                req.RequestState = GisGkhRequestState.NotFormed;
                GisGkhRequestsDomain.Save(req);
            }
            List<Tuple<Dictionary<ItemsChoiceType3, string>, bool?>> searchList = new List<Tuple<Dictionary<ItemsChoiceType3, string>, bool?>>();
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Формирование запроса на получение информации о {contragentIds.Count} организациях из ГИС ЖКХ\r\n";
                foreach (var contragentId in contragentIds)
                {
                    var contragent = ContragentRepo.Get(contragentId);
                    log += $"{contragent.Name}\r\n";
                    searchList.Add(
                            new Tuple<Dictionary<ItemsChoiceType3, string>, bool?>
                                (new Dictionary<ItemsChoiceType3, string>{
                                        { ItemsChoiceType3.OGRN, contragent.Ogrn.Trim() },
                                        { ItemsChoiceType3.KPP, contragent.Kpp.Trim() }
                                    }, null
                                ));
                }

                var request = RegOrgCommonAsync.exportOrgRegistryReq(searchList);

                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
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
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Запрос ответа из ГИС ЖКХ\r\n";
                var response = RegOrgCommonAsync.GetState(req.MessageGUID);
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
                            req.Answer = $"Задача на обработку ответа exportOrgRegistry поставлена в очередь с id {taskInfo.TaskId}";
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
                throw new Exception("Ошибка: " + e.Message);
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
                        var orgNum = 0;
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)responseItem;
                                req.Answer = $"{error.ErrorCode}:{error.Description}";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                            }
                            else if (responseItem is exportOrgRegistryResultType)
                            {
                                orgNum++;
                                var orgResult = (exportOrgRegistryResultType)responseItem;
                                string ogrn = null;
                                string kpp = null;
                                if (orgResult.OrgVersion.Item is LegalType)
                                {
                                    ogrn = ((LegalType)orgResult.OrgVersion.Item).OGRN;
                                    kpp = ((LegalType)orgResult.OrgVersion.Item).KPP;
                                }
                                else if (orgResult.OrgVersion.Item is exportOrgRegistryResultTypeOrgVersionSubsidiary)
                                {
                                    ogrn = ((exportOrgRegistryResultTypeOrgVersionSubsidiary)orgResult.OrgVersion.Item).OGRN;
                                    kpp = ((exportOrgRegistryResultTypeOrgVersionSubsidiary)orgResult.OrgVersion.Item).KPP;
                                }
                                else if (orgResult.OrgVersion.Item is EntpsType)
                                {
                                    ogrn = ((EntpsType)orgResult.OrgVersion.Item).OGRNIP;
                                }

                                if (ogrn != null)
                                {
                                    Contragent contragent = null;
                                    if (kpp != null)
                                    {
                                        contragent = ContragentRepo.GetAll()
                                        .Where(x => x.Ogrn.Trim() == ogrn && x.Kpp.Trim() == kpp)
                                        .FirstOrDefault();
                                    }
                                    else
                                    {
                                        contragent = ContragentRepo.GetAll()
                                        .Where(x => x.Ogrn.Trim() == ogrn)
                                        .FirstOrDefault();
                                    }
                                    if (contragent != null)
                                    {
                                        contragent.GisGkhGUID = orgResult.orgRootEntityGUID;
                                        contragent.GisGkhVersionGUID = orgResult.OrgVersion.orgVersionGUID;
                                        contragent.GisGkhOrgPPAGUID = orgResult.orgPPAGUID;
                                        ContragentRepo.Update(contragent);
                                        log += $"{DateTime.Now} Контрагент {contragent.Name} сопоставлен\r\n";
                                    }

                                    var creditOrg = CreditOrgRepo.GetAll()
                                        .Where(x => x.Ogrn.Trim() == ogrn && x.Kpp.Trim() == kpp)
                                        .FirstOrDefault();
                                    if (creditOrg != null)
                                    {
                                        creditOrg.GisGkhOrgRootEntityGUID = orgResult.orgRootEntityGUID;
                                        CreditOrgRepo.Update(creditOrg);
                                        log += $"{DateTime.Now} Кредитная организация {creditOrg.Name} сопоставлена\r\n";
                                    }
                                }
                            }
                        }
                        if (req.RequestState != GisGkhRequestState.Error)
                        {
                            req.Answer = "Данные из ГИС ЖКХ обработаны";
                            req.RequestState = GisGkhRequestState.ResponseProcessed;
                        }
                        log += $"{DateTime.Now} Обработка ответа завершена. Количество организаций, по которым пришла информация из ГИС ЖКХ: {orgNum}\r\n";
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
