namespace Bars.GkhRf.DomainService
{
    using B4;

    public interface ILimitCheckFinSourceService
    {
        IDataResult AddFinSources(BaseParams baseParams);
    }
}