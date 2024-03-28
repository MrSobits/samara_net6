namespace Bars.Gkh.Gis.DomainService.Dict
{
    using Bars.B4;

    /// <summary>
    /// Сервис импорта сведений по тарифам из ЕИАС
    /// </summary>
    public interface IEiasTariffImportService
    {
        /// <summary>
        /// Запуск импорта
        /// </summary>
        IDataResult Import(BaseParams baseParams);
    }
}