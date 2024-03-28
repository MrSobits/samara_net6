namespace Bars.GkhCr.DomainService
{
    using Bars.B4;

    public interface ISpecialFinanceSourceResourceService
    {
        IDataResult AddFinSources(BaseParams baseParams);
    }
}