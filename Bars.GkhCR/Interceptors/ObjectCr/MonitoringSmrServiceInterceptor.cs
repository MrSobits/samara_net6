namespace Bars.GkhCr.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class MonitoringSmrServiceInterceptor : EmptyDomainInterceptor<MonitoringSmr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MonitoringSmr> service, MonitoringSmr entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }
    }
}