namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    using Entities;
    using Entities.Dict;
    using Entities.PersonalAccount;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Utils;

    /// <summary>
    ///     Калькулятор пени (утилиты)
    /// </summary>
    public partial class PenaltyCalculator
    {
        private decimal RecalcCharge(BasePersonalAccount account, DateTime dateStart, DateTime dateEnd)
        {
            var calculator = new ChargeCalculator(this.cache, this.tracker);

            var chargeResult = calculator.Calculate(this.period, account, dateStart, dateEnd);

            return chargeResult;
        }

        private List<OperationProxy> GetPartitionOperations(Period period)
        {
            var operations = new List<OperationProxy>();

            var tempPayments = this.calculatePenaltyForDecisionTarif ? this.payments : this.paymentsBaseTariff;

            var cancelPayments = this.calculatePenaltyForDecisionTarif ? this.cancelPayments : this.cancelBaseTariffPayments;

            var tempCancelPayments = cancelPayments
                .Where(x => period.Contains(x.PaymentDate))
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
                    Percentage = period.RecalcPercent,
                    PeriodPartitionType = PeriodPartitionType.Refund
                };

                operations.Add(operation);
            }

            var lastPaymentDate = this.cache.RefinancingRate == RefinancingRate.AsConfigured ? period.DateStart : this.period.GetEndDate();

            foreach (var payment in tempPayments
                .Where(x => period.Contains(x.PaymentDate) && x.Operation.Reason != "Перенос долга при слиянии")
                .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection))
            {
                var type = PeriodPartitionType.Payment;

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

                var operation = new OperationProxy
                {
                    Date = payment.PaymentDate,
                    Amount = -payment.Amount,
                    PeriodPartitionType = type,
                    Percentage = this.cache.GetPeriodPenalty(lastPaymentDate, this.account)?.Percentage ?? 0
                };

                operations.Add(operation);
            }

            foreach (var payment in
                    tempPayments.Where(x => period.SimpleContains(x.PaymentDate))
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

            if (this.cache.RefinancingRate == RefinancingRate.AsConfigured)
            {
                var result = this.cache.GetPeriodPenalties(period.DateStart, period.DateEnd, this.account)
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

            foreach (var perfWork in this.perfWorks.Where(x => period.Contains(x.PaymentDate)))
            {
                var operation = new OperationProxy
                {
                    Date = perfWork.PaymentDate,
                    Amount = -perfWork.Amount,
                    PeriodPartitionType = PeriodPartitionType.PerfWork
                };

                operations.Add(operation);
            }

            return operations;
        }

        private List<OperationProxy> GetDebtPartitionOperations(Period period)
        {
            var operations = new List<OperationProxy>();

            var cancelPayments = this.calculatePenaltyForDecisionTarif ? this.cancelPayments : this.cancelBaseTariffPayments;

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

                lastPaymentDate = this.GetNextPaymentDate(payment.PaymentDate);

                if (!lastPaymentDate.IsValid())
                {
                    lastPaymentDate = this.cache.RefinancingRate == RefinancingRate.CurrentPeriod ? this.period.GetEndDate() : payment.PaymentDate;
                }

                var operation = new OperationProxy
                {
                    Date = payment.PaymentDate,
                    Amount = -payment.Amount,
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

            if (this.cache.RefinancingRate == RefinancingRate.AsConfigured)
            {         
                var result = this.cache.GetPeriodPenalties(period.Deadline, period.DebtEnd ?? period.Deadline.AddMonths(1), this.account)
                             .Where(x => x.Key > lastPaymentDate)
                             .ToList();

                result.ForEach(x =>
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


            period.OperationProxy.AddTo(operations);
            

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

            return operations;
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

            if (this.simpleCalc && this.DatePenaltyCalcFrom.HasValue)
            {
                dateStart = this.DatePenaltyCalcFrom.Value;
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
                        var allowDays = this.GetPenaltyParameter(prevPeriod.StartDate, prevPeriod.GetEndDate(), this.account)?.Days ?? 0;

                        var paymentPeriod = closedPeriods
                            .Where(x => x.StartDate.AddDays(allowDays) < firstPayment.PaymentDate)
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

            if (this.DatePenaltyCalcFrom.HasValue && this.DatePenaltyCalcTo.HasValue)
            {
                var dateMin = this.DatePenaltyCalcFrom.Value;
                var dateMax = this.DatePenaltyCalcTo.Value;

                if (dateStart >= dateMin && dateStart <= dateMax)
                {
                    dateStart = dateMin;
                }
            }

            var needToRecalcPeriods = closedPeriods
                .WhereIf(this.simpleCalc, x => dateStart.Date <= x.StartDate.Date && x.EndDate.Value.Date <= this.DatePenaltyCalcTo.Value.Date)
                .WhereIf(!this.simpleCalc, x => dateStart.Date <= x.EndDate.Value.Date && x.EndDate.Value.Date <= this.period.StartDate.Date)
                .ToList();

            var events = this.cache.GetRecalcEvents(this.account);
            if (events.IsNotEmpty() && !this.simpleCalc)
            {
                var @event = events.ToLookup(x => x.RecalcType)[PersonalAccountRecalcEvent.PenaltyType].OrderBy(x => x.EventDate).FirstOrDefault();
                if (@event != null)
                {
                    var periodsForEvent = this.numberDaysDelay == NumberDaysDelay.StartDateMonth
                        ? closedPeriods
                            .Where(x => x.EndDate >= @event.EventDate)
                        : closedPeriods
                            .Where(x => x.StartDate > @event.EventDate);

                    var except = periodsForEvent.Except(
                        needToRecalcPeriods,
                        FnEqualityComparer<IPeriod>.Member(x => x.Id)).ToList();

                    if (this.RecalculationReason == null || @event.EventDate < this.RecalculationReason.Date)
                    {
                        this.RecalculationReason = new RecalcReasonProxy
                        {
                            Reason = @event.PersonalAccount == null ? RecalcReason.ChangePenaltyParametrs : RecalcReason.Payment,
                            Date = @event.EventDate
                        };
                    }

                    needToRecalcPeriods.AddRange(except);
                }
            }

            // если используется упрощенный перерасчет 
            // то количество дней задолженности нужно считать от рассчитываемого периода до текущего
            if (this.simpleCalc)
            {
                return this.SimpleGetPeriods(this.account, needToRecalcPeriods).OrderBy(x => x.DateStart).ToList();
            }

            needToRecalcPeriods.Add(current);

            // Периоды для упрощенного расчета пени
            var simplePeriods = new List<Period>();

            if (this.simpleCalc)
            {
                if (this.DatePenaltyCalcTo != null)
                {
                    simplePeriods =
                    this.SimpleGetPeriods(
                        this.account,
                        needToRecalcPeriods.Where(x => x.StartDate < this.DatePenaltyCalcTo.ToDateTime().AddMonths(-1)))
                        .OrderBy(x => x.DateStart)
                        .ToList();
                }
            }

            // Периоды для обычно расчета пени
            var periods = this.numberDaysDelay == NumberDaysDelay.StartDateMonth
                ? this.GetPeriods(this.account, needToRecalcPeriods.WhereIf(this.DatePenaltyCalcTo != null, x => x.StartDate > this.DatePenaltyCalcTo).ToList())
                    .OrderBy(x => x.DateStart)
                    .ToList()
                : this.GetDebtPeriods(this.account, needToRecalcPeriods.WhereIf(this.DatePenaltyCalcTo != null, x => x.StartDate > this.DatePenaltyCalcTo).ToList())
                    .OrderBy(x => x.DateStart)
                    .ToList();

            periods.AddRange(simplePeriods);

            return periods;
        }

        private IEnumerable<Period> GetPeriods(BasePersonalAccount account, IList<IPeriod> periods)
        {
            var result = new List<Period>();

            var banRecalc = this.cache.GetBanRecalc(account).ToList();

            // независимо от того, какой период пересчитывается, используется ставка рефинансирования текущего периода
            // а количество дней просрочки рассчитываемого
            int days;
            decimal currentPercentage;
            this.AcquirePercentageAndDays(account, this.period, out currentPercentage, out days);

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

                decimal percentage;
                this.AcquirePercentageAndDays(account, periodDto, out percentage, out days);

                var parameter = this.GetPenaltyParameter(periodDto.StartDate, periodDto.GetEndDate(), account);

                var curPercentage = currentPercentage;

                if (this.AccountExcluded(account, parameter))
                {
                    curPercentage = 0;
                }

                if (parameter.IsNotNull())
                {
                    var delayDays = 0;
                    if (this.IsNewPenaltyCalcOfPeriod(periodDto))
                    {
                        delayDays = this.NewPenaltyCalcDays.Value;
                    }
                    var tempPeriod = new Period(
                        periodDto,
                        percentage,
                        charges.Return(x => x.Penalty),
                        ignore,
                        days,
                        delayDays,
                        fixPeriod);

                    if (fixPeriod != null)
                    {
                        tempPeriod.RecalcPercent = this.GetPenaltyParameter(tempPeriod.DateStart, tempPeriod.DateEnd, account)?.Percentage ?? tempPeriod.RecalcPercent;
                    }

                    result.Add(tempPeriod);
                }
            }

            return result;
        }

        private IEnumerable<Period> GetDebtPeriods(BasePersonalAccount account, IEnumerable<IPeriod> periods)
        {
            var result = new List<Period>();

            var banRecalc = this.cache.GetBanRecalc(account).ToList();
            
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
                var dayParam = this.AcquireDays(account, periodDto);

                var period1 = periodDto;

                var prevParameter = dayParam.Value <= 0 ? 0 : this.cache.GetPeriodDays(periodDto.StartDate.AddMonths(-1), periodDto.GetEndDate().AddMonths(-1), account).Value;

                // берем только те периоды, для которых есть параметры
                if (dayParam.Value > 0)
                {
                    int delayDays = 0;
                    if (this.NewPenaltyCalcStart < startDate && this.NewPenaltyCalcDays != null)
                    {
                        delayDays = this.NewPenaltyCalcDays.Value;
                    }

                    var per = new Period(
                        period1,
                        0,
                        charges.Return(x => x.Penalty),
                        ignore,
                        dayParam.Value,
                        delayDays,
                        null);

                    this.CheckDeadLine(per, dayParam.Value, prevParameter);

                    per.RecalcPercent = this.AcquirePercentage(account, per.Deadline, per.DebtEnd ?? DateTime.MaxValue).Value;

                    if (dayParam.Key.AddMonths(1) > per.Deadline && prevParameter == 0)
                    {
                        per.OperationProxy.Add(
                        new OperationProxy
                        {
                            Date = dayParam.Key.AddMonths(1).AddDays(-1),
                            Percentage = per.RecalcPercent,
                            PeriodPartitionType = PeriodPartitionType.Empty
                        });

                        per.RecalcPercent = 0;
                    }

                    if (per.OldDeadLine.HasValue && per.OldDeadLine > per.Deadline)
                    {
                        var operation = new OperationProxy
                        {
                            Date = per.OldDeadLine.Value.AddDays(-1),
                            PeriodPartitionType = PeriodPartitionType.DebtDaysChange,
                            PeriodDaysChange = per.PeriodDaysChange
                        };

                        per.OperationProxy.Add(operation);
                    }

                    result.Add(per);
                }
            }

            return result;
        }

        private IEnumerable<Period> SimpleGetPeriods(BasePersonalAccount account, IEnumerable<IPeriod> periods)
        {
            var result = new List<Period>();

            var deadLine = this.DatePenaltyCalcTo ?? periods
                .Where(x => x.IsClosed)
                .Select(x => x.StartDate.Date)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault().AddMonths(1);

            foreach (var period in periods)
            {
                var period1 = period;

                int days;
                decimal percentage;
                this.AcquirePercentageAndDays(account, period1, out percentage, out days);

                var summary = this.summaries.FirstOrDefault(x => x.Period.Id == period.Id);

                var currentDeadLine = period.StartDate.AddMonths(1);

                var daysAll = (deadLine - currentDeadLine).Days;

                int delayDays = 0;
                if (this.IsNewPenaltyCalcOfPeriod(period1))
                {
                    delayDays = this.NewPenaltyCalcDays.Value;
                }

                result.Add(new Period(
                    period1,
                    percentage,
                    summary.Return(x => x.Penalty),
                    false,
                    daysAll,
                    delayDays,
                    null));
            }

            return result;
        }

        private PaymentPenalties GetPenaltyParameter(DateTime startDate, DateTime endDate, BasePersonalAccount account)
        {
            return this.cache.GetPeriodPenalty(startDate, endDate, account);
        }

        private PaymentPenalties GetPenaltyParameter(DateTime date, BasePersonalAccount account)
        {
            return this.cache.GetPeriodPenalty(date, account);
        }

        /// <summary>
        ///     Проверяет, добавлен лицевой счет в исключения параметра начисления пеней
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="penaltyParameter">Параметр начисления пеней</param>
        /// <returns>True - если лицевой счет добавлен в список исключений, иначе - False</returns>
        private bool AccountExcluded(BasePersonalAccount account, PaymentPenalties penaltyParameter)
        {
            if (penaltyParameter == null) return false;

            return penaltyParameter.Excludes
                .Select(x => x.PersonalAccount.Id)
                .ToHashSet()
                .Contains(account.Id);
        }

        private void AcquirePercentageAndDays(
            BasePersonalAccount account,
            IPeriod period,
            out decimal percentage,
            out int days)
        {
            percentage = this.AcquirePercentage(account, period).Value;
            days = this.AcquireDays(account, period).Value;
        }

        private KeyValuePair<DateTime, int> AcquireDays(
            BasePersonalAccount account,
            IPeriod period)
        {
            var parameter = this.cache.GetPeriodDays(period.StartDate, period.GetEndDate(), this.account);

            var penaltyDelays = this.cache.GetOwnerPenaltyDelays(account, period.StartDate);

            var penaltyDelay = penaltyDelays
                .Where(x => x.From <= period.StartDate)
                .FirstOrDefault(x => !x.To.HasValue || x.To >= period.StartDate);

            if (penaltyDelay != null)
            {
                var value = penaltyDelay.MonthDelay
                    ? DateTime.DaysInMonth(period.StartDate.Year, period.StartDate.Month)
                    : penaltyDelay.DaysDelay;

                return new KeyValuePair<DateTime, int>(penaltyDelay.From, value);
            }
            else
            {
                return parameter;
            }
        }

        private KeyValuePair<DateTime, decimal> AcquirePercentage(
            BasePersonalAccount account,
            IPeriod period)
        {
            return this.AcquirePercentage(account, period.StartDate, period.GetEndDate());
        }

        private KeyValuePair<DateTime, decimal> AcquirePercentage(
            BasePersonalAccount account,
            DateTime startDate,
            DateTime endDate)
        {
            var date = this.GetNextPaymentDate(startDate);

            if (this.cache.RefinancingRate == RefinancingRate.CurrentPeriod && !date.IsValid())
            {
                date = this.period.GetEndDate();
            }

         //   var parameter = this.GetPenaltyParameter(date.IsValid() ? date : startDate, account); пробеум брать для расчета ставку за открытый период
            var parameter = this.GetPenaltyParameter(date.IsValid() ? date : endDate, account);

            return new KeyValuePair<DateTime, decimal>(parameter.Return(x => x.DateStart), parameter.Return(x => x.Percentage));
        }

        private DateTime GetNextPaymentDate(DateTime date)
        {
            return this.calculatePenaltyForDecisionTarif
                    ? this.payments
                        .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection && x.Operation.Reason != "Перенос долга при слиянии")
                        .Where(x => x.PaymentDate > date).OrderBy(x => x.PaymentDate).Select(x => x.PaymentDate).FirstOrDefault()
                    : this.paymentsBaseTariff
                        .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection && x.Operation.Reason != "Перенос долга при слиянии")
                        .Where(x => x.PaymentDate > date).OrderBy(x => x.PaymentDate).Select(x => x.PaymentDate).FirstOrDefault();
        }


        private void CheckDeadLine(Period period, int daysNow, int daysBefore)
        {
            if (daysBefore > 0)
            {
                var days = daysBefore - daysNow;

                if (days != 0)
                {
                    period.OldDeadLine = period.Deadline;
                    period.Deadline = period.Deadline.AddDays(days);
                    period.PeriodDaysChange = new PeriodDaysChange { NewDays = daysNow, OldDays = daysBefore };

                    var daysInPeriod = DateTime.DaysInMonth(period.DateStart.Year, period.DateStart.Month);
                    if (days < 0 && daysNow > daysInPeriod)
                    {
                        period.Deadline += TimeSpan.FromDays(daysNow - daysInPeriod + 1);
                    }
                }
            }
        }

        private Partition CreatePartitionPartition(IGrouping<PeriodPartitionType, OperationProxy> groupData)
        {
            if (groupData.Key == PeriodPartitionType.DebtDaysChange)
            {
                var parameter = groupData.FirstOrDefault(x => x.PeriodDaysChange != null)?.PeriodDaysChange;
                return new Partition(PeriodPartitionType.DebtDaysChange, parameter);
            }

            return new Partition(groupData.Key, groupData.Sum(z => z.Amount));
        }
    }
}