namespace Bars.Gkh.Regions.Nso
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage.DomainService;
    
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.Regions.Nso.Controllers;
    using Bars.Gkh.Regions.Nso.Entities;
    using Bars.Gkh.Regions.Nso.Import;
    using Bars.Gkh.Regions.Nso.Interceptors;
    using Bars.Gkh.Regions.Nso.Interceptors.dict;
    using Bars.Gkh.Regions.Nso.Navigation;
    using Bars.Gkh.Regions.Nso.Report;
    using Bars.Gkh.Regions.Nso.ViewModel;

    using Castle.MicroKernel.Registration;
    using Utils;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterPermissionMap<PermissionMap>();
            Container.RegisterResources<ResourceManifest>();
            Container.RegisterNavigationProvider<NavigationProvider>();
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            Container.RegisterImport<ManOrgRobjectImport>();
            Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();
            
            Container.RegisterAltDataController<RealityObjectDocument>();
            Container.RegisterDomainService<RealityObjectDocument, FileStorageDomainService<RealityObjectDocument>>();
            Container.RegisterViewModel<RealityObjectDocument, RealityObjectDocumentViewModel>();

            Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>("Nso RealityObject navigation");

            Container.RegisterTransient<INavigationProvider, ManOrgMenuProvider>();

            // Регистрация класса для получения информации о зависимостях
            Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.Gkh.Regions.Nso dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(Container).Init()));

            Container.RegisterDomainInterceptor<ZonalInspection, ZonalInspectionInterceptor>();

            Container.ReplaceController<NsoLocalGovernmentController>("localgovernment");
            Container.RegisterViewModel<NsoLocalGovernment, NsoLocalGovernmentViewModel>();
            Container.RegisterDomainInterceptor<NsoLocalGovernment, NsoLocalGovServiceInterceptor>();

            Container.ReplaceComponent<IDomainService<LocalGovernment>>(
                typeof(Gkh.DomainService.LocalGovernmentDomainService),
                Component.For<IDomainService<LocalGovernment>>()
                         .ImplementedBy<Gkh.Regions.Nso.DomainService.LocalGovernmentDomainService>()
                         .LifeStyle.Transient);

            Container.Register(Component.For<IPrintForm>().ImplementedBy<LocalGovernmentReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().ImplementedBy<TsjProvidedInfoReport>().LifeStyle.Transient);
        }
    }
}
