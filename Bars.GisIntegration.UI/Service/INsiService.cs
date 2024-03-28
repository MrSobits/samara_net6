namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для получения объектов при выполнении импорта/экспорта данных через сервис Nsi
    /// </summary>
    public interface INsiService
    {
        /// <summary>
        /// Получить список дополнительных услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetAdditionalServices(BaseParams baseParams);

        /// <summary>
        /// Получить список коммунальных услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetMunicipalServices(BaseParams baseParams);

        /// <summary>
        /// Получить список работ и услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetOrganizationWorks(BaseParams baseParams);
    }
}