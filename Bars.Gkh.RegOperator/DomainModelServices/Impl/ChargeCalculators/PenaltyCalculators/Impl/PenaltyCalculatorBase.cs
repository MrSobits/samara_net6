namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Базовый калькулятор пени
    /// </summary>
    public abstract partial class PenaltyCalculatorBase : IPenaltyCalculator
    {
        private const int TraktorsDriverJoy = 300;

        protected readonly ICalculationGlobalCache cache;
        protected readonly IParameterTracker tracker;

        private readonly bool simpleCalc;
        private readonly NumberDaysDelay numberDaysDelay;
        private readonly DateTime? datePenaltyCalcTo;
        private readonly DateTime? datePenaltyCalcFrom;
        protected readonly bool calculatePenaltyForDecisionTarif;
        protected readonly DateTime? newPenaltyCalcStart;
        protected readonly int? newPenaltyCalcDays;

        protected bool IsNewPenaltyCalcOfPeriod(IPeriod period) => this.newPenaltyCalcStart <= period.StartDate && this.newPenaltyCalcDays != null;

        protected IPeriod period;
        protected BasePersonalAccount account;
        protected List<RecalcHistory> chargeRecalcHistory;

        private decimal restructSum;
        protected List<PersonalAccountPeriodSummaryDto> summaries;
        protected List<RecalcHistoryDto> recalcHistory;
        protected List<TransferDto> payments;
        protected List<TransferDto> cancelPayments;
        protected List<TransferDto> cancelBaseTariffPayments;
        protected List<TransferDto> paymentsBaseTariff;
        protected List<TransferDto> returnsPaymentBaseTariff;
        protected List<TransferDto> returnsPaymentDecisionTariff;
        protected List<TransferDto> perfWorks;
        protected List<PersonalAccountChargeDto> accountCharges;
        protected List<CancelChargeDto> cancelCharges;
        protected List<CancelChargeDto> allcancelCharges;
        protected List<PerformedWorkCharge> performedWorkCharge;
        protected HashSet<RecalcHistory> chargeRecalcHistoryToUse;
        private List<CalculationParameterTrace> traces;
        private List<RecalcHistory> newRecalcHistory;

        protected RecalcReasonProxy RecalculationReason;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cache">Кэш расчетов</param>
        /// <param name="tracker">Версионируемые параметры</param>
        protected PenaltyCalculatorBase(
            ICalculationGlobalCache cache,
            IParameterTracker tracker)
        {
            this.cache = cache;
            this.tracker = tracker;

            this.simpleCalc = this.cache.SimpleCalculatePenalty;
            this.numberDaysDelay = this.cache.NumberDaysDelay;
            this.datePenaltyCalcTo = this.cache.DatePenaltyCalcTo;
            this.datePenaltyCalcFrom = this.cache.DatePenaltyCalcFrom;
            this.calculatePenaltyForDecisionTarif = this.cache.CalculatePenaltyForDecisionTarif;
            this.newPenaltyCalcStart = this.cache.NewPenaltyCalcStart;
            this.newPenaltyCalcDays = this.cache.NewPenaltyCalcDays;
            this.calculatePenaltyForDecisionTarif = this.cache.CalculatePenaltyForDecisionTarif;
        }

        /// <summary>
        /// Расчитать
        /// </summary>
        /// <returns></returns>
        public CalculationResult<PenaltyResult> Calculate()
        {
            var needToRecalcPeriods = this.GetPeriodsForRecalc(this.period);
            var currentPenalty = 0m;
            var recalc = 0m;
            var deadLine = this.GetDeadline();

            var paymentsSum =
                this.paymentsBaseTariff.Where(x => x.PaymentDate.Date <= deadLine).SafeSum(x => x.Amount) -
                    this.returnsPaymentBaseTariff.Where(x => x.PaymentDate.Date <= deadLine).SafeSum(x => x.Amount);

            var simplePeriods = needToRecalcPeriods.Where(x => x.Simple).OrderBy(x => x.DateStart).ToList();

            PenaltyPeriodProxy prevBalance = null;

            foreach (var recalcPeriod in needToRecalcPeriods.OrderBy(x => x.DateStart))
             {
                prevBalance = recalcPeriod.Simple
                    ? this.SimpleCalcPeriod(recalcPeriod, ref paymentsSum)
                    : this.CalcPeriodInternal(recalcPeriod, simplePeriods, prevBalance);

                if (prevBalance == null || recalcPeriod.Ignore)
                {
                    continue;
                }

                if (recalcPeriod.Id == this.period.Id)
                {
                    currentPenalty = this.ProcessCurrentPeriod(prevBalance, recalcPeriod);
                }
                else
                {
                    var recalcPenalty = this.ProcessClosedPeriod(prevBalance, recalcPeriod);

                    recalc += recalcPenalty;

                    var newRecalcHistory = new RecalcHistory
                    {
                        PersonalAccount = this.account,
                        CalcPeriod = (ChargePeriod)this.period,
                        RecalcPeriod = this.cache.GetClosedPeriods().FirstOrDefault(x => x.Id == recalcPeriod.Id),
                        RecalcSum = recalcPenalty,
                        RecalcType = RecalcType.Penalty
                    };

                    this.newRecalcHistory.Add(newRecalcHistory);
                }
            }

            var result = new CalculationResult<PenaltyResult>(new PenaltyResult(currentPenalty, recalc));

            result.Traces.AddRange(this.traces);
            result.RecalcHistory.AddRange(this.newRecalcHistory);

            return result;
        }

        /// <summary>
        /// Получить дату начала расчета месяца
        /// </summary>
        /// <returns></returns>
        protected DateTime GetDeadline()
        {
            var parameter = this.GetPenaltyParameter(this.period.StartDate, this.period.GetEndDate());

            return this.datePenaltyCalcTo != null
                ? this.datePenaltyCalcTo.Value.AddDays(parameter.Return(x => x.Days))
                : this.period.StartDate.AddDays(parameter.Return(x => x.Days));
        }

        /// <summary>
        /// Получить кэш для ЛС
        /// </summary>
        /// <param name="account"></param>
        /// <param name="period"></param>
        /// <param name="recalcHistory"></param>
        /// <returns></returns>
        public IPenaltyCalculator Init(BasePersonalAccount account, IPeriod period, List<RecalcHistory> recalcHistory)
        {
            this.account = account;
            this.period = period;
            this.chargeRecalcHistory = recalcHistory;

            this.InitAccountParams();

            return this;
        }

        private void InitAccountParams()
        {
            this.summaries = this.cache.GetAllSummaries(this.account).ToList();
            this.recalcHistory = this.cache.GetRecalcHistory(this.account).ToList();
            this.payments = this.cache.GetPaymentTransfers(this.account).ToList();
            this.cancelPayments = this.cache.GetCancelPaymentTransfers(this.account).ToList();
            this.cancelBaseTariffPayments = this.cache.GetBaseTariffCancelPaymentTransfers(this.account).ToList();

            this.paymentsBaseTariff = this.cache.GetPaymentTransfersBaseTariff(this.account).ToList();
            this.returnsPaymentBaseTariff = this.cache.GetReturnTransfersBaseTariff(this.account).ToList();
            this.returnsPaymentDecisionTariff = this.cache.GetReturnTransfersDecisionTariff(this.account).ToList();
            this.perfWorks = this.cache.GetPerfWorks(this.account).ToList();
            this.accountCharges = this.cache.GetAllCharges(this.account).ToList();
            this.cancelCharges = this.cache.GetCancelChargesInCurrentPeriod(this.account.Id, CancelType.BaseTariffCharge).ToList();
            this.allcancelCharges = this.cache.GetAllCancelChargesInCurrentPeriod(this.account.Id, CancelType.BaseTariffCharge).ToList();
            this.performedWorkCharge = this.cache.GetPerfWorkCharge(this.account).ToList();

            this.chargeRecalcHistoryToUse = new HashSet<RecalcHistory>();
            this.traces = new List<CalculationParameterTrace>();

            this.newRecalcHistory = new List<RecalcHistory>();
        }

        private List<Period> GetPeriodsForRecalc(IPeriod current)
        {
            var closedPeriods = this.cache.GetClosedPeriods().Cast<IPeriod>().ToList();

            var allChanges = this.tracker.GetChanges(this.account, this.period).ToList();

            if (allChanges.Any())
            {
                this.RecalculationReason = allChanges.OrderBy(x => x.DateActualChange).Select(
                    x => new RecalcReasonProxy
                    {
                        Reason = RecalcReason.RecalcCharge,
                        Date = x.DateActualChange
                    }).FirstOrDefault();
            }

            var penaltyparams = this.cache.GetPeriodPenalty(this.period.StartDate, this.account);

            var dateStart = allChanges.SafeMin(x => x.DateActualChange, DateTime.MaxValue);

            // минимальная дата по ручному перерасчету
            var manuallyRecalcDate = this.cache.GetManuallyRecalcDate(this.account);

            if (manuallyRecalcDate.HasValue && manuallyRecalcDate.Value < dateStart)
            {
                dateStart = manuallyRecalcDate.Value;
                if (this.RecalculationReason == null || this.RecalculationReason.Date < dateStart)
                {
                    this.RecalculationReason = new RecalcReasonProxy
                    {
                        Reason = RecalcReason.RecalcCharge,
                        Date = dateStart
                    };
                }
            }

            if (this.simpleCalc && this.datePenaltyCalcFrom.HasValue)
            {
                dateStart = this.datePenaltyCalcFrom.Value;
            }

            if (this.numberDaysDelay == NumberDaysDelay.StartDateDebt)
            {
                var prevPeriod = closedPeriods.OrderByDescending(x => x.EndDate).FirstOrDefault();

                if (prevPeriod.IsNotNull())
                {
                    // получаем даты оплат для перерасчета 
                    var tempPayments = this.calculatePenaltyForDecisionTarif ? this.payments : this.paymentsBaseTariff;

                    var firstPayment =
                        tempPayments.Where(x => x.OperationDate >= prevPeriod.StartDate && x.OperationDate <= prevPeriod.EndDate)
                            .OrderBy(x => x.PaymentDate)
                            .FirstOrDefault();

                    if (firstPayment.IsNotNull())
                    {
                        var allowDays = this.GetPenaltyParameter(prevPeriod.StartDate, prevPeriod.GetEndDate())?.Days ?? 0;

                        var paymentPeriod = closedPeriods
                            .Where(x => x.StartDate.AddDays(allowDays) <= firstPayment.PaymentDate)
                            .FirstOrDefault(x => x.EndDate.Value.AddDays(allowDays) >= firstPayment.PaymentDate);

                        if (paymentPeriod != null && dateStart > paymentPeriod.StartDate.AddMonths(1))
                        {
                            dateStart = paymentPeriod.StartDate.AddMonths(1);
                        }

                        if (this.RecalculationReason == null || dateStart < this.RecalculationReason.Date)
                        {
                            this.RecalculationReason = new RecalcReasonProxy
                            {
                                Reason = RecalcReason.Payment,
                                Date = firstPayment.PaymentDate
                            };
                        }
                    }
                }
            }

            if (this.datePenaltyCalcFrom.HasValue && this.datePenaltyCalcTo.HasValue)
            {
                var dateMin = this.datePenaltyCalcFrom.Value;
                var dateMax = this.datePenaltyCalcTo.Value;

                if (dateStart >= dateMin && dateStart <= dateMax)
                {
                    dateStart = dateMin;
                }
            }

            var needToRecalcPeriods = closedPeriods
                .WhereIf(this.simpleCalc, x => dateStart.Date <= x.StartDate.Date && x.EndDate.Value.Date <= this.datePenaltyCalcTo.Value.Date)
                .WhereIf(!this.simpleCalc, x => dateStart.Date <= x.EndDate.Value.Date && x.EndDate.Value.Date <= this.period.StartDate.Date)
                .WhereIf(this.cache.RecalcPenaltyByCurrentRefinancingRate && penaltyparams != null, x => x.StartDate >= penaltyparams.DateStart)
                .ToList();

            var events = this.cache.GetRecalcEvents(this.account);
            if (events.IsNotEmpty() && !this.simpleCalc)
            {
                var @event = events.ToLookup(x => x.RecalcType)[PersonalAccountRecalcEvent.PenaltyType].OrderBy(x => x.EventDate).FirstOrDefault();
                if (@event != null)
                {
                    var dateFrom = this.cache.RecalcPenaltyByCurrentRefinancingRate && penaltyparams != null ? penaltyparams.DateStart : @event.EventDate;

                    var periodsForEvent = closedPeriods
                            .Where(x => x.StartDate >= dateFrom);

                    var except = periodsForEvent.Except(
                        needToRecalcPeriods,
                        FnEqualityComparer<IPeriod>.Member(x => x.Id)).ToList();

                    if (this.RecalculationReason == null || dateFrom <= this.RecalculationReason.Date)
                    {
                        this.RecalculationReason = new RecalcReasonProxy
                        {
                            Reason = this.GetRecaclReason(@event.RecalcEventType),
                            Date = dateFrom
                        };
                    }

                    needToRecalcPeriods.AddRange(except);
                }
            }

            // если используется упрощенный перерасчет 
            // то количество дней задолженности нужно считать от рассчитываемого периода до текущего
            if (this.simpleCalc)
            {
                return this.SimpleGetPeriods(needToRecalcPeriods).OrderBy(x => x.DateStart).ToList();
            }

            needToRecalcPeriods.Add(current);

            // Периоды для упрощенного расчета пени
            var simplePeriods = new List<Period>();

            if (this.simpleCalc)
            {
                if (this.datePenaltyCalcTo != null)
                {
                    simplePeriods =
                    this.SimpleGetPeriods(
                        needToRecalcPeriods.Where(x => x.StartDate < this.datePenaltyCalcTo.ToDateTime().AddMonths(-1)))
                        .OrderBy(x => x.DateStart)
                        .ToList();
                }
            }

            // Периоды для обычно расчета пени
            var periods = this.GetPeriodsInternal(needToRecalcPeriods.WhereIf(this.datePenaltyCalcTo != null, x => x.StartDate > this.datePenaltyCalcTo).ToList())
                    .OrderBy(x => x.DateStart)
                    .ToList();

            periods.AddRange(simplePeriods);

            return periods;
        }

        /// <summary>
        /// Получить периоды для расчета
        /// </summary>
        /// <param name="periods"></param>
        /// <returns></returns>
        protected abstract IEnumerable<Period> GetPeriodsInternal(IEnumerable<IPeriod> periods);

        /// <summary>
        /// Расчитать период
        /// </summary>
        /// <param name="period"></param>
        /// <param name="simplePeriods"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        protected abstract PenaltyPeriodProxy CalcPeriodInternal(Period period, List<Period> simplePeriods, PenaltyPeriodProxy previous = null);

        // расчет периода
        // берем текущее начисление + перерасчет и вычитаем из нее оствшуюся оплату
        private PenaltyPeriodProxy SimpleCalcPeriod(Period period, ref decimal paymentBalance)
        {
            var charge = this.RecalcCharge(period.DateStart, period.DateEnd);

            PenaltyPeriodProxy periodBalance;

            if ((charge > paymentBalance && charge != 0) || charge < 0)
            {
                var balanceIn = charge - paymentBalance;
                paymentBalance = 0;
                periodBalance = new PenaltyPeriodProxy(period.DateStart, balanceIn, period.RecalcPercent, null, null);
            }
            else
            {
                paymentBalance -= charge;
                periodBalance = new PenaltyPeriodProxy(period.DateStart, 0, period.RecalcPercent, null, null);
            }

            periodBalance.SimpleApplySum(period.Deadline);

            periodBalance.Simple = true;

            return periodBalance;
        }

        private decimal ProcessCurrentPeriod(PenaltyPeriodProxy prevBalance, Period recalcPeriod)
        {
            var penalty = 0m;

            foreach (var penaltyPeriod in prevBalance.Penalties)
            {
                var end = penaltyPeriod.End ?? recalcPeriod.DateEnd;

                var days = (end - penaltyPeriod.Start.Date).Days + 1;

                if (days > 0)
                {
                    //задолженность должна быть неотрицательной
                    var recalcDebt = Math.Max(penaltyPeriod.RecalcBalance, 0m);

                    penalty += recalcDebt
                        * penaltyPeriod.Percentage.ToDivisional()
                        * days / PenaltyCalculatorBase.TraktorsDriverJoy;

                    var paramTrace = new CalculationParameterTrace
                    {
                        DateStart = penaltyPeriod.Start.Date,
                        DateEnd = end,
                        CalculationType = CalculationTraceType.Penalty,
                        ParameterValues = new Dictionary<string, object>
                    {
                        { "payment_penalty_percentage", penaltyPeriod.Percentage },
                        { "penalty_debt", recalcDebt },
                        { "numberdays_delay", this.numberDaysDelay },
                        { "penalty_decision", this.calculatePenaltyForDecisionTarif },
                        { "payment", penaltyPeriod.Payment },
                        { "payment_date", penaltyPeriod.End },
                        { "is_fix_calc_period", this.cache.IsFixCalcPeriod },
                        { "start_fix", recalcPeriod.StartFix },
                        { "end_fix", recalcPeriod.EndFix },
                        { "partition_details", penaltyPeriod.Partitions }
                    },
                        ChargePeriod = (ChargePeriod)this.period
                    };

                    this.traces.Add(paramTrace);
                }
            }

            return penalty;
        }

        private decimal ProcessClosedPeriod(PenaltyPeriodProxy prevBalance, Period recalcPeriod)
        {
            var currentPeriodRecalc = 0m;

            foreach (var penaltyPeriod in prevBalance.Penalties)
            {
                //если упрощенный расчет то задолженность может быть отрицательной
                var debt = recalcPeriod.Simple ? penaltyPeriod.RecalcBalance : Math.Max(penaltyPeriod.RecalcBalance, 0m);
                var end = penaltyPeriod.End ?? recalcPeriod.DateEnd;
                var days = (end - penaltyPeriod.Start.Date).Days + 1;

                if (days > 0)
                {
                    var currentRecalc = (debt * penaltyPeriod.Percentage.ToDivisional() * days / TraktorsDriverJoy).RegopRoundDecimal(2);

                    currentPeriodRecalc += currentRecalc;

                    this.traces.Add(
                        new CalculationParameterTrace
                        {
                            DateStart = penaltyPeriod.Start.Date,
                            DateEnd = penaltyPeriod.End,
                            CalculationType = CalculationTraceType.PenaltyRecalcDetail,
                            ParameterValues = new Dictionary<string, object>
                            {
                                {"payment", penaltyPeriod.Payment},
                                {"payment_date", penaltyPeriod.End},
                                {"partition_details", penaltyPeriod.Partitions},
                                {"debt", debt},
                                {"recalc_penalty", currentRecalc},
                                {"recalc_percent", penaltyPeriod.Percentage},
                                {"is_fix_calc_period", this.cache.IsFixCalcPeriod},
                                {"start_fix", recalcPeriod.StartFix},
                                {"end_fix", recalcPeriod.EndFix}
                        },
                            ChargePeriod = (ChargePeriod)this.period
                        });
                }
            }

            //ищем перерасчеты в других периодах и прибавляем
            var recalcInNextPeriods = this.recalcHistory
                .Where(x => x.RecalcPeriod.Id == recalcPeriod.Id)
                .Where(x => x.RecalcType == RecalcType.Penalty)
                .SafeSum(x => x.RecalcSum);

            var factPenaltyCharge = recalcPeriod.Penalty + recalcInNextPeriods;

            this.traces.Add(
                new CalculationParameterTrace
                {
                    DateStart = prevBalance.periodStart,
                    DateEnd = prevBalance.DebtEnd ?? prevBalance.periodStart.AddMonths(1).AddDays(-1),
                    CalculationType = recalcPeriod.Simple ? CalculationTraceType.DelayPenaltyRecalc : CalculationTraceType.PenaltyRecalc,
                    ParameterValues = new Dictionary<string, object>
                    {
                        {"recalc_percent", prevBalance.Percentage},
                        {"recalc_penalty", currentPeriodRecalc.RegopRoundDecimal(2) },
                        {"penalty", factPenaltyCharge },
                        {"recalc_reason", this.RecalculationReason?.Reason},
                        {"recalc_reason_date", this.RecalculationReason?.Date},
                        {"numberdays_delay", this.numberDaysDelay},
                        {"is_fix_calc_period", this.cache.IsFixCalcPeriod},
                        {"start_fix", recalcPeriod.StartFix },
                        {"end_fix", recalcPeriod.EndFix },
                        {"penalty_decision", this.calculatePenaltyForDecisionTarif},
                        {"calc_period_id", recalcPeriod.Id}
                    },
                    ChargePeriod = (ChargePeriod)this.period
                });

            return currentPeriodRecalc.RegopRoundDecimal(2) - factPenaltyCharge;
        }

        /// <summary>
        /// Проверить просрочку на дату
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected bool CheckIsOverdueRestruct(DateTime date)
        {
            var days = this.account.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual
                ? this.cache.IndividualAllowDelayPaymentDays
                : this.cache.LegalAllowDelayPaymentDays;

            return this.cache
                .GetRestructureSchedule(this.account)
                .Where(x => x.PlanedPaymentDate < date)
                .Any(x => x.PaymentDate > x.PlanedPaymentDate.AddDays(days));
        }

        /// <summary>
        /// Получить дату просрочки
        /// </summary>
        /// <returns></returns>
        protected DateTime? GetExpiredDate()
        {
            var days = this.account.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual
                ? this.cache.IndividualAllowDelayPaymentDays
                : this.cache.LegalAllowDelayPaymentDays;

            return this.cache
                .GetRestructureSchedule(this.account)
                .Where(x => x.IsExpired)
                .OrderBy(x => x.PlanedPaymentDate)
                .Select(x => x.PlanedPaymentDate.AddDays(days))
                .FirstOrDefault();
        }

        protected decimal GetDebtRestructSum()
        {
            var schedule = this.cache.GetRestructureSchedule(this.account);
            return schedule.FirstOrDefault()?.RestructDebt.DebtSum ?? 0;
        }

        private RecalcReason GetRecaclReason(RecalcEventType eventType)
        {
            switch (eventType)
            {
                case RecalcEventType.Payment:
                    return RecalcReason.Payment;

                case RecalcEventType.CancelPayment:
                    return RecalcReason.CancelPayment;

                case RecalcEventType.ChangePenaltyParam:
                    return RecalcReason.ChangePenaltyParametrs;

                case RecalcEventType.ChangeCloseDate:
                    return RecalcReason.ChangeCloseDate;

                default:
                    return RecalcReason.RecalcCharge;
            }
        }
    }
}