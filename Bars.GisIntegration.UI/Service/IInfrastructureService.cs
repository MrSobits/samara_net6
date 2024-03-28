namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для получения объектов при выполнении импорта/экспорта данных через сервис Infrastructure
    /// </summary>
    public interface IInfrastructureService
    {
        /// <summary>
        /// Получить список объектов ОКИ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetRkiList(BaseParams baseParams);
    }
}
