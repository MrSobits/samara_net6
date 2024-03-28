using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.FileManager;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.NsiServiceAsync;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ExportDataProviderNsiItemsService : IExportDataProviderNsiItemsService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<NsiList> NsiListDomain { get; set; }

        public IDomainService<NsiItem> NsiItemDomain { get; set; }

        public IDomainService<AttachmentField> AttachmentFieldDomain { get; set; }
        public IDomainService<BooleanField> BooleanFieldDomain { get; set; }
        public IDomainService<DateField> DateFieldDomain { get; set; }
        public IDomainService<EnumField> EnumFieldDomain { get; set; }
        public IDomainService<FiasAddressRefField> FiasAddressRefFieldDomain { get; set; }
        public IDomainService<FloatField> FloatFieldDomain { get; set; }
        public IDomainService<IntegerField> IntegerFieldDomain { get; set; }
        public IDomainService<NsiField> NsiFieldDomain { get; set; }
        public IDomainService<NsiRefField> NsiRefFieldDomain { get; set; }
        public IDomainService<OkeiRefField> OkeiRefFieldDomain { get; set; }
        public IDomainService<StringField> StringFieldDomain { get; set; }
        public IDomainService<GisGkhDownloads> GisGkhDownloadsDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

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

        public ExportDataProviderNsiItemsService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager,
            IExportOrgRegistryService exportOrgRegistryService, IFileService fileService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _ExportOrgRegistryService = exportOrgRegistryService;
            _fileService = fileService;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, exportDataProviderNsiItemRequestRegistryNumber registryNumber)
        {
            if (req == null)
            {
                req = new GisGkhRequests();
                req.TypeRequest = GisGkhTypeRequest.exportDataProviderNsiItem;
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
                log += $"{DateTime.Now} Формирование запроса на получение справочника поставщика {registryNumber})\r\n";
                object request = NsiServiceAsync.exportDataProviderNsiItemReq(registryNumber);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                req.Answer = $"*{registryNumber.ToString().Remove(0, 4)}* Сформирован запрос на получение пунктов справочника поставщика";
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
            int first = req.Answer.IndexOf('*');
            int len = req.Answer.IndexOf('*', first + 1) - first;
            string registryNumber = req.Answer.Substring(first, len + 1);

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
                var response = NsiServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer = registryNumber + " Ответ получен";
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
                            req.Answer = registryNumber + " Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            SaveLog(ref req, ref log);
                        }
                        else
                        {
                            req.Answer = registryNumber + $" Задача на обработку ответа exportDataProviderNsiItems поставлена в очередь с id {taskInfo.TaskId}";
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer = registryNumber + " Запрос ещё в очереди";
                    log += $"{DateTime.Now} Запрос ещё в очереди\r\n";
                    SaveLog(ref req, ref log);
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer = $"{registryNumber} {e.Message}";
                GisGkhRequestsDomain.Update(req);
            }
        }

        public void ProcessAnswer(GisGkhRequests req)
        {
            int first = req.Answer.IndexOf('*');
            int len = req.Answer.IndexOf('*', first + 1) - first;
            string registryNumber = req.Answer.Substring(first, len + 1);

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
                        var response = DeserializeData<getStateResult>(Encoding.UTF8.GetString(data));
                        foreach(var item in response.Items)
                        {
                            // если ошибка
                            if (item is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)item;
                                req.Answer = registryNumber + $" {error.ErrorCode}:{error.Description}";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                                if (error.ErrorCode == "INT016043")
                                {
                                    //var nsiList = NsiListDomain.GetAll().Where(x => x.GisGkhCode == registryNumber.Trim('*')).FirstOrDefault();
                                    //if (nsiList != null)
                                    //{
                                    //    var _ExportNsiPagingItemsService = Container.Resolve<IExportNsiPagingItemsService>();
                                    //    _ExportNsiPagingItemsService.SaveRequest(null, nsiList.ListGroup, nsiList.GisGkhCode, 1);
                                    //}
                                }
                            }
                            // если пункт справочника
                            else if (item is NsiItemType)
                            {
                                var nsiItem = (NsiItemType)item;
                                // ищем справочник контрагента, привязанного к оператору реквеста
                                // проверить версионность
                                var nsiList = NsiListDomain.GetAll().Where(x => x.GisGkhCode == nsiItem.NsiItemRegistryNumber && x.Contragent.Id == req.Operator.GisGkhContragent.Id).FirstOrDefault();
                                if (nsiList == null)
                                {
                                    string name = "";
                                    switch (registryNumber.Trim('*'))
                                    {
                                        case "1":
                                            name = "Дополнительные услуги";
                                            break;
                                        case "51":
                                            name = "Коммунальные услуги";
                                            break;
                                        case "59":
                                            name = "Работы и услуги организации";
                                            break;
                                        case "219":
                                            name = "Вид работ капитального ремонта";
                                            break;
                                        case "272":
                                            name = "Система коммунальной инфраструктуры";
                                            break;
                                        case "302":
                                            name = "Основание принятия решения о мерах социальной поддержки гражданина";
                                            break;
                                        case "337":
                                            name = "Коммунальные ресурсы, потребляемые при использовании и содержании общего имущества в многоквартирном доме";
                                            break;

                                    }
                                    nsiList = new NsiList
                                    {
                                        GisGkhCode = registryNumber.Trim('*'),
                                        GisGkhName = name,
                                        RefreshDate = DateTime.Now,
                                        Contragent = req.Operator.GisGkhContragent
                                    };
                                    NsiListDomain.Save(nsiList);
                                }
                                //Гуиды всех пунктов справочника в системе
                                        var allItems = NsiItemDomain.GetAll()
                                            .Where(x => x.NsiList == nsiList)
                                            .Select(x => x.GisGkhGUID).ToList(); // версионность - гуид новый, код старый!!!!
                                // проверяем каждый пришедший пункт на наличние в системе
                                foreach (var el in nsiItem.NsiElement)
                                {
                                    AddItem(el, nsiList, allItems, null, req.Operator.GisGkhContragent);
                                }
                                // дата обновления словаря
                                nsiList.RefreshDate = DateTime.Now;
                                NsiListDomain.Update(nsiList);

                                req.Answer = registryNumber + " Данные из ГИС ЖКХ обработаны";
                                log += $"{DateTime.Now} Пункты справочников добавлены\r\n";
                                req.RequestState = GisGkhRequestState.ResponseProcessed;
                            }
                        }
                        
                        //// иначе список пунктов справочника
                        //else
                        //{
                        //    var elements = (NsiItemType)response.Item;
                        //    // справочник в системе
                        //    var nsiList = NsiListDomain.GetAll().Where(x => x.GisGkhCode == elements.NsiItemRegistryNumber).FirstOrDefault();
                        //    if (nsiList != null)
                        //    {
                        //        // Гуиды всех пунктов справочника в системе
                        //        var allItems = NsiItemDomain.GetAll()
                        //            .Where(x => x.NsiList == nsiList)
                        //            .Select(x => x.GisGkhGUID).ToList();
                        //        // проверяем каждый пришедший пункт на наличние в системе
                        //        foreach (var el in elements.NsiElement)
                        //        {
                        //            AddItem(el, nsiList, allItems);
                        //        }
                        //        // дата обновления словаря
                        //        nsiList.RefreshDate = DateTime.Now;
                        //        NsiListDomain.Update(nsiList);

                        //        req.Answer = registryNumber + " Данные из ГИС ЖКХ обработаны";
                        //        req.RequestState = GisGkhRequestState.ResponseProcessed;
                        //    }
                        //    else
                        //    {
                        //        req.Answer = registryNumber + " В системе отсутствует запрошенный справочник. Сначала получите список справочников из ГИС ЖКХ";
                        //        req.RequestState = GisGkhRequestState.Error;
                        //    }
                        //}
                        //GisGkhRequestsDomain.Update(req);
                    }
                    else
                    {
                        log += $"{DateTime.Now} Не найден файл с ответом из ГИС ЖКХ\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                        throw new Exception("Не найден файл с ответом из ГИС ЖКХ");
                    }
                }
                catch (Exception e)
                {
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
                    log += $"{DateTime.Now} Ошибка: {e.Message}\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
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

        private void AddItem(NsiElementType el, NsiList nsiList, List<string> allItems, NsiItem parent = null, Contragent contragent = null)
        {
            // если нету - добавляем
            NsiItem item = null;
            if (!allItems.Contains(el.GUID))
            {
                item = new NsiItem();
                item.GisGkhGUID = el.GUID;
                item.GisGkhItemCode = el.Code;
                item.NsiList = nsiList;
                item.IsActual = el.IsActual ? YesNo.Yes : YesNo.No;
                item.ParentItem = parent;
                try
                {
                    NsiItemDomain.Save(item);
                }
                catch (Exception eeeeeeee)
                {

                }
            }
            // если итем уже есть, проверяем актуальность
            else
            {
                item = NsiItemDomain.GetAll()
                    .Where(x => x.GisGkhGUID == el.GUID).FirstOrDefault();
                if (item != null)
                {
                    item.IsActual = el.IsActual ? YesNo.Yes : YesNo.No;
                    try
                    {
                        NsiItemDomain.Update(item);

                    }
                    catch (Exception eeeeeeee)
                    {

                    }
                    AttachmentFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            AttachmentFieldDomain.Delete(x.Id);
                        }
                        );
                    BooleanFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            BooleanFieldDomain.Delete(x.Id);
                        }
                        );
                    DateFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            DateFieldDomain.Delete(x.Id);
                        }
                        );
                    EnumFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            EnumFieldDomain.Delete(x.Id);
                        }
                        );
                    FiasAddressRefFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            FiasAddressRefFieldDomain.Delete(x.Id);
                        }
                        );
                    FloatFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            FloatFieldDomain.Delete(x.Id);
                        }
                        );
                    IntegerFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            IntegerFieldDomain.Delete(x.Id);
                        }
                        );
                    NsiFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            NsiFieldDomain.Delete(x.Id);
                        }
                        );
                    NsiRefFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            NsiRefFieldDomain.Delete(x.Id);
                        }
                        );
                    OkeiRefFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item).ForEach(x =>
                        {
                            OkeiRefFieldDomain.Delete(x.Id);
                        }
                        );
                    StringFieldDomain.GetAll()
                        .Where(x => x.NsiItem == item)
                        .ForEach(x =>
                        {
                            StringFieldDomain.Delete(x.Id);
                        }
                        );
                }

            }

            var attachmentFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementAttachmentFieldType)).ToList();
            foreach (var attachmentField in attachmentFields)
            {
                AttachmentField field = new AttachmentField();
                field.NsiItem = item;
                field.Name = ((NsiElementAttachmentFieldType)attachmentField).Name;
                field.Description = ((NsiElementAttachmentFieldType)attachmentField).Document.Description;

                var download = new GisGkhDownloads
                {
                    Guid = field.Guid,
                    EntityT = "AttachmentField",
                    FileField = nameof(field.Attachment),
                    RecordId = field.Id,
                    orgPpaGuid = contragent.GisGkhOrgPPAGUID
                };
                GisGkhDownloadsDomain.Save(download);

                field.Hash = ((NsiElementAttachmentFieldType)attachmentField).Document.AttachmentHASH;
                field.Guid = ((NsiElementAttachmentFieldType)attachmentField).Document.Attachment.AttachmentGUID;
                try
                {
                    AttachmentFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var boolFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementBooleanFieldType)).ToList();
            foreach (var boolField in boolFields)
            {
                BooleanField field = new BooleanField();
                field.NsiItem = item;
                field.Name = ((NsiElementBooleanFieldType)boolField).Name;
                field.Value = ((NsiElementBooleanFieldType)boolField).Value;
                try
                {
                    BooleanFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var dateFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementDateFieldType)).ToList();
            foreach (var dateField in dateFields)
            {
                DateField field = new DateField();
                field.NsiItem = item;
                field.Name = ((NsiElementDateFieldType)dateField).Name;
                field.Value = ((NsiElementDateFieldType)dateField).Value;
                try
                {
                    DateFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var enumFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementEnumFieldType)).ToList();
            foreach (var enumField in enumFields)
            {
                EnumField field = new EnumField();
                field.NsiItem = item;
                field.Name = ((NsiElementEnumFieldType)enumField).Name;
                var positions = ((NsiElementEnumFieldType)enumField).Position
                    .Select(x => new
                    {
                        x.GUID,
                        x.Value,
                    }).ToString();
                field.Position = positions;
                try
                {
                    EnumFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var fiasFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementFiasAddressRefFieldType)).ToList();
            foreach (var fiasField in fiasFields)
            {
                FiasAddressRefField field = new FiasAddressRefField();
                field.NsiItem = item;
                field.Name = ((NsiElementFiasAddressRefFieldType)fiasField).Name;
                field.FiasGUID = ((NsiElementFiasAddressRefFieldType)fiasField).NsiRef.Guid;
                try
                {
                    FiasAddressRefFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var floatFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementFloatFieldType)).ToList();
            foreach (var floatField in floatFields)
            {
                FloatField field = new FloatField();
                field.NsiItem = item;
                field.Name = ((NsiElementFloatFieldType)floatField).Name;
                field.Value = ((NsiElementFloatFieldType)floatField).Value;
                try
                {
                    FloatFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var intFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementIntegerFieldType)).ToList();
            foreach (var intField in intFields)
            {
                IntegerField field = new IntegerField();
                field.NsiItem = item;
                field.Name = ((NsiElementIntegerFieldType)intField).Name;
                field.Value = ((NsiElementIntegerFieldType)intField).Value;
                try
                {
                    IntegerFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var nsiFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementNsiFieldType)).ToList();
            foreach (var nsiField in nsiFields)
            {
                NsiField field = new NsiField();
                field.NsiItem = item;
                field.Name = ((NsiElementNsiFieldType)nsiField).Name;
                field.NsiRegNumber = ((NsiElementNsiFieldType)nsiField).NsiRef?.NsiItemRegistryNumber;
                if (field.NsiRegNumber != null)
                {
                    var nsiRefList = NsiListDomain.GetAll()
                    .Where(x => x.GisGkhCode == ((NsiElementNsiFieldType)nsiField).NsiRef.NsiItemRegistryNumber).FirstOrDefault();
                    field.NsiList = nsiRefList;
                }
                else
                {
                    field.NsiList = null;
                }
                try
                {
                    NsiFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var nsiRefFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementNsiRefFieldType)).ToList();
            foreach (var nsiRefField in nsiRefFields)
            {
                var name = ((NsiElementNsiRefFieldType)nsiRefField).Name;
                // ссылка на справочник
                if (name.Contains("Справочник"))
                {
                    NsiField field = new NsiField();
                    field.NsiItem = item;
                    field.Name = name;
                    field.NsiRegNumber = ((NsiElementNsiRefFieldType)nsiRefField).NsiRef?.NsiItemRegistryNumber;
                    if (field.NsiRegNumber != null)
                    {
                        var nsiRefList = NsiListDomain.GetAll()
                        .Where(x => x.GisGkhCode == ((NsiElementNsiRefFieldType)nsiRefField).NsiRef.NsiItemRegistryNumber).FirstOrDefault();
                        field.NsiList = nsiRefList;
                    }
                    else
                    {
                        field.NsiList = null;
                    }
                    try
                    {
                        NsiFieldDomain.Save(field);
                    }
                    catch (Exception eeeeeee) { }
                }
                // ссылка на пункт справочника
                else
                {
                    NsiRefField field = new NsiRefField();
                    field.NsiItem = item;
                    field.Name = name;
                    field.RefGUID = ((NsiElementNsiRefFieldType)nsiRefField).NsiRef?.Ref?.GUID;
                    if (field.RefGUID != null)
                    {
                        var nsiRefItem = NsiItemDomain.GetAll()
                        .Where(x => x.GisGkhGUID == ((NsiElementNsiRefFieldType)nsiRefField).NsiRef.Ref.GUID).FirstOrDefault();
                        field.NsiRefItem = nsiRefItem;
                    }
                    else
                    {
                        field.NsiRefItem = null;
                    }
                    try
                    {
                        NsiRefFieldDomain.Save(field);
                    }
                    catch (Exception eeeeeee) { }
                }
            }

            var okeiFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementOkeiRefFieldType)).ToList();
            foreach (var okeiField in okeiFields)
            {
                OkeiRefField field = new OkeiRefField();
                field.NsiItem = item;
                field.Name = ((NsiElementOkeiRefFieldType)okeiField).Name;
                field.Code = ((NsiElementOkeiRefFieldType)okeiField).Code;
                try
                {
                    OkeiRefFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }

            var stringFields = el.NsiElementField
                .Where(y => y.GetType() == typeof(NsiElementStringFieldType)).ToList();
            foreach (var stringField in stringFields)
            {
                StringField field = new StringField();
                field.NsiItem = item;
                field.Name = ((NsiElementStringFieldType)stringField).Name;
                field.Value = ((NsiElementStringFieldType)stringField).Value;
                try
                {
                    StringFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }
            }
            
            if (el.ChildElement != null)
            {
                var childItems = el.ChildElement.ToList();
                foreach (var childEl in childItems)
                {
                    AddItem(childEl, nsiList, allItems, item, contragent);
                }
            }
        }

        #endregion

        #region Nested classes
        internal class Identifiers
        {
            internal string SenderIdentifier;
            internal string SenderRole;
            internal string OriginatorID;
        }

        #endregion

    }
}
