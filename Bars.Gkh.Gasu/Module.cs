using Bars.Gkh.Gasu.ViewModel;
using Castle.MicroKernel.Registration;

namespace Bars.Gkh.Gasu
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Windsor;
    using Bars.Gkh.Gasu.Controller;
    using Bars.Gkh.Gasu.Entities;
    using Bars.Gkh.Gasu.Controllers;
    using Bars.Gkh.Gasu.DomainService;
    using Bars.Gkh.Gasu.Export;


    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterPermissionMap<PermissionMap>();
            Container.RegisterNavigationProvider<NavigationProvider>();
            Container.RegisterResourceManifest<ResourceManifest>("GkhGasu resources");

            Container.RegisterAltDataController<GasuIndicator>();
            Container.RegisterController<GasuIndicatorValueController>();
            Container.RegisterController<GasuImportExportController>();

            Container.RegisterTransient<IGasuIndicatorValueService, GasuIndicatorValueService>();
            Container.RegisterTransient<IGasuIndicatorService, GasuIndicatorService>();
            Container.RegisterTransient<IGasuImportExportService, GasuImportExportService>();

            Container.RegisterTransient<IDataExportService, OverhaulToGasuExport>("OverhaulToGasuExport");
            Container.RegisterTransient<IDataExportService, InspGasAgregations>("ManagementSysExport");
            Container.RegisterTransient<IDataExportService, GasuIndicatorExport>("GasuIndicatorExport");

            Container.Register(Component.For<IViewModel<GasuIndicatorValue>>().ImplementedBy<GasuIndicatorValueViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<GasuIndicator>>().ImplementedBy<GasuIndicatorViewModel>().LifeStyle.Transient);
        }

    }
}