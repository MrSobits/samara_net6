using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.FileManager;
using Bars.GkhCr.Entities;
using Castle.Windsor;
//using CryptoPro.Sharpei;
using GisGkhLibrary.Enums;
using GisGkhLibrary.HcsCapitalRepairAsync;
using GisGkhLibrary.Services;
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
    public class ImportBuildContractService : IImportBuildContractService
    {
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<BuildContract> BuildContractDomain { get; set; }

        public IDomainService<BuildContractTypeWork> TypeWorkCrDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        private readonly IFileService _fileService;
        private readonly IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        public ImportBuildContractService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService fileService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _fileService = fileService;
        }

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

                var contractId = long.Parse(reqParams[1]);
                var contract = BuildContractDomain.Load(contractId);

                var works = TypeWorkCrDomain.GetAll()
                    .Where(x => x.BuildContract == contract)
                    .Select(x => x.TypeWork)
                    .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                    .ToList();

                if (string.IsNullOrEmpty(program.GisGkhGuid))
                {
                    throw new Exception("Данная КПР не выгружена");
                }

                var docDate = (contract.DocumentDateFrom != null && contract.DocumentDateFrom != DateTime.MinValue) ? contract.DocumentDateFrom.Value : contract.ObjectCreateDate;

                if (string.IsNullOrEmpty(contract.GisGkhGuid))
                {
                    if (works.IsNotEmpty())
                    {
                        if (string.IsNullOrEmpty(contract.GisGkhTransportGuid))
                        {
                            if (!string.IsNullOrEmpty(contract.Builder.Contragent.GisGkhOrgPPAGUID))
                            {
                                if (contract.DocumentFile != null)
                                {
                                    log += $"{DateTime.Now} Формирование запроса на выгрузку договора КПР \"{contract.DocumentNum}\" от \"{docDate}\" в ГИС ЖКХ\r\n";

                                    var hash = GetGhostHash(_fileManager.GetFile(contract.DocumentFile));
                                    var uploadResult = _fileService.UploadFile(GisFileRepository.capitalrepairprograms, contract.DocumentFile, OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        contract.GisGkhDocumentGuid = uploadResult.FileGuid;
                                        BuildContractDomain.Update(contract);

                                        List<WorkContractType> worksForImport = new List<WorkContractType>();

                                        foreach (var work in works)
                                        {
                                            WorkContractType importWork = new WorkContractType
                                            {
                                                Item = work.GisGkhGuid,
                                                StartDate = (work.DateStartWork != null && work.DateStartWork != DateTime.MinValue) ?
                                                    work.DateStartWork.Value :
                                                    work.ObjectCreateDate != DateTime.MinValue ?
                                                        work.ObjectCreateDate :
                                                        docDate,
                                                EndDate = work.DateEndWork ?? new DateTime(docDate.Year, 12, 31),
                                                Cost = work.Sum.HasValue ? work.Sum.Value.ToMagic(2) : 0,
                                                CostPlan = work.Sum.HasValue ? work.Sum.Value.ToMagic(2) : 0,
                                                Volume = (work.Volume ?? 0).ToMagic(2)
                                            };

                                            if (work.Work.UnitMeasure.OkeiCode != null)
                                            {
                                                importWork.Item1 = work.Work.UnitMeasure.OkeiCode;
                                                importWork.Item1ElementName = Item1ChoiceType.OKEI;
                                            }
                                            else
                                            {
                                                importWork.Item1 = work.Work.UnitMeasure.Name;
                                                importWork.Item1ElementName = Item1ChoiceType.OtherUnit;
                                            }

                                            worksForImport.Add(importWork);
                                        }

                                        var transportGuid = Guid.NewGuid().ToString();

                                        importContractsRequestImportContract[] importContract = new importContractsRequestImportContract[]
                                        {
                                            new importContractsRequestImportContract
                                            {
                                                TransportGuid = transportGuid,
                                                Item = new ContractType
                                                {
                                                    Number = contract.DocumentNum,
                                                    Date = docDate,
                                                    StartDate = (contract.DateStartWork != null && contract.DateStartWork != DateTime.MinValue) ? contract.DateStartWork.Value : docDate,
                                                    EndDate = contract.DateEndWork ?? new DateTime(docDate.Year, 12, 31),
                                                    Sum = (contract.Sum ?? 0).ToMagic(2),
                                                    Item = new RegOrgType()
                                                    {
                                                        orgRootEntityGUID = contract.Contragent.GisGkhOrgPPAGUID
                                                    },
                                                    Performer = new RegOrgType()
                                                    {
                                                        orgRootEntityGUID = contract.Builder.Contragent.GisGkhOrgPPAGUID
                                                    },
                                                    Work = worksForImport.ToArray(),
                                                    AttachContract = new AttachmentType[]
                                                    {
                                                        new AttachmentType
                                                        {
                                                            Attachment = new Attachment
                                                            {
                                                                AttachmentGUID = contract.GisGkhDocumentGuid
                                                            },
                                                            Description = contract.DocumentFile.Name,
                                                            Name = contract.DocumentFile.FullName,
                                                            AttachmentHASH = hash
                                                        }
                                                    },
                                                    Items = new object[]
                                                    {
                                                        true
                                                    },
                                                    Item2 = true
                                                }
                                            }
                                        };

                                        if (contract.GuaranteePeriod != null && contract.GuaranteePeriod != 0)
                                            (importContract[0].Item as ContractType).Item1 = (contract.GuaranteePeriod.Value * 12).ToString();
                                        else
                                            (importContract[0].Item as ContractType).Item1 = true;

                                        contract.GisGkhTransportGuid = transportGuid;
                                        var request = HcsCapitalRepairAsync.importBuildContractReq(program.GisGkhGuid, importContract);
                                        var prefixer = new GisGkhLibrary.Utils.XmlNsPrefixer();
                                        XmlDocument document = SerializeRequest(request);
                                        prefixer.Process(document);
                                        SaveFile(req, GisGkhFileType.request, document, "request.xml");
                                        req.RequestState = GisGkhRequestState.Formed;
                                        req.Answer = "Запрос на выгрузку договора КПР сформирован";
                                        log += $"{DateTime.Now} Запрос сформирован\r\n";
                                        SaveLog(ref req, ref log);
                                    }
                                    else
                                    {
                                        log += $"{DateTime.Now} Ошибка при выгрузке файла договора КПР \"{contract.DocumentNum}\" от \"{docDate}\": {uploadResult.Message}\r\n";
                                        req.Answer += $"{DateTime.Now} Ошибка при выгрузке файла договора КПР \"{contract.DocumentNum}\" от \"{docDate}\": {uploadResult.Message}\r\n";
                                        req.RequestState = GisGkhRequestState.Error;
                                    }
                                }
                                else
                                {
                                    log += $"{DateTime.Now} К договору КПР \"{contract.DocumentNum}\" от \"{docDate}\" не прикреплен документ договора\r\n";
                                    req.Answer += $"{DateTime.Now} К договору КПР \"{contract.DocumentNum}\" от \"{docDate}\" не прикреплен документ договора\r\n";
                                    req.RequestState = GisGkhRequestState.Error;
                                }
                            }
                            else
                            {
                                log += $"{DateTime.Now} Необьходимо выгрузить в ГИС ЖКХ подрядчика по договору \"{contract.DocumentNum}\" от \"{docDate}\"\r\n";
                                req.Answer += $"{DateTime.Now} Необьходимо выгрузить в ГИС ЖКХ подрядчика по договору \"{contract.DocumentNum}\" от \"{docDate}\"\r\n";
                                req.RequestState = GisGkhRequestState.Error;
                            }
                        }
                        else
                        {
                            log += $"{DateTime.Now} Запрос с договором КПР \"{contract.DocumentNum}\" от \"{docDate}\" уже отправлен\r\n";
                            req.Answer += $"{DateTime.Now} Запрос с договором КПР \"{contract.DocumentNum}\" от \"{docDate}\" уже отправлен\r\n";
                            req.RequestState = GisGkhRequestState.Error;
                        }
                    }
                    else
                    {
                        log += $"{DateTime.Now} По договору КПР \"{contract.DocumentNum}\" от \"{docDate}\" не выгружена ни одна работа\r\n";
                        req.Answer += $"{DateTime.Now} По договору КПР \"{contract.DocumentNum}\" от \"{docDate}\" не выгружена ни одна работа\r\n";
                        req.RequestState = GisGkhRequestState.Error;
                    }
                }
                else
                {
                    log += $"{DateTime.Now} Договор КПР \"{contract.DocumentNum}\" от \"{docDate}\" уже выгружен\r\n";
                    req.Answer += $"{DateTime.Now} Договор КПР \"{contract.DocumentNum}\" от \"{docDate}\" уже выгружен\r\n";
                    req.RequestState = GisGkhRequestState.Error;
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.Answer += $"{DateTime.Now} Ошибка: {e.Message}\r\n";
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
                            req.Answer = $"Задача на обработку ответа importContract поставлена в очередь с id {taskInfo.TaskId}";
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
                        var response = DeserializeData<getStateResult>(Encoding.UTF8.GetString(data));
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType type)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = type;
                                req.Answer = $"{error.ErrorCode}:{error.Description}";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                            }
                            else if (responseItem is CapRemCommonResultType contractResponse)
                            {
                                var contract = BuildContractDomain.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == contractResponse.TransportGUID)
                                    .FirstOrDefault();

                                if (contract != null)
                                {
                                    bool err = false;
                                    foreach (var item in contractResponse.Items)
                                    {
                                        if (item is CommonResultTypeError error)
                                        {
                                            log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                                            err = true;
                                        }
                                    }
                                    if (!err)
                                    {
                                        contract.GisGkhGuid = contractResponse.GUID;
                                        contract.GisGkhTransportGuid = null;
                                        BuildContractDomain.Update(contract);
                                        log += $"{DateTime.Now} Договор КПР выгружен в ГИС ЖКХ с GUID {contractResponse.GUID}\r\n";
                                    }
                                }
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
                    throw new Exception("Ошибка: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Получить хэш файла по алгоритму ГОСТ Р 34.11-94
        /// </summary>
        public static string GetGhostHash(Stream content)
        {
            return ";";
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
        }
    }
}
