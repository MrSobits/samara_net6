using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;

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
    using System.Xml;

    using Bars.B4.Utils;

    using Fasterflect;

    public class SMEVEGRIPService : ISMEVEGRIPService
    {
        #region Constants
        static XNamespace ns1Namespace = @"urn://x-artefacts-fns-vipip-tosmv-ru/311-15/4.0.5";
        static XNamespace fnstNamespace = @"urn://x-artefacts-fns/vipip-types/4.0.5";
        #endregion

        #region Properties
        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }

        public IDomainService<SMEVEGRIPFile> SMEVEGRIPFileDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }
        #endregion

        #region Fields
        private ISMEV3Service _SMEV3Service;
        private IFileManager _fileManager;
        #endregion

        #region Constructors
        public SMEVEGRIPService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(SMEVEGRIP requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVEGRIPFileDomain.GetAll().Where(x => x.SMEVEGRIP == requestData).ToList().ForEach(x => SMEVEGRIPFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVEGRIPDomain.Update(requestData);

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
                    SMEVEGRIPDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVEGRIP requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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

                    var FNSresponse = response.MessagePrimaryContent.Element(ns1Namespace + "FNSVipIPResponse");
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные в ЕГРИП отсутствуют");
                        return false;
                    }

                    switch (FNSresponse.Element(ns1Namespace + "КодОбр")?.Value)
                    {
                        case "53":
                            SetErrorState(requestData,
                                "сведения в отношении индивидуального предпринимателя не могут быть предоставлены в электронном виде");
                            return false;
                        case "54":
                            requestData.RequestState = RequestState.ResponseReceived;
                            requestData.Answer = "Данные содержатся в приложенном ZIP-архиве";
                            SMEVEGRIPDomain.Update(requestData);
                            return true;
                    }

                    var SvULElement = FNSresponse.Element(ns1Namespace + "СвИП");
                    if (SvULElement == null)
                    {
                        SetErrorState(requestData, "Секция СвИП отсутствует");
                        return false;
                    }

                    ProcessResponseXML(requestData, SvULElement);

                    XDocument doc = new XDocument(FNSresponse);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.OmitXmlDeclaration = true;
                        xws.Indent = true;

                        using (XmlWriter xw = XmlWriter.Create(ms, xws))
                        {
                            doc.WriteTo(xw);
                        }
                        requestData.XmlFile = _fileManager.SaveFile(ms, "ExtractXml.xml");
                    }

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVEGRIPDomain.Update(requestData);
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

        private void ProcessResponseXML(SMEVEGRIP requestData, XElement data) //костыль - public, норм - private
        {
            // Cтатус контрагента
            ContragentState contragentStatus = ContragentState.Active;

            var end = data.Element(ns1Namespace + "СвПрекр");
            if (end != null)
            {
                contragentStatus = ContragentState.Liquidated;
            }

            var ogrnip = data.Attribute("ОГРНИП")?.Value;

            //Меняем состояние контрагента
            if (ogrnip != null)
            {
                ChangeContragentStatus(requestData, ogrnip, contragentStatus);
            }

            //НаимВидИП
            requestData.IPType = data.Attribute("НаимВидИП")?.Value;
            requestData.OGRN = ogrnip;
            try
            {
                requestData.OGRNDate = Convert.ToDateTime(data.Attribute("ДатаОГРНИП")?.Value);
            }
            catch
            {
            }

            //  var svfl = data.Element(ns1Namespace + "СвФЛ");
            var names = data.Element(ns1Namespace + "СвФЛ")?.Element(ns1Namespace + "ФИОРус"); //ФИОРус находится внутри СвФЛ, исправить
            if (names != null)
            {
                requestData.FIO = names.Attribute("Фамилия")?.Value + " " + names.Attribute("Имя")?.Value + " " + names.Attribute("Отчество")?.Value;
            }

            requestData.ResponceDate = NullableDateParse(data.Attribute("ДатаВып")?.Value);


            //адрес
            var address = data.Element(ns1Namespace + "СвАдрМЖ")?.Element(ns1Namespace + "АдресРФ");
            if (address != null)
            {
                // requestData.RegionType = MakeAddress(address);

                requestData.RegionName = address.Element(fnstNamespace + "Регион")?.Attribute("НаимРегион")?.Value;
                requestData.RegionType = address.Element(fnstNamespace + "Регион")?.Attribute("ТипРегион")?.Value;
                requestData.CityName = address.Element(fnstNamespace + "Город")?.Attribute("НаимГород")?.Value;
                requestData.CityType = address.Element(fnstNamespace + "Город")?.Attribute("ТипГород")?.Value;
            }

            //СвОбрЮЛ
            requestData.CreateWayName = data.Element(ns1Namespace + "СвЗапЕГРИП")?.Element(ns1Namespace + "ВидЗап")?.Attribute("НаимВидЗап")?.Value;

            var registration = data.Element(ns1Namespace + "СвРегОрг");
            if (registration != null)
            {
                requestData.RegOrgName = registration.Attribute("НаимНО")?.Value;
                requestData.AddressRegOrg = registration.Attribute("АдрРО")?.Value;
                requestData.CodeRegOrg = registration.Attribute("КодНО")?.Value;
            }

            //ОКВЭД
            var okveds = data.Element(ns1Namespace + "СвОКВЭД")?.Elements(ns1Namespace + "СвОКВЭДОсн");
            if (okveds != null && okveds.Count() > 0)
            {
                foreach (var okved in okveds)
                {
                    requestData.OKVEDNames += okved.Attribute("НаимОКВЭД")?.Value + "; ";
                    requestData.OKVEDCodes += okved.Attribute("КодОКВЭД")?.Value + "; ";
                }
            }

            var okvedsdop = data.Element(ns1Namespace + "СвОКВЭД")?.Elements(ns1Namespace + "СвОКВЭДДоп");
            if (okvedsdop != null && okvedsdop.Count() > 0)
            {
                foreach (var okveddop in okvedsdop)
                {
                    requestData.OKVEDNames += okveddop.Attribute("НаимОКВЭД")?.Value + "; ";
                    requestData.OKVEDCodes += okveddop.Attribute("КодОКВЭД")?.Value + "; ";
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

        private string MakeAddress(XElement address)
        {
            string regiontype = address.Element(fnstNamespace + "Регион")?.Attribute("ТипРегион")?.Value;
            string regionname = address.Element(fnstNamespace + "Регион")?.Attribute("НаимРегион")?.Value;
            string streettype = address.Element(fnstNamespace + "Улица")?.Attribute("ТипУлица")?.Value;
            string streetname = address.Element(fnstNamespace + "Улица")?.Attribute("НаимУлица")?.Value;

            string house = address.Attribute("Дом")?.Value;
            string korpus = address.Attribute("Корпус")?.Value;

            return $"{regiontype} {regionname}, {streettype} {streetname}, д. {house}, к. {korpus}";
        }

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?) null);
        }
        #endregion

        #region Private methods
        private XElement GetInformationRequestXML(SMEVEGRIP requestData)
        {
            string messageId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();

            var result = new XElement(ns1Namespace + "FNSVipIPRequest",
                new XAttribute("ИдДок", messageId),
                new XAttribute("НомерДела", "БН"),
                new XElement(ns1Namespace + "ЗапросИП",
                    GetIdentifier(requestData)
                )
            );

            result.SetAttributeValue(XNamespace.Xmlns + "ns1", ns1Namespace);
            return result;
        }

        private XElement GetIdentifier(SMEVEGRIP requestData)
        {
            if (requestData.InnOgrn == InnOgrn.INN)
                return new XElement(ns1Namespace + "ИНН", requestData.INNReq);
            else
                return new XElement(ns1Namespace + "ОГРНИП", requestData.INNReq);
        }

        private void ChangeState(SMEVEGRIP requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVEGRIPDomain.Update(requestData);
        }

        private void SetErrorState(SMEVEGRIP requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVEGRIPDomain.Update(requestData);
        }

        private void SaveFile(SMEVEGRIP request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVEGRIPFileDomain.Save(new SMEVEGRIPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRIP = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVEGRIP request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVEGRIPFileDomain.Save(new SMEVEGRIPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRIP = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVEGRIP request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVEGRIPFileDomain.Save(new SMEVEGRIPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRIP = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVEGRIP request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVEGRIPFileDomain.Save(new SMEVEGRIPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRIP = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt",
                    ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeContragentStatus(SMEVEGRIP requestData, string ogrn, ContragentState status)
        {
            try
            {
                var contragents = ContragentDomain.GetAll().Where(x => x.Ogrn == ogrn);
                if (contragents != null && contragents.Any())
                {
                    foreach (var contragent in contragents)
                    {
                        try
                        {
                            // Если был статус "Не предоставляет услуги управления" - ничего не делаем
                            if (contragent.ContragentState != ContragentState.NotManagementService)
                            {
                                contragent.ContragentState = status;
                            }
                            contragent.EgrulExcDate = requestData.ResponceDate;
                            ContragentRepository.Update(contragent);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion
    }
}