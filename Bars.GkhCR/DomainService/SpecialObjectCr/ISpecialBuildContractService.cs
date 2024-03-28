namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface ISpecialBuildContractService
    {
        IDataResult AddTypeWorks(BaseParams baseParams);

        IDataResult ListAvailableBuilders(BaseParams baseParams);
    }
}
