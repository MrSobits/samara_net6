namespace Bars.GkhGji.Regions.Chelyabinsk
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
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.DomainService.GisGkhRegional;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules;
    using Bars.GkhGji.Regions.Chelyabinsk.Controllers.MKDLicRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService.MKDLicRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction;
    using Bars.GkhGji.Regions.Chelyabinsk.Export;
    using Bars.GkhGji.Regions.Chelyabinsk.InspectionRules.Impl;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl.Intfs;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Regions.Chelyabinsk.StateChange;
    using Bars.GkhGji.Regions.Chelyabinsk.StateChanges;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.CreateResolutionPayFines;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRIPSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRNSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRULSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.ERKNMDictRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.ERULSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.MVDSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.SendPaymentRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.SendPayRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.ViewModel.AppealCits;
    using Bars.GkhGji.Regions.Chelyabinsk.ViewModel.MKDLicRequest;
    using Castle.MicroKernel.Registration;
    using Controllers;
    using DataExport;
    using DomainService;
    using Entities;
    using GkhGji.InspectionRules;
    using InspectionRules;
    using Interceptors;
    using Permissions;
    using Report;
    using SMEV3Library.Services;
    using ViewModel;
    using AppealCitsExecutantViewModel = ViewModel.AppealCits.AppealCitsExecutantViewModel;
    using BaseAppealCitsService = BaseChelyabinsk.DomainService.Impl.AppealCitsService;
    using InspectionMenuService = DomainService.Impl.Inspection.InspectionMenuService;

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

            this.RegisterServices();

            RegisterActions();

            RegisterTasks();

            ReplaceComponents();

            this.Container.RegisterController<AppealCitsTransferResultController>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseTerminationChangeStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseApprovalChangeStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AcceptWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, NotAcceptWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SuccessCompletionOfWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, FailureCompletionOfWorkRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SuccessCitizensAppealCancelRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, FailureCitizensAppealCancelRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AppealSOPRStateRule>();
            this.Container.RegisterTransient<IStateChangeHandler, EndWorkHandler>();

            this.Container.RegisterDomainInterceptor<AppealCitsAnswerAttachment, AppCitsAnsAttachmentInterceptor>();

            Container.RegisterTransient<IGISGMPService, GISGMPService>();
            Container.RegisterTransient<IPAYREGService, PAYREGService>();
            Container.RegisterTransient<IGISERPService, GISERP502Service>();
            Container.RegisterTransient<ISMEVEGRULService, SMEVEGRULService>();
            //Container.RegisterTransient<ISMEVERULService, SMEVERULService>();
            Container.RegisterTransient<ISMEVERULService, SMEVERUL105Service>();
            Container.RegisterTransient<ISMEVEGRIPService, SMEVEGRIPService>();
            Container.RegisterTransient<ISMEVEGRNService, SMEVEGRNService>();
            Container.RegisterTransient<ISMEVMVDService, SMEVMVDService>();
            Container.RegisterSingleton<ISMEV3Service, SMEV3Service12>();

        }

        private void RegisterTasks()
        {
            Container.RegisterTaskExecutor<SendCalcRequestTaskExecutor>(SendCalcRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<ERKNMSendInitiateRequestTaskExecutor>(ERKNMSendInitiateRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendInitiateRequestTaskExecutor>(SendInitiateRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<CheckERPNeedCorrectionTaskExecutor>(CheckERPNeedCorrectionTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendPayRequestTaskExecutor>(SendPayRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendPaymentRequestTaskExecutor>(SendPaymentRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendAskProsecOfficesRequestExecutor>(SendAskProsecOfficesRequestExecutor.Id);
            Container.RegisterTaskExecutor<SendERULRequestTaskExecutor>(SendERULRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendInformationRequestTaskExecutor>(SendInformationRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendEGRIPRequestTaskExecutor>(SendEGRIPRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendEGRNRequestTaskExecutor>(SendEGRNRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendMVDRequestTaskExecutor>(SendMVDRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<GetSMEVAnswersTaskExecutor>(GetSMEVAnswersTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendReconcileRequestTaskExecutor>(SendReconcileRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<CreateResolutionPayFinesTaskExecutor>(CreateResolutionPayFinesTaskExecutor.Id);
            Container.RegisterTaskExecutor<SSTUExportTaskExecutor>(SSTUExportTaskExecutor.Id);
            Container.RegisterTaskExecutor<MVDPassportRequestTaskExecutor>(MVDPassportRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<MVDLivingPlaceRegistrationRequestTaskExecutor>(MVDLivingPlaceRegistrationRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<MVDStayingPlaceRegistrationRequestTaskExecutor>(MVDStayingPlaceRegistrationRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendERKNMDictRequestExecutor>(SendERKNMDictRequestExecutor.Id);
            Container.RegisterTaskExecutor<SendDORequestTaskExecutor>(SendDORequestTaskExecutor.Id);
        }

        private void RegisterActions()
        {
            this.Container.RegisterExecutionAction<GetSMEVAnswersAction>();
            this.Container.RegisterExecutionAction<ExportAppealsToSOPRAction>();
            this.Container.RegisterExecutionAction<SendPayRegRequestAction>();
            this.Container.RegisterExecutionAction<SendEGRULRequestsAction>();
            this.Container.RegisterExecutionAction<CreateResolutionPayFinesAction>();
            this.Container.RegisterExecutionAction<CheckERPNeedCorrectionAction>();
            this.Container.RegisterExecutionAction<SyncEmailGJIAction>();
            this.Container.RegisterExecutionAction<FindGisGmpDocNumbersAction>();
        }

        private void ReplaceComponents()
        {

            this.Container.ReplaceComponent<IInspectionMenuService>(
                typeof(GkhGji.DomainService.InspectionMenuService),
                Component.For<IInspectionMenuService>().ImplementedBy<InspectionMenuService>().LifeStyle.Transient);

            Container.ReplaceComponent<IResolutionService>(
                typeof(Bars.GkhGji.DomainService.ResolutionService),
                Component.For<IResolutionService>().ImplementedBy<DomainService.ResolutionService>().LifeStyle.Transient);

            Container.ReplaceComponent<IGisGkhRegionalService>(
                typeof(Bars.GkhGji.DomainService.GisGkhRegional.Impl.GisGkhRegionalService),
                Component.For<IGisGkhRegionalService>().ImplementedBy<DomainService.GisGkhRegional.Impl.GisGkhRegionalService>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IGkhBaseReport, BaseChelyabinsk.Report.ActCheck.ActCheckGjiStimulReport, ChelyabinskActCheckGjiStimulReport>();
            this.Container.ReplaceTransient<IGkhBaseReport, BaseChelyabinsk.Report.ChelyabinskDisposalStimulReport, GjiChelyabinskDisposalStimulReport>();
        }

        private void RegisterExports()
        {
            Container.Register(Component.For<IDataExportService>().Named("BaseOmsuDataExport").ImplementedBy<BaseOmsuDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("RiskOrientedMethodDataExport").ImplementedBy<RiskOrientedMethodDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("CourtPracticeDataExport").ImplementedBy<CourtPracticeDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("AppealOrderDataExport").ImplementedBy<AppealOrderDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("AdmonitionDataExport").ImplementedBy<AdmonitionDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("GisGmpDataExport").ImplementedBy<GisGmpExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("PayRegDataExport").ImplementedBy<PayRegExport>().LifeStyle.Transient);
        }

        private void RegisterControllers()
        {
            //справочники 
            Container.RegisterAltDataController<RegionCodeMVD>();
            Container.RegisterAltDataController<FLDocType>();
            Container.RegisterAltDataController<EGRNApplicantType>();
            Container.RegisterAltDataController<EGRNObjectType>();
            Container.RegisterAltDataController<EGRNDocType>();
            Container.RegisterAltDataController<EffectiveKNDIndex>();
            Container.RegisterAltDataController<GISGMPPayerStatus>();
            Container.RegisterController<FileTransportController>();

            Container.RegisterController<MVDPassportExecuteController>();
            Container.RegisterController<MVDLivingPlaceRegistrationExecuteController>();
            Container.RegisterController<MVDStayingPlaceRegistrationExecuteController>();

            Container.RegisterController<PreventiveVisitResultViolationController>();
            Container.RegisterController<PreventiveVisitOperationsController>();

            Container.RegisterController<FileStorageDataController<MKDLicRequestAnswer>>();
            Container.RegisterController<FileStorageDataController<MKDLicRequestAnswerAttachment>>();
            Container.RegisterAltDataController<MKDLicRequestSource>();
            Container.RegisterController<MKDLicRequestExecutantController>();
            Container.RegisterController<MKDLicRequestHeadInspectorController>();

            //административная практика
            Container.RegisterController<CourtPracticeOperationsController>();
            Container.RegisterAltDataController<CourtPractice>();
            Container.RegisterAltDataController<CourtPracticeFile>();
            Container.RegisterAltDataController<CourtPracticeInspector>();
            Container.RegisterAltDataController<CourtPracticeDisputeHistory>();
            Container.RegisterAltDataController<CourtPracticeRealityObject>();
            Container.RegisterController<FileStorageDataController<CourtPractice>>();
            Container.RegisterController<FileStorageDataController<CourtPracticeDisputeHistory>>();
            Container.RegisterController<FileStorageDataController<CourtPracticeFile>>();

            //Email
            Container.RegisterAltDataController<EmailLists>();

            //СМЭВ
            Container.RegisterController<ERKNMExecuteController>();
            Container.RegisterAltDataController<SMEVMVD>();
            Container.RegisterAltDataController<SMEVMVDFile>();
            Container.RegisterController<SMEVMVDExecuteController>();
            Container.RegisterAltDataController<SMEVEGRUL>();
            Container.RegisterAltDataController<SMEVEGRULFile>();
            Container.RegisterController<SMEVEGRULExecuteController>();
            Container.RegisterController<SMEVERULExecuteController>();
            Container.RegisterAltDataController<SMEVEGRIP>();
            Container.RegisterAltDataController<SMEVEGRIPFile>();
            Container.RegisterController<SMEVEGRIPExecuteController>();
            Container.RegisterController<GISGMPExecuteController>();
            Container.RegisterController<PAYREGExecuteController>();
            Container.RegisterController<GISERPExecuteController>();
            Container.RegisterAltDataController<GisGmp>();
            Container.RegisterAltDataController<GisGmpFile>();
            Container.RegisterAltDataController<GISGMPPayments>();
            Container.RegisterAltDataController<PayRegRequests>();
            Container.RegisterAltDataController<PayRegFile>();
            Container.RegisterAltDataController<PayReg>();
            Container.RegisterAltDataController<SMEVEGRN>();
            Container.RegisterAltDataController<SMEVEGRNFile>();
            Container.RegisterAltDataController<SMEVEGRNLog>();
            Container.RegisterController<SMEVEGRNExecuteController>();
            Container.RegisterAltDataController<SMEVEDO>();
            Container.RegisterAltDataController<SMEVEDOFile>();
            Container.RegisterController<SMEVDOExecuteController>();

            Container.RegisterAltDataController<ERKNM>();
            Container.RegisterAltDataController<ERKNMFile>();
            Container.RegisterFileStorageDataController<ERKNMDictFile>();
            Container.RegisterAltDataController<ERKNMResultViolations>();

            Container.RegisterAltDataController<GISERP>();
            Container.RegisterAltDataController<GISERPFile>();
            Container.RegisterAltDataController<GISERPResultViolations>();

            Container.RegisterAltDataController<AppealCitsEmergencyHouse>();
            Container.RegisterController<FileStorageDataController<AppealCitsEmergencyHouse>>();
            //предостережения ГЖИ
            Container.RegisterAltDataController<AppealCitsAdmonition>();
            Container.RegisterController<FileStorageDataController<AppealCitsAdmonition>>();
            Container.RegisterController<FileStorageDataController<AppCitAdmonAnnex>>();
            Container.RegisterAltDataController<AppCitAdmonVoilation>();
            Container.RegisterAltDataController<AppCitAdmonAnnex>();
            Container.RegisterAltDataController<BaseOMSU>();
            Container.RegisterAltDataController<AppCitAdmonAppeal>();
            Container.RegisterController<AppealCitsAdmonitionSignController>();

            //Риск-ориентированный подход
            Container.RegisterAltDataController<KindKNDDict>();
            Container.RegisterAltDataController<KindKNDDictArtLaw>();
            Container.RegisterAltDataController<ROMCategory>();
            Container.RegisterAltDataController<ROMCategoryMKD>();
            Container.RegisterAltDataController<VnResolution>();
            Container.RegisterAltDataController<VpResolution>();
            Container.RegisterAltDataController<VprResolution>();
            Container.RegisterAltDataController<G1Resolution>();
            Container.RegisterAltDataController<VprPrescription>();
            Container.RegisterAltDataController<ROMCalcTask>();
            Container.RegisterController<ROMCalcTaskManOrgController>();
            Container.RegisterController<ROMCalcTaskExecuteController>();
            Container.RegisterController<ROMCalculateController>();
            Container.RegisterController<ManOrgLicenseGisController>();

            //Отправка электронной почты
            Container.RegisterController<SendEmailController>();

            Container.RegisterController<AppCitOperationsController>();
            Container.RegisterController<AdmonitionOperationsController>();

            //Переоформление лицензии
            Container.RegisterAltDataController<LicenseReissuance>();
            Container.RegisterAltDataController<LicenseReissuanceProvDoc>();
            Container.RegisterController<LicenseReissuancePersonController>();
            Container.RegisterAltDataController<BaseLicenseReissuance>();
            //   Container.RegisterAltDataController<BaseLicenseReissuance>();

            //Статья закона в постановлении
            Container.RegisterAltDataController<ResolutionArtLaw>();
            Container.RegisterAltDataController<ResolutionFiz>();
            this.Container.RegisterController<ResolutionArticleLawController>();
            // отчеты
            Container.RegisterTransient<IGkhBaseReport, AppealCitsAdmonitionReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVEGRULReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVEGRIPReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVMVDReport>();
            Container.RegisterTransient<IPrintForm, SMEVReport1>();
            Container.RegisterTransient<IPrintForm, SMEVReport2>();

            //ССТУ
            Container.RegisterController<SSTUExportTaskExecuteController>();
            Container.RegisterController<SSTUExportTaskAppealController>();
            this.Container.RegisterAltDataController<SSTUExportTask>();
        }

        private void RegisterServices()
        {
            // TODO : WCF
            Component.For<ICitizensAppealService>()
                .ImplementedBy<CitizensAppealService>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);

            Component.For<IMobileApplicationService>()
                .ImplementedBy<MobileApplicationService>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);

            Component.For(typeof(IValidator<>))
                .ImplementedBy(typeof(Validator<>))
                .RegisterIn(this.Container);

            this.Container.Register(Component.For<GkhGji.DomainService.ISignature<AppealCitsAdmonition>>().ImplementedBy<AppealCitsAdmonitionSignature>().LifestyleTransient());

            Component.For<IPredLicIP>()
                .ImplementedBy<pmvGZHIgzhiPredLicIP>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);
            
            Component.For<IPredLicUL>()
                .ImplementedBy<pmvGZHIgzhiPredLicUL>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);
            
            Component.For<IPereofIP>()
                .ImplementedBy<pmvGZHIgzhiPereofIP>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);
            
            Component.For<IPereofUL>()
                .ImplementedBy<pmvGZHIgzhiPereofUL>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);
            
            Component.For<IDublikatIP>()
                .ImplementedBy<pmvGZHIgzhiDublikatIP>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);
            
            Component.For<IDublikatUL>()
                .ImplementedBy<pmvGZHIgzhiDublikatUL>()
                // .AsWcfSecurityService()
                .RegisterIn(this.Container);

            Container.Register(Component.For<IDomainService<CourtPractice>>().ImplementedBy<FileStorageDomainService<CourtPractice>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<CourtPracticeDisputeHistory>>().ImplementedBy<FileStorageDomainService<CourtPracticeDisputeHistory>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<CourtPracticeFile>>().ImplementedBy<FileStorageDomainService<CourtPracticeFile>>().LifeStyle.Transient);
            Container.Register(Component.For<ICourtPracticeOperationsService>().ImplementedBy<CourtPracticeOperationsService>().LifeStyle.Transient);
            Container.Register(Component.For<IComplaintsService>().ImplementedBy<ComplaintsService>().LifeStyle.Transient);

            Container.RegisterTransient<IMVDPassportService, MVDPassportService>();
            Container.RegisterTransient<IMVDLivingPlaceRegistrationService, MVDLivingPlaceRegistrationService>();
            Container.RegisterTransient<IMVDStayingPlaceRegistrationService, MVDStayingPlaceRegistrationService>();
            Container.RegisterTransient<IERKNMService, ERKNMService>();

            Container.Register(Component.For<IMKDLicRequestExecutantService>().ImplementedBy<MKDLicRequestExecutantService>().LifeStyle.Transient);
            Container.RegisterTransient<ISMEVEDOService, SMEVEDOService>();
        }

        private void RegisterViewModels()
        {
            ContainerHelper.ReplaceViewModel
                <DisposalInspFoundCheckNormDocItem, Bars.GkhGji.ViewModel.DisposalInspFoundCheckNormDocItemViewModel,
                    DisposalInspFoundCheckNormDocItemViewModel>();

            ContainerHelper.ReplaceViewModel
                <AppealCitsExecutant, Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits.AppealCitsExecutantViewModel,
                    AppealCitsExecutantViewModel>();

            ContainerHelper.ReplaceViewModel
                <MKDLicRequest, Bars.GkhGji.ViewModel.MKDLicRequestViewModel,
                    MKDLicRequestViewModel>();

            //Справочники
            Container.RegisterViewModel<RegionCodeMVD, RegionCodeMVDViewModel>();
            Container.RegisterViewModel<FLDocType, FLDocTypeViewModel>();
            Container.RegisterViewModel<EGRNApplicantType, EGRNApplicantTypeViewModel>();
            Container.RegisterViewModel<EGRNObjectType, EGRNObjectTypeViewModel>();
            Container.RegisterViewModel<EGRNDocType, EGRNDocTypeViewModel>();
            Container.RegisterViewModel<EffectiveKNDIndex, EffectiveKNDIndexViewModel>();
            Container.RegisterViewModel<GISGMPPayerStatus, GISGMPPayerStatusViewModel>();

            //рассылки
            Container.RegisterViewModel<EmailLists, EmailListsViewModel>();

            //СМЭВ
            Container.RegisterViewModel<ERKNM, ERKNMViewModel>();
            Container.RegisterViewModel<ERKNMFile, ERKNMFileViewModel>();
            Container.RegisterViewModel<ERKNMDictFile, ERKNMDictFileViewModel>();
            Container.RegisterViewModel<ERKNMResultViolations, ERKNMResultViolationsViewModel>();
            Container.RegisterViewModel<SMEVMVD, SMEVMVDViewModel>();
            Container.RegisterViewModel<SMEVMVDFile, SMEVMVDFileViewModel>();
            Container.RegisterViewModel<SMEVEGRUL, SMEVEGRULViewModel>();
            Container.RegisterViewModel<SMEVEGRULFile, SMEVEGRULFileViewModel>();
            Container.RegisterViewModel<SMEVEGRIP, SMEVEGRIPViewModel>();
            Container.RegisterViewModel<SMEVEGRIPFile, SMEVEGRIPFileViewModel>();
            Container.RegisterViewModel<GisGmp, GisGmpViewModel>();
            Container.RegisterViewModel<GisGmpFile, GisGmpFileViewModel>();
            Container.RegisterViewModel<GISGMPPayments, GISGMPPaymentsViewModel>();
            Container.RegisterViewModel<PayRegRequests, PayRegRequestsViewModel>();
            Container.RegisterViewModel<PayRegFile, PayRegFileViewModel>();
            Container.RegisterViewModel<PayReg, PayRegViewModel>();
            Container.RegisterViewModel<SMEVEGRN, SMEVEGRNViewModel>();
            Container.RegisterViewModel<SMEVEGRNFile, SMEVEGRNFileViewModel>();
            Container.RegisterViewModel<SMEVEGRNLog, SMEVEGRNLogViewModel>();
            Container.RegisterViewModel<GISERP, GISERPViewModel>();
            Container.RegisterViewModel<GISERPFile, GISERPFileViewModel>();
            Container.RegisterViewModel<GISERPResultViolations, GISERPResultViolationsViewModel>();
            Container.RegisterViewModel<SMEVEDO, SMEVEDOViewModel>();
            Container.RegisterViewModel<SMEVEDOFile, SMEVEDOFileViewModel>();

            //Административная практика
            Container.Register(Component.For<IViewModel<CourtPractice>>().ImplementedBy<CourtPracticeViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeDisputeHistory>>().ImplementedBy<CourtPracticeDisputeHistoryViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeFile>>().ImplementedBy<CourtPracticeFileViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeInspector>>().ImplementedBy<CourtPracticeInspectorViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeRealityObject>>().ImplementedBy<CourtPracticeRealityObjectViewModel>().LifeStyle.Transient);

            //предостережения
            Container.Register(Component.For<IViewModel<AppealCitsAdmonition>>().ImplementedBy<AppealCitsAdmonitionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppCitAdmonVoilation>>().ImplementedBy<AppCitAdmonVoilationViewModel>().LifeStyle.Transient);
            Container.RegisterViewModel<AppCitAdmonAppeal, AppCitAdmonAppealViewModel>();
            Container.RegisterViewModel<AppCitAdmonAnnex, AppCitAdmonAnnexViewModel>();

            //
            Container.Register(Component.For<IViewModel<AppealCitsEmergencyHouse>>().ImplementedBy<AppealCitsEmergencyHouseViewModel>().LifeStyle.Transient);

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
            Container.Register(Component.For<IViewModel<G1Resolution>>().ImplementedBy<G1ResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VprPrescription>>().ImplementedBy<VprVprPrescriptionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCalcTask>>().ImplementedBy<ROMCalcTaskViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCalcTaskManOrg>>().ImplementedBy<ROMCalcTaskManOrgViewModel>().LifeStyle.Transient);

            //обращение за переоформлением лицензии
            this.Container.RegisterViewModel<LicenseReissuance, LicenseReissuanceViewModel>();
            this.Container.RegisterViewModel<LicenseReissuancePerson, LicenseReissuancePersonViewModel>();
            this.Container.RegisterViewModel<LicenseReissuanceProvDoc, LicenseReissuanceProvDocViewModel>();
            this.Container.RegisterViewModel<BaseLicenseReissuance, BaseLicenseReissuanceViewModel>();
            // Container.RegisterViewModel<BaseLicenseReissuance, BaseLicenseReissuanceViewModel>();

            Container.Register(Component.For<IViewModel<SSTUExportTask>>().ImplementedBy<SSTUExportTaskViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<SSTUExportTaskAppeal>>().ImplementedBy<SSTUExportTaskAppealViewModel>().LifeStyle.Transient);
            Container.RegisterViewModel<BaseOMSU, BaseOMSUViewModel>();

            this.Container.RegisterViewModel<AppealCitsTransferResult, AppealCitsTransferResultViewModel>();

            Container.RegisterViewModel<MKDLicRequestAnswerAttachment, MKDLicRequestAnswerAttachmentViewModel>();
            Container.RegisterViewModel<MKDLicRequestAnswer, MKDLicRequestAnswerViewModel>();
            Container.RegisterViewModel<MKDLicRequestExecutant, MKDLicRequestExecutantViewModel>();
            Container.RegisterViewModel<MKDLicRequestHeadInspector, MKDLicRequestHeadInspectorViewModel>();
            Container.RegisterViewModel<MKDLicRequestSource, MKDLicRequestSourceViewModel>();
        }

        private void RegisterDomainServices()
        {
            this.Container.ReplaceTransient<ITaskCalendarService, BaseChelyabinsk.DomainService.TaskCalendarService, DomainService.TaskCalendarService>();
            this.Container.ReplaceTransient<IDisposalInsFoundationCheckService, GkhGji.DomainService.Impl.DisposalInsFoundationCheckService, DisposalInsFoundationCheckService>();
            this.Container.ReplaceTransient<IEDSDocumentService, BaseChelyabinsk.DomainService.Impl.EDSDocumentService, EDSDocumentService>();
            Container.Register(Component.For<IDomainService<AppealCitsEmergencyHouse>>().ImplementedBy<FileStorageDomainService<AppealCitsEmergencyHouse>>().LifeStyle.Transient);
            // предостережения ГЖИ
            Container.Register(Component.For<IDomainService<AppealCitsAdmonition>>().ImplementedBy<FileStorageDomainService<AppealCitsAdmonition>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<AppCitAdmonAnnex>>().ImplementedBy<FileStorageDomainService<AppCitAdmonAnnex>>().LifeStyle.Transient);
            //Статья закона в постановлении
            this.Container.Register(Component.For<IResolutionArticleLawService>().ImplementedBy<ResolutionArticleLawService>().LifeStyle.Transient);
            //риск-ориентированный подход
            Container.Register(Component.For<IROMCalcTaskManOrgService>().ImplementedBy<ROMCalcTaskManOrgService>().LifeStyle.Transient);
            Container.Register(Component.For<IKindKNDDictArtLawService>().ImplementedBy<KindKNDDictArtLawService>().LifeStyle.Transient);
            Container.RegisterTransient<IManOrgLicenseGisService, ManOrgLicenseGisService>();
            Container.RegisterTransient<ILicenseReissuancePersonService, LicenseReissuancePersonService>();
            Container.Register(Component.For<IDomainService<LicenseReissuanceProvDoc>>().ImplementedBy<FileStorageDomainService<LicenseReissuanceProvDoc>>().LifeStyle.Transient);
            Container.RegisterTransient<IAppCitOperationsService, AppCitOperationsService>();
            Container.RegisterTransient<IAdmonitionOperationsService, AdmonitionOperationsService>();
            Container.Register(Component.For<ISSTUExportTaskAppealService>().ImplementedBy<SSTUExportTaskAppealService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IManOrgLicenseRequestService>(
               typeof(Gkh.DomainService.ManOrgLicenseRequestService),
               Component.For<IManOrgLicenseRequestService>().ImplementedBy<DomainService.ManOrgLicenseRequestService>());

            Container.Register(Component.For<IPreventiveVisitService>().ImplementedBy<PreventiveVisitService>().LifeStyle.Transient);

            Container.Register(Component.For<IDomainService<MKDLicRequestAnswer>>().ImplementedBy<FileStorageDomainService<MKDLicRequestAnswer>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<MKDLicRequestAnswerAttachment>>().ImplementedBy<FileStorageDomainService<MKDLicRequestAnswerAttachment>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<MKDLicRequestExecutant>>().ImplementedBy<NewFileStorageDomainService<MKDLicRequestExecutant>>().LifeStyle.Transient);
            Container.RegisterDomainService<ERKNMDictFile, FileStorageDomainService<ERKNMDictFile>>();
        }

        private void RegisterInterceptors()
        {
            Container.Register(Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<ResolutionServiceInterceptor>().LifeStyle.Transient);
            Container.RegisterDomainInterceptor<ROMCalcTask, ROMCalcTaskInterceptor>();
            Container.RegisterDomainInterceptor<ROMCategory, ROMCategoryInterceptor>();
            Container.RegisterDomainInterceptor<BaseOMSU, BaseOMSUServiceInterceptor>();
            Container.RegisterDomainInterceptor<SMEVMVD, SMEVMVDInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsAdmonition, AppealCitsAdmonitionInterceptor>();
            Container.RegisterDomainInterceptor<AppCitAdmonVoilation, AppCitAdmonVoilationInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRUL, SMEVEGRULInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRN, SMEVEGRNInterceptor>();
            Container.RegisterDomainInterceptor<AppealOrder, AppealOrderInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRIP, SMEVEGRIPInterceptor>();
            Container.RegisterDomainInterceptor<GisGmp, GisGmpInterceptor>();
            Container.RegisterDomainInterceptor<GISERP, GISERPInterceptor>();
            Container.RegisterDomainInterceptor<DisposalSurveyObjective, DisposalSurveyObjectiveInterceptor>();
            Container.RegisterDomainInterceptor<PayRegRequests, PayRegInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionFiz, ResolutionFizInterceptor>();
            Container.RegisterDomainInterceptor<LicenseReissuance, LicenseReissuanceInterceptor>();
            Container.RegisterDomainInterceptor<ManOrgLicenseRequest, ManOrgLicenseRequestInterceptor>();
            Container.RegisterDomainInterceptor<BaseLicenseReissuance, BaseLicenseReissuanceInterceptor>();
            Container.RegisterDomainInterceptor<ResolPros, ResolProsInterceptor>();
            Container.RegisterDomainInterceptor<CourtPractice, CourtPracticeInterceptor>();
            Container.RegisterDomainInterceptor<SSTUExportTask, SSTUExportTaskInterceptor>();
            Container.ReplaceTransient<IDomainServiceInterceptor<DecisionInspectionReason>, Bars.GkhGji.Interceptors.DecisionInspectionReasonInterceptor, DecisionInspectionReasonInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckAnnex, ActCheckAnnexInterceptor>();
            Container.RegisterDomainInterceptor<DisposalAnnex, DisposalAnnexInterceptor>();
            Container.RegisterDomainInterceptor<DecisionAnnex, DecisionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionAnnex, PrescriptionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolAnnex, ProtocolAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionAnnex, ResolutionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ERKNM, ERKNMInterceptor>();

            Container.RegisterDomainInterceptor<MKDLicRequestAnswer, MKDLicRequestAnswerInterceptor>();
            Container.ReplaceTransient<IDomainServiceInterceptor<MKDLicRequest>, Bars.GkhGji.Interceptors.MKDLicRequestInterceptor, MKDLicRequestInterceptor>();

            Container.ReplaceTransient<IDomainServiceInterceptor<SpecialAccountReport>, GkhGji.Interceptors.SpecialAccountReportInterceptor, SpecialAccountReportInterceptor>();

            Container.RegisterDomainInterceptor<VprPrescription, VprPrescriptionInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionArtLaw, ResolutionArtLawInterceptor>();

            Container.RegisterDomainInterceptor<SMEVEDO, SMEVEDOInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsRequest, AppealCitsRequestInterceptor>();
        }

        private void RegisterInspectionRules()
        {
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DecisionlToActCheckRule>().LifeStyle.Transient);
            // регистрируем провайдер для правил
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseOMSUToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseOMSUToDecisionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseLicenseReissuanceToDisposalRule>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizensBaseChelyabinsk>, BaseAppealCitsService, DomainService.Impl.AppealCitsService>();
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DecisionlToActPreventiveVisitRule>().LifeStyle.Transient);
            this.Container.RegisterTransient<ICitizensAppealServiceClient, CitizensAppealServiceClient>();

            this.Container.ReplaceComponent<ISMEVRule>(
              typeof(GkhGji.InspectionRules.SMEVRule),
              Component.For<ISMEVRule>().ImplementedBy<CheLyabinskSMEVRule>().LifeStyle.Transient);
        }
    }
}