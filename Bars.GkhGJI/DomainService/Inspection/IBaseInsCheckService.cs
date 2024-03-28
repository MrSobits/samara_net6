namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBaseInsCheckService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult GetStartFilters();
    }
}