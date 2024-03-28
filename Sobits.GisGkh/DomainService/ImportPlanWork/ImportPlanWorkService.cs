using Bars.B4;
using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
using Bars.GkhCr.DomainService.GisGkhRegional;
using Bars.GkhCr.Entities;
using Castle.Windsor;
//using CryptoPro.Sharpei;
using GisGkhLibrary.HcsCapitalRepairAsync;
using GisGkhLibrary.Services;
//using GisGkhLibrary.Utils;
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
    public class ImportPlanWorkService : IImportPlanWorkService
    {
        #region Constants


        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }
        public IDomainService<Period> PeriodDomain { get; set; }
        public IRepository<ProgramCr> ProgramCrRepo { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

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

        public ImportPlanWorkService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

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
                var planId = long.Parse(reqParams[0]);
                var program = ProgramCrDomain.Load(planId);

                if (string.IsNullOrEmpty(program.GisGkhGuid))
                {
                    throw new Exception("Данный КПР не выгружен");
                }

                log += $"{DateTime.Now} Формирование запроса на выгрузку работ КПКР \"{program.Name}\" за период \"{program.Period.Name}\" в ГИС ЖКХ\r\n";

                var works = TypeWorkCrDomain.GetAll()
                    .Where(x => x.IsActive)
                    .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "")
                    .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "")
                    .Where(x => x.ObjectCr.ProgramCr == program).ToList(); // сразу берём из базы, т.к. прописываем гранспорт гуид и условие where даст другую выборку

                var reqNum = works.Count() / 1000;
                if (works.Count() % 1000 > 0)
                {
                    reqNum++;
                }
                var gisRegionalService = _container.Resolve<IGisGkhRegionalService>();
                for (int i = 0; i < reqNum; i++)
                {
                    var worksPart = works.Skip(i * 1000).Take(1000);
                    if (i > 0)
                    {
                        var newReq = new GisGkhRequests();
                        newReq.TypeRequest = req.TypeRequest;
                        newReq.Operator = req.Operator;
                        newReq.RequestState = GisGkhRequestState.NotFormed;
                        req = newReq;
                        GisGkhRequestsDomain.Save(req);
                    }
                    List<importPlanWorkRequestImportPlanWork> worksForImport = new List<importPlanWorkRequestImportPlanWork>();
                    string errs = "";
                    worksPart.Where(x => string.IsNullOrEmpty(x.Work.GisGkhGuid) || string.IsNullOrEmpty(x.Work.GisGkhCode)).ForEach(x =>
                    {
                        errs += $"\r\n{x.Work.Name} - вид работы не сопостаавлен с ГИС ЖКХ";
                        log += $"{DateTime.Now} ID работы TypeWorkCr {x.Id}: {x.Work.Name} - вид работы не сопостаавлен с ГИС ЖКХ\r\n";
                    });
                    worksPart.Where(x => string.IsNullOrEmpty(x.ObjectCr.RealityObject.HouseGuid) && (x.ObjectCr.RealityObject.FiasAddress.HouseGuid == null)).ForEach(x =>
                    {
                        errs += $"\r\n{x.ObjectCr.RealityObject.Id}: {x.ObjectCr.RealityObject.Address} - у дома отсутствует идентификатор ФИАС";
                        log += $"{DateTime.Now} ID работы TypeWorkCr {x.Id}: {x.ObjectCr.RealityObject.Address} - у дома отсутствует идентификатор ФИАС\r\n";
                    });
                    worksPart.Where(x => x.YearRepair == null && x.DateEndWork == null).ForEach(x =>
                    {
                        errs += $"\r\n{x.ObjectCr.RealityObject.Id}: {x.ObjectCr.RealityObject.Address} - у работы отсутствует год / дата окончания";
                        log += $"{DateTime.Now} ID работы TypeWorkCr {x.Id} - у работы отсутствует год / дата окончания\r\n";
                    });
                    int workNum = 0;
                    foreach (var work in worksPart.Where(x => !string.IsNullOrEmpty(x.Work.GisGkhCode) && !string.IsNullOrEmpty(x.Work.GisGkhGuid)))
                    {
                        var transportGuid = Guid.NewGuid().ToString();
                        var workItem = new WorkPlanType
                        {
                            OKTMO = new OKTMORefType
                            {
                                code = work.ObjectCr.RealityObject.Municipality.Oktmo
                            },
                            WorkKind = new nsiRef
                            {
                                Code = work.Work.GisGkhCode,
                                GUID = work.Work.GisGkhGuid
                            }
                        };
                        switch (gisRegionalService.GetWorkFinancingType(work))
                        {
                            case Bars.GkhCr.Enums.GisGkhWorkFinancingType.Fund:
                                workItem.Financing = new WorkFinancingType
                                {
                                    Fund = work.Sum.HasValue ? work.Sum.Value.ToMagic(2) : 0
                                };
                                break;
                            case Bars.GkhCr.Enums.GisGkhWorkFinancingType.MunicipalBudget:
                                workItem.Financing = new WorkFinancingType
                                {
                                    MunicipalBudget = work.Sum.HasValue ? work.Sum.Value.ToMagic(2) : 0
                                };
                                break;
                            case Bars.GkhCr.Enums.GisGkhWorkFinancingType.Owners:
                                workItem.Financing = new WorkFinancingType
                                {
                                    Owners = work.Sum.HasValue ? work.Sum.Value.ToMagic(2) : 0
                                };
                                break;
                            case Bars.GkhCr.Enums.GisGkhWorkFinancingType.RegionBudget:
                                workItem.Financing = new WorkFinancingType
                                {
                                    RegionBudget = work.Sum.HasValue ? work.Sum.Value.ToMagic(2) : 0
                                };
                                break;
                        }
                        if (work.DateEndWork.HasValue)
                        {
                            workItem.EndMonthYear = work.DateEndWork.Value.ToString("yyyy-MM");
                        }
                        else if (work.YearRepair.HasValue)
                        {
                            workItem.EndMonthYear = $"{work.YearRepair.Value}-12";
                        }
                        else
                        {
                            // Нет даты
                            continue;
                        }
                        if (!string.IsNullOrEmpty(work.ObjectCr.RealityObject.HouseGuid))
                        {
                            workItem.FiasHouseGUID = work.ObjectCr.RealityObject.HouseGuid;
                        }
                        else if (work.ObjectCr.RealityObject.FiasAddress.HouseGuid != null)
                        {
                            workItem.FiasHouseGUID = work.ObjectCr.RealityObject.FiasAddress.HouseGuid.Value.ToString();
                        }
                        else
                        {
                            // нет гуида на доме
                            continue; ;
                        }
                        importPlanWorkRequestImportPlanWork importWork = new importPlanWorkRequestImportPlanWork
                        {
                            TransportGuid = transportGuid,
                            Item = workItem
                        };

                        worksForImport.Add(importWork);
                        work.GisGkhTransportGuid = transportGuid;
                        TypeWorkCrDomain.Update(work);
                        workNum++;
                    }
                
                    var request = HcsCapitalRepairAsync.importPlanWorkReq(program.GisGkhGuid, worksForImport);
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    req.Answer = "Запрос на выгрузку работ КПР сформирован" + errs;
                    log += $"{DateTime.Now} Запрос сформирован, в запросе {workNum} работ\r\n";
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
                log += $"{DateTime.Now} Запрос ответа из ГИС ЖКХ\r\n";
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
                            req.Answer = $"Задача на обработку ответа importPlanWork поставлена в очередь с id {taskInfo.TaskId}";
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

        public void ProcessAnswer (GisGkhRequests req)
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
                        bool errorFlag = false;
                        string errs = "";
                        int workNum = 0;
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)responseItem;
                                req.Answer = $"{error.ErrorCode}:{error.Description}" + req.Answer + "\r\n";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                                errorFlag = true;
                            }
                            else if (responseItem is CapRemCommonResultType)
                            {
                                var planWorkResponse = (CapRemCommonResultType)responseItem;
                                if (planWorkResponse != null)
                                {
                                    var typeWorkCr = TypeWorkCrDomain.GetAll()
                                        .Where(x => x.GisGkhTransportGuid == planWorkResponse.TransportGUID).FirstOrDefault();
                                    if (typeWorkCr != null)
                                    {
                                        //bool err = false;
                                        foreach (var item in planWorkResponse.Items)
                                        {
                                            if (item is CommonResultTypeError)
                                            {
                                                // ошибка по конкретной работе
                                                //req.RequestState = GisGkhRequestState.Error;
                                                //req.Answer = $"{((CommonResultTypeError)item).ErrorCode}:{((CommonResultTypeError)item).Description}";
                                                //err = true;
                                                errs += $"{((CommonResultTypeError)item).ErrorCode}: {((CommonResultTypeError)item).Description}";
                                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {((CommonResultTypeError)item).ErrorCode}:{((CommonResultTypeError)item).Description}\r\n";
                                            }
                                            else
                                            {
                                                typeWorkCr.GisGkhGuid = planWorkResponse.GUID;
                                                typeWorkCr.GisGkhTransportGuid = null;
                                                TypeWorkCrDomain.Update(typeWorkCr);
                                                workNum++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        log += $"{DateTime.Now} В системе не найдена работа с TransportGUID {planWorkResponse.TransportGUID}\r\n";
                                    }
                                }

                                //req.Answer = "Ответ пришёл";
                                //req.RequestState = GisGkhRequestState.ResponseReceived;
                                //GisGkhRequestsDomain.Update(req);

                            }
                        }
                        if (req.RequestState != GisGkhRequestState.Error)
                        {
                            req.Answer = "Данные из ГИС ЖКХ обработаны" + errs + req.Answer + "\r\n";
                            req.RequestState = GisGkhRequestState.ResponseProcessed;
                        }
                        log += $"{DateTime.Now} Обработка ответа завершена. ГИС ЖКХ подтвердила выгрузку {workNum} работ\r\n";
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
            // TODO: Расскоментировать
          /*  using (var gost = Gost3411.Create())
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
          return null;
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
