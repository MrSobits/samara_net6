using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities.Suggestion;
using Bars.Gkh.FileManager;
using Bars.GkhGji.DomainService.GisGkhRegional;
using Castle.Windsor;
//using CryptoPro.Sharpei;
using GisGkhLibrary.AppealsServiceAsync;
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
    public class ImportAnswerCRService : IImportAnswerCRService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<CitizenSuggestion> CitizenSuggestionDomain { get; set; }
        public IDomainService<CitizenSuggestionFiles> CitizenSuggestionFilesDomain { get; set; }
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        private IFileService _fileService;
        private IGisGkhRegionalService _gisRegionalService;

        #endregion

        #region Constructors

        public ImportAnswerCRService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager, IFileService FileService, IGisGkhRegionalService gisRegionalService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _fileService = FileService;
            _gisRegionalService = gisRegionalService;
        }

        #endregion

        #region Public methods

        public void SaveAppealRequest(GisGkhRequests req, CitizenSuggestion citizenSuggestion)
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
                log += $"{DateTime.Now} Формирование запроса на назначение проверяющего и контрольного срока обращения ФКР {citizenSuggestion.Number} от " +
                    $"{citizenSuggestion.CreationDate.ToShortDateString()}\r\n";
                citizenSuggestion.GisGkhTransportGuid = Guid.NewGuid().ToString();
                CitizenSuggestionDomain.Update(citizenSuggestion);
                importAnswerRequestAppealAction appealAction = new importAnswerRequestAppealAction
                {
                    TransportGUID = citizenSuggestion.GisGkhTransportGuid,
                    ItemElementName = ItemChoiceType2.AppealGUID,
                    Item = citizenSuggestion.GisGkhGuid,
                    Performer = new importAnswerRequestAppealActionPerformer
                    {
                        AnswererGUID = citizenSuggestion.ExecutorCrFund.GisGkhGuid
                    }
                };
                if (citizenSuggestion.Deadline.HasValue)
                {
                    IGisGkhRegionalService gisGkhRegionalService = Container.Resolve<IGisGkhRegionalService>();
                    try
                    {
                        appealAction.Performer.DateOfAppointment = citizenSuggestion.Deadline.Value;
                        appealAction.Performer.DateOfAppointmentSpecified = true;
                    }
                    catch
                    {
                        // нет контрольного срока
                    }
                    finally
                    {
                        Container.Release(gisGkhRegionalService);
                    }
                }
                var request = AppealsServiceAsync.importAnswerReq(new importAnswerRequestAppealAction[] { appealAction });
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                req.Answer = "Запрос на назначение проверяющего и контрольного срока сформирован";
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void SaveAnswerRequest(GisGkhRequests req, CitizenSuggestion citizenSuggestion, string OrgPPAGUID)
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
                log += $"{DateTime.Now} Формирование запроса на выгрузку в ГИС ЖКХ ответа по обращению ФКР {citizenSuggestion.Number} от " +
                    $"{citizenSuggestion.CreationDate.ToShortDateString()}\r\n";
                citizenSuggestion.GisGkhTransportGuid = Guid.NewGuid().ToString();
                citizenSuggestion.GisGkhAnswerTransportGuid = Guid.NewGuid().ToString();
                CitizenSuggestionDomain.Update(citizenSuggestion);
                object request = null;
                XmlNsPrefixer prefixer = new XmlNsPrefixer();
                XmlDocument document = null;

                List<importAnswerRequestAnswerActionAnswer> gisAnswers = new List<importAnswerRequestAnswerActionAnswer>();
                citizenSuggestion.GisGkhAnswerTransportGuid = Guid.NewGuid().ToString();
                importAnswerRequestAnswerActionAnswerLoadAnswer loadAnswer = new importAnswerRequestAnswerActionAnswerLoadAnswer
                {
                    AnswerText = citizenSuggestion.AnswerText
                };
                var answeFiles = CitizenSuggestionFilesDomain.GetAll().Where(x => x.CitizenSuggestion.Id == citizenSuggestion.Id && x.isAnswer).ToList();
                // Приложения к ответу
                List<AttachmentType> attachmentsList = new List<AttachmentType>();
                foreach (var answer in answeFiles)
                {
                    if (string.IsNullOrEmpty(answer.GisGkhGuid))
                    {
                        var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.inspection, answer.DocumentFile, OrgPPAGUID);

                        if (uploadResult.Success)
                        {
                            answer.GisGkhGuid = uploadResult.FileGuid;
                            CitizenSuggestionFilesDomain.Update(answer);
                        }
                        else
                        {
                            throw new Exception(uploadResult.Message);
                        }
                    }
                    attachmentsList.Add(new AttachmentType
                    {
                        Attachment = new Attachment
                        {
                            AttachmentGUID = answer.GisGkhGuid
                        },
                        Description = answer.DocumentFile.FullName,
                        Name = answer.DocumentFile.FullName,
                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(answer.DocumentFile))

                    });
                }
                if (attachmentsList.Count > 0)
                {
                    loadAnswer.Attachment = attachmentsList.ToArray();
                }
                importAnswerRequestAnswerActionAnswer gisAnswer = new importAnswerRequestAnswerActionAnswer
                {
                    TransportGUID = citizenSuggestion.GisGkhAnswerTransportGuid,
                    AnswererGUID = citizenSuggestion.ExecutorCrFund.GisGkhGuid,
                    ItemsElementName = new ItemsChoiceType6[]
                    {
                                    ItemsChoiceType6.LoadAnswer,
                        //ItemsChoiceType6.SendAnswer
                    },
                    Items = new object[]
                    {
                                    loadAnswer,
                        //false // отправлять ответ или нет
                    }
                };
                gisAnswers.Add(gisAnswer);
                importAnswerRequestAnswerAction answerAction = new importAnswerRequestAnswerAction
                {
                    TransportGUID = citizenSuggestion.GisGkhTransportGuid,
                    ItemElementName = ItemChoiceType1.AppealGUID,
                    Item = citizenSuggestion.GisGkhGuid,
                    Answer = gisAnswers.ToArray(),
                };
                request = AppealsServiceAsync.importAnswerReq(null, new importAnswerRequestAnswerAction[] { answerAction });
                req.Answer = $"*{citizenSuggestion.Number})* - запрос на выгрузку ответа сформирован";
                log += $"{DateTime.Now} Запрос на выгрузку ответа сформирован\r\n";
                document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
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
                var response = AppealsServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer += ". Ответ получен";
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
                            req.Answer += ". Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            SaveLog(ref req, ref log);
                        }
                        else
                        {
                            req.Answer += $". Задача на обработку ответа importAnswerCR поставлена в очередь с id {taskInfo.TaskId}";
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer += ". Запрос ещё в очереди";
                    log += $"{DateTime.Now} Запрос ещё в очереди\r\n";
                    SaveLog(ref req, ref log);
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer += $". {e.Message}";
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
                            else if (responseItem is CommonResultType)
                            {
                                var gisResult = (CommonResultType)responseItem;
                                CitizenSuggestion citizenSuggestion = CitizenSuggestionDomain.GetAll()
                                        .Where(x => x.GisGkhGuid == gisResult.GUID).FirstOrDefault();
                                if (citizenSuggestion != null)
                                {
                                    citizenSuggestion.GisGkhTransportGuid = null;
                                    CitizenSuggestionDomain.Update(citizenSuggestion);
                                    log += $"{DateTime.Now} Запрос по обращению {citizenSuggestion.Number} от " +
                                        $"{citizenSuggestion.CreationDate.ToShortDateString()} успешно выполнен\r\n";
                                }
                                else
                                {
                                    citizenSuggestion = CitizenSuggestionDomain.GetAll()
                                        .Where(x => x.GisGkhAnswerTransportGuid == gisResult.TransportGUID).FirstOrDefault();
                                    if (citizenSuggestion != null)
                                    {
                                        citizenSuggestion.GisGkhAnswerGuid = gisResult.GUID;
                                        citizenSuggestion.GisGkhAnswerTransportGuid = null;
                                        CitizenSuggestionDomain.Update(citizenSuggestion);
                                        log += $"{DateTime.Now} Запрос по обращению {citizenSuggestion.Number} от " +
                                            $"{citizenSuggestion.CreationDate.ToShortDateString()} успешно выполнен\r\n";
                                    }
                                }
                                req.Answer += ". Данные из ГИС ЖКХ обработаны";
                                req.RequestState = GisGkhRequestState.ResponseProcessed;
                            }
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