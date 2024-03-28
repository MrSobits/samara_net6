namespace Bars.Gkh.RegOperator.DomainService.BankDocumentImport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository.MoneyOperations;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.Refund;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Exceptions;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.RegOperator.Report;
    using Bars.Gkh.RegOperator.Tasks.BankDocumentImport;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.StimulReport;
    using Bars.Gkh.Utils.PerformanceLogging;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using NHibernate.Linq;
    using PartialOperationCancellation;

    /// <summary>
    /// Сервис для документов, загруженных из банка
    /// </summary>
    public class BankDocumentImportService : IBankDocumentImportService
    {
        private static readonly ImportedPaymentType[] paymentCommands = 
        {
            ImportedPaymentType.Basic,
            ImportedPaymentType.ChargePayment,
            ImportedPaymentType.Payment,
            ImportedPaymentType.Penalty,
            ImportedPaymentType.SocialSupport,
            ImportedPaymentType.Sum
        };

        private static readonly ImportedPaymentType[] refundCommands = 
        {
            ImportedPaymentType.Refund,
            ImportedPaymentType.PenaltyRefund
        };

    #region Properties
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Диспетчер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Провайдер сессии
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Менеджер логгирования
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Доменный сервис для лицевого счета
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Доменный сервис для нераспределенных оплат
        /// </summary>
        public IDomainService<UnconfirmedPayments> UnconfirmedPaymentsDomain { get; set; }

        /// <summary>
        /// Доменный сервис для документов загруженных из банка
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        /// <summary>
        /// Доменный сервис для Трансфером между источником и получателем денег
        /// </summary>
        public IDomainService<PersonalAccountPaymentTransfer> TransferDomain { get; set; }

        /// <summary>
        /// Фабрика для создания команды
        /// </summary>
        public IPersonalAccountPaymentCommandFactory CommandFactory { get; set; }

        /// <summary>
        /// Фабрика для создания команды возвратов
        /// </summary>
        public IPersonalAccountRefundCommandFactory RefundCommandFactory { get; set; }

        /// <summary>
        /// Доменный сервис для импортируемых оплат
        /// </summary>
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }

        /// <summary>
        /// Доменный сервис для сумм
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Доменный сервис для денежных операций
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Доменный сервис кошельков
        /// </summary>
        public IDomainService<Wallet> WalletDomain { get; set; }

        /// <summary>
        /// Репозиторий для периодов
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepo { get; set; }

        /// <summary>
        /// Репозиторий по движениям денег
        /// </summary>
        public IMoneyOperationRepository MoneyOperationRepo { get; set; }

        /// <summary>
        /// Интерфейс создания отсечек перерасчета для ЛС
        /// </summary>
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }

        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Интерфейс фабрики логгера производительности
        /// </summary>
        public IPerformanceLoggerFactory PerformanceLoggerFactory { get; set; }

        public IPartialOperationCancellationService PartialOperationCancellationService { get; set; }

        /// <summary>
        /// Менеджер для масового создания и сохранения истории смены статусов
        /// </summary>
        private readonly IGeneralStateHistoryManager stateHistoryManager;
        #endregion

        public BankDocumentImportService(IGeneralStateHistoryManager stateHistoryManager)
        {
            this.stateHistoryManager = stateHistoryManager;
            this.stateHistoryManager.Init(typeof(ImportedPayment));
        }

        /// <inheritdoc />
        public IDataResult TaskImport(BaseParams baseParams)
        {
            var resultList = new List<IDataResult>();

            // Для каждого файла отдельная задача
            foreach (var file in baseParams.Files)
            {
                resultList.Add(this.TaskManager.CreateTasks(new BankDocumentImportFileProvider(this.Container),
                    new BaseParams()
                    {
                        Params = baseParams.Params,
                        Files = new Dictionary<string, FileData>() {{file.Key, file.Value}}
                    }));
            }
            return new BaseDataResult(resultList);
        }

        /// <inheritdoc />
        public IDataResult AcceptInternalPayments(BaseParams baseParams)
        {
            var bankDocumentImportId = baseParams.Params.GetAsId("bankDocumentImportId");
            var bankDocumentImport = this.BankDocumentImportDomain.GetAll().FirstOrDefault(x => x.Id == bankDocumentImportId && x.State != PaymentOrChargePacketState.Accepted);
            if (bankDocumentImport == null)
            {
                return new BaseDataResult(false, "Отсутствует документ загруженный из банка");
            }

            var chargePeriod = this.ChargePeriodRepo.GetCurrentPeriod();
            if (chargePeriod == null)
            {
                return new BaseDataResult(false, "Нет открытого периода");
            }

            var importedPaymentIds = baseParams.Params.GetAs<long[]>("importedPaymentIds", ignoreCase: true);

            if (importedPaymentIds.Length > 50)
            {
                return this.CreateAcceptPaymentsTask(baseParams);
            }

            return this.ProcessInternalAcceptPayments(importedPaymentIds, bankDocumentImport, chargePeriod);
        }

        /// <inheritdoc />
        public IDataResult CancelInternalPayments(BaseParams baseParams)
        {
            var bankDocumentImportId = baseParams.Params.GetAsId("bankDocumentImportId");
            var bankDocumentImport = this.BankDocumentImportDomain.GetAll().FirstOrDefault(x => x.Id == bankDocumentImportId && x.State != PaymentOrChargePacketState.Accepted);
            if (bankDocumentImport == null)
            {
                return new BaseDataResult(false, "Отсутствует документ загруженный из банка");
            }

            var chargePeriod = this.ChargePeriodRepo.GetCurrentPeriod();
            if (chargePeriod == null)
            {
                return new BaseDataResult(false, "Нет открытого периода");
            }
            
            return this.CreateCancelPaymentsTask(baseParams);
        }

        public IDataResult CancelInternalPayments(
            long[] importedPaymentIds,
            BankDocumentImport bankDocumentImport,
            ChargePeriod chargePeriod,
            IProgressIndicator progressIndicator = null)
        {
            var operations = this.MoneyOperationDomain.GetAll()
                .Where(x => x.CanceledOperation == null && !this.MoneyOperationDomain.GetAll().Any(y => y.CanceledOperation == x))
                .Where(x => x.OriginatorGuid == bankDocumentImport.TransferGuid)
                .ToList();

            this.ChargePeriodRepo.InitCache();

            var selectedImportedPayments = this.ImportedPaymentDomain.GetAll()
                .Fetch(x => x.PersonalAccount)
                .WhereContains(x => x.Id, importedPaymentIds)
                .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                .Where(x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                .ToList();

            var accountIds = selectedImportedPayments.Select(x => x.PersonalAccount.Id);

            if (selectedImportedPayments.Count == 0)
            {
                var message = "Не выбраны платежи, у которых статус определения ЛС = \"Определен\" и статус подтверждения оплат = \"Подтвержден\", " +
                    "либо статус ЛС не соответствует настройкам";

                this.SetPaymentsDistributed(bankDocumentImport, importedPaymentIds);

                return BaseDataResult.Error(message);
            }

            var periodSummaryDict =
                this.PersonalAccountPeriodSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == chargePeriod.Id)
                    .Where(x =>
                        this.ImportedPaymentDomain.GetAll()
                            .Where(y => y.BankDocumentImport.Id == bankDocumentImport.Id)
                            .Where(y => (y.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation
                                    || y.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.ConfirmationImpossible)
                                && y.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                            .Any(y => y.PersonalAccount == x.PersonalAccount))
                    .ToDictionary(x => x.PersonalAccount.Id);

            var anyPayments = selectedImportedPayments
                .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                .Any(x => BankDocumentImportService.paymentCommands.Contains(x.PaymentType));

            var anyRefunds = selectedImportedPayments
                .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                    .Any(x => BankDocumentImportService.refundCommands.Contains(x.PaymentType));

            var operationIds = operations.Select(x => x.Id);

            var transfers = this.TransferDomain.GetAll()
                .Where(x => accountIds.Contains(x.Owner.Id))
                .Where(x => operationIds.Contains(x.Operation.Id))
                .ToList();

            var importedPayments = selectedImportedPayments
                .GroupBy(x => x.PersonalAccount)
                .ToDictionary(x => x.Key.Id);

            var realityObjectPaymentSession = this.Container.Resolve<IRealtyObjectPaymentSession>();

            try
            {
                this.Container.InTransaction(() =>
                {
                    foreach (var operation in operations)
                    {
                        IDictionary<ImportedPayment, PersonalAccountPaymentTransfer[]> comparedPayment =
                            new Dictionary<ImportedPayment, PersonalAccountPaymentTransfer[]>();

                        var newOperation = operation.Clone().As<MoneyOperation>();

                        this.MoneyOperationDomain.Save(newOperation);

                        var owners = transfers
                            .Where(x => x.Operation.Id == operation.Id)
                            .GroupBy(x => x.Owner)
                            .ToDictionary(x => x.Key, y => y);

                        foreach (var owner in owners)
                        {
                            var payments = importedPayments.Get(owner.Key.Id);

                            var dates = payments.Select(x => x.PaymentDate);

                            PaymentComparator.Compare(owner.Key, payments, owner.Value.Where(x => dates.Contains(x.PaymentDate))).AddTo(comparedPayment);

                            foreach (var transfer in owner.Value)
                            {
                                transfer.Operation = newOperation;
                            }    
                        }

                        var cancelOperationParams = new CancelOperationParams
                        {
                            Operation = newOperation,
                            Period = this.ChargePeriodRepo.GetCurrentPeriod(),
                            SummariesToSave = new HashSet<PersonalAccountPeriodSummary>(),
                            TransfersToSave = new List<Transfer>(),
                            AnyPayments = anyPayments,
                            AnyRefunds = anyRefunds,
                            SummaryCache = periodSummaryDict
                        };

                        this.CancelOperationInternal(cancelOperationParams);

                        cancelOperationParams.TransfersToSave.ForEach(this.TransferDomain.Save);
                        cancelOperationParams.SummariesToSave.ForEach(this.PersonalAccountPeriodSummaryDomain.SaveOrUpdate);
                    }

                    foreach (var importedPayment in selectedImportedPayments)
                    {
                        this.stateHistoryManager.CreateStateHistory(
                            importedPayment,
                            importedPayment.PaymentConfirmationState,
                            ImportedPaymentPaymentConfirmState.NotDistributed);

                        importedPayment.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.NotDistributed;
                        importedPayment.Accepted = false;
                        importedPayment.AcceptDate = null;
                        this.ImportedPaymentDomain.Update(importedPayment);
                    }

                    var totalCount = this.ImportedPaymentDomain.GetAll().Count(x => x.BankDocumentImport.Id == bankDocumentImport.Id);

                    var canceledCount =
                        this.ImportedPaymentDomain.GetAll()
                            .Count(
                                x =>
                                    x.BankDocumentImport.Id == bankDocumentImport.Id
                                    && x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.NotDistributed);

                    if (totalCount > 0)
                    {
                        if (canceledCount == totalCount)
                        {
                            bankDocumentImport.SetInPending();
                            bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.NotDistributed;
                            bankDocumentImport.CheckState = BankDocumentImportCheckState.NotChecked;
                        }
                        else
                        {
                            bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.PartiallyDistributed;
                        }

                        this.BankDocumentImportDomain.Update(bankDocumentImport);
                    }                

                    realityObjectPaymentSession.Complete();
                    this.RecalcEventManager.SaveEvents();
                    this.stateHistoryManager.SaveStateHistories();
                });
            }
            catch (Exception exception)
            {
                realityObjectPaymentSession.Rollback();

                this.Container.InTransactionInNewScope(() =>
                {
                    this.SetPaymentsDistributed(bankDocumentImport, selectedImportedPayments.Select(x => x.Id).ToList());

                    this.LogManager.LogError(exception, "Ошибка отмены оплат");
                });

                return BaseDataResult.Error(exception.Message);
            }
            finally
            {
                this.Container.Release(realityObjectPaymentSession);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult AcceptPayments(
            long[] importedPaymentIds, 
            BankDocumentImport bankDocumentImport, 
            ChargePeriod chargePeriod,
            IProgressIndicator progressIndicator = null)
        {
            var progressLogger = new ProcessLogger(bankDocumentImport);
            var logger = this.PerformanceLoggerFactory.GetLogger();
            var collector = this.PerformanceLoggerFactory.GetCollector();

            this.ChargePeriodRepo.InitCache();

            logger.StartTimer("AcceptDocument");
            logger.StartTimer("ImportedPaymentsToAccept");
            var selectedImportedPayments = this.ImportedPaymentDomain.GetAll()
                    .Fetch(x => x.PersonalAccount)
                    .ThenFetch(x => x.BaseTariffWallet)
                    .Fetch(x => x.PersonalAccount)
                    .ThenFetch(x => x.DecisionTariffWallet)
                    .Fetch(x => x.PersonalAccount)
                    .ThenFetch(x => x.PenaltyWallet)
                    .Fetch(x => x.PersonalAccount)
                    .ThenFetch(x => x.SocialSupportWallet)
                    .Fetch(x => x.PersonalAccount)
                    .ThenFetch(x => x.Room)
                    .ThenFetch(x => x.RealityObject)
                    .WhereContains(x => x.Id, importedPaymentIds)
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingConfirmation)
                    .Where(x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                    .ToList();
            logger.StopTimer("ImportedPaymentsToAccept");

            logger.StartTimer("periodSummDict");
            var periodSummDict =
                this.PersonalAccountPeriodSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == chargePeriod.Id)
                    .Where(
                        x =>
                            this.ImportedPaymentDomain.GetAll()
                                .Where(y => y.BankDocumentImport.Id == bankDocumentImport.Id)
                                .Where(y => importedPaymentIds.Contains(y.Id))
                                .Any(y => y.PersonalAccount.Id == x.PersonalAccount.Id))
                    .ToDictionary(x => x.PersonalAccount.Id);

            selectedImportedPayments.ForEach(x => x.PersonalAccount.SetOpenedPeriodSummary(periodSummDict.Get(x.PersonalAccount.Id)));
            logger.StopTimer("periodSummDict");

            if (selectedImportedPayments.Count == 0)
            {
                var message = "Не выбраны платежи, у которых статус определения ЛС = \"Определен\" и статус подтверждения оплат = \"Не подтвержден\", " +
                    "либо статус ЛС не соответствует настройкам";

                this.SetPaymentsNotDistributed(bankDocumentImport);

                return BaseDataResult.Error(message);
            }

            try
            {
                var indicatorProxy = new ProgressIndicatorProxy(progressIndicator, bankDocumentImport, true);
                var operation = bankDocumentImport.CreateOperation(this.ChargePeriodRepo.GetCurrentPeriod());
                this.MoneyOperationDomain.Save(operation);
                operation.Reason = "Подтверждение оплаты";

                var amountDistrType = BankDocumentImportService.GetDistributionType(bankDocumentImport);

                logger.StartTimer("AcceptPayments");

                this.AcceptImportedPayment(
                    selectedImportedPayments,
                    amountDistrType,
                    operation,
                    bankDocumentImport,
                    indicatorProxy,
                    progressLogger,
                    logger,
                    importedPaymentIds.Length);

                logger.StopTimer("AcceptPayments");

                logger.StartTimer("StateUpdate");

                var query = this.ImportedPaymentDomain.GetAll().Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id);
                var countDistributed = query.Count(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed);
                var countImpossible = query.Count(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.ConfirmationImpossible);
                var countTotal = query.Count();

                var flag = false;
                if (countDistributed > 0 && countTotal > 0 && countDistributed != countTotal)
                {
                    flag = true;
                    bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.PartiallyDistributed;
                    bankDocumentImport.SetInPending();
                }
                else if (countTotal > 0 && countDistributed == countTotal)
                {
                    flag = true;
                    bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.Distributed;
                    bankDocumentImport.SetAccepted();
                }
                else if (countImpossible == countTotal)
                {
                    flag = true;
                    bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.ConfirmationImpossible;
                    bankDocumentImport.SetInPending();
                }

                if (flag)
                {
                    bankDocumentImport.CheckState = BankDocumentImportCheckState.NotChecked;
                    this.BankDocumentImportDomain.Update(bankDocumentImport);
                }

                logger.StopTimer("StateUpdate");
            }
            catch (Exception exception)
            {
                progressLogger.SetError(exception.ToString());

                return this.Container.InTransactionWithResultInNewScope(() =>
                {
                    this.SetPaymentsNotDistributedInternal(bankDocumentImport.Id, importedPaymentIds);
                    return BaseDataResult.Error(exception.Message);
                });
            }
            finally
            {
                logger.StopTimer("AcceptDocument");
                logger.SaveLogs(collector, x => x.OrderByDescending(y => y.TimeSpan).First());
                logger.ClearSession();

                this.Container.Release(collector);
                this.Container.Release(logger);

                this.SaveLogs(LogOperationType.AcceptBankDocumentImport, progressLogger);
            }

            var distributedCount = selectedImportedPayments.Count(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed);
            var stringMessages = $"Количество подтвержденных оплат: {distributedCount}";
            return new BaseDataResult(true, stringMessages);
        }

        /// <inheritdoc />
        public IDataResult CheckPayments(List<BankDocumentImport> bankDocumentImports)
        {
            if (!bankDocumentImports.Any())
            {
                return BaseDataResult.Error("Отсутствуют оплаты для проверки.");
            }

            foreach (var bdImport in bankDocumentImports)
            {
                this.CheckPayment(bdImport);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult DeletePayments(BaseParams baseParams)
        {
            var packetIds = baseParams.Params.GetAs<long[]>("packetIds", ignoreCase: true);
            if (packetIds.IsEmpty())
            {
                return new BaseDataResult(false, "Необходимо выбрать записи для удаления реестра оплат");
            }

            var bdImports = this.BankDocumentImportDomain.GetAll()
                    .WhereContains(x => x.Id, packetIds)
                    .Where(x =>
                            ((x.PersonalAccountDeterminationState == PersonalAccountDeterminationState.Defined
                              || x.PersonalAccountDeterminationState == PersonalAccountDeterminationState.PartiallyDefined
                              || x.PersonalAccountDeterminationState == PersonalAccountDeterminationState.NotDefined)
                             && x.PaymentConfirmationState == PaymentConfirmationState.NotDistributed))
                    .ToList();

            if (!bdImports.Any())
            {
                return new BaseDataResult(false, "Отсутствуют реестры оплат для удаления.");
            }

            var mainCount = 0;
            foreach (var bankDocumentImport in bdImports)
            {
                var bdImport = bankDocumentImport;
                if (
                    this.ImportedPaymentDomain.GetAll()
                        .Any(x => x.BankDocumentImport.Id == bdImport.Id && x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed))
                {
                    continue;
                }

                try
                {
                    this.Container.InTransaction(() =>
                    {
                        this.ImportedPaymentDomain.GetAll()
                            .Where(x => x.BankDocumentImport.Id == bdImport.Id)
                            .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.NotDistributed)
                            .ForEach(
                                x =>
                                {
                                    this.stateHistoryManager.CreateStateHistory(
                                        x,
                                        x.PaymentConfirmationState,
                                        ImportedPaymentPaymentConfirmState.Deleted);

                                    x.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Deleted;
                                    this.ImportedPaymentDomain.Update(x);
                                });

                        var totalCount = this.ImportedPaymentDomain.GetAll().Count(x => x.BankDocumentImport.Id == bdImport.Id);
                        var count =
                            this.ImportedPaymentDomain.GetAll()
                                .Count(
                                    x =>
                                        x.BankDocumentImport.Id == bdImport.Id
                                        && x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Deleted);
                        if (totalCount >= 0 && count == totalCount)
                        {
                            mainCount++;
                            bdImport.PaymentConfirmationState = PaymentConfirmationState.Deleted;
                            this.BankDocumentImportDomain.Update(bdImport);
                        }

                        this.stateHistoryManager.SaveStateHistories();
                    });
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var stringMessages = $"Количество удаленных реестров оплат: {mainCount}";
            return new BaseDataResult(true, stringMessages);
        }

        /// <inheritdoc />
        public IDataResult CancelPayments(List<BankDocumentImport> bankDocumentImports, IProgressIndicator indicator = null)
        {
            var errorDocuments = new List<string>();
            if (!bankDocumentImports.Any())
            {
                return new BaseDataResult(false, "Отсутствуют оплаты для отмены подтверждения.");
            }

            var transferGuids = bankDocumentImports.Select(x => x.TransferGuid).ToArray();
            var operations = this.MoneyOperationDomain.GetAll()
                .Where(x => x.CanceledOperation == null && !this.MoneyOperationDomain.GetAll().Any(y => y.CanceledOperation == x))
                .WhereContains(x => x.OriginatorGuid, transferGuids)
                .ToList();

            var operationsGrouped = operations.GroupBy(x => x.OriginatorGuid).ToDictionary(x => x.Key, x => x.ToList());
            var chargePeriod = this.ChargePeriodRepo.GetCurrentPeriod();
            var mainCanceledCount = 0;
            var indicatorProxy = new ProgressIndicatorProxy(indicator, bankDocumentImports.Count, false, 0.8d);

            foreach (var bankDocumentImport in bankDocumentImports)
            {
                indicatorProxy.SetCurrent(bankDocumentImport);
                indicatorProxy.IndicatePercentage(0);

                var impossibles = this.ImportedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.ConfirmationImpossible
                        && x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                    .Select(x => x.Id)
                    .ToList();

                var eventCacheDict = this.ImportedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                    .Select(x => new
                    {
                        x.PaymentDate,
                        x.PersonalAccount
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.PaymentDate)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(x => x.PersonalAccount).Distinct().ToArray());

                this.RecalcEventManager.InitCache(eventCacheDict);

               var isSkipDocument = this.ImportedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Any(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Deleted
                        || (x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed
                            && x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.NotDefined));

                if (isSkipDocument)
                {
                    this.SetPaymentsDistributed(bankDocumentImport, impossibles);
                    var errorDoc = $"дата операции: {bankDocumentImport.ImportDate:dd.MM.yyyy}, "
                        + $"дата реестра: {bankDocumentImport.DocumentDate:dd.MM.yyyy}, "
                        + $"номер реестра: {bankDocumentImport.DocumentNumber}, "
                        + $"предупреждение: имеются оплаты в статусе '{ImportedPaymentPaymentConfirmState.Deleted.GetDisplayName()}' или "
                        + $"в статусе '{ImportedPaymentPaymentConfirmState.Distributed.GetDisplayName()}' и статус определения ЛС - "
                        + $"'{ImportedPaymentPersAccDeterminateState.NotDefined.GetDisplayName()}'";

                    errorDocuments.Add(errorDoc);
                    this.LogManager.LogError($"Ошибка отмены реестра({errorDoc})");

                    continue;
                }

                var periodSummaryDict =
                    this.PersonalAccountPeriodSummaryDomain.GetAll()
                        .Where(x => x.Period.Id == chargePeriod.Id)
                        .Where(x =>
                                this.ImportedPaymentDomain.GetAll()
                                    .Where(y => y.BankDocumentImport.Id == bankDocumentImport.Id)
                                    .Where(y => (y.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation
                                            || y.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.ConfirmationImpossible)
                                        && y.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                                    .Any(y => y.PersonalAccount == x.PersonalAccount))
                        .ToDictionary(x => x.PersonalAccount.Id);

                var anyPayments = this.ImportedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                    .WhereContains(x => x.PaymentType, BankDocumentImportService.paymentCommands)
                    .Any();

                var anyRefunds = this.ImportedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                    .WhereContains(x => x.PaymentType, BankDocumentImportService.refundCommands)
                    .Any();

                var realityObjectPaymentSession = this.Container.Resolve<IRealtyObjectPaymentSession>();
                var logger = this.PerformanceLoggerFactory.GetLogger();
                var collector = this.PerformanceLoggerFactory.GetCollector();

                var cancelParameters = new CancelOperationParams
                {
                    AnyPayments = anyPayments,
                    AnyRefunds = anyRefunds,
                    Logger = logger,
                    Indicator = indicatorProxy,
                    Period = chargePeriod,
                    SummaryCache = periodSummaryDict
                };

                try
                {
                    logger.StartTimer("UndoDocument");
                    this.Container.InTransaction(() =>
                    {
                        if (operationsGrouped.ContainsKey(bankDocumentImport.TransferGuid))
                        {
                            cancelParameters.SummariesToSave = new HashSet<PersonalAccountPeriodSummary>();
                            cancelParameters.TransfersToSave = new List<Transfer>();
                            cancelParameters.Operations = operationsGrouped[bankDocumentImport.TransferGuid];

                            foreach (var operation in cancelParameters.Operations)
                            {
                                cancelParameters.Operation = operation;

                                this.CancelOperationInternal(cancelParameters);

                                cancelParameters.CancelOperationNum++;
                            }

                            indicatorProxy.SetSaving();

                            logger.StartTimer("SaveTransfers");
                            cancelParameters.TransfersToSave.ForEach(this.TransferDomain.Save);
                            logger.StopTimer("SaveTransfers");

                            indicatorProxy.SetSaving();
                            logger.StartTimer("SaveSummaries");
                            cancelParameters.SummariesToSave.ForEach(this.PersonalAccountPeriodSummaryDomain.SaveOrUpdate);
                            logger.StopTimer("SaveSummaries");
                        }

                        indicatorProxy.SetSaving();
                        logger.StartTimer("GetAndUpdatePayments");
                        var importedPayments =
                            this.ImportedPaymentDomain.GetAll()
                                .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                                .Where(
                                    x =>
                                        (x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation
                                            || x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.ConfirmationImpossible)
                                        && x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                                .ToList();

                        foreach (var importedPayment in importedPayments)
                        {
                            this.stateHistoryManager.CreateStateHistory(
                                importedPayment, 
                                importedPayment.PaymentConfirmationState,
                                ImportedPaymentPaymentConfirmState.NotDistributed);

                            importedPayment.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.NotDistributed;
                            importedPayment.Accepted = false;
                            importedPayment.AcceptDate = null;
                            this.ImportedPaymentDomain.Update(importedPayment);
                        }
                        logger.StopTimer("GetAndUpdatePayments");

                        var totalCount = this.ImportedPaymentDomain.GetAll().Count(x => x.BankDocumentImport.Id == bankDocumentImport.Id);

                        var canceledCount =
                            this.ImportedPaymentDomain.GetAll()
                                .Count(
                                    x =>
                                        x.BankDocumentImport.Id == bankDocumentImport.Id
                                        && x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.NotDistributed);

                        if (totalCount > 0 && canceledCount == totalCount)
                        {
                            bankDocumentImport.SetInPending();

                            bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.NotDistributed;
                            bankDocumentImport.CheckState = BankDocumentImportCheckState.NotChecked;
                            this.BankDocumentImportDomain.Update(bankDocumentImport);

                            indicatorProxy.SetSaving();
                            logger.StartTimer("realityObjectPaymentSession");
                            realityObjectPaymentSession.Complete();
                            logger.StopTimer("realityObjectPaymentSession");

                            mainCanceledCount++;
                        }

                        indicatorProxy.SetSaving();
                        this.RecalcEventManager.SaveEvents();
                        this.stateHistoryManager.SaveStateHistories();

                        indicatorProxy.SetSaving();
                        logger.StartTimer("Commit");
                    });
                    logger.StopTimer("Commit");
                    logger.StopTimer("UndoDocument");
                }
                catch (Exception exception)
                {
                    realityObjectPaymentSession.Rollback();

                    this.Container.InTransactionInNewScope(() =>
                    {
                        this.SetPaymentsDistributed(bankDocumentImport, impossibles);
                        var errorDoc = $"дата операции: {bankDocumentImport.ImportDate:dd.MM.yyyy}, "
                            + $"дата реестра: {bankDocumentImport.DocumentDate:dd.MM.yyyy}, "
                            + $"номер реестра: {bankDocumentImport.DocumentNumber}, "
                            + $"ошибка: {exception.Message}";

                        errorDocuments.Add(errorDoc);
                        this.LogManager.LogError($"Ошибка отмены реестра({errorDoc})", exception);
                    });
                }
                finally
                {
                    logger.SaveLogs(collector, x => x.OrderByDescending(y => y.TimeSpan).First());
                    logger.ClearSession();

                    this.Container.Release(realityObjectPaymentSession);
                    this.Container.Release(collector);
                    this.Container.Release(logger);
                }
            }

            var errorMessages = string.Empty;
            if (errorDocuments.Any())
            {
                errorMessages = Environment.NewLine + "Реестры с ошибкой:" + Environment.NewLine
                                + errorDocuments.AggregateWithSeparator("," + Environment.NewLine);
            }

            var stringMessages = $"Количество отмененных подтверждений реестров оплат: {mainCanceledCount}{errorMessages}";
            return new BaseDataResult(errorMessages.IsEmpty(), stringMessages);
        }

        private void CancelOperationInternal(CancelOperationParams parameters)
        {
            var cancelOperation = parameters.Operation.Cancel(parameters.Period);
            cancelOperation.Reason = "Отмена подтверждения оплаты";

            this.MoneyOperationDomain.Save(cancelOperation);
            this.MoneyOperationDomain.Update(parameters.Operation);

            parameters.StartTimer("GetPersonalAccounts", $"Количество ЛС по реестру: {parameters.SummaryCache?.Count ?? 0}");
            var personalAccounts = this.GetPersonalAccounts(parameters.Operation, parameters.SummaryCache, parameters.AnyPayments, parameters.AnyRefunds);
            parameters.StopTimer("GetPersonalAccounts");

            var currentAccount = 0;
            var persAccCount = personalAccounts.Count;

            foreach (var account in personalAccounts)
            {
                parameters.IndicatePercents((double)currentAccount / persAccCount);
                currentAccount++;

                // если в реестре была хоть 1 оплатная строчка, то пытаемся отменить оплаты
                if (parameters.AnyPayments)
                {
                    parameters.StartTimer("UndoPayment");

                    // отменим все оплаты независимо от типа
                    foreach (var command in this.GetUndoPaymentCommands())
                    {
                        parameters.StartTimer(command.GetType().Name);
                        parameters.TransfersToSave.AddRange(account.UndoPayment(command, parameters.Period, cancelOperation, null));
                        parameters.StopTimer(command.GetType().Name);
                    }

                    parameters.StopTimer("UndoPayment");
                }

                // если в реестре была хоть 1 возвратная строчка, то пытаемся отменить возвраты
                if (parameters.AnyRefunds)
                {
                    var refundCommand = this.GetUndoRefundCommand();

                    // отменит все возвраты независимо от типа (пени, по базовому, по тарифу решений)
                    parameters.StartTimer(refundCommand.GetType().Name);
                    parameters.TransfersToSave.AddRange(account.UndoRefund(refundCommand, cancelOperation, parameters.Period, null));
                    parameters.StopTimer(refundCommand.GetType().Name);
                }

                parameters.SummariesToSave.Add(account.GetOpenedPeriodSummary());
            }
        }

        /// <inheritdoc />
        public IDataResult TaskAcceptDocuments(BaseParams baseParams)
        {
            var provider = this.Container.Resolve<ISessionProvider>();
            try
            {
                using (var session = provider.GetCurrentSession())
                {
                    if (session.Query<TableLock>().Count(x => x.TableName == "regop_transfer") > 0)
                    {
                        return new BaseDataResult(
                                   false,
                                   "Выполнение действия невозможно, так как таблица заблокирована. Повторите попытку после снятия блокировки."
                                   + " В зависимости от настроек приложения, блокировка будет автоматически выключена либо после завершения расчета, либо после успешного завершения закрытия периода.");
                    }

                    return this.TaskManager.CreateTasks(new BankDocumentImportTaskProvider(this.Container), baseParams);
                }
            }
            finally
            {
                this.Container.Release(provider);
            }
        }

        /// <inheritdoc />
        public IDataResult TaskCheckPayments(BaseParams baseParams)
        {
            return this.TaskManager.CreateTasks(new BankDocumentImportTaskCheckProvider(this.Container), baseParams);
        }

        /// <summary>
        /// Задача на отмены подтверждение оплат реестра
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат отмены подтверждение оплат реестра</returns>
        public IDataResult TaskCancelDocuments(BaseParams baseParams)
        {
            var provider = this.Container.Resolve<ISessionProvider>();
            var ids = baseParams.Params.GetAs<long[]>("packetIds");
            try
            {
                using (var session = provider.GetCurrentSession())
                {
                    if (session.Query<TableLock>().Count(x => x.TableName == "regop_transfer") > 0)
                    {
                        return new BaseDataResult(
                                   false,
                                   "Выполнение действия невозможно, так как таблица заблокирована. Повторите попытку после снятия блокировки."
                                   + " В зависимости от настроек приложения, блокировка будет автоматически выключена либо после завершения расчета, либо после успешного завершения закрытия периода.");
                    }
                    this.Container.InTransaction(
                        () =>
                            {
                                var bankDocumentImports = this.BankDocumentImportDomain.GetAll().Where(x => ids.Contains(x.Id)).ToArray();
                                foreach (var bankDocumentImport in bankDocumentImports)
                                {
                                    bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.WaitingCancellation;
                                    this.BankDocumentImportDomain.Update(bankDocumentImport);
                                }
                            });

                    return this.TaskManager.CreateTasks(new BankDocumentImportTaskCancelProvider(this.Container), baseParams);
                }
            }
            finally
            {
                this.Container.Release(provider);
            }
        }

        /// <summary>
        /// Подтверждение оплат реестра
        /// </summary>
        /// <param name="bankDocumentImports">Список документов</param>
        /// <param name="indicator">Индикатор процесса выполнения</param>
        /// <returns>Результат подтверждения оплат реестра</returns>
        public IDataResult AcceptDocuments(List<BankDocumentImport> bankDocumentImports, IProgressIndicator indicator)
        {
            if (!bankDocumentImports.Any())
            {
                return new BaseDataResult(false, "Отсутствуют оплаты для подтверждения.");
            }

            this.ChargePeriodRepo.InitCache();
            var chargePeriod = this.ChargePeriodRepo.GetCurrentPeriod();

            if (chargePeriod == null)
            {
                return new BaseDataResult(false, "Нет открытого периода");
            }

            // выводим сообщение если статус уже Подтвержден
            if (bankDocumentImports.Any(x => x.State == PaymentOrChargePacketState.Accepted))
            {
                return new BaseDataResult(false, "Запись уже подтверждена, невозможно подтвердить запись дважды.");
            }
            var countOfConfirmed = 0;
            var countOfPartiallyConfirmed = 0;
            var errorDocuments = new List<string>();
            var indicatorProxy = new ProgressIndicatorProxy(indicator, bankDocumentImports.Count, true);

            foreach (var bankDocumentImport in bankDocumentImports)
            {
                indicatorProxy.SetCurrent(bankDocumentImport);
                indicatorProxy.IndicatePercentage(0);
                MoneyOperation moneyOperation = null;

                var progressLogger = new ProcessLogger(bankDocumentImport);
                var logger = this.PerformanceLoggerFactory.GetLogger();
                var collector = this.PerformanceLoggerFactory.GetCollector();

                logger.StartTimer("AcceptDocument");

                try
                {
                    moneyOperation = bankDocumentImport.CreateOperation(this.ChargePeriodRepo.GetCurrentPeriod());
                    moneyOperation.Reason = "Подтверждение оплаты";

                    this.MoneyOperationDomain.Save(moneyOperation);

                    logger.StartTimer("ImportedPaymentsToAccept");
                    var amountDistrType = BankDocumentImportService.GetDistributionType(bankDocumentImport);
                    var allImportedPaymentQuery =
                        this.ImportedPaymentDomain.GetAll()
                            .Fetch(x => x.PersonalAccount)
                            .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id);

                    var allImportedPayment = allImportedPaymentQuery
                        .Select(x => new
                        {
                            PersAccNotNull = x.PersonalAccount != null,
                        })
                        .ToList();

                    var importedPaymentsToAccept = allImportedPaymentQuery
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.BaseTariffWallet)
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.DecisionTariffWallet)
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.PenaltyWallet)
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.SocialSupportWallet)
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.Room)
                        .ThenFetch(x => x.RealityObject)
                        .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingConfirmation)
                        .Where(x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                        .ToList();

                    logger.StopTimer("ImportedPaymentsToAccept");

                    logger.StartTimer("periodSummDict");
                    var periodSummDict =
                        this.PersonalAccountPeriodSummaryDomain.GetAll()
                            .Where(x => x.Period.Id == chargePeriod.Id)
                            .Where(
                                x =>
                                    this.ImportedPaymentDomain.GetAll()
                                        .Where(y => y.BankDocumentImport.Id == bankDocumentImport.Id)
                                        .Any(y => y.PersonalAccount.Id == x.PersonalAccount.Id))
                            .ToDictionary(x => x.PersonalAccount.Id);

                    importedPaymentsToAccept.ForEach(x => x.PersonalAccount.SetOpenedPeriodSummary(periodSummDict.Get(x.PersonalAccount.Id)));
                    logger.StopTimer("periodSummDict");

                    var anyDistributed = false;
                    try
                    {
                        logger.StartTimer("AcceptPayments");

                        // если хоть одна пачка упадёт, у нас останутся подтверждённые оплаты
                        anyDistributed |= this.AcceptImportedPayment(
                            importedPaymentsToAccept,
                            amountDistrType,
                            moneyOperation,
                            bankDocumentImport,
                            indicatorProxy,
                            progressLogger,
                            logger,
                            4000);

                        logger.StopTimer("AcceptPayments");
                    }
                    catch (Exception exception)
                    {
                        progressLogger.SetError(exception.ToString());

                        // логгируем ошибку подтверждения пачки
                        var errorDoc =
                            $"дата операции: {bankDocumentImport.ImportDate:dd.MM.yyyy}, дата реестра: {bankDocumentImport.DocumentDate:dd.MM.yyyy}, номер реестра: {bankDocumentImport.DocumentNumber}: {exception.Message}";

                        errorDocuments.Add(errorDoc);
                        this.LogManager.LogError($"Ошибка подтверждения реестра({errorDoc})", exception);
                    }

                    // если ни одной оплаты не распредилилось, удаляем операцию
                    if (!anyDistributed)
                    {
                        this.Container.InTransaction(() => this.SetPaymentsNotDistributedInternal(bankDocumentImport.Id, moneyOperation: moneyOperation));
                    }
                    else
                    {
                        logger.StartTimer("StateUpdate");

                        // здесь мы это всё и проверим, статусы и т.п. у оплат
                        this.Container.InTransaction(() =>
                        {
                            var distributedPaymentsCount = allImportedPaymentQuery
                                .Count(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed);

                            var impossiblePaymentsCount = allImportedPaymentQuery
                                .Count(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.ConfirmationImpossible);

                            var foundedPaCount = allImportedPayment.Count(x => x.PersAccNotNull);

                            // проставить Статус определения ЛС
                            if (foundedPaCount == 0 && allImportedPayment.Count > 0)
                            {
                                bankDocumentImport.PersonalAccountDeterminationState = PersonalAccountDeterminationState.NotDefined;
                            }
                            else if (foundedPaCount != allImportedPayment.Count)
                            {
                                bankDocumentImport.PersonalAccountDeterminationState = PersonalAccountDeterminationState.PartiallyDefined;
                            }
                            else if (foundedPaCount == allImportedPayment.Count)
                            {
                                bankDocumentImport.PersonalAccountDeterminationState = PersonalAccountDeterminationState.Defined;
                            }

                            // проставить Статус подтверждения оплат
                            if (impossiblePaymentsCount == allImportedPayment.Count)
                            {
                                bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.ConfirmationImpossible;
                            }
                            else if (distributedPaymentsCount == 0 && allImportedPayment.Count > 0)
                            {
                                bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.NotDistributed;
                            }
                            else if (distributedPaymentsCount != allImportedPayment.Count)
                            {
                                bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.PartiallyDistributed;
                                countOfPartiallyConfirmed++;
                            }
                            else if (distributedPaymentsCount == allImportedPayment.Count)
                            {
                                countOfConfirmed++;
                                bankDocumentImport.PaymentConfirmationState = PaymentConfirmationState.Distributed;
                            }

                            bankDocumentImport.CheckState = BankDocumentImportCheckState.NotChecked;

                            if (bankDocumentImport.PaymentConfirmationState == PaymentConfirmationState.Distributed)
                            {
                                bankDocumentImport.SetAccepted();
                            }

                            this.BankDocumentImportDomain.Update(bankDocumentImport);
                        });

                        logger.StopTimer("StateUpdate");
                    }
                }
                catch (Exception exception)
                {
                    // логгируем падение сессии
                    using (this.Container.BeginScope())
                    {
                        this.Container.InTransaction(() =>
                        {
                            progressLogger.SetError(exception.ToString());

                            var errorDoc = $"дата операции: {bankDocumentImport.ImportDate:dd.MM.yyyy}, " +
                                $"дата реестра: {bankDocumentImport.DocumentDate:dd.MM.yyyy}, " +
                                $"номер реестра: {bankDocumentImport.DocumentNumber}, " +
                                $"ошибка: {exception.Message}";

                            errorDocuments.Add(errorDoc);

                            this.LogManager.LogError($"Ошибка подтверждения реестра({errorDoc})", exception);

                            this.SetPaymentsNotDistributedInternal(bankDocumentImport.Id, moneyOperation: moneyOperation);
                        });
                    }
                }
                finally
                {
                    logger.StopTimer("AcceptDocument");
                    logger.SaveLogs(collector, x => x.OrderByDescending(y => y.TimeSpan).First());
                    logger.ClearSession();
                    
                    this.Container.Release(collector);
                    this.Container.Release(logger);

                    this.SaveLogs(LogOperationType.AcceptBankDocumentImport, progressLogger);
                }
            }

            var errorMessages = string.Empty;
            if (errorDocuments.Any())
            {
                errorMessages = Environment.NewLine + "Реестры с ошибкой:" +
                                Environment.NewLine + errorDocuments.AggregateWithSeparator("," + Environment.NewLine);
            }

            var stringMessages = $"Подтверждено {countOfConfirmed}, частично подтверждено: {countOfPartiallyConfirmed} реестров оплат.{errorMessages}";
            return new BaseDataResult(errorMessages.IsEmpty(), stringMessages);
        }

        /// <inheritdoc />
        public IDataResult CancelOperation(MoneyOperation operation, RepaymentParameters parameter)
        {
            ArgumentChecker.NotNull(operation, nameof(operation));
            ArgumentChecker.NotNull(parameter, nameof(parameter));

            var sumBefore = parameter.UndoTransfers.Sum(x => x.Amount);

            var bankDocumentImport = this.BankDocumentImportDomain.GetAll().First(x => x.TransferGuid == operation.OriginatorGuid);

            var anyPayments = this.ImportedPaymentDomain.GetAll()
                   .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                   .WhereContains(x => x.PaymentType, BankDocumentImportService.paymentCommands)
                   .Any();

            var anyRefunds = this.ImportedPaymentDomain.GetAll()
                .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                .WhereContains(x => x.PaymentType, BankDocumentImportService.refundCommands)
                .Any();

            var cancelOperationParams = new CancelOperationParams
            {
                Operation = operation,
                Period = this.ChargePeriodRepo.GetCurrentPeriod(),
                CancelOperationNum = 0,
                Operations = new List<MoneyOperation> { operation },
                SummariesToSave = new HashSet<PersonalAccountPeriodSummary>(),
                TransfersToSave = new List<Transfer>(),
                AnyPayments = anyPayments,
                AnyRefunds = anyRefunds
            };

            var realityObjectPaymentSession = this.Container.Resolve<IRealtyObjectPaymentSession>();

            using (this.Container.Using(realityObjectPaymentSession))
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.CancelOperationInternal(cancelOperationParams);

                    cancelOperationParams.TransfersToSave.ForEach(this.TransferDomain.Save);
                    cancelOperationParams.SummariesToSave.ForEach(this.PersonalAccountPeriodSummaryDomain.SaveOrUpdate);

                    var paymentsByPersAcc = cancelOperationParams.TransfersToSave.Cast<PersonalAccountPaymentTransfer>().GroupBy(x => x.Owner).ToDictionary(x => x.Key);
                    var importedPayments = this.ImportedPaymentDomain.GetAll()
                        .WhereContains(x => x.PersonalAccount, paymentsByPersAcc.Keys)
                        .AsEnumerable()
                        .GroupBy(x => x.PersonalAccount)
                        .ToDictionary(x => x.Key.Id);

                    var sumAfter = 0m;
                    foreach (var paymentByPersAcc in paymentsByPersAcc)
                    {
                        var payments = importedPayments.Get(paymentByPersAcc.Key.Id);
                        var comparedPayment = PaymentComparator.Compare(paymentByPersAcc.Key, payments, paymentByPersAcc.Value);

                        if (comparedPayment.IsNotEmpty())
                        {
                            sumAfter += comparedPayment.Keys.Sum(x => x.Sum);

                            var acceptOperation = bankDocumentImport.CreateOperation(cancelOperationParams.Period);
                            this.MoneyOperationDomain.Save(acceptOperation);
                            foreach (var paymentKvp in comparedPayment)
                            {
                                paymentKvp.Key.PersonalAccount = parameter.OwnerToRepayment.To<BasePersonalAccount>();
                                this.ApplyPayments(
                                    bankDocumentImport,
                                    acceptOperation,
                                    paymentKvp.Key,
                                    BankDocumentImportService.GetDistributionType(bankDocumentImport)).ForEach(this.TransferDomain.Save);
                            }
                        }
                    }

                    if (sumAfter != sumBefore)
                    {
                        throw new ValidationException("Сумма списания не равна сумме зачисления");
                    }

                    realityObjectPaymentSession.Complete();
                    this.RecalcEventManager.SaveEvents();
                    transaction.Commit();
                }
                catch
                {
                    realityObjectPaymentSession.Rollback();
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult SetPaymentsNotDistributed(BankDocumentImport document)
        {
            try
            {
                this.Container.InTransaction(() => this.SetPaymentsNotDistributedInternal(document.Id));
            }
            catch (Exception exception)
            {
                this.LogManager.LogError(exception, "Ошибка актуализации статусов после неуспешного подтвреждения реестра");
                return BaseDataResult.Error(exception.Message);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Получение типа распределения суммы оплат
        /// </summary>
        /// <param name="bankDocumentImport">Документ</param>
        /// <returns></returns>
        private static AmountDistributionType GetDistributionType(BankDocumentImport bankDocumentImport)
        {
            if (bankDocumentImport == null)
            {
                return AmountDistributionType.Tariff;
            }

            return bankDocumentImport.DistributePenalty == YesNo.Yes
                ? AmountDistributionType.TariffAndPenalty
                : AmountDistributionType.Tariff;
        }

        private void CheckPayment(BankDocumentImport bankDocumentImport)
        {
            LogOperation log = null;
            var results = new List<BankDocumentImportCheckReport.Result>();

            var transferRepository = this.Container.Resolve<ITransferRepository<PersonalAccountPaymentTransfer>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            try
            {
                log = new LogOperation
                {
                    OperationType = LogOperationType.CheckBankDocumentImport,
                    StartDate = DateTime.Now,
                    User = userManager.GetActiveUser(),
                    Comment =
                                  string.Format(
                                      "Номер реестра {0}. Дата реестра {1}. Сумма реестра {2}.",
                                      bankDocumentImport.DocumentDate?.ToDateTime().ToShortDateString() ?? "",
                                      bankDocumentImport.DocumentNumber.ToStr(),
                                      bankDocumentImport.ImportedSum)
                };

                var success = true;
                var payments =
                    this.ImportedPaymentDomain.GetAll()
                        .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                        .Where(x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                        .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed)
                        .Select(
                            x =>
                                new
                                {
                                    PersonalAccountId = x.PersonalAccount.Id,
                                    x.PersonalAccount.PersonalAccountNum,
                                    BaseTariffWallet = x.PersonalAccount.BaseTariffWallet.WalletGuid,
                                    DecisionTariffWallet = x.PersonalAccount.DecisionTariffWallet.WalletGuid,
                                    PenaltyWallet = x.PersonalAccount.PenaltyWallet.WalletGuid,
                                    SocialSupportWallet = x.PersonalAccount.SocialSupportWallet.WalletGuid,
                                    x.Sum
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.PersonalAccountId)
                        .ToDictionary(
                            x =>
                                x.First()
                                    .Return(
                                        y =>
                                            new
                                            {
                                                y.PersonalAccountNum,
                                                y.BaseTariffWallet,
                                                y.DecisionTariffWallet,
                                                y.PenaltyWallet,
                                                y.SocialSupportWallet
                                            }),
                            x => x.SafeSum(y => y.Sum));

                bankDocumentImport.DistributedSum = 0;
                bankDocumentImport.AcceptedSum = 0;

                var transfers = transferRepository.GetByOriginatorGuid(bankDocumentImport.TransferGuid);
                foreach (var payment in payments)
                {
                    var expectedSum = payment.Value;
                    bankDocumentImport.AcceptedSum += expectedSum;

                    var wallets = new List<string>
                                      {
                                          payment.Key.BaseTariffWallet,
                                          payment.Key.DecisionTariffWallet,
                                          payment.Key.PenaltyWallet,
                                          payment.Key.SocialSupportWallet
                                      };

                    var actualSum = transfers.Where(x => !x.Operation.IsCancelled).WhereContains(x => x.TargetGuid, wallets).Sum(x => (decimal?)x.Amount)
                                    ?? 0;
                    if (expectedSum != actualSum)
                    {
                        success = false;
                    }

                    bankDocumentImport.DistributedSum += actualSum;

                    results.Add(
                        new BankDocumentImportCheckReport.Result { ЛС = payment.Key.PersonalAccountNum, Подтверждено = expectedSum, Учтено = actualSum });
                }

                bankDocumentImport.CheckState = success ? BankDocumentImportCheckState.Checked : BankDocumentImportCheckState.Error;
            }
            finally
            {
                this.Container.Release(transferRepository);

                if (log != null)
                {
                    log.EndDate = DateTime.Now;

                    var docDate = bankDocumentImport.DocumentDate ?? DateTime.Now;
                    var docNum = bankDocumentImport.DocumentNumber ?? "Номер отсутствует";
                    var docSum = bankDocumentImport.ImportedSum ?? 0m;

                    var argument = new Arguments { { "docDate", docDate }, { "docNum", docNum }, { "docSum", docSum }, { "results", results } };
                    var report =
                        (StimulReport)
                        this.Container.Resolve<IBaseReport>(
                            "BankDocumentImportCheckReport",
                            argument);
                    var fmgr = this.Container.Resolve<IFileManager>();
                    var logDomain = this.Container.ResolveDomain<LogOperation>();
                    try
                    {
                        var stream = report.GetGeneratedReport();
                        log.LogFile = fmgr.SaveFile(stream, "Результаты проверки реестра.xlsx");

                        logDomain.Save(log);
                    }
                    finally
                    {
                        this.Container.Release(fmgr);
                        this.Container.Release(report);
                        this.Container.Release(logDomain);
                    }
                }
            }
        }

        private IDataResult CreateAcceptPaymentsTask(BaseParams baseParams)
        {
            var provider = this.Container.Resolve<ISessionProvider>();
            try
            {
                using (var session = provider.GetCurrentSession())
                {
                    if (session.Query<TableLock>().Count(x => x.TableName == "regop_transfer") > 0)
                    {
                        return new BaseDataResult(false, "Выполнение действия невозможно, так как таблица заблокирована. Повторите попытку после снятия блокировки."
                                                         + " В зависимости от настроек приложения, блокировка будет автоматически выключена либо после завершения расчета, либо после успешного завершения закрытия периода.");
                    }

                    return this.TaskManager.CreateTasks(new BankDocumentImportSelectedTaskProvider(this.Container), baseParams);
                }
            }
            finally
            {
                this.Container.Release(provider);
            }
        }

        private IDataResult CreateCancelPaymentsTask(BaseParams baseParams)
        {
            var provider = this.Container.Resolve<ISessionProvider>();
            try
            {
                using (var session = provider.GetCurrentSession())
                {
                    if (session.Query<TableLock>().Count(x => x.TableName == "regop_transfer") > 0)
                    {
                        return new BaseDataResult(false, "Выполнение действия невозможно, так как таблица заблокирована. Повторите попытку после снятия блокировки."
                            + " В зависимости от настроек приложения, блокировка будет автоматически выключена либо после завершения расчета, либо после успешного завершения закрытия периода.");
                    }

                    return this.TaskManager.CreateTasks(new BankDocumentImportCancelSelectedTaskProvider(this.Container), baseParams);
                }
            }
            finally
            {
                this.Container.Release(provider);
            }
        }

        private IDataResult ProcessInternalAcceptPayments(long[] importedPaymentIds, BankDocumentImport bankDocumentImport, ChargePeriod chargePeriod)
        {
            IDataResult result = new BaseDataResult();
            try
            {
                this.Container.InTransaction(() =>
                {
                    result = this.AcceptPayments(importedPaymentIds, bankDocumentImport, chargePeriod);
                    if (result.Success)
                    {
                        this.RecalcEventManager.SaveEvents();
                    }
                });

                return result;
            }
            catch (TableLockedException)
            {
                return
                    BaseDataResult.Error(
                        "Выполнение действия невозможно, так как таблица заблокирована. Повторите попытку после снятия блокировки. В зависимости от настроек приложения, блокировка будет автоматически выключена либо после завершения расчета, либо после успешного завершения закрытия периода.");
            }
            catch (ValidationException exception)
            {
                if (exception.EntityType == typeof(RealityObjectPaymentAccount))
                {
                    return BaseDataResult.Error(exception.Message);
                }

                return
                    BaseDataResult.Error(" message: {0} {1}\r\n stacktrace: {2}".FormatUsing(exception.Message, exception.InnerException,
                        exception.StackTrace));
            }
            catch (Exception exception)
            {
                return
                    BaseDataResult.Error(" message: {0} {1}\r\n stacktrace: {2}".FormatUsing(exception.Message, exception.InnerException,
                        exception.StackTrace));
            }
        }

        private IPersonalAccountRefundCommand GetUndoRefundCommand(RefundType type = RefundType.CrPayments)
        {
            return new PersonalAccountRefundCommand(type);
        }

        private IPersonalAccountPaymentCommand[] GetUndoPaymentCommands()
        {
            return new IPersonalAccountPaymentCommand[]
            {
                new PersonalAccountTariffPaymentCommand(),
                new PersonalAccountSocialSupportPaymentCommand()
            };
        }

        private IList<BasePersonalAccount> GetPersonalAccounts(
            MoneyOperation operation, 
            IDictionary<long, PersonalAccountPeriodSummary> periodSummaryDict, 
            bool anyPayments,
            bool anyRefunds)
        {
            var persAccHashSet = new HashSet<BasePersonalAccount>();

            var transferQuery = this.TransferDomain.GetAll().Where(x => x.Operation.Id == operation.Id && x.ChargePeriod.Id == operation.Period.Id);

            this.BasePersonalAccountDomain.GetAll()
                .Where(x => transferQuery.Any(y => y.Owner == x))
                .Fetch(x => x.BaseTariffWallet)
                .Fetch(x => x.DecisionTariffWallet)
                .Fetch(x => x.PenaltyWallet)
                .Fetch(x => x.SocialSupportWallet)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .ForEach(x =>
                {
                    persAccHashSet.Add(x);
                    x.SetOpenedPeriodSummary(periodSummaryDict?.Get(x.Id));
                });

            if (anyPayments)
            {
                persAccHashSet.FetchMainWalletInTransfers(operation.Period);
            }

            if (anyRefunds)
            {
                persAccHashSet.FetchMainWalletOutTransfers(operation.Period);
            }

            if (periodSummaryDict.IsEmpty())
            {
                persAccHashSet.FetchCurrentOpenedPeriodSummary();
            }

            return persAccHashSet.ToList();
        }

        private void SetPaymentsDistributed(BankDocumentImport bankDocumentImport, IList<long> impossibles)
        {
            var docImport = this.BankDocumentImportDomain.Get(bankDocumentImport.Id);
            docImport.SetAccepted();

            var anyNotDistributed = this.ImportedPaymentDomain.GetAll()
                .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                .Where(x => !x.Accepted)
                .Any(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.NotDistributed
                    || x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Deleted);

            docImport.PaymentConfirmationState = anyNotDistributed
                ? PaymentConfirmationState.PartiallyDistributed
                : PaymentConfirmationState.Distributed;

            this.BankDocumentImportDomain.Update(docImport);

            this.Container.InTransactionInNewScope(() =>
            {
                this.ImportedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.Id == bankDocumentImport.Id)
                    .Where(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.WaitingCancellation)
                    .Where(x => x.PersonalAccountDeterminationState == ImportedPaymentPersAccDeterminateState.Defined)
                    .ForEach(x =>
                    {
                    var oldState = x.PaymentConfirmationState;

                        if (!x.Id.In(impossibles))
                        {
                            x.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Distributed;
                            x.Accepted = true;
                            //try
                            //{
                            //    UnconfirmedPayments unconfirmedPayment = UnconfirmedPaymentsDomain.GetAll()
                            //    .Where(z=> z.IsConfirmed == YesNo.No)
                            //    .Where(z => z.PersonalAccount == x.PersonalAccount)
                            //    .Where(z => z.PaymentDate == x.PaymentDate)
                            //    .Where(z => z.Sum == x.Sum).FirstOrDefault();
                            //    if (unconfirmedPayment != null)
                            //    {
                            //        unconfirmedPayment.IsConfirmed = YesNo.Yes;
                            //        UnconfirmedPaymentsDomain.Update(unconfirmedPayment);
                            //    }

                            //}
                            //catch
                            //{

                            //}
                        }
                        else
                        {
                            x.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.ConfirmationImpossible;
                            x.Accepted = false;
                        }

                    this.stateHistoryManager.CreateStateHistory(x, oldState, x.PaymentConfirmationState);
                        this.ImportedPaymentDomain.Update(x);
                 
                    });
            });
            this.stateHistoryManager.SaveStateHistories();
        }

        private void SetPaymentsNotDistributedInternal(
            long bankDocumentImportId, 
            IList<long> paymentIds = null, 
            MoneyOperation moneyOperation = null)
        {
            var docImport = this.BankDocumentImportDomain.Get(bankDocumentImportId);

            var countDistributed = this.ImportedPaymentDomain.GetAll()
                    .Count(x => x.BankDocumentImport.Id == bankDocumentImportId && x.Accepted
                            && x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed);

            var countNotDistributed = this.ImportedPaymentDomain.GetAll()
                    .Count(x => x.BankDocumentImport.Id == bankDocumentImportId
                            && x.PaymentConfirmationState != ImportedPaymentPaymentConfirmState.Distributed);

            docImport.PaymentConfirmationState = countNotDistributed == 0 
                ? PaymentConfirmationState.Distributed
                : countDistributed > 0
                    ? PaymentConfirmationState.PartiallyDistributed
                    : PaymentConfirmationState.NotDistributed;

            if (countNotDistributed == 0 && countDistributed > 0)
            {
                docImport.SetAccepted();
            }
            else
            {
                docImport.SetInPending();
            }

            this.BankDocumentImportDomain.Update(docImport);

            var query = this.SessionProvider.GetCurrentSession()
                .CreateSQLQuery($@"UPDATE regop_imported_payment
                    SET accepted = false,
                        pc_state = 10
                    WHERE bank_doc_id = :doc_id AND pc_state = :f_state 
                    {(paymentIds.IsNotEmpty() ? "AND id in (:ids)" : string.Empty)}")

                .SetParameter("f_state", (int)ImportedPaymentPaymentConfirmState.WaitingConfirmation)
                .SetParameter("doc_id", bankDocumentImportId);

            if (paymentIds.IsNotEmpty())
            {
                query.SetParameter("ids", paymentIds);
            }

            query.ExecuteUpdate();

            if (moneyOperation?.Id > 0)
            {
                this.MoneyOperationDomain.Delete(moneyOperation.Id);
            }
        }

        private bool AcceptImportedPayment(
            IList<ImportedPayment> allImportedPayment, 
            AmountDistributionType amountDistrType, 
            MoneyOperation operation, 
            BankDocumentImport bankDocumentImport, 
            ProgressIndicatorProxy indicator, 
            ProcessLogger progressLogger,
            IPerformanceLogger logger = null, 
            int portionSize = 1000)
        {
            int current = 0;
            bool anyDistributed = false;

            logger?.StartTimer("RecalcEventManager", $"Количество оплат {allImportedPayment.Count}");

            var types = new[] { ImportedPaymentType.Basic, ImportedPaymentType.Payment, ImportedPaymentType.ChargePayment, ImportedPaymentType.Sum };
            this.RecalcEventManager.InitCache(
                            allImportedPayment
                                //TODO: убрать, когда добавятся перерасчет при возвратах, пока нет смысла инициализировать кэш на возвраты
                                .Where(x => x.PaymentType.In(types))
                                .GroupBy(x => x.PaymentDate)
                                .ToDictionary(
                                    x => x.Key,
                                    y => y.Select(x => x.PersonalAccount).Distinct().ToArray()));
            logger?.StopTimer("RecalcEventManager");

            foreach (var importedPayments in allImportedPayment.Section(portionSize))
            {
                var confirmedInTransaction = new List<Tuple<ImportedPayment, List<Transfer>>>();
                var realityObjectPaymentSession = this.Container.Resolve<IRealtyObjectPaymentSession>();

                logger?.StartTimer("ProgressProtion");
                using (var transaction = this.Container.Resolve<IDataTransaction>())
                using (this.Container.Using(transaction, realityObjectPaymentSession))
                {
                    var transfers = new List<Transfer>(portionSize);

                    // пробуем подтвердить пачку оплат
                    try
                    {
                        foreach (var importedPayment in importedPayments)
                        {
                            indicator.IndicatePercentage((double)current++ / allImportedPayment.Count);

                            var oldState = importedPayment.PaymentConfirmationState;
                            var personalAccount = importedPayment.PersonalAccount;

                            logger?.StartTimer("ApplySinglePayment");
                            if (personalAccount != null)
                            {
                                try
                                {
                                    var currentTransfers = this.ApplyPayments(bankDocumentImport, operation, importedPayment, amountDistrType);
                                    transfers.AddRange(currentTransfers);

                                    importedPayment.Accepted = true;
                                    importedPayment.AcceptDate = DateTime.Now;
                                    importedPayment.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.Distributed;

                                    confirmedInTransaction.Add(Tuple.Create(importedPayment, currentTransfers));
                                }
                                catch (RefundException exception)
                                {
                                    importedPayment.Accepted = false;
                                    importedPayment.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.ConfirmationImpossible;
                                    progressLogger.SetError(importedPayment, exception.Message);
                                }
                                catch (Exception exception)
                                {
                                    importedPayment.Accepted = false;
                                    importedPayment.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.NotDistributed;
                                    progressLogger.SetError(importedPayment, exception.ToString());
                                }
                            }
                            else
                            {
                                importedPayment.PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.NotDefined;
                                importedPayment.PersonalAccountNotDeterminationStateReason = PersonalAccountNotDeterminationStateReason.AccountNumber;
                                importedPayment.PaymentConfirmationState = ImportedPaymentPaymentConfirmState.NotDistributed;
                                importedPayment.Accepted = false;
                                progressLogger.SetError(importedPayment, "Лицевой счёт не определён, подтверждение невозможно");
                            }

                            this.stateHistoryManager.CreateStateHistory(importedPayment, oldState, importedPayment.PaymentConfirmationState);

                            if(importedPayment.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed)
                            {
                                //try
                                //{
                                //    UnconfirmedPayments unconfirmedPayment = UnconfirmedPaymentsDomain.GetAll()
                                //    .Where(z => z.IsConfirmed == YesNo.No)
                                //    .Where(z => z.PersonalAccount == importedPayment.PersonalAccount)
                                //    .Where(z => z.PaymentDate == importedPayment.PaymentDate)
                                //    .Where(z => z.Sum == importedPayment.Sum).FirstOrDefault();
                                //    if (unconfirmedPayment != null)
                                //    {
                                //        unconfirmedPayment.IsConfirmed = YesNo.Yes;
                                //        UnconfirmedPaymentsDomain.Update(unconfirmedPayment);
                                //    }

                                //}
                                //catch
                                //{

                                //}
                            }
                            this.ImportedPaymentDomain.Update(importedPayment);
                            logger?.StopTimer("ApplySinglePayment");

                            if (personalAccount != null)
                            {
                                var summary = personalAccount.GetOpenedPeriodSummary();
                                this.PersonalAccountPeriodSummaryDomain.Update(summary);
                            }
                        }

                        transfers.ForEach(this.TransferDomain.Save);

                        // пробуем посадить деньги на дом
                        logger?.StartTimer("realityObjectPaymentSession");
                        realityObjectPaymentSession.Complete();
                        logger?.StopTimer("realityObjectPaymentSession");


                        logger?.StartTimer("RecalcEventManager.SaveEvents");
                        this.RecalcEventManager.SaveEvents();
                        logger?.StopTimer("RecalcEventManager.SaveEvents");

                        logger?.StartTimer("StateHistoryManager.SaveStateHistories");
                        this.stateHistoryManager.SaveStateHistories();
                        logger?.StopTimer("StateHistoryManager.SaveStateHistories");

                        transaction.Commit();
                        anyDistributed = true;
                    }
                    catch
                    {
                        realityObjectPaymentSession.Rollback();
                        transaction.Rollback();
                        throw;
                    }
                }
                logger?.StopTimer("ProgressProtion");

                // успешность подтверждения оплат запишем только, если всё посадим из пачки в транзакции
                foreach (var payment in confirmedInTransaction)
                {
                    progressLogger.SetSucess(payment.Item1, payment.Item2);
                }
            }

            return anyDistributed;
        }

        private List<Transfer> ApplyPayments(
            BankDocumentImport bankDocumentImport, 
            MoneyOperation operation, 
            ImportedPayment importedPayment,
            AmountDistributionType amountDistrType)
        {
            var refundCommand = this.RefundCommandFactory.GetCommand(importedPayment.PaymentType);

            if (refundCommand != null)
            {
                return importedPayment.PersonalAccount.ApplyRefund(
                    refundCommand,
                    new MoneyStream(
                        bankDocumentImport,
                        operation,
                        importedPayment.PaymentDate,
                        Gkh.Utils.DecimalExtensions.RegopRoundDecimal(importedPayment.Sum, 2))
                    {
                        Description = "Подтверждение возврата",
                        IsAffect = true
                    })
                    .ToList();
            }

            var command = this.CommandFactory.GetCommand(importedPayment.PaymentType);

            return importedPayment.PersonalAccount.ApplyPayment(
                command,
                new MoneyStream(
                    bankDocumentImport,
                    operation,
                    importedPayment.PaymentDate,
                    Gkh.Utils.DecimalExtensions.RegopRoundDecimal(importedPayment.Sum, 2))
                    {
                        Description = "Подтверждение оплаты"
                    }, 
                amountDistrType);
        }

        private void SaveLogs(LogOperationType logType, ProcessLogger logger)
        {
            using (this.Container.BeginScope())
            {
                this.Container.InTransaction(() =>
                {
                    var file = this.FileManager.SaveFile(logger.FileName, Encoding.GetEncoding(1251).GetBytes(logger.GetLogs()));

                    this.LogOperationDomainService.Save(new LogOperation
                    {
                        OperationType = logType,
                        StartDate = logger.StartDate,
                        EndDate = DateTime.Now,
                        Comment = $"Дата операции: {logger.Document.ImportDate:dd.MM.yyyy}, " +
                            $"дата реестра: {logger.Document.DocumentDate:dd.MM.yyyy}, " +
                            $"номер реестра: {logger.Document.DocumentNumber}, " +
                            $"успешно: {logger.Count}, " +
                            $"с ошибкой: {logger.CountErrors}",
                        LogFile = file,
                        User = this.UserManager.GetActiveUser()
                    });
                });
            }
        }

        private class ProcessLogger
        {
            private readonly IDictionary<ImportedPayment, string> logs;

            public DateTime StartDate { get; }

            public BankDocumentImport Document { get; }

            public int Count { get; private set; }

            public int CountErrors { get; private set; }

            public IList<string> ErrorMessages { get; }

            public string FileName => $"{this.Document.Id}_{DateTime.Now:yyyyMMdd}.csv";

            public ProcessLogger(BankDocumentImport document)
            {
                this.StartDate = DateTime.Now;
                this.Document = document;
                this.ErrorMessages = new List<string>();

                this.logs = new Dictionary<ImportedPayment, string>();
            }

            public void SetSucess(ImportedPayment payment, IEnumerable<Transfer> transfers)
            {
                this.Count++;
                this.logs[payment] = this.FormatPayment(payment, transfers.Select(x => x.Amount.ToString(CultureInfo.InvariantCulture)).AggregateWithSeparator(","));
            }

            public void SetError(ImportedPayment payment, string errorMessage)
            {
                this.CountErrors++;
                this.logs[payment] = this.FormatPayment(payment, errorMessage);
            }

            public void SetError(string error)
            {
                this.ErrorMessages.Add(error);
            }

            public string GetLogs()
            {
                var stringBuilder = new StringBuilder();

                if (this.ErrorMessages.Any())
                {
                    stringBuilder.AppendLine("Ошибки подтверждения реестра:");

                    foreach (var errorMessage in this.ErrorMessages)
                    {
                        stringBuilder.AppendLine(errorMessage);
                    }
                }

                stringBuilder.AppendLine("Лицевой счёт;Сумма оплаты;Статус оплаты;Сообщение;");

                foreach (var log in this.logs)
                {
                    stringBuilder.AppendLine(log.Value);
                }

                return stringBuilder.ToString();
            }

            private string FormatPayment(ImportedPayment payment, string message)
            {
                return $"{payment.PersonalAccount?.PersonalAccountNum ?? payment.Account};{payment.Sum};{payment.PaymentConfirmationState.GetDisplayName()};{message};";
            }
        }

        private class ProgressIndicatorProxy
        {
            private readonly bool accept;
            private readonly double coef;

            private uint savingPercent;

            public ProgressIndicatorProxy(IProgressIndicator indicator, int count, bool accept, double coef = 1.0)
            {
                this.Indicator = indicator;
                this.Count = count;
                this.Current = -1;

                this.accept = accept;
                this.coef = coef;

                this.savingPercent = (uint)(100 * this.coef);
            }

            public ProgressIndicatorProxy(IProgressIndicator indicator, BankDocumentImport document, bool accept) : this(indicator, 1, accept)
            {
                this.Document = document;
            }

            public IProgressIndicator Indicator { get; }

            public BankDocumentImport Document { get; private set; }

            public int Current { get; private set; }

            public int Count { get; }

            public void SetCurrent(BankDocumentImport document)
            {
                this.Current++;
                this.Document = document;
            }

            public void SetSaving()
            {
                this.Indicator?.Report(null, Math.Min(this.savingPercent++, 100), "Сохранение данных");
            }

            public void IndicatePercentage(double percentage)
            {
                if (this.Document == null || this.Indicator == null)
                {
                    return;
                }

                var detailMessage = this.accept
                    ? "Подтверждается реестр({0} из {1}): дата операции: {2:dd.MM.yyyy}, дата реестра: {3:dd.MM.yyyy}, номер реестра: {4}"
                    : "Отменяется реестр({0} из {1}): дата операции: {2:dd.MM.yyyy}, дата реестра: {3:dd.MM.yyyy}, номер реестра: {4}";

                var message = detailMessage.FormatUsing(
                    this.Current + 1,
                    this.Count,
                    this.Document.ImportDate,
                    this.Document.DocumentDate,
                    this.Document.DocumentNumber);

                var percent = Math.Min(100, ((double)this.Current / this.Count + percentage) * this.coef * 100);
                this.Indicator.Report(this.Document, (uint)percent, message);
            }
        }

        private class CancelOperationParams
        {
            public IList<MoneyOperation> Operations { get; set; }
            public MoneyOperation Operation { get; set; }

            public ChargePeriod Period { get; set; }

            public IDictionary<long, PersonalAccountPeriodSummary> SummaryCache { get; set; }

            public List<Transfer> TransfersToSave { get; set; }

            public HashSet<PersonalAccountPeriodSummary> SummariesToSave { get; set; }

            public int CancelOperationNum { get; set; }

            public IPerformanceLogger Logger { private get; set; }

            public ProgressIndicatorProxy Indicator { private get; set; }

            public bool AnyRefunds { get; set; }
            public bool AnyPayments { get; set; }

            public void IndicatePercents(double percents)
            {
                this.Indicator?.IndicatePercentage(percents + (double)this.CancelOperationNum / this.Operations.Count);
            }

            public void StartTimer(string key, string description = null)
            {
                this.Logger?.StartTimer(key, description);
            }

            public void StopTimer(string key)
            {
                this.Logger?.StopTimer(key);
            }
        }
    }
}