namespace Bars.GkhCr.Hmao.DomainService
{
    using Bars.B4;

    public interface IFinanceSourceResourceService
    {
        IDataResult AddFinSources(BaseParams baseParams);
    }
}