namespace Bars.Gkh.RegOperator.Distribution
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс для подтверждения/отмены банковских выписок
    /// </summary>
    public interface IDistributionService
    {
        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Apply(BaseParams baseParams);

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Undo(BaseParams baseParams);

        /// <summary>
        /// Отменить распределение частично
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult UndoPartially(BaseParams baseParams);
    }
}