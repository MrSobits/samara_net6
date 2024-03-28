using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;

using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.MVDPassportProxy;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Fasterflect;

    public class MVDPassportService : IMVDPassportService
    {

        #region Properties
        public IDomainService<MVDPassport> MVDPassportDomain { get; set; }

        public IDomainService<MVDPassportFile> MVDPassportFileDomain { get; set; }

        #endregion

        #region Fields
        private ISMEV3Service _SMEV3Service;
        private IFileManager _fileManager;
        #endregion

        #region Constructors
        public MVDPassportService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Отправка запроса выписки ЕГРИП
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(MVDPassport requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                MVDPassportFileDomain.GetAll().Where(x => x.MVDPassport == requestData).ToList().ForEach(x => MVDPassportFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetPassportRequest(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                MVDPassportDomain.Update(requestData);

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
                    MVDPassportDomain.Update(requestData);
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

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessResponse(MVDPassport requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");

                //сохраняем все файлы, которые прислал сервер
                if(response.Attachments != null)
                response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                //_SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }

                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }

                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    SaveFile(requestData, response.RequestRejected, "RequestRejected.xml");
                    SetErrorState(requestData, "Сервер отклонил запрос, подробности в приаттаченом файле RequestRejected.xml");
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
                    XElement newElement = XElement.Parse(response.MessagePrimaryContent.FirstNode.ToString());
                    XDocument doc = new XDocument(newElement);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.OmitXmlDeclaration = true;
                        xws.Indent = true;

                        using (XmlWriter xw = XmlWriter.Create(ms, xws))
                        {
                            doc.WriteTo(xw);
                        }
                        requestData.FileInfo = _fileManager.SaveFile(ms, "ExtractXml.xml");
                    }
                    passportDossierResponse newResponce = Deserialize<passportDossierResponse>(XElement.Parse(response.MessagePrimaryContent.FirstNode.ToString()));
                    requestData.Answer = "Данные получены";
                    requestData.AnswerInfo = response.MessagePrimaryContent.FirstNode.ToString();
                    requestData.RequestState = RequestState.ResponseReceived;
                    MVDPassportDomain.Update(requestData);
                    try
                    {
                        string answer = "";
                        if (newResponce != null && newResponce.person != null)
                        {
                            answer += $"Информация о ФЛ: <br>ФИО: {newResponce.person.lastName} {newResponce.person.firstName} {newResponce.person.middleName} <br> Дата и место рождения: {newResponce.person.birthDate.ToString("dd.MM.yyyy")}";
                        }
                        if (newResponce != null && newResponce.passportDossier != null)
                        {
                            answer += !string.IsNullOrEmpty(answer)?"<br>Информация о паспортах РФ:": "Информация о паспортах РФ:";
                            foreach (var pass in newResponce.passportDossier.ToList())
                            {
                                if (pass.passport != null)
                                answer += $"<br>серия {pass.passport.series} номер {pass.passport.number} выдан {pass.passport.issuerName} ({pass.passport.issuerCode}) {pass.passport.issueDate.ToString("dd.MM.yyyy")}";
                            }
                        }
                        requestData.AnswerInfo = answer;
                        MVDPassportDomain.Update(requestData);
                    }
                    catch
                    { }
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

        #endregion

        #region Private methods

        private void ChangeState(MVDPassport requestData, RequestState state)
        {
            requestData.RequestState = state;
            MVDPassportDomain.Update(requestData);
        }

        private void SetErrorState(MVDPassport requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            MVDPassportDomain.Update(requestData);
        }

        private void SaveFile(MVDPassport request, Stream data, string fileName)
        {
            //сохраняем ошибку
            MVDPassportFileDomain.Save(new MVDPassportFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDPassport = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(MVDPassport request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            MVDPassportFileDomain.Save(new MVDPassportFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDPassport = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(MVDPassport request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            MVDPassportFileDomain.Save(new MVDPassportFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDPassport = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(MVDPassport request, Exception exception)
        {
            if (exception == null)
                return;

            MVDPassportFileDomain.Save(new MVDPassportFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDPassport = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt",
                    ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private XElement GetPassportRequest(MVDPassport req)
        {
            passportDossierRequest request = new passportDossierRequest();
            if (req.MVDPassportRequestType == MVDPassportRequestType.PersonInfo)
            {
                request.Item = new PersonInfo
                {
                    birthDate = req.BirthDate.Value,
                    birthPlace = req.BirthPlace,
                    firstName = req.Name,
                    lastName = req.Surname,
                    middleName = !string.IsNullOrEmpty(req.PatronymicName) ? req.PatronymicName : ""
                };
            }
            else
            {
                request.Item = new RussianPassportBase
                {
                   number = req.PassportNumber,
                   series = req.PassportSeries
                };
            }
            return ToXElement<passportDossierRequest>(request);
        }

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private T Deserialize<T>(XElement element)
        where T : class
        {
            XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }

        #endregion
    }
}