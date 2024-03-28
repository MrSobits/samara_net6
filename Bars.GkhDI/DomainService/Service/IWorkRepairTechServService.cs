namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    public interface IWorkRepairTechServService
    {
        IDataResult AddWorks(BaseParams baseParams);

        IDataResult SaveRepairService(BaseParams baseParams);

        IDataResult ListTree(BaseParams baseParams);

        IDataResult ListSelected(BaseParams baseParams);
    }
}