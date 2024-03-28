using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using System.Linq;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System.Net.Http;
using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System.Xml;

    using Fasterflect;

    public class SMEVEGRULService : ISMEVEGRULService
    {
        #region Constants

        static XNamespace ns1Namespace = @"urn://x-artefacts-fns-vipul-tosmv-ru/311-14/4.0.6";
        static XNamespace fnstNamespace = @"urn://x-artefacts-fns/vipul-types/4.0.6";

        #endregion

        #region Properties

        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }

        public IDomainService<SMEVEGRULFile> SMEVEGRULFileDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }


        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVEGRULService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendInformationRequest(SMEVEGRUL requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVEGRULFileDomain.GetAll().Where(x => x.SMEVEGRUL == requestData).ToList().ForEach(x => SMEVEGRULFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVEGRULDomain.Update(requestData);

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
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
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
                    SMEVEGRULDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVEGRUL requestData, GetResponseResponse response, IProgressIndicator indicator = null)
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
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в приаттаченом файле AsyncProcessingStatus.xml");
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

                    var FNSresponse = response.MessagePrimaryContent.Element(ns1Namespace + "FNSVipULResponse");
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные в ЕГРЮЛ отсутствуют");

                        // Cтатус контрагента - если такого ЮЛ нету, но в контрагентах он будет - считаем удалённым из ЕГРЮЛ
                        ContragentState contragentStatus = ContragentState.Delisted;
                        ChangeContragentStatus(requestData.INNReq, contragentStatus);
                        return false;
                    }

                    switch (FNSresponse.Element(ns1Namespace + "КодОбр")?.Value)
                    {
                        case "53":
                            SetErrorState(requestData, "Cведения в отношении юридического лица не могут быть предоставлены в электронном виде");
                            return false;
                        case "54":
                            requestData.RequestState = RequestState.ResponseReceived;
                            requestData.Answer = "Данные содержатся в приложенном ZIP-архиве";
                            SMEVEGRULDomain.Update(requestData);
                            return true;
                    }

                    var SvULElement = FNSresponse.Element(ns1Namespace + "СвЮЛ");
                    if (SvULElement == null)
                    {
                        SetErrorState(requestData, "Секция СвЮЛ отсутствует");
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
                    SMEVEGRULDomain.Update(requestData);

                    //обновление Адреса у контрагента
                    Contragent contragent = ContragentRepository.GetAll().Where(x => x.Inn == requestData.INN).FirstOrDefault();
                    if (contragent != null)
                    {
                        try
                        {
                            string log = "";
                            if (contragent.Name.ToLower() != requestData.Name.ToLower())
                            {
                                log += $"Изменено наименование контрагента c {contragent.Name} на {requestData.Name}";
                            }
                            if (contragent.JuridicalAddress.ToLower() != requestData.AddressUL.ToLower())
                            {
                                log += $"Изменен юр. адрес контрагента c {contragent.JuridicalAddress} на {requestData.AddressUL}";
                            }
                            EmailSender emailSender = EmailSender.Instance;
                            emailSender.Send("ov_popova@govvrn.ru", $"Уведомление о изменении данных у контрагента {contragent.Name}", log, null);
                        }
                        catch { }
                        contragent.DateRegistration = requestData.OGRNDate;
                        contragent.Ogrn = requestData.OGRN;
                        contragent.Kpp = requestData.KPP;
                        contragent.Name = requestData.Name;
                        contragent.ShortName = requestData.ShortName;
                        contragent.JuridicalAddress = requestData.AddressUL;
                        contragent.Okved = requestData.OKVEDCodes;
                        contragent.EgrulExcDate = requestData.ResponceDate;
                        contragent.OgrnRegistration = requestData.RegOrgName;
                        ContragentRepository.Update(contragent);

                    }
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

        private void ProcessResponseXML(SMEVEGRUL requestData, XElement data)
        {
            // Cтатус контрагента
            ContragentState contragentStatus = ContragentState.Active;

            //var ulInfos = data.Elements(ns1Namespace + "СвЗапЕГРЮЛ");
            //List<string> codes = new List<string>();
            //foreach (var ulInfo in ulInfos)
            //{

            //}



            // Cначала проверяем в сведениях о ЮЛ, есть ли банкротство с назначением конкурсного управляющего (14103) или решение о ликвидации (14101-14102)
            //var ulInfoCode = data.Element(ns1Namespace + "СвЗапЕГРЮЛ")?.Element(ns1Namespace + "ВидЗап")?.Attribute("КодСПВЗ")?.Value;
            //if (ulInfoCode.Contains("14103"))
            //{
            //    contragentStatus = ContragentState.Bankrupt;
            //}
            //else if (ulInfoCode.Contains("14101") || ulInfoCode.Contains("14102"))
            //{
            //    contragentStatus = ContragentState.LiquidationSoon;
            //}
            //else
            //{
            //    contragentStatus = ContragentState.Active;
            //}
            var statuses = data.Elements(ns1Namespace + "СвСтатус");

            //Дальше проверяем коды по СЮЛСТ в разделе "Статус".
            //Т.к. кодов может быть несколько одновременно, статус, требующий большего внимания, перезаписывает предыдущий.
            //Статусы по рейтингу:

            //Active
            //ChangeAddress
            //MinimiseAutorizedCapital
            /////////////Bankrupt
            /////////////LiquidationSoon
            //Reorganized
            //LiquidationProcess
            //DelistingSoon
            //NotValidRegistration
            //Liquidated

            if (statuses != null && statuses.Any())
            {
                foreach (var status in statuses)
                {
                    if ((status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "701") ||
                       (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "702") ||
                       (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "801"))
                    {
                        contragentStatus = ContragentState.NotValidRegistration;
                    }
                    else if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "105")
                    {
                        contragentStatus = ContragentState.DelistingSoon;
                    }
                    else if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "101")
                    {
                        contragentStatus = ContragentState.LiquidationProcess;
                    }
                    else if (Enumerable.Range(121, 199).Contains(int.Parse(status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value)))
                    {
                        contragentStatus = ContragentState.Reorganized;
                    }
                    //else if (contragentStatus == ContragentState.Active)
                    //{
                    //    if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "111")
                    //    {
                    //        contragentStatus = ContragentState.MinimiseAutorizedCapital;
                    //    }
                    //    else if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "112")
                    //    {
                    //        contragentStatus = ContragentState.ChangeAddress;
                    //    }
                    //}
                    else if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "111")
                    {
                        contragentStatus = ContragentState.MinimiseAutorizedCapital;
                    }
                    else if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "112")
                    {
                        contragentStatus = ContragentState.ChangeAddress;
                    }
                    else if (status.Element(ns1Namespace + "СвСтатус")?.Attribute("КодСтатусЮЛ")?.Value == "117")
                    {
                        contragentStatus = ContragentState.Bankrupt;
                    }

                }
            }

            var end = data.Element(ns1Namespace + "СвПрекрЮЛ");
            if (end != null)
            {
                contragentStatus = ContragentState.Liquidated;
            }

            var ogrn = data.Attribute("ОГРН")?.Value;
            //Меняем состояние контрагента
            if (ogrn != null)
            {
                ChangeContragentStatus(ogrn, contragentStatus);
            }
            requestData.StateNameUL = ((DisplayAttribute)contragentStatus.GetType().GetMember(contragentStatus.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).First()).Value;

            requestData.OPFName = data.Attribute("ПолнНаимОПФ")?.Value;
            requestData.INN = data.Attribute("ИНН")?.Value;
            requestData.KPP = data.Attribute("КПП")?.Value;
            requestData.OGRN = ogrn;

            requestData.OGRNDate = NullableDateParse(data.Attribute("ДатаОГРН")?.Value);
            requestData.ResponceDate = NullableDateParse(data.Attribute("ДатаВып")?.Value);

            //наименование
            var names = data.Element(ns1Namespace + "СвНаимЮЛ");
            if (names != null)
            {
                requestData.Name = names.Attribute("НаимЮЛПолн")?.Value;
                requestData.ShortName = names.Attribute("НаимЮЛСокр")?.Value;
            }

            //адрес
            var address = data.Element(ns1Namespace + "СвАдресЮЛ")?.Element(ns1Namespace + "АдресРФ");
            if (address != null)
            {
                requestData.AddressUL = MakeAddress(address);

                requestData.Name = names.Attribute("НаимЮЛПолн")?.Value;
                requestData.ShortName = names.Attribute("НаимЮЛСокр")?.Value;
            }

            //СвОбрЮЛ
            requestData.CreateWayName = data.Element(ns1Namespace + "СвОбрЮЛ")?.Element(ns1Namespace + "СпОбрЮЛ")?.Attribute("НаимСпОбрЮЛ")?.Value;

            var registration = data.Element(ns1Namespace + "СвРегОрг");
            if (registration != null)
            {
                requestData.RegOrgName = registration.Attribute("НаимНО")?.Value;
                requestData.AddressRegOrg = registration.Attribute("АдрРО")?.Value;
                requestData.CodeRegOrg = registration.Attribute("КодНО")?.Value;
            }



            var capital = data.Element(ns1Namespace + "СвУстКап");
            if (capital != null)
            {
                requestData.AuthorizedCapitalAmmount = NullableDecimalParse(capital.Attribute("СумКап")?.Value);
                requestData.AuthorizedCapitalType = capital.Attribute("НаимВидКап")?.Value;
            }

            var post = data.Element(ns1Namespace + "СведДолжнФЛ")?.Element(ns1Namespace + "СвДолжн");
            if (post != null)
            {
                requestData.TypePozitionName = post.Attribute("НаимВидДолжн")?.Value;
                requestData.Pozition = post.Attribute("НаимДолжн")?.Value;
            }

            var fio = data.Element(ns1Namespace + "СведДолжнФЛ")?.Element(ns1Namespace + "СвФЛ");
            if (fio != null)
            {
                requestData.FIO = fio.Attribute("Фамилия")?.Value + " " +
                                  fio.Attribute("Имя")?.Value + " " +
                                  fio.Attribute("Отчество")?.Value;
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
            string towntype = address.Element(fnstNamespace + "Город")?.Attribute("ТипГород")?.Value;
            string townname = address.Element(fnstNamespace + "Город")?.Attribute("НаимГород")?.Value;
            string nptype = address.Element(fnstNamespace + "НаселПункт")?.Attribute("ТипНаселПункт")?.Value;
            string npname = address.Element(fnstNamespace + "НаселПункт")?.Attribute("НаимНаселПункт")?.Value;
            string streettype = address.Element(fnstNamespace + "Улица")?.Attribute("ТипУлица")?.Value;
            string streetname = address.Element(fnstNamespace + "Улица")?.Attribute("НаимУлица")?.Value;

            string house = address.Attribute("Дом")?.Value;
            string korpus = address.Attribute("Корпус")?.Value;
            string pom = address.Attribute("Кварт")?.Value;

            string result = string.Empty;
            if (!string.IsNullOrEmpty(regiontype))
            {
                result += $"{regiontype} {regionname}";
            }
            if (!string.IsNullOrEmpty(towntype))
            {
                result += $", {towntype} {townname}";
            }
            if (!string.IsNullOrEmpty(nptype))
            {
                result += $", {nptype} {npname}";
            }
            if (!string.IsNullOrEmpty(streettype))
            {
                result += $", {streettype} {streetname}";
            }
            if (!string.IsNullOrEmpty(house))
            {
                result += $", {house}";
            }
            if (!string.IsNullOrEmpty(korpus))
            {
                result += $", к. {korpus}";
            }
            if (!string.IsNullOrEmpty(pom))
            {
                result += $", {pom}";
            }

            return result;
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

        private XElement GetInformationRequestXML(SMEVEGRUL requestData)
        {
            string messageId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();

            var result = new XElement(ns1Namespace + "FNSVipULRequest",
                new XAttribute("ИдДок", messageId),
                new XAttribute("НомерДела", "БН"),
                new XElement(ns1Namespace + "ЗапросЮЛ",
                    GetIdentifier(requestData)
                    )
            );

            result.SetAttributeValue(XNamespace.Xmlns + "ns1", ns1Namespace);
            return result;
        }

        private XElement GetIdentifier(SMEVEGRUL requestData)
        {
            if (requestData.InnOgrn == InnOgrn.INN)
                return new XElement(ns1Namespace + "ИННЮЛ", requestData.INNReq);
            else
                return new XElement(ns1Namespace + "ОГРН", requestData.INNReq);
        }

        private void ChangeState(SMEVEGRUL requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVEGRULDomain.Update(requestData);
        }

        private void SetErrorState(SMEVEGRUL requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVEGRULDomain.Update(requestData);
        }

        private void SaveFile(SMEVEGRUL request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVEGRULFileDomain.Save(new SMEVEGRULFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRUL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVEGRUL request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVEGRULFileDomain.Save(new SMEVEGRULFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRUL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVEGRUL request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVEGRULFileDomain.Save(new SMEVEGRULFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRUL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVEGRUL request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVEGRULFileDomain.Save(new SMEVEGRULFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRUL = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeContragentStatus(string ogrn, ContragentState status)
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
                                ContragentRepository.Update(contragent);
                            }
                        }
                        catch (Exception e) { }
                    }
                }
            }
            catch { }
        }
        #endregion
    }
}
