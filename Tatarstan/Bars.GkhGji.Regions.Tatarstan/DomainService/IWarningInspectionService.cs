namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface IWarningInspectionService
    {
        /// <summary>
        /// Проверить наличие документа
        /// </summary>
        IDataResult CheckAppealCits(BaseParams baseParams);

        /// <summary>
        /// Сформировать предостережение из обращения граждан
        /// </summary>
        IDataResult CreateWithAppealCits(BaseParams baseParams);

        /// <summary>
        /// Метод формирующий список проверок по обращению граждан
        /// </summary>
        IDataResult ListByAppealCits(BaseParams baseParams);
    }
}