using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.HouseManagementAsync;
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

namespace Sobits.GisGkh.DomainService
{
    public class ExportBriefApartmentHouseService : IExportBriefApartmentHouseService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

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

        public ExportBriefApartmentHouseService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string roId)
        {
            try
            {
                var house = RealityObjectDomain.Get(long.Parse(roId));
                string GUID = "";
                if (house.HouseGuid != null && house.HouseGuid != "")
                {
                    GUID = house.HouseGuid;
                }
                else
                {
                    GUID = house.FiasAddress.HouseGuid.ToString();
                }
                //Guid guid = new Guid("4b631569-9bd3-4f9a-b109-24d2c5ca4248");
                Guid guid = new Guid(GUID);
                var request = HouseManagementAsyncService.exportBriefApartmentHouseReq(guid);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
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
                var response = HouseManagementAsyncService.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    //if (req.RespFileInfo != null)
                    //{
                    //    _fileManager.Delete(req.RespFileInfo);
                    //    req.RespFileInfo = null;
                    //}
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    //req.RespFileInfo = SaveFile(SerializeRequest(response), "Responce.dat");
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
                            req.Answer = $"Задача на обработку ответа exportBriefApartmentHouse поставлена в очередь с id {taskInfo.TaskId}";
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
                            else if (responseItem is BriefApartmentHouseType)
                            {

                                var briefApartData = (BriefApartmentHouseType)responseItem;
                                req.Answer = "Ответ пришёл";
                                req.RequestState = GisGkhRequestState.ResponseReceived;
                                //GisGkhRequestsDomain.Update(req);

                            }
                        }
                        req.Answer = "Данные из ГИС ЖКХ обработаны";
                        req.RequestState = GisGkhRequestState.ResponseProcessed;
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

        private void AddPremission(RealityObject House, exportHouseResultTypeApartmentHouseResidentialPremises prem)
        {
            var PremisesRepo = this.Container.ResolveRepository<GisGkhPremises>();
            var oldPremises = PremisesRepo.GetAll().Where(x => x.PremisesGUID == prem.PremisesGUID);
            foreach (var oldPremise in oldPremises)
            {
                PremisesRepo.Delete(oldPremise.Id);
            }
            var premise = new GisGkhPremises();
            
            switch (prem.ItemElementName)
            {
                case ItemChoiceType3.CadastralNumber:
                    premise.CadastralNumber = (string)prem.Item;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Data:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = true;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Registered:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = true;
                    break;
            }
            if (prem.Item1 != null)
            {
                if (prem.Item1 is string)
                {
                    premise.EntranceNum = (string)prem.Item1;
                }
                else
                {
                    premise.EntranceNum = "";
                }
            }
            else
            {
                premise.EntranceNum = null;
            }
            premise.Floor = prem.Floor;
            if (prem.Item2 != null)
            {
                if (prem.Item2 is string)
                {
                    premise.GrossArea = (decimal)prem.Item2;
                }
                else
                {
                    premise.GrossArea = null;
                }
            }
            else
            {
                premise.GrossArea = null;
            }
            premise.IsCommonProperty = null;
            premise.ModificationDate = prem.ModificationDate;
            premise.PremisesGUID = prem.PremisesGUID;
            premise.PremisesNum = prem.PremisesNum;
            premise.PremisesUniqueNumber = prem.PremisesUniqueNumber;
            premise.RealityObject = House;
            premise.RoomType = Bars.Gkh.Enums.RoomType.Living;
            premise.TerminationDate = prem.TerminationDate;
            premise.TotalArea = prem.TotalArea;

            PremisesRepo.Save(premise);
        }

        private void AddPremission(RealityObject House, exportHouseResultTypeApartmentHouseNonResidentialPremises prem)
        {
            var PremisesRepo = this.Container.ResolveRepository<GisGkhPremises>();
            var oldPremises = PremisesRepo.GetAll().Where(x => x.PremisesGUID == prem.PremisesGUID);
            foreach (var oldPremise in oldPremises)
            {
                PremisesRepo.Delete(oldPremise.Id);
            }
            var premise = new GisGkhPremises();

            switch (prem.ItemElementName)
            {
                case ItemChoiceType3.CadastralNumber:
                    premise.CadastralNumber = (string)prem.Item;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Data:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = true;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Registered:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = true;
                    break;
            }
            premise.EntranceNum = null;
            premise.Floor = prem.Floor;
            premise.GrossArea = null;
            premise.IsCommonProperty = prem.IsCommonProperty;
            premise.ModificationDate = prem.ModificationDate;
            premise.PremisesGUID = prem.PremisesGUID;
            premise.PremisesNum = prem.PremisesNum;
            premise.PremisesUniqueNumber = prem.PremisesUniqueNumber;
            premise.RealityObject = House;
            premise.RoomType = Bars.Gkh.Enums.RoomType.NonLiving;
            premise.TerminationDate = prem.TerminationDate;
            premise.TotalArea = prem.TotalArea;

            PremisesRepo.Save(premise);
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
