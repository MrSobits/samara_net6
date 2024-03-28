namespace Bars.Gkh.Repositories.ChargePeriod
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Репозиторий периода начислений
    /// </summary>
    public interface IChargePeriodRepository
    {
        /// <summary>
        /// Получить первый период
        /// </summary>
        ChargePeriod GetFirstPeriod(bool useCache = true);

        /// <summary>
        /// Получить текущий открытый период
        /// </summary>
        ChargePeriod GetCurrentPeriod(bool useCache = true);

        /// <summary>
        /// Получить последний закрытый период
        /// </summary>
        ChargePeriod GetLastClosedPeriod();

        /// <summary>
        /// Метод возвращает период с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор периода</param>
        /// <param name="useCache">Использовать кэш, если возможно</param>
        /// <returns>Период</returns>
        ChargePeriod Get(long id, bool useCache = true);

        /// <summary>
        /// Получить период по дате
        /// </summary>
        /// <param name="date">Дата для поиска</param>
        /// <param name="checkEndPeriod">Проверять, не уходит ли дата за конец периода</param>
        ChargePeriod GetPeriodByDate(DateTime date, bool checkEndPeriod = false);

        /// <summary>
        /// Получить предыдущий период относительно нужного нам периода
        /// </summary>
        ChargePeriod GetPreviousPeriod(ChargePeriod period);

        /// <summary>
        /// Метод указывает, что кэш является устаревшим
        /// </summary>
        void InvalidateCache();

        /// <summary>
        /// Инициализировать кэш
        /// </summary>
        void InitCache();

        /// <summary>
        /// Получить все закрытые периоды
        /// </summary>
        IQueryable<ChargePeriod> GetAllClosedPeriods();

        /// <summary>
        /// Получить все периоды
        /// </summary>
        /// <param name="useCache"></param>
        /// <returns>Периоды</returns>
        List<ChargePeriod> GetAll(bool useCache = true);
    }
}