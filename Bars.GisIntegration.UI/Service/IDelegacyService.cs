namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса делегирования
    /// </summary>
    public interface IDelegacyService
    {
        /// <summary>
        /// Добавить поставщиков информации
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult AddInformationProviders(BaseParams baseParams);
    }
}
