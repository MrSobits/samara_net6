using Bars.B4;

namespace Bars.Gkh.UserActionRetention.DomainService
{
    public interface IUserLoginService
    {
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}
