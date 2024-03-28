namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для получения объектов при выполнении импорта/экспорта данных через сервис Inspection
    /// </summary>
    public interface IInspectionService
    {
        /// <summary>
        /// Получить список планов проверок
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetPlanList(BaseParams baseParams);

        /// <summary>
        /// Получить список инспекционных проверок
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetInspectionList(BaseParams baseParams);
    }
}
