namespace Bars.Gkh.Repair
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.Pivot;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Repair.Controllers;
    using Bars.Gkh.Repair.DomainService;
    using Bars.Gkh.Repair.DomainService.Dict.Impl;
    using Bars.Gkh.Repair.DomainService.Impl;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Entities.RepairControlDate;
    using Bars.Gkh.Repair.Export;
    using Bars.Gkh.Repair.Interceptors;
    using Bars.Gkh.Repair.Navigation;
    using Bars.Gkh.Repair.Permissions;
    using Bars.Gkh.Repair.ViewModel;
    using Bars.GkhCr.Regions.Tatarstan.Report;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.RegisterAuditLogMap();
            this.RegisterBundlers();
            this.RegisterCommonComponents();
            this.RegisterControllers();
            this.RegisterDomainServices();
            this.RegisterInterceptors();
            this.RegisterNavigations();
            this.RegisterReports();
            this.RegisterServices();
            this.RegisterViewModels();
        }

        public void RegisterAuditLogMap()
        {
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhRepair statefulentity");

            this.Container.RegisterResourceManifest<ResourceManifest>();
            this.Container.RegisterPermissionMap<GkhRepairPermissionMap>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<RepairProgramController>();
            this.Container.RegisterController<RepairProgramMunicipalityController>();
            this.Container.RegisterController<RepairObjectController>();
            this.Container.RegisterAltDataController<RepairWork>();
            this.Container.RegisterController<MenuRepairObjectController>();
            this.Container.RegisterController<RepairControlDateController>();
            this.Container.RegisterController<FileStorageDataController<RepairObject>>();
            this.Container.RegisterController<FileStorageDataController<PerformedRepairWorkAct>>();
        }

        private void RegisterDomainServices()
        {
            this.Container.Register(Component.For<IRepairControlDateService>().ImplementedBy<RepairControlDateService>().LifeStyle.Transient);
            this.Container.RegisterDomainService<RepairObject, FileStorageDomainService<RepairObject>>();
            this.Container.RegisterDomainService<PerformedRepairWorkAct, FileStorageDomainService<PerformedRepairWorkAct>>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<RepairObject, RepairObjectServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<RepairControlDate, RepairControlDateInterceptor>();
            this.Container.RegisterDomainInterceptor<RepairWork, RepairWorkServiceInterceptor>();
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterNavigationProvider<NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, RepairObjectMenuProvider>("RepairObject navigation");
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IDataExportService, RepairObjectDataExport>("RepairObjectDataExport");
            this.Container.Register(
                Component.For<IPrintForm, IPivotModel>()
                    .Named("GkhRepair Report.MkdRepairInfoReport")
                    .ImplementedBy<MkdRepairInfoReport>()
                    .LifeStyle.Transient);
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IRepairProgramMunicipalityService, RepairProgramMunicipalityService>();
            this.Container.RegisterTransient<IRepairObjectService, RepairObjectService>();
            this.Container.RegisterTransient<IRepairProgramService, RepairProgramService>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<RepairProgram, RepairProgramViewModel>();
            this.Container.RegisterViewModel<RepairProgramMunicipality, RepairProgramMunicipalityViewModel>();
            this.Container.RegisterViewModel<RepairObject, RepairObjectViewModel>();
            this.Container.RegisterViewModel<RepairControlDate, RepairControlDateViewModel>();
            this.Container.RegisterViewModel<RepairWork, RepairWorkViewModel>();
            this.Container.RegisterViewModel<PerformedRepairWorkAct, PerformedRepairWorkActViewModel>();
        }
    }
}