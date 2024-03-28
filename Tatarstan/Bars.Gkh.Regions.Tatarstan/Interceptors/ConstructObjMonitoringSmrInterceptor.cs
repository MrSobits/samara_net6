namespace Bars.Gkh.Regions.Tatarstan.Interceptors
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructObjMonitoringSmrInterceptor : EmptyDomainInterceptor<ConstructObjMonitoringSmr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ConstructObjMonitoringSmr> service, ConstructObjMonitoringSmr entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            try
            {
                stateProvider.SetDefaultState(entity);
            }
            finally
            {
                this.Container.Release(stateProvider);
            }

            return this.Success();
        }
    }
}