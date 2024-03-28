using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Bars.GkhGji.DomainService.GISGMP;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Namespaces;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    public class GISGMPService : IGISGMPService
    {
        #region Constants

        static XNamespace bdiNamespace = @"http://roskazna.ru/gisgmp/xsd/BudgetIndex/2.5.0";
        static XNamespace orgNamespace = @"http://roskazna.ru/gisgmp/xsd/Organization/2.5.0";
        static XNamespace comNamespace = @"http://roskazna.ru/gisgmp/xsd/Common/2.5.0";
        static XNamespace chgNamespace = @"http://roskazna.ru/gisgmp/xsd/Charge/2.5.0";
        static XNamespace pkgNamespace = @"http://roskazna.ru/gisgmp/xsd/Package/2.5.0";
        static XNamespace reqNamespace = @"urn://roskazna.ru/gisgmp/xsd/services/import-charges/2.5.0";
        static XNamespace rfdNamespace = @"http://roskazna.ru/gisgmp/xsd/Refund/2.5.0";
        static XNamespace pmntNamespace = @"http://roskazna.ru/gisgmp/xsd/Payment/2.5.0";
        static XNamespace ns0Namespace = @"urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0";
        static XNamespace scNamespace = @"http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0";
        static XNamespace faNamespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0";

        #endregion

        #region Properties              

        public IDomainService<GisGmpFile> GisGmpFileDomain { get; set; }

        public IDomainService<GISGMPPayments> GISGMPPaymentsDomain { get; set; }

        public IDomainService<PayReg> PayRegDomain { get; set; }

        public IDomainService<GisGmp> GisGmpDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public GISGMPService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка начисления в ГИС ГМП
        /// </summary>
        public bool SendCalcRequest(GisGmp requestData, IProgressIndicator indicator = null)
        {
            try
            {
                if (requestData.RequestState == RequestState.ResponseReceived && requestData.GisGmpChargeType == GisGmpChargeType.First &&
                (requestData.Answer.ToLower().Contains("успешно") || requestData.Answer.ToLower().Contains("уже присутствуют")))
                {
                    
                }
                else
                {
                    //Очищаем список файлов
                    indicator?.Report(null, 0, "Очистка старых файлов");
                    GisGmpFileDomain.GetAll().Where(x => x.GisGmp == requestData).ToList().ForEach(x => GisGmpFileDomain.Delete(x.Id));

                    //формируем отправляемую xml
                    indicator?.Report(null, 10, "Формирование запроса");
                    if (requestData.GisGmpChargeType == GisGmpChargeType.First)
                    {
                        requestData.ChargeId = $"I_{Guid.NewGuid()}";
                    }
                    requestData.LegalAct = FindDocNumbers(requestData, true);
                    requestData.LegalAct = requestData.LegalAct != "" ? requestData.LegalAct : null;
                    requestData.DocNumDate = FindDocNumbers(requestData, false);
                    requestData.PayerName = FindPayerName(requestData);
                    XElement request = GetCalcRequestXML(requestData);
                    ChangeState(requestData, RequestState.Formed);

                    indicator?.Report(null, 20, "Отправка запроса");
                    var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                    requestData.MessageId = requestResult.MessageId;
                    // заполняем отправленные поля начисления
                    requestData.AltPayerIdentifierSent = requestData.AltPayerIdentifier;
                    requestData.BillDateSent = requestData.BillDate;
                    requestData.BillForSent = requestData.BillFor;
                    requestData.KBKSent = requestData.KBK;
                    requestData.LegalActSent = requestData.LegalAct;
                    requestData.OKTMOSent = requestData.OKTMO;
                    requestData.PayerNameSent = requestData.PayerName;
                    requestData.PaytReasonSent = requestData.PaytReason;
                    requestData.StatusSent = requestData.Status;
                    requestData.TaxDocDateSent = requestData.TaxDocDate;
                    requestData.TaxDocNumberSent = requestData.TaxDocNumber;
                    requestData.TaxPeriodSent = requestData.TaxPeriod;
                    requestData.TotalAmountSent = requestData.TotalAmount;

                    GisGmpDomain.Update(requestData);

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
                        return false;
                    }
                    else if (requestResult.Status != "requestIsQueued")
                    {
                        SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                        return false;
                    }
                    else
                    {
                        //изменяем статус
                        //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                        requestData.RequestState = RequestState.Queued;
                        requestData.Answer = "Поставлено в очередь";
                        GisGmpDomain.Update(requestData);
                        return true;
                    }
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
                SetErrorState(requestData, "SendCalcRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        public bool SendPayRequest(GisGmp requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                GisGmpFileDomain.GetAll().Where(x => x.GisGmp == requestData).ToList().ForEach(x => GisGmpFileDomain.Delete(x.Id));

                GISGMPPaymentsDomain.GetAll().Where(x => x.GisGmp == requestData).ToList().ForEach(x => GISGMPPaymentsDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request;
                if (requestData.GisGmpPaymentsType == GisGmpPaymentsType.Current)
                {
                    //тестовая отправка
                    // request = GetPayRequestXMLTest(requestData);
                    request = GetPayRequestXML(requestData);
                }
                else
                {
                    request = GetPayRequestByPayerIdentifierXML(requestData);
                }

                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.SMEVStage = "Запрос оплат";
                requestData.MessageId = requestResult.MessageId;
                GisGmpDomain.Update(requestData);

                //
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
                    GisGmpDomain.Update(requestData);
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
                SetErrorState(requestData, "SendPayRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Запрос квитирования
        /// </summary>
        public bool SendReconcileRequest(GisGmp requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                GisGmpFileDomain.GetAll().Where(x => x.GisGmp == requestData).ToList().ForEach(x => GisGmpFileDomain.Delete(x.Id));


                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetReconcileRequestXML(requestData);


                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.SMEVStage = "Запрос квитирования";
                requestData.MessageId = requestResult.MessageId;
                GisGmpDomain.Update(requestData);

                //
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
                    requestData.ReconcileAnswer = "Квитирование поставлено в очередь";
                    GisGmpDomain.Update(requestData);
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
                SetErrorState(requestData, "SendPayRequest exception: " + e.Message);
            }

            return false;
        }

        ///// <summary>
        ///// Запрос квитирования
        ///// </summary>
        //public bool SendReconcileRequest(GISGMPPayments requestData, IProgressIndicator indicator = null)
        //{
        //    try
        //    {
        //        //Очищаем список файлов
        //        indicator?.Report(null, 0, "Очистка старых файлов");
        //        GisGmpFileDomain.GetAll().Where(x => x.GisGmp == requestData.GisGmp).ToList().ForEach(x => GisGmpFileDomain.Delete(x.Id));


        //        //формируем отправляемую xml
        //        indicator?.Report(null, 10, "Формирование запроса");
        //        XElement request = GetReconcileRequestXML(requestData);


        //        ChangeState(requestData.GisGmp, RequestState.Formed);

        //        //
        //        indicator?.Report(null, 20, "Отправка запроса");
        //        var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
        //        requestData.GisGmp.MessageId = requestResult.MessageId;
        //        GisGmpDomain.Update(requestData.GisGmp);

        //        //
        //        indicator?.Report(null, 80, "Сохранение данных для отладки");
        //        SaveFile(requestData.GisGmp, requestResult.SendedData, "SendRequestRequest.dat");
        //        SaveFile(requestData.GisGmp, requestResult.ReceivedData, "SendRequestResponse.dat");

        //        indicator?.Report(null, 90, "Обработка результата");
        //        if (requestResult.Error != null)
        //        {
        //            SetErrorState(requestData.GisGmp, $"Ошибка при отправке: {requestResult.Error}");
        //            SaveException(requestData.GisGmp, requestResult.Error.Exception);
        //        }
        //        else if (requestResult.FaultXML != null)
        //        {
        //            SaveFile(requestData.GisGmp, requestResult.FaultXML, "Fault.xml");
        //            SetErrorState(requestData.GisGmp, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
        //        }
        //        else if (requestResult.Status != "requestIsQueued")
        //        {
        //            SetErrorState(requestData.GisGmp, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
        //        }
        //        else
        //        {
        //            //изменяем статус
        //            //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
        //            requestData.GisGmp.RequestState = RequestState.Queued;
        //            requestData.GisGmp.Answer = "Поставлено в очередь";
        //            GisGmpDomain.Update(requestData.GisGmp);
        //            return true;
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        //ошибки связи прокидываем в контроллер
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        SaveException(requestData.GisGmp, e);
        //        SetErrorState(requestData.GisGmp, "SendPayRequest exception: " + e.Message);
        //    }

        //    return false;
        //}

        /// <summary>
        /// Проверить наличие ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <returns>ответ, если есть ответ, иначе null</returns>
        public GetResponseResponse CheckResponse(GisGmp requestData)
        {
            //запрашиваем ответ
            return Smev3Helper.TryGetResponseAsync(_SMEV3Service, null, null, requestData.MessageId, true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <param name="response">Ответ</param>
        /// <param name="indicator">Индикатор прогресса для таски</param>
        public bool TryProcessResponse(GisGmp requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //сохранение данных
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponceRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponceResponse.dat");
                response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");

                //ошибки наши
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво - чтобы можнор было считать повторно ,если это наш косяк
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                //Ошибки присланные
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
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = response.RequestRejected.Element(SMEVNamespaces12.TypesNamespace + "RejectionReasonDescription")?.Value.Cut(500);
                    GisGmpDomain.Update(requestData);
                    return true;
                }
                else
                {
                    //ответ пустой?
                    if (response.MessagePrimaryContent == null)
                    {
                        SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                        return false;
                    }

                    // принимаем платежи - тут больше не используется
                    if (response.MessagePrimaryContent.Element(ns0Namespace + "ExportPaymentsResponse") != null)
                    {
                        var paymentsElements = response.MessagePrimaryContent.Element(ns0Namespace + "ExportPaymentsResponse")?.Elements();
                        foreach (XElement element in paymentsElements)
                        {
                            // var Payments = Container.Resolve<IDomainService<PayReg>>();
                            var payId = element.Attribute("paymentId")?.Value;
                            // var fdsthrdythdst = Payments.GetAll().ToList();
                            // var fdsthrst = Payments.GetAll()
                            // .Where(x => x.PaymentId == payId).ToList();

                            //MemoryStream streamResp = new MemoryStream();
                            //StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));
                            //try
                            //{
                            //    xwResp.WriteLine(response.MessagePrimaryContent.Document.ToXmlDocument().InnerText);
                            //}
                            //catch (Exception e)
                            //{
                            //    xwResp.WriteLine("Ошибка: " + e.Message);
                            //}
                            //xwResp.Flush();
                            //streamResp.Position = 0;

                            if ((PayRegDomain.GetAll()
                                .Where(x => x.PaymentId == payId).Count() == 0))
                            {
                                PayRegDomain.Save(new PayReg
                                {
                                    Amount = element.Attribute("amount") != null ? Convert.ToDecimal(element.Attribute("amount").Value) / 100 : 0,
                                    Kbk = element.Attribute("kbk")?.Value,
                                    OKTMO = element.Attribute("oktmo")?.Value,
                                    PaymentDate = NullableDateParse(element.Attribute("paymentDate")?.Value),
                                    PaymentId = payId,
                                    SupplierBillID = element.Attribute("supplierBillID")?.Value,
                                    Purpose = element.Attribute("purpose")?.Value,
                                    PaymentOrg = ParsePaymentOrg(element.Element(pmntNamespace + "PaymentOrg")),
                                    PaymentOrgDescr = ParsePaymentOrgDescr(element.Element(pmntNamespace + "PaymentOrg")),
                                    PayerId = element.Element(pmntNamespace + "Payer")?.Attribute("payerIdentifier")?.Value,
                                    PayerAccount = element.Element(pmntNamespace + "Payer")?.Attribute("payerAccount")?.Value,
                                    PayerName = element.Element(pmntNamespace + "Payer")?.Attribute("payerName")?.Value,
                                    BdiStatus = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("status")?.Value,
                                    BdiPaytReason = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("paytReason")?.Value,
                                    BdiTaxPeriod = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("taxPeriod")?.Value,
                                    BdiTaxDocNumber = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("taxDocNumber")?.Value,
                                    BdiTaxDocDate = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("taxDocDate")?.Value,
                                    AccDocDate = NullableDateParse(element.Element(pmntNamespace + "AccDoc")?.Attribute("accDocDate")?.Value),
                                    AccDocNo = element.Element(pmntNamespace + "AccDoc")?.Attribute("accDocNo")?.Value,
                                    Status = byte.Parse(element.Element(comNamespace + "ChangeStatus")?.Attribute("meaning")?.Value),
                                    GisGmp = requestData,
                                    Reconcile = element.Attribute("supplierBillID") == null ? Gkh.Enums.YesNoNotSet.NotSet : element.Attribute("supplierBillID").Value != "0" ? Gkh.Enums.YesNoNotSet.Yes : Gkh.Enums.YesNoNotSet.No

                                });
                                // PayReg newPayment = ;
                            };
                        }



                        // разбираем оплаты
                        requestData.RequestState = RequestState.ResponseReceived;
                        requestData.Answer = "Оплаты найдены, список во вкладке Все оплаты плательщика";
                        GisGmpDomain.Update(requestData);
                        return true;
                    }

                    // ответ на запрос квитирования
                    else if (response.MessagePrimaryContent.Element(faNamespace + "ForcedAcknowledgementResponse") != null)
                    {
                        var paymentsElements = response.MessagePrimaryContent.Element(faNamespace + "ForcedAcknowledgementResponse")?.Elements();
                        // List<GISGMPPayments> newPaymentsList = new List<GISGMPPayments>();
                        //MemoryStream streamResp = new MemoryStream();
                        //StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));
                        //xwResp.WriteLine(response.MessagePrimaryContent.Document);
                        //xwResp.Flush();
                        //streamResp.Position = 0;
                        //billStatus
                        foreach (XElement element in paymentsElements)
                        {
                            if (element.Attribute("billStatus") != null && element.Attribute("paymentId") != null)
                            {
                                var paymentId = element.Attribute("paymentId").Value;
                                var payment = PayRegDomain.GetAll()
                                    .Where(x => x.GisGmp == requestData)
                                    .Where(x => x.PaymentId == paymentId).FirstOrDefault();

                                var status = element.Attribute("billStatus").Value;
                                if (status != "3")
                                {
                                    if (payment != null)
                                    {
                                        payment.Reconcile = Gkh.Enums.YesNoNotSet.Yes;
                                        PayRegDomain.Update(payment);
                                    }
                                }
                            }
                        }

                        requestData.RequestState = RequestState.ResponseReceived;
                        requestData.ReconcileAnswer = "Сквитировано";
                        GisGmpDomain.Update(requestData);
                        return true;
                    }

                    //разбираем xml, которую прислал сервер
                    var code = response.MessagePrimaryContent.Element(reqNamespace + "ImportChargesResponse")?.Element(comNamespace + "ImportProtocol")?.Attribute("code")?.Value;
                    var value = response.MessagePrimaryContent.Element(reqNamespace + "ImportChargesResponse")?.Element(comNamespace + "ImportProtocol")?.Attribute("description")?.Value;

                    if (value == "Успешно")
                    {
                        //проставляем флаг квитирования всем платежам с УИНом начисления, т.к. они автоматически сквитируются в ГИС ГМП и сопоставляем все эти платежи с начислением
                        var payments = PayRegDomain.GetAll()
                                .Where(x => x.SupplierBillID == requestData.UIN);
                        foreach (PayReg payment in payments)
                        {
                            payment.GisGmp = requestData;
                            payment.Reconcile = Gkh.Enums.YesNoNotSet.Yes;
                            PayRegDomain.Update(payment);
                        }
                        //изменяем статус
                        requestData.RequestState = RequestState.ResponseReceived;
                        requestData.Answer = value;
                        GisGmpDomain.Update(requestData);
                        return true;
                    }
                    else if (value.Contains("уже присутствуют"))
                    {
                        //изменяем статус
                        requestData.RequestState = RequestState.ResponseReceived;
                        requestData.Answer = value;
                        GisGmpDomain.Update(requestData);
                        return true;
                    }
                    else
                    {
                        SetErrorState(requestData, $"ГИС ГМП вернул ошибку: {value}");
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "TryProcessResponse exception: " + e.Message);
                return false;
            }
        }

        //public bool TryProcessResponse(GisGmp requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        //{
        //    try
        //    {
        //        //сохранение данных
        //        indicator?.Report(null, 40, "Сохранение данных для отладки");
        //        SaveFile(requestData, response.SendedData, "GetResponceRequest.dat");
        //        SaveFile(requestData, response.ReceivedData, "GetResponceResponse.dat");
        //        response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

        //        indicator?.Report(null, 70, "Обработка результата");

        //        //ошибки наши
        //        if (response.Error != null)
        //        {
        //            SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
        //            SaveException(requestData, response.Error.Exception);
        //            return false;
        //        }

        //        //ACK - ставим вдумчиво - чтобы можнор было считать повторно ,если это наш косяк
        //        _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

        //        //Ошибки присланные
        //        if (response.FaultXML != null)
        //        {
        //            SaveFile(requestData, response.FaultXML, "Fault.xml");
        //            SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
        //        }
        //        //сервер вернул ошибку?
        //        else if (response.AsyncProcessingStatus != null)
        //        {
        //            SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
        //            SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в приаттаченом файле AsyncProcessingStatus.xml");
        //        }
        //        //сервер отклонил запрос?
        //        else if (response.RequestRejected != null)
        //        {
        //            requestData.RequestState = RequestState.ResponseReceived;
        //            requestData.Answer = response.RequestRejected.Element(SMEVNamespaces12.TypesNamespace + "RejectionReasonDescription")?.Value;
        //            GisGmpDomain.Update(requestData);
        //            return true;
        //        }
        //        else
        //        {
        //            //ответ пустой?
        //            if (response.MessagePrimaryContent == null)
        //            {
        //                SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
        //                return false;
        //            }

        //            if (response.MessagePrimaryContent.Element(ns0Namespace + "ExportPaymentsResponse") != null)
        //            {
        //                var paymentsElements = response.MessagePrimaryContent.Element(ns0Namespace + "ExportPaymentsResponse")?.Elements();
        //                List<GISGMPPayments> newPaymentsList = new List<GISGMPPayments>();
        //                MemoryStream streamResp = new MemoryStream();
        //                StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));
        //                xwResp.WriteLine(response.MessagePrimaryContent.Document);
        //                xwResp.Flush();
        //                streamResp.Position = 0;

        //                foreach (XElement element in paymentsElements)
        //                {
        //                    GISGMPPayments newPayment = new GISGMPPayments
        //                    {
        //                        Amount = element.Attribute("amount") != null ? Convert.ToDecimal(element.Attribute("amount").Value) / 100 : 0,
        //                        GisGmp = requestData,
        //                        Kbk = element.Attribute("kbk")?.Value,
        //                        ObjectCreateDate = DateTime.Now,
        //                        ObjectEditDate = DateTime.Now,
        //                        ObjectVersion = 333,
        //                        OKTMO = element.Attribute("oktmo")?.Value,
        //                        PaymentDate = NullableDateParse(element.Attribute("paymentDate")?.Value),
        //                        PaymentId = element.Attribute("paymentId")?.Value,
        //                        SupplierBillID = element.Attribute("supplierBillID")?.Value,
        //                        Purpose = element.Attribute("purpose")?.Value,
        //                        FileInfo = _fileManager.SaveFile(streamResp, "GisGMPAnswer.xml")
        //                    };
        //                    newPaymentsList.Add(newPayment);

        //                }

        //                foreach (GISGMPPayments pay in newPaymentsList)
        //                {
        //                    GISGMPPaymentsDomain.Save(pay);
        //                }
        //                // разбираем оплаты
        //                requestData.RequestState = RequestState.ResponseReceived;
        //                requestData.Answer = "Оплаты найдены, список во вкладке Все оплаты плательщика";
        //                GisGmpDomain.Update(requestData);
        //                return true;
        //            }

        //            else if (response.MessagePrimaryContent.Element(faNamespace + "ForcedAcknowledgementResponse") != null)
        //            {
        //                var paymentsElements = response.MessagePrimaryContent.Element(faNamespace + "ForcedAcknowledgementResponse")?.Elements();
        //                List<GISGMPPayments> newPaymentsList = new List<GISGMPPayments>();
        //                MemoryStream streamResp = new MemoryStream();
        //                StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));
        //                xwResp.WriteLine(response.MessagePrimaryContent.Document);
        //                xwResp.Flush();
        //                streamResp.Position = 0;
        //                //billStatus
        //                foreach (XElement element in paymentsElements)
        //                {
        //                    if (element.Attribute("billStatus") != null && element.Attribute("paymentId") != null)
        //                    {
        //                        var paymentId = element.Attribute("paymentId").Value;
        //                        var payment = GISGMPPaymentsDomain.GetAll()
        //                            .Where(x => x.GisGmp == requestData)
        //                            .Where(x => x.PaymentId == paymentId).FirstOrDefault();

        //                        var status = element.Attribute("billStatus").Value;
        //                        if (status != "3")
        //                        {
        //                            if (payment != null)
        //                            {
        //                                payment.Reconcile = Gkh.Enums.YesNoNotSet.Yes;
        //                                GISGMPPaymentsDomain.Update(payment);
        //                            }
        //                        }
        //                    }
        //                }

        //                // разбираем оплаты
        //                requestData.RequestState = RequestState.ResponseReceived;
        //                requestData.Answer = "Сквитировано";
        //                GisGmpDomain.Update(requestData);
        //                return true;
        //            }

        //            //разбираем xml, которую прислал сервер
        //            var code = response.MessagePrimaryContent.Element(reqNamespace + "ImportChargesResponse")?.Element(comNamespace + "ImportProtocol")?.Attribute("code")?.Value;
        //            var value = response.MessagePrimaryContent.Element(reqNamespace + "ImportChargesResponse")?.Element(comNamespace + "ImportProtocol")?.Attribute("description")?.Value;

        //            if (code != "0")
        //                SetErrorState(requestData, $"ГИС ГМП вернул ошибку: {value}");
        //            else
        //            {
        //                //изменяем статус
        //                requestData.RequestState = RequestState.ResponseReceived;
        //                requestData.Answer = value;
        //                GisGmpDomain.Update(requestData);
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        SaveException(requestData, e);
        //        SetErrorState(requestData, "TryProcessResponse exception: " + e.Message);
        //        return false;
        //    }
        //}

        public IDataResult ListGisGmp(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = GisGmpDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.AltPayerIdentifier,
                    x.UIN,
                    x.BillFor,
                    x.TotalAmount,
                    x.PayerType,
                    x.PayerName,
                    x.LegalAct,
                    x.INN,
                    x.DocNumDate
                }).ToList();

            var data2 = data.AsQueryable().Select(x => new
            {
                x.Id,
                x.AltPayerIdentifier,
                x.UIN,
                x.BillFor,
                x.TotalAmount,
                x.PayerType,
                x.PayerName,
                x.LegalAct,
                x.INN,
                x.DocNumDate
            }).Filter(loadParams, Container);

            int totalCount = data2.Count();

            return new ListDataResult(data2.Order(loadParams).Paging(loadParams).ToList(), data2.Count());
            //var preData = GisGmpDomain.GetAll()
            //    .Filter(loadParams, Container);

            //var data = preData
            //    .Select(x => new
            //    {
            //        x.Id,
            //        x.AltPayerIdentifier,
            //        x.UIN,
            //        x.BillFor,
            //        x.TotalAmount,
            //        x.PayerType,
            //        x.PayerName,
            //        x.LegalAct,
            //        x.INN,
            //        DocNumDate = FindDocNumbers(x, false)
            //    })
            //    ;

            //int totalCount = data.Count();

            //return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        #endregion

        #region Private methods

        private string ParsePaymentOrg(XElement element)
        {
            string org = "";
            if (element.Element(orgNamespace + "Bank") != null)
            {
                org = "Банк";
            }
            else
            {
                if (element.Element(orgNamespace + "UFK") != null)
                {
                    org = "ТОФК или другая организация";
                };
                if (element.Element(orgNamespace + "Other") != null)
                {
                    org = org == "" ? "Иной способ" : org + ", иной способ";
                }
            }
            return org;
        }

        private string ParsePaymentOrgDescr(XElement element)
        {
            string descr = "";
            if (element.Element(orgNamespace + "Bank") != null)
            {
                descr = element.Element(orgNamespace + "Bank").Attribute("bik").Value;
            }
            else
            {
                if (element.Element(orgNamespace + "UFK") != null)
                {
                    descr = element.Element(orgNamespace + "UFK").Value;
                };
                if (element.Element(orgNamespace + "Other") != null)
                {
                    descr = descr == "" ? element.Element(orgNamespace + "Other").Value : descr + ", " + element.Element(orgNamespace + "Other").Value;
                }
            }
            return descr;
        }

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        private XElement GetPayRequestByPayerIdentifierXML(GisGmp requestData)
        {
            var identifiers = GetIdentifiersFromConfig();

            var payReqByIdentifier = new ExportPaymentsProxy.ExportPaymentsRequest
            {
                Id = requestData.Id.ToString(),
                timestamp = DateTime.Now,
                senderIdentifier = identifiers.SenderIdentifier,
                originatorId = identifiers.OriginatorID,
                senderRole = identifiers.SenderRole,
                Paging = new ExportPaymentsProxy.PagingType
                {
                    pageNumber = "1",
                    pageLength = "1000"
                },
                PaymentsExportConditions = new ExportPaymentsProxy.PaymentsExportConditions
                {
                    kind = requestData.GisGmpPaymentsKind.ToString().Replace('_', '-'),
                    ItemElementName = ExportPaymentsProxy.ItemChoiceType.PayersConditions,
                    Item = new ExportPaymentsProxy.PayersConditionsType
                    {
                        TimeInterval = new ExportPaymentsProxy.TimeIntervalType
                        {
                            startDate = requestData.GetPaymentsStartDate.HasValue ? requestData.GetPaymentsStartDate.Value : DateTime.Now.AddDays(-10),
                            endDate = requestData.GetPaymentsEndDate.HasValue ? requestData.GetPaymentsEndDate.Value : DateTime.Now.AddDays(-10)
                        },
                        ItemsElementName = new ExportPaymentsProxy.ItemsChoiceType[]
                        {
                            ExportPaymentsProxy.ItemsChoiceType.PayerIdentifier
                        },
                        Items = new string[]
                        {
                            requestData.AltPayerIdentifier
                        }
                    }
                }
            };

            var result = ToXElement<ExportPaymentsProxy.ExportPaymentsRequest>(payReqByIdentifier);

            result.SetAttributeValue(XNamespace.Xmlns + "com", comNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "org", orgNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "sc", scNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "pmnt", pmntNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "ns0", ns0Namespace);

            return result;
        }

        private XElement GetPayRequestXML(GisGmp requestData)
        {
            var identifiers = GetIdentifiersFromConfig();

            var payReq = new ExportPaymentsProxy.ExportPaymentsRequest
            {
                Id = requestData.Id.ToString(),
                timestamp = DateTime.Now,
                senderIdentifier = identifiers.SenderIdentifier,
                originatorId = identifiers.OriginatorID,
                senderRole = identifiers.SenderRole,
                Paging = new ExportPaymentsProxy.PagingType
                {
                    pageNumber = "1",
                    pageLength = "1000"
                },
                PaymentsExportConditions = new ExportPaymentsProxy.PaymentsExportConditions
                {
                    kind = requestData.GisGmpPaymentsKind.ToString().Replace('_', '-'),
                    ItemElementName = ExportPaymentsProxy.ItemChoiceType.ChargesConditions,
                    Item = new ExportPaymentsProxy.ChargesConditionsType
                    {
                        SupplierBillID = new string[] { requestData.UIN }
                    }
                }
            };

            var result = ToXElement<ExportPaymentsProxy.ExportPaymentsRequest>(payReq);

            //result.SetAttributeValue(XNamespace.Xmlns + "bdi", bdiNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "com", comNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "org", orgNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "sc", scNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "pmnt", pmntNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "ns0", ns0Namespace);

            return result;
        }

        private XElement GetReconcileRequestXML(GisGmp requestData)
        {
            var identifiers = GetIdentifiersFromConfig();

            List<string> paymentIdsList = PayRegDomain.GetAll().Where(x => x.GisGmp == requestData).Where(x => x.Reconcile != Gkh.Enums.YesNoNotSet.Yes).Select(x => x.PaymentId).ToList();

            var res = new ForcedAckmowledgmentProxy.ForcedAcknowledgementRequest
            {
                Id = $"ID_{DateTime.Now.Ticks}",
                timestamp = DateTime.Now,
                senderIdentifier = identifiers.SenderIdentifier,
                senderRole = identifiers.SenderRole,
                originatorId = identifiers.OriginatorID,
                Item = new ForcedAckmowledgmentProxy.ForcedAcknowledgementRequestReconcile
                {
                    supplierBillId = requestData.UIN,
                    Items = paymentIdsList.ToArray()
                }
            };

            return ToXElement<ForcedAckmowledgmentProxy.ForcedAcknowledgementRequest>(res);
        }

        private XElement GetPayRequestXMLTest(GisGmp requestData)
        {
            var identifiers = GetIdentifiersFromConfig();

            var result = new XElement(ns0Namespace + "ExportPaymentsRequest",
                new XAttribute("Id", $"ID_{ requestData.Id }"),
                new XAttribute("timestamp", DateTime.Now.ToString("o")),
                new XAttribute("senderIdentifier", identifiers.SenderIdentifier),
                new XAttribute("senderRole", identifiers.SenderRole),
                new XElement(comNamespace + "Paging",
                    new XAttribute("pageNumber", 1),
                    new XAttribute("pageLength", 1000)
                ),
                new XElement(scNamespace + "PaymentsExportConditions",
                    new XAttribute("kind", requestData.GisGmpPaymentsKind.ToString().Replace('_', '-')),
                    new XElement(scNamespace + "ChargesConditions",
                        new XElement(scNamespace + "supplierBillID", "32117072411021588933"
                        )
                    )
                )
            );

          //  result.SetAttributeValue(XNamespace.Xmlns + "bdi", bdiNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "com", comNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "org", orgNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "sc", scNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "pmnt", pmntNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "ns0", ns0Namespace);
            return result;
        }

        private XElement GetCalcRequestXML(GisGmp requestData)
        {
            var identifiers = GetIdentifiersFromConfig();

            var importCHReq = new ImportChargesProxy.ImportChargesRequest
            {
                Id = $"ID_{requestData.Id}",
                timestamp = DateTime.Now,
                senderIdentifier = identifiers.SenderIdentifier,
                senderRole = identifiers.SenderRole,
                ChargesPackage = GetChargeOrChangePackage(requestData)
            };

            XElement result = ToXElement<ImportChargesProxy.ImportChargesRequest>(importCHReq);

            //result.SetAttributeValue(XNamespace.Xmlns + "bdi", bdiNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "org", orgNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "com", comNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "chg", chgNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "pkg", pkgNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "req", reqNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "rfd", rfdNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "pmnt", pmntNamespace);

            return result;
        }

        private ImportChargesProxy.ChargesPackage GetChargeOrChangePackage(GisGmp requestData)
        {
            if (requestData.GisGmpChargeType == GisGmpChargeType.First)
            {
                return new ImportChargesProxy.ChargesPackage
                {
                    Items = GetImportChargeArray(requestData)
                };
            }
            else
            {
                return new ImportChargesProxy.ChargesPackage
                {
                    Items = GetImportChangeArray(requestData)
                };
            }
        }

        private ImportChargesProxy.ImportedChargeType[] GetImportChargeArray(GisGmp requestData)
        {
            List<ImportChargesProxy.ImportedChargeType> chargesList = new List<ImportChargesProxy.ImportedChargeType>();
            var childResolId = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Parent.Id == requestData.Protocol.Id)
                .Select(x => x.Children.Id)
                .FirstOrDefault();
            var resolution = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x => x.Id == childResolId)
                .FirstOrDefault();
            var identifiers = GetIdentifiersFromConfig();
            chargesList.Add(new ImportChargesProxy.ImportedChargeType
            {
                originatorId = identifiers.OriginatorID,
                Id = requestData.ChargeId,
                supplierBillID = requestData.UIN,
                billDate = requestData.BillDate,
                totalAmount = (ulong)Math.Round(requestData.TotalAmount * 100, 0),
                purpose = requestData.BillFor,
                kbk = requestData.KBK,
                oktmo = requestData.OKTMO,
                legalAct = requestData.LegalAct != null ? requestData.LegalAct : null,
                Payee = GetPayee(),
                Payer = new ImportChargesProxy.Payer
                {
                    payerIdentifier = requestData.AltPayerIdentifier,
                    payerName = requestData.PayerName.Trim()
                },
                BudgetIndex = new ImportChargesProxy.BudgetIndexType
                {
                    status = requestData.Status,
                    paytReason = requestData.PaytReason,
                    taxPeriod = requestData.TaxPeriod,
                    taxDocNumber = requestData.TaxDocNumber,
                    taxDocDate = requestData.TaxDocDate
                }
            });
            return chargesList.ToArray();
        }

        private ImportChargesProxy.Payee GetPayee()
        {
            return new ImportChargesProxy.Payee
            {
                name = Payee.Name,
                inn = Payee.INN,
                kpp = Payee.KPP,
                ogrn = Payee.OGRN,
                OrgAccount = new ImportChargesProxy.OrgAccount
                {
                    accountNumber = Payee.AccountNumber,
                    Bank = new ImportChargesProxy.BankType
                    {
                        name = Payee.BankName,
                        bik = Payee.BIK,
                        correspondentBankAccount = Payee.CorrespondentBankAccount
                    }
                }
            };
        }

        private ImportChargesProxy.ImportedChangeType[] GetImportChangeArray(GisGmp requestData)
        {
            List<ImportChargesProxy.ImportedChangeType> chargesList = new List<ImportChargesProxy.ImportedChangeType>();
            var identifiers = GetIdentifiersFromConfig();
            chargesList.Add(new ImportChargesProxy.ImportedChangeType
            {
                originatorId = identifiers.OriginatorID,
                Id = requestData.ChargeId != null ? requestData.ChargeId : $"I_{Guid.NewGuid()}",
                ItemElementName = ImportChargesProxy.ItemChoiceType.SupplierBillId,
                Item = requestData.UIN,
                Change = new ImportChargesProxy.ChangeType[]
                {
                    requestData.AltPayerIdentifierSent != requestData.AltPayerIdentifier ?
                    new ImportChargesProxy.ChangeType
                    {
                    fieldNum = "201",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.AltPayerIdentifier ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.BillDateSent != requestData.BillDate ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "4",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.BillDate.ToString("yyyy-MM-ddTHH:mm:ss.ssszzz") ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.BillForSent != requestData.BillFor ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "24",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.BillFor ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.KBKSent != requestData.KBK ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "104",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.KBK ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.LegalActSent != requestData.LegalAct ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "1010",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.LegalAct ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.OKTMOSent != requestData.OKTMO ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "105",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.OKTMO ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.PayerNameSent != requestData.PayerName ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "8",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.PayerName ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.PaytReasonSent != requestData.PaytReason ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "106",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.PaytReason ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.StatusSent != requestData.Status ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "101",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.Status ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.TaxDocDateSent != requestData.TaxDocDate ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "109",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.TaxDocDate ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.TaxDocNumberSent != requestData.TaxDocNumber ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "108",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.TaxDocNumber ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.TaxPeriodSent != requestData.TaxPeriod ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "107",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.TaxPeriod ?? "NULL"
                            }
                        }
                    } : null,

                    requestData.TotalAmountSent != requestData.TotalAmount ?
                    new ImportChargesProxy.ChangeType
                    {
                        fieldNum = "7",
                        ChangeValue = new ImportChargesProxy.ChangeTypeChangeValue[]
                        {
                            new ImportChargesProxy.ChangeTypeChangeValue
                            {
                                value = requestData.TotalAmount.ToString() ?? "NULL"
                            }
                        }
                    } : null
                },
                ChangeStatus = new ImportChargesProxy.ChangeStatus
                {
                    Meaning = GetMeaning(requestData.GisGmpChargeType).ToString(),
                    Reason = requestData.Reason,
                    ChangeDate = DateTime.Now,
                    ChangeDateSpecified = true
                }
            });
            return chargesList.ToArray();
        }

        [Obsolete("Переделано, используется GetChargeOrChangePackage")]
        private XElement GetChargeOrChange(GisGmp requestData)
        {
            var identifiers = GetIdentifiersFromConfig();

            if (requestData.GisGmpChargeType == GisGmpChargeType.First)
                return new XElement(pkgNamespace + "ChargesPackage",
                    new XElement(pkgNamespace + "ImportedCharge",
                        new XAttribute("originatorId", identifiers.OriginatorID),
                        new XAttribute("Id", requestData.ChargeId), //идентификатор платежа, если их несколько
                        new XAttribute("supplierBillID", requestData.UIN),
                        new XAttribute("billDate", requestData.BillDate.ToString("yyyy-MM-ddTHH:mm:ss.ssszzz")),
                        //new XAttribute("billDate", requestData.BillDate.ToString("yyyy-MM-dd")),2018-11-02T14:06:30.313+03:00
                        new XAttribute("totalAmount", Math.Round(requestData.TotalAmount * 100, 0)),
                        new XAttribute("purpose", requestData.BillFor),
                        new XAttribute("kbk", requestData.KBK),
                        new XAttribute("oktmo", requestData.OKTMO),
                        requestData.LegalAct != null ? new XAttribute("legalAct", requestData.LegalAct) : null,
                        Payee.GetXml(),
                        new XElement(chgNamespace + "Payer",
                            new XAttribute("payerIdentifier", requestData.AltPayerIdentifier),
                            new XAttribute("payerName", requestData.PayerName.Trim())
                            ),
                        new XElement(chgNamespace + "BudgetIndex",
                            new XAttribute("status", requestData.Status),
                            new XAttribute("paytReason", requestData.PaytReason),
                            new XAttribute("taxPeriod", requestData.TaxPeriod),
                            new XAttribute("taxDocNumber", requestData.TaxDocNumber),
                            new XAttribute("taxDocDate", requestData.TaxDocDate)
                            )
                        )
                    );
            else
                return new XElement(pkgNamespace + "ChargesPackage",
                    new XElement(pkgNamespace + "ImportedChange",
                        new XAttribute("originatorId", identifiers.OriginatorID),
                        requestData.ChargeId != null ? 
                            new XAttribute("Id", requestData.ChargeId) :
                            new XAttribute("Id", $"I_{Guid.NewGuid()}"), //идентификатор платежа, если их несколько
                            new XElement(pkgNamespace + "SupplierBillId", requestData.UIN),

                        requestData.AltPayerIdentifierSent != requestData.AltPayerIdentifier ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 201),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.AltPayerIdentifier ?? "NULL")
                            )
                        ) : null,

                        requestData.BillDateSent != requestData.BillDate ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 4),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.BillDate.ToString("yyyy-MM-ddTHH:mm:ss.ssszzz") ?? "NULL")
                            )
                        ) : null,

                        requestData.BillForSent != requestData.BillFor ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 24),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.BillFor ?? "NULL")
                            )
                        ) : null,

                        requestData.KBKSent != requestData.KBK ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 104),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.KBK ?? "NULL")
                            )
                        ) : null,

                        requestData.LegalActSent != requestData.LegalAct ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 1010),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.LegalAct ?? "NULL")
                            )
                        ) : null,

                        requestData.OKTMOSent != requestData.OKTMO ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 105),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.OKTMO ?? "NULL")
                            )
                        ) : null,

                        requestData.PayerNameSent != requestData.PayerName ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 8),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.PayerName.Trim() ?? "NULL")
                            )
                        ) : null,

                        requestData.PaytReasonSent != requestData.PaytReason ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 106),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.PaytReason ?? "NULL")
                            )
                        ) : null,

                        requestData.StatusSent != requestData.Status ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 101),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.Status ?? "NULL")
                            )
                        ) : null,

                        requestData.TaxDocDateSent != requestData.TaxDocDate ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 109),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.TaxDocDate ?? "NULL")
                            )
                        ) : null,

                        requestData.TaxDocNumberSent != requestData.TaxDocNumber ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 108),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.TaxDocNumber ?? "NULL")
                            )
                        ) : null,

                        requestData.TaxPeriodSent != requestData.TaxPeriod ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 107),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.TaxPeriod ?? "NULL")
                            )
                        ) : null,

                        requestData.TotalAmountSent != requestData.TotalAmount ?
                        new XElement(pkgNamespace + "Change",
                            new XAttribute("fieldNum", 7),
                            new XElement(pkgNamespace + "ChangeValue",
                                new XAttribute("value", requestData.TotalAmount)
                            )
                        ) : null,

                        new XElement(comNamespace + "ChangeStatus",
                            new XElement(comNamespace + "Meaning", GetMeaning(requestData.GisGmpChargeType)),
                            new XElement(comNamespace + "Reason", requestData.Reason),
                            new XElement(comNamespace + "ChangeDate", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ssszzz"))
                            )
                    )
                );
        }

        /// <summary>
        /// Получает УРН участника-отправителя запроса из конфига
        /// </summary>
        private Identifiers GetIdentifiersFromConfig()
        {
            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Chelyabinsk");
            var identifiers = new Identifiers
            {
                SenderIdentifier = config.GetAs("SenderIdentifier", (string)null, true),
                SenderRole = config.GetAs("SenderRole", (string)null, true),
                OriginatorID = config.GetAs("OriginatorID", (string)null, true),
            };

            if (String.IsNullOrEmpty(identifiers.SenderIdentifier))
                throw new ApplicationException("Не найден SenderIdetifier в конфиге модуля Bars.GkhGji.Regions.Chelyabinsk");
            if (String.IsNullOrEmpty(identifiers.SenderRole))
                throw new ApplicationException("Не найден SenderRole в конфиге модуля Bars.GkhGji.Regions.Chelyabinsk");
            if (String.IsNullOrEmpty(identifiers.OriginatorID))
                throw new ApplicationException("Не найден OriginatorID в конфиге модуля Bars.GkhGji.Regions.Chelyabinsk");

            return identifiers;
        }

        private int GetMeaning(GisGmpChargeType chargeType)
        {
            switch (chargeType)
            {
                case GisGmpChargeType.First:
                    return 1;
                case GisGmpChargeType.Refinement:
                    return 2;
                case GisGmpChargeType.Cancellation:
                    return 3;
                case GisGmpChargeType.ReCancellation:
                    return 4;
                default:
                    throw new Exception($"GISGMPService: не найдено значение Meaning для {chargeType}");
            }
        }

        private void SaveFile(GisGmp request, byte[] data, string fileName)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            GisGmpFileDomain.Save(new GisGmpFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GisGmp = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(GisGmp request, XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            GisGmpFile faultRec = new GisGmpFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GisGmp = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            };

            GisGmpFileDomain.Save(faultRec);
        }

        private void SaveException(GisGmp request, Exception exception)
        {
            if (exception == null)
                return;

            GisGmpFileDomain.Save(new GisGmpFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GisGmp = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeState(GisGmp requestData, RequestState state)
        {
            requestData.RequestState = state;
            GisGmpDomain.Update(requestData);
        }

        private void SetErrorState(GisGmp requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            GisGmpDomain.Update(requestData);
        }

        private Int32 CheckSum(String number)
        {
            Int32 result = CheckSum(number, 1);

            return result != 10 ? result : CheckSum(number, 3) % 10;
        }

        private Int32 CheckSum(String number, Int32 ves)
        {
            int sum = 0;
            for (int i = 0; i < number.Length; i++)
            {
                int t = (int)Char.GetNumericValue(number[i]);
                int rrr = ((ves % 10) == 0 ? 10 : ves % 10);

                sum += t * rrr;
                ves++;
            }

            return sum % 11;
        }

        private string FindPayerName(GisGmp x)
        {
            var DocumentGjiChildrenRepo = Container.Resolve<IRepository<DocumentGjiChildren>>();
            var DocumentGjiRepo = Container.Resolve<IRepository<DocumentGji>>();
            var ResolutionRepo = Container.Resolve<IRepository<Resolution>>();
            var Protocol197Repo = Container.Resolve<IRepository<Protocol197>>();
            var ResolutionProsecutorRepo = Container.Resolve<IRepository<ResolPros>>();
            var ResolutionRospotrebnadzorRepo = Container.Resolve<IRepository<ResolutionRospotrebnadzor>>();
            var ManOrgLicenseRequestRepo = Container.Resolve<IRepository<ManOrgLicenseRequest>>();
            var LicenseReissuanceRepo = Container.Resolve<IRepository<LicenseReissuance>>();


            string payerName = "";
            if (x.TypeLicenseRequest == TypeLicenseRequest.NotSet && x.Protocol != null)
            {

                var document = DocumentGjiRepo.GetAll()
                    .Where(y => y == x.Protocol).FirstOrDefault();
                if (document != null)
                {

                    if (document.TypeDocumentGji == TypeDocumentGji.Protocol ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                    {
                        var docChildrenParent = DocumentGjiChildrenRepo.GetAll()
                            .Where(y => y.Parent == document).FirstOrDefault();
                        if (docChildrenParent != null)
                        {
                            var resolution = ResolutionRepo.GetAll()
                                .Where(y => y == docChildrenParent.Children).FirstOrDefault();
                            if (resolution != null)
                            {
                                if ((resolution.PhysicalPerson != null) && (resolution.PhysicalPerson != ""))
                                {
                                    payerName = resolution.PhysicalPerson.Trim();
                                }
                                else if ((resolution.Contragent != null) && (resolution.Contragent.Name != null) && (resolution.Contragent.Name != ""))
                                {
                                    payerName = resolution.Contragent.Name.Trim();
                                }
                            }

                        }

                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                    {
                        var resolution = ResolutionRepo.GetAll()
                            .Where(y => y == document).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.PhysicalPerson != null) && (resolution.PhysicalPerson != ""))
                            {
                                payerName = resolution.PhysicalPerson;
                            }
                            else if ((resolution.Contragent != null) && (resolution.Contragent.Name != null) && (resolution.Contragent.Name != ""))
                            {
                                payerName = resolution.Contragent.Name;
                            }
                        }
                    }
                    else if (
                        document.TypeDocumentGji == TypeDocumentGji.Protocol197)
                    {
                        var resolution = Protocol197Repo.GetAll()
                            .Where(y => y == document).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.PhysicalPerson != null) && (resolution.PhysicalPerson != ""))
                            {
                                payerName = resolution.PhysicalPerson;
                            }
                            else if ((resolution.Contragent != null) && (resolution.Contragent.Name != null) && (resolution.Contragent.Name != ""))
                            {
                                payerName = resolution.Contragent.Name;
                            }
                        }
                    }

                }

            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.First && x.ManOrgLicenseRequest != null)
            {
                var manOrgLicReq = ManOrgLicenseRequestRepo.GetAll()
                    .Where(y => y == x.ManOrgLicenseRequest).FirstOrDefault();
                if (manOrgLicReq != null)
                {
                    if ((manOrgLicReq.Contragent != null) && (manOrgLicReq.Contragent.Name != null) && (manOrgLicReq.Contragent.Name != ""))
                    {
                        payerName = manOrgLicReq.Contragent.Name;
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Reissuance && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.Contragent != null) && (licReissuance.Contragent.Name != null) && (licReissuance.Contragent.Name != ""))
                    {
                        payerName = licReissuance.Contragent.Name;
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Copy && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.Contragent != null) && (licReissuance.Contragent.Name != null) && (licReissuance.Contragent.Name != ""))
                    {
                        payerName = licReissuance.Contragent.Name;
                    }
                }
            }
            if (payerName == "")
            {
                payerName = "-";
            }
            payerName.Trim();
            return payerName;
        }

        private string FindDocNumbers(GisGmp x, bool onlyOne)
        {
            // x.Protocol.DocumentNumber
            var DocumentGjiChildrenRepo = Container.Resolve<IRepository<DocumentGjiChildren>>();
            var DocumentGjiRepo = Container.Resolve<IRepository<DocumentGji>>();
            var ResolutionRepo = Container.Resolve<IRepository<Resolution>>();
            var Protocol197Repo = Container.Resolve<IRepository<Protocol197>>();
            var ResolutionProsecutorRepo = Container.Resolve<IRepository<ResolPros>>();
            var ResolutionRospotrebnadzorRepo = Container.Resolve<IRepository<ResolutionRospotrebnadzor>>();
            var ManOrgLicenseRequestRepo = Container.Resolve<IRepository<ManOrgLicenseRequest>>();
            var LicenseReissuanceRepo = Container.Resolve<IRepository<LicenseReissuance>>();


            string docNums = "";
            if (x.TypeLicenseRequest == TypeLicenseRequest.NotSet && x.Protocol != null)
            {

                var document = DocumentGjiRepo.GetAll()
                    .Where(y => y == x.Protocol)
                    .Select(y => new
                    {
                        y.Id,
                        y.TypeDocumentGji
                    })
                    .FirstOrDefault();
                if (document != null)
                {

                    if (document.TypeDocumentGji == TypeDocumentGji.Protocol ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                        //document.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                        document.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                    {
                        var docChildrenParent = DocumentGjiChildrenRepo.GetAll()
                            .Where(y => y.Parent.Id == document.Id)
                            .Select(y => new
                            {
                                y.Id,
                                y.Children
                            }).FirstOrDefault();
                        if (docChildrenParent != null)
                        {
                            var resolution = ResolutionRepo.GetAll()
                                .Where(y => y == docChildrenParent.Children)
                                .Select(y => new
                                {
                                    y.DocumentNumber,
                                    y.DocumentDate,
                                    y.DecisionDate,
                                    y.DecisionNumber,
                                    y.ExecuteSSPNumber,
                                    y.DateExecuteSSP
                                }).FirstOrDefault();
                            if (resolution != null)
                            {
                                if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                                {
                                    docNums = "Постановление " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                                }
                                if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != "") && !onlyOne)
                                {
                                    docNums = docNums + ", судебное решение " + resolution.DecisionNumber + " от " + resolution.DecisionDate.Value.ToShortDateString();
                                }
                                if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != "") && !onlyOne)
                                {
                                    docNums = docNums + ", ИП " + resolution.ExecuteSSPNumber;// + " от " + resolution.DateExecuteSSP.Value.ToShortDateString();
                                }
                            }

                        }

                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                    {
                        var resolution = ResolutionRepo.GetAll()
                            .Where(y => y.Id == document.Id)
                            .Select(y => new
                            {
                                y.DocumentNumber,
                                y.DocumentDate,
                                y.DecisionDate,
                                y.DecisionNumber,
                                y.ExecuteSSPNumber,
                                y.DateExecuteSSP
                            }).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                            {
                                docNums = "Постановление " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                            }
                            if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != "") && !onlyOne)
                            {
                                docNums = docNums + ", судебное решение " + resolution.DecisionNumber + " от " + resolution.DecisionDate.Value.ToShortDateString();
                            }
                            if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != "") && !onlyOne)
                            {
                                docNums = docNums + ", ИП " + resolution.ExecuteSSPNumber;// + " от " + resolution.DateExecuteSSP.Value.ToShortDateString();
                            }
                        }
                    }
                    else if (document.TypeDocumentGji == TypeDocumentGji.Protocol197)
                    {
                        var resolution = Protocol197Repo.GetAll()
                            .Where(y => y.Id == document.Id)
                            .Select(y => new
                            {
                                y.DocumentNumber,
                                y.DocumentDate
                            }).FirstOrDefault();
                        if (resolution != null)
                        {
                            if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                            {
                                docNums = "Протокол по статье 19.7 " + resolution.DocumentNumber + " от " + resolution.DocumentDate.Value.ToShortDateString();
                            }
                        }
                    }
                }

            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.First && x.ManOrgLicenseRequest != null)
            {
                var manOrgLicReq = ManOrgLicenseRequestRepo.GetAll()
                    .Where(y => y == x.ManOrgLicenseRequest)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.DateRequest
                    }).FirstOrDefault();
                if (manOrgLicReq != null)
                {
                    if ((manOrgLicReq.RegisterNumber != null) && (manOrgLicReq.RegisterNumber != ""))
                    {
                        docNums = "Заявка на лицензию " + manOrgLicReq.RegisterNumber + " от " + manOrgLicReq.DateRequest.Value.ToShortDateString();
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Reissuance && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.ReissuanceDate
                    }).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.RegisterNumber != null) && (licReissuance.RegisterNumber != ""))
                    {
                        docNums = "Заявка на переоформление лицензии " + licReissuance.RegisterNumber + " от " + licReissuance.ReissuanceDate.Value.ToShortDateString();
                    }
                }
            }
            else if (x.TypeLicenseRequest == TypeLicenseRequest.Copy && x.LicenseReissuance != null)
            {
                var licReissuance = LicenseReissuanceRepo.GetAll()
                    .Where(y => y == x.LicenseReissuance)
                    .Select(y => new
                    {
                        y.RegisterNumber,
                        y.ReissuanceDate
                    }).FirstOrDefault();
                if (licReissuance != null)
                {
                    if ((licReissuance.RegisterNumber != null) && (licReissuance.RegisterNumber != ""))
                    {
                        docNums = "Заявка на дубликат лицензии " + licReissuance.RegisterNumber + " от " + licReissuance.ReissuanceDate.Value.ToShortDateString();
                    }
                }
            }
            return docNums;
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
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }
        #endregion

        #region Nested classes
        internal class Identifiers
        {
            internal string SenderIdentifier;
            internal string SenderRole;
            internal string OriginatorID;
        }

        #endregion

        public IDataResult GetListLicRequest(BaseParams baseParams)
        {
            var dataDomain = this.Container.Resolve<IDomainService<ManOrgLicenseRequest>>();
            var loadParams = baseParams.GetLoadParam();

            var data = dataDomain.GetAll()
                .Where(x => !x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    DateRequest = x.DateRequest.HasValue ? x.DateRequest.Value : DateTime.MinValue,
                    x.RegisterNumber,
                    Contragent = x.Contragent.Inn + "; " + x.Contragent.Name
                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult GetListReissuance(BaseParams baseParams)
        {
            var dataDomain = this.Container.Resolve<IDomainService<LicenseReissuance>>();
            var loadParams = baseParams.GetLoadParam();

            var data = dataDomain.GetAll()
                .Where(x => !x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    x.ReissuanceDate,
                    x.RegisterNumber,
                    x.RegisterNum,
                    Contragent = x.Contragent.Inn + "; " + x.Contragent.Name
                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
