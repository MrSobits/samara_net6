namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;

    using Entities;
    using Gkh.Utils;

    public static class ChargePeriodExtensions
    {
        public static bool InPeriod(this ChargePeriod period, DateTime date)
        {
            var endDate = period.GetEndDate();
            return date.InRange(period.StartDate, endDate);
        }

        public static DateTime GetEndDate(this IPeriod period)
        {
            ArgumentChecker.NotNull(period, "period");
            return period.EndDate ?? period.StartDate.AddMonths(1).AddDays(-1);
        }

        public static DateTime GetCurrentInPeriodDate(this ChargePeriod period)
        {
            var now = DateTime.Now;

            var periodDays = DateTime.DaysInMonth(period.StartDate.Year, period.StartDate.Month);

            var periodStart = period.StartDate;

            return new DateTime(periodStart.Year, periodStart.Month, now.Day > periodDays ? periodDays : now.Day, now.Hour, now.Minute, now.Second);
        }
    }
} 