namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface IDpkrCorrectionService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ChangeIndexNumber(BaseParams baseParams);

        IDataResult GetActualizeYears(BaseParams baseParams);

        IDataResult ListForMassChangeYear(BaseParams baseParams);

        IDataResult MassChangeYear(BaseParams baseParams);

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