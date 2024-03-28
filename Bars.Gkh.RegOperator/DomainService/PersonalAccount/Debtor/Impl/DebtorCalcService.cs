namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Debtor;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Log;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Core;

    /// <summary>
    /// Сервис вычисления должников
    /// </summary>
    public partial class DebtorCalcService : IDebtorCalcService, IInitializable
    {
        private FundFormationVariant fundFormationVariant;
        private ChargePeriod period;
        private long[] personalAccountStates;
        private decimal? penaltyDebt;
        private DebtorTariffSumChecker debtorTariffSumChecker;
        private int expirationDaysCount;
        private bool checkDays;
        private DateTime lastDate;
        private PaymentPenalties[] penaltyParams;
        private DebtorLogicalOperands operand;

        private IList<long> accountIds;
        private PeriodSummaryDto[] summaries;
        private RecalcHistoryProxy[] recalcHistory;
        private IList<RecalcHistoryProxy> allRecalcHistory;
        private Dictionary<long, Dictionary<DateTime, decimal>> charges;
        private Dictionary<long, decimal> paymentSums;
        private Dictionary<long, SumData> accountDebts;
        private Dictionary<long, DateTime> lastPaymentDates;
        private Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> roFundDecision;
        private IProcessLog log;

        public IChargePeriodRepository ChargePeriodRepository { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }
        public IDomainService<RecalcHistory> RecalcHistoryDomain { get; set; }
        public IDomainService<PersonalAccountPaymentTransfer> TransferDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccService { get; set; }
        public IDomainService<PaymentPenalties> PenaltyParamsDomain { get; set; }
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }
        public IDomainService<CalculationParameterTrace> CalculationParameterTraceDomain { get; set; }
        public IRealityObjectDecisionsService RoDecisionService { get; set; }
        public IDebtorJurInstitutionCache JurInstitutionCache { get; set; }
        public IGkhConfigProvider GkhConfigProvider { get; set; }

        /// <inheritdoc />
        public int BulkSize => 10000;

        /// <inheritdoc />
        public void InitRecalcHistory()
        {
            this.allRecalcHistory = this.RecalcHistoryDomain.GetAll()
                .Where(x => x.RecalcSum != 0) //берем периоды с непустыми начислениями
                .Where(x => x.RecalcType != RecalcType.Penalty)
                .Select(x => new RecalcHistoryProxy
                {
                    AccountId = x.PersonalAccount.Id,
                    RecalcSum = x.RecalcSum,
                    RecalcPeriod = x.RecalcPeriod
                })
                .ToList();
        }

        /// <inheritdoc />
        public IQueryable<BasePersonalAccount> GetQuery()
        {
            return this.GetQueryInternal();
        }

        /// <summary>
        /// Вычисление неплательщиков
        /// </summary>
        /// <param name="skip">Число пропускаемых</param>
        /// <param name="take">Число берущихся</param>
        /// <param name="log">Лог</param>
        public List<Debtor> GetDebtors(int skip, int take, IProcessLog log = null)
        {
            var query = this.GetQueryInternal();

            if (take > 0)
            {
                query = query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take);
            }

            return this.GetDebtorsInternal(query, log:log);
        }

        ///// <summary>
        ///// Вычисление неплательщиков
        ///// </summary>
        ///// <param name="query">Запрос</param>
        ///// <param name="log">Лог</param>
        //public List<Debtor> GetDebtors(IQueryable<BasePersonalAccount> query, IProcessLog log = null)
        //{
        //    ArgumentChecker.NotNull(query, nameof(query));

        //    return this.GetDebtorsInternal(query, log:log);
        //}

        public IDataResult CheckDebtor(Debtor debtor)
        {
            string daysMessage;
            string tariffMessage;
            string penaltyMessage;

            var dateCheck = this.CheckDate(debtor, out daysMessage);
            var tariffCheck = this.debtorTariffSumChecker.Check(debtor, out tariffMessage);
            var penaltyCheck = this.CheckPenaltyDebt(debtor, out penaltyMessage);

            bool check = false;

            switch (this.operand)
            {
                case DebtorLogicalOperands.And:
                    check = dateCheck && tariffCheck && (penaltyCheck || !this.penaltyDebt.HasValue);
                    break;

                case DebtorLogicalOperands.Or:
                    check = dateCheck && (tariffCheck || penaltyCheck);
                    break;
            }

            return new BaseDataResult(check, $"{daysMessage}. {tariffMessage}. {penaltyMessage}.");
        }

        /// <inheritdoc />
        public IDictionary<long, DebtorInfo> GetDebtorsInfo(IQueryable<BasePersonalAccount> query, IProcessLog log)
        {
            this.log = log;
            ArgumentChecker.NotNull(query, nameof(query));
            var result = new Dictionary<long, DebtorInfo>();

            foreach (var account in this.InitPortionCache(query))
            {
                var debtor = this.ProcessAccount(account, true);

                if (debtor != null)
                {
                    result.Add(account.Id, new DebtorInfo
                    {
                        PersonalAccountId = account.Id,
                        OwnerId = account.OwnerId,
                        OwnerType = account.OwnerType,
                        OwnerName = account.OwnerName,
                        DebtSum = debtor.DebtSum,
                        DebtBaseTariffSum = debtor.DebtBaseTariffSum,
                        DebtDecisionTariffSum = debtor.DebtDecisionTariffSum,
                        PenaltyDebt = debtor.PenaltyDebt,
                        StartDate = debtor.StartDate,
                        ExpiredDaysCount = debtor.ExpirationDaysCount,
                        ExpiredMonthCount = debtor.ExpirationMonthCount,
                        IsDebtPaidResult = this.CheckDebtor(debtor)
                    });
                }
            }

            return result;
        }

        private List<Debtor> GetDebtorsInternal(IQueryable<BasePersonalAccount> query, bool notCheck = false, IProcessLog log = null)
        {
            this.log = log;

            var accountData = this.InitPortionCache(query);

            var resultList = new List<Debtor>();

            foreach (var account in accountData)
            {
                var debtor = this.ProcessAccount(account, notCheck);

                if (debtor != null)
                {
                    resultList.Add(debtor);
                }
            }

            return resultList;
        }

        private IQueryable<BasePersonalAccount> GetQueryInternal()
        {
            var query = this.BasePersonalAccService.GetAll()
                .Where(x=> x.ObjectCreateDate < DateTime.Now.AddMonths(-3))
                .WhereIf(!this.personalAccountStates.IsEmpty(), x => this.personalAccountStates.Contains(x.State.Id));

            // если включена хотя бы одна настройка, то фильтруем по способу формирования фонда
            if (this.fundFormationVariant.RegopCalcAccount ||
                this.fundFormationVariant.RegopSpecialCalcAccount ||
                this.fundFormationVariant.SpecialCalcAccount ||
                this.fundFormationVariant.Unknown)
            {
                query = query
                    .WhereIf(!this.fundFormationVariant.RegopCalcAccount,
                        x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.RegOpAccount)

                    .WhereIf(!this.fundFormationVariant.RegopSpecialCalcAccount,
                        x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.SpecialRegOpAccount)

                    .WhereIf(!this.fundFormationVariant.SpecialCalcAccount,
                        x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.SpecialAccount)

                    .WhereIf(!this.fundFormationVariant.Unknown,
                        x => x.Room.RealityObject.AccountFormationVariant != null
                            && x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.Unknown
                            && x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.NotSelected)
                    .Where(x => !x.IsNotDebtor && !x.InstallmentPlan );
                
            }

            return query;
        }

        public void Initialize()
        {
            var config = this.GkhConfigProvider.Get<RegOperatorConfig>().DebtorConfig;

            this.fundFormationVariant = config.FundFormationVariant;

            this.period = this.ChargePeriodRepository.GetCurrentPeriod();

            this.operand = config.DebtorRegistryConfig.DebtOperand;

            var states = config.DebtorRegistryConfig.States.Where(x => x != null);

            this.personalAccountStates = states
                .Select(x => x.Id)
                .ToArray();

            this.debtorTariffSumChecker = new DebtorTariffSumChecker(config);

            this.penaltyDebt = config.DebtorRegistryConfig.PenaltyDebt.ToDecimal();

            this.expirationDaysCount = config.DebtorRegistryConfig.ExpirationDaysCount;

            this.checkDays = this.expirationDaysCount > 0;

            this.lastDate = DateTime.Today.AddDays(-this.expirationDaysCount);

            this.penaltyParams = this.PenaltyParamsDomain.GetAll()
                .OrderByDescending(x => x.DateStart)
                .ToArray();

            this.log.SafeInfo(
                "Параметры формирования реестра неплательщиков: "
                + $"Количество дней просрочки: {this.expirationDaysCount}. "
                + this.debtorTariffSumChecker.ParamMessage
                + $"Сумма задолженности по пени: {this.penaltyDebt}. "
                + $"Тип операции: {this.operand.GetEnumMeta().Display}. "
                + $"Статусы ЛС: {states.AggregateWithSeparator(x => x.Name, ", ")}.");
        }

        private BasePersonalAccountDto[] InitPortionCache(IQueryable<BasePersonalAccount> queryLs)
        {
            var listLs = queryLs.Select(x => x.Id).Distinct().ToList();
            var query = BasePersonalAccService.GetAll().Where(x => listLs.Contains(x.Id));
            var roIdQuery = query.Select(x => x.Room.RealityObject.Id);
            var accountData = query
                .Select(
                    x => new BasePersonalAccountDto
                    {
                        Id = x.Id,
                        OwnerId = x.AccountOwner.Id,
                        OwnerName = x.AccountOwner.Name,
                        RealityObjectId = x.Room.RealityObject.Id,
                        MunicipalityId = x.Room.RealityObject.Municipality.Id,
                        MoSettlementId = x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Id : 0,
                        PersonalAccountNum = x.PersonalAccountNum,
                        OwnerType = x.AccountOwner.OwnerType,

                        BaseWalletGuid = x.BaseTariffWallet.WalletGuid,
                        DecisionWalletGuid = x.DecisionTariffWallet.WalletGuid,
                        PenaltyWalletGuid = x.PenaltyWallet.WalletGuid,
                        RestructAmicAgrWalletGuid = x.RestructAmicableAgreementWallet.WalletGuid,

                        Balance = x.BaseTariffWallet.Balance
                            + x.DecisionTariffWallet.Balance
                            + x.RentWallet.Balance
                            + x.PreviosWorkPaymentWallet.Balance
                            + x.AccumulatedFundWallet.Balance
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToArray();
            this.accountIds = accountData.Select(x => x.Id).ToList();

            this.roFundDecision = this.RoDecisionService.GetRobjectsFundFormation(roIdQuery);
            this.paymentSums = this.GetPaymentSums(accountData);

            this.summaries = this.PeriodSummaryDomain.GetAll()
                .WhereContainsBulked(x => x.PersonalAccount.Id, this.accountIds, this.BulkSize)
                .Select(x => new PeriodSummaryDto
                {
                    AccountId = x.PersonalAccount.Id,
                    Change = x.BaseTariffChange + x.DecisionTariffChange,
                    Charge = x.ChargeTariff,
                    Debt = x.BaseTariffDebt + x.DecisionTariffDebt,
                    DebtBaseTariff = x.BaseTariffDebt,
                    DebtDecisionTariff = x.DecisionTariffDebt,
                    Payment = x.TariffPayment + x.TariffDecisionPayment,
                    TariffPayment = x.TariffPayment,
                    TariffDecisionPayment = x.TariffDecisionPayment,
                    PenaltyDebt = x.PenaltyDebt,
                    PenaltyPayment = x.PenaltyPayment,
                    Period = x.Period,
                    Recalc = x.RecalcByBaseTariff + x.RecalcByDecisionTariff,
                    RecalcByBaseTariff = x.RecalcByBaseTariff,
                    RecalcByDecisionTariff = x.RecalcByDecisionTariff,
                    RecalcByPenalty = x.RecalcByPenalty,
                    BaseTariffChange = x.BaseTariffChange,
                    DecisionTariffChange = x.DecisionTariffChange,
                    PenaltyChange = x.PenaltyChange,
                    ChargedByBaseTariff = x.ChargedByBaseTariff,
                    Penalty = x.Penalty
                })
                .ToArray();

            if (this.allRecalcHistory == null)
            {
                this.recalcHistory = this.RecalcHistoryDomain.GetAll()
                    .Where(x => x.RecalcType != RecalcType.Penalty)
                    .Where(x => x.RecalcSum != 0) //берем периоды с непустыми начислениями
                    .WhereContainsBulked(x => x.PersonalAccount.Id, this.accountIds, this.BulkSize)
                    .Select(x => new RecalcHistoryProxy
                    {
                        AccountId = x.PersonalAccount.Id,
                        RecalcSum = x.RecalcSum,
                        RecalcPeriod = x.RecalcPeriod
                    })
                    .ToArray();
            }
            else
            {
                this.recalcHistory = this.allRecalcHistory
                    .Where(x => this.accountIds.Contains(x.AccountId))
                    .ToArray();
            }

            this.charges = this.CalculateCharges();
            this.accountDebts = this.CalculateAccountDebts();
            this.lastPaymentDates = this.GetLastPaymentDates(accountData);
            this.JurInstitutionCache.InitCache(roIdQuery);

            return accountData;
        }

        private Debtor ProcessAccount(BasePersonalAccountDto account, bool notCheck)
        {
            var summary = this.accountDebts.Get(account.Id);

            if (summary == null)
            {
                this.log.SafeWarning($"ЛС: {account.PersonalAccountNum}, отсутствует информация о задолженностях");
                return null;
            }
           

            var debtor = new Debtor
            {
                PersonalAccount = new BasePersonalAccount
                {
                    Id = account.Id,
                    AccountOwner = new PersonalAccountOwner(account.OwnerName)
                    {
                        Id = account.OwnerId,
                        OwnerType = account.OwnerType
                    }
                }
            };

            this.JurInstitutionCache.SetJurInstitution(debtor, account);

            this.SetDate(debtor, account);
            this.SetTariffDebt(debtor, account);
            this.SetPenaltyDebt(debtor, account);

            var check = this.CheckDebtor(debtor);

            this.log.SafeInfo(check.Message, account.PersonalAccountNum);

            if (check.Success || notCheck)
            {
                this.log.SafeInfo("Добавлен в реестр неплательщиков", account.PersonalAccountNum);
                return debtor;
            }

            return null;
        }

        private bool CheckDate(Debtor debtor, out string message)
        {
            message = string.Empty;
            if (debtor.StartDate == null)
            {
                return false;
            }

            if (!this.checkDays)
            {
                message = "Количество дней просрочки: Не указано";
                return true;
            }

            if (this.lastDate > debtor.StartDate)
            {
                message = "Количество дней просрочки: " + debtor.ExpirationDaysCount;
                return true;
            }

            message = $"Количество дней просрочки: {debtor.ExpirationDaysCount}, требуется: {this.expirationDaysCount}";
            return false;
        }

        private bool CheckPenaltyDebt(Debtor debtor, out string message)
        {
            if (!this.penaltyDebt.HasValue)
            {
                message = "Сумма задолженности по пени: Не указано";
                return false;
            }

            if (debtor.PenaltyDebt >= this.penaltyDebt)
            {
                message = $"Сумма задолженности по пени: {debtor.PenaltyDebt}";
                return true;
            }
            else
            {
                message = $"Сумма задолженности по пени: {debtor.PenaltyDebt}, требуется: {this.penaltyDebt.Value.RegopRoundDecimal(2)}";
                return false;
            }
        }

        private void SetDate(Debtor debtor, BasePersonalAccountDto account)
        {
            var startDebt = this.GetDateStartDebtor(account);
            debtor.StartDate = startDebt;

            int months;
            int days = Utils.CalculateDaysAndMonths(startDebt, out months);

            debtor.ExpirationDaysCount = days;
            debtor.ExpirationMonthCount = months;
        }

        private void SetPenaltyDebt(Debtor debtor, BasePersonalAccountDto account)
        {
            var debts = this.accountDebts.Get(account.Id);
            debtor.PenaltyDebt = debts?.Penalty ?? 0;
        }

        private void SetTariffDebt(Debtor debtor, BasePersonalAccountDto account)
        {
            var debts = this.accountDebts.Get(account.Id);
            debtor.DebtSum = debts?.Tariff ?? 0;
            debtor.DebtBaseTariffSum = debts?.BaseTariff ?? 0;
            debtor.DebtDecisionTariffSum = debts?.DecisionTariff ?? 0;
        }

        private DateTime GetDateStartDebtor(BasePersonalAccountDto account)
        {
            var accountId = account.Id;
            var dateStartDebtor = DateTime.Today;
            var factCharges = this.charges.Get(accountId)?.OrderBy(x => x.Key);

            if (factCharges != null)
            {
                var roId = account.RealityObjectId;

                var lastPaymentDate = this.lastPaymentDates.ContainsKey(accountId)
                    ? this.lastPaymentDates.Get(accountId)
                    : DateTime.Today;

                var paymentSum = this.paymentSums.Get(accountId); //всего оплачено за периоды по бт и тр

                DateTime start;
                DateTime obligationDate;
                PaymentPenalties penalty;

                var isPaidInTime = false;

                foreach (var periodCharge in factCharges)
                {
                    paymentSum -= periodCharge.Value;

                    if (paymentSum <= 0)
                    {
                        start = periodCharge.Key;
                        penalty = this.GetCurrentPenaltyParameter(roId, start);
                        obligationDate = start.AddDays(penalty.Return(x => x.Days));

                        if (paymentSum == 0 && lastPaymentDate < obligationDate && !isPaidInTime)
                        {
                            isPaidInTime = true; //оплатил все вовремя, будет должен только со следующего начисления
                            continue;
                        }

                        dateStartDebtor = obligationDate;
                        break;
                    }
                }
            }
            return dateStartDebtor;
        }
    }
}