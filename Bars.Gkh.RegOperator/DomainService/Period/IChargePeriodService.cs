namespace Bars.Gkh.RegOperator.DomainService.Period
{
	using System;
	using B4;

    /// <summary>
    /// Интерфейс для работы с периодами начислений
    /// </summary>
    public interface IChargePeriodService
    {
        /// <summary>
        /// Получить список закрытых периодов
        /// </summary>
        IDataResult ListClosedPeriods(BaseParams baseParams);

        /// <summary>
        /// Открыть начальный период
        /// </summary>
        /// <returns></returns>
        IDataResult CreateFirstPeriod();

		/// <summary>
		/// Получить дату начала первого периода
		/// </summary>
		/// <returns>Дата начала первого периода</returns>
		DateTime GetStartDateOfFirstPeriod();

        /// <summary>
        /// Откатить закрытый месяц
        /// </summary>
        /// <returns></returns>
        IDataResult RollbackClosedPeriod(BaseParams baseParams);

        /// <summary>
        /// Список периодов ЛС
        /// </summary>
        /// <param name="baseParams">id - идентификатор ЛС</param>
        IDataResult ListPeriodsByPersonalAccount(BaseParams baseParams);
    }
}