namespace Bars.GkhDi.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Windsor;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.FormatDataExport.ProxySelectors.Impl;
    using Bars.GkhDi.GroupAction;
    using Bars.GkhDi.PercentCalculationProvider;
    using Bars.GkhDi.Regions.Tatarstan.Controllers;
    using Bars.GkhDi.Regions.Tatarstan.DomainService;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhDi.Regions.Tatarstan.ExecutionAction;
    using Bars.GkhDi.Regions.Tatarstan.GroupAction;
    using Bars.GkhDi.Regions.Tatarstan.Interceptors;
    using Bars.GkhDi.Regions.Tatarstan.PercentCalculationProvider;
    using Bars.GkhDi.Regions.Tatarstan.Permissions;
    using Bars.GkhDi.Regions.Tatarstan.Report;
    using Bars.GkhDi.Regions.Tatarstan.Services;
    using Bars.GkhDi.Regions.Tatarstan.ViewModels;
    using Bars.GkhDi.Services.Domain;

    using Castle.MicroKernel.Registration;
    using DomainService.Impl;
    using GkhDi.DomainService.Impl;

    using PercentCalculation = Bars.GkhDi.PercentCalculationProvider.PercentCalculation;
    using PercentCalculation988 = Bars.GkhDi.PercentCalculationProvider.PercentCalculation988;
    using Service = Bars.GkhDi.Regions.Tatarstan.Services.Service;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterPermissionMap<GkhDiRegionsTatarstanPermissionMap>();

            this.Container.Register(Component.For<INavigationProvider>().Named("GkhDi.Regions.Tatarstan navigation").ImplementedBy<NavigationProvider>().LifeStyle.Transient);

            Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient().RegisterIn(this.Container);

            this.Container.Register(
                Component.For<IResourceManifest>().Named("GkhDi.Regions.Tatarstan resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

            this.Container.ReplaceComponent<IPercentCalculation>(
                typeof(PercentCalculation),
                Component.For<IPercentCalculation>().ImplementedBy<PercentCalculationProvider.PercentCalculation>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IPercentCalculation>(
                typeof(PercentCalculation988),
                Component.For<IPercentCalculation>().ImplementedBy<PercentCalculationProvider.PercentCalculation988>().LifeStyle.Transient);

            this.Container.RegisterTransient<IPercentCalculation, PercentCalculationGji988>("PercentCalculationGji988");

            this.Container.RegisterTransient<IPrintForm, PercentCalculationTatReport>("DI Report.PercentCalculationTat");

            this.Container.ReplaceComponent<IWorkRepairDetailService>(
                typeof(WorkRepairDetailService),
                Component.For<IWorkRepairDetailService>().ImplementedBy<WorkRepairDetailTatService>());

            this.Container.ReplaceComponent<IServService>(
                typeof(ServService),
                Component.For<IServService>().ImplementedBy<ServTatService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IWorkRepairListService>(
                typeof(WorkRepairListService),
                Component.For<IWorkRepairListService>().ImplementedBy<WorkRepairListTatService>());

            this.Container.RegisterTransient<IDomainServiceInterceptor<WorkRepairListTat>, WorkRepairListTatInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RepairService>, RepairServiceTatInterceptor>();

            this.Container.RegisterAltDataController<WorkRepairListTat>();
            this.Container.RegisterAltDataController<WorkRepairDetailTat>();

            this.Container.RegisterViewModel<WorkRepairListTat, WorkRepairListTatViewModel>();
            this.Container.RegisterViewModel<WorkRepairDetailTat, WorkRepairDetailTatViewModel>();

            this.Container.ReplaceComponent<IViewModel<PlanWorkServiceRepair>>(
                typeof(GkhDi.DomainService.PlanWorkServiceRepairViewModel),
                Component.For<IViewModel<PlanWorkServiceRepair>>().ImplementedBy<ViewModels.PlanWorkServiceRepairViewModel>());

            this.Container.ReplaceComponent(
                typeof(IServiceDi),
                typeof(ServiceDi),
                Component.For<IServiceDi>().ImplementedBy<Services.Domain.ServiceDi>());

            this.RegisterExecutionActions();

            #region Веб-сервисы
            this.Container.RegisterTransient<IService, Service>();


            //Container.ReplaceComponent(typeof(GkhDi.Services.IService), typeof(GkhDi.Services.Service), Component.For<GkhDi.Services.IService>().ImplementedBy<Service>().LifestyleTransient());

            #endregion

            this.Container.RegisterAltDataController<MeasuresReduceCosts>();

            this.Container.ReplaceComponent<IViewModel<PlanReductionExpenseWorks>>(
                typeof(GkhDi.DomainService.PlanRedExpWorksViewModel),
                Component.For<IViewModel<PlanReductionExpenseWorks>>().ImplementedBy<ViewModels.PlanRedExpWorksViewModel>());

            this.Container.RegisterTransient<IPlanReduceMeasureNameService, PlanReduceMeasureNameService>();
            this.Container.RegisterController<PlanReduceMeasureNameController>();
            this.Container.RegisterDomainInterceptor<PlanReductionExpenseWorks, PlanReductionExpenseWorksInterceptor>();

            this.Container.RegisterDomainInterceptor<InfoAboutUseCommonFacilities, InfoAboutUseCommonFacilitiesInterceptor>();
            this.Container.ReplaceComponent<IDomainServiceInterceptor<NonResidentialPlacement>, GkhDi.Interceptors.NonResidentialPlacementInterceptor>(
                 Component.For<IDomainServiceInterceptor<NonResidentialPlacement>>()
                     .ImplementedBy<NonResidentialPlacementInterceptor>()
                     .LifestyleTransient());

            this.Container.ReplaceComponent(
                typeof (IServicePprService),
                typeof (ServicePprService),
                Component.For<IServicePprService>().ImplementedBy<ServicePprServiceTat>().LifestyleTransient());
            
            this.Container.Register(Component.For<IDiGroupAction>().ImplementedBy<CopyUninhabitablePremises>());
            this.Container.Register(Component.For<IDiGroupAction>().ImplementedBy<CopyCommonAreas>());

            this.RegisterFormatDataExport();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<RepairPlanCreationAction>();
            this.Container.RegisterExecutionAction<ChangeWorksToGroupAction>();
            this.Container.RegisterExecutionAction<DeleteDuplicateDiRealObjPercentAction>();
            this.Container.RegisterExecutionAction<UpdateRepairServiceToBeProvidedByManOrgAction>();
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.ReplaceProxySelectorService<UoProxy,
                Gkh.FormatDataExport.ProxySelectors.Impl.UoSelectorService,
                UoSelectorService>(this.Container);
        }
    }
}