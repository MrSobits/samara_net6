namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class RealityObjectLoanInterceptor : EmptyDomainInterceptor<RealityObjectLoan>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectLoan> service, RealityObjectLoan entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (entity.State == null)
            {
                throw new ValidationException("Отсутствует начальный статус");
            }

            return Success();
        }
    }
}