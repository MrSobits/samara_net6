using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
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
    public class ImportPlanService : IImportPlanService
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

        public ImportPlanService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
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

                //List<ProgramCr> programs = ProgramCrDomain.GetAll()
                //    .Where(x => x.Id == planId).ToList();
                //bool imported = false;
                //bool importing = false;
                //if (programs.Count == 0)
                //    {
                //        throw new Exception("КПР не найден");
                //    }
                //else foreach (var program in programs)
                //{
                //    if (!string.IsNullOrEmpty(program.GisGkhGuid))
                //        {
                //            imported = true;
                //        }
                //    if (!string.IsNullOrEmpty(program.GisGkhTransportGuid))
                //        {
                //            importing = true;
                //        }
                //}
                if (!string.IsNullOrEmpty(program.GisGkhGuid))
                {
                    throw new Exception("Данный КПР уже выгружен");
                }
                if (!string.IsNullOrEmpty(program.GisGkhTransportGuid))
                {
                    throw new Exception("Запрос с данным КПР уже отправлен");
                }
                //if (importing)
                //{
                //    throw new Exception("Запрос с КПР данного периода уже отправлен");
                //}
                //if (imported)
                //{
                //    throw new Exception("КПР за данный период уже выгружен");
                //}

                //var planId = long.Parse(reqParams[0]);
                //var plan = ProgramCrDomain.GetAll()
                //    .Where(x => x.Id == planId).FirstOrDefault();
                //if (plan == null)
                //{
                //    throw new Exception("КПР не найден");
                //}
                //if (plan.GisGkhTransportGuid != null && plan.GisGkhTransportGuid != "")
                //{
                //    throw new Exception("Запрос с данным КПР уже отправлен");
                //}
                //else
                //{
                log += $"{DateTime.Now} Формирование запроса на выгрузку КПКР \"{program.Name}\" за период \"{program.Period.Name}\" в ГИС ЖКХ\r\n";
                var OKTMO = reqParams[1];
                // PlanPassportTypeType и exportPlanRequestType совпадают
                var planType = (PlanPassportTypeType)Enum.Parse(typeof(PlanPassportTypeType), reqParams[2]);
                var transportGuid = Guid.NewGuid().ToString();
                List<importPlanRequestImportPlanPlanDocument> PlanDocument = new List<importPlanRequestImportPlanPlanDocument>();
                //List<string> hashes = new List<string>();
                //foreach (var program in programs)
                //{
                program.GisGkhTransportGuid = transportGuid;
                string docTransportGuid = Guid.NewGuid().ToString();
                if (program.File != null)
                {
                    if (string.IsNullOrEmpty(program.GisGkhDocumentAttachmentGuid))
                    {
                        var hash = GetGhostHash(_fileManager.GetFile(program.File));
                        //if (!hashes.Contains(hash))
                        //{
                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, program.File, OrgPPAGUID);
                        if (uploadResult.Success)
                        {
                            program.GisGkhDocumentAttachmentGuid = uploadResult.FileGuid;
                            program.GisGkhDocumentTransportGuid = docTransportGuid;
                            ProgramCrRepo.Update(program);
                            //hashes.Add(hash);
                            PlanDocument.Add(new importPlanRequestImportPlanPlanDocument
                            {
                                TransportGuid = docTransportGuid,
                                Item = new ImportDocumentTypeLoadDocument
                                {
                                    Kind = new nsiRef
                                    {
                                        Code = "26",
                                        GUID = "fed218e8-4925-4403-8a48-341b74270894"
                                    },
                                    FullName = program.File.Name,
                                    Number = program.DocumentNumber,
                                    Date = program.DocumentDate != null ? program.DocumentDate.Value : DateTime.MinValue,
                                    Legislature = program.DocumentDepartment,
                                    AttachDocument = new AttachmentType[]
                                {
                                            new AttachmentType
                                            {
                                                Attachment = new Attachment
                                                {
                                                    AttachmentGUID = program.GisGkhDocumentAttachmentGuid
                                                },
                                                Description = program.File.Name,
                                                Name = program.File.FullName,
                                                AttachmentHASH = hash
                                            }
                                }
                                }
                            });
                        }
                        else
                        {
                            throw new Exception(uploadResult.Message);
                        }
                        //}
                    }
                    //plan.GisGkhDocumentTransportGuid = Guid.NewGuid().ToString();
                }
                //}

                importPlanRequestImportPlan importPlan = new importPlanRequestImportPlan
                {
                    TransportGuid = transportGuid,
                    LoadPlan = new PlanPassportType
                    {
                        Name = program.Name,
                        StartMonthYear = program.Period.DateStart.ToString("yyyy-MM"),
                        EndMonthYear = program.Period.DateEnd != null ? program.Period.DateEnd.Value.ToString("yyyy-MM")
                                    : program.Period.DateStart.Year.ToString() + "-12",
                        Territory = new OKTMORefType
                        {
                            code = OKTMO
                        },
                        Type = planType
                    },
                    Item = true,
                    ItemElementName = ItemChoiceType3.PublishPlan
                };
                if (PlanDocument.Count > 0)
                {
                    importPlan.PlanDocument = PlanDocument.ToArray();
                }



                /*if (plan.File != null)
                {
                    if (plan.GisGkhDocumentAttachmentGuid == null || plan.GisGkhDocumentAttachmentGuid == "")
                    {
                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, plan.File, OrgPPAGUID);
                        if (uploadResult.Success)
                        {
                            plan.GisGkhDocumentAttachmentGuid = uploadResult.FileGuid;
                            ProgramCrRepo.Update(plan);
                        }
                        else
                        {
                            throw new Exception(uploadResult.Message);
                        }
                    }
                    plan.GisGkhDocumentTransportGuid = Guid.NewGuid().ToString();




                    importPlan = new importPlanRequestImportPlan
                    {
                        TransportGuid = plan.GisGkhTransportGuid,
                        LoadPlan = new PlanPassportType
                        {
                            Name = plan.Name,
                            StartMonthYear = plan.Period.DateStart.Year.ToString() + "-" + plan.Period.DateStart.Month.ToString(),
                            EndMonthYear = plan.Period.DateEnd != null ? plan.Period.DateEnd.Value.Year.ToString() + "-" + plan.Period.DateEnd.Value.Month.ToString()
                                : plan.Period.DateStart.Year.ToString() + "-12",
                            Territory = new OKTMORefType
                            {
                                code = OKTMO
                            },
                            Type = planType
                        },
                        PlanDocument = new importPlanRequestImportPlanPlanDocument[]
                        {
                                new importPlanRequestImportPlanPlanDocument
                                {
                                    TransportGuid = plan.GisGkhDocumentTransportGuid,
                                    Item = new ImportDocumentTypeLoadDocument
                                    {
                                        Kind = new nsiRef
                                        {
                                            Code = "26",
                                            GUID = "fed218e8-4925-4403-8a48-341b74270894"
                                        },
                                        FullName = plan.File.Name,
                                        Number = plan.DocumentNumber,
                                        Date = plan.DocumentDate != null ? plan.DocumentDate.Value : DateTime.MinValue,
                                        Legislature = plan.DocumentDepartment,
                                        AttachDocument = new AttachmentType[]
                                        {
                                            new AttachmentType
                                            {
                                                Attachment = new Attachment
                                                {
                                                    AttachmentGUID = plan.GisGkhDocumentAttachmentGuid
                                                },
                                                Description = plan.File.Name,
                                                Name = plan.File.FullName,
                                                AttachmentHASH = GetGhostHash(_fileManager.GetFile(plan.File))
                                            }
                                        }
                                    }
                                }
                        },
                        ItemElementName = ItemChoiceType3.PublishPlan,
                        Item = true
                    };
                }
                else
                {
                    importPlan = new importPlanRequestImportPlan
                    {
                        TransportGuid = plan.GisGkhTransportGuid,
                        LoadPlan = new PlanPassportType
                        {
                            Name = plan.Name,
                            StartMonthYear = plan.Period.DateStart.ToString("yyyy-MM"),/*.Year.ToString() + " - " + plan.Period.DateStart.Month..ToString(),
                            EndMonthYear = plan.Period.DateEnd != null ? plan.Period.DateEnd.Value.ToString("yyyy-MM")
                                : plan.Period.DateStart.Year.ToString() + "-12",
                            Territory = new OKTMORefType
                            {
                                code = OKTMO
                            },
                            Type = planType
                        },
                        //PlanDocument = null,
                        ItemElementName = ItemChoiceType3.PublishPlan,
                        Item = true
                    };
                    //}
                    ProgramCrRepo.Update(plan);*/


                var request = HcsCapitalRepairAsync.importPlanReq(importPlan);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                //SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");
                //req.ReqFileInfo = SaveFile(document, "Request.dat");
                req.RequestState = GisGkhRequestState.Formed;
                req.Answer = "Запрос на выгрузку КПР сформирован";
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
                //}

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
                log += $"{DateTime.Now} Запрос ответа\r\n";
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
                            //req.RequestState = GisGkhRequestState.У; ("Сбой создания задачи");
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа importPlan поставлена в очередь с id {taskInfo.TaskId}";
                            //log += $"{DateTime.Now} Задача на обработку ответа importPlan поставлена в очередь с id {taskInfo.TaskId}\r\n";
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
                            else if (responseItem is CapRemCommonResultType)
                            {
                                var planResponse = (CapRemCommonResultType)responseItem;
                                var programCr = ProgramCrRepo.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == planResponse.TransportGUID).FirstOrDefault();
                                if (programCr != null)
                                {
                                    bool err = false;
                                    foreach (var item in planResponse.Items)
                                    {
                                        if (item is CommonResultTypeError)
                                        {
                                            //req.RequestState = GisGkhRequestState.Error;
                                            //req.Answer = $"{((CommonResultTypeError)item).ErrorCode}:{((CommonResultTypeError)item).Description}";
                                            log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {((CommonResultTypeError)item).ErrorCode}:{((CommonResultTypeError)item).Description}\r\n";
                                            err = true;
                                        }
                                    }
                                    if (!err)
                                    {
                                        programCr.GisGkhGuid = planResponse.GUID;
                                        programCr.GisGkhTransportGuid = null;
                                        ProgramCrRepo.Update(programCr);
                                        log += $"{DateTime.Now} КПКР выгружен в ГИС ЖКХ с GUID {planResponse.GUID}\r\n";
                                    }
                                }

                                //req.Answer = "Ответ пришёл";
                                //req.RequestState = GisGkhRequestState.ResponseReceived;
                                //GisGkhRequestsDomain.Update(req);

                            }
                        }
                        if (req.RequestState != GisGkhRequestState.Error)
                        {
                            req.Answer = "Данные из ГИС ЖКХ обработаны";
                            req.RequestState = GisGkhRequestState.ResponseProcessed;
                        }
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
        /// <summary>
        /// Получить хэш файла по алгоритму ГОСТ Р 34.11-94
        /// </summary>
        public static string GetGhostHash(Stream content)
        {
            return "";
            /*using (var gost = Gost3411.Create())
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
