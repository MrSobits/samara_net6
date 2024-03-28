namespace Bars.GkhCr.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface ITypeWorkRealityObjectOutdoorHistoryService
    {
        /// <summary>
        /// Восстанавливает записи из истории.
        /// </summary>
        IDataResult Recover(BaseParams baseParams);
    }
}
