namespace Bars.GkhGji.Regions.Tatarstan.Integration.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    using Castle.Windsor;
    using DomainService;
    using Entities;
    using Gkh.Domain;
    using Gkh.Import;
    using Gkh.Utils;
    using GkhGji.Entities;
    using global::Quartz.Util;
    using Newtonsoft.Json;

    using NHibernate.Linq;

    public class GisGmpIntegration : IGisGmpIntegration
    {
        public IWindsorContainer Container { get; set; }

        #region Upload

        private static readonly object UploadSyncLock = new object();

        public IDataResult UploadCharges()
        {
            if (!Monitor.TryEnter(GisGmpIntegration.UploadSyncLock))
            {
                return new BaseDataResult(false, "Отправка начислений уже выполняется");
            }

            var domain = this.Container.ResolveDomain<GisChargeToSend>();
            var documentGjiDomain = this.Container.ResolveDomain<DocumentGji>();

            try
            {
                var config = this.GetConfig();
                var logEnable = config.GetAs<bool>("GisGmpLogEnable");

                //интеграция отключена
                if (!config.GetAs<bool>("GisGmpEnable"))
                {
                    return new BaseDataResult(false, "Отправка начислений отключена");
                }

                var uri = GisGmpIntegration.GetUri(config, "Upload");
                var encoding = new UTF8Encoding();
                var errorsExists = false; // Признак наличия ошибок отправки
                var isSent = false; // Признак успешной отправки хотя бы одного начисления

                using (var webClient = GisGmpIntegration.GetWebClient(config))
                {
                    try
                    {
                        foreach (var charge in this.GetCharges(domain))
                        {
                            var json = this.GetUploadJson(charge, config);
                            var bytes = encoding.GetBytes(json);
                            var response = webClient.UploadData(uri, "POST", bytes);
                            var responseJson = Encoding.UTF8.GetString(response);
                            var messages = logEnable ? new List<string>() : null;
                            var isUpdate = false;

                            try
                            {
                                if (this.TryParseJson(responseJson, out ResponseArrayJson responseObject))
                                {
                                    foreach (var importCharge in responseObject.Response)
                                    {
                                        if (importCharge.Error.IsNotEmpty())
                                        {
                                            messages?.Add(importCharge.Error);
                                            errorsExists = true;
                                        }
                                        else
                                        {
                                            if (importCharge.SupplierBillId.IsNotEmpty())
                                            {
                                                charge.Document.GisUin = importCharge.SupplierBillId;

                                                documentGjiDomain.Update(charge.Document);
                                            }

                                            charge.IsSent = true;
                                            charge.DateSend = DateTime.Now;
                                            isUpdate = true;
                                            isSent = true;
                                        }
                                    }
                                    continue;
                                }

                                if (this.TryParseJson(responseJson, out ResponseStringJson responseStringObject))
                                {
                                    messages?.Add(responseStringObject.Response);
                                    errorsExists = true;
                                    continue;
                                }

                                messages?.Add("Не удалось распознать ответ сервиса");
                                errorsExists = true;
                            }
                            catch (Exception e)
                            {
                                messages?.Add($"При отправке произошла ошибка\r\n{e.Message}");
                                errorsExists = true;
                                throw;
                            }
                            finally
                            {
                                if (logEnable)
                                {
                                    if (!errorsExists) messages?.Add("Успешно отправлено");
                                    messages?.Insert(0, $"Время выполнения: {DateTime.Now:G}");
                                    charge.SendLog = messages.AggregateWithSeparator("\r\n");
                                }

                                if (logEnable || isUpdate)
                                {
                                    domain.Update(charge);
                                }
                            }
                        }

                        if (!errorsExists)
                        {
                            return new BaseDataResult(true);
                        }

                        if (!isSent)
                        {
                            return new BaseDataResult(false, "Ошибка отправки начислений");
                        }

                        return new BaseDataResult(false, "Отправка начислений завершена с ошибками");
                    }
                    catch (WebException e)
                    {
                        return new BaseDataResult(false, e.Message);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(documentGjiDomain);
                Monitor.Exit(GisGmpIntegration.UploadSyncLock);
            }
        }

        private IEnumerable<GisChargeToSend> GetCharges(IDomainService<GisChargeToSend> domain)
        {
            return domain.GetAll()
                .Where(x => !x.IsSent)
                .Fetch(x => x.Document)
                .ToList();
        }

        private string GetUploadJson(GisChargeToSend charge, DynamicDictionary config)
        {
            var systemCode = config.GetAs<string>("GisGmpSystemCode");

            var dummyObject = new
            {
                system_code = systemCode,
                charges = new[]
                {
                    charge.JsonObject
                }
                
            };

            return JsonConvert.SerializeObject(dummyObject);
        }

        private bool TryParseJson<T>(string json, out T obj) where T: class
        {
            var success = false;
            try
            {
                obj = (T) JsonConvert.DeserializeObject(json, typeof(T));
                success = true;
            }
            catch (Exception ex)
            {
                obj = null;
            }

            return success;
        }

        #endregion Upload

        #region Load

        private static readonly object LoadSyncLock = new object();

        public IDataResult LoadPayments()
        {
            if (!Monitor.TryEnter(GisGmpIntegration.LoadSyncLock))
            {
                return new BaseDataResult(false, "Загрузка оплат из ГИС ГМП уже выполняется");
            }

            try
            {
                var config = this.GetConfig();

                //интеграция отключена
                if (!config.GetAs<bool>("GisGmpEnable"))
                {
                    return new BaseDataResult(false, "Загрузка оплат из ГИС ГМП отключена");
                }

                var inn = config.GetAs<string>("GisGmpPayeeInn");
                var kpp = config.GetAs<string>("GisGmpPayeeKpp");

                if (inn.IsEmpty())
                {
                    return new BaseDataResult(false, "Не указан ИНН МЖФ");
                }

                if (kpp.IsEmpty())
                {
                    return new BaseDataResult(false, "Не указан КПП МЖФ");
                }

                var uri = GisGmpIntegration.GetUri(config, "Load");

                // Всегда берем данные с самого начала, потому что начисления могут проводится задним числом
                var dateStart = DateTime.MinValue;

                using (var webClient = GisGmpIntegration.GetWebClient(config))
                {
                    try
                    {
                        var json = GisGmpIntegration.GetLoadJson(config, dateStart, DateTime.Today, inn, kpp);

                        var response = webClient.UploadValues(uri, "POST", new NameValueCollection
                        {
                            { "request", json }
                        });

                        var responseText = Encoding.UTF8.GetString(response);

                        var responseObject = (ResponseJsonLoad)JsonConvert.DeserializeObject(responseText, typeof(ResponseJsonLoad));

                        if (responseObject == null)
                        {
                            return new BaseDataResult(false, "Неверный формат ответа");
                        }

                        if (!responseObject.GetPayments.Data.Error.IsEmpty())
                        {
                            return new BaseDataResult(
                                false,
                                "При загрузке оплат произошла ошибка: " + responseObject.GetPayments.Data.Error);
                        }

                        var payments = (GisPaymentJson[])JsonConvert.DeserializeObject(
                                responseObject.GetPayments.Data.Payments,
                                typeof(GisPaymentJson[]));

                        this.ProcessPayments(payments);
                    }
                    catch (WebException e)
                    {
                        return new BaseDataResult(false, e.Message);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Monitor.Exit(GisGmpIntegration.LoadSyncLock);
            }

            return new BaseDataResult();
        }

        private void ProcessPayments(GisPaymentJson[] payments)
        {
            var logImport = this.Container.Resolve<ILogImport>();
            var logManager = this.Container.Resolve<ILogImportManager>();

            try
            {
                var now = DateTime.Now;

                logManager.FileNameWithoutExtention = string.Format("gis_gmp_load");
                logManager.UploadDate = now;

                logImport.SetFileName("gis_gmp_load.json");
                logImport.ImportKey = "Загрузка оплат из ГИС ГМП";

                var gisPaymentToSave = new List<GisPayment>();
                var itemsToCreate = new List<TatarstanResolutionPayFine>();
                var itemsToUpdate = new List<TatarstanResolutionPayFine>();
                var itemsToDelete = new List<TatarstanResolutionPayFine>();

                var uins = payments.Where(x => x.Uin != null).Select(x => x.Uin).Distinct().ToList();
                var paymentsIds = payments.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();

                var queryResolution = this.Container.ResolveDomain<Resolution>().GetAll()
                    .Where(x => uins.Contains(x.GisUin));

                var payfineDomain = this.Container.ResolveDomain<TatarstanResolutionPayFine>();

                var cachePayfine = payfineDomain.GetAll()
                    .Where(y => queryResolution.Any(x => x.Id == y.Resolution.Id))
                    .Where(x => x.DocumentNum != null)
                    .Where(x => paymentsIds.Contains(x.DocumentNum))
                    .GroupBy(x => x.DocumentNum)
                    .ToDictionary(x => x.Key, y => y.First());

                var cacheResolutions = queryResolution
                    .GroupBy(x => x.GisUin)
                    .ToDictionary(x => x.Key, y => y.First());

                foreach (var payment in payments)
                {
                    this.ProcessOnePayment(
                        payment,
                        logImport,
                        cachePayfine,
                        cacheResolutions,
                        itemsToCreate,
                        itemsToUpdate,
                        itemsToDelete,
                        gisPaymentToSave);
                }

                var gisPaymentDomain = this.Container.ResolveDomain<GisPayment>();

                this.Container.InTransaction(() =>
                {
                    GisGmpIntegration.Save(payfineDomain, itemsToCreate);
                    GisGmpIntegration.Update(payfineDomain, itemsToUpdate);
                    GisGmpIntegration.Delete(payfineDomain, itemsToDelete);

                    GisGmpIntegration.Save(gisPaymentDomain, gisPaymentToSave);
                });
            }
            catch(Exception e)
            {
                logImport.Error("Ошибка сохранения", e.Message);
            }
            finally
            {
                logManager.Add(new MemoryStream(
                    Encoding.GetEncoding(1251).GetBytes(JsonConvert.SerializeObject(payments))),
                    "gis_gmp_load.json",
                    logImport);
                logManager.Save();

                this.Container.Release(logManager);
                this.Container.Release(logImport);
            }
        }

        private void ProcessOnePayment(
            GisPaymentJson payment,
            ILogImport logImport,
            Dictionary<string, TatarstanResolutionPayFine> cachePayfine,
            Dictionary<string, Resolution> cacheResolutions,
            List<TatarstanResolutionPayFine> itemsToCreate,
            List<TatarstanResolutionPayFine> itemsToUpdate,
            List<TatarstanResolutionPayFine> itemsToDelete,
            List<GisPayment> gisPaymentToSave)
        {
            var uip = payment.Uip;
            var paymentId = payment.PaymentId;
            var now = DateTime.Now;

            if (uip.IsEmpty())
            {
                logImport.Warn(
                    "Оплата за {0} на сумму {1}".FormatUsing(
                        payment.PaymentDate.ToDateTime(),
                        payment.Amount.ToDecimal()),
                    "Не указан УИП");
                return;
            }

            if (paymentId.IsEmpty())
            {
                logImport.Warn(
                    "Оплата за {0} на сумму {1}".FormatUsing(
                        payment.PaymentDate.ToDateTime(),
                        payment.Amount.ToDecimal()),
                    "Не указан уникальный идентификатор платежа");
                return;
            }

            // Платеж анулирован, нужно удалять
            if (payment.ChangeStatus == "3")
            {
                var payfine = cachePayfine.Get(paymentId);

                if (payfine == null)
                {
                    logImport.Info("Оплата УИП: {0}".FormatUsing(uip), "Оплата не существует");
                    return;
                }

                itemsToDelete.Add(payfine);

                logImport.CountChangedRows++;
            }
            else
            {
                var payfine = cachePayfine.Get(paymentId);

                // Cоздаем
                if (payfine == null)
                {
                    if (payment.Uin.IsEmpty())
                    {
                        logImport.Warn("Оплата УИП: {0}".FormatUsing(uip), "Не указан УИН");
                        return;
                    }

                    var resolution = cacheResolutions.Get(payment.Uin);

                    if (resolution == null)
                    {
                        logImport.Warn(
                            "Оплата УИП: {0}".FormatUsing(uip),
                            "Не удалось получить постановление с УИН {0}".FormatUsing(payment.Uin));
                        return;
                    }

                    payfine = new TatarstanResolutionPayFine
                    {
                        Amount = payment.Amount.ToDecimal(),
                        Resolution = resolution,
                        DocumentDate = payment.PaymentDate.ToDateTime(),
                        GisUip = uip,
                        TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp,
                        DocumentNum = payment.PaymentId,
                        AdmissionType = AdmissionType.RecievedFromGisGmp
                    };

                    var description = "Добавлена оплата для постановления {0} от {1} на сумму {2}".FormatUsing(
                        resolution.DocumentNumber,
                        resolution.DocumentDate.ToDateString(),
                        payment.Amount);
                    logImport.Info("Оплата УИП: {0}".FormatUsing(uip), description);

                    itemsToCreate.Add(payfine);
                    logImport.CountAddedRows++;
                }
                else
                {
                    payfine.Amount = payment.Amount.ToDecimal();
                    payfine.DocumentDate = payment.PaymentDate.ToDateTime();
                    payfine.TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp;
                    payfine.DocumentNum = payment.PaymentId;

                    var description = "Добавлена оплата для постановления {0} от {1} на сумму {2}".FormatUsing(
                        payfine.Resolution.DocumentNumber,
                        payfine.Resolution.DocumentDate.ToDateString(),
                        payment.Amount);
                    logImport.Info("Оплата УИП: {0}".FormatUsing(uip), description);

                    itemsToUpdate.Add(payfine);
                    logImport.CountChangedRows++;
                }

                logImport.Info("Оплата УИП: {0}".FormatUsing(uip), "Добавлена информация о загруженной оплате");

                gisPaymentToSave.Add(new GisPayment
                {
                    JsonObject = payment,
                    Uip = payment.Uip,
                    DateRecieve = now,
                    PayFine = payfine
                });

                logImport.CountAddedRows++;
            }
        }

        private static void Save<T>(IDomainService<T> domain, IEnumerable<T> list) where T: IEntity
        {
            foreach (var item in list)
            {
                domain.Save(item);
            }
        }

        private static void Update<T>(IDomainService<T> domain, IEnumerable<T> list) where T : IEntity
        {
            foreach (var item in list)
            {
                domain.Update(item);
            }
        }

        private static void Delete<T>(IDomainService<T> domain, IEnumerable<T> list) where T : IEntity
        {
            foreach (var item in list)
            {
                domain.Delete(item.Id);
            }
        }

        private static string GetLoadJson(DynamicDictionary config, DateTime dateBegin, DateTime dateEnd, string inn, string kpp)
        {
            var systemCode = config.GetAs<string>("GisGmpSystemCode");

            var dummyObject = new
            {
                get_payments = new
                {
                    @params = new
                    {
                        json = new
                        {
                            system_code = systemCode,
                            date_begin = dateBegin.ToShortDateString(),
                            date_end = dateEnd.ToShortDateString(),
                            payee = new
                            {
                                payee_inn = inn,
                                payee_kpp = kpp
                            }
                        }
                    },
                    type = "Action"
                }
            };

            return JsonConvert.SerializeObject(dummyObject);
        }

        #endregion Load
        
        private static WebClient GetWebClient(DynamicDictionary config)
        {
            var wc = new WebClient();

            var proxy = config.GetAs<string>("GisGmpProxy");

            if (!proxy.IsNullOrWhiteSpace())
            {
                wc.Proxy = new WebProxy(new Uri(proxy, UriKind.RelativeOrAbsolute));

                var proxyUser = config.GetAs<string>("GisGmpProxyUser");
                var proxyPassword = config.GetAs<string>("GisGmpProxyPassword");

                if (!proxyUser.IsNullOrWhiteSpace() && !proxyPassword.IsNullOrWhiteSpace())
                {
                    wc.Proxy.Credentials = new NetworkCredential(proxyUser, proxyPassword);
                }
            }

            return wc;
        }

        private static Uri GetUri(DynamicDictionary config, string method)
        {
            var uri = config.GetAs<string>("GisGmpUri" + method);

            if (uri.IsEmpty())
            {
                throw new ArgumentNullException("GisGmpUri" + method, @"Не указан адрес");
            }

            uri = Uri.UnescapeDataString(uri);

            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }

        private DynamicDictionary GetConfig()
        {
            return this.Container.Resolve<IGjiTatParamService>().GetConfig();
        }

        private class ResponseJson
        {
            [JsonProperty("status")]
            public string Status { get; set; }
        }

        private class ResponseArrayJson : ResponseJson
        {
            [JsonProperty("response")]
            public ResponseJsonResult[] Response { get; set; }
        }

        private class ResponseStringJson : ResponseJson
        {
            [JsonProperty("response")]
            public string Response { get; set; }
        }

        /// <summary>
        /// Контейнер "result"
        /// </summary>
        private class ResponseJsonResult
        {
            [JsonProperty("supplier_billd")]
            public string SupplierBillId { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }

        #region Payments JSON Response

        private class ResponseJsonLoad
        {
             [JsonProperty("get_payments")]
            public ResponseJsonGetPayments GetPayments { get; set; }
        }

        private class ResponseJsonGetPayments
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("data")]
            public ResponseJsonPayData Data { get; set; }
        }

        private class ResponseJsonPayData
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("payments")]
            public string Payments { get; set; }
        }
        #endregion
    }
}