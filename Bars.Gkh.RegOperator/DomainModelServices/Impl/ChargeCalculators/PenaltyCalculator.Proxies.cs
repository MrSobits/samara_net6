namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Enums;

    partial class PenaltyCalculator
    {
        public class Period
        {
            public Period(IPeriod period, decimal recalcPercent, decimal realPenalty, bool ignore, int days, int delayDays, FixedPeriodCalcPenalties fixPeriod, bool simple = false)
                : this(period)
            {
                var deadline = period.StartDate.AddDays(days);

                this.Ignore = ignore;
                this.Penalty = realPenalty;
                this.RecalcPercent = recalcPercent;
                this.Deadline = deadline < period.GetEndDate() ? period.StartDate.AddDays(days) : period.GetEndDate();
                this.DebtEnd = this.Deadline.AddMonths(1).AddDays(-1);
                this.Simple = simple;
                this.StartFix = fixPeriod?.StartDay;
                this.EndFix = fixPeriod?.EndDay;

                if (fixPeriod != null)
                {
                    if (fixPeriod.EndDay < days)
                    {
                        deadline = period.StartDate.AddMonths(-1).AddDays(days);
                        this.Deadline = deadline <= period.StartDate.AddDays(-1) ? period.StartDate.AddMonths(-1).AddDays(days) : period.StartDate;
                    }
                      
                    this.DateEnd = new DateTime(this.DateStart.Year, this.DateStart.Month, fixPeriod.EndDay);
                    this.DateStart = new DateTime(this.DateStart.Year, this.DateStart.Month, fixPeriod.StartDay).AddMonths(-1);
                }

                if (delayDays != 0)
                {
                    this.DebtEnd = this.Deadline.AddDays(delayDays - 1);
                    this.Deadline = this.Deadline.AddMonths(-1).AddDays(delayDays);
                }
            }

            public Period(IPeriod period, PaymentPenalties parameter)
                : this(period)
            {
                this.RecalcPercent = parameter.Return(x => x.Percentage);
                this.Deadline = period.StartDate.AddDays(parameter.Return(x => x.Days));
            }

            private Period(IPeriod period)
            {
                this.Id = period.Id;
                this.DateStart = period.StartDate;
                this.DateEnd = period.GetEndDate();
                this.Name = period.Name;
                this.OperationProxy = new List<OperationProxy>();
            }

            public long Id { get; private set; }

            public string Name { get; private set; }

            public decimal RecalcPercent { get; set; }

            public DateTime DateStart { get; private set; }

            public DateTime Deadline { get; set; }

            public DateTime? DebtEnd { get; set; }

            public DateTime DateEnd { get; private set; }

            public decimal Penalty { get; private set; }

            public bool Ignore { get; set; }

            public bool Simple { get; set; }

            public DateTime? OldDeadLine { get; set; }

            public PeriodDaysChange PeriodDaysChange { get; set; }

            public int? StartFix { get; set; }

            public int? EndFix { get; set; }

            public List<OperationProxy> OperationProxy { get; set; }
             
            public bool Contains(DateTime date)
            {
                return this.DateStart <= date && date <= this.DateEnd;
            }

            public bool SimpleContains(DateTime date)
            {
                return this.Deadline <= date && date <= (this.DebtEnd ?? this.Deadline.AddMonths(1).AddDays(-1));
            }
        }

        private class PenaltyPeriodProxy
        {
            public DateTime periodStart;

            public readonly List<PenaltyPeriod> Penalties;

            public PenaltyPeriodProxy(DateTime dateStart, decimal currRecalcBalance, decimal percentage, int? startFix, int? endFix)
                : this()
            {
                this.periodStart = dateStart;
                this.RecalcBalance = currRecalcBalance;
                this.DebtEnd = dateStart.AddMonths(1).AddDays(-1);
                this.Percentage = percentage;
                this.StartFix = startFix;
                this.EndFix = endFix;
            }

            public PenaltyPeriodProxy(DateTime dateStart, decimal currRecalcBalance, DateTime? debtEnd, decimal percentage)
                : this()
            {
                this.periodStart = dateStart;
                this.RecalcBalance = currRecalcBalance;
                this.DebtEnd = debtEnd ?? dateStart.AddMonths(1).AddDays(-1);
                this.Percentage = percentage;
            }

            private PenaltyPeriodProxy()
            {
                this.Penalties = new List<PenaltyPeriod>();
            }

            public decimal RecalcBalance { get; set; }

            public bool Simple { get; set; }

            public DateTime? DebtEnd { get; set; }

            public decimal Percentage { get; set; }

            public int? StartFix { get; set; }

            public int? EndFix { get; set; }

            // если sum > 0 то начисление, иначе оплата
            public void ApplySum(OperationProxyResult operation)
            {
                if (this.NeedIgnore(operation))
                {
                    return;
                }

                var penalty = this.GetCurrent();

                var newPercentageFromPeriodStart = operation.Has(PeriodPartitionType.PercentageChange)
                                                   && operation.Date.Date.AddDays(1) == this.periodStart;

                if (newPercentageFromPeriodStart)
                {
                    penalty.Percentage = operation.Percentage;
                    this.Percentage = operation.Percentage;

                    // если нет других операций, то на этом и заканчиваем
                    if (operation.Partitions.All(x => x.PartitionType == PeriodPartitionType.PercentageChange))
                    {
                        return;
                    }
                }

                // не создаём однодневный разделитель
                if (this.Penalties.IsNotEmpty())
                {
                    // закрываем предыдущий период
                    penalty.RecalcBalance = this.RecalcBalance;
                    var tmpEnd = operation.SkipOperation ? operation.Date.AddDays(1) : operation.Date;
                    penalty.End = this.periodStart > tmpEnd ? this.periodStart : tmpEnd;
                    penalty.Partitions = operation.Partitions;

                    this.RecalcBalance -= operation.Amount;

                    if (operation.NeedSavePayment)
                    {
                        penalty.Payment = operation.Amount;
                    }
                }

                if (!operation.SkipOperation)
                {
                    this.Percentage = operation.Percentage != 0 ? operation.Percentage : this.Percentage;

                    var newPenalty = new PenaltyPeriod
                    {
                        Start = operation.Date.AddDays(1),
                        RecalcBalance = this.RecalcBalance,
                        End = this.DebtEnd,
                        Percentage = this.Percentage,
                    };

                    this.Penalties.Add(newPenalty);
                }
            }

            //если sum > 0 то начисление, иначе оплата
            public void ApplyDebtSum(OperationProxyResult operation)
            {
                if (this.NeedIgnore(operation))
                {
                    return;
                }

                var penalty = this.GetCurrent();

                var newPercentageFromPeriodStart = operation.Has(PeriodPartitionType.PercentageChange)
                                                   && operation.Date.Date.AddDays(1) == this.periodStart;

                if (newPercentageFromPeriodStart)
                {
                    penalty.Percentage = operation.Percentage;
                    this.Percentage = operation.Percentage;

                    // если нет других операций, то на этом и заканчиваем
                    if (operation.Partitions.All(x => x.PartitionType == PeriodPartitionType.PercentageChange))
                    {
                        return;
                    }
                }
                
                //закрываем предыдущий период
                penalty.RecalcBalance = this.RecalcBalance;

                var tmpEnd = operation.Date;
                penalty.End = this.periodStart > tmpEnd ? this.periodStart : tmpEnd;
                penalty.Partitions = operation.Partitions;
                this.RecalcBalance -= operation.Amount;
                penalty.Payment = operation.Amount;

                if (operation.Date != this.DebtEnd)
                {
                    var newPenalty = new PenaltyPeriod
                    {
                        Start = operation.Date.AddDays(1),
                        RecalcBalance = this.RecalcBalance,
                        End = this.DebtEnd,
                        Percentage = operation.Percentage != 0 ? operation.Percentage : penalty.Percentage,
                    };

                    this.Penalties.Add(newPenalty);
                }
            }

            public void SimpleApplySum(DateTime date)
            {
                var penalty = this.GetCurrent();

                //закрываем предыдущий период
                penalty.RecalcBalance = this.RecalcBalance;

                var tmpEnd = date.AddDays(-1);
                penalty.End = this.periodStart > tmpEnd ? this.periodStart : tmpEnd;
            }

            private bool NeedIgnore(OperationProxyResult operation)
            {
                // если изменение касается ставки рефинансирования, но значение ставки не изменилось, то не делим период
                if (operation.Has(PeriodPartitionType.PercentageChange))
                {
                    // здесь хранится последняя ставка рефинансирования
                    var lastCalcPercentage = this.Penalties.LastOrDefault()?.Percentage ?? this.Percentage;
                    if (operation.Percentage == lastCalcPercentage)
                    {
                        // если период делит только смена ставки
                        if (operation.Partitions.All(x => x.PartitionType == PeriodPartitionType.PercentageChange))
                        {
                            return true;
                        }
                    }
                }

                // если суммарно оплата нулевая, то не учитываем
                if (operation.Partitions.Any(x => 
                    x.PartitionType == PeriodPartitionType.Payment || 
                    x.PartitionType == PeriodPartitionType.Refund  || 
                    x.PartitionType == PeriodPartitionType.PaymentCorrection)
                    && operation.Amount == 0)
                {
                    return true;
                }

                return false;
            }

            private PenaltyPeriod GetCurrent()
            {
                var penalty = this.Penalties.FirstOrDefault(x => !x.End.HasValue || x.End == this.DebtEnd);

                if (penalty == null)
                {
                    penalty = new PenaltyPeriod { Start = this.periodStart, End = this.DebtEnd, Percentage = this.Percentage };
                    this.Penalties.Add(penalty);
                }

                return penalty;
            }
        }

        /// <summary>
        /// Период задолженности
        /// </summary>
        private class PenaltyPeriod
        {
            public DateTime Start { get; set; }

            public DateTime? End { get; set; }

            public decimal RecalcBalance { get; set; }

            public decimal Payment { get; set; }

            public decimal Percentage { get; set; }

            public Partition[] Partitions { get; set; }
        }

        /// <summary>
        /// Операция по лс
        /// </summary>
        public class OperationProxy
        {
            public OperationProxy()
            {
                this.NeedSavePayment = true;
            }

            public DateTime Date { get; set; }

            public decimal Amount { get; set; }

            public PeriodPartitionType PeriodPartitionType { get; set; }

            public bool NeedSavePayment { get; set; }

            public bool SkipOperation { get; set; }

            public decimal Percentage { get; set; }

            public PeriodDaysChange PeriodDaysChange { get; set; }
        }

        private class OperationProxyResult
        {
            public DateTime Date { get; set; }

            public decimal Amount => this.Partitions.Sum(x => x.Amount);

            public bool NeedSavePayment { get; set; }

            public bool SkipOperation { get; set; }

            public decimal Percentage { get; set; }

            public Partition[] Partitions { get; set; }

            public bool Has(PeriodPartitionType type) => this.Partitions?.Any(x => x.PartitionType == type) ?? false;
        }

        /// <summary>
        /// Причина перерасчета
        /// </summary>
        private class RecalcReasonProxy
        {
            public RecalcReason Reason { get; set; }

            public DateTime Date { get; set; }
        }
    }
}