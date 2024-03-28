namespace Bars.Gkh.Utils
{
    using System;

    public interface IDateRange<T>
        where T : struct
    {
        T StartPeriod { get; }
        T EndPeriod { get; }
        bool Includes(T value);
        bool Includes(IDateRange<T> range);
        bool Intersect(IDateRange<T> range);
    }

    /// <summary>
    /// Класс для работы с периодами дат
    /// <example>
    ///     // Исходный период(первый)
    ///     var startDate1 = new DateTime(2014, 04, 01);
    ///     var endDate1 = new DateTime(2014, 06, 01);
    /// 
    ///     // Период который сравниваем с исходным(второй)
    ///     var startDate2 = new DateTime(2014, 05, 01);
    ///     var endDate2 = new DateTime(2014, 07, 01);
    /// 
    ///     var range = new DateRange(startDate1, endDate1);
    ///     var range2 = new DateRange(startDate2, endDate2);
    /// 
    ///     return range.Includes(range2); // Вернет false так как второй период не входит в первый полностью
    ///     return range.Intersect(range2) // Вернет true так как второй период пересекается с первым
    /// </example>
    /// </summary>
    public class DateRange : IDateRange<DateTime>
    {
        public DateRange(DateTime? start, DateTime? end)
        {
            StartPeriod = start.HasValue
                ? start.Value
                : DateTime.MinValue;
            EndPeriod = end.HasValue
                ? end.Value
                : DateTime.MaxValue;
        }

        /// <summary>Начало периода</summary>
        public DateTime StartPeriod { get; private set; }
        /// <summary>Окончание периода</summary>
        public DateTime EndPeriod { get; private set; }

        /// <summary>
        /// Проверяет попадает ли переданное значение в диапазон дат
        /// </summary>
        /// <param name="value">Значение даты</param>
        /// <returns>
        ///     True - если дата попадает в заданный период
        ///     False - иначе
        /// </returns>
        public bool Includes(DateTime value)
        {
            return (StartPeriod <= value) && (value <= EndPeriod);
        }

        /// <summary>
        /// Проверяет попадает ли переданный период дат в заданный диапазон
        /// </summary>
        /// <param name="range">Период</param>
        /// <returns>
        ///     True - если переданный период попадает в заданный период
        ///     False - иначе
        /// </returns>
        public bool Includes(IDateRange<DateTime> range)
        {
            return (StartPeriod <= range.StartPeriod) && (range.EndPeriod <= EndPeriod);
        }

        /// <summary>
        /// Проверяет пересекается ли переданный период дат с заданным диапазоном
        /// </summary>
        /// <param name="range">Период</param>
        /// <returns>
        ///     True - если переданный период пересекается со сравниваемым периодом
        ///     False - иначе
        /// </returns>
        public bool Intersect(IDateRange<DateTime> range)
        {
            if (EndPeriod < range.StartPeriod)
            {
                return false;
            }

            return (EndPeriod >= range.StartPeriod) && (StartPeriod <= range.EndPeriod);
        }
    }
}
