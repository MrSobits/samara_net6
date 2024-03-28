namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Сервис дерева задач
    /// </summary>
    public interface ITaskTreeService
    {
        /// <summary>
        /// Получить результат триггера подготовки данных
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера или задачи</param>
        /// <returns>Результат подготовки данных</returns>
        IDataResult GetPreparingDataTriggerResult(BaseParams baseParams);
    }
}