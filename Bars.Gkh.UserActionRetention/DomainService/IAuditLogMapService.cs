using Bars.B4;

namespace Bars.Gkh.UserActionRetention.DomainService
{
    public interface IAuditLogMapService
    {
        IDataResult List(BaseParams baseParams);
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}
