namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBasePlanActionService
    {
        
        IDataResult GetContragentInfo(BaseParams baseParams);

        IDataResult GetStartFilters();
    }
}