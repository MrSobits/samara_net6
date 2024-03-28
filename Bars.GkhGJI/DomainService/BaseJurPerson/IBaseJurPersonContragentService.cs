namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBaseJurPersonContragentService
    {
        IDataResult AddContragents(BaseParams baseParams);

        IDataResult List(BaseParams baseParams);
    }
}