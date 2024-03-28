using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using Bars.GkhGji.Regions.Habarovsk.SMEVNDFLProxy;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Namespaces;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using SMEV3Library.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVNDFLService : ISMEVNDFLService
    {
        #region Constants

        static XNamespace ns1Namespace = @"urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1";
        static XNamespace env = @"http://schemas.xmlsoap.org/soap/envelope/";

        #endregion

        #region Properties

        public IDomainService<SMEVNDFL> SMEVNDFLDomain { get; set; }

        public IDomainService<SMEVNDFLFile> SMEVNDFLFileDomain { get; set; }

        public IDomainService<SMEVNDFLAnswer> SMEVNDFLAnswerDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public SMEVNDFLService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        public string GetXML(long reqId)
        {
            var data = SMEVNDFLDomain.Get(reqId);
            XElement request = GetInformationRequestXML(data);
            return request.ToString();
        }

        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVNDFL requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVNDFLFileDomain.GetAll().Where(x => x.SMEVNDFL == requestData).ToList().ForEach(x => SMEVNDFLFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsyncPersonalSig(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVNDFLDomain.Update(requestData);

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
                    SMEVNDFLDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVNDFL requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {

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
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

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
                    var FNSresponse = response.MessagePrimaryContent.Element(ns1Namespace + "NDFL2Response");
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                    if (FNSresponse.Element(ns1Namespace + "КодОбраб")?.Value == "01")
                    {
                        SetErrorState(requestData, "Запрашиваемые сведения не найдены");
                        return false;
                    }

                    NDFL2Response ndflResponse = DeSerializerOrder(FNSresponse);
                    ProcessResponseXML(requestData, ndflResponse);

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVNDFLDomain.Update(requestData);
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

        private NDFL2Response DeSerializerOrder(XElement element)
        {
            StringReader reader = new StringReader(element.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(NDFL2Response));
            return ((NDFL2Response)xmlSerializer.Deserialize(reader));
        }

        private void ProcessResponseXML(SMEVNDFL requestData, NDFL2Response data)
        {
            var prevList = SMEVNDFLAnswerDomain.GetAll()
                .Where(x => x.SMEVNDFL.Id == requestData.Id)
                .ToList();

            if (prevList.Count > 0)
            {
                prevList.ForEach(x => SMEVNDFLAnswerDomain.Delete(x.Id));
            }

            SMEVNDFL a = SMEVNDFLDomain.GetAll().Select(x => x).OrderByDescending(x => x.Id).FirstOrDefault();
            var listToSave = new List<SMEVNDFLAnswer>();
            foreach (var rec in data.ДохФЛ.ДохФЛ_НА)
            { 
                var INNUL = rec.СвНА.СвНАЮЛ.ИННЮЛ.ToString();
                var KPP = rec.СвНА.СвНАЮЛ.КПП.ToString();
                var OrgName = rec.СвНА.СвНАЮЛ.НаимОрг.ToString();
                var Rate = rec.СведДох.Ставка;
                var DutyBase = rec.СведДох.СГДНалПер.НалБаза;
                var DutySum = rec.СведДох.СГДНалПер.НалИсчисл;
                var UnretentionSum = rec.СведДох.СГДНалПер.НалНеУдерж;
                var RevenueTotalSum = rec.СведДох.СГДНалПер.СумДохОбщ;

                foreach (var doh in rec.СведДох.ДохВыч)
                {
                    var answer = new SMEVNDFLAnswer
                    {
                        SMEVNDFL = a,
                        INNUL = INNUL,
                        KPP = KPP,
                        OrgName = OrgName,
                        Rate = Rate,
                        DutyBase = DutyBase,
                        DutySum = DutySum,
                        UnretentionSum = UnretentionSum,
                        RevenueTotalSum = RevenueTotalSum,
                        RevenueCode = doh.КодДоход.ToString(),
                        Month = doh.Месяц.ToString(),
                        RevenueSum = doh.СумДоход
                    };

                    SMEVNDFLAnswerDomain.Save(answer);
                }
            }
        }

        private decimal NullableDecimalParse(string value)
        {
            if (value == null)
                return 0;

            decimal result;

            return (decimal.TryParse(value, out result) ? result : 0);
        }

        private int NullableIntParse(string value)
        {
            if (value == null)
                return 0;

            int result;

            return (int.TryParse(value, out result) ? result : 0);
        }

        #endregion

        #region Private methods

        private string GetTimeStampUuid()
        {
            return GUIDHelper.GenerateTimeBasedGuid().ToString();
        }

        private XElement GetInformationRequestXML(SMEVNDFL requestData)
        {
            XElement signElement = XElement.Parse(Properties.Resources.signtemplate);

          

            var result = new XElement(ns1Namespace + "NDFL2Request",
                new XAttribute("Id", "PERSONAL_SIGNATURE"),
                new XAttribute("ОтчетГод", requestData.PeriodYear),
                new XAttribute("ТипЗапросП", "2"),
           //     new XAttribute("КодУслуги", requestData.ServiceCode),
                new XAttribute("ИдЗапрос", requestData.RequestId),
                new XElement(ns1Namespace + "СвФЛ",
                    new XAttribute("ДатаРожд", requestData.BirthDate.ToString("yyyy-MM-dd")),
                    string.IsNullOrEmpty(requestData.SNILS)? null: new XAttribute("СНИЛС", requestData.SNILS),
                    new XAttribute("НомЗаявФЛ", requestData.RegNumber),
                    new XAttribute("ДатаЗаявФЛ", requestData.RegDate.ToString("yyyy-MM-dd")),
                        new XElement(ns1Namespace + "ФИОФЛ",
                        new XAttribute("FirstName", requestData.FirstName),
                        new XAttribute("Patronymic", requestData.Patronymic),
                        new XAttribute("FamilyName", requestData.FamilyName)
                        ),
                        new XElement(ns1Namespace + "УдЛичн",
                        new XAttribute("DocumentCode", requestData.DocumentCode.Code),
                        new XAttribute("SeriesNumber", requestData.SeriesNumber)
                        )
                    )
            );

            return result;
            //   result.SetAttributeValue(XNamespace.Xmlns + "tns", ns1Namespace);

            //  var senderProvidedRequestDataXElement = new XElement(env + "Envelope", new XAttribute(XNamespace.Xmlns + "env", "http://schemas.xmlsoap.org/soap/envelope/"), result, signElement);

            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces12.TypesNamespace + "SenderProvidedRequestData",
            new XAttribute("Id", "SIGNED_BY_CONSUMER"),
            new XElement(SMEVNamespaces12.TypesNamespace + "MessageID", GetTimeStampUuid()),
            new XElement(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent", result),
            new XElement(SMEVNamespaces12.TypesNamespace + "PersonalSignature", signElement)
        );
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);
            return senderProvidedRequestDataXElement;
        }

        private void ChangeState(SMEVNDFL requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVNDFLDomain.Update(requestData);
        }

        private void SetErrorState(SMEVNDFL requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVNDFLDomain.Update(requestData);
        }

        private void SaveFile(SMEVNDFL request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVNDFLFileDomain.Save(new SMEVNDFLFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVNDFL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVNDFL request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVNDFLFileDomain.Save(new SMEVNDFLFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVNDFL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVNDFL request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVNDFLFileDomain.Save(new SMEVNDFLFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVNDFL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVNDFL request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVNDFLFileDomain.Save(new SMEVNDFLFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVNDFL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
