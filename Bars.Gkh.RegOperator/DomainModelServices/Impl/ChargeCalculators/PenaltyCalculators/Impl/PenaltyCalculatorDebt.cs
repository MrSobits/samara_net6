namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Калькулятор пени с даты возникновения задолженности
    /// </summary>
    public class PenaltyCalculatorDebt : PenaltyCalculatorBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cache">Кэш расчетов</param>
        /// <param name="tracker">Версионируемые параметры</param>
        public PenaltyCalculatorDebt(ICalculationGlobalCache cache, IParameterTracker tracker)
            : base(cache, tracker)
        {
        }

        /// <summary>
        /// Получить периоды для расчета
        /// </summary>
        /// <param name="periods"></param>
        /// <returns></returns>
        protected override IEnumerable<Period> GetPeriodsInternal(IEnumerable<IPeriod> periods)
        {
            var result = new List<Period>();

            var banRecalc = this.cache.GetBanRecalc(this.account).ToList();

            var periodsDto = periods.Select(
                x => new PeriodDto
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Name = x.Name
                }).ToList();

            foreach (var periodDto in periodsDto)
            {
                // если расчитываемый период входит в период запрета перерасчет то помечаем как ignore
                var ignore =
                    banRecalc.Any(
                        x =>
                            x.DateStart <= periodDto.StartDate && x.DateEnd >= periodDto.StartDate && this.period.StartDate != periodDto.StartDate
                            && x.Type.HasFlag(BanRecalcType.Penalty));

                var charges = this.accountCharges.FirstOrDefault(x => x.ChargeDate >= periodDto.StartDate && x.ChargeDate <= periodDto.GetEndDate());

                //будем сдвигать даты здесь, а не в конструкторе, иначе параметры не те
                var startDate = periodDto.StartDate;
                periodDto.StartDate = startDate.AddMonths(-1);
                periodDto.EndDate = startDate.AddDays(-1);

                // допустимое количество дней просрочки берём из прошлого периода
                var dayParam = this.AcquireDays(periodDto);

                var period1 = periodDto;

                // берем только те периоды, для которых есть параметры
                if (dayParam.Value > 0)
                {
                    int delayDays = 0;
                    if (this.newPenaltyCalcStart < startDate && this.newPenaltyCalcDays != null)
                    {
                        delayDays = this.newPenaltyCalcDays.Value;
                    }

                    var per = this.GetPeriodForCalc(
                        period1,
                        charges.Return(x => x.Penalty),
                        ignore,
                        dayParam.Value,
                        delayDays);

                    result.Add(per);
                }
            }

            return result;
        }

        private Period GetPeriodForCalc(
            IPeriod periodDto,
            decimal realPenalty,
            bool ignore,
            int days,
            int delayDays)
        {
            var calcPeriod = new Period(periodDto);

            if (days < 30)
            {
                var deadline = periodDto.StartDate.AddDays(days);

                calcPeriod.Ignore = ignore;
                calcPeriod.Penalty = realPenalty;
                calcPeriod.Deadline = deadline < periodDto.GetEndDate() ? deadline : periodDto.GetEndDate();
                calcPeriod.DebtEnd = calcPeriod.Deadline.AddMonths(1).AddDays(-1);

                if (delayDays != 0)
                {
                    calcPeriod.DebtEnd = calcPeriod.Deadline.AddDays(delayDays - 1);
                    calcPeriod.Deadline = calcPeriod.Deadline.AddMonths(-1).AddDays(delayDays);
                }
            }
            else
            {
                var dateStart = calcPeriod.DateStart.AddMonths(1);
                var dateEnd = dateStart.WithLastDayMonth();
                var deadline = dateStart.AddDays(days - 1);

                calcPeriod.Ignore = ignore;
                calcPeriod.Penalty = realPenalty;
                calcPeriod.Deadline = deadline < dateEnd ? deadline : dateEnd;

                calcPeriod.Deadline = delayDays != 0
                    ? calcPeriod.Deadline.AddMonthByCalc(-2).AddDays(30)
                    : calcPeriod.Deadline.AddMonthByCalc(-1);

                calcPeriod.DebtEnd = calcPeriod.Deadline.AddDays(DateTime.DaysInMonth(calcPeriod.DateStart.Year, calcPeriod.DateStart.Month) - 1);
            }

            var prevParameter = days <= 0
                    ? 0
                    : this.cache.GetPeriodDays(periodDto.StartDate.AddMonths(-1), periodDto.GetEndDate().AddMonths(-1), this.account).Value;

            this.CheckDeadLine(calcPeriod, days, prevParameter);

            calcPeriod.RecalcPercent = this.AcquirePercentage(calcPeriod.Deadline, calcPeriod.DebtEnd ?? DateTime.MaxValue).Value;

            return calcPeriod;
        }

        /// <summary>
        /// Расчитать период
        /// </summary>
        /// <param name="period"></param>
        /// <param name="simplePeriods"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        protected override PenaltyPeriodProxy CalcPeriodInternal(Period period, List<Period> simplePeriods, PenaltyPeriodProxy previous = null)
        {
            var periodStartDate = period.DateStart;

            if (this.newPenaltyCalcStart != null && this.newPenaltyCalcStart <= period.DateStart)
            {
                periodStartDate = period.DateStart.AddMonths(-1); //пока отключаем эту херню
            }

            //получаем предпредыдущий период
            var prevSummary = this.summaries
                .Where(x => x.Period.StartDate < periodStartDate)
                .OrderByDescending(x => x.Period.StartDate)
                .FirstOrDefault();

            //получаем предыдущий период
            var summary = this.summaries
                .FirstOrDefault(x => x.Period.StartDate == periodStartDate);

            //получаем сумму установки изменения сальдо
            var saldoChange = summaries
                 .Where(x => x.Period.StartDate >= periodStartDate)
                 .SafeSum(x => x.BaseTariffChange);

            var prevCharge = this.accountCharges.Where(x => x.ChargeDate < periodStartDate)
                .OrderByDescending(x => x.ChargeDate)
                .FirstOrDefault();

            var cancelCharge = this.allcancelCharges.SafeSum(x => x.CancelSum);

            decimal balanceIn = 0;
            decimal charge = 0;
            decimal recalcBalanceIn = 0;
            decimal percentage = 0;

            if (summary != null && prevCharge != null)
            {               
                charge = this.calculatePenaltyForDecisionTarif ? prevCharge.ChargeTariff + prevCharge.OverPlus + summary.BaseTariffChange : prevCharge.ChargeTariff + summary.BaseTariffChange;

                balanceIn = this.calculatePenaltyForDecisionTarif ? summary.BaseTariffDebt + summary.DecisionTariffDebt - cancelCharge: summary.BaseTariffDebt - cancelCharge;

                if (period != null && this.period != null && period.Name == this.period.Name && summary.BaseTariffChange < 0)
                {
                    balanceIn += summary.BaseTariffChange;
                }
            }

          

            decimal paymentRestructSum = 0;

            if (this.cache.IsClaimWorkEnabled)
            {
                var restructDetails = this.cache.GetRestructureScheduleDetails(this.account);

                var paymentIds = this.payments
                    .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                    .Where(x => x.PaymentDate <= this.GetExpiredDate())
                    .Select(x => x.Id);

                paymentRestructSum = restructDetails
                    .Where(x => paymentIds.Contains(x.TransferId))
                    .SafeSum(x => x.Sum);
            }

            var payBt = this.paymentsBaseTariff
                    .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline);
            var cancelBt = this.cancelBaseTariffPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline);

            var balIn = balanceIn;

            //все оплаты, которые были в пердыдущем периоде
            var paymentSum = this.calculatePenaltyForDecisionTarif
                ? this.payments
                    .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline)
                    .SafeSum(x => x.Amount)
                    - this.cancelPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline).SafeSum(x => x.Amount)
                : this.paymentsBaseTariff
                    .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline)
                    .SafeSum(x => x.Amount)
                    - this.cancelBaseTariffPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline)
                        .SafeSum(x => x.Amount);

            //вычитаем оплаты
            balanceIn -= Math.Max(paymentSum - paymentRestructSum, 0);

            var startDate = prevSummary != null ? prevSummary.Period.StartDate : period.DateStart.AddMonths(-1);
            var endDate = prevSummary != null ? prevSummary.Period.GetEndDate() : period.DateStart.AddDays(-1);

            //Если предыдущий период был простой, то нужно пересчитать задолженность до расчитываемого периода
            if (simplePeriods != null && previous.ReturnSafe(x => x.Simple) && previous != null)
            {
                previous.RecalcBalance = 0;
                balanceIn += charge;

                foreach (var per in simplePeriods)
                {
                    previous.RecalcBalance += this.RecalcCharge(per.DateStart, per.DateEnd).RegopRoundDecimal(2);
                }

                previous.RecalcBalance -= this.paymentsBaseTariff.Where(x => x.PaymentDate.Date <= this.GetDeadline()).SafeSum(x => x.Amount);
            }

            if (previous == null)
            {
                var recalcs = recalcHistory.WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                     .Where(x => x.RecalcPeriod.StartDate <= startDate)
                    .Where(x => x.CalcPeriod.StartDate >= startDate);

                var recalcHistorySum = this.recalcHistory
                    .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                    .WhereIf(
                        this.calculatePenaltyForDecisionTarif,
                        x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                    .Where(x => x.RecalcPeriod.StartDate <= startDate)
                    .Where(x => x.CalcPeriod.StartDate >= period.DateStart)
                    .SafeSum(x => x.RecalcSum);

                var currentRecalc = this.chargeRecalcHistory
                    .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate <= periodStartDate)
                    .SafeSum(x => x.RecalcSum);

                recalcBalanceIn = balanceIn + Math.Min(recalcHistorySum + currentRecalc, 0);

                if (summary == null)
                {
                    recalcBalanceIn -= this.calculatePenaltyForDecisionTarif
                        ? this.payments
                            .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                            .SafeSum(x => x.Amount)
                            - this.cancelPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline).SafeSum(x => x.Amount)
                        : this.paymentsBaseTariff
                            .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                            .SafeSum(x => x.Amount)
                            - this.cancelBaseTariffPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline)
                                .SafeSum(x => x.Amount);
                }

                percentage = period.RecalcPercent;
            }
            else
            {
                var banRecalc = this.cache.GetBanRecalc(this.account).Any(x => x.DateStart <= startDate && x.DateEnd >= startDate && x.Type.HasFlag(BanRecalcType.Penalty));

                charge = banRecalc
                    ? charge
                    : this.RecalcCharge(startDate, endDate);

                var recalcHistorySum = this.recalcHistory
                    .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                    .WhereIf(
                        this.calculatePenaltyForDecisionTarif,
                        x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                    .Where(x => x.RecalcPeriod.StartDate <= startDate.AddMonths(-1))
                    .Where(x => x.CalcPeriod.StartDate >= startDate.AddMonths(-1))
                    .SafeSum(x => x.RecalcSum);

                // Если считаем первый период с новой настройкой, то не прибавляем начисление ко второму периоду задолженности
                if ((this.newPenaltyCalcStart != null && this.newPenaltyCalcStart == period.DateStart) || prevSummary == null)
                {
                    recalcBalanceIn = previous.RecalcBalance;
                }
                else
                {
                    previous.RecalcBalance += charge;

                    recalcBalanceIn = banRecalc || recalcHistorySum < 0 ? previous.RecalcBalance : Math.Min(balanceIn, previous.RecalcBalance);

                    var recalcSum = this.recalcHistory
                        .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                        .WhereIf(
                            this.calculatePenaltyForDecisionTarif,
                            x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                        .Where(x => x.CalcPeriod.StartDate == startDate)
                        .SafeSum(x => x.RecalcSum);

                    recalcBalanceIn += recalcSum != 0 ? recalcSum : 0;
                }

                percentage = period.RecalcPercent;
            }

            if (prevSummary != null)
            {
                if (charge != 0)//странная тема, до открытия ЛС чардж обнуляется, а тут еще и отнимается рекалк, и задолженность становится отрицательной. ПМСМ такого быть не должно
                {
                    var prevRecalcCharge = this.chargeRecalcHistoryToUse.SafeSum(x => x.RecalcSum);
                    this.chargeRecalcHistoryToUse.Clear();

                    recalcBalanceIn += prevRecalcCharge < 0 ? prevRecalcCharge : 0M;
                }
                else
                {
                    var prevRecalcCharge = this.chargeRecalcHistoryToUse.SafeSum(x => x.RecalcSum) + prevSummary.ChargedByBaseTariff;
                    this.chargeRecalcHistoryToUse.Clear();

                    recalcBalanceIn += prevRecalcCharge < 0 ? prevRecalcCharge : 0M;
                }
            }
            else
            {
                this.chargeRecalcHistory
                    .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate == startDate)
                    .AddTo(this.chargeRecalcHistoryToUse);
            }

            // считаем сумму всех возвратов по базовому тарифу
            var returnPaymentsByBase = this.returnsPaymentBaseTariff
                .Where(x => x.PaymentDate > periodStartDate && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            // считаем сумму всех возвратов по тарифу решения
            var returnPaymentsByDecision = this.returnsPaymentDecisionTariff
                .Where(x => x.PaymentDate > periodStartDate && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            var transfersMerge = this.payments
                .Where(
                    x =>
                        x.Operation.Reason == "Перенос долга при слиянии" && x.PaymentDate <= periodStartDate.AddMonths(1).AddDays(-1)
                            && x.PaymentDate >= periodStartDate)
                .SafeSum(x => x.Amount);

            // зачет средств за проделанные работы
            var perfWorkCharge = this.performedWorkCharge
                .Where(x => x.ChargePeriod.StartDate == periodStartDate.AddMonths(-1))
                .SafeSum(x => x.Sum);

            //увеличиваем задолженность на сумму возвратов и уменьшаем на сумму зачетов
            if (this.newPenaltyCalcStart == null || this.newPenaltyCalcStart != period.DateStart)
            {
                recalcBalanceIn += this.calculatePenaltyForDecisionTarif
                    ? returnPaymentsByBase + returnPaymentsByDecision + transfersMerge - perfWorkCharge
                    : returnPaymentsByBase + transfersMerge - perfWorkCharge;
            }

            var operations = this.GetDebtPartitionOperations(period);

            var periodBalance = new PenaltyPeriodProxy(period.Deadline, recalcBalanceIn, period.DebtEnd, percentage);

            // важен порядок и группировка по дате без времени операции
            operations.OrderBy(x => x.Date).GroupBy(x => x.Date.Date).Select(
                x =>
                {
                    var partitions = x.GroupBy(y => y.PeriodPartitionType)
                        .ToDictionary(y => y.Key, this.CreatePartitionPartition)
                        .Values.ToArray();

                    return new OperationProxyResult
                    {
                        Date = x.Key,
                        NeedSavePayment = x.Last().NeedSavePayment,
                        SkipOperation = x.All(z => z.SkipOperation),
                        Percentage = x.Select(z => z.Percentage).FirstOrDefault(z => z > 0),
                        Partitions = partitions
                    };
                }).OrderBy(x => x.Date).ForEach(x => periodBalance.ApplyDebtSum(x));

            if (periodBalance.Penalties.Count == 0)
            {
                var newPenalty = new PenaltyPeriod
                {
                    Start = period.Deadline,
                    RecalcBalance = periodBalance.RecalcBalance,
                    End = periodBalance.DebtEnd,
                    Percentage = percentage
                };

                periodBalance.Penalties.Add(newPenalty);
            }

            if (this.cache.GetRecalcEvents(this.account)
                .Where(x => x.RecalcEventType == RecalcEventType.ChangeCloseDate)
                .Any(x => x.EventDate < periodBalance.periodStart))
            {
                periodBalance.Penalties = new List<PenaltyPeriod>();
            }

            return periodBalance;
        }

        private List<OperationProxy> GetDebtPartitionOperations(Period period)
        {
            var operations = new List<OperationProxy>();

            var cancelPayments = this.calculatePenaltyForDecisionTarif ? this.cancelPayments : this.cancelBaseTariffPayments;

            var detailSchedule = this.cache.GetRestructureScheduleDetails(this.account);

            foreach (var cancelPayment in cancelPayments.Where(x => period.SimpleContains(x.PaymentDate)))
            {
                var operation = new OperationProxy
                {
                    Date = cancelPayment.PaymentDate,
                    Amount = cancelPayment.Amount,
                    PeriodPartitionType = PeriodPartitionType.Refund
                };

                operations.Add(operation);
            }

            var transfers = this.calculatePenaltyForDecisionTarif ? this.payments : this.paymentsBaseTariff;

            var lastPaymentDate = this.cache.RefinancingRate == RefinancingRate.AsConfigured ? period.DateStart : this.period.GetEndDate();

            foreach (var payment in
                transfers.Where(x => period.SimpleContains(x.PaymentDate) && x.Operation.Reason != "Перенос долга при слиянии")
                    .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection)
                    .OrderBy(x => x.PaymentDate))
            {
                var type = PeriodPartitionType.Payment;

                decimal restructSum = 0;

                if (!this.CheckIsOverdueRestruct(payment.PaymentDate))
                {
                    restructSum = detailSchedule
                        .Where(x => x.TransferId == payment.Id)
                        .SafeSum(x => x.Sum);
                }

                lastPaymentDate = this.GetNextPaymentDate(payment.PaymentDate);

                if (!lastPaymentDate.IsValid())
                {
                    lastPaymentDate = this.cache.RefinancingRate == RefinancingRate.CurrentPeriod ? this.period.GetEndDate() : payment.PaymentDate;
                }

                var operation = new OperationProxy
                {
                    Date = payment.PaymentDate,
                    Amount = -payment.Amount,
                    RestructAmount = restructSum,
                    PeriodPartitionType = type,
                    NeedSavePayment = true,
                    Percentage = this.cache.GetPeriodPenalty(lastPaymentDate, this.account)?.Percentage ?? 0
                };

                operations.Add(operation);
            }

            foreach (var payment in
                transfers.Where(x => period.SimpleContains(x.PaymentDate))
                    .Where(x => x.PaymentSource == TypeTransferSource.PaymentCorrection)
                    .OrderBy(x => x.PaymentDate))
            {
                var type = PeriodPartitionType.PaymentCorrection;

                var operation = new OperationProxy
                {
                    Date = payment.PaymentDate,
                    Amount = -payment.Amount,
                    PeriodPartitionType = type,
                    NeedSavePayment = true
                };

                operations.Add(operation);
            }

            if (this.cache.RefinancingRate == RefinancingRate.AsConfigured && !this.GetNextPaymentDate(lastPaymentDate).IsValid())
            {
                var result = this.cache.GetPeriodPenalties(period.Deadline, period.DebtEnd ?? period.Deadline.AddMonths(1), this.account)
                    .Where(x => x.Key > lastPaymentDate)
                    .ToList();

                result.ForEach(
                    x =>
                    {
                        var operation = new OperationProxy
                        {
                            Date = x.Key.AddDays(-1),
                            Percentage = x.Value,
                            PeriodPartitionType = PeriodPartitionType.PercentageChange,
                        };

                        operations.Add(operation);
                    });
            }

            if (period.OldDeadLine.HasValue && period.OldDeadLine > period.Deadline)
            {
                var operation = new OperationProxy
                {
                    Date = period.OldDeadLine.Value.AddDays(-1),
                    PeriodPartitionType = PeriodPartitionType.DebtDaysChange,
                    PeriodDaysChange = period.PeriodDaysChange
                };

                operations.Add(operation);
            }

            foreach (var perfWork in this.perfWorks.Where(x => period.SimpleContains(x.PaymentDate)))
            {
                var operation = new OperationProxy
                {
                    Date = perfWork.PaymentDate,
                    Amount = -perfWork.Amount,
                    PeriodPartitionType = PeriodPartitionType.PerfWork
                };

                operations.Add(operation);
            }

            var eventCloseDate = this.cache.GetRecalcEvents(this.account).Where(x => x.RecalcEventType == RecalcEventType.ChangeCloseDate).OrderBy(x => x.EventDate).FirstOrDefault();

            if (eventCloseDate != null && period.SimpleContains(eventCloseDate.EventDate))
            {
                var operation = new OperationProxy
                {
                    Date = eventCloseDate.EventDate,
                    PeriodPartitionType = PeriodPartitionType.CloseAccount
                };

                operations.Add(operation);
            }

            if (this.cache.IsClaimWorkEnabled)
            {
                var restructDate = this.cache.GetRestructDate(this.account);

                if (restructDate != null && period.SimpleContains(restructDate.Value))
                {
                    var debtSum = this.GetDebtRestructSum();

                    var operation = new OperationProxy
                    {
                        Date = restructDate.Value,
                        Amount = -debtSum,
                        PeriodPartitionType = PeriodPartitionType.Restruct
                    };

                    operations.Add(operation);
                }

                var expiredDate = this.GetExpiredDate();

                if (expiredDate != null && period.SimpleContains(expiredDate.Value))
                {
                    var debtSum = this.GetDebtRestructSum();

                    var operation = new OperationProxy
                    {
                        Date = expiredDate.Value.AddDays(1),
                        Amount = debtSum,
                        PeriodPartitionType = PeriodPartitionType.LatePymentRestruct
                    };

                    operations.Add(operation);
                }
            }

            return operations;
        }
    }
}