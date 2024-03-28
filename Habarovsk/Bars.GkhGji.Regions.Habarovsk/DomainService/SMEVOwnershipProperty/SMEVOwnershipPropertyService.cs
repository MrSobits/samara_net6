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
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVOwnershipProperty;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using System.Xml;
using Bars.GkhGji.Regions.Habarovsk.SGIO.Ownership;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVOwnershipPropertyService : ISMEVOwnershipPropertyService
    {
        #region Constants

        static XNamespace tns = @"urn://ru.sgio.information-provision/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVOwnershipProperty> SMEVOwnershipPropertyDomain { get; set; }

        public IDomainService<SMEVOwnershipPropertyFile> SMEVOwnershipPropertyFileDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVOwnershipPropertyService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendRequest(SMEVOwnershipProperty requestData, IProgressIndicator indicator = null)
        {
            try
            {
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVOwnershipPropertyFileDomain.GetAll().Where(x => x.SMEVOwnershipProperty == requestData).ToList().ForEach(x => SMEVOwnershipPropertyFileDomain.Delete(x.Id));

                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVOwnershipPropertyDomain.Update(requestData);

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
                    SMEVOwnershipPropertyDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVOwnershipProperty requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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

                //_SMEV3Service.GetAckAsyncSGIO(response.MessageId, true).GetAwaiter().GetResult();

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

                    var OwResponse = response.MessagePrimaryContent.Element(tns + "Excerpt");
                    if (OwResponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }
                 
                    XDocument doc = new XDocument(OwResponse);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.OmitXmlDeclaration = true;
                        xws.Indent = true;

                        using (XmlWriter xw = XmlWriter.Create(ms, xws))
                        {
                            doc.WriteTo(xw);
                        }

                        requestData.AnswerFile = _fileManager.SaveFile(ms, "Response.xml");
                    }

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVOwnershipPropertyDomain.Update(requestData);
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
        private XElement GetInformationRequestXML(SMEVOwnershipProperty requestData)
        {
            List<object> adresses = new List<object>();
            List<object> cadastral = new List<object>();
            List<ItemsChoiceType> items = new List<ItemsChoiceType>();
            if (requestData.QueryType == Enums.QueryTypeType.AddressQuery)
            {
                adresses.Add(new QueryAddressType
                {
                    Build = requestData.RealityObject.FiasAddress.Building,
                    City = requestData.RealityObject.FiasAddress.PlaceAddressName,
                    District = requestData.RealityObject.FiasAddress.PlaceAddressName,
                    Region = "20701000",
                    Locality = requestData.RealityObject.FiasAddress.PlaceName,
                    Flat = requestData.Room != null? requestData.Room.RoomNum:"",
                    House = requestData.RealityObject.FiasAddress.House,
                    Street = requestData.RealityObject.FiasAddress.StreetName
                });
                items.Add(ItemsChoiceType.Address);
            }
            if (requestData.QueryType == Enums.QueryTypeType.CadasterNumberQuery)
            {
                cadastral.Add(requestData.CadasterNumber);
                items.Add(ItemsChoiceType.CadasterNumber);
            }
                //формируем из класса
            RequestType requestType = new RequestType
            {
                DeclarantINN = "3664032439",
                DeclarantName = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ",
                DeclarantOGRN = "1033600084968",
                IncomingDate = DateTime.Now,
                IncomingDateSpecified = true,
                IncomingNumber = requestData.Id.ToString(),
                PropertyType = requestData.Room != null ? RequestTypePropertyType.Premise : RequestTypePropertyType.Building,
                PublicPropertyLevel = requestData.PublicPropertyLevel == PublicPropertyLevel.Mun ? RequestTypePublicPropertyLevel.M : RequestTypePublicPropertyLevel.S,
                ReceiverOKTMO = requestData.PublicPropertyLevel == PublicPropertyLevel.Subj ? "20000000001" : requestData.Municipality.Oktmo,
                Query = new QueryType
                {
                    type = requestData.QueryType == Enums.QueryTypeType.AddressQuery ? SGIO.Ownership.QueryTypeType.AddressQuery :
                    requestData.QueryType == Enums.QueryTypeType.CadasterNumberQuery ? SGIO.Ownership.QueryTypeType.CadasterNumberQuery:SGIO.Ownership.QueryTypeType.RegisterNumberQuery,
                    typeSpecified = true,
                    ItemsElementName = items.ToArray(),
                    Items = requestData.QueryType == Enums.QueryTypeType.AddressQuery ? adresses.ToArray(): cadastral.ToArray()
                }
            };

            var result = GetRequestElement(requestType);

            //var result = new XElement(tns + "Request",
            //    new XElement("DeclarantName", "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"),
            //    new XElement("DeclarantINN", "3664032439"),
            //    new XElement("DeclarantOGRN", "1033600084968"),
            //    new XElement("IncomingNumber", requestData.Id.ToString()),
            //    new XElement("IncomingDate", DateTime.Now.ToString("yyyy-MM-dd")),
            //     new XElement("ReceiverOKTMO", requestData.PublicPropertyLevel == PublicPropertyLevel.Subj? "20000000001" : requestData.Municipality.Oktmo),
            //                new XElement("PropertyType", requestData.Room != null? "Premise": "Building"),
            //                new XElement("PublicPropertyLevel",  requestData.PublicPropertyLevel == PublicPropertyLevel.Mun ? "M" : "S"),
            //                new XElement("Query",
            //                requestData.QueryType == QueryTypeType.AddressQuery ?
            //                    new XAttribute("type", "AddressQuery")
            //                    :
            //                    requestData.QueryType == QueryTypeType.CadasterNumberQuery ?
            //                    new XAttribute("type", "CadasterNumberQuery")
            //                    :
            //                    new XAttribute("type", "RegisterNumberQuery"),
            //                  new XElement("RequestCopyingNote", "Запрос из системы АИС ГЖИ ВО"),
            //                    requestData.QueryType == QueryTypeType.AddressQuery ?
            //                        new XElement("Address",
            //                            new XElement("Region", "20701000"),
            //                            new XElement("District", requestData.RealityObject.FiasAddress.PlaceAddressName),
            //                            new XElement("City", requestData.RealityObject.FiasAddress.PlaceAddressName),
            //                            new XElement("Locality", requestData.RealityObject.FiasAddress.PlaceName),
            //                            new XElement("Street", requestData.RealityObject.FiasAddress.StreetName),
            //                            new XElement("House", requestData.RealityObject.FiasAddress.House),
            //                            new XElement("Build", requestData.RealityObject.FiasAddress.Building),
            //                            new XElement("Flat", requestData.Room.RoomNum)
            //                            )
            //                        :
            //                        requestData.QueryType == QueryTypeType.CadasterNumberQuery ?
            //                        new XElement("CadasterNumber", requestData.CadasterNumber)
            //                        :
            //                        new XElement("RegisterNumber", requestData.RegisterNumber)
            //                )
            //            );

            //result.SetAttributeValue(XNamespace.Xmlns + "tns", tns);
            return result;
        }

        private void ChangeState(SMEVOwnershipProperty requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVOwnershipPropertyDomain.Update(requestData);
        }

        private void SetErrorState(SMEVOwnershipProperty requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVOwnershipPropertyDomain.Update(requestData);
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

        private void SaveFile(SMEVOwnershipProperty request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVOwnershipPropertyFileDomain.Save(new SMEVOwnershipPropertyFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVOwnershipProperty = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVOwnershipProperty request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVOwnershipPropertyFileDomain.Save(new SMEVOwnershipPropertyFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVOwnershipProperty = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
            if (fileName.Contains("ID"))
            {
                request.AttachmentFile = _fileManager.SaveFile(fileName, data);
                SMEVOwnershipPropertyDomain.Update(request);
            }
        }

        private void SaveFile(SMEVOwnershipProperty request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVOwnershipPropertyFileDomain.Save(new SMEVOwnershipPropertyFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVOwnershipProperty = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVOwnershipProperty request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVOwnershipPropertyFileDomain.Save(new SMEVOwnershipPropertyFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVOwnershipProperty = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
