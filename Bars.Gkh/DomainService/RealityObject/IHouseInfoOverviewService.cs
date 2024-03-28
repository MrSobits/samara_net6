namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IHouseInfoOverviewService
    {
        IDataResult GetHouseInfoOverviewByRealityObjectId(BaseParams baseParams);
    }
}