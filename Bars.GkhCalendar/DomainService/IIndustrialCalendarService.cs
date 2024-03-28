namespace Bars.GkhCalendar.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.GkhCalendar.Entities;

    public interface IIndustrialCalendarService
    {
        /// <summary>
        /// Возвращает количество рабочих дней между двумя датами
        /// </summary>
        /// <param name="startDate">
        /// Начальная дата
        /// </param>
        /// <param name="endDate">
        /// Конечная дата
        /// </param>
        /// <returns>
        /// Количество рабочих дней
        /// </returns>
        int GetWorkDaysCount(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Возвращает рабочие дни в указанном диапазоне дат
        /// </summary>
        /// <param name="startDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата</param>
        /// <returns>Список рабочих дней</returns>
        List<Day> GetWorkDays(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Возвращает дату n-того рабочего дня, считая с переданной
        /// </summary>
        /// <param name="date">
        /// Дата
        /// </param>
        /// <param name="workDaysCount">
        /// Количество рабочих дней
        /// </param>
        /// <returns>
        /// Дата n-того рабочего дня
        /// </returns>
        DateTime GetDateAfterWorkDays(DateTime date, uint workDaysCount);
    }
}