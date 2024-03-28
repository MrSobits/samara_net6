namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using Entities;

    public class CalcAccountOverdraftInterceptor : EmptyDomainInterceptor<CalcAccountOverdraft>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CalcAccountOverdraft> service, CalcAccountOverdraft entity)
        {
            entity.AvailableSum = entity.OverdraftLimit;

            return Success();
        }
    }
}
