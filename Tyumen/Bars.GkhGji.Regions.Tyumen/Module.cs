namespace Bars.GkhGji.Regions.Tyumen
{
    using B4.Modules.DataExport.Domain;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.NumberRule;
    using Bars.GkhGji.NumberValidation;
    using Bars.GkhGji.Regions.Tyumen.Controllers;
    using Bars.GkhGji.Regions.Tyumen.Domain;
    using Bars.GkhGji.Regions.Tyumen.DomainService.NetworkOperator;
    using Bars.GkhGji.Regions.Tyumen.DomainService.NetworkOperator.Impl;
    using DomainService;
    using Bars.GkhGji.Regions.Tyumen.Entities;
    using Bars.GkhGji.Regions.Tyumen.Entities.Dicts;
    using Bars.GkhGji.Regions.Tyumen.Entities.Suggestion;
    using Bars.GkhGji.Regions.Tyumen.Interceptors.NetworkOperatorRealityObject;
    using Bars.GkhGji.Regions.Tyumen.Interceptors.Suggestion;
    using Bars.GkhGji.Regions.Tyumen.NumberRule;
    using Bars.GkhGji.Regions.Tyumen.NumberValidation;
    using Bars.GkhGji.Regions.Tyumen.Permissions;
    using Bars.GkhGji.Regions.Tyumen.StateChange;
    using Bars.GkhGji.Regions.Tyumen.ViewModel;

    using Castle.MicroKernel.Registration;
    using DomainService.Suggestion;
    using Bars.GkhGji.Regions.Tyumen.Interceptors;
    using SMEV3Library.Services;
    using Bars.GkhGji.Regions.Tyumen.Tasks.EGRNSendInformationRequest;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>().Named("GkhGji.Regions.Tyumen resources").ImplementedBy<ResourceManifest>()
                    .LifeStyle.Transient);

            Container.ReplaceComponent<IDisposalText>(
                typeof(GkhGji.TextValues.DisposalText),
                Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

            Container.ReplaceComponent<IBaseStatementNumberRule>(
                typeof(BaseStatementNumberRuleTat),
                Component.For<IBaseStatementNumberRule>().ImplementedBy<BaseStatementNumberRuleTyumen>());

            Container.ReplaceComponent<IAppealCitsNumberRule>(
                typeof(AppealCitsNumberRuleTat),
                Component.For<IAppealCitsNumberRule>().ImplementedBy<AppealCitsNumberRuleTyumen>());

            Container.ReplaceComponent<IBaseStatementAction>(
                typeof(InspectionAction.BaseStatementAction),
                Component.For<IBaseStatementAction>().ImplementedBy<BaseStatementAction>());

            Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ActCheckValidationRule>().LifeStyle.Transient);
            Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<DisposalValidationRule>().LifeStyle.Transient);
            Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<PrescriptionValidationRule>().LifeStyle.Transient);
            Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ProtocolValidationRule>().LifeStyle.Transient);
            Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ResolutionValidationRule>().LifeStyle.Transient);
            Container.Register(Component.For<INumberValidationRule>().ImplementedBy<BaseTyumenValidationRule>().LifeStyle.Transient);
            Container.RegisterTransient<IStateChangeHandler, CitizenSuggestionStateChangeHandler>();

            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<INavigationProvider, NavigationProvider>("Gkh gji tyumen navigation");
            Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();
            Container.RegisterPermissionMap<TyumenPermissionMap>();

            Container.RegisterTransient<INetworOperatorRealityObjectService, NetworkOperatorRealityObjectService>();
            Container.RegisterTransient<IMenuModificator, NetworkOperatorMenuModificator>();

            this.Container.RegisterTransient<ISuggestionChangeHandler, SuggestionChangeHandler>();

            this.Container.RegisterTransient<IManOrgLicenseGisService, ManOrgLicenseGisService>();

            Container.Register(Component.For<IDomainService<LicensePrescription>>().ImplementedBy<FileStorageDomainService<LicensePrescription>>().LifeStyle.Transient);

            RegisterBundlers();
            RegisterViewModels();
            RegisterControllers();
            RegisterInterceptors();
            RegisterService();

        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<NetworkOperator, NetworkOperatorViewModel>();
            Container.RegisterViewModel<LicensePrescription, LicensePrescriptionViewModel>();
            Container.RegisterViewModel<NetworkOperatorRealityObject, NetworkOperatorRealityObjectViewModel>();
            Container.RegisterViewModel<LicenseNotification, LicenseNotificationViewModel>();

            Container.RegisterViewModel<RegionCodeMVD, RegionCodeMVDViewModel>();
            Container.RegisterViewModel<EGRNApplicantType, EGRNApplicantTypeViewModel>();
            Container.RegisterViewModel<EGRNObjectType, EGRNObjectTypeViewModel>();
            Container.RegisterViewModel<EGRNDocType, EGRNDocTypeViewModel>();

            Container.RegisterViewModel<SMEVEGRN, SMEVEGRNViewModel>();
            Container.RegisterViewModel<SMEVEGRNFile, SMEVEGRNFileViewModel>();
        }  

        private void RegisterService()
        {
            Container.RegisterTransient<ISMEVEGRNService, SMEVEGRNService>();
            Container.RegisterSingleton<ISMEV3Service, SMEV3Service12>();
            Container.RegisterTaskExecutor<SendEGRNRequestTaskExecutor>(SendEGRNRequestTaskExecutor.Id);
        }

        private void RegisterControllers()
        {
            //справочники 
            Container.RegisterAltDataController<RegionCodeMVD>();
            Container.RegisterAltDataController<EGRNApplicantType>();
            Container.RegisterAltDataController<EGRNObjectType>();
            Container.RegisterAltDataController<EGRNDocType>();

            Container.RegisterAltDataController<ApplicantNotification>();
            Container.RegisterAltDataController<NetworkOperator>();
            Container.RegisterAltDataController<TechDecision>();
            Container.RegisterAltDataController<LicenseNotification>();
            Container.RegisterAltDataController<LicensePrescription>();
            Container.RegisterController<NetworkOperatorRealityObjectController>();
            Container.RegisterAltDataController<NetworkOperatorRealityObjectTechDecision>();
            this.Container.RegisterController<ManOrgLicenseGisController>();

            Container.RegisterAltDataController<SMEVEGRN>();
            Container.RegisterAltDataController<SMEVEGRNFile>();
            Container.RegisterController<SMEVEGRNExecuteController>();
        }

        private void RegisterInterceptors()
        {
            Container.RegisterDomainInterceptor<SMEVEGRN, SMEVEGRNInterceptor>();
            Container.RegisterDomainInterceptor<CitizenSuggestion, CitizenSuggestionServiceInterceptor>();
            Container.RegisterDomainInterceptor<LicenseNotification, LicenseNotificationInterceptor>();
            this.Container.Register(Component.For<IDomainServiceInterceptor<Disposal>>().ImplementedBy<DisposalServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActCheck>>().ImplementedBy<ActCheckServiceInterceptor>().LifeStyle.Transient);
            Container.RegisterDomainInterceptor<SuggestionComment, SuggestionCommentInterceptor>();
            Container.RegisterDomainInterceptor<NetworkOperatorRealityObject, NetworkOperatorRealityObjectServiceInterceptor>();
        }
    }
}