namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using Entities;
    using Enums;

    public class SuspenseAccountInterceptor : EmptyDomainInterceptor<SuspenseAccount>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SuspenseAccount> service, SuspenseAccount entity)
        {
            entity.DistributeState = DistributionState.NotDistributed;

            return Success();
        }
    }
}