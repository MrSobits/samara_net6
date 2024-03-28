namespace Bars.Gkh.Regions.Tyumen
{
    using B4;
    using B4.Application;
    using B4.Events;
    using B4.IoC;
    using B4.Modules.Quartz;
    using B4.Windsor;

    using Bars.Gkh.DomainService;
    using Bars.Gkh.Regions.Tyumen.DomainServices.Impl;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tyumen.Navigation;
    using Bars.Gkh.Services.Override;
    using Bars.Gkh.Utils;
    
    using Castle.MicroKernel.Registration;
    using Controllers;
    using DomainServices.Suggestions;
    using DomainServices.Suggestions.Impl;
    using Entities.Suggestion;
    using ExecutionAction;
    using Gkh.Services.Impl.Suggestion;
    using Gkh.Services.ServiceContracts.Suggestion;
    using Gkh.ViewModel;
    using Permissions;
    using Services.Impl.Suggestion;
    using Sheduler;
    using Sheduler.Tasks;
    using TextValues;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Regions.Tyumen.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tyumen.Interceptors;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterPermissionMap<PermissionMap>();

            this.Container.Register(
                Component.For<IResourceManifest>()
                    .Named("Gkh.Regions.Tyumen resources")
                    .ImplementedBy<ResourceManifest>()
                    .LifeStyle.Transient);

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("Gkh Tyumen navigation");
            this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();

            //Интерцепторы
            Container.RegisterDomainInterceptor<RealityObject, RealityObjectInterceptor>();

            this.RegisterServices();

            this.Container.ReplaceComponent<ISuggestionService>(
                typeof(SuggestionService),
                Component.For<ISuggestionService>().ImplementedBy<SuggestionServiceTyumen>());
                // TODO: Wcf
                /*.AsWcfService(
                    new DefaultServiceModel(
                        WcfEndpoint
                            .BoundTo(
                                new BasicHttpBinding
                                {
                                    MaxReceivedMessageSize = int.MaxValue,
                                    CloseTimeout = TimeSpan.FromMinutes(30),
                                    OpenTimeout = TimeSpan.FromMinutes(30),
                                    ReceiveTimeout = TimeSpan.FromMinutes(30),
                                    SendTimeout = TimeSpan.FromMinutes(30)
                                })
                        )
                        .Hosted()));*/

            this.Container.ReplaceComponent<IViewModel<SuggestionComment>>(
                typeof (SuggestionCommentViewModel),
                Component.For<IViewModel<SuggestionComment>>().ImplementedBy<ViewModel.SuggestionCommentViewModel>());

            this.Container.ReplaceComponent<IViewModel<CitizenSuggestion>>(
                typeof(CitizenSuggestionViewModel),
                Component.For<IViewModel<CitizenSuggestion>>().ImplementedBy<ViewModel.CitizenSuggestionViewModel>());

            this.Container.ReplaceTransient<IServiceOverride,
                Bars.Gkh.Services.Override.ServiceOverride,
                Bars.Gkh.Regions.Tyumen.Services.Override.ServiceOverride>();

            this.Container.ReplaceController<SuggestionCommentController>("suggestioncomment");

            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhPermissionMap>());

            this.Container.RegisterExecutionAction<MigrationCitizenSuggestionAction>();
            this.Container.RegisterExecutionAction<GkhTyumenFiasAddressCorrectAction>();
            this.Container.RegisterExecutionAction<GkhTyumenSetHouseGuidFromFiasHouseAction>();

            this.Container.UsingForResolved<IMenuItemText>((container, menuItemText) =>
            {
                menuItemText.Override("Обращения граждан", "Сообщения граждан");
                menuItemText.Override("Обращения", "Сообщения граждан");
            });

            this.RegistrationQuartz();

            this.Container.RegisterImport<Import.RealityObjectExaminationImport.RoImport>();
        }

        public void RegisterServices()
        {
            this.Container.RegisterTransient<IExpiredSuggestionWithTermCloser, ExpiredSuggestionWithTermCloser>();
            this.Container.RegisterTransient<ISuggestionCommentService, SuggestionCommentService>();
            this.Container.ReplaceComponent(Component.For<IProgramCRImportRealityObject>().ImplementedBy<ProgramCRImportRealityObjectService>().LifestyleTransient());
            this.Container.RegisterTransient<IImportAddressMatchService, TymenImportAddressMatchService>();
        }

        public void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
            {
                this.Container.RegisterTransient<ITask, CloseExpireSuggestionWithTermTask>();

                ApplicationContext.Current.Events.GetEvent<AppStartEvent>().Subscribe<InitSheduler>();
            }
        }
    }
}