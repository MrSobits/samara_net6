namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.PerformanceLogging;

    using Castle.Windsor;
    using Dto;

    /// <summary>
    /// Кеш для расчетов
    /// </summary>
    public partial class CalculationGlobalCache : ICalculationGlobalCache, IDisposable
    {
        private readonly IWindsorContainer container;
        private readonly IEntityLogCache entityLogCache;
        private readonly ITariffCache tariffCache;
        private readonly IPerformanceLogger performanceLogger;
        private bool initialized;
        private readonly object _lock = new object();

        /// <summary>
        /// .ctor
        /// </summary>
        public CalculationGlobalCache(IWindsorContainer container, IEntityLogCache entityLogCache, ITariffCache tariffCache, IPerformanceLogger performanceLogger)
        {
            this.container = container;
            this.entityLogCache = entityLogCache;
            this.tariffCache = tariffCache;
            this.performanceLogger = performanceLogger;

            this.listChargeReasonTransfers = new List<string>
            {
                "Начисление по базовому тарифу",
                "Начисление пени",
                "Перерасчет пени",
                "Перерасчет по базовому тарифу",
                "Начисление по тарифу решения",
                "Перерасчет по тарифу решения"
            };
        }

        public IEnumerable<ChargePeriod> GetClosedPeriods()
        {
            return this.periodCache;
        }

        public IEnumerable<PersonalAccountChargeDto> GetAllCharges(BasePersonalAccount account)
        {
            return this.accountChargeCache.Get(account.Id) ?? new PersonalAccountChargeDto[0];
        }

        /// <summary>
        /// Вернуть историю перерасчетов
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns></returns>
        public IEnumerable<RecalcHistoryDto> GetRecalcHistory(BasePersonalAccount account)
        {
            return this.accountRecalcHistory.Get(account.Id) ?? new RecalcHistoryDto[0];
        }

        /// <summary>
        /// Вернуть зепреты перерасчета
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns></returns>
        public IEnumerable<PersonalAccountBanRecalc> GetBanRecalc(BasePersonalAccount account)
        {
            return this.accountBanRecalc.Get(account.Id) ?? new PersonalAccountBanRecalc[0];
        }

        /// <summary>
        /// Вернуть зачеты средств
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns></returns>
        public IEnumerable<PerformedWorkCharge> GetPerfWorkCharge(BasePersonalAccount account)
        {
            return this.perfWorkCharge.Get(account.Id) ?? new PerformedWorkCharge[0];
        }

        /// <summary>
        /// Вернуть протоколы расчёта
        /// </summary>
        /// <param name="guid">Guid начислений</param>
        /// <param name="first">Берём данные протокола на начало периода в случае, если передать true</param>
        /// <returns></returns>
        [Obsolete("Ставку рефинансирования получаем из параметров")]
        public CalculationParameterTraceDto GetCalcParamTrace(string guid, bool first = true)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<TransferDto> GetPaymentTransfers(BasePersonalAccount account)
        {
            var baseTariffTransfers = this.transferCache.Get(account.BaseTariffWallet.WalletGuid) ?? new TransferDto[0];
            var desTariffTransfers = this.transferCache.Get(account.DecisionTariffWallet.WalletGuid) ?? new TransferDto[0];

            return baseTariffTransfers.Union(desTariffTransfers);
        }

        /// <summary>
        /// Получить отмененные оплаты
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns></returns>
        public IEnumerable<TransferDto> GetCancelPaymentTransfers(BasePersonalAccount account)
        {
            var baseTariffTransfers = this.cancelTransferCache.Get(account.BaseTariffWallet.WalletGuid) ?? new TransferDto[0];
            var desTariffTransfers = this.cancelTransferCache.Get(account.DecisionTariffWallet.WalletGuid) ?? new TransferDto[0];

            return baseTariffTransfers.Union(desTariffTransfers);
        }

        /// <summary>
        /// Получить отмененные оплаты по базовому тарифу
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns></returns>
        public IEnumerable<TransferDto> GetBaseTariffCancelPaymentTransfers(BasePersonalAccount account)
        {
            var baseTariffTransfers = this.cancelTransferCache.Get(account.BaseTariffWallet.WalletGuid) ?? new TransferDto[0];

            return baseTariffTransfers;
        }

        /// <summary>
        /// Получить оплаты по базовому тарифу
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<TransferDto> GetPaymentTransfersBaseTariff(BasePersonalAccount account)
        {
            var baseTariffTransfers = this.transferCache.Get(account.BaseTariffWallet.WalletGuid)
                ?.Where(x => x.Operation.Reason != "Перенос долга при слиянии")
                ?? new TransferDto[0];

            return baseTariffTransfers;
        }

        /// <summary>
        /// Получить возвраты по базовому тарифу
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<TransferDto> GetReturnTransfersBaseTariff(BasePersonalAccount account)
        {
            var baseTariffTransfers = this.transferReturnCache.Get(account.BaseTariffWallet.WalletGuid) ?? new TransferDto[0];

            return baseTariffTransfers;
        }

        /// <summary>
        /// Получить возвраты по тарифу решения
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns>возвраты по тарифу решения</returns>
        public IEnumerable<TransferDto> GetReturnTransfersDecisionTariff(BasePersonalAccount account)
        {
            TransferDto[] decisionTariffTransfers;
            if (this.transferReturnCache.TryGetValue(account.DecisionTariffWallet.WalletGuid, out decisionTariffTransfers))
            {
                return decisionTariffTransfers;
            }

            return new TransferDto[0];
        }

        /// <summary>
        /// Получить трансферы с кошелька
        /// по мировому соглашению
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<Transfer> GetRaaWalletTransfers(BasePersonalAccount account)
        {
            var transferDomain = this.container.ResolveDomain<PersonalAccountPaymentTransfer>();
            var raaWalletGuid = account.RestructAmicableAgreementWallet.WalletGuid;

            try
            {
                return transferDomain.GetAll()
                    .Where(
                        x => x.TargetGuid == raaWalletGuid
                            || (x.SourceGuid == raaWalletGuid && x.Operation.CanceledOperation != null))
                    .ToList();
            }
            finally
            {
                this.container.Release(transferDomain);
            }
        }

        /// <summary>
        /// Получить Зачеты средств за выполненные ранее работы
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns>Зачеты средств за выполненные ранее работы</returns>
        public IEnumerable<TransferDto> GetPerfWorks(BasePersonalAccount account)
        {
            var transfers = this.perfWorkCache.Get(account.BaseTariffWallet.WalletGuid) ?? new TransferDto[0];

            return transfers;
        }

        public IEnumerable<PersonalAccountPeriodSummaryDto> GetAllSummaries(BasePersonalAccount account)
        {
            return this.accountSummariesCache.Get(account.Id) ?? new PersonalAccountPeriodSummaryDto[0];
        }

        public IEnumerable<PaymentPenalties> GetPenaltyParams()
        {
            return this.penaltyParamCache;
        }

        public IEnumerable<PenaltyParameterValue<decimal>> GetPenaltyParameters()
        {
            return this.penaltyPercentages;
        }

        public IEnumerable<PenaltyParameterValue<int>> GetPenaltyDaysParameters()
        {
            return this.penaltyDebtDays;
        }

        public IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>> GetRoFundFormationType(BasePersonalAccount account)
        {
            var roId = account.Return(x => x.Room).Return(x => x.RealityObject).Return(x => x.Id);

            return this.roCrFundCache.Get(roId) ?? new List<Tuple<DateTime, CrFundFormationDecisionType>>();
        }

        /// <summary>
        /// Получить день начала и день окончания фиксированного пеирода
        /// </summary>
        /// <returns></returns>
        public FixedPeriodCalcPenalties GetFixPeriodCalc(DateTime start)
        {
            return this.fixperCalcPenaltieses
                .Where(x => x.DateStart <= start)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получить события для перерасчета по ЛС
        /// </summary>
        /// <param name="account">ЛС</param>
        public List<PersonalAccountRecalcEvent> GetRecalcEvents(BasePersonalAccount account)
        {
            var events = this.recalcEvents.Get(account.Return(x => x.Id)) ?? new List<PersonalAccountRecalcEvent>();

            if (this.recalcEvents.ContainsKey(0L))
            {
                events.AddRange(this.recalcEvents[0L]);
            }

            return events;
        }

        public IEnumerable<MonthlyFeeAmountDecision> GetFees(RealityObject realty)
        {
            return this.monthlyDecisions.Get(realty.Id);
        }

        public DecisionTariffDto GetDecisionTarif(RealityObject realty,  DateTime begin)
        {
            return (this.GetFees(realty) ?? new List<MonthlyFeeAmountDecision>())
                .Where(x => x.Decision != null)
                .Where(x => x.Decision.Any(z => z.From <= begin))
                .SelectMany(x => x.Decision.Select(
                    y => new DecisionTariffDto
                    {
                        Protocol = x.Protocol,
                        Decision = y
                    }))
                .OrderByDescending(x => x.Decision.From)
                .FirstOrDefault(x => !x.Decision.To.HasValue || x.Decision.To >= begin);
        }

        /// <summary>
        /// Получить параметр начисления пени по периоду
        /// и типу решения в доме
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public PaymentPenalties GetPeriodPenalty(DateTime dateStart, DateTime dateEnd, BasePersonalAccount account)
        {
            var roFundFormation = this.GetRoFundFormationType(account)
                .OrderByDescending(x => x.Item1)
                .FirstOrDefault(x => x.Item1 <= dateStart)
                .Return(x => x.Item2, CrFundFormationDecisionType.Unknown);

            if (roFundFormation == CrFundFormationDecisionType.Unknown)
            {
                var fakeValue = new PaymentPenalties()
                {
                    Percentage = 0,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    Days = (dateEnd - dateStart).TotalDays.ToInt(),
                    DecisionType = CrFundFormationDecisionType.Unknown
                };

                return fakeValue;
            }

            // здесь нам важна ставка рефинансирования на начало периода, 
            // чтобы потом отловить её изменения
            var value = this.GetPenaltyParams()
                .OrderByDescending(x => x.DateStart)
                .Where(x => x.DecisionType == roFundFormation)
                .FirstOrDefault(x => x.DateStart <= dateStart);

            return value.Return(x => x.Excludes.All(y => y.PersonalAccount.Id != account.Id)) ? value : null;
        }

        /// <summary>
        /// Получить параметр начисления пени на дату
        /// и типу решения в доме
        /// </summary>
        /// <param name="date"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public PaymentPenalties GetPeriodPenalty(DateTime date, BasePersonalAccount account)
        {
            var roFundFormation = this.GetRoFundFormationType(account)
                .OrderByDescending(x => x.Item1)
                .FirstOrDefault(x => x.Item1 <= date)
                .Return(x => x.Item2, CrFundFormationDecisionType.Unknown);

            var value = this.GetPenaltyParams()
                    .OrderByDescending(x => x.DateStart)
                    .Where(x => x.DecisionType == roFundFormation)
                    .FirstOrDefault(x => x.DateStart <= date);

            return value.Return(x => x.Excludes.All(y => y.PersonalAccount.Id != account.Id)) ? value : null;
        }

        /// <summary>
        /// Получить параметры начисления пени на даты
        /// и типу решения в доме
        /// </summary>
        public IEnumerable<KeyValuePair<DateTime, decimal>> GetPeriodPenalties(DateTime startDate, DateTime endDate, BasePersonalAccount account)
        {
            var roFundFormation = this.GetRoFundFormationType(account)
                .OrderByDescending(x => x.Item1)
                .FirstOrDefault(x => x.Item1 <= startDate)
                .Return(x => x.Item2, CrFundFormationDecisionType.Unknown);

            var penalties = this.GetPenaltyParameters()
                    .Where(x => x.FundFormationType == roFundFormation)
                    .Where(x => (x.DateStart >= startDate && x.DateStart <= endDate))
                    .ToList();
            var listResult = new List<KeyValuePair<DateTime, decimal>>();

            foreach (var penaltyParameterValue in penalties)
            {
                var dateStart = penaltyParameterValue.DateStart;

                foreach (var paymentPenalties in penaltyParameterValue.Source)
                {
                    // если в этом периоде ЛС исключен из расчёта, то ставим ставку на 0
                    if (paymentPenalties.Excludes.Any(x => x.PersonalAccount.Id == account.Id))
                    {
                        // создаём отметку только, если до этого была ненулевая ставка или нет разделений
                        if (listResult.IsEmpty() || listResult.Last().Value != 0M)
                        {
                            listResult.Add(new KeyValuePair<DateTime, decimal>(dateStart, 0M));
                        }

                        dateStart = paymentPenalties.DateEnd ?? DateTime.MaxValue;
                    }
                }

                // добавляем последний кусочек, если не во всех периодах ЛС исключен из расчётов
                // иными словами, либо весь период считается по 1 ставке, либо ставка 0 частично или полностью
                if (dateStart != (penaltyParameterValue.DateEnd ?? DateTime.MaxValue))
                {
                    listResult.Add(new KeyValuePair<DateTime, decimal>(dateStart, penaltyParameterValue.Value));
                }
            }

            return listResult;
        }

        /// <summary>
        /// Получить параметры количества допустимой просрочки
        /// и типу решения в доме
        /// </summary>
        public KeyValuePair<DateTime, int> GetPeriodDays(DateTime startDate, DateTime endDate, BasePersonalAccount account)
        {
            var roFundFormation = this.GetRoFundFormationType(account)
                .OrderByDescending(x => x.Item1)
                .FirstOrDefault(x => x.Item1 <= startDate)
                .Return(x => x.Item2, CrFundFormationDecisionType.Unknown);

            var val = this.GetPenaltyDaysParameters()
                .OrderByDescending(x => x.DateStart)
                .Where(x => x.FundFormationType == roFundFormation)
                .FirstOrDefault(x => endDate >= x.DateStart);

            var penaltyDelays = this.GetOwnerPenaltyDelays(account, startDate);

            var penaltyDelay = penaltyDelays
                .Where(x => x.From <= startDate)
                .FirstOrDefault(x => !x.To.HasValue || x.To >= startDate);

            if (penaltyDelay != null)
            {
                var value = penaltyDelay.MonthDelay
                    ? DateTime.DaysInMonth(startDate.Year, startDate.Month)
                    : penaltyDelay.DaysDelay;

                return new KeyValuePair<DateTime, int>(penaltyDelay.From, value);
            }

            return val?.ToKeyValuePair() ?? default(KeyValuePair<DateTime, int>);
        }

        /// <summary>
        /// Получить трансферы образованные от слияния ЛС (amount>0) в открытом периоде
        /// </summary>
        /// <param name="walletGuid">Гуид кошелька</param>
        /// <returns></returns>
        public IEnumerable<TransferDto> GetTransferFromAccountsInCurrentPeriod(string walletGuid)
        {
            return this.transferFromOtherAccountsCache.Get(walletGuid);
        }

        /// <summary>
        /// Получить отмены начислений в открытом периоде
        /// </summary>
        /// <param name="personalAccountId">Код ЛС</param>
        /// <param name="сancelType">Тип отмены</param>
        /// <returns></returns>
        public IEnumerable<CancelChargeDto> GetCancelChargesInCurrentPeriod(long personalAccountId, CancelType сancelType)
        {
            var cCharge = this.cancelChargeCache.Get(personalAccountId)?.Where(x => x.CancelType == сancelType);
            
            if (cCharge == null)
            {
                return new List<CancelChargeDto> { new CancelChargeDto { CancelSum = 0 }  };
            }

            return cCharge;
        }

        /// <summary>
        /// Получить все отмены начислений в открытом периоде
        /// </summary>
        /// <param name="personalAccountId">Код ЛС</param>
        /// <param name="сancelType">Тип отмены</param>
        /// <returns></returns>
        public IEnumerable<CancelChargeDto> GetAllCancelChargesInCurrentPeriod(long personalAccountId, CancelType сancelType)
        {
            var cCharge = this.allcancelChargeCache.Get(personalAccountId)?.Where(x => x.CancelType == сancelType);

            if (cCharge == null)
            {
                return new List<CancelChargeDto> { new CancelChargeDto { CancelSum = 0 } };
            }

            return cCharge;
        }

        /// <summary>
        /// Получить объект с кошельками на ЛС
        /// </summary>
        /// <param name="personalAccountId"></param>
        /// <returns></returns>
        public WalletDto GetWalletByAccountId(long personalAccountId)
        {
            return this.walletCache.Get(personalAccountId);
        }

        /// <summary>
        /// Получить параметры начисления пени на даты
        /// и типу решения в доме
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public PaymentPenalties GetPeriodPenalty(DateTime startDate, DateTime endDate, BasePersonalAccount account, out PaymentPenalties prevPenalty)
        {
            var roFundFormation = this.GetRoFundFormationType(account)
                .OrderByDescending(x => x.Item1)
                .FirstOrDefault(x => x.Item1 <= startDate)
                .Return(x => x.Item2, CrFundFormationDecisionType.Unknown);

            var data = this.GetPenaltyParams()
                .OrderByDescending(x => x.DateStart)
                .Where(x => x.DecisionType == roFundFormation)
                .Where(x => (x.DateStart <= startDate || (x.DateEnd >= endDate && x.DateStart <= endDate)) && x.DateStart >= (this.DatePenaltyCalcTo ?? DateTime.MinValue))
                .Take(2);

            prevPenalty = data.Skip(1).FirstOrDefault();

            return data.FirstOrDefault();
        }

        /// <summary>
        /// Получить расписание реструктуризации
        /// </summary>
        public IEnumerable<RestructDebtSchedule> GetRestructureSchedule(BasePersonalAccount account)
        {
            return this.schedules.Get(account.Id) ?? new RestructDebtSchedule[0];
        }

        public IEnumerable<RestructDebtScheduleDetail> GetRestructureScheduleDetails(BasePersonalAccount account)
        {
            return this.detailSchedules.Get(account.Id) ?? new RestructDebtScheduleDetail[0];
        }

        /// <summary>
        /// Получить дату документа реструктуризации
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DateTime? GetRestructDate(BasePersonalAccount account)
        {
            return this.schedules.Get(account.Id)?.FirstOrDefault()?.RestructDebt.DocumentDate;
        }

        /// <summary>
        /// Получить значения решений собственников по сроку уплаты взноса
        /// </summary>
        public IEnumerable<OwnerPenaltyDelay> GetOwnerPenaltyDelays(BasePersonalAccount account, DateTime actualDate)
        {
            var roId = account.Return(x => x.Room)
                .Return(x => x.RealityObject)
                .Return(x => x.Id);

            var actualParameter =
                (this.ownerDelayParams.Get(roId) ?? new List<Tuple<DateTime, List<OwnerPenaltyDelay>>>())
                    .OrderByDescending(x => x.Item1)
                    .FirstOrDefault(x => x.Item1 <= actualDate);

            return actualParameter.Return(x => x.Item2) ?? new List<OwnerPenaltyDelay>();
        }

        public DateTime? GetManuallyRecalcDate(BasePersonalAccount account)
        {
            return this.manuallyRecalcDates.Get(account.Id);
        }

        /// <summary>
        /// Включен ли модуль ПИР
        /// </summary>
        public bool IsClaimWorkEnabled { get; private set; }

        /// <summary>
        /// Рассчитывать пени
        /// </summary>
        public bool CalculatePenalty { get; private set; }


        /// <summary>
        /// Перерасчет только за период действия ставки
        /// </summary>
        public bool RecalcPenaltyByCurrentRefinancingRate { get; private set; }

        /// <summary>
        /// Рассчитывать пени исходя из сальдо ЛС
        /// </summary>
        public Dictionary<long, bool> CalcPenByAccId { get; private set; }

        /// <summary>
        /// Упрощенный расчет пени
        /// </summary>
        public bool SimpleCalculatePenalty { get; private set; }

        /// <summary>
        /// Упрощенный расчет пени
        /// </summary>
        public bool CalcPenaltyOneTimeMunicipalProperty { get; private set; }

        /// <summary>
        /// Не пересчитывать пени за предыдущий период
        /// </summary>
        public NumberDaysDelay NumberDaysDelay { get; private set; }

        /// <summary>
        /// Дата окончания расчётов пени по упрощённой схеме
        /// </summary>
        public DateTime? DatePenaltyCalcTo { get; private set; }

        /// <summary>
        /// Дата начала расчёта пени по упрощённой схеме
        /// </summary>
        public DateTime? DatePenaltyCalcFrom { get; private set; }

        /// <summary>
        /// Начислять пени на доп. взносы
        /// </summary>
        public virtual bool CalculatePenaltyForDecisionTarif { get; set; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public virtual DateTime? NewPenaltyCalcStart { get; set; }

        /// <summary>
        /// Допустимая просрочка
        /// </summary>
        public virtual int? NewPenaltyCalcDays { get; set; }

        /// <summary>
        /// Ставка рефинансирования (при отсутствии оплат)
        /// </summary>
        public virtual RefinancingRate RefinancingRate { get; set; }

        /// <summary>
        /// Использовать фиксированный период расчета пени
        /// </summary>
        public bool IsFixCalcPeriod { get; set; }

        /// <summary>
        /// Допустимый срок просрочки оплаты для физ лиц
        /// </summary>
        public int IndividualAllowDelayPaymentDays { get; set; }

        /// <summary>
        /// Допустимый срок просрочки оплаты для юр лиц
        /// </summary>
        public int LegalAllowDelayPaymentDays { get; set; }

        public void Clear()
        {
            lock (this._lock)
            {
                this.initialized = false;

                this.roCrFundCache.Clear();
                this.periodCache.Clear();
                this.accountChargeCache.Clear();
                this.accountSummariesCache.Clear();
                this.penaltyParamCache.Clear();
                this.transferCache.Clear();
                this.transferReturnCache.Clear();
                this.monthlyDecisions.Clear();
                this.recalcEvents.Clear();
                this.schedules.Clear();
                this.detailSchedules.Clear();
                this.ownerDelayParams.Clear();
                this.accountRecalcHistory.Clear();
                this.accountBanRecalc.Clear();
                this.entityLogCache.Dispose();
                this.perfWorkCharge.Clear();
                this.perfWorkCache.Clear();
                this.manualCalcDates.Clear();
                GC.Collect();
            }
        }

        private HashSet<ChargePeriod> periodCache = new HashSet<ChargePeriod>();
        private Dictionary<long, PersonalAccountChargeDto[]> accountChargeCache = new Dictionary<long, PersonalAccountChargeDto[]>();
        private Dictionary<long, PersonalAccountPeriodSummaryDto[]> accountSummariesCache = new Dictionary<long, PersonalAccountPeriodSummaryDto[]>();
        private HashSet<PaymentPenalties> penaltyParamCache = new HashSet<PaymentPenalties>();
        
        private Dictionary<string, TransferDto[]> transferCache = new Dictionary<string, TransferDto[]>();
        private Dictionary<string, TransferDto[]> cancelTransferCache = new Dictionary<string, TransferDto[]>();
        private Dictionary<string, TransferDto[]> transferReturnCache = new Dictionary<string, TransferDto[]>();
        private Dictionary<string, TransferDto[]> perfWorkCache = new Dictionary<string, TransferDto[]>();

        private Dictionary<long, List<PersonalAccountRecalcEvent>> recalcEvents = new Dictionary<long, List<PersonalAccountRecalcEvent>>();

        private Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> roCrFundCache =
            new Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>>();

        private Dictionary<long, MonthlyFeeAmountDecision[]> monthlyDecisions = new Dictionary<long, MonthlyFeeAmountDecision[]>();

        private IList<PenaltyParameterValue<decimal>> penaltyPercentages = new List<PenaltyParameterValue<decimal>>();
        private IList<PenaltyParameterValue<int>> penaltyDebtDays = new List<PenaltyParameterValue<int>>();

        private Dictionary<long, RestructDebtSchedule[]> schedules = new Dictionary<long, RestructDebtSchedule[]>();
        private Dictionary<long, RestructDebtScheduleDetail[]> detailSchedules = new Dictionary<long, RestructDebtScheduleDetail[]>();
        private Dictionary<long, DateTime?> manuallyRecalcDates = new Dictionary<long, DateTime?>();

        private Dictionary<long, List<Tuple<DateTime, List<OwnerPenaltyDelay>>>> ownerDelayParams = new Dictionary<long, List<Tuple<DateTime, List<OwnerPenaltyDelay>>>>();

        private Dictionary<long, RecalcHistoryDto[]> accountRecalcHistory = new Dictionary<long, RecalcHistoryDto[]>();

        private Dictionary<long, PersonalAccountBanRecalc[]> accountBanRecalc = new Dictionary<long, PersonalAccountBanRecalc[]>();
        
        private List<ClaimWorkAccountDetail> claimWorkAccountDetails = new List<ClaimWorkAccountDetail>();

        private List<FixedPeriodCalcPenalties> fixperCalcPenaltieses = new List<FixedPeriodCalcPenalties>();
        private Dictionary<long, PerformedWorkCharge[]> perfWorkCharge = new Dictionary<long, PerformedWorkCharge[]>();
        private Dictionary<long, CancelChargeDto[]> cancelChargeCache = new Dictionary<long, CancelChargeDto[]>();
        private Dictionary<long, CancelChargeDto[]> allcancelChargeCache = new Dictionary<long, CancelChargeDto[]>();
        private Dictionary<string, TransferDto[]> transferFromOtherAccountsCache = new Dictionary<string, TransferDto[]>();
        private Dictionary<long, WalletDto> walletCache = new Dictionary<long, WalletDto>(); 

        public void Dispose()
        {
            this.Clear();
        }
    }

    public class PenaltyParameter
    {
        public PenaltyParameter(PaymentPenalties parameter)
        {
            this.Parameter = parameter;

            this.AccountIds = parameter.Excludes.Select(x => x.PersonalAccount.Id).ToHashSet();
        }

        public PaymentPenalties Parameter { get; private set; }

        public HashSet<long> AccountIds { get; private set; }
    }

    public class DecisionTariffDto
    {
        public RealityObjectDecisionProtocol Protocol { get; set; }

        public PeriodMonthlyFee Decision { get; set; }
    }
}