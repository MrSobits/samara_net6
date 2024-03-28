namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с Мониторинг жилищного фонда
    /// </summary>
    public interface IHousingFundMonitoringService
    {
        /// <summary>
        /// Массовое создание периодов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult MassCreate(BaseParams baseParams);
    }
}
