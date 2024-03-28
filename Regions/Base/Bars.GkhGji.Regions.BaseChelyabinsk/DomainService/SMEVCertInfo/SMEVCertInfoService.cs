using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.ConfigSections;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;
using SMEV3Library.Entities;
using Bars.B4.Modules.Tasks.Common.Utils;
using Bars.B4.Modules.FIAS;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Bars.B4.Config;
using Bars.B4.Application;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    public class SMEVCertInfoService : ISMEVCertInfoService
    {
        #region Constants

        static XNamespace com = @"http://roskazna.ru/gisgmp/xsd/Common/2.2.0";
        static XNamespace ic = @"urn://roskazna.ru/gisgmp/xsd/services/import-certificates/2.2.0";

        #endregion

        #region Properties

        public IDomainService<SMEVCertInfo> SMEVCertInfoDomain { get; set; }
        public IDomainService<SMEVCertInfoFile> SMEVCertInfoFileDomain { get; set; }
        public IWindsorContainer Container { get; set; }

        public IConfigProvider ConfigProvider { get; set; }
        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public SMEVCertInfoService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса выписки ЕГРН
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVCertInfo requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                SMEVCertInfoFileDomain.GetAll().Where(x => x.SMEVCertInfo == requestData).ToList().ForEach(x => SMEVCertInfoFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                requestData.Request_ID = $"I_{Guid.NewGuid()}";
                requestData.Entry_ID = $"I_{Guid.NewGuid()}";
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var attachment = GetSignedAttachment(requestData);
                var requestResult = _SMEV3Service.SendRequestAsync(request, new List<FileAttachment> { new FileAttachment { FileData = attachment, FileName = requestData.FileInfo.FullName, FileGuid = requestData.Entry_ID } }, true).GetAwaiter().GetResult();
                //requestData.MessageId = requestResult.MessageId;
                SMEVCertInfoDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                //
                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVCertInfoDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                //ошибки связи прокидываем в контроллер
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        public byte[] GetSignedAttachment(SMEVCertInfo requestData)
        {
            var path = this.ConfigProvider.GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty);
            var filesDirectory = new DirectoryInfo(Path.IsPathRooted(path) ? path : ApplicationContext.Current.MapPath("~/" + path.TrimStart('~', '/')));
            var newFilePath = Path.Combine(
                filesDirectory.FullName,
                requestData.FileInfo.ObjectCreateDate.Year.ToString(),
                requestData.FileInfo.ObjectCreateDate.Month.ToString(),
                string.Format("{0}.{1}", requestData.FileInfo.Id, requestData.FileInfo.Extention));

            return _SMEV3Service.SignFileDetached2012256(newFilePath);
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessResponse(SMEVCertInfo requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
                response.Attachments.ForEach(x => SaveAttachmentFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    SaveFile(requestData, response.RequestRejected, "RequestRejected.xml");
                    SetErrorState(requestData, "Сервер отклонил запрос, подробности в приаттаченом файле RequestRejected.xml");
                }
                else if (response.RequestStatus != null)
                {
                    var status = response.RequestStatus.Element(ic + "description")?.Value;
                    ChangeStateAnswer(requestData, RequestState.Queued, status);
                }
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                }
                else
                {
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");

                    var importCertificateResponse = response.MessagePrimaryContent.Element(ic + "ImportCertificateResponse");
                    if (importCertificateResponse == null)
                    {
                        SetErrorState(requestData, "Пустой ответ");
                        return false;
                    }

                    var importProtocol = importCertificateResponse.Element(ic + "ImportProtocol");
                    requestData.Answer = importProtocol?.Attribute("description")?.Value ?? "Секция ImportProtocol отсутствует";

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVCertInfoDomain.Update(requestData);
                    return true;
                }
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        private decimal NullableDecimalParse(string value)
        {
            if (value == null)
                return 0;

            decimal result;

            return (decimal.TryParse(value, out result) ? result : 0);
        }

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        #endregion

        #region Private methods

        private string GetOriginatorFromConfig()
        {
            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Chelyabinsk");
            var originatorId = config.GetAs("OriginatorId", (string)null, true);
            if (string.IsNullOrEmpty(originatorId))
                throw new ApplicationException("Не найден OriginatorID в конфиге модуля Bars.GkhGji.Regions.Chelyabinsk");
            return originatorId;
        }

        private XElement GetInformationRequestXML(SMEVCertInfo requestData)
        {
            var byteFile = _fileManager.GetFile(requestData.FileInfo).ReadAllBytes();
            X509Certificate2 x509Cert = new X509Certificate2(byteFile);
       
            var result = new XElement(ic + "ImportCertificateRequest",
                                new XAttribute("Id", requestData.Request_ID),
                                new XAttribute("timestamp", DateTime.Now.ToString("o")),
                                new XAttribute("senderIdentifier", GetOriginatorFromConfig()),
                                new XAttribute("senderRole", "3"),
                            new XElement(ic + "RequestEntry",
                                new XAttribute("Id", requestData.Entry_ID),
                                new XAttribute("ownership", GetOriginatorFromConfig())
                            )
                         );

            result.SetAttributeValue(XNamespace.Xmlns + "ic", ic);
            result.SetAttributeValue(XNamespace.Xmlns + "com", com);
            return result;
        }

        private void ChangeState(SMEVCertInfo requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVCertInfoDomain.Update(requestData);
        }

        private void ChangeStateAnswer(SMEVCertInfo requestData, RequestState state, string answer)
        {
            requestData.RequestState = state;
            requestData.Answer = answer;
            SMEVCertInfoDomain.Update(requestData);
        }

        private void SetErrorState(SMEVCertInfo requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVCertInfoDomain.Update(requestData);
        }

        private void SaveFile(SMEVCertInfo request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVCertInfo = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVCertInfo request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет

            SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVCertInfo = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveAttachmentFile(SMEVCertInfo request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            try
            {
                SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVCertInfo = request,
                    SMEVFileType = SMEVFileType.ResponseAttachment,
                    FileInfo = _fileManager.SaveFile(fileName, data)
                });
            }
            catch
            {
                try
                {
                    SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1,
                        SMEVCertInfo = request,
                        SMEVFileType = SMEVFileType.ResponseAttachment,
                        FileInfo = _fileManager.SaveFile("Answer.zip", data)
                    });
                }
                catch
                {
                    throw new Exception($"некорректное имя файла ответа {fileName}");
                }
            }
        }

        private void SaveAttachment(SMEVCertInfo request, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVCertInfo = request,
                SMEVFileType = SMEVFileType.ResponseAttachment,
                FileInfo = _fileManager.SaveFile(fileName.Split('\\').Last(), File.ReadAllBytes(fileName))
            });
        }

        private void SaveFile(SMEVCertInfo request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVCertInfo = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVCertInfo request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVCertInfoFileDomain.Save(new SMEVCertInfoFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVCertInfo = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
