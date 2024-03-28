using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;

using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.MVDLivPlaceProxy;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using Fasterflect;

    public class MVDLivingPlaceRegistrationService : IMVDLivingPlaceRegistrationService
    {

        #region Properties
        public IDomainService<MVDLivingPlaceRegistration> MVDLivingPlaceRegistrationDomain { get; set; }

        public IDomainService<MVDLivingPlaceRegistrationFile> MVDLivingPlaceRegistrationFileDomain { get; set; }

        #endregion

        #region Fields
        private ISMEV3Service _SMEV3Service;
        private IFileManager _fileManager;
        #endregion

        #region Constructors
        public MVDLivingPlaceRegistrationService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(MVDLivingPlaceRegistration requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                MVDLivingPlaceRegistrationFileDomain.GetAll().Where(x => x.MVDLivingPlaceRegistration == requestData).ToList().ForEach(x => MVDLivingPlaceRegistrationFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetPassportRequest(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                MVDLivingPlaceRegistrationDomain.Update(requestData);

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
                    MVDLivingPlaceRegistrationDomain.Update(requestData);
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
        public bool TryProcessResponse(MVDLivingPlaceRegistration requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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
                    livingPlaceRegistrationResponse newResponce = Deserialize<livingPlaceRegistrationResponse>(XElement.Parse(response.MessagePrimaryContent.FirstNode.ToString()));
                    requestData.Answer = "Данные получены";
                    requestData.AnswerInfo = response.MessagePrimaryContent.FirstNode.ToString();
                    requestData.RequestState = RequestState.ResponseReceived;
                    MVDLivingPlaceRegistrationDomain.Update(requestData);
                    try
                    {
                        string answer = "";
                        if (newResponce.Item is NotFoundRegistration)
                        {
                            answer = "Сведения о регистрации не обнаружены";
                        }
                        else if (newResponce.Item is RegistrationInfo)
                        {
                            var data = newResponce.Item as RegistrationInfo;
                            answer += $"Адрес регистрации: {data.region}, ОКТМО {data.regionOktmo}, {data.unstructuredAddress}";
                            answer += $"<br>{data.district}, {data.settlement}, {data.street}, {data.house}, {data.flat}";
                            answer += $"<br>Дата регистрации: {data.registrationDateFrom.ToString("dd.MM.yyyy")}";
                        }
                        requestData.AnswerInfo = answer;
                        MVDLivingPlaceRegistrationDomain.Update(requestData);
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

        private void ChangeState(MVDLivingPlaceRegistration requestData, RequestState state)
        {
            requestData.RequestState = state;
            MVDLivingPlaceRegistrationDomain.Update(requestData);
        }

        private void SetErrorState(MVDLivingPlaceRegistration requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            MVDLivingPlaceRegistrationDomain.Update(requestData);
        }

        private void SaveFile(MVDLivingPlaceRegistration request, Stream data, string fileName)
        {
            //сохраняем ошибку
            MVDLivingPlaceRegistrationFileDomain.Save(new MVDLivingPlaceRegistrationFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDLivingPlaceRegistration = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(MVDLivingPlaceRegistration request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            MVDLivingPlaceRegistrationFileDomain.Save(new MVDLivingPlaceRegistrationFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDLivingPlaceRegistration = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(MVDLivingPlaceRegistration request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            MVDLivingPlaceRegistrationFileDomain.Save(new MVDLivingPlaceRegistrationFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDLivingPlaceRegistration = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(MVDLivingPlaceRegistration request, Exception exception)
        {
            if (exception == null)
                return;

            MVDLivingPlaceRegistrationFileDomain.Save(new MVDLivingPlaceRegistrationFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                MVDLivingPlaceRegistration = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt",
                    ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private XElement GetPassportRequest(MVDLivingPlaceRegistration req)
        {
            RegistrationRequest request = new RegistrationRequest
            {
                personInfo = new PersonInfo1
                {
                    birthDate = req.BirthDate.Value,
                    firstName = req.Name,
                    lastName = req.Surname,
                    middleName = req.PatronymicName
                },
                document = new DocumentInfo
                {
                    Item = new RussianPassport
                    {
                        issueDate = req.IssueDate.Value,
                        number = req.PassportNumber,
                        series = req.PassportSeries
                    }
                }
            };
       
            return ToXElement<RegistrationRequest>(request);
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