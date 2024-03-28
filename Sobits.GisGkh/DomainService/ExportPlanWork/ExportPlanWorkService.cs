using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Bars.GkhCr.Entities;
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

namespace Sobits.GisGkh.DomainService
{
    public class ExportPlanWorkService : IExportPlanWorkService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IRepository<ProgramCr> ProgramCrRepo { get; set; }

        public IRepository<ObjectCr> ObjectCrRepo { get; set; }

        public IRepository<TypeWorkCr> TypeWorkCrRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportPlanWorkService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string[] reqParams)
        {
            try
            {
                var planId = long.Parse(reqParams[0]);
                var program = ProgramCrRepo.Load(planId);
                //var plans = ProgramCrRepo.GetAll()
                //    .Where(x => x.Id == long.Parse(reqParams[0])).ToList();
                //if (!plans.Any())
                //{
                //    throw new Exception("Ошибка: в системе отсутствуют программы капитального ремонта за указанный период");
                //}
                //if (plans.Any(x => x.GisGkhGuid == null || x.GisGkhGuid == ""))
                //{
                //    throw new Exception("Ошибка: не все программы капитального ремонта за указанный период сопоставлены с КПР в ГИС ЖКХ");
                //}
                if (string.IsNullOrEmpty(program.GisGkhGuid))
                {
                    throw new Exception("Данный КПР не сопоставлен с ГИС ЖКХ");
                }
                var request = HcsCapitalRepairAsync.exportPlanWorkReq(program.GisGkhGuid);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                //req.Answer = $"*{registryNumber}* Сформирован запрос на получение пунктов справочника, страница {page}";
                req.RequestState = GisGkhRequestState.Formed;
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
                            req.Answer = $"Задача на обработку ответа exportPlanWork поставлена в очередь с id {taskInfo.TaskId}";
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
                            else if (responseItem is ExportWorkPlanType)
                            {
                                var gisWork = (ExportWorkPlanType)responseItem;
                                //var objectCr = ObjectCrRepo.GetAll()
                                //    .Where(x => x.ProgramCr.GisGkhGuid == gisWork.gui)
                                //    .FirstOrDefault(x => x.RealityObject.HouseGuid == gisWork.FiasHouseGUID ||
                                //    x.RealityObject.FiasAddress.HouseGuid.ToString() == gisWork.FiasHouseGUID);
                                //if (objectCr == null)
                                //{
                                //    // не нашли объект капремонта. Возможно, нет гуида дома
                                //}

                                //if (regProg.Type == PlanPassportTypeType.Plan && (regProg.Status == StatusExtendedType.Published || regProg.Status == StatusExtendedType.PublishedAndProject))
                                //{
                                //    var startDate = DateTime.Parse(regProg.StartMonthYear);
                                //    var endDate = DateTime.Parse(regProg.EndMonthYear);

                                //    ProgramCrRepo.GetAll()
                                //        .Where(x => x.Period.DateStart.Year == startDate.Year
                                //            && x.Period.DateStart.Month == startDate.Month
                                //            && x.Period.DateEnd.Value.Year == endDate.Year
                                //            && x.Period.DateEnd.Value.Month == endDate.Month)
                                //        .ForEach(x =>
                                //        {
                                //            x.GisGkhGuid = regProg.PlanGUID;
                                //            ProgramCrRepo.Update(x);
                                //        });

                                //    //foreach (var program in programs)
                                //    //{
                                //    //    program.GisGkhGuid = ((ExportPlanPassportType)responseItem).PlanGUID;
                                //    //    ProgramCrRepo.Update(program);
                                //    //}
                                //}
                                req.Answer = "Ответ пришёл";
                                req.RequestState = GisGkhRequestState.ResponseReceived;
                                //GisGkhRequestsDomain.Update(req);

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

        #endregion

    }
}
