namespace Bars.GkhCr.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Params;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.GkhCr.Regions.Tatarstan.Controllers;
    using Bars.GkhCr.Regions.Tatarstan.Controllers.Dict;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Regions.Tatarstan.DomainService.Impl;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;
    using Bars.GkhCr.Regions.Tatarstan.Export;
    using Bars.GkhCr.Regions.Tatarstan.Interceptors;
    using Bars.GkhCr.Regions.Tatarstan.Interceptors.Dict;
    using Bars.GkhCr.Regions.Tatarstan.Navigation;
    using Bars.GkhCr.Regions.Tatarstan.Permissions;
    using Bars.GkhCr.Regions.Tatarstan.ViewModel.ObjectOutdoorCr;
    using Bars.GkhCr.Regions.Tatarstan.ViewModel.RealityObjectOutdoorProgram;
    using Bars.GkhCr.Regions.Tatarstan.ViewModel;

    using Castle.MicroKernel.Registration;

    using NavigationProvider = Navigation.NavigationProvider;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.GkhCr.Regions.Tatarstan.Enum;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.RegisterCommonComponents();
            this.RegisterNavigations();
            this.RegisterControllers();
            this.RegisterServices();
            this.RegisterViewModels();
            this.RegisterInterceptors();
            this.RegisterEnums();

            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Bars.GkhCr.Regions.Tatarstan statefulentity");
        }

        private void RegisterEnums()
        {
            EnumRegistry.Add(new B4.Modules.Analytics.Reports.Params.Enum
            {
                Id = "DpkrProgramType",
                Display = "Вид программы",
                EnumJsClass = "B4.enums.DpkrProgramType"
            });
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhCrPermissionMap>());
            this.Container.Register(Component.For<IFieldRequirementSource>().ImplementedBy<GkhCrFieldRequirementMap>());
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, ObjectOutdoorCrMenuProvider>("ObjectOutdoorCr navigation");
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<RealityObjectOutdoorProgramController>();
            this.Container.RegisterAltDataController<RealityObjectOutdoorProgramChangeJournal>();
            this.Container.RegisterController<TatarstanCrMenuController>();
            this.Container.RegisterController<ObjectOutdoorCrController>();
            this.Container.RegisterAltDataController<TypeWorkRealityObjectOutdoor>();
            this.Container.RegisterController<TypeWorkRealityObjectOutdoorHistoryController>();
            this.Container.RegisterController<ElementOutdoorController>();
            this.Container.RegisterAltDataController<ElementOutdoor>();
            this.Container.RegisterAltDataController<WorksElementOutdoor>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IRealityObjectOutdoorProgramService, RealityObjectOutdoorProgramService>();
            this.Container.RegisterTransient<IObjectOutdoorCrService, ObjectOutdoorCrService>();
            this.Container.RegisterTransient<ITypeWorkRealityObjectOutdoorHistoryService, TypeWorkRealityObjectOutdoorHistoryService>();
            this.Container.RegisterTransient<IDataExportService, ObjectOutdoorCrDataExport>("ObjectOutdoorCrDataExport");
            this.Container.RegisterTransient<IElementOutdoorService, ElementOutdoorService>();
            this.Container.RegisterTransient<IOutdoorTypeWorkProvider, OutdoorTypeWorkProvider>();

            //DomainService
            this.Container.RegisterDomainService<RealityObjectOutdoorProgram, RealityObjectOutdoorProgramDomainService>();
            this.Container.RegisterDomainService<ObjectOutdoorCr, ObjectOutdoorCrDomainService>();
            this.Container.RegisterDomainService<TypeWorkRealityObjectOutdoor, TypeWorkRealityObjectOutdoorDomainService>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<RealityObjectOutdoorProgramChangeJournal, RealityObjectOutdoorProgramChangeJournalViewModel>();
            this.Container.RegisterViewModel<RealityObjectOutdoorProgram, RealityObjectOutdoorProgramViewModel>();
            this.Container.RegisterViewModel<ObjectOutdoorCr, ObjectOutdoorCrViewModel>();
            this.Container.RegisterViewModel<TypeWorkRealityObjectOutdoor, TypeWorkRealityObjectOutdoorViewModel>();
            this.Container.RegisterViewModel<TypeWorkRealityObjectOutdoorHistory, TypeWorkRealityObjectOutdoorHistoryViewModel>();
            this.Container.RegisterViewModel<ElementOutdoor, ElementOutdoorViewModel>();
            this.Container.RegisterViewModel<WorksElementOutdoor, WorksElementOutdoorViewModel>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<RealityObjectOutdoorProgram, RealityObjectOutdoorProgramInterceptor>();
            this.Container.RegisterDomainInterceptor<ObjectOutdoorCr, ObjectOutdoorCrInterceptor>();
            this.Container.RegisterDomainInterceptor<WorkRealityObjectOutdoor, WorkRoOutdoorInterceptor>();
            this.Container.RegisterDomainInterceptor<ElementOutdoor, ElementOutdoorInterceptor>();
        }
    }
}