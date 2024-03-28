namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface IFinanceSourceWorkService
    {
        IDataResult AddWorks(BaseParams baseParams);

        IDataResult ListWorksByFinSource(BaseParams baseParams);
    }
}
