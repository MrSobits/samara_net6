namespace Bars.Gkh.Overhaul.DomainService
{
    using Bars.B4;

    public interface IRealityObjectStructElService
    {
        /// <summary>
        /// Получить историю изменений
        /// </summary>
        IDataResult GetHistory(BaseParams baseParams);

        /// <summary>
        /// Получить детализацию история изменений
        /// </summary>
        IDataResult GetHistoryDetail(BaseParams baseParams);
    }
}