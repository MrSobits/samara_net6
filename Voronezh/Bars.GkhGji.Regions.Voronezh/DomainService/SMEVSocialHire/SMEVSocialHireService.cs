using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.GkhGji.Regions.Voronezh.SGIO.SMEVSocialHire;
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
    public class SMEVSocialHireService : ISMEVSocialHireService
    {
        #region Constants

        static XNamespace sec = @"urn://ru.sgio.hiringcontract/1.0.0";
        static XNamespace com = @"urn://ru.sgio.hiringcontract/commons/1.0.0";

        #endregion

        #region Properties

        public IDomainService<SMEVSocialHire> SMEVSocialHireDomain { get; set; }

        public IDomainService<SMEVSocialHireFile> SMEVSocialHireFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVSocialHireService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVSocialHire requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVSocialHireFileDomain.GetAll().Where(x => x.SMEVSocialHire == requestData).ToList().ForEach(x => SMEVSocialHireFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVSocialHireDomain.Update(requestData);

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
                    SMEVSocialHireDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVSocialHire requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
                response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                //_SMEV3Service.GetAckAsyncSGIO(response.MessageId, true).GetAwaiter().GetResult();

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

                    var SGIOresponse = response.MessagePrimaryContent.Element(sec + "SECResponse");
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

                    ProcessResponseXML(requestData, SGIOresponse);

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVSocialHireDomain.Update(requestData);
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

        private void ProcessResponseXML(SMEVSocialHire requestData, XElement data)
        {

            var objDesc = data.Element(sec + "ObjectDescription");
            requestData.ContractNumber = objDesc.Element(sec + "ContractNumber")?.Value;
            requestData.ContractType = objDesc.Element(sec + "Name")?.Value == "Social" ? SocialHireContractType.Social : SocialHireContractType.Commercial ;
            requestData.Name = objDesc.Element(sec + "Name")?.Value;
            requestData.Purpose = objDesc.Element(sec + "Purpose")?.Value;
            requestData.TotalArea = objDesc.Element(sec + "TotalArea")?.Value;
            requestData.LiveArea = objDesc.Element(sec + "LiveArea")?.Value;

            var objAddress = data.Element(sec + "ObjectAddress");
            requestData.AnswerRegion = objAddress.Element(sec + "Region")?.Value;
            requestData.AnswerDistrict = objAddress.Element(sec + "District")?.Value;
            requestData.AnswerCity = objAddress.Element(sec + "City")?.Value;
            requestData.AnswerStreet = objAddress.Element(sec + "Street")?.Value;
            requestData.AnswerHouse = objAddress.Element(sec + "House")?.Value;
            requestData.AnswerFlat = objAddress.Element(sec + "Flat")?.Value;

            var employer = data.Element(com + "Employers").Element(sec + "Employer");
            requestData.LastName = employer.Element(sec + "LastName")?.Value;
            requestData.FirstName = employer.Element(sec + "FirstName")?.Value;
            requestData.MiddleName = employer.Element(sec + "MiddleName")?.Value;
            requestData.Birthday = NullableDateParse(employer.Element(sec + "Birthday")?.Value);
            requestData.Birthplace = employer.Element(sec + "Birthplace")?.Value;
            requestData.Citizenship = employer.Element(sec + "Citizenship")?.Value;
            requestData.DocumentType = EnumParse(employer.Element(sec + "DocumentType")?.Value);
            requestData.DocumentNumber = employer.Element(sec + "DocumentNumber")?.Value;
            requestData.DocumentSeries = employer.Element(sec + "DocumentSeries")?.Value;
            requestData.DocumentDate = NullableDateParse(employer.Element(sec + "DocumentDate")?.Value);
            requestData.IsContractOwner = employer.Element(sec + "Is_contract_owner")?.Value == "true" ? YesNo.Yes : YesNo.No;
        }

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        private SocialHireDocType EnumParse(string value)
        {
            int e = int.TryParse(value, out int result) ? result : 0;
            if (Enum.IsDefined(typeof(SocialHireDocType), value)) 
            {
                return (SocialHireDocType)e;
            }

            return 0;
        }
        #endregion

        #region Private methods

        private XElement GetInformationRequestXML(SMEVSocialHire requestData)
        {
            SECRequestType request = new SECRequestType
            {
                RegionCode = "20701000",
                District = requestData.RealityObject.FiasAddress.PlaceAddressName,
                City = requestData.RealityObject.FiasAddress.PlaceAddressName,
                Locality = requestData.RealityObject.FiasAddress.PlaceName,
                Street = requestData.RealityObject.FiasAddress.StreetName,
                House = requestData.RealityObject.FiasAddress.House,
                Building = string.IsNullOrEmpty(requestData.RealityObject.FiasAddress.Building)? "-": requestData.RealityObject.FiasAddress.Building,
                Flat = requestData.Room.RoomNum,
                Structure = "-",
                ReceiverOKTMO = requestData.Municipality.Oktmo
            };
            var result = GetRequestElement(request);
            //XElement result = new XElement(sec + "SECRequest",
            //    new XElement(com + "RegionCode", "20701000"),
            //    new XElement(com + "District", requestData.RealityObject.FiasAddress.PlaceAddressName),
            //    new XElement(com + "City", requestData.RealityObject.FiasAddress.PlaceAddressName),
            //    new XElement(com + "Locality", requestData.RealityObject.FiasAddress.PlaceName),
            //    new XElement(com + "Street", requestData.RealityObject.FiasAddress.StreetName),
            //    new XElement(com + "House", requestData.RealityObject.FiasAddress.House),
            //    new XElement(com + "Building", requestData.RealityObject.FiasAddress.Building),
            //    new XElement(com + "Flat", requestData.Room.RoomNum),
            //    new XElement(com + "ReceiverOKTMO", requestData.Municipality.Oktmo)
            //    );

            //result.SetAttributeValue(XNamespace.Xmlns + "sec", sec);
            //result.SetAttributeValue(XNamespace.Xmlns + "com", com);
            return result;
        }

        private XElement GetRequestElement(SECRequestType request)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(SECRequestType));
                    xmlSerializer.Serialize(streamWriter, request);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private void ChangeState(SMEVSocialHire requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVSocialHireDomain.Update(requestData);
        }

        private void SetErrorState(SMEVSocialHire requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVSocialHireDomain.Update(requestData);
        }

        private void SaveFile(SMEVSocialHire request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVSocialHireFileDomain.Save(new SMEVSocialHireFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSocialHire = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVSocialHire request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVSocialHireFileDomain.Save(new SMEVSocialHireFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSocialHire = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVSocialHire request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVSocialHireFileDomain.Save(new SMEVSocialHireFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSocialHire = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVSocialHire request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVSocialHireFileDomain.Save(new SMEVSocialHireFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVSocialHire = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
