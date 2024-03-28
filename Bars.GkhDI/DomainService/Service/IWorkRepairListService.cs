namespace Bars.GkhDi.DomainService
{
    using B4;

    public interface IWorkRepairListService
    {
        IDataResult ListSelected(BaseParams baseParams);

        IDataResult SaveWorkPpr(BaseParams baseParams);

        IDataResult HasDetailAllWorkRepair(BaseParams baseParams);
    }
}