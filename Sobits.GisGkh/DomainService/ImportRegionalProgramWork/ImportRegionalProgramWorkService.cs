using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.FileManager;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Enum;
using Bars.GkhCr.Entities;
using Castle.Windsor;
//using CryptoPro.Sharpei;
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
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ImportRegionalProgramWorkService : IImportRegionalProgramWorkService
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
        public IDomainService<GisGkhRegionalProgramCR> GisGkhRegionalProgramCRDomain { get; set; }
        public IDomainService<StructuralElementWork> StructuralElementWorkDomain { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }
        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }
        public IDomainService<GisGkhVersionRecord> GisGkhVersionRecordDomain { get; set; }
        public IDomainService<CrPeriod> CrPeriodDomain { get; set; }

        ///// <summary>
        ///// Сервис ДПКР
        ///// </summary>
        //public ILongProgramService LongProgramService { get; set; }

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

        public ImportRegionalProgramWorkService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string OrgPPAGUID)
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
                var programs = GisGkhRegionalProgramCRDomain.GetAll().Where(x => x.WorkWith).ToList();
                if (programs.Count == 0)
                {
                    throw new Exception("Отсутствует сопоставленная региональная программа");
                }
                else if (programs.Count > 1)
                {
                    throw new Exception("Признак работы с региональной программой должен быть проставлен только для одной программы");
                }
                log += $"{DateTime.Now} Формирование запроса на выгрузку работ ДПКР в ГИС ЖКХ\r\n";
                var program = programs[0];

                // Периоды
                var periodsCr = CrPeriodDomain.GetAll().ToList();

                // словарь работ для кэ
                // если несколько видов работ по одному кэ, то берём первый
                Dictionary<long, Work> strElGisCodes = StructuralElementWorkDomain.GetAll()
                    .Select(x => new
                    {
                        x.StructuralElement.Id,
                        x.Job.Work
                    }).GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.Select(y => y.Work).FirstOrDefault());

                var allStages3 = VersionRecordStage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain) // все stage1 по всем актуальным версиям
                    .Where(x => x.VersionRecordState != VersionRecordState.NonActual) // только актуальные
                    .GroupBy(x => x.Stage2Version.Stage3Version)
                    .ToDictionary(x => x.Key, y => y.ToList()); // ключ - работа, значение - список stage1

                List<GisGkhVersionRecord> gisWorkList = new List<GisGkhVersionRecord>();

                // список работ в ГИС ЖКХ по актуальным долгосрочкам (в системе и для раоты с ГИС), с гуидами и без
                List<GisGkhVersionRecord> gisVersionRec = GisGkhVersionRecordDomain.GetAll()
                    .Where(x => x.GisGkhRegionalProgramCR == program)
                    .Where(x => x.VersionRecord.ProgramVersion.IsMain)
                    .Where(x => x.VersionRecordStage1.VersionRecordState != VersionRecordState.NonActual).ToList();

                allStages3.ForEach(x =>
                {
                    foreach (VersionRecordStage1 stage1 in x.Value)
                    {
                        GisGkhVersionRecord gisWork = gisVersionRec.Where(y => y.VersionRecord == x.Key && y.VersionRecordStage1 == stage1).FirstOrDefault();
                        // если уже есть работа для ГИС ЖКХ
                        if (gisWork != null)
                        {
                            // значит её уже отправили и делать ничего не надо
                            // но если нет гуидов, то что-то пошло не так, значит добавим в выгрузку
                            if (string.IsNullOrEmpty(gisWork.GisGkhTransportGuid) && string.IsNullOrEmpty(gisWork.GisGkhGuid))
                            {
                                gisWork.GisGkhTransportGuid = Guid.NewGuid().ToString();
                                GisGkhVersionRecordDomain.Update(gisWork);
                                gisWorkList.Add(gisWork);
                            }
                        }
                        // если нет работы для ГИС ЖКХ - создаём
                        else
                        {
                            gisWork = new GisGkhVersionRecord
                            {
                                VersionRecord = x.Key,
                                VersionRecordStage1 = stage1,
                                GisGkhTransportGuid = Guid.NewGuid().ToString()
                            };
                            GisGkhVersionRecordDomain.Save(gisWork);
                            gisWorkList.Add(gisWork);
                        }
                    }
                });

                var reqNum = gisWorkList.Count() / 1000;
                if (gisWorkList.Count() % 1000 > 0)
                {
                    reqNum++;
                }
                for (int i = 0; i < reqNum; i++)
                {
                    var worksPart = gisWorkList.Skip(i * 1000).Take(1000);
                    if (i > 0)
                    {
                        var newReq = new GisGkhRequests();
                        newReq.TypeRequest = req.TypeRequest;
                        newReq.Operator = req.Operator;
                        newReq.RequestState = GisGkhRequestState.NotFormed;
                        req = newReq;
                        GisGkhRequestsDomain.Save(req);
                        log += $"{DateTime.Now} Запрос на выгрузку {i + 1} части работ ДПКР в ГИС ЖКХ\r\n";
                    }
                    List<importRegionalProgramWorkRequestImportRegionalProgramWork> worksForImport = new List<importRegionalProgramWorkRequestImportRegionalProgramWork>();
                    //string errs = "";
                    worksPart.Where(x => string.IsNullOrEmpty(strElGisCodes[x.VersionRecordStage1.StructuralElement.Id].GisGkhGuid) || string.IsNullOrEmpty(strElGisCodes[x.VersionRecordStage1.StructuralElement.Id].GisGkhCode)).ForEach(x =>
                    {
                        //errs += $"\r\n{strElGisCodes[x.VersionRecordStage1.StructuralElement.Id].Name} - вид работы не сопостаавлен с ГИС ЖКХ";
                        log += $"{strElGisCodes[x.VersionRecordStage1.StructuralElement.Id].Name} - вид работы не сопостаавлен с ГИС ЖКХ\r\n";
                    });
                    worksPart.Where(x => string.IsNullOrEmpty(x.VersionRecord.RealityObject.HouseGuid) && (x.VersionRecord.RealityObject.FiasAddress.HouseGuid == null)).ForEach(x =>
                    {
                        //errs += $"\r\n{x.VersionRecord.RealityObject.Id}: {x.VersionRecord.RealityObject.Address} - у дома отсутствует идентификатор ФИАС";
                        log += $"{x.VersionRecord.RealityObject.Id}: {x.VersionRecord.RealityObject.Address} - у дома отсутствует идентификатор ФИАС\r\n";
                    });
                    int workNum = 0;
                    foreach (var work in worksPart.Where(x => !string.IsNullOrEmpty(strElGisCodes[x.VersionRecordStage1.StructuralElement.Id].GisGkhGuid) && !string.IsNullOrEmpty(strElGisCodes[x.VersionRecordStage1.StructuralElement.Id].GisGkhCode)))
                    {
                        CrPeriod workPeriod = null;
                        foreach (var periodCr in periodsCr)
                        {
                            if (work.VersionRecord.Year >= periodCr.YearStart && work.VersionRecord.Year <= periodCr.YearEnd)
                            {
                                workPeriod = periodCr;
                                break;
                            }
                        }
                        var workItem = new RegionalProgramWorkType
                        {
                            EndYearMonth = workPeriod != null ? $"{workPeriod.YearEnd}-12" : $"{work.VersionRecord.Year}-12",
                            StartYearMonth = workPeriod != null ? $"{workPeriod.YearStart}-01" : $"{work.VersionRecord.Year}-01",
                            WorkType = new nsiRef
                            {
                                Code = strElGisCodes[work.VersionRecordStage1.StructuralElement.Id].GisGkhCode,
                                GUID = strElGisCodes[work.VersionRecordStage1.StructuralElement.Id].GisGkhGuid
                            },
                            OKTMO = new OKTMORefType
                            {
                                code = work.VersionRecord.RealityObject.Municipality.Oktmo
                            }
                        };
                        if (!string.IsNullOrEmpty(work.VersionRecord.RealityObject.HouseGuid))
                        {
                            workItem.FiasHouseGUID = work.VersionRecord.RealityObject.HouseGuid;
                        }
                        else if (work.VersionRecord.RealityObject.FiasAddress.HouseGuid != null)
                        {
                            workItem.FiasHouseGUID = work.VersionRecord.RealityObject.FiasAddress.HouseGuid.Value.ToString();
                        }
                        else
                        {
                            // нет гуида на доме
                            continue;
                        }
                        importRegionalProgramWorkRequestImportRegionalProgramWork importWork = new importRegionalProgramWorkRequestImportRegionalProgramWork
                        {
                            TransportGuid = work.GisGkhTransportGuid,
                            Item = workItem
                        };

                        worksForImport.Add(importWork);
                        //work.GisGkhTransportGuid = transportGuid;
                        //TypeWorkCrDomain.Update(work);
                        workNum++;
                    }
                
                    var request = HcsCapitalRepairAsync.importRegionalProgramWorkReq(program.GisGkhGuid, worksForImport);
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    req.Answer = "Запрос на выгрузку работ ДПКР сформирован";
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
                            }
                            else if (responseItem is CapRemCommonResultType)
                            {
                                var regionalProgramWorkResponse = (CapRemCommonResultType)responseItem;
                                if (regionalProgramWorkResponse != null)
                                {
                                    var gisGkhVersionRecord = GisGkhVersionRecordDomain.GetAll()
                                        .Where(x => x.GisGkhTransportGuid == regionalProgramWorkResponse.TransportGUID).FirstOrDefault();
                                    if (gisGkhVersionRecord != null)
                                    {
                                        //bool err = false;
                                        foreach (var item in regionalProgramWorkResponse.Items)
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
                                                gisGkhVersionRecord.GisGkhGuid = regionalProgramWorkResponse.GUID;
                                                gisGkhVersionRecord.GisGkhTransportGuid = null;
                                                GisGkhVersionRecordDomain.Update(gisGkhVersionRecord);
                                                workNum++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        log += $"{DateTime.Now} В системе не найдена работа GisGkhVersionRecord с TransportGUID {regionalProgramWorkResponse.TransportGUID}\r\n";
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

    ///// <summary>
    ///// Детализированная запись работы долгосрочки
    ///// </summary>
    //public class VersionRecDetail
    //{
    //    public virtual VersionRecord VersionRecord { get; set; }

    //    public virtual long StrElId { get; set; }

    //    public virtual decimal Sum { get; set; }



    //    public virtual string CommonEstateObjects { get; set; }

    //    public virtual int Year { get; set; }

    //    public virtual int IndexNumber { get; set; }

    //    public virtual bool IsChangedYear { get; set; }

    //    public virtual decimal Point { get; set; }

    //    public virtual decimal Sum { get; set; }

    //    public virtual string Changes { get; set; }

    //    public virtual string StructuralElements { get; set; }

    //    public virtual string EntranceNum { get; set; }

    //    public virtual string KPKR { get; set; }

    //    public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

    //    public virtual List<StoredPointParam> StoredPointParams { get; set; }

    //    public bool Hidden { get; set; }

    //    public bool IsSubProgram { get; set; }
    //}
}
