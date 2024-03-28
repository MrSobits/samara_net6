namespace Bars.Gkh.Reforma.Domain
{
    using Bars.B4;

    /// <summary>
    /// Сервис логов
    /// </summary>
    public interface ISyncLogService
    {
        /// <summary>
        /// Возвращает список сессий
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult ListSessions(BaseParams baseParams);

        /// <summary>
        /// Возвращает список действий в рамках сессии
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult ListActions(BaseParams baseParams);

        /// <summary>
        /// Возвращает детали действия
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult GetActionDetails(BaseParams baseParams);

        /// <summary>
        /// Повторяет указанное действие.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult ReplayAction(BaseParams baseParams);
    }
}