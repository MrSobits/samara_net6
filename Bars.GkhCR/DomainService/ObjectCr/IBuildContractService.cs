namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface IBuildContractService
    {
        IDataResult AddTypeWorks(BaseParams baseParams);
        IDataResult AddTermination(BaseParams baseParams);
        IDataResult ListAvailableStates(BaseParams baseParams);
        IDataResult ListAvailableBuilders(BaseParams baseParams);
        IDataResult GetForMap(BaseParams baseParams);
    }
}
