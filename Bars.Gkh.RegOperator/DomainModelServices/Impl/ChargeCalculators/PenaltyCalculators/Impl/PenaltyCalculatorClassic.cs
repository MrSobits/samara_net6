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
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Калькулятор пени для классического алгоритма
    /// </summary>
    public class PenaltyCalculatorClassic : PenaltyCalculatorBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cache">Кэш расчетов</param>
        /// <param name="tracker">Версионируемые параметры</param>
        public PenaltyCalculatorClassic(ICalculationGlobalCache cache, IParameterTracker tracker)
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

            // независимо от того, какой период пересчитывается, используется ставка рефинансирования текущего периода
            // а количество дней просрочки рассчитываемого
            int days;
            decimal currentPercentage;
            this.AcquirePercentageAndDays(this.period, out currentPercentage, out days);

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
                var ignore =
                    banRecalc.Any(
                        x =>
                            x.DateStart.Date <= periodDto.StartDate.Date && x.DateEnd.Date >= periodDto.StartDate.Date
                                && this.period.StartDate != periodDto.StartDate && x.Type.HasFlag(BanRecalcType.Penalty));

                FixedPeriodCalcPenalties fixPeriod = null;

                var charges = this.accountCharges.FirstOrDefault(x => x.ChargeDate >= periodDto.StartDate && x.ChargeDate <= periodDto.GetEndDate());

                if (this.cache.IsFixCalcPeriod)
                {
                    fixPeriod = this.cache.GetFixPeriodCalc(periodDto.StartDate);
                }

                var period1 = periodDto;

                decimal percentage;
                this.AcquirePercentageAndDays(period1, out percentage, out days);

                var parameter = this.GetPenaltyParameter(periodDto.StartDate, periodDto.GetEndDate());

                if (parameter.IsNotNull())
                {
                    var delayDays = 0;
                    if (this.IsNewPenaltyCalcOfPeriod(period1))
                    {
                        delayDays = this.newPenaltyCalcDays.Value;
                    }

                    result.Add(
                        this.GetPeriodForCalc(
                            period1,
                            percentage,
                            charges.Return(x => x.Penalty),
                            ignore,
                            days,
                            delayDays,
                            fixPeriod));
                }
            }

            return result;
        }

        private Period GetPeriodForCalc(
            IPeriod periodDto,
            decimal recalcPercent,
            decimal realPenalty,
            bool ignore,
            int days,
            int delayDays,
            FixedPeriodCalcPenalties fixPeriod)
        {
            var calcPeriod = new Period(periodDto);

            var deadline = periodDto.StartDate.AddDays(days);

            calcPeriod.Ignore = ignore;
            calcPeriod.Penalty = realPenalty;
            calcPeriod.RecalcPercent = recalcPercent;
            calcPeriod.Deadline = deadline < periodDto.GetEndDate() ? periodDto.StartDate.AddDays(days) : periodDto.GetEndDate();
            calcPeriod.DebtEnd = calcPeriod.Deadline.AddMonths(1).AddDays(-1);
            calcPeriod.StartFix = fixPeriod?.StartDay;
            calcPeriod.EndFix = fixPeriod?.EndDay;

            if (fixPeriod != null)
            {
                if (fixPeriod.EndDay < days)
                {
                    deadline = periodDto.StartDate.AddMonths(-1).AddDays(days);
                    calcPeriod.Deadline = deadline <= periodDto.StartDate.AddDays(-1)
                        ? periodDto.StartDate.AddMonths(-1).AddDays(days)
                        : periodDto.StartDate;
                }

                calcPeriod.DateEnd = new DateTime(calcPeriod.DateStart.Year, calcPeriod.DateStart.Month, fixPeriod.EndDay);
                calcPeriod.DateStart = new DateTime(calcPeriod.DateStart.Year, calcPeriod.DateStart.Month, fixPeriod.StartDay).AddMonths(-1);
            }

            if (delayDays != 0 && fixPeriod == null)
            {
                calcPeriod.DebtEnd = calcPeriod.Deadline.AddDays(delayDays - 1);
                calcPeriod.Deadline = calcPeriod.Deadline.AddMonths(-1).AddDays(delayDays);
            }

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

            if (this.newPenaltyCalcStart != null && this.newPenaltyCalcStart < period.DateStart)
            {
                periodStartDate = period.DateStart.AddMonths(-1);
            }

            //-----1
            var prevSummary = this.summaries
                .Where(x => x.Period.StartDate < periodStartDate)
                .OrderByDescending(x => x.Period.StartDate)
                .FirstOrDefault();

            decimal balanceIn = 0;
            decimal percentage = 0;

            if (prevSummary != null)
            {
                balanceIn = this.calculatePenaltyForDecisionTarif
                    ? prevSummary.BaseTariffDebt + prevSummary.DecisionTariffDebt - (prevSummary.TariffPayment + prevSummary.TariffDecisionPayment)
                    : prevSummary.BaseTariffDebt - prevSummary.TariffPayment;
            }

            DateTime startPayment;

            if (period.StartFix != null || prevSummary == null)
            {
                startPayment = new DateTime(period.DateStart.Year, period.DateStart.Month, 1);
            }
            else
            {
                startPayment = prevSummary.Period.StartDate;
            }

            decimal paymentRestructSum = 0;

            if (this.cache.IsClaimWorkEnabled)
            {
                var restructDetails = this.cache.GetRestructureScheduleDetails(this.account);

                var paymentIds = this.payments
                    .Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                    .Where(x => x.PaymentDate <= this.GetExpiredDate())
                    .Select(x => x.Id);

                paymentRestructSum = restructDetails
                    .Where(x => paymentIds.Contains(x.TransferId))
                    .SafeSum(x => x.Sum);
            }

            var paymentSum = this.calculatePenaltyForDecisionTarif
                ? this.payments
                    .Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                    .SafeSum(x => x.Amount)
                    - this.cancelPayments.Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart).SafeSum(x => x.Amount)
                : this.paymentsBaseTariff
                    .Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                    .SafeSum(x => x.Amount)
                    - this.cancelBaseTariffPayments.Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                        .SafeSum(x => x.Amount);

            //вычитаем оплаты
            balanceIn -= Math.Max(paymentSum - paymentRestructSum, 0);

            var startDate = prevSummary != null ? prevSummary.Period.StartDate : period.DateStart.AddMonths(-1);
            var endDate = prevSummary != null ? prevSummary.Period.GetEndDate() : period.DateStart.AddDays(-1);

            var currentRecalc = this.chargeRecalcHistory
                    .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate < periodStartDate.AddMonths(-1))
                    .Where(x => x.RecalcSum < 0)
                    .SafeSum(x => x.RecalcSum);

            var recalcHistories = this.recalcHistory
                .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                .WhereIf(
                    this.calculatePenaltyForDecisionTarif,
                    x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                .Where(x => x.RecalcPeriod.StartDate <= startDate.AddMonths(-1))
                .Where(x => x.CalcPeriod.StartDate >= startDate)
                .Where(x => x.RecalcSum < 0)
                .ToList();

            var recalcHistorySum = recalcHistories.SafeSum(x => x.RecalcSum) + currentRecalc;

            var recalcBalanceIn = balanceIn + recalcHistorySum;

            var recalcCharge = this.RecalcCharge(startDate, endDate);

            percentage = period.RecalcPercent;

            // Если считаем первый период с новой настройкой, то не прибавляем начисление ко второму периоду задолженности
            if ((this.newPenaltyCalcStart != null && this.newPenaltyCalcStart.Value.Month == period.DateEnd.Month) || prevSummary == null)
            {
                recalcCharge = 0;
            }

            // считаем сумму всех возвратов по базовому тарифу
            var returnPaymentsByBase = this.returnsPaymentBaseTariff
                .Where(x => x.PaymentDate > period.DateStart && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            // считаем сумму всех возвратов по тарифу решения
            var returnPaymentsByDecision = this.returnsPaymentDecisionTariff
                .Where(x => x.PaymentDate > period.DateStart && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            var transfersMerge = this.payments
                .Where(x => x.Operation.Reason == "Перенос долга при слиянии" && x.PaymentDate <= endDate && x.PaymentDate >= startDate)
                .SafeSum(x => x.Amount);

            // зачет средств за проделанные работы
            var perfWorkCharge = this.performedWorkCharge
                .Where(x => x.ChargePeriod.StartDate == periodStartDate.AddMonths(-1))
                .SafeSum(x => x.Sum);

            recalcCharge -= perfWorkCharge;

            //увеличиваем задолженность на сумму возвратов и уменьшаем на сумму возвратов
            recalcBalanceIn += this.calculatePenaltyForDecisionTarif
                ? returnPaymentsByBase + returnPaymentsByDecision + transfersMerge
                : returnPaymentsByBase + transfersMerge;

            var operations = this.GetPartitionOperations(period);

            var periodBalance = new PenaltyPeriodProxy(period.DateStart, recalcBalanceIn, percentage, period.StartFix, period.EndFix);

            if (this.RecalculationReason?.Reason == RecalcReason.ChangeCloseDate && this.RecalculationReason.Date <= periodStartDate)
            {
                periodBalance.RecalcBalance = 0;
                return periodBalance;
            }

            if (period.Id == this.period.Id)
            {
                operations.Add(
                    new OperationProxy
                    {
                        Amount = recalcCharge,
                        Date = period.Deadline.AddDays(-1),
                        NeedSavePayment = false,
                        SkipOperation = period.Deadline == period.DateEnd
                    });
            }
            else
            {
                operations.Add(
                    new OperationProxy
                    {
                        Date = period.Deadline.AddDays(-1),
                        Amount = recalcCharge,
                        SkipOperation = period.Deadline == period.DateEnd,
                        NeedSavePayment = false
                    });
            }

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
                }).OrderBy(x => x.Date).ForEach(x => periodBalance.ApplySum(x));

            return periodBalance;
        }

        private List<OperationProxy> GetPartitionOperations(Period calcPeriod)
        {
            var operations = new List<OperationProxy>();

            var tempPayments = this.calculatePenaltyForDecisionTarif ? this.payments : this.paymentsBaseTariff;

            var cancelPayments = this.calculatePenaltyForDecisionTarif ? this.cancelPayments : this.cancelBaseTariffPayments;

            var detailSchedule = this.cache.GetRestructureScheduleDetails(this.account);

            var tempCancelPayments = cancelPayments
                .Where(x => calcPeriod.Contains(x.PaymentDate))
                .GroupBy(x => x.PaymentDate.Date)
                .Select(
                    x => new
                    {
                        PaymentDate = x.Key,
                        Amount = x.Sum(y => y.Amount)
                    });

            foreach (var cancelPayment in tempCancelPayments)
            {
                var operation = new OperationProxy
                {
                    Date = cancelPayment.PaymentDate,
                    Amount = cancelPayment.Amount,
                    Percentage = calcPeriod.RecalcPercent,
                    PeriodPartitionType = PeriodPartitionType.Refund
                };

                operations.Add(operation);
            }

            var lastPaymentDate = this.cache.RefinancingRate == RefinancingRate.AsConfigured ? calcPeriod.DateStart : this.period.GetEndDate();

            foreach (var payment in tempPayments
                .Where(x => calcPeriod.Contains(x.PaymentDate) && x.Operation.Reason != "Перенос долга при слиянии")
                .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection))
            {
                lastPaymentDate = this.GetNextPaymentDate(payment.PaymentDate);

                if (!lastPaymentDate.IsValid())
                {
                    if (this.cache.RefinancingRate == RefinancingRate.CurrentPeriod)
                    {
                        lastPaymentDate = this.period.GetEndDate();
                    }
                    else
                    {
                        lastPaymentDate = payment.PaymentDate;
                    }
                }

                decimal restructSum = 0;

                if (!this.CheckIsOverdueRestruct(payment.PaymentDate))
                {
                    restructSum = detailSchedule
                        .Where(x => x.TransferId == payment.Id)
                        .SafeSum(x => x.Sum);
                }

                var operation = new OperationProxy
                {
                    Date = payment.PaymentDate,
                    Amount = -payment.Amount,
                    RestructAmount = restructSum,
                    PeriodPartitionType = PeriodPartitionType.Payment,
                    Percentage = this.cache.GetPeriodPenalty(lastPaymentDate, this.account)?.Percentage ?? 0
                };

                operations.Add(operation);
            }

            foreach (var payment in
                tempPayments.Where(x => calcPeriod.Contains(x.PaymentDate))
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

            //  если в текущем периоде и следующих больше нет оплат, то разделяем периоды из-за смены ставки
            if (this.cache.RefinancingRate == RefinancingRate.AsConfigured && !this.GetNextPaymentDate(lastPaymentDate).IsValid())
            {
                var result = this.cache.GetPeriodPenalties(calcPeriod.DateStart, calcPeriod.DateEnd, this.account)
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

            foreach (var perfWork in this.perfWorks.Where(x => calcPeriod.Contains(x.PaymentDate)))
            {
                var operation = new OperationProxy
                {
                    Date = perfWork.PaymentDate,
                    Amount = -perfWork.Amount,
                    PeriodPartitionType = PeriodPartitionType.PerfWork
                };

                operations.Add(operation);
            }

            if (this.RecalculationReason != null && this.RecalculationReason.Reason == RecalcReason.ChangeCloseDate
                && calcPeriod.Contains(this.RecalculationReason.Date))
            {
                var operation = new OperationProxy
                {
                    Date = this.RecalculationReason.Date,
                    PeriodPartitionType = PeriodPartitionType.CloseAccount
                };

                operations.Add(operation);
            }

            if (this.cache.IsClaimWorkEnabled)
            {
                var restructDate = this.cache.GetRestructDate(this.account);

                if (restructDate != null && calcPeriod.Contains(restructDate.Value))
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

                if (expiredDate != null && calcPeriod.Contains(expiredDate.Value))
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