using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Utils;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVEmergencyHouse;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using System.Xml;
using System.Xml.Serialization;
using Bars.GkhGji.Regions.Habarovsk.SGIO.Emergency;
using System.Text;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVEmergencyHouseService : ISMEVEmergencyHouseService
    {
        #region Constants

        static XNamespace tns = @"urn://ru.sgio.acceptregcitizen/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVEmergencyHouse> SMEVEmergencyHouseDomain { get; set; }

        public IDomainService<SMEVEmergencyHouseFile> SMEVEmergencyHouseFileDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVEmergencyHouseService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendRequest(SMEVEmergencyHouse requestData, IProgressIndicator indicator = null)
        {
            try
            {
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVEmergencyHouseFileDomain.GetAll().Where(x => x.SMEVEmergencyHouse == requestData).ToList().ForEach(x => SMEVEmergencyHouseFileDomain.Delete(x.Id));

                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVEmergencyHouseDomain.Update(requestData);

                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVEmergencyHouseDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
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
        public bool TryProcessResponse(SMEVEmergencyHouse requestData, SMEV3Library.Entities.GetResponseResponse.GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {

                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");

                if (response.Attachments != null)
                {
                    response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));
                }

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

               // _SMEV3Service.GetAckAsyncSGIO(response.MessageId, true).GetAwaiter().GetResult();

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
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                }
                else
                {
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");

                    var Response = response.MessagePrimaryContent.Element(tns + "response");
                    ResponseType resp = new ResponseType();
                    try
                    {
                        resp = DeSerializerResponce(Response);
                    }
                    catch (Exception e)
                    {
                    }
                    if (Response == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                
                    //XDocument doc = new XDocument(Response);
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    XmlWriterSettings xws = new XmlWriterSettings();
                    //    xws.OmitXmlDeclaration = true;
                    //    xws.Indent = true;

                    //    using (XmlWriter xw = XmlWriter.Create(ms, xws))
                    //    {
                    //        doc.WriteTo(xw);
                    //    }

                    //    requestData.AnswerFile = _fileManager.SaveFile(ms, "Response.xml");
                    //}

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVEmergencyHouseDomain.Update(requestData);
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

        private ResponseType DeSerializerResponce(XElement element)
        {
            StringReader reader = new StringReader(element.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ResponseType));
            return ((ResponseType)xmlSerializer.Deserialize(reader));
        }
        private XElement GetInformationRequestXML(SMEVEmergencyHouse requestData)
        {
            if (requestData.Room != null)
            {
                RequestType request = new RequestType
                {
                    Address = requestData.RealityObject.Municipality.Name + ", " + requestData.RealityObject.Address,
                    ReceiverOKTMO = requestData.Municipality.Oktmo,
                    Room = requestData.Room != null? requestData.Room.RoomNum:"не указан"
                };
                var result = GetRequestElement(request);
                return result;
            }
            else
            {
                RequestType request = new RequestType
                {
                    Address = requestData.RealityObject.Municipality.Name + ", " + requestData.RealityObject.Address,
                    ReceiverOKTMO = requestData.Municipality.Oktmo
                };
                var result = GetRequestElement(request);
                return result;
            }
       

            //var result = new XElement(tns + "request",
            //                new XElement("Address", requestData.RealityObject.Municipality.Name + ", " + requestData.RealityObject.Address),
            //                new XElement("ReceiverOKTMO", requestData.Municipality.Oktmo)
            //                );

            //result.SetAttributeValue(XNamespace.Xmlns + "tns", tns);
      
        }

        private XElement GetRequestElement(RequestType request)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(RequestType));
                    xmlSerializer.Serialize(streamWriter, request);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private void ChangeState(SMEVEmergencyHouse requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVEmergencyHouseDomain.Update(requestData);
        }

        private void SetErrorState(SMEVEmergencyHouse requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVEmergencyHouseDomain.Update(requestData);
        }

        private void SaveFile(SMEVEmergencyHouse request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVEmergencyHouseFileDomain.Save(new SMEVEmergencyHouseFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEmergencyHouse = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVEmergencyHouse request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVEmergencyHouseFileDomain.Save(new SMEVEmergencyHouseFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEmergencyHouse = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
            request.AnswerFile = _fileManager.SaveFile(fileName, data);
            SMEVEmergencyHouseDomain.Update(request);
        }

        private void SaveFile(SMEVEmergencyHouse request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVEmergencyHouseFileDomain.Save(new SMEVEmergencyHouseFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEmergencyHouse = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVEmergencyHouse request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVEmergencyHouseFileDomain.Save(new SMEVEmergencyHouseFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEmergencyHouse = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
