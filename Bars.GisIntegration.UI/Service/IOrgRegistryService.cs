namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для получения объектов при выполнении импорта/экспорта данных через сервис OrgRegistry
    /// </summary>
    public interface IOrgRegistryService
    {
        /// <summary>
        /// Получить список контрагентов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetContragentList(BaseParams baseParams);
    }
}
