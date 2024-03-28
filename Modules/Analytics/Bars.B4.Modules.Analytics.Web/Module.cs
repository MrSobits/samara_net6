namespace Bars.B4.Modules.Analytics.Web
{
    using Bars.B4.Alt;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Web.Controllers;
    using Bars.B4.Modules.Analytics.Web.ViewModels;
    using Bars.B4.Windsor;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // Controllers
            //Container.RegisterController<DataController<StoredFilter>>();
            Container.RegisterController<DataController<DataSource>>();
            Container.RegisterController<DataProviderController>();
            Container.RegisterController<DataProviderMetaController>();
            Container.RegisterController<FilterExpressionController>();
            Container.RegisterController<DataSourceTreeController>();
            Container.RegisterController<DataSourceParamController>();

            // Client routes
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<INavigationProvider, AnalyticsNavigationProvider>();

            // Resources
            Container.RegisterResourceManifest<ResourceManifest>();

            // ViewModels
            Container.RegisterViewModel<DataSource, DataSourceViewModel>();
            //Container.RegisterViewModel<StoredFilter, StoredFilterViewModel>();
        }
    }
}
