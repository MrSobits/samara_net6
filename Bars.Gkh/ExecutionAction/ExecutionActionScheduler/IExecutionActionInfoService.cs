namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using System.Collections.Generic;

    /// <summary>
    /// Сервис получения информации о зарегистрированных в контейнере действиях
    /// </summary>
    public interface IExecutionActionInfoService
    {
        /// <summary>
        /// Получить информацию о действии по коду
        /// </summary>
        /// <param name="actionCode">Код выполняемого действия <see cref="IExecutionAction.Code"/></param>
        /// <returns>null если действие не зарегистрировано</returns>
        ExecutionActionInfo GetInfo(string actionCode);

        /// <summary>
        /// Получить информацию обо всех действиях
        /// </summary>
        /// <param name="includeHidden">Включая действия скрытые аттрибутом <see cref="HiddenActionAttribute"/></param>
        IList<ExecutionActionInfo> GetInfoAll(bool includeHidden = false);
    }
}