namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using B4;

    public interface IDefaultPlanCollectionInfoService
    {
        IDataResult UpdatePeriod(BaseParams baseParams);

        IDataResult CopyCollectionInfo(BaseParams baseParams);
    }
}