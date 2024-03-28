namespace Bars.GkhGji.Regions.Chelyabinsk
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using ViewModel;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Windsor;
    using Bars.Gkh.Report;
    using Bars.B4.Modules.ExtJs;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Entities;
    using Controllers;
    using DomainService;
    using Permissions;
    using InspectionRules;
    using Interceptors;
    using Report;
    using Bars.GkhGji.DomainService.Impl;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Chelyabinsk.Controllers;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Interceptors;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl.Intfs;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Regions.Chelyabinsk.StateChange;
    using Bars.GkhGji.Regions.Chelyabinsk.ViewModel.AppealCits;
    using Bars.GkhGji.Regions.Chelyabinsk.ViewModel;

    using Castle.MicroKernel.Registration;
    using B4.Modules.DataExport.Domain;
    using DataExport;
    using GkhGji.InspectionRules;
    using InspectionMenuService = Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl.Inspection.InspectionMenuService;
    using GkhGji.Enums;

    using AppealCitsExecutantViewModel = Bars.GkhGji.Regions.Chelyabinsk.ViewModel.AppealCits.AppealCitsExecutantViewModel;
    using BaseDisposalInsFoundationCheckService = GkhGji.DomainService.Impl.DisposalInsFoundationCheckService;
    using BaseAppealCitsService = BaseChelyabinsk.DomainService.Impl.AppealCitsService;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
          //  this.Container.RegisterResources<ResourceManifest>();
            Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGji.Regions.Chelyabinsk resources");
            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Chelyabinsk statefulentity");

          //  this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            this.Container.Register(Component.For<INavigationProvider>().Named("GkhGJI.Regions.Chelyabinsk navigation").ImplementedBy<NavigationProvider>()
                   .LifeStyle.Transient);

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IPermissionSource, PermissionMap>();

            Container.RegisterTransient<IPermissionSource, GkhGjiChelyabinskPermissionMap>();
            this.Container.RegisterTransient<IFieldRequirementSource, GjiChFieldRequirementMap>();

            this.Container.Register(
                Component.For<IModuleDependencies>()
                    .Named("Bars.GkhGji.Regions.BaseChelyabinsk dependencies")
                    .LifeStyle.Singleton
                    .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.RegisterBundlers();

            this.RegisterControllers();

            this.RegisterViewModels();

            this.RegisterDomainServices();

            this.RegisterInspectionRules();

            this.RegisterInterceptors();

            this.RegisterExports();

            ReplaceComponents();

            // TODO : WCF
            Component.For<ICitizensAppealService>()
                .ImplementedBy<CitizensAppealService>()
                //.AsWcfSecurityService()
                .RegisterIn(this.Container);

            Component.For(typeof(IValidator<>))
                .ImplementedBy(typeof(Validator<>))
                .RegisterIn(this.Container);

            this.Container.RegisterController<AppealCitsTransferResultController>();

            this.Container.RegisterTransient<IRuleChangeStatus, AcceptWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, NotAcceptWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SuccessCompletionOfWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, FailureCompletionOfWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SuccessCitizensAppealCancelRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, FailureCitizensAppealCancelRule>();
            this.Container.RegisterTransient<IStateChangeHandler, EndWorkHandler>();

            this.Container.RegisterDomainInterceptor<AppealCitsAnswerAttachment, AppCitsAnsAttachmentInterceptor>();
        }

        private void ReplaceComponents()
        {
           
            this.Container.ReplaceComponent<IInspectionMenuService>(
                typeof(GkhGji.DomainService.InspectionMenuService),
                Component.For<IInspectionMenuService>().ImplementedBy<InspectionMenuService>().LifeStyle.Transient);
        }

        private void RegisterExports()
        {
           Container.Register(Component.For<IDataExportService>().Named("BaseOmsuDataExport").ImplementedBy<BaseOmsuDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("RiskOrientedMethodDataExport").ImplementedBy<RiskOrientedMethodDataExport>().LifeStyle.Transient);
        }

        private void RegisterControllers()
        {
            //справочники 
            Container.RegisterAltDataController<RegionCodeMVD>();
            Container.RegisterAltDataController<FLDocType>();
            Container.RegisterAltDataController<EGRNApplicantType>();
            Container.RegisterAltDataController<EGRNDocType>();
            Container.RegisterAltDataController<EffectiveKNDIndex>();


            //СМЭВ
            Container.RegisterAltDataController<SMEVMVD>();
            Container.RegisterAltDataController<SMEVMVDFile>();
            Container.RegisterController<SMEVMVDExecuteController>();
            Container.RegisterAltDataController<SMEVEGRUL>();
            Container.RegisterAltDataController<SMEVEGRULFile>();
            Container.RegisterController<SMEVEGRULExecuteController>();
            Container.RegisterAltDataController<SMEVEGRIP>();
            Container.RegisterAltDataController<SMEVEGRIPFile>();
            Container.RegisterController<SMEVEGRIPExecuteController>();
            Container.RegisterController<GISGMPExecuteController>();
            Container.RegisterAltDataController<GisGmp>();
            Container.RegisterAltDataController<GisGmpFile>();
            Container.RegisterAltDataController<SMEVEGRN>();
            Container.RegisterAltDataController<SMEVEGRNFile>();
            Container.RegisterController<SMEVEGRNExecuteController>();

            //предостережения ГЖИ
            Container.RegisterAltDataController<AppealCitsAdmonition>();
            Container.RegisterController<FileStorageDataController<AppealCitsAdmonition>>();
            Container.RegisterAltDataController<AppCitAdmonVoilation>();
            Container.RegisterAltDataController<BaseOMSU>();

            //Риск-ориентированный подход
            Container.RegisterAltDataController<KindKNDDict>();
            Container.RegisterAltDataController<KindKNDDictArtLaw>();
            Container.RegisterAltDataController<ROMCategory>();
            Container.RegisterAltDataController<ROMCategoryMKD>();
            Container.RegisterAltDataController<VnResolution>();
            Container.RegisterAltDataController<VpResolution>();
            Container.RegisterAltDataController<VprResolution>();
            Container.RegisterAltDataController<ROMCalcTask>();
            Container.RegisterController<ROMCalcTaskManOrgController>();
            Container.RegisterController<ROMCalcTaskExecuteController>();
            Container.RegisterController<ROMCalculateController>();
            Container.RegisterController<ManOrgLicenseGisController>();

            //Переоформление лицензии
            Container.RegisterAltDataController<LicenseReissuance>();
            Container.RegisterAltDataController<LicenseReissuanceProvDoc>();
            Container.RegisterController<LicenseReissuancePersonController>();
            //   Container.RegisterAltDataController<BaseLicenseReissuance>();

            //Статья закона в постановлении
            Container.RegisterAltDataController<ResolutionArtLaw>();
            Container.RegisterAltDataController<ResolutionFiz>();
            this.Container.RegisterController<ResolutionArticleLawController>();
            // отчеты
            Container.RegisterTransient<IGkhBaseReport, AppealCitsAdmonitionReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVEGRULReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVEGRIPReport>();
            Container.RegisterTransient<IPrintForm, SMEVReport1>();
            Container.RegisterTransient<IPrintForm, SMEVReport2>();
        }

        private void RegisterViewModels()
        {
            ContainerHelper.ReplaceViewModel
                <DisposalInspFoundCheckNormDocItem, Bars.GkhGji.ViewModel.DisposalInspFoundCheckNormDocItemViewModel,
                    DisposalInspFoundCheckNormDocItemViewModel>();

            ContainerHelper.ReplaceViewModel
                <AppealCitsExecutant, Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits.AppealCitsExecutantViewModel,
                    AppealCitsExecutantViewModel>();
            //Справочники
            Container.RegisterViewModel<RegionCodeMVD, RegionCodeMVDViewModel>();
            Container.RegisterViewModel<FLDocType, FLDocTypeViewModel>();
            Container.RegisterViewModel<EGRNApplicantType, EGRNApplicantTypeViewModel>();
            Container.RegisterViewModel<EGRNDocType, EGRNDocTypeViewModel>();
            Container.RegisterViewModel<EffectiveKNDIndex, EffectiveKNDIndexViewModel>();

            //СМЭВ
            Container.RegisterViewModel<SMEVMVD, SMEVMVDViewModel>();
            Container.RegisterViewModel<SMEVMVDFile, SMEVMVDFileViewModel>();
            Container.RegisterViewModel<SMEVEGRUL, SMEVEGRULViewModel>();
            Container.RegisterViewModel<SMEVEGRULFile, SMEVEGRULFileViewModel>();
            Container.RegisterViewModel<SMEVEGRIP, SMEVEGRIPViewModel>();
            Container.RegisterViewModel<SMEVEGRIPFile, SMEVEGRIPFileViewModel>();
            Container.RegisterViewModel<GisGmp, GisGmpViewModel>();
            Container.RegisterViewModel<GisGmpFile, GisGmpFileViewModel>();
            Container.RegisterViewModel<SMEVEGRN, SMEVEGRNViewModel>();
            Container.RegisterViewModel<SMEVEGRNFile, SMEVEGRNFileViewModel>();

            //предостережения
            Container.Register(Component.For<IViewModel<AppealCitsAdmonition>>().ImplementedBy<AppealCitsAdmonitionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppCitAdmonVoilation>>().ImplementedBy<AppCitAdmonVoilationViewModel>().LifeStyle.Transient);

            //Статья закона в постановлении
            Container.Register(Component.For<IViewModel<ResolutionArtLaw>>().ImplementedBy<ResolutionArtLawViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ResolutionFiz>>().ImplementedBy<ResolutionFizViewModel>().LifeStyle.Transient);

            //Риск-ориентированный подход
            Container.Register(Component.For<IViewModel<KindKNDDict>>().ImplementedBy<KindKNDDictViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<KindKNDDictArtLaw>>().ImplementedBy<KindKNDDictArtLawViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCategory>>().ImplementedBy<ROMCategoryViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCategoryMKD>>().ImplementedBy<ROMCategoryMKDViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VnResolution>>().ImplementedBy<VnResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VpResolution>>().ImplementedBy<VpResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VprResolution>>().ImplementedBy<VprResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCalcTask>>().ImplementedBy<ROMCalcTaskViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCalcTaskManOrg>>().ImplementedBy<ROMCalcTaskManOrgViewModel>().LifeStyle.Transient);

            //обращение за переоформлением лицензии
            this.Container.RegisterViewModel<LicenseReissuance, LicenseReissuanceViewModel>();
            this.Container.RegisterViewModel<LicenseReissuancePerson, LicenseReissuancePersonViewModel>();
            this.Container.RegisterViewModel<LicenseReissuanceProvDoc, LicenseReissuanceProvDocViewModel>();
            // Container.RegisterViewModel<BaseLicenseReissuance, BaseLicenseReissuanceViewModel>();


            Container.RegisterViewModel<BaseOMSU, BaseOMSUViewModel>();

            this.Container.RegisterViewModel<AppealCitsTransferResult, AppealCitsTransferResultViewModel>();

        }

        private void RegisterDomainServices()
        {
            this.Container.ReplaceTransient<IDisposalInsFoundationCheckService, Bars.GkhGji.DomainService.Impl.DisposalInsFoundationCheckService,
                    Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl.DisposalInsFoundationCheckService>();
            // предостережения ГЖИ
            Container.Register(Component.For<IDomainService<AppealCitsAdmonition>>().ImplementedBy<FileStorageDomainService<AppealCitsAdmonition>>().LifeStyle.Transient);
            //Статья закона в постановлении
            this.Container.Register(Component.For<IResolutionArticleLawService>().ImplementedBy<ResolutionArticleLawService>().LifeStyle.Transient);
            //риск-ориентированный подход
            Container.Register(Component.For<IROMCalcTaskManOrgService>().ImplementedBy<ROMCalcTaskManOrgService>().LifeStyle.Transient);
            Container.Register(Component.For<IKindKNDDictArtLawService>().ImplementedBy<KindKNDDictArtLawService>().LifeStyle.Transient);
            Container.RegisterTransient<IManOrgLicenseGisService, ManOrgLicenseGisService>();
            Container.RegisterTransient<ILicenseReissuancePersonService, LicenseReissuancePersonService>();
            Container.Register(Component.For<IDomainService<LicenseReissuanceProvDoc>>().ImplementedBy<FileStorageDomainService<LicenseReissuanceProvDoc>>().LifeStyle.Transient);

        }

        private void RegisterInterceptors()
        {
           Container.Register(Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<ResolutionServiceInterceptor>().LifeStyle.Transient);
            Container.RegisterDomainInterceptor<ROMCalcTask, ROMCalcTaskInterceptor>();
            Container.RegisterDomainInterceptor<ROMCategory, ROMCategoryInterceptor>();
            Container.RegisterDomainInterceptor<BaseOMSU, BaseOMSUServiceInterceptor>();
            Container.RegisterDomainInterceptor<SMEVMVD, SMEVMVDInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRUL, SMEVEGRULInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRN, SMEVEGRNInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRIP, SMEVEGRIPInterceptor>();
            Container.RegisterDomainInterceptor<GisGmp, GisGmpInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionFiz, ResolutionFizInterceptor>();
            Container.RegisterDomainInterceptor<LicenseReissuance, LicenseReissuanceInterceptor>();

        }

        private void RegisterInspectionRules()
        {
            // регистрируем провайдер для правил
           this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseOMSUToDisposalRule>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizensBaseChelyabinsk>, BaseAppealCitsService, DomainService.Impl.AppealCitsService>();

            this.Container.RegisterTransient<ICitizensAppealServiceClient, CitizensAppealServiceClient>();
        }
    }
}