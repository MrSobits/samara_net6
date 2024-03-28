namespace Bars.GkhEdoInteg
{
    using B4;
    using B4.Application;
    using B4.Events;
    using B4.IoC;
    using B4.Modules.Quartz;
    using B4.Windsor;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhEdoInteg.Configuration;
    using Bars.GkhGji.DomainService;

    using Gkh;
    using Interceptors;
    using Controllers;
    using DomainService;
    using DomainService.Impl;
    using Entities;
    using Permissions;
    using Quartz;
    using GkhGji;
    using GkhGji.Entities;
    using GkhGji.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            EdoModuleConfiguration.Init(this.Container);
            
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhEdoInteg resources");
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhEdoInteg navigation");

            this.Container.RegisterController<EdoIntegrationController>();
            this.Container.RegisterAltDataController<RevenueSourceCompareEdo>();
            this.Container.RegisterAltDataController<RevenueFormCompareEdo>();
            this.Container.RegisterAltDataController<InspectorCompareEdo>();
            this.Container.RegisterAltDataController<KindStatementCompareEdo>();
            this.Container.RegisterController<LogRequestsController>();

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.Container.RegisterViewModel<InspectorCompareEdo, InspectorCompareEdoViewModel>();
            this.Container.RegisterViewModel<KindStatementCompareEdo, KindStatementCompareEdoViewModel>();
            this.Container.RegisterViewModel<RevenueFormCompareEdo, RevenueFormCompareEdoViewModel>();
            this.Container.RegisterViewModel<RevenueSourceCompareEdo, RevenueSourceCompareEdoViewModel>();
            this.Container.RegisterViewModel<AppealCitsCompareEdo, AppealCitsEdoIntegViewModel>();

            this.Container.RegisterSingleton<IPermissionSource, GkhEdoIntegPermissionMap>();

            this.Container.RegisterTransient<IAppealCitsEdoIntegService, AppealCitsEdoIntegService>();

            this.Container.RegisterTransient<IEmsedService, IntegrationEmsedService>("Edo integration");

            // Перекрываем ViewModel модуля ГЖИ
            this.Container.ReplaceComponent<IViewModel<AppealCits>>(
                typeof(AppealCitsViewModel),
                Component.For<IViewModel<AppealCits>>().ImplementedBy<AppealCitsEdoViewModel>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizens>, GkhGji.DomainService.AppealCitsService, Bars.GkhEdoInteg.DomainService.Impl.AppealCitsService>();

            this.Container.Register(
                Component.For<IViewCollection>().Named("GkhEdoIntegViewCollection").ImplementedBy<GkhEdoIntegViewCollection>().LifeStyle.Transient);

            // Регистрация класса для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.GkhEdoInteg dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.Container.RegisterDomainInterceptor<InspectorCompareEdo, InspectorCompareEdoInterceptor>();
            this.Container.RegisterDomainInterceptor<KindStatementCompareEdo, KindStatementCompareEdoInterceptor>();
            this.Container.RegisterDomainInterceptor<RevenueFormCompareEdo, RevenueFormCompareEdoInterceptor>();
            this.Container.RegisterDomainInterceptor<RevenueSourceCompareEdo, RevenueSourceCompareEdoInterceptor>();

            this.RegistrationQuartz();
        }

        private void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
            {
                this.Container.RegisterTransient<ITask, EdoLoadAppealCitsTask>();
                this.Container.RegisterTransient<ITask, DocEdoLoadAppealCitsTask>();

                ApplicationContext.Current.Events.GetEvent<AppStartEvent>().Subscribe<InitQuartz>();
            }
        }
    }
}