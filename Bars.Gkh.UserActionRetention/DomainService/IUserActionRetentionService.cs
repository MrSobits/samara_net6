namespace Bars.Gkh.UserActionRetention.DomainService
{
    using Bars.B4;

    public interface IUserActionRetentionService
    {
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}
