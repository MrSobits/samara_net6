using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.FileManager;
using Bars.GkhCr.Entities;
using Bars.GkhCr.Enums;
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
    public class ImportPerfWorkActService : IImportPerfWorkActService
    {
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<BuildContract> BuildContractDomain { get; set; }

        public IDomainService<PerformedWorkAct> CertificateDomain { get; set; }

        public IDomainService<PerformedWorkActPhoto> PhotoDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        private readonly IFileService _fileService;
        private readonly IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        public ImportPerfWorkActService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService fileService)
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

                var certificateId = long.Parse(reqParams[2]);
                var certificate = CertificateDomain.Load(certificateId);

                var work = certificate.TypeWorkCr;

                if (string.IsNullOrEmpty(program.GisGkhGuid))
                {
                    throw new Exception("Данная КПР не выгружена");
                }

                var docDate = (certificate.DateFrom != null && certificate.DateFrom != DateTime.MinValue) ? certificate.DateFrom.Value : certificate.ObjectCreateDate;

                if (string.IsNullOrEmpty(certificate.GisGkhGuid))
                {
                    if (!string.IsNullOrEmpty(contract.GisGkhGuid))
                    {
                        if (string.IsNullOrEmpty(certificate.GisGkhTransportGuid))
                        {
                            if (certificate.DocumentFile != null)
                            {
                                log += $"{DateTime.Now} Формирование запроса на выгрузку акта КПР \"{certificate.DocumentNum}\" от \"{docDate}\" в ГИС ЖКХ\r\n";

                                var transportGuid = Guid.NewGuid().ToString();

                                var hash = GetGhostHash(_fileManager.GetFile(certificate.DocumentFile));
                                var uploadResult = _fileService.UploadFile(GisFileRepository.capitalrepairprograms, certificate.DocumentFile, OrgPPAGUID);

                                if (uploadResult.Success)
                                {
                                    certificate.GisGkhDocumentGuid = uploadResult.FileGuid;
                                    CertificateDomain.Update(certificate);

                                    importCertificatesRequestImportCertificate[] importCertificate = new importCertificatesRequestImportCertificate[]
                                    {
                                        new importCertificatesRequestImportCertificate
                                        {
                                            TransportGuid = transportGuid,
                                            Item = new importCertificatesRequestImportCertificateLoadCertificate
                                            {
                                                Name = $"{certificate.DocumentNum} от {docDate}",
                                                Number = certificate.DocumentNum,
                                                Date = docDate,
                                                SumAcceptedWorks = (certificate.Sum ?? 0).ToMagic(2),
                                                PerformerPenalties = 0,
                                                CustomerPenalties = 0,
                                                AttachCertificate = new AttachmentType[]
                                                {
                                                    new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = certificate.GisGkhDocumentGuid
                                                        },
                                                        Description = certificate.DocumentFile.Name,
                                                        Name = certificate.DocumentFile.FullName,
                                                        AttachmentHASH = hash
                                                    }
                                                },
                                                Item = true,
                                                Items = new object[]
                                                {
                                                    true
                                                },
                                                Work = new CertificateTypeWork[]
                                                {
                                                    new CertificateTypeWork
                                                    {
                                                        WorkInContract = new WorkContractIdentityType
                                                        {
                                                            Item = work.GisGkhGuid
                                                        },
                                                        WorkCost = (certificate.Sum ?? 0).ToMagic(2),
                                                        WorkValue = (certificate.Volume ?? 0).ToMagic(2),
                                                        Item = new WorkCertificateTypeAppForUse
                                                        {
                                                            StartDateGuarantee = docDate,
                                                            EndDateGuarantee = contract.GuaranteePeriod != null ? docDate.AddYears(contract.GuaranteePeriod.Value) : docDate
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    };

                                    var photos = PhotoDomain.GetAll()
                                        .Where(x => x.PerformedWorkAct.Id == certificateId)
                                        .ToList();

                                    if (photos.Any())
                                    {
                                        var deforePhotosList = new List<AttachmentType>();
                                        var afterPhotosList = new List<AttachmentType>();

                                        foreach (var photo in photos) 
                                        {
                                            var photoGuid = Guid.NewGuid().ToString();
                                            var photohash = GetGhostHash(_fileManager.GetFile(photo.Photo));
                                            var photouploadResult = _fileService.UploadFile(GisFileRepository.capitalrepairprograms, photo.Photo, OrgPPAGUID);
                                            if (photouploadResult.Success)
                                            {
                                                var photoAttach = new AttachmentType
                                                {
                                                    Attachment = new Attachment
                                                    {
                                                        AttachmentGUID = photoGuid
                                                    },
                                                    Description = photo.Photo.Name,
                                                    Name = photo.Photo.FullName,
                                                    AttachmentHASH = photohash
                                                };

                                                if (photo.PhotoType == PerfWorkActPhotoType.Before)
                                                {
                                                    deforePhotosList.Add(photoAttach);
                                                }
                                                else
                                                {
                                                    afterPhotosList.Add(photoAttach);
                                                }
                                            }
                                        }

                                        if (deforePhotosList.Any()) 
                                        {
                                            ((importCertificatesRequestImportCertificateLoadCertificate)importCertificate[0].Item).AttachPhotoBefore = deforePhotosList.ToArray();
                                        }

                                        if (afterPhotosList.Any())
                                        {
                                            ((importCertificatesRequestImportCertificateLoadCertificate)importCertificate[0].Item).AttachPhotoAfter = afterPhotosList.ToArray();
                                        }
                                    }

                                    certificate.GisGkhTransportGuid = transportGuid;
                                    var request = HcsCapitalRepairAsync.importPerfWorkActReq(contract.GisGkhGuid, importCertificate);
                                    var prefixer = new GisGkhLibrary.Utils.XmlNsPrefixer();
                                    XmlDocument document = SerializeRequest(request);
                                    prefixer.Process(document);
                                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                                    req.RequestState = GisGkhRequestState.Formed;
                                    req.Answer = "Запрос на выгрузку акта КПР сформирован";
                                    log += $"{DateTime.Now} Запрос сформирован\r\n";
                                    SaveLog(ref req, ref log);
                                }
                                else
                                {
                                    log += $"{DateTime.Now} Ошибка при выгрузке файла акта КПР \"{certificate.DocumentNum}\" от \"{docDate}\": {uploadResult.Message}\r\n";
                                    req.Answer += $"{DateTime.Now} Ошибка при выгрузке файла акта КПР \"{certificate.DocumentNum}\" от \"{docDate}\": {uploadResult.Message}\r\n";
                                    req.RequestState = GisGkhRequestState.Error;
                                }
                            }
                            else
                            {
                                log += $"{DateTime.Now} К акту КПР \"{certificate.DocumentNum}\" от \"{docDate}\" не прикреплен документ акта\r\n";
                                req.Answer += $"{DateTime.Now} К акту КПР \"{certificate.DocumentNum}\" от \"{docDate}\" не прикреплен документ акта\r\n";
                                req.RequestState = GisGkhRequestState.Error;
                            }
                        }
                        else
                        {
                            log += $"{DateTime.Now} Запрос с актом КПР \"{certificate.DocumentNum}\" от \"{docDate}\" уже отправлен\r\n";
                            req.Answer += $"{DateTime.Now} Запрос с актом КПР \"{certificate.DocumentNum}\" от \"{docDate}\" уже отправлен\r\n";
                            req.RequestState = GisGkhRequestState.Error;
                        }
                    }
                    else
                    {
                        log += $"{DateTime.Now} Договор КПР \"{contract.DocumentNum}\" по акту \"{certificate.DocumentNum}\" от \"{docDate}\" не выгружен\r\n";
                        req.Answer += $"{DateTime.Now} Договор КПР \"{contract.DocumentNum}\" по акту \"{certificate.DocumentNum}\" от \"{docDate}\" не выгружен\r\n";
                        req.RequestState = GisGkhRequestState.Error;
                    }
                }
                else
                {
                    log += $"{DateTime.Now} Акт КПР \"{certificate.DocumentNum}\" от \"{docDate}\" уже выгружен\r\n";
                    req.Answer += $"{DateTime.Now} Акт КПР \"{certificate.DocumentNum}\" от \"{docDate}\" уже выгружен\r\n";
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
                            req.Answer = $"Задача на обработку ответа importCertificate поставлена в очередь с id {taskInfo.TaskId}";
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
                            else if (responseItem is CapRemCommonResultType certificateResponse)
                            {
                                var certificate = CertificateDomain.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == certificateResponse.TransportGUID)
                                    .FirstOrDefault();

                                if (certificate != null)
                                {
                                    bool err = false;
                                    foreach (var item in certificateResponse.Items)
                                    {
                                        if (item is CommonResultTypeError error)
                                        {
                                            log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                                            err = true;
                                        }
                                    }
                                    if (!err)
                                    {
                                        certificate.GisGkhGuid = certificateResponse.GUID;
                                        certificate.GisGkhTransportGuid = null;
                                        CertificateDomain.Update(certificate);
                                        log += $"{DateTime.Now} Акт КПР выгружен в ГИС ЖКХ с GUID {certificateResponse.GUID}\r\n";
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
            // TODO: Заменить криптопро
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
