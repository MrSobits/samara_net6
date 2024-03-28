namespace Bars.GkhCr.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class ContractCrServiceInterceptor : EmptyDomainInterceptor<ContractCr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ContractCr> service, ContractCr entity)
        {
            //Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }
    }
}