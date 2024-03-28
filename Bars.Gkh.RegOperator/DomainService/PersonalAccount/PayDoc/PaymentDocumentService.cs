namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using B4.Modules.FileStorage;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments.Snapshoted;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Mail;
    using System.Reflection;
    using System.Text;

    using AutoMapper;

    using Bars.B4.Application;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using Ionic.Zip;
    using Ionic.Zlib;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;

    /// <summary>
    /// Сервис для работы со слепками документов на оплату
    /// </summary>
    public class PaymentDocumentService : IPaymentDocumentService
    {
        private readonly IWindsorContainer container;
        private readonly IDomainService<PaymentDocumentSnapshot> snapshotDomainService;
        private readonly IDomainService<AccountPaymentInfoSnapshot> accountInfoDomainService;
        private readonly IDomainService<PersonalAccountPeriodSummary> periodSummaryDomain;
        private readonly IMapper mapper;
        public IDomainService<AccountOwnershipHistory> OwnershipHistoryDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager GkhUserManager { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="BasePersonalAccount"/>
        /// </summary>
        public IRepository<BasePersonalAccount> AccountRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="PersonalAccountOwner"/>
        /// </summary>
        public IRepository<PersonalAccountOwner> OwnerRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="IndividualAccountOwner"/>
        /// </summary>
        public IRepository<IndividualAccountOwner> IndividualOwnerRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="LegalAccountOwner"/>
        /// </summary>
        public IRepository<LegalAccountOwner> LegalOwnerRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="LogOperation"/>
        /// </summary>
        public IRepository<LogOperation> LogOperationRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="ChargePeriod"/>
        /// </summary>
        public IRepository<ChargePeriod> ChargePeriodRepository { get; set; } 

        /// <summary>
        /// Конструктор сервиса квитанция на оплату
        /// </summary>
        /// <param name="container"></param>
        /// <param name="snapshotDomainService">история платежных документов</param>
        /// <param name="accountInfoDomainService">Данные для документа на оплату</param>
        /// <param name="periodSummaryDomain">Детализация по лс</param>
        public PaymentDocumentService(
            IWindsorContainer container,
            IMapper mapper,
            IDomainService<PaymentDocumentSnapshot> snapshotDomainService,
            IDomainService<AccountPaymentInfoSnapshot> accountInfoDomainService,
            IDomainService<PersonalAccountPeriodSummary> periodSummaryDomain)
        {
            this.container = container;
            this.mapper = mapper;
            this.snapshotDomainService = snapshotDomainService;
            this.accountInfoDomainService = accountInfoDomainService;
            this.periodSummaryDomain = periodSummaryDomain;
        }

        /// <summary>
        /// Метод формирования квитанций на оплату
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат постановки задачи</returns>
        public IDataResult GetPaymentDocuments(BaseParams baseParams)
        {
            var documentTaskCreater = new DocumentTaskCreater(this.container, this.mapper);
            var documentTaskCreateResult = documentTaskCreater.CreateTasks(baseParams);

            if (!documentTaskCreateResult.CanExecuteTasks)
            {
                return new BaseDataResult(false, "Задача формирования документов на оплату не будет выполнена, т.к. отключен источник \"Основная информация\" для выгружаемого типа квитанций");
            }

            if (documentTaskCreateResult.DocumentTaskDescriptors.Count == 0)
            {
                return new BaseDataResult(false, "Задача формирования документов на оплату не будет выполнена из-за отсутствия данных о начислениях по лицевым счетам");
            }

            var documentTaskProvider = new DocumentTaskProvider(documentTaskCreateResult.DocumentTaskDescriptors.ToArray());

            return this.TaskManager.CreateTasks(documentTaskProvider, baseParams);
        }

        /// <summary>
        /// Создать документы на оплаты напрямую из сохраненных данных
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат постановки задачи</returns>
        public IDataResult CreateDocumentsFromSnapshots(BaseParams baseParams)
        {
            return this.TaskManager.CreateTasks(new SnapshotDocTaskProvider(this.container), baseParams);
        }

        /// <summary>
        /// Получить сгенерированный документ из слепка
        /// </summary>
        /// <param name="snapshotid">Идентификатор слепка</param>
        /// <returns>Сгенерированный документ</returns>
        public Stream CreateDocumentFromSnapshot(long snapshotid)
        {
            var snapshot = this.snapshotDomainService.Get(snapshotid);
            if (snapshot == null)
            {
                throw new ArgumentException("Переданы неверные параметры выгрузки!");
            }

            Stream stream;

            var fileManager = this.container.Resolve<IFileManager>();
            var userManager = this.container.Resolve<IGkhUserManager>();

            using (this.container.Using(fileManager, userManager))
            {
                stream = snapshot.HolderType == PaymentDocumentData.OwnerholderType
                    ? this.CreateOwnerDocument(snapshot)
                    : this.CreateAccountDocument(snapshot);
            }

            return stream;
        }

        /// <summary>
        /// Удалить снапшоты сохраненных платежек
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат выполнения удаления</returns>
        public IDataResult DeleteSnapshots(BaseParams baseParams)
        {
            var personalAccountsId = baseParams.Params.GetAs<string>("accountsId").ToLongArray().ToList();
            var snapshotsId = Converter.ToLongArray(baseParams.Params, "records");
            var periodId = baseParams.Params.GetAs<long>("periodId");

            if (snapshotsId.Length == 0 && personalAccountsId.Count > 0)
            {
                snapshotsId = this.accountInfoDomainService.GetAll()
                              .Where(x => x.Snapshot.Period.Id == periodId)
                              .WhereContains(x => x.AccountId, personalAccountsId)
                              .Select(x => x.Snapshot.Id).ToArray();
            }
            return this.DeleteSnapshots(snapshotsId, periodId);
        }

        /// <summary>
        /// Удалить снапшоты сохраненных платежек
        /// </summary>
        public IDataResult DeleteSnapshots(long[] snapshotsId, long periodId)
        {
            var sessionProvider = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(sessionProvider))
            {
                using (var session = sessionProvider.OpenStatelessSession())
                {
                    var transaction = session.BeginTransaction();
                    try
                    {
                        var deleteAccountQuery = "DELETE AccountPaymentInfoSnapshot a WHERE a.id IN ("
                            + " SELECT a2.id FROM AccountPaymentInfoSnapshot a2 JOIN a2.Snapshot p"
                            + " WHERE p.Period.id = :periodId{0})";

                        var deletePaymentDocumentQuery =
                            "DELETE PaymentDocumentSnapshot p WHERE p.Period.Id = :periodId";

                        if (snapshotsId.IsEmpty())
                        {
                            session.CreateQuery(deleteAccountQuery.FormatUsing(""))
                                .SetInt64("periodId", periodId)
                                .ExecuteUpdate();
                            session.CreateQuery(deletePaymentDocumentQuery)
                                .SetInt64("periodId", periodId)
                                .ExecuteUpdate();
                        }
                        else
                        {
                            foreach (var partOfIds in snapshotsId.SplitArray())
                            {
                                session.CreateQuery(deleteAccountQuery.FormatUsing(" AND p.id IN (:idList)"))
                                    .SetInt64("periodId", periodId)
                                    .SetParameterList("idList", partOfIds)
                                    .ExecuteUpdate();

                                session.CreateQuery(deletePaymentDocumentQuery + " AND p.id IN (:idList)")
                                    .SetInt64("periodId", periodId)
                                    .SetParameterList("idList", partOfIds)
                                    .ExecuteUpdate();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Получить слепки по лицевым счетам
        /// </summary>
        public IQueryable<PaymentInfoSnapshotProxy> GetPaymentInfoSnapshots(long snapshotId)
        {
            return this.accountInfoDomainService.GetAll()
                .Where(x => x.Snapshot.Id == snapshotId)
                .Select(this.GetSelectExpression());
        }

        public IDataResult CheckIsBaseSnapshots(BaseParams baseParams)
        {
            var accountIds = baseParams.Params.GetAs<string>("accountIds").ToLongArray();
            var periodId = baseParams.Params.GetAs<long>("periodId");

            var ownerIds = this.BasePersonalAccountDomain.GetAll()
                .Where(x => accountIds.Contains(x.Id))
                .Select(x => x.AccountOwner.Id)
                .Distinct()
                .ToArray();

            var snapshots = this.snapshotDomainService
                .GetAll()
                .Where(x => x.Period.Id == periodId)
                .Where(x => x.OwnerType == PersonalAccountOwnerType.Legal)
                .Where(x => x.PaymentDocumentType == PaymentDocumentType.RegistryLegal)
                .Where(x => ownerIds.Contains(x.HolderId))
                .Where(x => x.IsBase)
                .ToList();

            if (snapshots.Count == 0)
            {
                return new BaseDataResult(false, "Не сформирована печатная форма квитанции по всем помещениям ЮЛ (полный реестр)");
            }


            return new BaseDataResult();
        }

        /// <summary>
        /// Получить сгенерированный документ по периоду
        /// </summary>
        /// <param name="account">Счёт</param>
        /// <param name="period">Период</param>
        /// <param name="saveSnapshot">Сохранять ли слепки</param>
        /// <param name="useHistory">Как выбирать владельца лс: из истории или текущего</param>
        /// <returns>Сгенерированный документ</returns>
        /// <returns></returns>
        public Stream GetPaymentDocument(BasePersonalAccount account, ChargePeriod period, bool saveSnapshot, bool useHistory)
        {
            ArgumentChecker.NotNull(account, "account");
            ArgumentChecker.NotNull(period, "period");

            var accountDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var reportPeriodRepository = this.container.ResolveRepository<PaymentDocumentTemplate>();
            var fileManager = this.container.Resolve<IFileManager>();
            var userManager = this.container.Resolve<IGkhUserManager>();
            var ownershipHistoryService = this.container.Resolve<IAccountOwnershipHistoryService>();

            using (this.container.Using(accountDomain, reportPeriodRepository, fileManager, userManager, ownershipHistoryService))
            {
                var reportMan = new PaymentDocReportManager(reportPeriodRepository);
                reportMan.CreateTemplateCopyIfNotExist(period);

                var owner = useHistory ? ownershipHistoryService.GetOwner(account.Id, period) : account.AccountOwner;


                var primarySource = new PayDocPrimarySource(account.Id, owner?.Id ?? 0);

                var paymentDocumentType = owner?.OwnerType == PersonalAccountOwnerType.Legal
                    ? PaymentDocumentType.Legal
                    : PaymentDocumentType.Individual;

                using (var docSource = new PaymentDocumentDataSource(this.container, period, new[] { primarySource }, paymentDocumentType))
                {
                    docSource.CreateSnapshots(false);

                    var invoiceReport = paymentDocumentType == PaymentDocumentType.Individual
                        ? new BaseInvoiceReport(docSource.DocumentData.GetAllAccountData())
                        : new InvoiceAndActReport(docSource.DocumentData.GetAllAccountData());

                    var report = reportMan.GenerateReport(invoiceReport, ReportPrintFormat.pdf, period);

                    if (saveSnapshot)
                    {
                        docSource.Save();
                    }
                    return report;
                }
            }
        }

        /// <summary>
        /// Отправить документы на оплату по эл. почте
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="indicator">Индикатор выполнения задачи</param>
        public IDataResult SendEmails(BaseParams @params, IProgressIndicator indicator)
        {
            var snapshotIds = @params.Params.GetAs<long[]>("records");
            var periodId = @params.Params.GetAs<long>("periodId");
            var loadParams = @params.GetLoadParam();

            var emailConfig = this.container.Resolve<IGkhConfigProvider>()
                .Get<RegOperatorConfig>().PaymentDocumentEmailConfig;

            var emailOwnerType = emailConfig.PaymentDocumentEmailOwnerType;
            var emailSubject = emailConfig.PaymentDocumentEmailOptions.Subject;
            var emailBody = emailConfig.PaymentDocumentEmailOptions.Body;

            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var smtpEmail = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");

            var startDate = DateTime.Now;
            var period = this.ChargePeriodRepository.Get(periodId);
            var logOperation = new LogOperation
            {
                StartDate = startDate,
                OperationType = LogOperationType.SendEmails,
                User = this.GkhUserManager.GetActiveUser(),
                Comment = $"Период: {period.Name}. Дата отправки: {startDate.ToShortDateString()}"
            };

            var log = new StringBuilder();
            log.AppendLine(@"Плательщик;Период;Адрес электронной почты;Результат");

            indicator.Indicate(null, 0, "Инициализация данных");

            var snapshotsQuery = this.snapshotDomainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .WhereIf(snapshotIds.Any(), x => snapshotIds.Contains(x.Id));
            
            if (!snapshotIds.Any())
            {
                snapshotsQuery = snapshotsQuery.Filter(loadParams, this.container);
            }

            var snapshotIdsByHolderDict = snapshotsQuery
                .GroupBy(x => $"{x.HolderType}#{x.HolderId}")
                .ToDictionary(x => x.Key, x => x.First());

            var ownersDataByAccount = this.AccountRepository.GetAll()
                .Where(x => snapshotsQuery.Where(y => y.HolderType == PaymentDocumentData.AccountHolderType).Any(y => y.HolderId == x.Id))
                .WhereIf(emailOwnerType == PaymentDocumentEmailOwnerType.Individual, x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                .WhereIf(emailOwnerType == PaymentDocumentEmailOwnerType.Legal, x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal)
                .Select(x => new
                    {
                        AccountId = x.Id,
                        OwnerId = x.AccountOwner.Id,
                        x.AccountOwner.OwnerType
                    })
                .AsEnumerable()
                .GroupBy(x => x.OwnerId)
                .Select(x => x.Select(y => new
                    {
                        y.OwnerId,
                        y.OwnerType,
                        Snapshot = snapshotIdsByHolderDict.Get($"{PaymentDocumentData.AccountHolderType}#{y.AccountId}")
                    }).First());

            var ownersDataByOwner = this.OwnerRepository.GetAll()
                .Where(x => snapshotsQuery.Where(y => y.HolderType == PaymentDocumentData.OwnerholderType).Any(y => y.HolderId == x.Id))
                .WhereIf(emailOwnerType == PaymentDocumentEmailOwnerType.Individual, x => x.OwnerType == PersonalAccountOwnerType.Individual)
                .WhereIf(emailOwnerType == PaymentDocumentEmailOwnerType.Legal, x => x.OwnerType == PersonalAccountOwnerType.Legal)
                .Select(x => new
                    {
                        OwnerId = x.Id,
                        x.OwnerType
                    })
                .AsEnumerable()
                .GroupBy(x => x.OwnerId)
                .Select(x => x.Select(y => new
                    {
                        y.OwnerId,
                        y.OwnerType,
                        Snapshot = snapshotIdsByHolderDict.Get($"{PaymentDocumentData.OwnerholderType}#{y.OwnerId}")
                    }).First());

            var ownersData = ownersDataByAccount.Union(ownersDataByOwner)
                .DistinctBy(x => x.OwnerId)
                .OrderBy(x => x.OwnerId)
                .ToArray();

            indicator.Indicate(null, 20, "Подготовка к отправке");

            var errorsCount = 0;
            try
            {
                var totalCount = ownersData.Length;
                var take = 10000;
                for (int skip = 0; skip < totalCount; skip += take)
                {
                    var ownersPartData = ownersData.Skip(skip).Take(take).ToArray();
                    var percentage = 20 + ownersPartData.Length / totalCount * 60;
                    indicator.Indicate(null, (uint)(percentage > 80 ? 80 : percentage), $"Отправка документов {ownersPartData.Length} из {totalCount}");

                    var snapshotsToQueue = ownersPartData.Select(x => x.Snapshot).ToArray();
                    foreach (var snapshot in snapshotsToQueue)
                    {
                        snapshot.SendingEmailState = PaymentDocumentSendingEmailState.Queue;
                    }
                    TransactionHelper.InsertInManyTransactions(this.container, snapshotsToQueue, 10000);

                    var individualOwnerIds = ownersPartData.Where(x => x.OwnerType == PersonalAccountOwnerType.Individual)
                        .Select(x => x.OwnerId)
                        .ToArray();

                    var legalOwnerIds = ownersPartData.Where(x => x.OwnerType == PersonalAccountOwnerType.Legal)
                        .Select(x => x.OwnerId)
                        .ToArray();

                    var individialEmails = this.IndividualOwnerRepository.GetAll()
                        .Where(x => individualOwnerIds.Contains(x.Id))
                        .Where(x => x.Email != null)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.Email
                            })
                        .ToArray();

                    var legalEmails = this.LegalOwnerRepository.GetAll()
                        .Where(x => legalOwnerIds.Contains(x.Id))
                        .Where(x => x.Contragent != null)
                        .Where(x => x.Contragent.Email != null)
                        .Where(x => x.BillingAddressType == AddressType.Email)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.Contragent.Email
                            })
                        .ToArray();

                    var ownerEmailsDict = individialEmails.Union(legalEmails)
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, x => x.First().Email);

                    foreach (var ownerData in ownersPartData)
                    {
                        var emailAddress = ownerEmailsDict.Get(ownerData.OwnerId);
                        try
                        {
                            if (emailAddress.IsNotEmpty())
                            {
                                var stream = this.CreateDocumentFromSnapshot(ownerData.Snapshot.Id);
                                stream.Position = 0;

                                using (stream)
                                using (var client = new SmtpClient(smtpClient, smtpPort))
                                using (var message = new MailMessage(smtpEmail, emailAddress, emailSubject, emailBody))
                                using (var attachment = new Attachment(stream, "Payment.pdf", "application/pdf"))
                                {
                                    client.EnableSsl = false;
                                    client.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
                                    message.Attachments.Add(attachment);
                                    try
                                    {
                                        client.Send(message);
                                       
                                    }
                                    catch(Exception e)
                                    {
                                        errorsCount++;
                                        log.AppendLine($@"{ownerData.Snapshot.Payer};{period.Name};{emailAddress};При отправке без ssl произошла ошибка: {e.Message}");
                                        ownerData.Snapshot.SendingEmailState = PaymentDocumentSendingEmailState.NotSent;
                                        try
                                        {
                                            client.EnableSsl = true;
                                            client.Send(message);
                                        }
                                        catch (Exception ex)
                                        {
                                            errorsCount++;
                                            log.AppendLine($@"{ownerData.Snapshot.Payer};{period.Name};{emailAddress};При отправке с ssl произошла ошибка: {ex.Message}");
                                            ownerData.Snapshot.SendingEmailState = PaymentDocumentSendingEmailState.NotSent;
                                            this.snapshotDomainService.Update(ownerData.Snapshot);
                                            continue;
                                        }
                                    }

                                    ownerData.Snapshot.SendingEmailState = PaymentDocumentSendingEmailState.Sent;
                                    log.AppendLine($@"{ownerData.Snapshot.Payer};{period.Name};{emailAddress};Успешно отправлено");
                                }
                            }
                            else
                            {
                                errorsCount++;
                                log.AppendLine($@"{ownerData.Snapshot.Payer};{period.Name};{emailAddress};Не указан адрес эл. почты");
                                ownerData.Snapshot.SendingEmailState = PaymentDocumentSendingEmailState.NotSent;
                            }
                        }
                        catch (Exception e)
                        {
                            errorsCount++;
                            log.AppendLine($@"{ownerData.Snapshot.Payer};{period.Name};{emailAddress};При отправке произошла ошибка: {e.Message}");
                            ownerData.Snapshot.SendingEmailState = PaymentDocumentSendingEmailState.NotSent;
                        }

                        this.snapshotDomainService.Update(ownerData.Snapshot);
                    }
                }
            }
            catch (Exception e)
            {
                errorsCount++;
                log.AppendLine($@"При отправке сообщений произошла ошибка: {e.Message}");
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                indicator.Indicate(null, 90, "Завершение отправки");

                log.AppendLine($"{(errorsCount > 0 ? "Завершено с ошибками" : "Успешно завершено")};Количество ошибок: {errorsCount}");
                logOperation.EndDate = DateTime.Now;

                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                using (var logFile = new MemoryStream())
                {
                    var logAsBytes = Encoding.GetEncoding(1251).GetBytes(log.ToString());

                    logsZip.AddEntry($"{logOperation.OperationType.GetEnumMeta().Display}.csv", logAsBytes);
                    logsZip.Save(logFile);

                    var logFileInfo = this.FileManager.SaveFile(logFile, $"{logOperation.OperationType.GetEnumMeta().Display}.zip");
                    logOperation.LogFile = logFileInfo;
                }

                this.LogOperationRepository.Save(logOperation);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Установить значение эл. почты
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="indicator">Индикатор выполнения задачи</param>
        public IDataResult SetEmails(BaseParams @params, IProgressIndicator indicator)
        {
            var periodId = @params.Params.GetAs<long>("periodId");

            //пилим всем опять значение нет, если вдруг поменяли тип
            var snapshotDefault = this.snapshotDomainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Where(x => x.HasEmail == YesNo.Yes)
                .ToList();

            foreach (var snap in snapshotDefault)
            {
                snap.HasEmail = YesNo.No;
                snapshotDomainService.Update(snap);
            }

            //физики
            var _indOwnerIds = IndividualOwnerRepository.GetAll()
                .Where(x => x.BillingAddressType == AddressType.Email)
                .Select(x => x.Id)
                .ToList();

            var persAccs = AccountRepository.GetAll()
                .Where(x => _indOwnerIds.Contains(x.AccountOwner.Id))
                .Select(x => x.Id)
                .ToList();

            var snapshotsQueryInd = this.snapshotDomainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Where(x => x.OwnerType == PersonalAccountOwnerType.Individual)
                .Where(x => persAccs.Contains(x.HolderId))
                .ToList();
            
            foreach (var snap in snapshotsQueryInd)
            {
                snap.HasEmail = YesNo.Yes;
                snapshotDomainService.Update(snap);
            }

            //юрики
            var _legOwnerIds = LegalOwnerRepository.GetAll()
                .Where(x => x.BillingAddressType == AddressType.Email)
                .Select(x => x.Id)
                .ToList();

            var snapshotsQueryLeg = this.snapshotDomainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Where(x => x.OwnerType == PersonalAccountOwnerType.Legal)
                .Where(x => _legOwnerIds.Contains(x.HolderId))
                .ToList();

            foreach (var snap in snapshotsQueryLeg)
            {
                snap.HasEmail = YesNo.Yes;
                snapshotDomainService.Update(snap);
            }

            return new BaseDataResult();
        }

        private Stream CreateAccountDocument(PaymentDocumentSnapshot snapshot)
        {
            return this.CreateDocument(new BaseInvoiceReport(new[] { snapshot }), snapshot.Period);
        }

        private Stream CreateOwnerDocument(PaymentDocumentSnapshot snapshot)
        {
            var accountInfo = this.accountInfoDomainService.GetAll().Where(x => x.Snapshot.Id == snapshot.Id).ToList();

            return accountInfo.Count > 1
                //шаблон "Счет (с реестром помещений)"
                ? this.CreateDocument(new InvoiceRegistryAndActReport(new[] { snapshot }, accountInfo), snapshot.Period)
                //шаблон "Счет(с адресом)"
                : this.CreateDocument(new InvoiceAndActReport(new[] { snapshot }), snapshot.Period);
        }

        private Stream CreateDocument(ICodedReport report, ChargePeriod period)
        {
            var paymentDocumentTemplateRepository = this.container.ResolveRepository<PaymentDocumentTemplate>();

            using (this.container.Using(paymentDocumentTemplateRepository))
            {
                var generator = new PaymentDocReportManager(paymentDocumentTemplateRepository);
                return generator.GenerateReport(report, ReportPrintFormat.pdf, period);
            }
        }

        private Expression<Func<AccountPaymentInfoSnapshot, PaymentInfoSnapshotProxy>> GetSelectExpression()
        {
            var sourceProperties = typeof(AccountPaymentInfoSnapshot).GetProperties();
            var destinationProperties = typeof(PaymentInfoSnapshotProxy).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(AccountPaymentInfoSnapshot), "src");

            var bindings = destinationProperties
                .Select(destinationProperty => this.BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<AccountPaymentInfoSnapshot, PaymentInfoSnapshotProxy>>(
                Expression.MemberInit(
                    Expression.New(typeof(PaymentInfoSnapshotProxy)),
                    bindings),
                parameterExpression);

            return expression;
        }

        private MemberAssignment BuildBinding(
            ParameterExpression parameterExpression,
            PropertyInfo destinationProperty,
            IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);
            var calcAmount = this.container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.PaymentDocumentRegistryConfig;

            Expression valueExpression;

            var ownerTypeSelector = Expression.Property(Expression.Property(parameterExpression, "Snapshot"), "OwnerType");
            var conditionForTypeExpression = Expression.Condition(
                Expression.Equal(ownerTypeSelector, Expression.Constant(PersonalAccountOwnerType.Individual)),
                Expression.Constant(calcAmount.CalcAmountIndividual),
                Expression.Constant(calcAmount.CalcAmountLegal));

            if (destinationProperty.Name == "SaldoOut")
            {
                var queryExpression = this.GetFunc(this.periodSummaryDomain.GetAll(), x => x.SaldoOut);
                queryExpression = ParameterRebinder.ReplaceParameters(queryExpression.Parameters[0], parameterExpression, queryExpression);

                valueExpression = Expression.Condition(
                    Expression.Equal(
                        conditionForTypeExpression,
                        Expression.Constant(CalcAmountType.ChargeAndRecalc)),
                    Expression.Add(Expression.Property(parameterExpression, "BaseTariffSum"), Expression.Property(parameterExpression, "DecisionTariffSum")),
                    queryExpression.Body);
            }
            else if (destinationProperty.Name == "PenaltySum")
            {
                var queryExpression = this.GetFunc(this.periodSummaryDomain.GetAll(), x => x.PenaltyDebt + x.Penalty + x.RecalcByPenalty + x.PenaltyChange - x.PenaltyPayment);
                queryExpression = ParameterRebinder.ReplaceParameters(queryExpression.Parameters[0], parameterExpression, queryExpression);

                valueExpression = Expression.Condition(
                    Expression.Equal(
                        conditionForTypeExpression,
                        Expression.Constant(CalcAmountType.ChargeAndRecalc)),
                    Expression.Property(parameterExpression, sourceProperty),
                    queryExpression.Body);
            }
            else
            {
                valueExpression = Expression.Property(parameterExpression, sourceProperty);
            }

            if (valueExpression.IsNotNull())
            {
                return Expression.Bind(destinationProperty, valueExpression);
            }

            return null;
        }

        private Expression<Func<AccountPaymentInfoSnapshot, decimal>> GetFunc(
            IQueryable<PersonalAccountPeriodSummary> query,
            Expression<Func<PersonalAccountPeriodSummary, decimal>> selectExpression)
        {
            return x => query
                .Where(y => y.PersonalAccount.Id == x.AccountId && x.Snapshot.Period.Id == y.Period.Id)
                .Select(selectExpression)
                .FirstOrDefault();
        }
    }
}