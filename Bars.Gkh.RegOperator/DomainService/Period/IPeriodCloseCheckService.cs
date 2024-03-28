namespace Bars.Gkh.RegOperator.DomainService.Period
{
    using Bars.B4;

    /// <summary>
    /// Сервис работы с проверками перед закрытием периода
    /// </summary>
    public interface IPeriodCloseCheckService
    {
        /// <summary>
        /// Возвращает список зарегистрированных чекеров
        /// </summary>
        /// <returns></returns>
        IDataResult ListCheckers(BaseParams baseParams);

        /// <summary>
        /// Запускает проверку
        /// </summary>
        /// <returns></returns>
        IDataResult RunCheck();

        /// <summary>
        /// Запускает проверку и закрытие периода в случае успеха
        /// </summary>
        /// <returns></returns>
        IDataResult RunCheckAndClosePeriod(BaseParams baseParams);        
    }
}