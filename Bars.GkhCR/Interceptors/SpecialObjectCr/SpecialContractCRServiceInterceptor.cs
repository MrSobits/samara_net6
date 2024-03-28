namespace Bars.GkhCr.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class SpecialContractCrServiceInterceptor : EmptyDomainInterceptor<SpecialContractCr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialContractCr> service, SpecialContractCr entity)
        {
            //Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }
    }
}