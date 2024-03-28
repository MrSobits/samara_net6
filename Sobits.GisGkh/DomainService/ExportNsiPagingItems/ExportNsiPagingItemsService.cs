using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.NsiCommonAsync;
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
    public class ExportNsiPagingItemsService : IExportNsiPagingItemsService
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

        public ExportNsiPagingItemsService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager,
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

        public void SaveRequest(GisGkhRequests req, ListGroup ListGroup, string registryNumber, int page)
        {
            if (req == null)
            {
                req = new GisGkhRequests();
                req.TypeRequest = GisGkhTypeRequest.exportNsiPagingItems;
                req.RequestState = GisGkhRequestState.NotFormed;
                GisGkhRequestsDomain.Save(req);
            }
            try
            {
                object request = NsiServiceCommonAsync.exportNsiPagingItemReq(registryNumber, ListGroup, page);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                req.Answer = $"*{registryNumber}* Сформирован запрос на получение пунктов справочника, страница {page}";
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
                var response = NsiServiceCommonAsync.GetState(req.MessageGUID);
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
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = registryNumber + " Сбой создания задачи обработки ответа";
                            GisGkhRequestsDomain.Update(req);
                            //req.RequestState = GisGkhRequestState.У; ("Сбой создания задачи");
                        }
                        else
                        {
                            req.Answer = registryNumber + $" Задача на обработку ответа exportNsiPagingItems поставлена в очередь с id {taskInfo.TaskId}";
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
                    req.Answer = registryNumber + " Запрос ещё в очереди";
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.Answer = registryNumber + e.Message;
                GisGkhRequestsDomain.Update(req);
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void ProcessAnswer(GisGkhRequests req)
        {
            int first = req.Answer.IndexOf('*');
            int len = req.Answer.IndexOf('*', first + 1) - first;
            string registryNumber = req.Answer.Substring(first, len + 1);

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
                        // если в ответе Item с ошибкой
                        if (response.Item is ErrorMessageType)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            var error = (ErrorMessageType)response.Item;
                            req.Answer = registryNumber + $" {error.ErrorCode}:{error.Description}";
                        }
                        // иначе список пунктов справочника
                        else
                        {
                            var elements = (getStateResultNsiPagingItem)response.Item;
                            // справочник в системе
                            var nsiList = NsiListDomain.GetAll().Where(x => x.GisGkhCode == elements.NsiItemRegistryNumber).FirstOrDefault();
                            if (nsiList != null)
                            {
                                // Гуиды всех пунктов справочника в системе
                                var allItems = NsiItemDomain.GetAll()
                                    .Where(x => x.NsiList == nsiList)
                                    .Select(x => x.GisGkhGUID).ToList();
                                // проверяем каждый пришедший пункт на наличние в системе
                                foreach (var el in elements.NsiElement)
                                {
                                    AddItem(el, nsiList, allItems, null, req.Operator.GisGkhContragent);
                                }
                                // дата обновления словаря
                                nsiList.RefreshDate = DateTime.Now;
                                NsiListDomain.Update(nsiList);

                                if ((int)elements.CurrentPage < (int)elements.TotalPages)
                                {
                                    SaveRequest(null, nsiList.ListGroup, nsiList.GisGkhCode, (int)elements.CurrentPage + 1);
                                }

                                req.Answer = registryNumber + $" Данные из ГИС ЖКХ обработаны. Получена страница {elements.CurrentPage} из {elements.TotalPages} справочника";
                                req.RequestState = GisGkhRequestState.ResponseProcessed;
                            }
                            else
                            {
                                req.Answer = registryNumber + " В системе отсутствует запрошенный справочник. Сначала получите список справочников из ГИС ЖКХ";
                                req.RequestState = GisGkhRequestState.Error;
                            }
                        }
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
                field.Hash = ((NsiElementAttachmentFieldType)attachmentField).Document.AttachmentHASH;
                field.Guid = ((NsiElementAttachmentFieldType)attachmentField).Document.Attachment.AttachmentGUID;
                try
                {
                    AttachmentFieldDomain.Save(field);
                }
                catch (Exception eeeeeee) { }

                var download = new GisGkhDownloads
                {
                    Guid = field.Guid,
                    EntityT = "AttachmentField",
                    FileField = nameof(field.Attachment),
                    RecordId = field.Id,
                    orgPpaGuid = contragent.GisGkhOrgPPAGUID
                };
                GisGkhDownloadsDomain.Save(download);
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
