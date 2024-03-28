namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using Entities;
    using Gkh.Entities;

    public interface IRealityObjectChargeAccountRepository
    {
        RealityObjectChargeAccountOperation GetOperationInOpenedPeriod(RealityObject realityObject);
    }
}