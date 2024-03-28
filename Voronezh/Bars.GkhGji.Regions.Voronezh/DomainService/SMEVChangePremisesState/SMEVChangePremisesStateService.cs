using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.GkhGji.Regions.Voronezh.SGIO.SMEVChangePremisesState;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class SMEVChangePremisesStateService : ISMEVChangePremisesStateService
    {
        #region Constants

        static XNamespace mcp = @"urn://ru.sgio.buildingnotice/1.0.0";
        static XNamespace com = @"urn://ru.sgio.buildingnotice/commons/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVChangePremisesState> SMEVChangePremisesStateDomain { get; set; }

        public IDomainService<SMEVChangePremisesStateFile> SMEVChangePremisesStateFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVChangePremisesStateService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(SMEVChangePremisesState requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVChangePremisesStateFileDomain.GetAll().Where(x => x.SMEVChangePremisesState == requestData).ToList().ForEach(x => SMEVChangePremisesStateFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVChangePremisesStateDomain.Update(requestData);

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
                    SMEVChangePremisesStateDomain.Update(requestData);
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
                SetErrorState(requestData, "SendRequest exception: " + e.Message);
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
        public bool TryProcessResponse(SMEVChangePremisesState requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
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

                //ACK - ставим вдумчиво
              //  _SMEV3Service.GetAckAsyncSGIO(response.MessageId, true).GetAwaiter().GetResult();

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

                    var SGIOresponse = response.MessagePrimaryContent.Element(mcp + "MstrConversionPremisesResponse");
                    if (SGIOresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                    //if (SGIOresponse.Element(com + "ResponseInfo")?.Element(com + "Status")?.Value != "1075")
                    //{
                    //    SetErrorState(requestData, "Запрашиваемые сведения не найдены");
                    //    return false;
                    //}

                    var notice = SGIOresponse.Element(mcp + "Notice");
                    if (notice == null)
                    {
                        SetErrorState(requestData, "Секция Notice отсутствует");
                        return false;
                    }

                    ProcessResponseXML(requestData, notice);

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVChangePremisesStateDomain.Update(requestData);
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

        private void ProcessResponseXML(SMEVChangePremisesState requestData, XElement data)
        {

            var declarant = data.Element(mcp + "Declarant");
            requestData.DeclarantType = declarant.Element(mcp + "Type")?.Value == "1" ? OwnerType.Individual : OwnerType.JurPerson;
            requestData.DeclarantName = declarant.Element(mcp + "Name")?.Value;
            requestData.DeclarantAddress = declarant.Element(mcp + "Address")?.Value;
            requestData.Department = data.Element(mcp + "Department")?.Value;

            var objectInfo = data.Element(mcp + "ObjectInfo");
            requestData.Area = objectInfo.Element(mcp + "Area")?.Value;
            requestData.City = objectInfo.Element(mcp + "Address")?.Element(com + "City")?.Value;
            requestData.Street = objectInfo.Element(mcp + "Address")?.Element(com + "Street")?.Value;
            requestData.House = objectInfo.Element(mcp + "Address")?.Element(com + "House")?.Value;
            requestData.Block = objectInfo.Element(mcp + "Address")?.Element(com + "Block")?.Value;
            requestData.Apartment = objectInfo.Element(mcp + "Address")?.Element(com + "Apartment")?.Value;
            requestData.RoomType = objectInfo.Element(mcp + "Type")?.Value == "1" ? RoomType.Living : RoomType.NonLiving;
            requestData.Appointment = objectInfo.Element(mcp + "Appointment")?.Value;

            var conclusion = data.Element(mcp + "Conclusion");
            requestData.ActNumber = conclusion.Element(mcp + "ActNumber")?.Value;
            requestData.ActName = conclusion.Element(mcp + "ActName")?.Value;
            requestData.ActDate = NullableDateParse(conclusion.Element(com + "Date")?.Value);
            requestData.OldPremisesType = conclusion.Element(mcp + "OldPremisesType")?.Value == "1" ? RoomType.Living : RoomType.NonLiving;
            requestData.NewPremisesType = conclusion.Element(mcp + "NewPremisesType")?.Value == "1" ? RoomType.Living : RoomType.NonLiving;
            requestData.ConditionTransfer = conclusion.Element(mcp + "ConditionTransfer")?.Value;

            var responsible = data.Element(mcp + "Responsible");
            requestData.ResponsibleName = responsible.Element(mcp + "Name")?.Value;
            requestData.ResponsiblePost = responsible.Element(mcp + "Post")?.Value;
            requestData.ResponsibleDate = NullableDateParse(responsible.Element(mcp + "Date")?.Value);
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

        private XElement GetInformationRequestXML(SMEVChangePremisesState requestData)
        {
            if (requestData.ChangePremisesType == ChangePremisesType.Address)
            {
                MstrConversionPremisesRequestType request = new MstrConversionPremisesRequestType
                {
                    RegionCode = "20701000",
                    ActDate = requestData.ActDate.HasValue? requestData.ActDate.Value.ToString("dd.MM.yyyy"):" ",
                    ActName = "Акт",
                    ActNumber = "б/н",
                    ActDepartment = "Орган местного самоуравления " + requestData.Municipality.Name,
                    CadastralNumber = "-",
                    Address = new MstrConversionPremisesRequestAddressType
                    {
                      PostalCode = requestData.RealityObject.FiasAddress.PostCode,
                      Region = requestData.RealityObject.FiasAddress.AddressName,
                      District = requestData.RealityObject.FiasAddress.PlaceAddressName,
                      Locality = requestData.RealityObject.FiasAddress.PlaceName,
                      Street = requestData.RealityObject.FiasAddress.StreetName,
                      House = requestData.RealityObject.FiasAddress.House,
                      Building = !string.IsNullOrEmpty(requestData.RealityObject.FiasAddress.Building)? requestData.RealityObject.FiasAddress.Building : "-",
                      Apartment = requestData.Room.RoomNum
                    },
                    ReceiverOKTMO = requestData.Municipality.Oktmo
                };
                //
                var result = GetRequestElement(request);
                //result = new XElement(mcp + "MstrConversionPremisesRequest",
                //    new XElement(com + "RegionCode", "20701000"),
                //    new XElement(com + "Address",
                //        new XElement(com + "PostalCode", requestData.RealityObject.FiasAddress.PostCode),
                //        new XElement(com + "Region", requestData.RealityObject.FiasAddress.AddressName),
                //        new XElement(com + "District", requestData.RealityObject.FiasAddress.PlaceAddressName),
                //        new XElement(com + "Locality", requestData.RealityObject.FiasAddress.PlaceName),
                //        new XElement(com + "Street", requestData.RealityObject.FiasAddress.StreetName),
                //        new XElement(com + "House", requestData.RealityObject.FiasAddress.House),
                //        new XElement(com + "Building", requestData.RealityObject.FiasAddress.Building),
                //        new XElement(com + "Apartment", requestData.Room.RoomNum)
                //        ),
                //    new XElement(com + "ReceiverOKTMO", requestData.Municipality.Oktmo)
                //    );
                return result;
            }
            else
            {
                MstrConversionPremisesRequestType request = new MstrConversionPremisesRequestType
                {
                    RegionCode = "20701000",
                    ActDate = requestData.ActDate.HasValue ? requestData.ActDate.Value.ToString("dd.MM.yyyy") : " ",
                    ActName = "Акт",
                    ActNumber = "б/н",
                    ActDepartment = "Орган местного самоуравления " + requestData.Municipality.Name,
                    CadastralNumber = requestData.CadastralNumber,
                    ReceiverOKTMO = requestData.Municipality.Oktmo
                };
                var result = GetRequestElement(request);
                //result = new XElement(mcp + "MstrConversionPremisesRequest",
                //    new XElement(com + "RegionCode", "20701000"),
                //    new XElement(com + "CadastralNumber", requestData.CadastralNumber),
                //    new XElement(com + "ReceiverOKTMO", requestData.Municipality.Oktmo)
                //    );
                return result;
            }
          
        }

        private XElement GetRequestElement(MstrConversionPremisesRequestType request)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(MstrConversionPremisesRequestType));
                    xmlSerializer.Serialize(streamWriter, request);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }


        private void ChangeState(SMEVChangePremisesState requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVChangePremisesStateDomain.Update(requestData);
        }

        private void SetErrorState(SMEVChangePremisesState requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVChangePremisesStateDomain.Update(requestData);
        }

        private void SaveFile(SMEVChangePremisesState request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVChangePremisesStateFileDomain.Save(new SMEVChangePremisesStateFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVChangePremisesState = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVChangePremisesState request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVChangePremisesStateFileDomain.Save(new SMEVChangePremisesStateFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVChangePremisesState = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
            request.AnswerFile = _fileManager.SaveFile(fileName, data);
            SMEVChangePremisesStateDomain.Update(request);
        }

        private void SaveFile(SMEVChangePremisesState request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVChangePremisesStateFileDomain.Save(new SMEVChangePremisesStateFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVChangePremisesState = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVChangePremisesState request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVChangePremisesStateFileDomain.Save(new SMEVChangePremisesStateFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVChangePremisesState = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
