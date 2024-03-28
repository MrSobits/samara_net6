using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class SMEVMVDService : ISMEVMVDService
    {
        #region Constants

        static XNamespace nsNamespace = @"urn://ru/mvd/ibd-m/convictions/search/1.0.2";
        static XNamespace xsiNamespace = @"http://www.w3.org/2001/XMLSchema-instance";

        //static XNamespace commonNamespace = @"urn://x-artefacts-mvd-gov-ru/wcs-epgu-criminal-record/types/1.1.2";
        //static XNamespace smevNamespace = @"urn://x-artefacts-smev-gov-ru/supplementary/commons/1.2";


        #endregion

        #region Properties

        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }

        public IDomainService<SMEVMVDFile> SMEVMVDFileDomain { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVMVDService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запросана на получение справки о судимости
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVMVD requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVMVDFileDomain.GetAll().Where(x => x.SMEVMVD == requestData).ToList().ForEach(x => SMEVMVDFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVMVDDomain.Update(requestData);

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
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ, подробности в файле Fault.xml");
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
                    SMEVMVDDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVMVD requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.FullMessageElement, "SmevMessage.xml");
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
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ, подробности в приаттаченом файле AsyncProcessingStatus.xml");
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
                                                                                                                                                                                      

                    var resp = response.MessagePrimaryContent.Element(nsNamespace + "response");
                    if (response == null)
                    {
                        SetErrorState(requestData, "Секция сведений из МВД отсутствует");
                        return false;
                    }
                   
                    if (resp.Element(nsNamespace + "noRecords") != null)
                    {
                        requestData.Answer = "Сведения о судимости отсутствуют";
                        requestData.AnswerInfo = "Сведения о судимости отсутствуют";
                    }
                    else if (resp.Element(nsNamespace + "records") != null)
                    {
                        var rec = resp.Element(nsNamespace + "records");

                        requestData.Answer = "Имеется информация о судимостях";

                        requestData.AnswerInfo = rec.Element(nsNamespace + "additionalInfo").Value;
                    }
                    else
                    {
                        SetErrorState(requestData, "Пустой ответ от МВД");
                        return false;
                    }
                    requestData.RequestState = RequestState.ResponseReceived;
                    SMEVMVDDomain.Update(requestData);
                    
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
                // использовать, если надо будет показывать конкретные судимости
        private void ProcessResponseXML(SMEVMVD requestData, XElement data) 
        {

            // Ответ
            requestData.Answer = "Имеется информация о судимостях";
            // и дальше парсить


            


            //try
            //{
            //    requestData.OGRNDate = Convert.ToDateTime(data.Attribute("ДатаОГРНИП")?.Value);
            //}
            //catch
            //{ }
            //  var svfl = data.Element(ns1Namespace + "СвФЛ");
            //var names = data.Element(ns1Namespace + "СвФЛ")?.Element(ns1Namespace + "ФИОРус"); //ФИОРус находится внутри СвФЛ, исправить
            //if (names != null)
            //{
            //    requestData.FIO = names.Attribute("Фамилия")?.Value + " " + names.Attribute("Имя")?.Value + " " + names.Attribute("Отчество")?.Value;
            //}

            //requestData.ResponceDate = NullableDateParse(data.Attribute("ДатаВып")?.Value);


            ////адрес
            //var address = data.Element(ns1Namespace + "СвАдрМЖ")?.Element(ns1Namespace + "АдресРФ");
            //if (address != null)
            //{
            //    // requestData.RegionType = MakeAddress(address);

            //    requestData.RegionName = address.Element(fnstNamespace + "Регион")?.Attribute("НаимРегион")?.Value;
            //    requestData.RegionType = address.Element(fnstNamespace + "Регион")?.Attribute("ТипРегион")?.Value;
            //    requestData.CityName = address.Element(fnstNamespace + "Город")?.Attribute("НаимГород")?.Value;
            //    requestData.CityType = address.Element(fnstNamespace + "Город")?.Attribute("ТипГород")?.Value;
            //}

            ////СвОбрЮЛ
            //requestData.CreateWayName = data.Element(ns1Namespace + "СвЗапЕГРИП")?.Element(ns1Namespace + "ВидЗап")?.Attribute("НаимВидЗап")?.Value;

            //var registration = data.Element(ns1Namespace + "СвРегОрг");
            //if (registration != null)
            //{
            //    requestData.RegOrgName = registration.Attribute("НаимНО")?.Value;
            //    requestData.AddressRegOrg = registration.Attribute("АдрРО")?.Value;
            //    requestData.CodeRegOrg = registration.Attribute("КодНО")?.Value;

            //}



            ////ОКВЭД
            //var okveds = data.Element(ns1Namespace + "СвОКВЭД")?.Elements(ns1Namespace + "СвОКВЭДОсн");
            //if (okveds != null && okveds.Count() > 0)
            //{
            //    foreach (var okved in okveds)
            //    {
            //        requestData.OKVEDNames += okved.Attribute("НаимОКВЭД")?.Value + "; ";
            //        requestData.OKVEDCodes += okved.Attribute("КодОКВЭД")?.Value + "; ";
            //    }
            //}
            //var okvedsdop = data.Element(ns1Namespace + "СвОКВЭД")?.Elements(ns1Namespace + "СвОКВЭДДоп");
            //if (okvedsdop != null && okvedsdop.Count() > 0)
            //{
            //    foreach (var okveddop in okvedsdop)
            //    {
            //        requestData.OKVEDNames += okveddop.Attribute("НаимОКВЭД")?.Value + "; ";
            //        requestData.OKVEDCodes += okveddop.Attribute("КодОКВЭД")?.Value + "; ";
            //    }
            //}
        }



        //private decimal NullableDecimalParse(string value)
        //{
        //    if (value == null)
        //        return 0;

        //    decimal result;

        //    return (decimal.TryParse(value, out result) ? result : 0);
        //}

        //private string MakeAddress(XElement address)
        //{
        //    string regiontype = address.Element(fnstNamespace + "Регион")?.Attribute("ТипРегион")?.Value;
        //    string regionname = address.Element(fnstNamespace + "Регион")?.Attribute("НаимРегион")?.Value;
        //    string streettype = address.Element(fnstNamespace + "Улица")?.Attribute("ТипУлица")?.Value;
        //    string streetname = address.Element(fnstNamespace + "Улица")?.Attribute("НаимУлица")?.Value;

        //    string house = address.Attribute("Дом")?.Value;
        //    string korpus = address.Attribute("Корпус")?.Value;

        //    return $"{regiontype} {regionname}, {streettype} {streetname}, д. {house}, к. {korpus}";
        //}

        //private DateTime? NullableDateParse(string value)
        //{
        //    if (value == null)
        //        return null;

        //    DateTime result;

        //    return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        //}

        #endregion

        #region Private methods

        //private string CancelAllowedTranslate(string str)
        //{
        //    string result = null;
        //    switch (str)
        //    {
        //        case "false":
        //            return result = "нет";
        //        case "true":
        //            return result = "да";
        //    }
        //    return result;
        //}

        private XElement GetInformationRequestXML(SMEVMVD requestData)
        {
            string messageId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();

            var result =
                new XElement(nsNamespace + "request",
                new XAttribute("Id", $"ID_{ requestData.Id }"),               
                    new XElement(nsNamespace + "birthDate",
                        new XElement(nsNamespace + "year", requestData.BirthDate.Year),
                        new XElement(nsNamespace + "month", requestData.BirthDate.Month),
                        new XElement(nsNamespace + "day", requestData.BirthDate.Day)
                    ),
                    !string.IsNullOrEmpty(requestData.SNILS) ? new XElement(nsNamespace + "SNILS", requestData.SNILS) : null,
                    new XElement(nsNamespace + "surname", requestData.Surname),
                    new XElement(nsNamespace + "name", requestData.Name),
                    !string.IsNullOrEmpty(requestData.PatronymicName) ? new XElement(nsNamespace + "patronymicName", requestData.PatronymicName) : null,
                    new XElement(nsNamespace + "registrationPlace",
                        new XElement(nsNamespace + "type", GetTypeAddress(requestData.MVDTypeAddressPrimary)),
                        new XElement(nsNamespace + "regionCode", requestData.RegionCodePrimary.Code),
                        new XElement(nsNamespace + "place", requestData.AddressPrimary)
                    )
                );

            if (requestData.AddressAdditional != null && requestData.AddressAdditional != "")
            {
                result.Add(new XElement(nsNamespace + "registrationPlace",
                        new XElement(nsNamespace + "type", GetTypeAddress(requestData.MVDTypeAddressAdditional)),
                        new XElement(nsNamespace + "regionCode", requestData.RegionCodeAdditional.Code),
                        new XElement(nsNamespace + "place", requestData.AddressAdditional)
                    ));
            }

            result.SetAttributeValue(XNamespace.Xmlns + "ns", nsNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "xsi", xsiNamespace);
            return result;
        }

        private string GetTypeAddress(MVDTypeAddress en)
        {
            switch (en)
            {
                case MVDTypeAddress.BirthPlace:
                    return "000";
                case MVDTypeAddress.LivingPlace:
                    return "200";
                case MVDTypeAddress.FactPlace:
                    return "201";
            }
            return "";
        }
       
        //private XElement GetIdentifier(SMEVMVD requestData)
        //{
        //    if (requestData.InnOgrn == InnOgrn.INN)
        //        return new XElement(ns1Namespace + "ИНН", requestData.INNReq);
        //    else
        //        return new XElement(ns1Namespace + "ОГРНИП", requestData.INNReq);
        //}

        private void ChangeState(SMEVMVD requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVMVDDomain.Update(requestData);
        }

        private void SetErrorState(SMEVMVD requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVMVDDomain.Update(requestData);
        }

        private void SaveFile(SMEVMVD request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVMVDFileDomain.Save(new SMEVMVDFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVMVD = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVMVD request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVMVDFileDomain.Save(new SMEVMVDFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVMVD = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVMVD request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVMVDFileDomain.Save(new SMEVMVDFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVMVD = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVMVD request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVMVDFileDomain.Save(new SMEVMVDFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVMVD = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
