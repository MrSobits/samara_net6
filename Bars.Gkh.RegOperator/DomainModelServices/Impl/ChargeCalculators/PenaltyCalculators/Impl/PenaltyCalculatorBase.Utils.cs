namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    public partial class PenaltyCalculatorBase
    {
        /// <summary>
        /// Расчитать начисления за пеирод
        /// </summary>
        /// <param name="dateStart">Дата начала расчета</param>
        /// <param name="dateEnd">Дата окончания расчета</param>
        /// <returns></returns>
        protected decimal RecalcCharge(DateTime dateStart, DateTime dateEnd)
        {
            var calculator = new ChargeCalculator(this.cache, this.tracker);

            var chargeResult = calculator.Calculate(this.period, this.account, dateStart, dateEnd);

            return chargeResult;
        }

        /// <summary>
        /// Получить параметр начисления пени на пеирод
        /// </summary>
        /// <param name="startDate">Дата начала</param>
        /// <param name="endDate">Дата окончания</param>
        /// <returns></returns>
        protected PaymentPenalties GetPenaltyParameter(DateTime startDate, DateTime endDate)
        {
            return this.cache.GetPeriodPenalty(startDate, endDate, this.account);
        }

        /// <summary>
        /// Получить параметр начисления пени на дату
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected PaymentPenalties GetPenaltyParameter(DateTime date)
        {
            return this.cache.GetPeriodPenalty(date, this.account);
        }

        /// <summary>
        ///     Проверяет, добавлен лицевой счет в исключения параметра начисления пеней
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="penaltyParameter">Параметр начисления пеней</param>
        /// <returns>True - если лицевой счет добавлен в список исключений, иначе - False</returns>
        protected bool AccountExcluded(PaymentPenalties penaltyParameter)
        {
            if (penaltyParameter == null) return false;

            return penaltyParameter.Excludes
                .Select(x => x.PersonalAccount.Id)
                .ToHashSet()
                .Contains(this.account.Id);
        }

        /// <summary>
        /// Получить количество дней просрочки и ставку рефенансирования на период
        /// </summary>
        /// <param name="period"></param>
        /// <param name="percentage"></param>
        /// <param name="days"></param>
        protected void AcquirePercentageAndDays(
            IPeriod period,
            out decimal percentage,
            out int days)
        {
            percentage = this.AcquirePercentage(period).Value;
            days = this.AcquireDays(period).Value;
        }

        /// <summary>
        /// Получить количество дней просрочки на период
        /// </summary>
        /// <param name="period">Пеирод</param>
        /// <returns></returns>
        protected KeyValuePair<DateTime, int> AcquireDays(IPeriod period)
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

        /// <summary>
        /// Получить ставку рефенансирвоания на период
        /// </summary>
        /// <param name="period">Период</param>
        /// <returns></returns>
        protected KeyValuePair<DateTime, decimal> AcquirePercentage(
            IPeriod period)
        {
            return this.AcquirePercentage(period.StartDate, period.GetEndDate());
        }

        /// <summary>
        /// Получить ставку рефенансирвоани на дату
        /// </summary>
        /// <param name="startDate"> Дата начала</param>
        /// <param name="endDate">Дата окончания</param>
        /// <returns></returns>
        protected KeyValuePair<DateTime, decimal> AcquirePercentage(
            DateTime startDate,
            DateTime endDate)
        {
            var parameter = this.GetPenaltyParameter(startDate, endDate);
            var date = startDate;

            if (parameter.Percentage != 0)
            {
                date = this.GetNextPaymentDate(startDate);
            }

            if (this.cache.RefinancingRate == RefinancingRate.CurrentPeriod && !date.IsValid())
            {
                date = this.period.GetEndDate();
            }

            parameter = this.GetPenaltyParameter(date.IsValid() ? date : startDate, endDate);

            return new KeyValuePair<DateTime, decimal>(parameter.Return(x => x.DateStart), parameter.Return(x => x.Percentage));
        }

        /// <summary>
        /// Поулучить дату следующей оплаты
        /// </summary>
        /// <param name="date">Дата оплаты</param>
        /// <returns></returns>
        protected DateTime GetNextPaymentDate(DateTime date)
        {
            return this.calculatePenaltyForDecisionTarif
                    ? this.payments
                        .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection && x.Operation.Reason != "Перенос долга при слиянии")
                        .Where(x => x.PaymentDate > date).OrderBy(x => x.PaymentDate).Select(x => x.PaymentDate).FirstOrDefault()
                    : this.paymentsBaseTariff
                        .Where(x => x.PaymentSource != TypeTransferSource.PaymentCorrection && x.Operation.Reason != "Перенос долга при слиянии")
                        .Where(x => x.PaymentDate > date).OrderBy(x => x.PaymentDate).Select(x => x.PaymentDate).FirstOrDefault();
        }


        /// <summary>
        /// Метод проевряет количество дней допустимой просрочки в предыдущем периоде.
        /// Если дней меньше, то сдвигает начала расчета текущего периода на разницу
        /// <para>Вызывается для разделения периода в связи со сменой допустимого количества дней просрочки</para>
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="daysNow">Дней в предыдущем периоде</param>
        /// <param name="daysBefore">Дней в текущем периоде</param>
        protected void CheckDeadLine(Period period, int daysNow, int daysBefore)
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

        /// <summary>
        /// Создать причины разделения периода
        /// </summary>
        /// <param name="groupData"></param>
        /// <returns></returns>
        protected Partition CreatePartitionPartition(IGrouping<PeriodPartitionType, OperationProxy> groupData)
        {
            if (groupData.Key == PeriodPartitionType.DebtDaysChange)
            {
                var parameter = groupData.FirstOrDefault(x => x.PeriodDaysChange != null)?.PeriodDaysChange;
                return new Partition(PeriodPartitionType.DebtDaysChange, parameter);
            }

            return new Partition(groupData.Key, groupData.Sum(z => z.Amount), groupData.Sum(z => z.RestructAmount));
        }

        private IEnumerable<Period> SimpleGetPeriods(IEnumerable<IPeriod> periods)
        {
            var result = new List<Period>();

            var deadLine = this.datePenaltyCalcTo ?? periods
                .Where(x => x.IsClosed)
                .Select(x => x.StartDate.Date)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault().AddMonths(1);

            foreach (var period in periods)
            {
                var period1 = period;

                int days;
                decimal percentage;
                this.AcquirePercentageAndDays(period1, out percentage, out days);

                var summary = this.summaries.FirstOrDefault(x => x.Period.Id == period.Id);

                var currentDeadLine = period.StartDate.AddMonths(1);

                var daysAll = (deadLine - currentDeadLine).Days;

                result.Add(new Period(
                    period1,
                    percentage,
                    summary.Return(x => x.Penalty),
                    false,
                    daysAll));
            }

            return result;
        }
    }
}