namespace Bars.B4.Modules.Analytics
{
    using Bars.B4;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Domain;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Executions;
    using Bars.B4.Modules.Analytics.Filters;
    using Bars.B4.Modules.Analytics.Interceptors;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IDataProviderService, DataProviderService>();
            Container.RegisterTransient<IDataProviderParamService, DataProviderParamService>();
            Container.RegisterTransient<IFilterExpressionService, FilterExpressionService>();

            Container.RegisterSingleton<IMacrosContainer, MacrosContainer>();
            Container
               .Resolve<IEventAggregator>()
               .GetEvent<AppStartEvent>()
               .Subscribe<MigrateDataProvidersHandler>();

            Container.RegisterPermission<Permissions.AnalyticsPermissionMap>();

            Container.RegisterDomainInterceptor<DataSource, DataSourceInterceptor>();
        }
    }
}
