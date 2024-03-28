namespace Bars.GkhCr.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class SpecialMonitoringSmrServiceInterceptor : EmptyDomainInterceptor<SpecialMonitoringSmr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialMonitoringSmr> service, SpecialMonitoringSmr entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }
    }
}