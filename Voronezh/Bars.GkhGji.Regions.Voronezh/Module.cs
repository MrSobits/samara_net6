namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.Report;
    using Bars.Gkh.ViewModel;
    using Bars.GkhGji.Contracts;
    using Controllers;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules;
    using Bars.GkhGji.Regions.BaseChelyabinsk.NumberRule.Impl;
    using Bars.GkhGji.Regions.BaseChelyabinsk.TextValues;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.DomainService.Impl;
    using Bars.GkhGji.Regions.Voronezh.ExecutionAction;
    using Bars.GkhGji.Regions.Voronezh.InspectionRules.DocumentRules;
    using Bars.GkhGji.Regions.Voronezh.NumberRule;
    using Bars.GkhGji.Regions.Voronezh.Report;
    using Bars.GkhGji.StateChange;
    using Gkh.Utils;
    using AppealCitsService = Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl.AppealCitsService;
    using Chelyabinsk = Bars.GkhGji.Regions.BaseChelyabinsk.Report;
    using Component = Castle.MicroKernel.Registration.Component;
    using B4.Windsor;
    using Entities;
    using ViewModel;
    using Imports;
    using Bars.GkhGji.Regions.Voronezh.Permissions;
    using Bars.GkhGji.Regions.Voronezh.Tasks;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using SMEV3Library.Services;
    using Bars.GkhGji.Regions.Voronezh.Interceptors;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Voronezh.Controllers.FileTransport;
    using Bars.GkhGji.Regions.Voronezh.DataExport;
    using Bars.GkhGji.Regions.Voronezh.StateChanges;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Voronezh.Controllers.ActRemoval;
    using Bars.GkhGji.Regions.Voronezh.Controllers.Protocol197;
    using Bars.GkhGji.Regions.Voronezh.Services.ServiceContracts;
    using Bars.GkhGji.Regions.Voronezh.Services.Impl;
    using Bars.GkhGji.Regions.Voronezh.InspectionRules;
    using Bars.GkhGji.Regions.Voronezh.Navigation;
    using Bars.GkhGji.Regions.Voronezh.Tasks.EGRULSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.EGRNSendInformationRequest;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.Voronezh.Tasks.MVDSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.EGRIPSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendSMEVPropertyType;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVEmergencyHouse;
    using Bars.GkhGji.Regions.Voronezh.Import;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty;
    using Bars.GkhGji.DomainService.GisGkhRegional;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers;
    using Bars.GkhGji.Regions.Voronezh.DomainService.SMEVHelpers;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendSMEVValidPassport;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendSMEVLivingPlace;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendSMEVStayingPlace;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.Voronezh.StateChange;
    using Bars.GkhGji.Regions.Voronezh.Tasks.ERULSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Controllers.SMEV;
    using Bars.GkhGji.Regions.Voronezh.DomainService.InterdepartmentalRequests;
    using Bars.GkhGji.ViewModel.Email;

    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// Метод инициализации модуля
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();
            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Voronezh statefulentity");
            this.Container.Register(Component.For<INavigationProvider>().Named("GkhGJI.Regions.Voronazh navigation").ImplementedBy<NavigationProvider>()
                   .LifeStyle.Transient);
            Container.RegisterTransient<IPermissionSource, GkhGjiVoronezhPermissionMap>();

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.Container.ReplaceTransient<IDisposalText, DisposalText, TextValues.DisposalText>();
            this.Container.RegisterTransient<IFieldRequirementSource, GjiVoronezhFieldRequirementMap>();
            this.Container.RegisterSingleton<ISMEV3Service, SMEV3Service12>();
            this.RegisterServices();
            this.RegisterControllers();
            this.RegisterRules();
            this.ReplaceReports();
            this.RegisterViewModels();
            this.ReplaceRules();
            this.RegisterInspectionRules();
            this.RegisterViews();
            this.RegisterExecuteActions();
            this.RegisterTasks();
            this.RegisterActions();
            RegisterInterceptors();
            RegisterExports();
            ReplaceComponents();

            Container.RegisterTransient<ISMEVEGRULService, SMEVEGRULService>();
   //         Container.RegisterTransient<ISMEVEDOService, SMEVEDOService>();
            Container.RegisterTransient<ISMEVEGRNService, SMEVEGRNService>();
            Container.RegisterTransient<ISMEVFNSLicRequestService, SMEVFNSLicRequestService>();
            Container.RegisterTransient<ISMEVEGRIPService, SMEVEGRIPService>();
            Container.RegisterTransient<ISMEVEDOService, SMEVEDOService>();
            Container.RegisterTransient<IMVDPassportService, MVDPassportService>();
            Container.RegisterTransient<IMVDLivingPlaceRegistrationService, MVDLivingPlaceRegistrationService>();
            Container.RegisterTransient<IMVDStayingPlaceRegistrationService, MVDStayingPlaceRegistrationService>();
            Container.RegisterTransient<IGASUService, GASUService>();
            Container.RegisterTransient<IRPGUService, RPGUService>();
            Container.RegisterTransient<ISMEVPremisesService, SMEVPremisesService>();
            Container.RegisterTransient<ISMEVEmergencyHouseService, SMEVEmergencyHouseService>();
            Container.RegisterTransient<ISMEVDISKVLICService, SMEVDISKVLICService>();
            Container.RegisterTransient<ISMEVSNILSService, SMEVSNILSService>();
            Container.RegisterTransient<ISMEVNDFLService, SMEVNDFLService>();
            Container.RegisterTransient<ISMEVExploitResolutionService, SMEVExploitResolutionService>();
            Container.RegisterTransient<ISMEVChangePremisesStateService, SMEVChangePremisesStateService>();
            Container.RegisterTransient<ISMEVSocialHireService, SMEVSocialHireService>();
            Container.RegisterTransient<ISMEVRedevelopmentService, SMEVRedevelopmentService>();
            Container.RegisterTransient<ISMEVOwnershipPropertyService, SMEVOwnershipPropertyService>();

            Container.RegisterTransient<IVDGOViolatorsService, VDGOViolatorsService>();

            this.Container.RegisterImport<TarifImport>();
            
            this.Container.ReplaceComponent<ISmevPrintPdfHelper>(
                typeof(SmevPrintPdfHelper),
                Component.For<ISmevPrintPdfHelper>().ImplementedBy<SmevPdfHelperVoronezh>().LifeStyle.Transient);

        }

            private void RegisterTasks()
        {
            Container.RegisterTaskExecutor<SendMVDRequestTaskExecutor>(SendMVDRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendInitiateRequestTaskExecutor>(SendInitiateRequestTaskExecutor.Id);           
            Container.RegisterTaskExecutor<SendAskProsecOfficesRequestExecutor>(SendAskProsecOfficesRequestExecutor.Id);
            Container.RegisterTaskExecutor<ERKNMSendInitiateRequestTaskExecutor>(ERKNMSendInitiateRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<GetSMEVAnswersTaskExecutor>(GetSMEVAnswersTaskExecutor.Id);
            Container.RegisterTaskExecutor<GetSMEV2AnswersTaskExecutor>(GetSMEV2AnswersTaskExecutor.Id);
            Container.RegisterTaskExecutor<GetRPGUAnswersTaskExecutor>(GetRPGUAnswersTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendEGRNRequestTaskExecutor>(SendEGRNRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendInformationRequestTaskExecutor>(SendInformationRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendEGRIPRequestTaskExecutor>(SendEGRIPRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendDORequestTaskExecutor>(SendDORequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<MVDPassportRequestTaskExecutor>(MVDPassportRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<MVDLivingPlaceRegistrationRequestTaskExecutor>(MVDLivingPlaceRegistrationRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<MVDStayingPlaceRegistrationRequestTaskExecutor>(MVDStayingPlaceRegistrationRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendSMEVPropertyTypeTaskExecutor>(SendSMEVPropertyTypeTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendSMEVValidPassportTaskExecutor>(SendSMEVValidPassportTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendSMEVLivingPlaceTaskExecutor>(SendSMEVLivingPlaceTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendSMEVStayingPlaceTaskExecutor>(SendSMEVStayingPlaceTaskExecutor.Id);
            Container.RegisterTaskExecutor<SSTUExportTaskExecutor>(SSTUExportTaskExecutor.Id);
            Container.RegisterTaskExecutor<CheckERPNeedCorrectionTaskExecutor>(CheckERPNeedCorrectionTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendCalcRequestTaskExecutor>(SendCalcRequestTaskExecutor.Id);    
            Container.RegisterTaskExecutor<SendPayRequestTaskExecutor>(SendPayRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendPaymentRequestTaskExecutor>(SendPaymentRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendComplaintsCustomRequestTaskExecutor>(SendComplaintsCustomRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SMEVSNILSTaskExecutor>(SMEVSNILSTaskExecutor.Id);
            Container.RegisterTaskExecutor<SMEVExploitResolutionTaskExecutor>(SMEVExploitResolutionTaskExecutor.Id);
            Container.RegisterTaskExecutor<OwnershipPropertyTaskExecutor>(OwnershipPropertyTaskExecutor.Id);
            Container.RegisterTaskExecutor<GASUTaskExecutor>(GASUTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendReconcileRequestTaskExecutor>(SendReconcileRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<CreateResolutionPayFinesTaskExecutor>(CreateResolutionPayFinesTaskExecutor.Id);
            Container.RegisterTaskExecutor<FNSSendRequestTaskExecutor>(FNSSendRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<FileRegisterTaskExecutor>(FileRegisterTaskExecutor.Id);
            Container.RegisterTaskExecutor<PremisesTaskExecutor>(PremisesTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendDISKVLICRequestTaskExecutor>(SendDISKVLICRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendNDFLRequestTaskExecutor>(SendNDFLRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendExploitResolutionRequestTaskExecutor>(SendExploitResolutionRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendChangePremisesStateRequestTaskExecutor>(SendChangePremisesStateRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendSocialHireRequestTaskExecutor>(SendSocialHireRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<RedevelopmentTaskExecutor>(RedevelopmentTaskExecutor.Id);
            Container.RegisterTaskExecutor<EmergencyHouseTaskExecutor>(EmergencyHouseTaskExecutor.Id);
            Container.RegisterTaskExecutor<SendERULRequestTaskExecutor>(SendERULRequestTaskExecutor.Id);
        }

        private void RegisterActions()
        {
            this.Container.RegisterExecutionAction<GetSMEVAnswersAction>();
            this.Container.RegisterExecutionAction<GetRPGUAnswersAction>();
            this.Container.RegisterExecutionAction<CheckERPNeedCorrectionAction>();
            this.Container.RegisterExecutionAction<ExportAppealsToSOPRAction>();
            this.Container.RegisterExecutionAction<SendPayRegRequestAction>();
            this.Container.RegisterExecutionAction<GetComplaintsAction>();
            this.Container.RegisterExecutionAction<CreateResolutionPayFinesAction>();
            this.Container.RegisterExecutionAction<FindGisGmpDocNumbersAction>();
            this.Container.RegisterExecutionAction<SendEGRULRequestsAction>();
            this.Container.RegisterExecutionAction<GetSMEV2AnswersAction>();
        }

        private void RegisterControllers()
        {
            ///ВДГО
            Container.RegisterController<VDGOViolatorsController>();

            Container.RegisterController<SMEVComplaintsLTController>();

            //справочники 
            Container.RegisterAltDataController<RegionCodeMVD>();
            Container.RegisterAltDataController<FLDocType>();
            Container.RegisterAltDataController<EGRNApplicantType>();
            Container.RegisterAltDataController<EGRNDocType>();
            Container.RegisterAltDataController<EGRNObjectType>();
            Container.RegisterAltDataController<GISGMPPayerStatus>();
            Container.RegisterAltDataController<AppealExecutionType>();
            Container.RegisterController<AppealCitsExecutionTypeController>();

            Container.RegisterController<DepartmentlRequestsDTOController>();
            //Email
            Container.RegisterAltDataController<EmailLists>();
            Container.RegisterAltDataController<PreliminaryCheck>();
            Container.RegisterController<ProtocolOSPRequestOperationsController>();
            //Отправка электронной почты
            Container.RegisterController<SendEmailController>();
            Container.RegisterController<AmirsImportController>();
            Container.RegisterController<CourtPracticeOperationsController>();

            Container.RegisterController<GISERPExecuteController>();
            Container.RegisterController<ERKNMExecuteController>();
            Container.RegisterController<ROMCalculateController>();

            //СМЭВ
            Container.RegisterController<SMEVMVDExecuteController>();
            Container.RegisterController<GASUExecuteController>();
            Container.RegisterAltDataController<GASU>();
            Container.RegisterAltDataController<GASUData>();
            Container.RegisterAltDataController<GASUFile>();
            Container.RegisterAltDataController<SMEVMVD>();
            Container.RegisterAltDataController<SMEVMVDFile>();
            Container.RegisterAltDataController<SMEVPropertyType>();
            Container.RegisterAltDataController<SMEVPropertyTypeFile>();
            Container.RegisterAltDataController<SMEVValidPassport>();
            Container.RegisterAltDataController<SMEVValidPassportFile>();
            Container.RegisterAltDataController<SMEVLivingPlace>();
            Container.RegisterAltDataController<SMEVLivingPlaceFile>();
            Container.RegisterAltDataController<SMEVStayingPlace>();
            Container.RegisterAltDataController<SMEVStayingPlaceFile>();
            Container.RegisterAltDataController<GISERP>();
            Container.RegisterAltDataController<ERKNM>();
            Container.RegisterAltDataController<GISERPFile>();
            Container.RegisterAltDataController<ERKNMFile>();
            Container.RegisterAltDataController<GISERPResultViolations>();
            Container.RegisterAltDataController<ERKNMResultViolations>();
            Container.RegisterController<GISGMPExecuteController>();
            Container.RegisterController<PAYREGExecuteController>();
            Container.RegisterAltDataController<GisGmp>();
            Container.RegisterAltDataController<GisGmpFile>();
            Container.RegisterAltDataController<GISGMPPayments>();
            Container.RegisterAltDataController<PayRegRequests>();
            Container.RegisterAltDataController<PayRegFile>();
            Container.RegisterAltDataController<PayReg>();
            this.Container.RegisterController<ResolutionArticleLawController>();
            Container.RegisterAltDataController<SMEVEGRUL>();
            Container.RegisterAltDataController<SMEVEGRULFile>();
            Container.RegisterController<SMEVEGRULExecuteController>();
            Container.RegisterAltDataController<SMEVEDO>();
            Container.RegisterAltDataController<SMEVEDOFile>();
    //        Container.RegisterController<SMEVEDOExecuteController>();
            Container.RegisterAltDataController<SMEVDISKVLIC>();
            Container.RegisterAltDataController<SMEVDISKVLICFile>();
            Container.RegisterAltDataController<SMEVSNILS>();
            Container.RegisterAltDataController<SMEVSNILSFile>();
            Container.RegisterController<SMEVDISKVLICExecuteController>();
            Container.RegisterController<SMEVSNILSExecuteController>();
            Container.RegisterAltDataController<SMEVNDFL>();
            Container.RegisterAltDataController<SMEVNDFLAnswer>();
            Container.RegisterAltDataController<SMEVNDFLFile>();
            Container.RegisterController<SMEVNDFLExecuteController>();
            Container.RegisterAltDataController<SMEVChangePremisesState>();
            Container.RegisterAltDataController<SMEVChangePremisesStateFile>();
            Container.RegisterController<SMEVChangePremisesStateExecuteController>();
            Container.RegisterAltDataController<SMEVSocialHire>();
            Container.RegisterAltDataController<SMEVSocialHireFile>();
            Container.RegisterController<SMEVSocialHireExecuteController>();
            Container.RegisterAltDataController<SMEVRedevelopment>();
            Container.RegisterAltDataController<SMEVRedevelopmentFile>();
            Container.RegisterController<SMEVRedevelopmentExecuteController>();
            Container.RegisterAltDataController<SMEVOwnershipProperty>();
            Container.RegisterAltDataController<SMEVOwnershipPropertyFile>();
            Container.RegisterController<SMEVOwnershipPropertyExecuteController>();
            Container.RegisterAltDataController<SMEVExploitResolution>();
            Container.RegisterAltDataController<SMEVExploitResolutionFile>();
            Container.RegisterController<SMEVExploitResolutionExecuteController>();
            Container.RegisterAltDataController<SMEVEGRN>();
            Container.RegisterController<SMEVFNSLicRequestExecuteController>();
            Container.RegisterAltDataController<SMEVEGRNFile>();
            Container.RegisterController<SMEVEGRNExecuteController>();
            Container.RegisterAltDataController<SMEVEGRIP>();
            Container.RegisterAltDataController<SMEVEGRIPFile>();
            Container.RegisterController<SMEVEGRIPExecuteController>();
            Container.RegisterController<SMEVDOExecuteController>();
            Container.RegisterController<MVDPassportExecuteController>();
            Container.RegisterController<MVDLivingPlaceRegistrationExecuteController>();
            Container.RegisterController<MVDStayingPlaceRegistrationExecuteController>();
            Container.RegisterAltDataController<SMEVEmergencyHouse>();
            Container.RegisterAltDataController<SMEVEmergencyHouseFile>();
            Container.RegisterController<SMEVEmergencyHouseExecuteController>();
            Container.RegisterAltDataController<SMEVPremises>();
            Container.RegisterAltDataController<SMEVPremisesFile>();
            Container.RegisterController<SMEVPremisesExecuteController>();

            //Риск-ориентированный подход
            Container.RegisterAltDataController<EffectiveKNDIndex>();
            Container.RegisterAltDataController<KindKNDDict>();
            Container.RegisterAltDataController<KindKNDDictArtLaw>();
            Container.RegisterAltDataController<ROMCategory>();
            Container.RegisterAltDataController<ROMCategoryMKD>();
            Container.RegisterAltDataController<VnResolution>();
            Container.RegisterAltDataController<VpResolution>();
            Container.RegisterAltDataController<VprResolution>();
            Container.RegisterAltDataController<VprPrescription>();
            Container.RegisterAltDataController<ROMCalcTask>();
            Container.RegisterController<ROMCalcTaskManOrgController>();
            Container.RegisterController<ROMCalcTaskExecuteController>();
            Container.RegisterController<ManOrgLicenseGisController>();

            //административная практика
            Container.RegisterAltDataController<CourtPractice>();
            Container.RegisterAltDataController<CourtPracticeFile>();
            Container.RegisterAltDataController<CourtPracticeInspector>();
            Container.RegisterAltDataController<CourtPracticeRealityObject>();
            Container.RegisterController<FileStorageDataController<CourtPractice>>();
            Container.RegisterController<FileStorageDataController<CourtPracticeFile>>();

            //ССТУ
            Container.RegisterController<SSTUExportTaskExecuteController>();
            Container.RegisterController<SSTUExportTaskAppealController>();
            this.Container.RegisterAltDataController<SSTUExportTask>();

            //Протокол ОСП (не знали к кому подсунуть)
            Container.RegisterAltDataController<ProtocolOSPRequest>();

            //fileTransport
            //TODO: Разобрться с контроллером
            //this.Container.RegisterController<FileTransportController>();

            //Переоформление лицензии
            Container.RegisterAltDataController<LicenseReissuance>();
            Container.RegisterAltDataController<LicenseReissuanceProvDoc>();
            Container.RegisterController<LicenseReissuancePersonController>();
            Container.RegisterAltDataController<LicenseReissuanceRPGU>();
            Container.RegisterAltDataController<BaseLicenseReissuance>();

            Container.RegisterAltDataController<BaseOMSU>();
            Container.RegisterController<SMEVERULExecuteController>();

            Container.RegisterController<PreventiveVisitResultViolationController>();
            Container.RegisterController<PreventiveVisitOperationsController>();

            //Предостережения
            Container.RegisterAltDataController<AppealCitsAdmonition>();
            Container.RegisterAltDataController<AppCitAdmonVoilation>();
            Container.RegisterController<AppealCitsAdmonitionSignController>();
            Container.RegisterController<AppealCitsAdmonitionAnswerSignController>();
            Container.RegisterAltDataController<AppCitAdmonAnnex>();
            Container.RegisterController<FileStorageDataController<AppCitAdmonAnnex>>();
            Container.RegisterAltDataController<AppCitAdmonAppeal>();
            Container.RegisterAltDataController<AppealAnswerExecutionType>();
            Container.RegisterController<AdmonitionOperationsController>();

            //Предписания ФКР
            Container.RegisterAltDataController<AppealCitsPrescriptionFond>();
            Container.RegisterAltDataController<AppCitPrFondVoilation>();
            Container.RegisterAltDataController<AppCitPrFondObjectCr>();

            //отчеты
            Container.RegisterTransient<IGkhBaseReport, SMEVEGRULReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVEGRIPReport>();
            Container.RegisterTransient<IGkhBaseReport, SMEVMVDReport>();
            Container.RegisterTransient<IGkhBaseReport, PayRegReport>();

            //Резолюции
            Container.RegisterAltDataController<AppealCitsResolution>();
            Container.RegisterAltDataController<AppealCitsResolutionExecutor>();

            //Архив файлов ГЖИ
            this.Container.RegisterController<FileRegisterController>();

            this.Container.RegisterController<AppCitPrFondOperationsController>();
        }

        private void RegisterExports()
        {
            Container.Register(Component.For<IDataExportService>().Named("CourtPracticeDataExport").ImplementedBy<CourtPracticeDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("ProtocolOSPRequestDataExport").ImplementedBy<ProtocolOSPRequestDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("AppealOrderDataExport").ImplementedBy<AppealOrderDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("VDGOViolatorsDataExport").ImplementedBy<VDGOViolatorsDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("GISERPDataExport").ImplementedBy<GISERPDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("AdmonitionDataExport").ImplementedBy<AdmonitionDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("BaseOmsuDataExport").ImplementedBy<BaseOmsuDataExport>().LifeStyle.Transient);
        }

        private void RegisterServices()
        {
            Container.RegisterTransient<IAdmonitionOperationsService, AdmonitionOperationsService>();

            this.Container.ReplaceTransient<ITaskCalendarService, Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.TaskCalendarService,
                 Bars.GkhGji.Regions.Voronezh.DomainService.TaskCalendarService>();

            Container.RegisterTransient<IAppCitPrFondOperationsService, AppCitPrFondOperationsService>();
            Container.Register(Component.For<IDomainService<LicenseReissuanceProvDoc>>().ImplementedBy<FileStorageDomainService<LicenseReissuanceProvDoc>>().LifeStyle.Transient);
            Container.RegisterTransient<ILicenseReissuancePersonService, LicenseReissuancePersonService>();

            Component.For<IMobileApplicationService>()
             .ImplementedBy<MobileApplicationService>()
             //.AsWcfSecurityService()
             .RegisterIn(this.Container);

            Component.For<IAppealCitService>()
             .ImplementedBy<AppealCitService>()
             //.AsWcfSecurityService()
             .RegisterIn(this.Container);

            Container.Register(Component.For<ISSTUExportTaskAppealService>().ImplementedBy<SSTUExportTaskAppealService>().LifeStyle.Transient);
            this.Container.ReplaceTransient<IViewModel<CitizenSuggestion>, Bars.Gkh.ViewModel.CitizenSuggestionViewModel, ViewModel.CitizenSuggestionViewModel>();
            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizensBaseChelyabinsk>, AppealCitsService, DomainService.Impl.AppealCitsService>();
            this.Container.ReplaceTransient<IExtReminderService, ChelyabinskReminderService, VoronezhReminderService>();
            this.Container.ReplaceTransient<IExportSuggestionService, ExportSuggestionService, VoronezhExportSuggestionService>();
            Container.Register(Component.For<IPreventiveVisitService>().ImplementedBy<PreventiveVisitService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IInspectionMenuService>(
                typeof(Regions.BaseChelyabinsk.DomainService.Impl.Inspection.InspectionMenuService),
                Component.For<IInspectionMenuService>().ImplementedBy<DomainService.Impl.Inspection.InspectionMenuService>().LifeStyle.Transient);

            Container.RegisterTransient<IGISERPService, GISERP401Service>();
            Container.RegisterTransient<IERKNMService, ERKNMService>();
            //   Container.RegisterTransient<IGISERPService, GISERP502Service>();
            Container.RegisterTransient<ISMEVPropertyTypeService, SMEVPropertyTypeService>();
            Container.RegisterTransient<ISMEVValidPassportService, SMEVValidPassportService>();
            Container.RegisterTransient<ISMEVStayingPlaceService, SMEVStayingPlaceService>();
            Container.RegisterTransient<ISMEVLivingPlaceService, SMEVLivingPlaceService>();
            Container.RegisterTransient<ISMEVMVDService, SMEVMVDService>();

            Container.RegisterTransient<IProtocolOSPRequestOperationsService, ProtocolOSPRequestOperationsService>();

            Container.RegisterTransient<IIRDTOService, IRDTOService>();

            Container.Register(Component.For<IKindKNDDictArtLawService>().ImplementedBy<KindKNDDictArtLawService>().LifeStyle.Transient);
            Container.Register(Component.For<ICourtPracticeOperationsService>().ImplementedBy<CourtPracticeOperationsService>().LifeStyle.Transient);
            Container.Register(Component.For<IROMCalcTaskManOrgService>().ImplementedBy<ROMCalcTaskManOrgService>().LifeStyle.Transient);
            Container.RegisterTransient<IManOrgLicenseGisService, ManOrgLicenseGisService>();

            Container.Register(Component.For<IDomainService<CourtPractice>>().ImplementedBy<FileStorageDomainService<CourtPractice>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<CourtPracticeFile>>().ImplementedBy<FileStorageDomainService<CourtPracticeFile>>().LifeStyle.Transient);

            //Предостережения
            this.Container.Register(Component.For<GkhGji.DomainService.ISignature<AppealCitsAdmonition>>().ImplementedBy<AppealCitsAdmonitionSignature>().LifestyleTransient());
            this.Container.Register(Component.For<IAnswerSignature<AppealCitsAdmonition>>().ImplementedBy<AppealCitsAdmonitionAnswerSignature>().LifestyleTransient());
            Container.Register(Component.For<IDomainService<AppealCitsAdmonition>>().ImplementedBy<FileStorageDomainService<AppealCitsAdmonition>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ProtocolOSPRequest>>().ImplementedBy<FileStorageDomainService<ProtocolOSPRequest>>().LifeStyle.Transient);

            Container.Register(Component.For<IDomainService<AppealCitsPrescriptionFond>>().ImplementedBy<FileStorageDomainService<AppealCitsPrescriptionFond>>().LifeStyle.Transient);
            //СМЭВ
            Container.RegisterTransient<IGISGMPService, GISGMPService>();
            Container.RegisterTransient<IPAYREGService, PAYREGService>();
            Container.RegisterTransient<IComplaintsService, ComplaintsService>();
            this.Container.Register(Component.For<IResolutionArticleLawService>().ImplementedBy<ResolutionArticleLawService>().LifeStyle.Transient);
            Container.RegisterTransient<ISMEVERULService, SMEVERUL105Service>();
           // Container.RegisterTransient<ISMEVERULService, SMEVERULService>();

            Container.RegisterTransient<IFileRegisterService, FileRegisterService>();
            Container.Register(Component.For<IDomainService<VDGOViolators>>().ImplementedBy<FileStorageDomainService<VDGOViolators>>().LifeStyle.Transient);

        }

        private void RegisterRules()
        {
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DecisionlToActCheckRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DecisionlToActPreventiveVisitRule>().LifeStyle.Transient);
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DecisionNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PresentationNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionNumberValidationVoronezhRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AppealSOPRStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AppealCitsPosRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AppealCitsPosAnswerRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ASDOUSendRule>();
            Container.RegisterTransient<IRuleChangeStatus, ComplaintsInWorkRule>();
            Container.RegisterTransient<IRuleChangeStatus, ComplaintsInWorkAcceptRule>();
            Container.RegisterTransient<IRuleChangeStatus, ComplaintsRejectWorkRule>();
            Container.RegisterTransient<IRuleChangeStatus, ComplaintsCompleteWorkRule>();
            Container.RegisterTransient<IRuleChangeStatus, ComplaintsSentExecutatnRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicRequestAcceptRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseReissuanceAcceptRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicRequestDeclineRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolOSPRequestRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseReissuanceDeclineRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseTerminationChangeStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseApprovalChangeStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseInfoRequestAcceptRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseInfoRequestDeclineRule>();
        }

        private void RegisterViewModels()
        {
            ///ВДГО
            Container.RegisterViewModel<VDGOViolators, VDGOViolatorsViewModel>();

            //Справочники
            Container.RegisterViewModel<RegionCodeMVD, RegionCodeMVDViewModel>();
            Container.RegisterViewModel<EGRNApplicantType, EGRNApplicantTypeViewModel>();
            Container.RegisterViewModel<EGRNObjectType, EGRNObjectTypeViewModel>();
            Container.RegisterViewModel<EGRNDocType, EGRNDocTypeViewModel>();
            this.Container.RegisterViewModel<AppealCitsExecutionType, AppealCitsExecutionTypeViewModel>();

            Container.RegisterViewModel<BaseOMSU, BaseOMSUViewModel>();

            //рассылки
            Container.RegisterViewModel<EmailLists, EmailListsViewModel>();
            Container.RegisterViewModel<PreliminaryCheck, PreliminaryCheckViewModel>();

            //СМЭВ
            Container.RegisterViewModel<GASU, GASUViewModel>();
            Container.RegisterViewModel<GASUData, GASUDataViewModel>();
            Container.RegisterViewModel<GASUFile, GASUFileViewModel>();
            Container.RegisterViewModel<SMEVMVD, SMEVMVDViewModel>();
            Container.RegisterViewModel<SMEVMVDFile, SMEVMVDFileViewModel>();
            Container.RegisterViewModel<SMEVPropertyType, SMEVPropertyTypeViewModel>();
            Container.RegisterViewModel<SMEVPropertyTypeFile, SMEVPropertyTypeFileViewModel>();
            Container.RegisterViewModel<SMEVValidPassport, SMEVValidPassportViewModel>();
            Container.RegisterViewModel<SMEVValidPassportFile, SMEVValidPassportFileViewModel>();
            Container.RegisterViewModel<SMEVLivingPlace, SMEVLivingPlaceViewModel>();
            Container.RegisterViewModel<SMEVLivingPlaceFile, SMEVLivingPlaceFileViewModel>();
            Container.RegisterViewModel<SMEVStayingPlace, SMEVStayingPlaceViewModel>();
            Container.RegisterViewModel<SMEVStayingPlaceFile, SMEVStayingPlaceFileViewModel>();
            Container.RegisterViewModel<GISERP, GISERPViewModel>();
            Container.RegisterViewModel<GISERPFile, GISERPFileViewModel>();
            Container.RegisterViewModel<GISERPResultViolations, GISERPResultViolationsViewModel>();
            Container.RegisterViewModel<ERKNM, ERKNMViewModel>();
            Container.RegisterViewModel<ERKNMFile, ERKNMFileViewModel>();
            Container.RegisterViewModel<ERKNMResultViolations, ERKNMResultViolationsViewModel>();
            Container.RegisterViewModel<GisGmp, GisGmpViewModel>();
            Container.RegisterViewModel<GisGmpFile, GisGmpFileViewModel>();
            Container.RegisterViewModel<GISGMPPayments, GISGMPPaymentsViewModel>();
            Container.RegisterViewModel<PayRegRequests, PayRegRequestsViewModel>();
            Container.RegisterViewModel<PayRegFile, PayRegFileViewModel>();
            Container.RegisterViewModel<PayReg, PayRegViewModel>();
            Container.RegisterViewModel<SMEVEGRUL, SMEVEGRULViewModel>();
            Container.RegisterViewModel<SMEVEGRULFile, SMEVEGRULFileViewModel>();
            Container.RegisterViewModel<SMEVEDO, SMEVEDOViewModel>();
            Container.RegisterViewModel<SMEVEDOFile, SMEVEDOFileViewModel>();
            Container.RegisterViewModel<SMEVDISKVLIC, SMEVDISKVLICViewModel>();
            Container.RegisterViewModel<SMEVDISKVLICFile, SMEVDISKVLICFileViewModel>();
            Container.RegisterViewModel<SMEVSNILS, SMEVSNILSViewModel>();
            Container.RegisterViewModel<SMEVSNILSFile, SMEVSNILSFileViewModel>();
            Container.RegisterViewModel<SMEVNDFL, SMEVNDFLViewModel>();
            Container.RegisterViewModel<SMEVNDFLAnswer, SMEVNDFLAnswerViewModel>();
            Container.RegisterViewModel<SMEVNDFLFile, SMEVNDFLFileViewModel>();
            Container.RegisterViewModel<SMEVChangePremisesState, SMEVChangePremisesStateViewModel>();
            Container.RegisterViewModel<SMEVChangePremisesStateFile, SMEVChangePremisesStateFileViewModel>();
            Container.RegisterViewModel<SMEVSocialHire, SMEVSocialHireViewModel>();
            Container.RegisterViewModel<SMEVSocialHireFile, SMEVSocialHireFileViewModel>();
            Container.RegisterViewModel<SMEVRedevelopment, SMEVRedevelopmentViewModel>();
            Container.RegisterViewModel<SMEVRedevelopmentFile, SMEVRedevelopmentFileViewModel>();
            Container.RegisterViewModel<SMEVOwnershipProperty, SMEVOwnershipPropertyViewModel>();
            Container.RegisterViewModel<SMEVOwnershipPropertyFile, SMEVOwnershipPropertyFileViewModel>();
            Container.RegisterViewModel<SMEVExploitResolution, SMEVExploitResolutionViewModel>();
            Container.RegisterViewModel<SMEVExploitResolutionFile, SMEVExploitResolutionFileViewModel>();
            Container.RegisterViewModel<SMEVEGRN, SMEVEGRNViewModel>();
            Container.RegisterViewModel<SMEVEGRNFile, SMEVEGRNFileViewModel>();
            Container.RegisterViewModel<SMEVEGRIP, SMEVEGRIPViewModel>();
            Container.RegisterViewModel<SMEVEGRIPFile, SMEVEGRIPFileViewModel>();
            Container.RegisterViewModel<SMEVPremises, SMEVPremisesViewModel>();
            Container.RegisterViewModel<SMEVPremisesFile, SMEVPremisesFileViewModel>();
            Container.RegisterViewModel<SMEVEmergencyHouse, SMEVEmergencyHouseViewModel>();
            Container.RegisterViewModel<SMEVEmergencyHouseFile, SMEVEmergencyHousesFileViewModel>();
            Container.RegisterViewModel<EffectiveKNDIndex, EffectiveKNDIndexViewModel>();

            //Риск-ориентированный подход
            Container.Register(Component.For<IViewModel<KindKNDDict>>().ImplementedBy<KindKNDDictViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<KindKNDDictArtLaw>>().ImplementedBy<KindKNDDictArtLawViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCategory>>().ImplementedBy<ROMCategoryViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCategoryMKD>>().ImplementedBy<ROMCategoryMKDViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VnResolution>>().ImplementedBy<VnResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VpResolution>>().ImplementedBy<VpResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VprResolution>>().ImplementedBy<VprResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<VprPrescription>>().ImplementedBy<VprVprPrescriptionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCalcTask>>().ImplementedBy<ROMCalcTaskViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ROMCalcTaskManOrg>>().ImplementedBy<ROMCalcTaskManOrgViewModel>().LifeStyle.Transient);

            //Административная практика
            Container.Register(Component.For<IViewModel<CourtPractice>>().ImplementedBy<CourtPracticeViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeFile>>().ImplementedBy<CourtPracticeFileViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeInspector>>().ImplementedBy<CourtPracticeInspectorViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<CourtPracticeRealityObject>>().ImplementedBy<CourtPracticeRealityObjectViewModel>().LifeStyle.Transient);

            //SSTU
            Container.Register(Component.For<IViewModel<SSTUExportTask>>().ImplementedBy<SSTUExportTaskViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<SSTUExportTaskAppeal>>().ImplementedBy<SSTUExportTaskAppealViewModel>().LifeStyle.Transient);

            //Протокол ОСП
            Container.RegisterViewModel<ProtocolOSPRequest, ProtocolOSPRequestViewModel>();

            //обращение за переоформлением лицензии
            this.Container.RegisterViewModel<LicenseReissuance, LicenseReissuanceViewModel>();
            this.Container.RegisterViewModel<LicenseReissuancePerson, LicenseReissuancePersonViewModel>();
            this.Container.RegisterViewModel<LicenseReissuanceProvDoc, LicenseReissuanceProvDocViewModel>();
            this.Container.RegisterViewModel<BaseLicenseReissuance, BaseLicenseReissuanceViewModel>();

            //Предостережения
            Container.Register(Component.For<IViewModel<AppealCitsAdmonition>>().ImplementedBy<AppealCitsAdmonitionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppCitAdmonVoilation>>().ImplementedBy<AppCitAdmonVoilationViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppealAnswerExecutionType>>().ImplementedBy<AppealAnswerExecutionTypeViewModel>().LifeStyle.Transient);
            Container.RegisterViewModel<AppCitAdmonAppeal, AppCitAdmonAppealViewModel>();
            Container.RegisterViewModel<AppCitAdmonAnnex, AppCitAdmonAnnexViewModel>();

            //Предписания ФКР
            Container.Register(Component.For<IViewModel<AppealCitsPrescriptionFond>>().ImplementedBy<AppealCitsPrescriptionFondViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppCitPrFondVoilation>>().ImplementedBy<AppCitPrFondVoilationViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppCitPrFondObjectCr>>().ImplementedBy<AppCitPrFondObjectCrViewModel>().LifeStyle.Transient);

            //Резолюции
            Container.Register(Component.For<IViewModel<AppealCitsResolution>>().ImplementedBy<AppealCitsResolutionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<AppealCitsResolutionExecutor>>().ImplementedBy<AppealCitsResolutionExecutorViewModel>().LifeStyle.Transient);

            //Архив файлов ГЖИ
            this.Container.RegisterViewModel<FileRegister, FileRegisterViewModel>();
        }

        private void ReplaceReports()
        {
            this.Container.ReplaceTransient<IGkhBaseReport, Chelyabinsk.ChelyabinskDisposalStimulReport, VoronezhDisposalStimulReport>();
            Container.RegisterTransient<IGkhBaseReport, AppealCitsAdmonitionReport>();
            Container.RegisterTransient<IGkhBaseReport, AppealAnswer1Report>();
            Container.RegisterTransient<IGkhBaseReport, AppealAnswer2Report>();
            Container.RegisterTransient<IGkhBaseReport, AppealLetterp4st8Report>();
            Container.RegisterTransient<IGkhBaseReport, AppealLetterp3st8Report>();
            Container.RegisterTransient<IGkhBaseReport, AppealResolutionReport>();
        }

        private void ReplaceRules()
        {
            this.Container.ReplaceTransient<IAppealCitsNumberRule, AppealCitsNumberRuleChelyabinsk, AppealCitsNumberRule>();
        }

        private void RegisterInspectionRules()
        {
            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(PrescriptionToDisposalRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<VoronezhPrescriptionToDisposalRule>().LifeStyle.Transient);

            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseLicenseReissuanceToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseOMSUToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseOMSUToDecisionRule>().LifeStyle.Transient);
          //  this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DecisionlToActPreventiveVisitRule>().LifeStyle.Transient);
        }

        private void RegisterViews()
        {
            this.Container.RegisterTransient<IViewCollection, GkhGjiVoronezhViewCollection>("GkhGjiVoronezhViewCollection");
        }


        private void RegisterExecuteActions()
        {
            this.Container.RegisterExecutionAction<FillAppealCitsNumberAction>();
        }

        private void RegisterInterceptors()
        {
            Container.RegisterDomainInterceptor<AppealCitsAdmonition, AppealCitsAdmonitionInterceptor>();
            Container.RegisterDomainInterceptor<AppCitAdmonVoilation, AppCitAdmonVoilationInterceptor>();

            Container.RegisterDomainInterceptor<BaseOMSU, BaseOMSUServiceInterceptor>();
            Container.RegisterDomainInterceptor<SMEVMVD, SMEVMVDInterceptor>();
            Container.RegisterDomainInterceptor<SSTUExportTask, SSTUExportTaskInterceptor>();
            Container.RegisterDomainInterceptor<ROMCategory, ROMCategoryInterceptor>();
            Container.RegisterDomainInterceptor<ROMCalcTask, ROMCalcTaskInterceptor>();
            Container.RegisterDomainInterceptor<GISERP, GISERPInterceptor>();
            Container.RegisterDomainInterceptor<BaseStatement, BaseStatementServiceInterceptor>();
            Container.RegisterDomainInterceptor<AppealOrder, AppealOrderInterceptor>();
            Container.RegisterDomainInterceptor<AppealOrderFile, AppealOrderFileInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsPrescriptionFond, AppealCitsPrescriptionFondInterceptor>();
            Container.RegisterDomainInterceptor<CourtPractice, CourtPracticeInterceptor>();
            Container.RegisterDomainInterceptor<GisGmp, GisGmpInterceptor>();
            Container.RegisterDomainInterceptor<SMEVPropertyType, SMEVPropertyTypeInterceptor>();
            Container.RegisterDomainInterceptor<SMEVValidPassport, SMEVValidPassportInterceptor>();
            Container.RegisterDomainInterceptor<SMEVStayingPlace, SMEVStayingPlaceInterceptor>();
            Container.RegisterDomainInterceptor<SMEVLivingPlace, SMEVLivingPlaceInterceptor>();
            Container.RegisterDomainInterceptor<PayRegRequests, PayRegInterceptor>();         
            Container.RegisterDomainInterceptor<LicenseReissuance, LicenseReissuanceInterceptor>();
            Container.RegisterDomainInterceptor<BaseLicenseReissuance, BaseLicenseReissuanceInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionViol, PrescriptionViolInterceptor>();
            //Container.RegisterDomainInterceptor<ActCheckAnnex, ActCheckAnnexInterceptor>();
            Container.RegisterDomainInterceptor<MKDLicRequestQueryAnswer, MKDLicRequestQueryAnswertInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsRequestAnswer, AppealCitsRequestAnswerInterceptor>();
            Container.RegisterDomainInterceptor<UKDocument, UKDocumentInterceptor>();
            Container.RegisterDomainInterceptor<DisposalAnnex, DisposalAnnexInterceptor>();
            Container.RegisterDomainInterceptor<DecisionAnnex, DecisionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionAnnex, PrescriptionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolAnnex, ProtocolAnnexInterceptor>();
            //Container.RegisterDomainInterceptor<Protocol197Annex, Protocol197AnnexInterceptor>();
            //Container.RegisterDomainInterceptor<ActCheckDefinition, ActCheckDefinitionInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionAnnex, ResolutionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRUL, SMEVEGRULInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEDO, SMEVEDOInterceptor>();
            Container.RegisterDomainInterceptor<SMEVDISKVLIC, SMEVDISKVLICInterceptor>();
            Container.RegisterDomainInterceptor<SMEVSNILS, SMEVSNILSInterceptor>();
            Container.RegisterDomainInterceptor<SMEVNDFL, SMEVNDFLInterceptor>();
            Container.RegisterDomainInterceptor<GASU, GASUInterceptor>();
            Container.RegisterDomainInterceptor<SMEVChangePremisesState, SMEVChangePremisesStateInterceptor>();
            Container.RegisterDomainInterceptor<SMEVSocialHire, SMEVSocialHireInterceptor>();
            Container.RegisterDomainInterceptor<SMEVRedevelopment, SMEVRedevelopmentInterceptor>();
            Container.RegisterDomainInterceptor<SMEVOwnershipProperty, SMEVOwnershipPropertyInterceptor>();
            Container.RegisterDomainInterceptor<SMEVExploitResolution, SMEVExploitResolutionInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRN, SMEVEGRNInterceptor>();
            Container.RegisterDomainInterceptor<SMEVFNSLicRequest, SMEVFNSLicRequestInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEGRIP, SMEVEGRIPInterceptor>();
            Container.RegisterDomainInterceptor<SMEVPremises, SMEVPremisesInterceptor>();
            Container.RegisterDomainInterceptor<ERKNM, ERKNMInterceptor>();
            Container.RegisterDomainInterceptor<SMEVEmergencyHouse, SMEVEmergencyHouseInterceptor>();
            Container.RegisterDomainInterceptor<FileRegister, FileRegisterInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsRequest, AppealCitsRequestInterceptor>();
            Container.RegisterDomainInterceptor<VDGOViolators, VDGOViolatorsInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolOSPRequest, ProtocolOSPRequestInterceptor>();
        }

        private void ReplaceComponents()
        {
            this.Container.ReplaceController<ActRemovalAnnexController>("ActRemovalAnnex");
            this.Container.ReplaceController<Protocol197AnnexController>("Protocol197Annex");

            this.Container.ReplaceController<AppealCitsRequestController>("AppealCitsRequest");
            this.Container.ReplaceController<ActCheckAnnexController>("ActCheckAnnex");
            this.Container.ReplaceController<ActSurveyAnnexController>("ActSurveyAnnex");
            this.Container.ReplaceController<DisposalAnnexController>("DisposalAnnex");
            this.Container.ReplaceController<DecisionAnnexController>("DecisionAnnex");
            this.Container.ReplaceController<PrescriptionAnnexController>("PrescriptionAnnex");
            this.Container.ReplaceController<PresentationAnnexController>("PresentationAnnex");
            this.Container.ReplaceController<ProtocolAnnexController>("ProtocolAnnex");
            this.Container.ReplaceController<ProtocolMhcAnnexController>("ProtocolMhcAnnex");
            this.Container.ReplaceController<ProtocolMvdAnnexController>("ProtocolMvdAnnex");
            this.Container.ReplaceController<ProtocolRSOAnnexController>("ProtocolRSOAnnex");
            this.Container.ReplaceController<ResolProsAnnexController>("ResolProsAnnex");
            this.Container.ReplaceController<ResolutionAnnexController>("ResolutionAnnex");
            this.Container.ReplaceController<ResolutionRospotrebnadzorAnnexController>("ResolutionRospotrebnadzorAnnex");
            this.Container.ReplaceController<MKDLicRequestFileController>("MKDLicRequestFile");
            this.Container.ReplaceController<MKDLicRequestQueryController>("MKDLicRequestQuery");
            this.Container.ReplaceController<AppealCitsAnswerController>("AppealCitsAnswer");

            this.Container.ReplaceComponent<INavigationProvider>(
               typeof(BaseChelyabinsk.Navigation.DocumentsGjiRegisterMenuProvider),
               Component.For<INavigationProvider>().ImplementedBy<DocumentsGjiRegisterMenuProvider>().LifeStyle.Transient);

            Container.ReplaceComponent<IGisGkhRegionalService>(
               typeof(Bars.GkhGji.DomainService.GisGkhRegional.Impl.GisGkhRegionalService),
               Component.For<IGisGkhRegionalService>().ImplementedBy<Bars.GkhGji.Regions.Voronezh.DomainService.GisGkhRegional.Impl.GisGkhRegionalService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IManOrgLicenseRequestService>(
             typeof(Gkh.DomainService.ManOrgLicenseRequestService),
             Component.For<IManOrgLicenseRequestService>().ImplementedBy<DomainService.ManOrgLicenseRequestService>());

            Container.ReplaceComponent<IDomainServiceInterceptor<Protocol197Annex>>(
               typeof(BaseChelyabinsk.Interceptors.Protocol197AnnexInterceptor),
               Component.For<IDomainServiceInterceptor<Protocol197Annex>>().ImplementedBy<Protocol197AnnexInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<ActCheckAnnex>>(
               typeof(GkhGji.Interceptors.ActCheckAnnexInterceptor),
               Component.For<IDomainServiceInterceptor<ActCheckAnnex>>().ImplementedBy<ActCheckAnnexInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<ActCheckDefinition>>(
               typeof(GkhGji.Interceptors.ActCheckDefinitionInterceptor),
               Component.For<IDomainServiceInterceptor<ActCheckDefinition>>().ImplementedBy<ActCheckDefinitionInterceptor>().LifeStyle.Transient);

            Container.ReplaceTransient<IViewModel<EntityChangeLogRecord>, EntityChangeLogRecordViewModel, VorEntityChangeLogRecordViewModel>();
        }
    }
}
