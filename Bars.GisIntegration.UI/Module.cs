namespace Bars.GisIntegration.UI
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Base.Entities.Delegacy;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using Bars.GisIntegration.UI.Controllers;
    using Bars.GisIntegration.UI.Service;
    using Bars.GisIntegration.UI.Service.Impl;
    using ViewModel;
    using ViewModel.Delegacy;
    using ViewModel.Dictionary;
    using ViewModel.GisRole;
    using ViewModel.Package;
    using ViewModel.Protocol;
    using ViewModel.Result;
    using ViewModel.Task;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<RisPermissionMap>());

            this.Container.RegisterSingletonService<ExporterWizardRefs>();

            this.RegisterControllers();

            this.RegisterServices();

            this.RegisterDictionaries();

            this.RegisterDataExtractors();

            this.RegisterDataSelectors();

            this.RegisterViewModels();

            this.RegisterBundlers();
        }

        public void RegisterViewModels()
        {
            this.Container.RegisterTransient<ITreeViewModel, TaskTreeViewModel>("TaskTreeViewModel");
            this.Container.RegisterTransient<ITriggerProtocolViewModel, TriggerProtocolViewModel>();
            this.Container.RegisterTransient<IObjectProcessingResultViewModel, ObjectProcessingResultViewModel>();
            this.Container.RegisterTransient<IDictionaryViewModel, DictionaryViewModel>();
            this.Container.RegisterTransient<IPackageViewModel, PackageViewModel>();
            this.Container.RegisterViewModel<Delegacy, DelegacyViewModel>();
            this.Container.RegisterViewModel<GisRoleMethod, GisRoleMethodViewModel>();
            this.Container.RegisterViewModel<RisContragentRole, RisContragentRoleViewModel>();
            this.Container.RegisterViewModel<GisOperator, GisOperatorViewModel>();
        }

        public void RegisterControllers()
        {
            this.Container.RegisterController<InspectionServiceController>();
            this.Container.RegisterController<OrgRegistryController>();
            this.Container.RegisterController<CapitalRepairController>();
            this.Container.RegisterController<ServicesController>();
            this.Container.RegisterController<InfrastructureController>();
            this.Container.RegisterController<PaymentServiceController>();
            this.Container.RegisterController<BillsController>();
            this.Container.RegisterController<HouseManagementController>();
            this.Container.RegisterController<NsiController>();
            this.Container.RegisterController<GisIntegrationController>();
            this.Container.RegisterController<TaskTreeController>();
            this.Container.RegisterController<DictionaryController>();
            this.Container.RegisterController<PackageController>();
            this.Container.RegisterController<DelegacyController>();
            this.Container.RegisterController<GisRoleMethodController>();
            this.Container.RegisterController<RisContragentRoleController>();
        }

        public void RegisterServices()
        {
            this.Container.RegisterTransient<IGisIntegrationService, GisIntegrationService>();
            this.Container.RegisterTransient<ITaskTreeService, TaskTreeService>();
            this.Container.RegisterTransient<IPackageService, PackageService>();
            this.Container.RegisterTransient<IDictionaryService, DictionaryService>();
            this.Container.RegisterTransient<IDelegacyService, DelegacyService>();
            this.Container.RegisterTransient<IGisRoleMethodService, GisRoleMethodService>();
            this.Container.RegisterTransient<IRisContragentRoleService, RisContragentRoleService>();
        }

        public void RegisterDictionaries()
        {
        }

        public void RegisterDataSelectors()
        {
        }

        public void RegisterDataExtractors()
        {
        }
    }
}