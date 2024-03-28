namespace Bars.Gkh.Gku
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using B4.Modules.Reports;
    using DomainService;
    using DomainService.Impl;
    using Entities;
    using Reports;

    using Castle.MicroKernel.Registration;
    using Controllers;
    using ViewModel;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IModuleDependencies>()
                    .Named(string.Format("{0} dependencies", AssemblyName))
                    .LifeStyle.Singleton.UsingFactoryMethod(
                        () => new ModuleDependencies(Container).Init()));
            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGkuTatPermissionMap>());
            Container.Register(Component.For<IViewModel<GkuTariffGji>>().ImplementedBy<GkuTariffGjiViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IGkuTariffGjiService>().ImplementedBy<GkuTariffGjiService>().LifeStyle.Transient);
            Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhGku navigation");
            Container.RegisterTransient<INavigationProvider, GkuInfoMenuProvider>("GkuInfo navigation");
            Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGku resources");
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterController<GkuTariffGjiController>();

            Container.RegisterTransient<IPrintForm, OperationalDataOfPayments>("Report Bars.Gkh OperationalDataOfPayments");
        }
    }
}
