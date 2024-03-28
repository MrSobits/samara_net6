using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVFNSLicRequestService : ISMEVFNSLicRequestService
    {
        #region Constants

        static XNamespace ns1NamespaceUl = @"urn://x-artefacts-fns-licul-tofns-ru/301-06/4.0.0";
        static XNamespace ns1NamespaceIp = @"urn://x-artefacts-fns-licip-tofns-ru/301-07/4.0.0";
        static XNamespace ns1;
        static string xName;

        #endregion

        #region Properties

        public IDomainService<SMEVFNSLicRequest> SMEVFNSLicRequestDomain { get; set; }

        public IDomainService<SMEVFNSLicRequestFile> SMEVFNSLicRequestFileDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVFNSLicRequestService(IFileManager fileManager, ISMEV3Service SMEV3Service)
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
        public bool SendRequest(SMEVFNSLicRequest requestData, IProgressIndicator indicator = null)
        {
            if (requestData.FNSLicPersonType == FNSLicPersonType.IP)
            {
                xName = "FNSLicIPRequest";
                ns1 = ns1NamespaceIp;
            }
            else
            {
                xName = "FNSLicUlRequest";
                ns1 = ns1NamespaceUl;
            }

            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVFNSLicRequestFileDomain.GetAll().Where(x => x.SMEVFNSLicRequest == requestData).ToList().ForEach(x => SMEVFNSLicRequestFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVFNSLicRequestDomain.Update(requestData);

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
                    SMEVFNSLicRequestDomain.Update(requestData);
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
        public bool TryProcessResponse(SMEVFNSLicRequest requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            if (requestData.FNSLicPersonType == FNSLicPersonType.IP)
            {
                xName = "FNSLicIPRequest";
                ns1 = ns1NamespaceIp;
            }
            else
            {
                xName = "FNSLicUlRequest";
                ns1 = ns1NamespaceUl;
            }

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

                    var FNSresponse = response.MessagePrimaryContent.Element(xName);
                    if (FNSresponse == null)
                    {
                        SetErrorState(requestData, "Данные отсутствуют");
                        return false;
                    }

                    if (FNSresponse.Element(ns1 + "КодОбр")?.Value != "50")
                    {
                        SetErrorState(requestData, "Ошибка.");
                        return false;
                    }

                    var docId = FNSresponse.Element(ns1 + "ИдДок").Value;

                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Документ размещен с ИД " + docId;
                    SMEVFNSLicRequestDomain.Update(requestData);
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

        private string GetDecision(FNSLicDecisionType typeDec)
        {

            switch (typeDec)
            {
                case FNSLicDecisionType.MakeLicense: { return "1"; }
                case FNSLicDecisionType.Reissuance: { return "2"; }
                case FNSLicDecisionType.Pause: { return "3"; }
                case FNSLicDecisionType.Restart: { return "4"; }
                case FNSLicDecisionType.Cancellation: { return "5"; }
                case FNSLicDecisionType.Invalid: { return "6"; }
                case FNSLicDecisionType.Limitation: { return "7"; }
                case FNSLicDecisionType.Revocation: { return "8"; }
                default: return "1";

            }
        }

        private XElement GetInformationRequestXML(SMEVFNSLicRequest requestData)
        {
            string decision = GetDecision(requestData.FNSLicDecisionType);
            if (requestData.FNSLicPersonType == FNSLicPersonType.IP)
            {
                ns1NamespaceUl = ns1NamespaceIp;
            }
            XElement result = null;
            if (requestData.FNSLicRequestType == FNSLicRequestType.Add)
            {
                result = new XElement(ns1 + xName,
                            new XAttribute("ИдДок", requestData.IdDoc),
                            new XElement(ns1 + "СвЮЛ",
                                new XAttribute("ИННЮЛ", "3664032439"),
                                new XAttribute("НаимЮЛ", "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"),
                                new XAttribute("ОГРН", "1033600084968"),
                                new XAttribute("ОКОГУ", "2300230")
                                ),
                            new XElement(ns1 + "СвЛиц",
                                requestData.FNSLicPersonType == FNSLicPersonType.IP ?
                                new XElement(ns1 + "СвИП",
                                    new XAttribute("ИННФЛ", requestData.INN),
                                    new XAttribute("ОГРНИП", requestData.OGRN),
                                        new XElement(ns1 + "ФИО",
                                        new XAttribute("FirstName", requestData.FirstName),
                                        new XAttribute("FamilyName", requestData.FamilyName)
                                        )
                                    )
                                :
                                new XElement(ns1 + "СведЮЛ",
                                    new XAttribute("ИНН", requestData.INN),
                                    new XAttribute("НаимЮЛПолн", requestData.NameUL),
                                    new XAttribute("ОГРН", requestData.OGRN)
                                    ),
                            new XElement(ns1 + "Лицензия",
                                new XAttribute("ВидЛиц", "Лицензия"),
                                new XAttribute("ДатаЛиц", requestData.DateLic.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("ДатаНачЛиц", requestData.DateStartLic.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("ДатаОкончЛиц", requestData.DateEndLic.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("НомЛиц", requestData.NumLic),
                                new XAttribute("СерЛиц", requestData.SerLic),
                                    new XElement(ns1 + "ЛицВидДеят",
                                    new XAttribute("КодСЛВД", requestData.SLVDCode),
                                    new XAttribute("НаимВД", requestData.VDName),
                                    new XAttribute("ПрДейств", "1")
                                    ),
                                    new XElement(ns1 + "СведМестоДейстЛиц",
                                    new XAttribute("АдресТекст", requestData.Address),
                                    new XAttribute("ПрДейств", "1")
                                    )
                                    ),
                            new XElement(ns1 + "РешениеЛО",
                                new XAttribute("ВидРеш", decision),
                                new XAttribute("ДатаНачРеш", requestData.DecisionDateStart.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("ДатаОкончРеш", requestData.DecisionDateEnd.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("ДатаРеш", requestData.DecisionDate.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("НомРеш", requestData.DecisionNum),
                                    new XElement(ns1+"ЛицОргРеш",
                                    new XAttribute("ИННЛО", requestData.LicOrgINN),
                                    new XAttribute("НаимЛОПолн", requestData.LicOrgFullName),
                                    new XAttribute("НаимЛОСокр", requestData.LicOrgShortName),
                                    new XAttribute("ОГРНЛО", requestData.LicOrgOGRN),
                                    new XAttribute("ОКОГУ", requestData.LicOrgOKOGU),
                                    new XAttribute("Регион", requestData.LicOrgRegion)
                                    )
                                )
                            )
                        );
            }
            else if (requestData.FNSLicRequestType == FNSLicRequestType.Delete)
            {
                result = new XElement(ns1+xName,
                            new XAttribute("ИдДок", requestData.IdDoc),
                            new XElement(ns1 + "СвЮЛ",
                                new XAttribute("ИННЮЛ", "7737000005"),
                                new XAttribute("НаимЮЛ", "ГЖИ ВО"),
                                new XAttribute("ОГРН", "1085004000025"),
                                new XAttribute("ОКОГУ", "1000000")
                                ),
                            new XElement(ns1 + "ИдДокИскл", requestData.DeleteIdDoc)
                            );
            } 
            result.SetAttributeValue(XNamespace.Xmlns + "ns1", ns1);
            return result;
        }

        private void ChangeState(SMEVFNSLicRequest requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVFNSLicRequestDomain.Update(requestData);
        }

        private void SetErrorState(SMEVFNSLicRequest requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVFNSLicRequestDomain.Update(requestData);
        }

        private void SaveFile(SMEVFNSLicRequest request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVFNSLicRequestFileDomain.Save(new SMEVFNSLicRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVFNSLicRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVFNSLicRequest request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVFNSLicRequestFileDomain.Save(new SMEVFNSLicRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVFNSLicRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVFNSLicRequest request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVFNSLicRequestFileDomain.Save(new SMEVFNSLicRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVFNSLicRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVFNSLicRequest request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVFNSLicRequestFileDomain.Save(new SMEVFNSLicRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVFNSLicRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
