namespace Bars.Gkh.Utils
{
    using System;

    /// <summary>
    /// Утилиты работы с датой
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Проверка даты на попадание в период
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <param name="start">Левый край диапазона</param>
        /// <param name="end">Правый край диапазона</param>
        public static bool InRange(this DateTime value, DateTime start, DateTime end)
        {
            return value >= start && value <= end;
        }

        /// <summary>
        /// Получить количество месяцев между двумя датами
        /// </summary>
        /// <param name="from">Дата с</param>
        /// <param name="to">Дата по</param>
        public static int GetMonthsUntilDate(this DateTime from, DateTime to)
        {
            if (from > to) return 0;

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }

            return monthDiff;
        }

        /// <summary>
        /// Возвращает исходную дату с последним днем месяца
        /// пример: (2015,06,01) -> (2015,06,30)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime WithLastDayMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        /// <summary>
        /// Проверяет значение даты
        /// <para><see cref="DateTime.MinValue"/> &lt; date &lt; <see cref="DateTime.MaxValue"/></para>
        /// </summary>
        public static bool IsValid(this DateTime dt)
        {
            return dt > DateTime.MinValue && dt < DateTime.MaxValue;
        }

        /// <summary>
        /// Проверяет значение даты
        /// <para><see cref="DateTime.MinValue"/> &lt; date &lt; <see cref="DateTime.MaxValue"/></para>
        /// </summary>
        public static bool IsValid(this DateTime? dt)
        {
            return dt.HasValue && dt > DateTime.MinValue && dt < DateTime.MaxValue;
        }

        /// <summary>
        /// Прибавить месяц, и установить последний день месяца если 
        /// в исходной дате - последний день месяца
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static DateTime AddMonthByCalc(this DateTime dt, int months)
        {
            if (dt.Day == DateTime.DaysInMonth(dt.Year, dt.Month))
            {
                return dt.AddMonths(months).WithLastDayMonth();
            }

            return dt.AddMonths(months);
        }

        /// <summary>
        /// Получить количество месяцев между датами
        /// </summary>
        public static int GetDiffMonthCount(this DateTime firstDate, DateTime secondDate)
        {
            var dt1 = firstDate.GetFirstDayOfMonth();
            var dt2 = secondDate.GetFirstDayOfMonth();

            return (int) Math.Round(Math.Abs(dt2.Subtract(dt1).Days) / (365.25 / 12));
        }

        /// <summary>
        /// Получить дату первого дня месяца
        /// </summary>
        /// <param name="dt">Дата</param>
        /// <param name="monthOffset">Смещение в месяцах от текущей даты</param>
        public static DateTime GetFirstDayOfMonth(this DateTime dt, int monthOffset = 0)
        {
            if (monthOffset != 0)
            {
                dt = dt.AddMonths(monthOffset);
            }

            return new DateTime(dt.Year, dt.Month, 1);
        }
    }
}