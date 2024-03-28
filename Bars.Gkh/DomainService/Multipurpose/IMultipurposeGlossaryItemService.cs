namespace Bars.Gkh.DomainService.Multipurpose
{
    using Bars.B4;

    public interface IMultipurposeGlossaryItemService
    {
        /// <summary>
        /// Получает элементы справочника по коду,
        /// если код пустой - в результате ничего
        /// </summary>
        ListDataResult ListByGlossaryCode(BaseParams baseParams);
    }
}