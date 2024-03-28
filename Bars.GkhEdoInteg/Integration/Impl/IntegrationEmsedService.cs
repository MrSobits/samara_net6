namespace Bars.GkhEdoInteg
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using B4;
    using B4.Config;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;
    using Bars.GkhEdoInteg.Configuration;
    using Bars.GkhEdoInteg.Extensions;
    using Gkh.Entities;
    using Entities;
    using Enums;
    using Serialization;
    using GkhGji.Entities;
    using GkhGji.Enums;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using NHibernate.Linq;

    /// <summary>
    /// Получение документов из системы и отправка в электронный документооборот
    /// </summary>
    public class IntegrationEmsedService : IEmsedService
    {
        private AppConfig config;
        private LogRequests logRequests;
        private const int DefaultBatchSize = 1000;

        private int countAdded;
        private int countUpdated;

        private int countToAdd;
        private int countToUpdate;

        private readonly List<AppealCits> appealCitsToSave = new List<AppealCits>(IntegrationEmsedService.DefaultBatchSize / 2);
        private readonly List<AppealCitsSource> appealCitsSourceToSave = new List<AppealCitsSource>(IntegrationEmsedService.DefaultBatchSize / 2);
        private readonly List<AppealCitsCompareEdo> appealCitsCompareEdoToSave = new List<AppealCitsCompareEdo>(IntegrationEmsedService.DefaultBatchSize / 2);
        private readonly List<LogRequestsAppCitsEdo> logRequestsAppCitsEdoToSave = new List<LogRequestsAppCitsEdo>(IntegrationEmsedService.DefaultBatchSize / 2);

        /// <summary>
        /// Количество дней, по сколько выгружаем из системы ЭДО
        /// </summary>
        private const int DaysToLoad = 10;

        private IDomainService<DisposalAnnex> servDisposalAnnex;

        private IDomainService<ActCheckAnnex> servActCheckAnnex;

        private IDomainService<ActSurveyAnnex> servActSurveyAnnex;

        private IDomainService<ResolutionAnnex> servResolutionAnnex;

        private IDomainService<PrescriptionAnnex> servPrescriptionAnnex;

        private IDomainService<ProtocolAnnex> servProtocolAnnex;

        public IWindsorContainer Container { get; set; }

        public IDomainService<LogRequests> LogRequestsDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsCompareEdo> AppealCitsCompareEdoDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<LogRequestsAppCitsEdo> LogRequestsAppCitsEdoDomain { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public ILogger LogManager { get; set; }

        /// <summary>
        /// Источник поступления
        /// </summary>
        private Dictionary<int, RevenueSourceCompareEdo> revenueSourceCache;

        /// <summary>
        /// Форма поступления
        /// </summary>
        private Dictionary<int, RevenueFormCompareEdo> revenueFormCache;

        /// <summary>
        /// Виды обращений
        /// </summary>
        private Dictionary<int, KindStatementCompareEdo> kindStatementCache;

        /// <summary>
        /// Инспектора
        /// </summary>
        private Dictionary<int, InspectorCompareEdo> inspectorCache;

        /// <summary>
        /// Сопоставление обращения граждан с интеграцией Эдо
        /// </summary>
        private Dictionary<int, AppealCitsCompareEdo> appealCitsCompareEdoCache;

        /// <summary>
        /// Отправка выбранных документов в электронный документооборот
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Send(BaseParams baseParams, out string msg)
        {
            msg = string.Empty;
            this.servDisposalAnnex = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            this.servActCheckAnnex = this.Container.Resolve<IDomainService<ActCheckAnnex>>();
            this.servActSurveyAnnex = this.Container.Resolve<IDomainService<ActSurveyAnnex>>();
            this.servResolutionAnnex = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            this.servPrescriptionAnnex = this.Container.Resolve<IDomainService<PrescriptionAnnex>>();
            this.servProtocolAnnex = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            this.config = this.Container.Resolve<IConfigProvider>().GetConfig();

            using (var client = this.GetClient())
            {
                var dnsid = this.GetDnsid(ref msg, client, "SendEdoDnsid");
                if (dnsid == null)
                {
                    return false;
                }

                var appealCitsId = baseParams.Params.ContainsKey("appealCitsId") ? baseParams.Params["appealCitsId"].ToLong() : 0;
                if (appealCitsId == 0)
                {
                    this.LogManager.LogError(new Exception(), "Отправка данных в ЭДО. Идентификатор обращения граждан отсутствует");
                    return false;
                }

                var appealCits = this.AppealCitsDomain.Get(appealCitsId);
                if (appealCits == null)
                {
                    this.LogManager.LogError(new Exception(), "Отправка данных в ЭДО. Обращение граждан не найдено");
                    return false;
                }

                var appealCitsCompareEdo = this.AppealCitsCompareEdoDomain.GetAll().FirstOrDefault(x => x.AppealCits.Id == appealCitsId);
                if (appealCitsCompareEdo == null)
                {
                    msg = "Обращение не содержит идентификатор из ЭДО, отправка не возможна";
                    return false;
                }

                var dictionary = baseParams.Params.ContainsKey("docs") ? (baseParams.Params["docs"] as List<object>)
                        .Select(x => x as DynamicDictionary)
                        .ToList() : null;

                if (dictionary == null || dictionary.Count == 0)
                {
                    msg = "Не выбраны документы";
                    return false;
                }

                var surety = string.Empty;
                var suretyId = 0;
                var verifier = string.Empty;
                var verifierId = 0;
                if (appealCitsCompareEdo.AppealCits != null)
                {
                    surety = appealCitsCompareEdo.AppealCits.Surety != null ? appealCitsCompareEdo.AppealCits.Surety.Fio : string.Empty;
                    suretyId = this.GetCodeEdoInspector(appealCitsCompareEdo.AppealCits.Surety);
                    verifier = appealCitsCompareEdo.AppealCits.Tester != null ? appealCitsCompareEdo.AppealCits.Tester.Fio : string.Empty;
                    verifierId = this.GetCodeEdoInspector(appealCitsCompareEdo.AppealCits.Tester);
                }

                var info = new SendEdo
                {
                    Id = appealCitsCompareEdo.CodeEdo,
                    Number = appealCits.NumberGji,
                    Surety = surety,
                    SuretyId = suretyId,
                    Verifier = verifier,
                    VerifierId = verifierId
                };

                var serAppealCitsAnswer = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();

                var i = 0;
                var d = 0;
                foreach (var val in dictionary)
                {
                    var id = val["Id"].ToLong();
                    var type = val["Type"].ToLong();

                    if (type == 0)
                    {
                        var answer = serAppealCitsAnswer.Get(id);

                        var inspector = string.Empty;
                        var inspectorId = 0;
                        if (answer.Executor != null)
                        {
                            var inspectorCompareEdo = this.Container.Resolve<IDomainService<InspectorCompareEdo>>().GetAll()
                                         .FirstOrDefault(x => x.Inspector.Id == answer.Executor.Id);
                            if (inspectorCompareEdo != null)
                            {
                                inspector = answer.Executor != null ? answer.Executor.Fio : string.Empty;
                                inspectorId = inspectorCompareEdo.CodeEdo;
                            }
                        }

                        info.Answers.Add(
                            i.ToStr(),
                            new Answer
                            {
                                Id = answer.File.Id,
                                ContentAnswer = answer.AnswerContent != null ? answer.AnswerContent.Name : string.Empty,
                                Date = answer.DocumentDate.HasValue ? answer.DocumentDate.Value.ToShortDateString() : DateTime.MinValue.ToShortDateString(),
                                Description = answer.Description,
                                Inspector = inspector,
                                InspectorId = inspectorId,
                                Name = answer.DocumentName,
                                Number = answer.DocumentNumber
                            });
                        i++;
                    }
                    else
                    {
                        var docs = this.GetDocuments(id, type);
                        foreach (var document in docs)
                        {
                            info.Documents.Add(d.ToStr(), document);
                            d++;
                        }
                    }
                }

                var json = JsonConvert.SerializeObject(info);
                try
                {
                    var sendEdoUri = this.GetUri("SendEdo");
                    var url = new Uri(string.Format(sendEdoUri, dnsid));
                    client.UploadString(url, json);
                }
                catch (Exception exp)
                {
                    this.LogManager.LogError(exp, "Отправка данных в ЭДО. Не удалось отправить данные");
                    msg = "Не удалось отправить данные в систему ЕМСЭД";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Получение данных по обращениям с ЭДО
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool LoadData(DynamicDictionary dict, out string message)
        {
            message = string.Empty;
            this.logRequests = new LogRequests { TimeExecution = 0 };

            var actualDate = this.GetDateActual(dict);
            var untilDate = this.GetDateUntil(dict);

            var sw = new Stopwatch();
            sw.Start();
            var data = this.RequestEdo(actualDate, ref message, untilDate);
            if (data == null || (!string.IsNullOrEmpty(data.Errors?.ErrCode) && !string.IsNullOrEmpty(data.Errors?.ErrMessage)))
            {
                this.LogManager.LogError($"Интеграция Эдо. {message}");
                return false;
            }

            this.countAdded = 0;
            this.countUpdated = 0;

            var documentCount = data.Data.Documents.Values.Count;
            var skip = 0;
            try
            {
                this.LogRequestsDomain.Save(this.logRequests);

                this.InitLoadDataCache(data.Data.Documents.Values.Select(x => x.Id));

                while (skip < documentCount)
                {
                    var skipValues = skip;
                    this.Container.InTransaction(() =>
                    {
                        this.countToAdd = 0;
                        this.countToUpdate = 0;
                        foreach (var doc in data.Data.Documents.Values.Skip(skipValues).Take(IntegrationEmsedService.DefaultBatchSize))
                        {
                            this.LoadAppealCits(doc);
                        }
                        this.SaveCache();
                    });

                    this.countAdded += this.countToAdd;
                    this.countUpdated += this.countToUpdate;

                    this.logRequests.Count = documentCount;
                    this.logRequests.CountAdded = this.countAdded;
                    this.logRequests.CountUpdated = this.countUpdated;
                    this.logRequests.DateEnd = DateTime.Now;
                    this.logRequests.TimeExecution = (sw.ElapsedMilliseconds / 1000m).RoundDecimal(2);

                    this.LogRequestsDomain.Update(this.logRequests);
                    skip += IntegrationEmsedService.DefaultBatchSize;
                }
                sw.Stop();
            }
            catch (Exception exp)
            {
                message = $"Ошибка при сохранении обращений. Получено обращений: {documentCount}; "
                    + $"Добавлено: {this.countAdded}; Обновлено {this.countUpdated}; Время запроса: {sw.Elapsed}";
                this.LogManager.LogError($"Интеграция Эдо. {message}", exp);
                return false;
            }

            message = $"Получено обращений: {documentCount}; Добавлено: {this.countAdded}; Обновлено {this.countUpdated}; Время запроса: {sw.Elapsed}";
            return true;
        }

        private void InitLoadDataCache(IEnumerable<int> docIds)
        {
            var revenueSourceCompareEdo = this.Container.Resolve<IDomainService<RevenueSourceCompareEdo>>();
            var revenueFormCompareEdo = this.Container.Resolve<IDomainService<RevenueFormCompareEdo>>();
            var kindStatementCompareEdo = this.Container.Resolve<IDomainService<KindStatementCompareEdo>>();
            var inspectorCompareEdo = this.Container.Resolve<IDomainService<InspectorCompareEdo>>();

            try
            {
                this.revenueSourceCache = revenueSourceCompareEdo.GetAll()
                    .Where(x => x.CodeEdo > 0)
                    .Fetch(x => x.RevenueSource)
                    .ToDictionary(x => x.CodeEdo);

                this.revenueFormCache = revenueFormCompareEdo.GetAll()
                    .Where(x => x.CodeEdo > 0)
                    .Fetch(x => x.RevenueForm)
                    .ToDictionary(x => x.CodeEdo);

                this.kindStatementCache = kindStatementCompareEdo.GetAll()
                    .Where(x => x.CodeEdo > 0)
                    .Fetch(x => x.KindStatement)
                    .ToDictionary(x => x.CodeEdo);

                this.inspectorCache = inspectorCompareEdo.GetAll()
                    .Where(x => x.CodeEdo > 0)
                    .Fetch(x => x.Inspector)
                    .ToDictionary(x => x.CodeEdo);

                this.appealCitsCompareEdoCache = this.AppealCitsCompareEdoDomain.GetAll()
                    .WhereContainsBulked(x => x.CodeEdo, docIds, 2500)
                    .Fetch(x => x.AppealCits)
                    .AsEnumerable()
                    .GroupBy(x => x.CodeEdo)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());
            }
            finally
            {
                this.Container.Release(revenueSourceCompareEdo);
                this.Container.Release(revenueFormCompareEdo);
                this.Container.Release(kindStatementCompareEdo);
                this.Container.Release(inspectorCompareEdo);
            }
        }

        /// <summary>
        /// Загрузка сканов обращений граждан в систему
        /// </summary>
        /// <param name="dict">
        /// The dict.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool LoadDocuments(DynamicDictionary dict, out string message)
        {
            this.logRequests = new LogRequests();
            message = string.Empty;
            this.config = this.Container.Resolve<IConfigProvider>().GetConfig();

            var documents = this.AppealCitsCompareEdoDomain.GetAll()
                .Where(x => (x.DateDocLoad == null || x.DateDocLoad <= DateTime.MinValue) && x.IsDocEdo && x.CountLoadDoc < 5 && x.AppealCits.File == null)
                .OrderBy(x => x.DateActual)
                .ToList();

            if (documents.Count == 0)
            {
                message = "Отсутствуют актуальные данных для загрузки документов";
                return true;
            }

            using (var client = this.GetClient())
            {
                var date = documents.SafeMin(x => x.DateActual);
                if (!date.HasValue || date.Value == DateTime.MinValue)
                {
                    return true;
                }

                var handDate = dict.GetAs<DateTime?>("dateActual");

                var dateToStartLoad = handDate ?? date.Value;
                var dnsid = this.GetDnsid(ref message, client, "SendEdoDnsid");

                if (string.IsNullOrEmpty(dnsid))
                {
                    return false;
                }

                var fileManager = this.Container.Resolve<IFileManager>();

                var edoUriFile = this.GetUri("EdoUriFile");
                try
                {
                    for (var curStartDay = dateToStartLoad; curStartDay < DateTime.Now; curStartDay = curStartDay.AddDays(IntegrationEmsedService.DaysToLoad))
                    {
                        var dateActual = this.GetDateInFormat(curStartDay);
                        var dateUntil = this.GetDateInFormat(curStartDay.AddDays(IntegrationEmsedService.DaysToLoad));

                        var url = string.Format(this.GetUri("EdoUriAppealCitsUntilDate"), dnsid, dateActual, dateUntil);
                        var uriAppealCits = new Uri(url, UriKind.RelativeOrAbsolute);

                        byte[] bytes;
                        try
                        {
                            bytes = client.DownloadData(uriAppealCits);
                        }
                        catch (Exception exp)
                        {
                            var errorMsg =
                                $"Интеграция с ЭДО. Не удалось получить данные за период с {curStartDay} по {curStartDay.AddDays(IntegrationEmsedService.DaysToLoad)} при получении файлов";

                            this.LogManager.LogError(errorMsg, exp);
                            message = "Не удалось получить данные для получения документов";
                            return false;
                        }

                        var data = this.SerealizeData(ref message, bytes);
                        if (data == null)
                        {
                            return false;
                        }

                        var dictDocuments = data.Data.Documents.Values.Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    DocumentId = x.Attachments != null ? x.Attachments.FileId : string.Empty
                                })
                                    .GroupBy(x => x.Id)
                                    .ToDictionary(x => x.Key, x => x.Select(y => y.DocumentId).FirstOrDefault());

                        foreach (var appealCitsCompareEdo in documents.Where(x => dictDocuments.ContainsKey(x.CodeEdo)))
                        {
                            this.LoadDocument(dictDocuments, ref message, edoUriFile, dnsid, appealCitsCompareEdo, client, fileManager);
                        }
                    }
                }
                finally
                {
                    this.Container.Release(fileManager);
                }
            }

            return true;
        }

        /// <summary>
        /// Формирование номера ГЖИ (Номера через запятую)
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="inNum"></param>
        /// <returns></returns>
        private static string GetNumber(string numbers, string inNum)
        {
            if (string.IsNullOrEmpty(numbers))
            {
                return inNum;
            }

            if (numbers.Contains(","))
            {
                return numbers.Split(',').Contains(inNum) ? numbers : $"{numbers},{inNum}";
            }

            return numbers.Trim() == inNum.Trim() ? numbers : $"{numbers},{inNum}";
        }

        private static string FormatAddress(Correspondent x)
        {
            var district = string.IsNullOrEmpty(x.Address.District)
                ? string.Empty
                : $"р-н {x.Address.District}";
            var building = string.IsNullOrEmpty(x.Address.Building)
                ? string.Empty
                : $"корп{x.Address.Building}";
            return $"{x.Address.City} {district} ул.{x.Address.Street} д.{x.Address.House} {building} кв.{x.Address.Apartment}";
        }

        private AnswerEdo RequestEdo(DateTime actualDate, ref string message, DateTime? untilDate = null)
        {
            this.logRequests.DateStart = actualDate;

            this.config = this.Container.Resolve<IConfigProvider>().GetConfig();
            byte[] bytes;
            using (var client = this.GetClient())
            {
                var dnsid = this.GetDnsid(ref message, client, "EdoDnsid");
                if (string.IsNullOrEmpty(dnsid))
                {
                    return null;
                }

                var uriStr = untilDate.HasValue
                    ? string.Format(this.GetUri("EdoUriAppealCitsUntilDate"), dnsid, this.GetDateInFormat(actualDate), this.GetDateInFormat(untilDate.Value))
                    : string.Format(this.GetUri("EdoUriAppealCits"), dnsid, this.GetDateInFormat(actualDate));
                this.logRequests.Uri = uriStr;

                var uriAppealCits = new Uri(uriStr, UriKind.RelativeOrAbsolute);
                try
                {
                    bytes = client.DownloadData(uriAppealCits);
                }
                catch (Exception exp)
                {
                    this.LogManager.LogError(exp, "Интеграция с ЭДО. Не удалось получить данные");
                    message = $"Не удалось получить данные. {exp.Message}";
                    return null;
                }
            }

            // Сериализуем пришедшие данные
            var answerEdo = this.SerealizeData(ref message, bytes);
            try
            {
                if (answerEdo != null)
                {
                    using (var memoryStream = new MemoryStream(bytes))
                    {
                        var fileName = this.GetFileName(actualDate, untilDate ?? DateTime.Now);
                        var fileInfo = this.Container.Resolve<IFileManager>().SaveFile(memoryStream, fileName);
                        this.logRequests.File = fileInfo;
                    }
                }
            }
            catch (Exception exp)
            {
                this.LogManager.LogError(exp, "Интеграция с ЭДО. Не удалось сохранить JSON для лога");
            }

            return answerEdo;
        }

        private string GetFileName(DateTime actualDate, DateTime untilDate)
        {
            return $"IntegEdo{actualDate.ToShortDateString()}{actualDate.Hour}.{actualDate.Minute}.{actualDate.Second} - "
                + $"{untilDate.ToShortDateString()}{untilDate.Hour}.{untilDate.Minute}.{untilDate.Second}.txt";
        }

        private void LoadDocument(
            Dictionary<int, string> dictDocs,
            ref string message,
            string edoUriFile,
            string dnsid,
            AppealCitsCompareEdo appealCitsCompareEdo,
            WebClient client,
            IFileManager fileManager)
        {
            var documentEdoId = dictDocs.Get(appealCitsCompareEdo.CodeEdo);
            if (string.IsNullOrEmpty(documentEdoId))
            {
                return;
            }

            var appealCits = appealCitsCompareEdo.AppealCits;

            var uriDoc = new Uri(string.Format(edoUriFile, dnsid, documentEdoId), UriKind.RelativeOrAbsolute);
            byte[] fileData;
            try
            {
                fileData = client.DownloadData(uriDoc);
            }
            catch (Exception exp)
            {
                var msg = $"При получении документа {appealCits.NumberGji}. Сервис ЭДО вернул ошибку {exp.Message}";

                appealCitsCompareEdo.MsgLoadDoc = msg;
                appealCitsCompareEdo.CountLoadDoc++;
                this.AppealCitsCompareEdoDomain.Update(appealCitsCompareEdo);

                message += msg;
                return;
            }

            try
            {
                var fileName = $"Обращение_{appealCits.NumberGji}.pdf";
                var fileInfo = fileManager.SaveFile(new MemoryStream(fileData), fileName);

                appealCits.File = fileInfo;
                this.AppealCitsDomain.Update(appealCits);

                appealCitsCompareEdo.DateDocLoad = DateTime.Now;
                this.AppealCitsCompareEdoDomain.Update(appealCitsCompareEdo);
            }
            catch (OutOfMemoryException exp)
            {
                message += $"Нехватка памяти для загрузки документа {appealCits.NumberGji};";
                this.LogManager.LogError(exp, message);
            }
            catch (Exception exp)
            {
                message += $"Ошибка загрузки документа для {appealCits.NumberGji}. {exp.Message};";
                this.LogManager.LogError(exp, $"Интеграция с ЭДО. {message}");
            }
        }

        /// <summary>
        /// Получение идентификатора сессии ЭДО
        /// </summary>
        /// <param name="msg">
        /// Сообщение об ошибке
        /// </param>
        /// <param name="client">
        /// Веб клиент
        /// </param>
        /// <param name="key">
        /// Ключ Uri для получения Dnsid
        /// </param>
        /// <returns>
        /// Dnsid 
        /// </returns>
        private string GetDnsid(ref string msg, WebClient client, string key)
        {
            // На случай если сертификат самопальный или кривой
            ServicePointManager.ServerCertificateValidationCallback +=
              (sender1, certificate, chain, sslPolicyErrors) => true;

            try
            {
                if (string.IsNullOrEmpty(EdoModuleConfiguration.UserId) || string.IsNullOrEmpty(EdoModuleConfiguration.PasswordHash))
                {
                    msg = "Не удалось авторизоватся в системе ЕМСЭД. Отсутвуют данные для авторизации";
                    return null;
                }
                
                var requestData = new NameValueCollection
                {
                    ["api"] = EdoModuleConfiguration.ApiVersion,
                    ["user_id"] = EdoModuleConfiguration.UserId,
                    ["password"] = EdoModuleConfiguration.PasswordHash
                };

                var uri = this.GetUri(key);
                
                var data = client.UploadValues(new Uri(uri, UriKind.RelativeOrAbsolute), requestData);
                
                var response = JsonConvert.DeserializeObject<DnsidResponse>(client.Encoding.GetString(data));
                
                if (response.Errors.IsNotEmpty())
                {
                    var edoMsg = response.Errors.Length > 0 ? string.Join(", ", response.Errors) : string.Empty;
                    msg = $"Не удалось авторизоватся в системе ЕМСЭД. {edoMsg}";
                }

                return response.Dnsid;
            }
            catch (Exception exp)
            {
                this.LogManager.LogError(exp, "Отправка данных в ЭДО. Не удалось получить DNSID");
                msg = "Не удалось авторизоватся в системе ЕМСЭД";
                return null;
            }
        }

        /// <summary>
        /// Получение URI сервисов ЭДО
        /// </summary>
        /// <param name="key">Ключ URI</param>
        /// <returns>Строка запроса</returns>
        private string GetUri(string key)
        {
            if (this.config.AppSettings.ContainsKey(key))
            {
                return this.config.AppSettings[key].ToStr();
            }

            switch (key)
            {
                case "EdoDnsid":
                case "SendEdoDnsid":
                    return $"https://doc.tatar.ru/login.php";
                case "EdoUriFile": return "https://doc.tatar.ru/public/api/mgf/get_pages3?DNSID={0}&file_id={1}";
                case "EdoUriAppealCits": return "https://doc.tatar.ru/public/api/mgf/get_documents2?DNSID={0}&date={1}";
                case "EdoUriAppealCitsUntilDate": return "https://doc.tatar.ru/public/api/mgf/get_documents2?DNSID={0}&date={1}&until={2}";
                case "SendEdo": return "https://doc.tatar.ru/public/api/mgf/create_document/?DNSID={0}";
            }

            if (this.config != null && this.config.AppSettings.ContainsKey(key))
            {
                return this.config.AppSettings[key].ToStr();
            }

            return null;
        }

        private WebClient GetClient()
        {
            // Используем переопределённый WebClient, чтобы использовать cookie, полученные из ответа при авторизации 
            // (см. GKH-7427 для получения информации по запросам)
            var client = new CookieAwareWebClient(EdoModuleConfiguration.RequestTimeout);

            if (this.config.AppSettings.ContainsKey("UseProxy")
                && this.config.AppSettings.ContainsKey("IpProxy") && this.config.AppSettings.ContainsKey("PortProxy")
                && this.config.AppSettings.ContainsKey("UserNameProxy") && this.config.AppSettings.ContainsKey("PasswordProxy"))
            {
                if (this.config.AppSettings["UseProxy"].ToBool())
                {
                    var proxy = new WebProxy(this.config.AppSettings["IpProxy"].ToStr(), this.config.AppSettings["PortProxy"].ToInt())
                                    {
                                        Credentials =
                                            new NetworkCredential
                                                {
                                                    UserName = this.config.AppSettings["UserNameProxy"].ToStr(),
                                                    Password = this.config.AppSettings["PasswordProxy"].ToStr()
                                                }
                                    };
                    client.Proxy = proxy;
                }
            }

            return client;
        }

        /// <summary>
        /// Получение идентификатора ЭДО Инспектора
        /// </summary>
        /// <param name="inspector"></param>
        /// <returns></returns>
        private int GetCodeEdoInspector(Inspector inspector)
        {
            if (inspector == null)
            {
                return 0;
            }

            var insp = this.Container.Resolve<IDomainService<InspectorCompareEdo>>().GetAll()
                                .FirstOrDefault(x => x.Inspector.Id == inspector.Id);
            return insp != null ? insp.CodeEdo : 0;
        }

        /// <summary>
        /// Возвращение идентификаторов выбранных документов
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<Document> GetDocuments(long id, long type)
        {
            var documents = new List<Document>();
            switch ((TypeDocumentGji)type)
            {
                case TypeDocumentGji.Disposal:
                    documents.AddRange(this.servDisposalAnnex.GetAll().Where(x => x.Disposal.Id == id).Select(document => new Document { Id = document.File.Id }));

                    break;
                case TypeDocumentGji.ActCheck:
                    var doc = this.servActCheckAnnex.GetAll().Where(x => x.Id == id).Select(x => new Document { Id = x.File.Id });
                    documents.AddRange(doc.ToList());
                    break;
                case TypeDocumentGji.ActRemoval:

                    break;
                case TypeDocumentGji.ActSurvey:

                    documents.AddRange(this.servActSurveyAnnex.GetAll().Where(x => x.Id == id).Select(document => new Document { Id = document.File.Id }));
                    break;
                case TypeDocumentGji.Resolution:
                    documents.AddRange(this.servResolutionAnnex.GetAll().Where(x => x.Id == id).Select(document => new Document { Id = document.File.Id }));
                    break;

                case TypeDocumentGji.Prescription:
                    documents.AddRange(this.servPrescriptionAnnex.GetAll().Where(x => x.Id == id).Select(document => new Document { Id = document.File.Id }));
                    break;

                case TypeDocumentGji.Protocol:
                    documents.AddRange(this.servProtocolAnnex.GetAll().Where(x => x.Id == id).Select(document => new Document { Id = document.File.Id }));
                    break;
            }

            return documents;
        }

        private AnswerEdo SerealizeData(ref string message, byte[] bytes)
        {
            AnswerEdo data;
            try
            {
                var dataString = Encoding.UTF8.GetString(bytes);
                data = JsonConvert.DeserializeObject<AnswerEdo>(dataString);
            }
            catch (Exception exp)
            {
                this.LogManager.LogError(exp, "Интеграция с ЭДО. Ошибка сериализации данных");
                message = "Ошибка сериализации данных";
                return null;
            }

            if (data.Data.Documents == null || data.Data.Documents.Values.Count == 0)
            {
                message = "Сервис не вернул данные";
                return null;
            }

            return data;
        }

        private DateTime GetDateActual(DynamicDictionary dynamicParams)
        {
            var dateActual = DateTime.Now.AddDays(-3).Date;

            var handDate = dynamicParams.GetAs<DateTime?>("dateActual");
            if (handDate.HasValue)
            {
                dateActual = handDate.Value;
            }
            else
            {
                var dateMax = this.AppealCitsCompareEdoDomain.GetAll().Where(x => x.IsEdo).Select(x => x.DateActual).Max();
                if (dateMax.HasValue)
                {
                    dateActual = dateMax.Value.AddHours(-1);
                }
            }

            return dateActual;
        }

        private DateTime? GetDateUntil(DynamicDictionary dynamicParams)
        {
            return dynamicParams.GetAs<DateTime?>("dateUntil");
        }

        private string GetDateInFormat(DateTime date)
        {
            return $"{date.Year:D4}{date.Month:D2}{date.Day:D2}{date.Hour:D2}{date.Minute:D2}{date.Second:D2}";
        }

        /// <summary>
        /// Загрузка обращения в систему
        /// </summary>
        private void LoadAppealCits(Docs appealCitsEdo)
        {
            var correspondent = string.Empty;
            var addressEdo = string.Empty;
            var emailsForEdo = string.Empty;
            var phone = string.Empty;

            var appealCitsCompEdo = this.appealCitsCompareEdoCache.Get(appealCitsEdo.Id);
            var authors = appealCitsEdo.Authors;
            if (authors != null)
            {
                var authorsList = authors.Where(x => x.Value.Author != null)
                    .Select(x => $" {x.Value.Author.Surname} {x.Value.Author.Name} {x.Value.Author.Patronymic}")
                    .ToList();
                correspondent = authorsList.Count == 0 ? string.Empty : authorsList.Aggregate((current, next) => $"{current}, {next}");

                var address = authors.Values
                    .Where(x => x.Address != null)
                    .Select(IntegrationEmsedService.FormatAddress)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .ToList();

                var emails = authors.Values
                    .Where(x => !string.IsNullOrEmpty(x.Address?.Email))
                    .Select(x => x.Address.Email.Trim())
                    .Distinct()
                    .ToList();

                var phones = authors.Values
                    .Where(x => !string.IsNullOrEmpty(x.Address?.Phone))
                    .Select(x => x.Address.Phone.Trim())
                    .Distinct()
                    .ToList();

                addressEdo = address.Aggregate(addressEdo, (current, адрес) => current + (адрес + "; ")).Trim().TrimEnd(';');
                emailsForEdo = emails.Aggregate(emailsForEdo, (current, email) => current + (email + "; ")).Trim().TrimEnd(';');
                phone = phones.Aggregate(phone, (current, ph) => current + (ph + "; ")).Trim().TrimEnd(';');

                if (addressEdo.Length > 2000)
                {
                    addressEdo = addressEdo.Substring(0, 2000);
                }

                if (emailsForEdo.Length > 250)
                {
                    emailsForEdo = emailsForEdo.Substring(0, 250);
                }

                if (phone.Length > 250)
                {
                    phone = phone.Substring(0, 250);
                }
            }

            var kindStatement = this.kindStatementCache.Get(appealCitsEdo.DocType)?.KindStatement;

            // Берем только первый, т.к. остальные данные в единственном числе
            var codeRevenueSource = appealCitsEdo.Recipients.Count > 0
                ? appealCitsEdo.Recipients["0"].ToInt()
                : -1;
            var revenueSource = this.revenueSourceCache.Get(codeRevenueSource)?.RevenueSource;
            var revenueForm = this.revenueFormCache.Get(appealCitsEdo.Delivery.ToInt())?.RevenueForm;

            var checkTime = appealCitsEdo.ResContolDate;
            var dateFrom = appealCitsEdo.RegDate;

            var appealCits = appealCitsCompEdo?.AppealCits;

            if (appealCits == null)
            {
                this.countToAdd++;

                appealCits = new AppealCits
                {
                    KindStatement = kindStatement,
                    TypeCorrespondent = TypeCorrespondent.CitizenHe,
                    NumberGji = appealCitsEdo.NumberGji,
                    DateFrom = dateFrom,
                    Correspondent = correspondent,
                    CorrespondentAddress = addressEdo,
                    Email = emailsForEdo,
                    Phone = phone,
                    Description = appealCitsEdo.Description,
                    CheckTime = checkTime
                };
                this.appealCitsToSave.Add(appealCits);

                appealCitsCompEdo = new AppealCitsCompareEdo
                {
                    AppealCits = appealCits,
                    IsEdo = true,
                    AddressEdo = addressEdo,
                    CodeEdo = appealCitsEdo.Id,
                    DateActual = appealCitsEdo.DateActual,
                    IsDocEdo = !string.IsNullOrEmpty(appealCitsEdo.Attachments?.FileId)
                };
                this.appealCitsCompareEdoToSave.Add(appealCitsCompEdo);

                var edoLogAppCitsEdo = new LogRequestsAppCitsEdo
                {
                    AppealCitsCompareEdo = appealCitsCompEdo,
                    ActionIntegrationRow = ActionIntegrationRow.Added,
                    DateActual = appealCitsEdo.DateActual.GetValueOrDefault(),
                    LogRequests = this.logRequests
                };
                this.logRequestsAppCitsEdoToSave.Add(edoLogAppCitsEdo);

                var source = new AppealCitsSource
                {
                    AppealCits = appealCits,
                    RevenueSource = revenueSource,
                    RevenueForm = revenueForm,
                    RevenueSourceNumber = appealCitsEdo.SrcNum,
                    RevenueDate = appealCitsEdo.InDate.To<DateTime?>()
                };
                this.appealCitsSourceToSave.Add(source);
            }
            else
            {
                this.countToUpdate++;
                appealCits.NumberGji = IntegrationEmsedService.GetNumber(appealCits.NumberGji, appealCitsEdo.NumberGji);
                appealCitsCompEdo.DateActual = appealCitsEdo.DateActual;
                appealCits.CorrespondentAddress = addressEdo;

                if (!string.IsNullOrEmpty(correspondent))
                {
                    appealCits.Correspondent = correspondent;
                }

                if (!string.IsNullOrEmpty(emailsForEdo))
                {
                    appealCits.Email = emailsForEdo;
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    appealCits.Phone = phone;
                }

                if (kindStatement != null)
                {
                    appealCits.KindStatement = kindStatement;
                }

                if (!string.IsNullOrEmpty(appealCitsEdo.Description))
                {
                    appealCits.Description = appealCitsEdo.Description;
                }

                if (checkTime > DateTime.MinValue)
                {
                    appealCits.CheckTime = checkTime;
                }

                // добавил проверку на IsNullOrEmpty, т.к. если код ЭДО инспектора не задан брался инспектор с кодом 0
                var surety = !string.IsNullOrEmpty(appealCitsEdo.ResAuthor) && this.inspectorCache.ContainsKey(appealCitsEdo.ResAuthor.ToInt())
                    ? this.inspectorCache[appealCitsEdo.ResAuthor.ToInt()].Inspector
                    : null;
                var tester = !string.IsNullOrEmpty(appealCitsEdo.Inspector) && this.inspectorCache.ContainsKey(appealCitsEdo.Inspector.ToInt())
                    ? this.inspectorCache[appealCitsEdo.Inspector.ToInt()].Inspector
                    : null;
                var executant = !string.IsNullOrEmpty(appealCitsEdo.Executant) && this.inspectorCache.ContainsKey(appealCitsEdo.Executant.ToInt())
                    ? this.inspectorCache[appealCitsEdo.Executant.ToInt()].Inspector
                    : null;
                if (surety != null || tester != null || executant != null)
                {
                    if (appealCits.Surety == null)
                    {
                        appealCits.Surety = surety;
                    }

                    if (appealCits.Tester == null)
                    {
                        appealCits.Tester = tester;
                    }

                    if (appealCits.Executant == null)
                    {
                        appealCits.Executant = executant;
                    }
                }

                appealCitsCompEdo.IsDocEdo = !string.IsNullOrEmpty(appealCitsEdo.Attachments?.FileId);
                appealCitsCompEdo.AddressEdo = addressEdo;

                this.appealCitsCompareEdoToSave.Add(appealCitsCompEdo);
                this.appealCitsToSave.Add(appealCits);

                var edoLogAppCitsEdo = new LogRequestsAppCitsEdo
                {
                    AppealCitsCompareEdo = appealCitsCompEdo,
                    ActionIntegrationRow = ActionIntegrationRow.Updated,
                    DateActual = appealCitsEdo.DateActual.GetValueOrDefault(),
                    LogRequests = this.logRequests
                };
                this.logRequestsAppCitsEdoToSave.Add(edoLogAppCitsEdo);
            }
        }

        private void SaveCache()
        {
            this.appealCitsToSave.ForEach(this.AppealCitsDomain.SaveOrUpdate);
            this.appealCitsSourceToSave.ForEach(this.AppealCitsSourceDomain.SaveOrUpdate);
            this.appealCitsCompareEdoToSave.ForEach(this.AppealCitsCompareEdoDomain.SaveOrUpdate);
            this.logRequestsAppCitsEdoToSave.ForEach(this.LogRequestsAppCitsEdoDomain.SaveOrUpdate);

            this.appealCitsToSave.Clear();
            this.appealCitsSourceToSave.Clear();
            this.appealCitsCompareEdoToSave.Clear();
            this.logRequestsAppCitsEdoToSave.Clear();
        }
    }
}