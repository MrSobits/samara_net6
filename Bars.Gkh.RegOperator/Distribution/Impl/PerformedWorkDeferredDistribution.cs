namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Extensions.Expressions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Dapper;

    using NHibernate.Linq;

    /// <summary>
    /// Отложенное распределение средств за ранее выполненные работы
    /// <para>Средства используются для оплаты новых начислений</para>
    /// </summary>
    public class PerformedWorkDeferredDistribution : PerformedWorkDistribution, IPerformedWorkDetailService
    {
        private const string PerformedWorkCharged = "Зачисление зачёта средств за выполненные работы";
        private const string PerformedWorkTaked = "Снятие зачета средств за выполненные работы";

        private IDbConnection connection;

        private IDictionary<long, Dictionary<Tuple<long, WalletType>, decimal>> distributedCache =
            new Dictionary<long, Dictionary<Tuple<long, WalletType>, decimal>>();

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<PersonalAccountCharge> ChargeDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RecalcHistory" />
        /// </summary>
        public IDomainService<RecalcHistory> RecalcHistoryDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Базовая операция по зачету средств
        /// </summary>
        public IDomainService<PerformedWorkChargeSource> PerfWorkChargeSourceDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Операция по зачету средств
        /// </summary>
        public IDomainService<PerformedWorkCharge> PerfWorkChargeDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Период начислений
        /// </summary>
        public IDomainService<ChargePeriod> PeriodDomain { get; set; }

        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Репозиторий периодов
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <inheritdoc />
        public IDataResult ListDistributed(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("accId");
            var loadParams = baseParams.GetLoadParam();

            var chargeSources = this.PerfWorkChargeSourceDomain.GetAll()
                .FetchMany(x => x.PerformedWorkCharges)
                .Where(x => x.PersonalAccount.Id == accountId)
                .OrderBy(x => x.Id)
                .ToDictionary(x => x, x => x.PerformedWorkCharges.ToList());

            var result = new List<DataProxy>();

            foreach (var chargeSource in chargeSources)
            {
                result.Add(
                    new DataProxy
                    {
                        PeriodName = chargeSource.Key.OperationDate.ToString("yyyy MMMM", CultureInfo.GetCultureInfo("ru-RU")),
                        Sum = chargeSource.Key.Sum,
                        TypeOperation = PerformedWorkDeferredDistribution.PerformedWorkCharged
                    });

                result.AddRange(
                    chargeSource.Value
                        .GroupBy(y => Tuple.Create(y.ChargePeriod, y.DistributeType))
                        .OrderBy(y => y.Key.Item1.Id)
                        .Select(
                            y => new DataProxy
                            {
                                DistributionType = y.Key.Item2,
                                PeriodName = y.Key.Item1.Name,
                                Sum = y.Sum(z => -z.Sum), // снятие с минусом!
                                TypeOperation = PerformedWorkDeferredDistribution.PerformedWorkTaked
                            }));
            }

            return new ListDataResult(result.AsQueryable().Order(loadParams).ToList(), result.Count);
        }

        /// <inheritdoc />
        public IList<PeriodOperationDetail> GetSupposedDistributions(BasePersonalAccount account)
        {
            var query = this.PersonalAccountDomain.GetAll().Where(x => x == account);
            var result = new List<PeriodOperationDetail>();
            var currPeriod = this.PeriodRepository.GetCurrentPeriod();

            var sources = this.GetSources(query, currPeriod);
            foreach (var source in sources)
            {
                source.PerformedWorkCharges.Where(x => !x.Active)
                    .Select(x => new PeriodOperationDetail
                    {
                        TransferId = 0,
                        Date = currPeriod.GetCurrentInPeriodDate(),
                        Name = "Зачет средств за выполненные работы",
                        SaldoChange = -x.Sum,
                        Period = currPeriod.Name,
                        Document = source.Document
                    })
                    .AddTo(result);
            }

            return result;
        }

        /// <inheritdoc />
        public IDictionary<long, Tuple<decimal, decimal>> GetResultDistributionSum(
            IQueryable<BasePersonalAccount> accountQuery,
            IDbConnection connection = null)
        {
            var result = new Dictionary<long, Tuple<decimal, decimal>>();
            var currPeriod = this.PeriodRepository.GetCurrentPeriod();

            try
            {
                this.connection = connection;
                this.Container.InTransaction(() =>
                {
                    this.WarmCache(accountQuery);
                    var sources = this.GetSources(accountQuery, currPeriod);

                    // получаем предполагаемые распределения
                    foreach (var source in sources)
                    {
                        // удаляем предыдущие неактивные распределения
                        source.ClearUnactive();

                        var distributionResult = this.DistributeSource(source);
                        if (distributionResult.IsNotNull())
                        {
                            var sum = result.Get(source.PersonalAccount.Id) ?? Tuple.Create(0m, 0m);
                            var distributed = source.PerformedWorkCharges
                                .Where(x => !x.Active)
                                .GroupBy(x => x.DistributeType)
                                .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));

                            // сорян за такое решение, ибо надо было быстро сделать
                            result[source.PersonalAccount.Id] = Tuple.Create(
                                distributed.Get(WalletType.BaseTariffWallet) + sum.Item1,
                                distributed.Get(WalletType.DecisionTariffWallet) + sum.Item2);
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container, sources.Select(x => x.PersonalAccount.GetOpenedPeriodSummary()));
                    TransactionHelper.InsertInManyTransactions(this.Container, sources.Select(x => x.PersonalAccount));
                    TransactionHelper.InsertInManyTransactions(this.Container, sources);
                });
            }
            finally
            {
                this.connection = null;
            }

            return result;
        }

        /// <summary>
        /// Применить распределение по лс
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Apply(BaseParams baseParams)
        {
            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        using (new DatabaseMutexContext("Apply", "Применить распределение по лс"))
                        {
                            var distributionDate = baseParams.Params.GetAs<DateTime>("OperationDate");
                            var distributionReason = baseParams.Params.GetAs<string>("Reason");
                            var distributeForBaseTariff = baseParams.Params.GetAs<bool>("distributeForBaseTariff");
                            var distributeForDecisionTariff = baseParams.Params.GetAs<bool>("distributeForDecisionTariff");
                            var distributionDocument = baseParams.Files.ContainsKey("Document") ? baseParams.Files["Document"] : null;

                            FileInfo fileInfo = null;
                            if (distributionDocument != null)
                            {
                                fileInfo = this.FileManager.SaveFile(distributionDocument);
                                this.FileInfoDomain.Save(fileInfo);
                            }

                            var records = baseParams.Params.GetAs<List<AccountProxy>>("records");
                            var accIds = records.Select(x => x.Id).ToArray();

                            var accQuery = this.PersonalAccountDomain.GetAll()
                                .Where(x => accIds.Contains(x.Id));

                            var accounts = accQuery
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                            var currPeriod = this.PeriodRepository.GetCurrentPeriod();
                            var user = this.UserManager.GetActiveUser();

                            var chargeSources = records
                                .Select(
                                    x => new PerformedWorkChargeSource(distributionDate, accounts.Get(x.Id), x.DistributionSum, currPeriod)
                                    {
                                        Reason = distributionReason,
                                        Document = fileInfo,
                                        User = user?.Login,
                                        DistributeForBaseTariff = distributeForBaseTariff,
                                        DistributeForDecisionTariff = distributeForDecisionTariff
                                    })
                                .ToArray();

                            this.WarmCache(accQuery);

                            var listHistoryInfo = new List<PersistentObject>();
                            var results = new List<PersonalAccountOperationResult>();
                            foreach (var source in chargeSources)
                            {
                                if (distributionDate.Date <= currPeriod.StartDate.Date)
                                {
                                    var result = this.DistributeSource(source);
                                    if (result.IsNotNull())
                                    {
                                        results.Add(result);
                                    }
                                }

                                listHistoryInfo.Add(new PersonalAccountChange(
                                    source.PersonalAccount,
                                    this.GetDesctiptionText(source),
                                    PersonalAccountChangeType.PerformedWorkFundsDistribution,
                                    source.FactOperationDate,
                                    source.OperationDate,
                                    source.User,
                                    source.Sum.ToFormatedString(','),
                                    null,
                                    currPeriod)
                                {
                                    Document = fileInfo,
                                    Reason = source.Reason
                                });
                            }

                            TransactionHelper.InsertInManyTransactions(this.Container,
                                chargeSources.Select(x => x.PersonalAccount.GetOpenedPeriodSummary()));
                            TransactionHelper.InsertInManyTransactions(this.Container, chargeSources.Select(x => x.PersonalAccount));
                            TransactionHelper.InsertInManyTransactions(this.Container, chargeSources);
                            TransactionHelper.InsertInManyTransactions(this.Container, results.Select(x => x.Operation));
                            TransactionHelper.InsertInManyTransactions(this.Container, results.SelectMany(x => x.Transfers));
                            TransactionHelper.InsertInManyTransactions(this.Container, listHistoryInfo);
                        }
                    });

                return new BaseDataResult();
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Применить распределение уже запущено");
            }
        }

        /// <summary>
        /// Распределить за зачеты заранее выполненные работы для пакета ЛС
        /// </summary>
        /// <param name="personalAccounts">Список ЛС</param>
        public override void DistributeForAccountPacket(IQueryable<BasePersonalAccount> personalAccounts)
        {
            //this.Container.InTransaction(() =>
            //{
            //    this.LogManager.DebugFormat("Начало распределения зачетов, количество ЛС:{0}", personalAccounts.Count());
            //    this.WarmCache(personalAccounts);
            this.Container.InTransaction(() =>
            {
                this.LogManager.LogDebug("Начало распределения зачетов, количество ЛС:{0}", personalAccounts.Count());
                this.WarmCache(personalAccounts);

                var currPeriod = this.PeriodRepository.GetCurrentPeriod(false);

                var sources = this.GetSources(personalAccounts, currPeriod);

                this.LogManager.LogDebug("Количество источников для распределения: {0}", sources.Count());

            //    var currPeriod = this.PeriodRepository.GetCurrentPeriod(false);

            //    var sources = this.GetSources(personalAccounts, currPeriod);

            //    this.LogManager.DebugFormat("Количество источников для распределения: {0}", sources.Count());

            //    var results = new List<PersonalAccountOperationResult>();

            //    foreach (var source in sources)
            //    {
            //        var unactive = source.PerformedWorkCharges.Where(x => !x.Active).ToArray();

            //        if (unactive.Any())
            //        {
            //            var distrResult = new PersonalAccountOperationResult(source.CreateOperation(currPeriod));
            //            foreach (var performedWorkCharge in unactive)
            //            {
            //                distrResult.AddTransfer(source.ApplyCharge(performedWorkCharge, applyPeriodSummary: false));
            //            }

            //            if (source.GetBalance(true) == 0)
            //            {
            //                source.Distributed = true;
            //            }

            //            results.Add(distrResult);
            //        }
            //    }

            //    TransactionHelper.InsertInManyTransactions(this.Container, sources.Select(x => x.PersonalAccount.GetOpenedPeriodSummary()));
            //    TransactionHelper.InsertInManyTransactions(this.Container, sources.Select(x => x.PersonalAccount));
            //    TransactionHelper.InsertInManyTransactions(this.Container, sources);
            //    TransactionHelper.InsertInManyTransactions(this.Container, results.Select(x => x.Operation));
            //    TransactionHelper.InsertInManyTransactions(this.Container, results.SelectMany(x => x.Transfers));
            //});
            });
        }

        /// <summary>
        /// Получить остаток зачетов за ранее выполненные работы
        /// <para>С учетом предполагаемых зачетов</para>
        /// </summary>
        /// <param name="accountId">ЛС</param>
        public override decimal GetPerformedWorkChargeBalance(long accountId)
        {
            var balance = this.PerfWorkChargeSourceDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == accountId)
                .Where(x => !x.Distributed)
                .ToArray()
                .Sum(x => x.GetBalance());

            return balance;
        }

        private PersonalAccountOperationResult DistributeSource(PerformedWorkChargeSource source)
        {
            var account = source.PersonalAccount;

            var currPeriod = this.PeriodRepository.GetCurrentPeriod();

            var firstPeriodId = this.PeriodDomain.GetAll()
                .Where(x => x.StartDate == source.OperationDate)
                .Select(x => x.Id)
                .FirstOrDefault();

            var charges = this.GetChargesInternal(account, firstPeriodId);

            // если не выбрали ни одной галки, то перерасчёты не должны подгрузиться вообще
            Expression<Func<RecalcHistory, bool>> expression = x => false;

            if (source.DistributeForBaseTariff)
            {
                expression = expression.Or(x => x.RecalcType == RecalcType.BaseTariffCharge);
            }

            if (source.DistributeForDecisionTariff)
            {
                expression = expression.Or(x => x.RecalcType == RecalcType.DecisionTariffCharge);
            }

            // TODO: ждём ответа по перерасчёту зачёта средств от СА
            //var recalcHistories = this.RecalcHistoryDomain.GetAll()
            //    .Where(expression)
            //    .Where(x => x.PersonalAccount == account)
            //    .Where(x => x.RecalcPeriod.Id >= firstPeriodId)
            //    .Select(x => new
            //    {
            //        RecalcPeriodId = x.RecalcPeriod.Id,
            //        x.RecalcSum,
            //        x.RecalcType
            //    })
            //    .AsEnumerable()
            //    .GroupBy(x => Tuple.Create(x.RecalcPeriodId, x.RecalcType.ToWalletType()))
            //    .ToDictionary(x => x.Key, y => y.Sum(x => x.RecalcSum));

            this.LogManager.LogDebug(
                $"ЛС: {account.PersonalAccountNum}, " +
                $"currPeriodName: {currPeriod.Name}, " +
                $"количество начислений: {charges.Length}, " +
                $"баланс: {source.GetBalance(true)}:");

            if (charges.Length == 0)
            {
                return null;
            }

            var perfWorkChargeDict = this.distributedCache.Get(account.Id);
            var moneyOperation = source.CreateOperation(currPeriod);
            var result = new PersonalAccountOperationResult(moneyOperation);

            var sum = source.GetBalance(true);

            foreach (var charge in charges)
            {
                foreach (var walletType in source.GetDistributableWalletTypes())
                {
                    var need = this.GetCharged(charge, walletType) - (perfWorkChargeDict?.Get(charge.Period.Id, walletType) ?? 0m);

                    //var recalcSum = recalcHistories.Get(charge.ChargePeriod.Id, walletType);

                    var howMuch = Math.Min(need, sum);

                    // перерасчёт учитываем всегда, т.е. можем баланс распределения увеличить
                    // howMuch -= recalcSum;

                    if (howMuch != 0)
                    {
                        this.LogManager.LogDebug("Сумма распределения ({0}): {1}", walletType.GetDisplayName(), howMuch);

                        var active = charge.Period.IsClosed || charge.Period.IsClosing;
                        result.AddTransfer(source.AddCharge(charge.Period, walletType, howMuch, active));

                        sum -= howMuch;
                    }

                    if (sum == 0)
                    {
                        if (source.GetBalance(true) == 0)
                        {
                            source.Distributed = true;
                        }

                        break;
                    }
                }
            }

            if (source.GetBalance(true) == 0)
            {
                source.Distributed = true;
            }

            this.LogManager.LogDebug("Количество трансферов: {0}", result.Transfers.Count);

            return result;
        }

        private ChargeDto[] GetChargesInternal(BasePersonalAccount account, long firstPeriodId)
        {
            ChargeDto[] result;

            // данное разделение сделано на тот случай, если нам нужна информация из транзакции Dapper'а во время расчётов
            if (this.connection.IsNotNull())
            {
                var sql = $@"SELECT 
                    ch.charge_tariff as ChargeTariff, 
                    ch.overplus as OverPlus,
                    ch.penalty as Penalty,
                    p.id as Id,
                    p.period_name as Name,
                    p.cstart as StartDate,
                    p.cend as EndDate,
                    p.cis_closed as IsClosed,
                    p.is_closing as IsClosing
                    FROM regop_pers_acc_charge ch
                      JOIN regop_period p ON ch.period_id = p.id
                    WHERE ch.pers_acc_id = {account.Id} AND ch.is_active AND ch.period_id >= {firstPeriodId}";

                result = this.connection.Query<ChargeDto, ChargePeriod, ChargeDto>(
                        sql,
                        (x, y) =>
                        {
                            x.Period = y;
                            return x;
                        })
                    .ToArray();
            }
            else
            {
                result = this.ChargeDomain.GetAll()
                    .Where(x => x.IsActive)
                    .Where(x => x.BasePersonalAccount.Id == account.Id)
                    .Where(x => x.ChargePeriod.Id >= firstPeriodId)
                    .Select(x => new ChargeDto
                    {
                        Period = x.ChargePeriod,
                        ChargeTariff = x.ChargeTariff,
                        Penalty = x.Penalty,
                        OverPlus = x.OverPlus
                    })
                    .ToArray();
            }

            return result;
        }

        private IEnumerable<PerformedWorkChargeSource> GetSources(IQueryable<BasePersonalAccount> personalAccounts, ChargePeriod currPeriod)
        {
            return this.PerfWorkChargeSourceDomain.GetAll()
                .FetchMany(x => x.PerformedWorkCharges)
                .Where(x => x.OperationDate.Date <= currPeriod.StartDate.Date)
                .Where(x => personalAccounts.Any(y => y.Id == x.PersonalAccount.Id))
                .Where(x => !x.Distributed)
                .ToArray();
        }

        private void WarmCache(IQueryable<BasePersonalAccount> accounts)
        {
            this.distributedCache = this.PerfWorkChargeSourceDomain.GetAll()
                .FetchMany(x => x.PerformedWorkCharges)
                .Where(x => accounts.Any(y => y.Id == x.PersonalAccount.Id))
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.SelectMany(z => z.PerformedWorkCharges)
                        .GroupBy(x => Tuple.Create(x.ChargePeriod.Id, x.DistributeType))
                        .ToDictionary(x => x.Key, c => c.Where(z => z.Active).Sum(z => z.Sum))); // распределенные - активные
        }

        private decimal GetCharged(ChargeDto charge, WalletType walletType)
        {
            switch (walletType)
            {
                case WalletType.BaseTariffWallet:
                    return charge.ChargeTariff - charge.OverPlus;

                case WalletType.DecisionTariffWallet:
                    return charge.OverPlus;

                default:
                    throw new ArgumentOutOfRangeException(nameof(walletType), walletType, null);
            }
        }

        private string GetDesctiptionText(PerformedWorkChargeSource source)
        {
            var walletNames = source.GetDistributableWalletTypes().Select(x => x.GetDisplayName().ToLower()).AggregateWithSeparator(", ");
            var periodName = this.ChargePeriodRepository.GetPeriodByDate(source.OperationDate)?.Name
                ?? source.OperationDate.ToString("yyyy MMMM", CultureInfo.GetCultureInfo("ru-RU"));

            var sum = source.Sum.ToFormatedString(',');

            return $"Зачет средств {walletNames} с периода {periodName} на сумму {sum} рублей";
        }

        private class ChargeDto
        {
            public decimal ChargeTariff { get; set; }

            public decimal OverPlus { get; set; }

            public decimal Penalty { get; set; }

            public ChargePeriod Period { get; set; }
        }

        private class DataProxy
        {
            public string PeriodName { get; set; }

            public string TypeOperation { get; set; }

            public WalletType? DistributionType { get; set; }

            public decimal Sum { get; set; }
        }
    }
}