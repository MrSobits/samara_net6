namespace Bars.GisIntegration.Smev.DomainService
{
    using B4;

    public interface IErpIntegrationService
    {
        /// <summary>
        /// Отправить распоряжение
        /// </summary>
        IDataResult SendDisposal(BaseParams baseParams);

        /// <summary>
        /// Запросить справочник прокуратур
        /// </summary>
        IDataResult RequestProsecutorsOffices(BaseParams baseParams);
    }
}