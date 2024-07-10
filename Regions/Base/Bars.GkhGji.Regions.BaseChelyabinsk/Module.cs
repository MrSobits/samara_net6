namespace Bars.GkhGji.Regions.BaseChelyabinsk
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.DomainService.Impl;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Export;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.NumberRule;
    using Bars.GkhGji.Permissions;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controller;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controllers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ExecutionAction;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Export;
    using Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.HeatingSeason;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Inspection;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.LogMap.Provider;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Navigation;
    using Bars.GkhGji.Regions.BaseChelyabinsk.NumberRule;
    using Bars.GkhGji.Regions.BaseChelyabinsk.NumberRule.Impl;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Permissions;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ReminderRule;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.DisposalGji;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.SahalinStateChange.Rule;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl.Intfs;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Services.ServiceContracts;
    using Bars.GkhGji.Regions.BaseChelyabinsk.StateChange;
    using Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Tasks;
    using Bars.GkhGji.Regions.BaseChelyabinsk.TextValues;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol197;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.ViewModel;
    using Bars.GkhGjiCr.Report;

    using Castle.MicroKernel.Registration;

    using ActCheckToDisposalBaseRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.ActCheckToDisposalBaseRule;
    using ActCheckToDisposalRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.ActCheckToDisposalRule;
    using ActCheckToPrescriptionRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.ActCheckToPrescriptionRule;
    using ActCheckViolationViewModel = Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActCheck.ActCheckViolationViewModel;
    using ActRemovalViolationViewModel = Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActCheck.ActRemovalViolationViewModel;
    using DisposalToActCheckByRoRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.DisposalToActCheckByRoRule;
    using DisposalToActCheckRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.DisposalToActCheckRule;
    using DisposalToActCheckWithoutRoRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.DisposalToActCheckWithoutRoRule;
    using HeatSeasonDocService = Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl.HeatSeasonDocService;
    using HeatSeasonService = Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl.HeatSeasonService;
    using InspectionMenuService = Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl.Inspection.InspectionMenuService;
    using PrescriptionViolViewModel = Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Prescription.PrescriptionViolViewModel;
    using ProtocolViolationViewModel = Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol.ProtocolViolationViewModel;
    using ReminderService = Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl.ReminderService;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();

            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Chelyabinsk44 statefulentity");

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiChelyabinskPermissionMap>());
            this.Container.Register(Component.For<IGjiPermission>().ImplementedBy<GjiChelyabinskPermission>());
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            Container.RegisterTransient<ISMEVCertInfoService, SMEVCertInfoService>();
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DecisionNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActRemovalNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActSurveyNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolMhcNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolProsNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolMvdNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PresentationNumberValidationChelyabinskRule>();

            this.Container.RegisterTransient<IRuleChangeStatus, SahalinDisposalNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SahalinActCheckNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SahalinProtocolNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SahalinPrescriptionNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, SahalinActRemovalNumberValidationChelyabinskRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckAddDateStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckRemoveDateStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, CheckActTotalDurationRule>();
            this.Container.RegisterTransient<IViewCollection, GkhGjiChelyabinskViewCollection>("GkhGjiChelyabinskViewCollection");

            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionStateChangeChelyabinskRule>();

            this.Container.RegisterExecutionAction<ActCheckAddDateAction>();
            this.Container.RegisterExecutionAction<MigrateActCheckRemovalToActRemovalAction>();
            this.Container.RegisterExecutionAction<UpdateReminderAction>();
            this.Container.RegisterExecutionAction<GetSMEVAction>();

            this.Container.RegisterTransient<ISmevPrintPdfHelper, SmevPrintPdfHelper>();

            this.Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.GkhGji.Regions.Chelyabinsk dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.RegisterBundlers();

            this.ReplaceComponents();

            this.RegisterControllers();

            this.RegisterInterceptors();

            this.RegisterService();

            this.RegisterServices();

            this.RegisterDomainServices();

            this.RegisterViewModels();

            this.RegisterReports();

            this.RegisterInstectionRules();

            this.RegisterExports();

            this.RegisterAuditLogMap();

            this.RegisterTasks();
        }

        private void RegisterInstectionRules()
        {
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<PrescriptionToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<PrescriptionToDecisionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DecisionToActRemovalPrescriptionRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckWithoutRoRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckWithoutRoRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckByRoRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckByRoRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckPrescriptionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActRemovalPrescriptionRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.ActCheckToDisposalBaseRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToDisposalBaseRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.ActCheckToDisposalRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToDisposalRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.ActCheckToPrescriptionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToPrescriptionRule>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<Protocol197ToResolutionRule>().LifeStyle.Transient);
        }

        private void ReplaceComponents()
        {
            this.Container.ReplaceComponent<IHeatSeasonService>(
                typeof(Bars.GkhGji.DomainService.HeatSeasonService),
                Component.For<IHeatSeasonService>().ImplementedBy<HeatSeasonService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IReminderService>(
                typeof(Bars.GkhGji.DomainService.ReminderService),
                Component.For<IReminderService>().ImplementedBy<ReminderService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IHeatSeasonDocService>(
                typeof(Bars.GkhGji.DomainService.HeatSeasonDocService),
                Component.For<IHeatSeasonDocService>().ImplementedBy<HeatSeasonDocService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<ActCheckViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActCheckViolationViewModel),
                Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ActCheckViolationViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<ActRemovalViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActRemovalViolationViewModel),
                Component.For<IViewModel<ActRemovalViolation>>().ImplementedBy<ActRemovalViolationViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<PrescriptionViol>>(
                typeof(Bars.GkhGji.ViewModel.PrescriptionViolViewModel),
                Component.For<IViewModel<PrescriptionViol>>().ImplementedBy<PrescriptionViolViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<ProtocolViolation>>(
                typeof(Bars.GkhGji.ViewModel.ProtocolViolationViewModel),
                Component.For<IViewModel<ProtocolViolation>>().ImplementedBy<ProtocolViolationViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiNotificationStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IGkhBaseReport, GkhGji.Report.DisposalGjiReport, ChelyabinskDisposalStimulReport>();

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiStateToProsecReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ChelyabinskDisposalGjiStateToProsecReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ActCheckGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActRemovalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ActRemovalStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IDisposalText, GkhGji.TextValues.DisposalText, DisposalText>();

            this.Container.ReplaceComponent<IAppealCitsNumberRule>(
                typeof(AppealCitsNumberRuleTat),
                Component.For<IAppealCitsNumberRule>().ImplementedBy<AppealCitsNumberRuleChelyabinsk>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<BaseJurPerson>>(
                typeof(GkhGji.Interceptors.BaseJurPersonServiceInterceptor),
                Component.For<IDomainServiceInterceptor<BaseJurPerson>>().ImplementedBy<BaseJurPersonServiceInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<AppealCits>>(
                typeof(GkhGji.Interceptors.AppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<AppealCitsServiceInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<HeatSeasonDoc>>(
                typeof(GkhGji.Interceptors.HeatSeasonDocServiceInterceptor),
                Component.For<IDomainServiceInterceptor<HeatSeasonDoc>>().ImplementedBy<HeatSeasonDocServiceInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ChelyabinskProtocolStimulReport>().LifeStyle.Transient);

            // Имена нужны именно такие (не вздумайте поменять)
            this.Container.ReplaceComponent<IPrintForm>(
                typeof(Form1StateHousingInspection),
                Component.For<IPrintForm>()
                    .ImplementedBy(typeof(ChelyabinskForm1StateHousingInspection))
                    .Named("GkhCr.Regions.Chelyabinsk Report.ChelyabinskForm1StateHousingInspection")
                    .LifestyleTransient());

            // Имена нужны именно такие (не вздумайте поменять)
            this.Container.ReplaceComponent<IPrintForm>(
                typeof(HouseTechPassportReport),
                Component.For<IPrintForm>()
                    .ImplementedBy(typeof(NosHouseTechPassportReport))
                    .Named("GkhCr.Regions.Chelyabinsk Report.TechPassport")
                    .LifestyleTransient());

            this.Container.ReplaceComponent<IPrescriptionViolService>(
                typeof(GkhGji.DomainService.PrescriptionViolService),
                Component.For<IPrescriptionViolService>().ImplementedBy<DomainService.Impl.PrescriptionViolService>());

            this.Container.ReplaceComponent<INavigationProvider>(
                typeof(GkhGji.Navigation.DocumentsGjiRegisterMenuProvider),
                Component.For<INavigationProvider>().ImplementedBy<DocumentsGjiRegisterMenuProvider>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IInspectionMenuService>(
                typeof(GkhGji.DomainService.InspectionMenuService),
                Component.For<IInspectionMenuService>().ImplementedBy<InspectionMenuService>().LifeStyle.Transient);
        }

        private void RegisterTasks()
        {
            Container.RegisterTaskExecutor<SendCertInfoRequestTaskExecutor>(SendCertInfoRequestTaskExecutor.Id);
            Container.RegisterTaskExecutor<GetSMEVAnswersExecutor>(GetSMEVAnswersExecutor.Id);
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<Protocol197DefinitionController>();

            Container.RegisterAltDataController<MVDPassport>();
            Container.RegisterAltDataController<MVDPassportFile>();
            Container.RegisterAltDataController<MVDLivingPlaceRegistration>();
            Container.RegisterAltDataController<MVDLivingPlaceRegistrationFile>();
            Container.RegisterAltDataController<MVDStayingPlaceRegistration>();
            Container.RegisterAltDataController<MVDStayingPlaceRegistrationFile>();

            Container.RegisterController<FileStorageDataController<SMEVCertInfo>>();
            Container.RegisterController<FileStorageDataController<SMEVCertInfoFile>>();
            // заменяем контроллеры поскольку в НСО появились новые сущности котоыре заменили старые 
            this.Container.ReplaceController<ChelyabinskDisposalController>("disposal");
            this.Container.ReplaceController<ChelyabinskProtocolController>("protocol");
            this.Container.ReplaceController<ActCheckRealityObjectController>("actcheckrealityobject");
            this.Container.ReplaceController<ChelyabinskActCheckController>("actcheck");
            this.Container.ReplaceController<ChelyabinskPrescriptionController>("prescription");
            this.Container.RegisterAltDataController<PrescriptionBaseDocument>();
            this.Container.RegisterAltDataController<PrescriptionActivityDirection>();
            this.Container.RegisterController<AppealCitsExecutantController>();
            this.Container.RegisterController<TaskCalendarController>();
            this.Container.RegisterAltDataController<DisposalDocConfirm>();
            this.Container.RegisterAltDataController<DisposalAdditionalDoc>();
            this.Container.RegisterAltDataController<AppealOrder>();
            this.Container.RegisterAltDataController<ResolPros>();
            this.Container.RegisterAltDataController<AppealOrderFile>();
            this.Container.RegisterController<DisposalControlMeasuresController>();
            this.Container.RegisterController<StoreContragentController>();
            this.Container.RegisterController<ChelyabinskDocumentGjiController>();
            this.Container.ReplaceController<ActCheckViolationController>("actcheckviolation");
            this.Container.RegisterAltDataController<ProtocolBaseDocument>();
            this.Container.ReplaceController<ChelyabinskDisposalProvidedDocController>("disposalprovideddoc");
            this.Container.RegisterController<MkdChangeNotificationController>();
            this.Container.RegisterFileStorageDataController<MkdChangeNotificationFile>();
            this.Container.RegisterController<Protocol197Controller>();
            this.Container.RegisterController<Protocol197AnnexController>();
            this.Container.RegisterController<Protocol197ArticleLawController>();
            this.Container.RegisterController<FileStorageDataController<Protocol197Annex>>();
            this.Container.RegisterController<Protocol197ViolationController>();
            this.Container.ReplaceController<ChelyabinskActRemovalController>("actremoval");
            this.Container.RegisterController<ActRemovalProvidedDocController>();
            this.Container.RegisterController<ActRemovalInspectedPartController>();
            this.Container.RegisterAltDataController<ActRemovalPeriod>();
            this.Container.RegisterAltDataController<ActRemovalWitness>();
            this.Container.RegisterAltDataController<ActRemovalDefinition>();
            this.Container.RegisterFileStorageDataController<ActRemovalAnnex>();
            this.Container.RegisterController<ActRemovalAnnexController>();
            this.Container.RegisterController<EDSScriptController>();
            this.Container.RegisterAltDataController<SMEVFNSLicRequest>();
            this.Container.RegisterAltDataController<SMEVFNSLicRequestFile>();
            this.Container.RegisterAltDataController<LicenseAction>();
            this.Container.RegisterAltDataController<LicenseActionFile>();
            Container.RegisterAltDataController<SMEVCertInfo>();
            Container.RegisterAltDataController<SMEVCertInfoFile>();
            Container.RegisterController<SMEVCertInfoExecuteController>();

            Container.RegisterAltDataController<SMEVERULReqNumber>();
            Container.RegisterAltDataController<SMEVERULReqNumberFile>();

            //Досудебное
            this.Container.RegisterAltDataController<SMEVComplaints>();
            Container.RegisterAltDataController<SMEVComplaintsDecision>();
            Container.RegisterAltDataController<SMEVComplaintsDecisionLifeSituation>();
            this.Container.RegisterFileStorageDataController<SMEVComplaints>();
            this.Container.RegisterFileStorageDataController<SMEVComplaintsStep>();
            this.Container.RegisterAltDataController<SMEVComplaintsExecutant>();
            this.Container.RegisterAltDataController<SMEVComplaintsRequest>();
            this.Container.RegisterAltDataController<SMEVComplaintsRequestFile>();

            Container.RegisterController<AppealCitsAnswerRegistrationController>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<BaseEntity, BaseEntityInterceptor>();

            this.Container.RegisterDomainInterceptor<AppealCitsExecutant, AppealCitsExecutantInterceptor>();
            this.Container.RegisterDomainInterceptor<ChelyabinskDisposal, ChelyabinskDisposalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ChelyabinskPrescription, ChelyabinskPrescriptionInterceptor>();
            this.Container.RegisterDomainInterceptor<SMEVComplaintsRequest, SMEVComplaintsRequestInterceptor>();
            this.Container.RegisterDomainInterceptor<SMEVComplaints, SMEVComplaintsInterceptor>();
            this.Container.RegisterDomainInterceptor<ChelyabinskProtocol, ChelyabinskProtocolInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckRealityObject, ActCheckRealityObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<ChelyabinskActCheck, ChelyabinskActCheckServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckViolation, ActCheckViolationInterceptor>();
            this.Container.RegisterDomainInterceptor<PrescriptionViol, PrescriptionViolInterceptor>();
            Container.RegisterDomainInterceptor<MVDPassport, MVDPassportInterceptor>();
            Container.RegisterDomainInterceptor<MVDLivingPlaceRegistration, MVDLivingPlaceRegistrationInterceptor>();
            Container.RegisterDomainInterceptor<MVDStayingPlaceRegistration, MVDStayingPlaceRegistrationInterceptor>();
            this.Container.RegisterDomainInterceptor<MkdChangeNotification, MkdChangeNotificationInterceptor>();
            this.Container.RegisterDomainInterceptor<Protocol197, Protocol197ServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ChelyabinskActRemoval, ChelyabinskActRemovalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActRemovalPeriod, ActRemovalPeriodServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckPeriod, ChelyabinskActCheckPeriodServiceInterceptor>();
            Container.RegisterDomainInterceptor<SMEVCertInfo, SMEVCertInfoInterceptor>();
            Container.RegisterDomainInterceptor<TaskCalendar, TaskCalendarInterceptor>();
            Container.RegisterDomainInterceptor<SMEVERULReqNumber, SMEVERULReqNumberInterceptor>();

            Container.RegisterDomainInterceptor<ActRemovalAnnex, ActRemovalAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ActRemovalDefinition, ActRemovalDefinitionInterceptor>();
            Container.RegisterDomainInterceptor<ActRemovalInspectedPart, ActRemovalInspectedPartInterceptor>();
            Container.RegisterDomainInterceptor<ActRemovalProvidedDoc, ActRemovalProvidedDocInterceptor>();
            Container.RegisterDomainInterceptor<ActRemovalWitness, ActRemovalWitnessInterceptor>();
            Container.RegisterDomainInterceptor<DisposalAdditionalDoc, DisposalAdditionalDocInterceptor>();
            Container.RegisterDomainInterceptor<DisposalControlMeasures, DisposalControlMeasuresInterceptor>();
            Container.RegisterDomainInterceptor<DisposalDocConfirm, DisposalDocConfirmInterceptor>();
            Container.RegisterDomainInterceptor<DisposalFactViolation, DisposalFactViolationInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionActivityDirection, PrescriptionActivityDirectionInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionBaseDocument, PrescriptionBaseDocumentInterceptor>();
            Container.RegisterDomainInterceptor<ChelyabinskProtocolSurveySubjectRequirement, ChelyabinskProtocolSurveySubjectRequirementInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolActivityDirection, ProtocolActivityDirectionInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolBaseDocument, ProtocolBaseDocumentInterceptor>();
            Container.RegisterDomainInterceptor<Protocol197ActivityDirection, Protocol197ActivityDirectionInterceptor>();
            Container.RegisterDomainInterceptor<Protocol197Annex, Protocol197AnnexInterceptor>();
            Container.RegisterDomainInterceptor<Protocol197ArticleLaw, Protocol197ArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<Protocol197SurveySubjectRequirement, Protocol197SurveySubjectRequirementInterceptor>();
        }

        private void RegisterDomainServices()
        {
            // Заменяю ДоменСервис для Disposal поскольку теперь сущность Disposal расширена новой сущностью subclass ChelyabinskDisposal 
            // что бы при Disposal.Save выполнилоссь сохранение ChelyabinskDisposal требудется так заменит ьи переопределит ьсвои методы SaveInternal и DeleteInternal
            this.Container.Register(Component.For<IDomainService<Protocol197Definition>>().ImplementedBy<FileStorageDomainService<Protocol197Definition>>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDomainService<SMEVCertInfo>>().ImplementedBy<FileStorageDomainService<SMEVCertInfo>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<SMEVCertInfoFile>>().ImplementedBy<FileStorageDomainService<SMEVCertInfoFile>>().LifeStyle.Transient);
            this.Container.RegisterDomainService<ActRemovalAnnex, FileStorageDomainService<ActRemovalAnnex>>();

            this.Container.ReplaceComponent<IDomainService<Disposal>>(
                typeof(GkhGji.DomainService.DisposalDomainService),
                Component.For<IDomainService<Disposal>>()
                    .ImplementedBy<DomainService.ReplacementDisposalDomainService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<Protocol>>(
                typeof(GkhGji.DomainService.ProtocolDomainService),
                Component.For<IDomainService<Protocol>>()
                    .ImplementedBy<DomainService.ReplacementProtocolDomainService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<ActCheck>>(
                typeof(GkhGji.DomainService.ActCheckDomainService),
                Component.For<IDomainService<ActCheck>>()
                    .ImplementedBy<DomainService.ReplacementActCheckDomainService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<Prescription>>(
                typeof(GkhGji.DomainService.PrescriptionDomainService),
                Component.For<IDomainService<Prescription>>()
                    .ImplementedBy<DomainService.ReplacementPrescriptionDomainService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDisposalProvidedDocService>(
                typeof(DisposalProvidedDocService),
                Component.For<IDisposalProvidedDocService>()
                    .ImplementedBy<ChelyabinskDisposalProvidedDocService>()
                    .LifeStyle.Transient);

            this.Container.RegisterDomainService<MkdChangeNotificationFile, FileStorageDomainService<MkdChangeNotificationFile>>();
            this.Container.RegisterDomainService<SMEVComplaints, FileStorageDomainService<SMEVComplaints>>();
            this.Container.RegisterDomainService<Protocol197Annex, FileStorageDomainService<Protocol197Annex>>();

            this.Container.RegisterDomainService<SMEVComplaintsStep, FileStorageDomainService<SMEVComplaintsStep>>();

            this.Container.ReplaceComponent<IDomainService<ActRemoval>>(
                typeof(GkhGji.DomainService.ActRemovalDomainService),
                Component.For<IDomainService<ActRemoval>>()
                    .ImplementedBy<ReplacementActRemovalDomainService>()
                    .LifeStyle.Transient);

            this.Container.RegisterDomainService<AppealCitsExecutant, NewFileStorageDomainService<AppealCitsExecutant>>();
            this.Container.Register(Component.For<IProtocol197DefinitionService>().ImplementedBy<Protocol197DefinitionService>().LifeStyle.Transient);

            //  this.Container.RegisterDomainService<ChelyabinskDisposal, FileStorageDomainService<ChelyabinskDisposal>>();

            this.Container.Register(Component.For<ISignature<ActRemovalAnnex>>().ImplementedBy<ActRemovalAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<Protocol197Annex>>().ImplementedBy<Protocol197AnnexSignature>().LifestyleTransient());

            Container.Register(Component.For<IDomainService<AppealOrderFile>>().ImplementedBy<FileStorageDomainService<AppealOrderFile>>().LifeStyle
                .Transient);
            Container.Register(Component.For<IEDSDocumentService>().ImplementedBy<EDSDocumentService>().LifeStyle.Transient);
            Container.Register(Component.For<ITaskCalendarService>().ImplementedBy<TaskCalendarService>().LifeStyle.Transient);
        }

        private void RegisterService()
        {
            this.Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Chelyabinsk.Reminder.AppealCitsReminderRule")
                    .ImplementedBy<AppealCitsReminderRule>()
                    .LifeStyle.Transient);

            this.Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Chelyabinsk.Reminder.InspectionReminderRule")
                    .ImplementedBy<InspectionReminderRule>()
                    .LifeStyle.Transient);

            this.Container.RegisterTransient<IExtReminderService, ChelyabinskReminderService>();
            this.Container.RegisterTransient<IAppealCitsExecutantService, AppealCitsExecutantService>();
            this.Container.RegisterTransient<INumberRuleChelyabinsk, NumberRuleChelyabinsk>();
            this.Container.RegisterTransient<IPrescriptionActivityDirectionService, PrescriptionActivityDirectionService>();
            this.Container.RegisterTransient<IDisposalControlMeasuresService, DisposalControlMeasuresService>();

            this.Container
                .RegisterTransient<IAppealCitsService<ViewAppealCitizensBaseChelyabinsk>, AppealCitsService<ViewAppealCitizensBaseChelyabinsk>>();

            this.Container.ReplaceComponent<IPrescriptionService>(
                typeof(PrescriptionService),
                Component
                    .For<IPrescriptionService>()
                    .ImplementedBy<ChelyabinskPrescriptionService>()
                    .LifestyleTransient());

            this.Container.ReplaceTransient<IProtocolService, ProtocolService, ChelyabinskProtocolService>();

            this.Container.RegisterTransient<IDisposalFactViolationService, DisposalFactViolationService>();

            this.Container.RegisterTransient<IActCheckRoChelyabinskService, ActCheckRoChelyabinskService>();
            this.Container.RegisterTransient<IChelyabinskActCheckService, ChelyabinskActCheckService>();
            this.Container.RegisterTransient<IChelyabinskActRemovalService, ChelyabinskActRemovalService>();
            this.Container.RegisterTransient<IMkdChangeNotificationService, MkdChangeNotificationService>();
            this.Container.RegisterTransient<IProtocol197Service, Protocol197Service>();
            this.Container.RegisterTransient<IProtocol197ArticleLawService, Protocol197ArticleLawService>();
            this.Container.RegisterTransient<IProtocol197ViolationService, Protocol197ViolationService>();
            this.Container.RegisterTransient<IActRemovalInspectedPartService, ActRemovalInspectedPartService>();
            this.Container.RegisterTransient<IActRemovalProvidedDocService, ActRemovalProvidedDocService>();

            this.Container.RegisterTransient<IAppealCitsAnswerRegistrationService, AppealCitsAnswerRegistrationService>();
        }

        private void RegisterServices()
        {
            Component.For<IAmirsService>()
                .ImplementedBy<AmirsService>()
                //.AsWcfSecurityService()
                .RegisterIn(this.Container);

            Component.For(typeof(IValidator<>))
                .ImplementedBy(typeof(Validator<>))
                .RegisterIn(this.Container);
        }

        private void RegisterViewModels()
        {
            this.Container.Register(Component.For<IViewModel<Protocol197Definition>>().ImplementedBy<Protocol197DefinitionViewModel>().LifeStyle.Transient);

            Container.RegisterViewModel<MVDPassport, MVDPassportViewModel>();
            Container.RegisterViewModel<MVDPassportFile, MVDPassportFileViewModel>();
            Container.RegisterViewModel<MVDLivingPlaceRegistration, MVDLivingPlaceRegistrationViewModel>();
            Container.RegisterViewModel<MVDLivingPlaceRegistrationFile, MVDLivingPlaceRegistrationFileViewModel>();
            Container.RegisterViewModel<MVDStayingPlaceRegistration, MVDStayingPlaceRegistrationViewModel>();
            Container.RegisterViewModel<MVDStayingPlaceRegistrationFile, MVDStayingPlaceRegistrationFileViewModel>();

            this.Container.RegisterViewModel<AppealCitsExecutant, AppealCitsExecutantViewModel>();
            this.Container.RegisterViewModel<DisposalControlMeasures, DisposalControlMeasuresViewModel>();
            this.Container.RegisterViewModel<AppealOrder, AppealOrderViewModel>();
            this.Container.RegisterViewModel<AppealOrderFile, AppealOrderFileViewModel>();
            this.Container.RegisterViewModel<TaskCalendar, TaskCalendarViewModel>();
            this.Container.RegisterViewModel<ChelyabinskDisposal, ChelyabinskDisposalViewModel>();
            this.Container.RegisterViewModel<DisposalDocConfirm, DisposalDocConfirmViewModel>();
            this.Container.RegisterViewModel<DisposalAdditionalDoc, DisposalAdditionalDocViewModel>();
            this.Container.RegisterViewModel<ChelyabinskProtocol, ChelyabinskProtocolViewModel>();
            this.Container.RegisterViewModel<ChelyabinskActCheck, ChelyabinskActCheckViewModel>();
            this.Container.RegisterViewModel<ChelyabinskPrescription, ChelyabinskPrescriptionViewModel>();

            Container.RegisterViewModel<SMEVERULReqNumber, SMEVERULReqNumberViewModel>();
            Container.RegisterViewModel<SMEVERULReqNumberFile, SMEVERULReqNumberFileViewModel>();

            this.Container.RegisterViewModel<PrescriptionBaseDocument, PrescriptionBaseDocViewModel>();

            this.Container.RegisterViewModel<ProtocolBaseDocument, ProtocolBaseDocViewModel>();

            this.Container.ReplaceComponent<IViewModel<ChelyabinskDisposalProvidedDoc>>(
                typeof(DisposalProvidedDocViewModel),
                Component.For<IViewModel<ChelyabinskDisposalProvidedDoc>>()
                    .ImplementedBy<ChelyabinskDisposalProvidedDocViewModel>()
                    .LifeStyle.Transient);

            this.Container.RegisterViewModel<LicenseAction, LicenseActionViewModel>();
            this.Container.RegisterViewModel<LicenseActionFile, LicenseActionFileViewModel>();
            this.Container.RegisterViewModel<MkdChangeNotification, MkdChangeNotificationViewModel>();
            this.Container.RegisterViewModel<MkdChangeNotificationFile, MkdChangeNotificationFileViewModel>();
            this.Container.RegisterViewModel<Protocol197ArticleLaw, Protocol197ArticleLawViewModel>();
            this.Container.RegisterViewModel<Protocol197Annex, Protocol197AnnexViewModel>();
            this.Container.RegisterViewModel<Protocol197Violation, Protocol197ViolationViewModel>();
            this.Container.RegisterViewModel<ChelyabinskActRemoval, ChelyabinskActRemovalViewModel>();
            this.Container.RegisterViewModel<ActRemovalAnnex, ActRemovalAnnexViewModel>();
            this.Container.RegisterViewModel<ActRemovalDefinition, ActRemovalDefinitionViewModel>();
            this.Container.RegisterViewModel<ActRemovalInspectedPart, ActRemovalInspectedPartViewModel>();
            this.Container.RegisterViewModel<ActRemovalPeriod, ActRemovalPeriodViewModel>();
            this.Container.RegisterViewModel<ActRemovalProvidedDoc, ActRemovalProvidedDocViewModel>();
            this.Container.RegisterViewModel<ActRemovalWitness, ActRemovalWitnessViewModel>();
            Container.RegisterViewModel<SMEVCertInfo, SMEVCertInfoViewModel>();
            Container.RegisterViewModel<SMEVCertInfoFile, SMEVCertInfoFileViewModel>();

            // Перекрываем ViewModel модуля ГЖИ
            this.Container.ReplaceTransient<IViewModel<AppealCits>, AppealCitsViewModel, AppealCitsBaseChelyabinskViewModel>();

            //SMEV
            this.Container.RegisterViewModel<SMEVFNSLicRequest, SMEVFNSLicRequestViewModel>();
            this.Container.RegisterViewModel<SMEVFNSLicRequestFile, SMEVFNSLicRequestFileViewModel>();

            //досудебное
            this.Container.RegisterViewModel<SMEVComplaints, SMEVComplaintsViewModel>();
            this.Container.RegisterViewModel<SMEVComplaintsExecutant, SMEVComplaintsExecutantViewModel>();
            this.Container.RegisterViewModel<SMEVComplaintsRequest, SMEVComplaintsRequestViewModel>();
            this.Container.RegisterViewModel<SMEVComplaintsRequestFile, SMEVComplaintsRequestFileViewModel>();
            this.Container.RegisterViewModel<SMEVComplaintsDecision, SMEVComplaintsDecisionViewModel>();
            this.Container.RegisterViewModel<SMEVComplaintsDecisionLifeSituation, SMEVComplaintsDecisionLifeSituationViewModel>();
            this.Container.RegisterViewModel<SMEVComplaintsStep, SMEVComplaintsStepViewModel>();
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IGkhBaseReport, ActChectQListAnswerReport>();
            this.Container.RegisterTransient<IGkhBaseReport, Protocol197DefinitionReport>();
            this.Container.RegisterTransient<IGkhBaseReport, DecisionStimulReport>();
            this.Container.RegisterTransient<IGkhBaseReport, DecisionNotificationStimulReport>();
            this.Container.RegisterTransient<IGkhBaseReport, DecisionRequirementStimulReport>();
            this.Container.RegisterTransient<IGkhBaseReport, PrescriptionOfficialReportReport>();
            this.Container.RegisterTransient<IGkhBaseReport, DisposalGjiMotivatedRequestReport>();
            this.Container.RegisterTransient<IPrintForm, JurPersonInspectionPlanReport>("Reports.GJI.JurPersonInspectionPlan");
            this.Container.RegisterTransient<IPrintForm, ActReviseInspectionHalfYearReport>("Reports.GJI.ActReviseInspectionHalfYear");
            this.Container.RegisterTransient<IPrintForm, NoActionsMadeListPrescriptionsReport>("Reports.GJI.NoActionsMadeListPrescriptions");
            this.Container.RegisterTransient<IPrintForm, ChelyabinskBusinessActivityReport>("GJI Report.ChelyabinskBusinessActivityReport");
            this.Container.RegisterTransient<IGkhBaseReport, Protocol197StimulReport>();
            this.Container.ReplaceComponent<IGkhBaseReport>(typeof(ResolutionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiStimulReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.JournalAppealsChelyabinsk").ImplementedBy<Report.JournalAppeals>().LifeStyle
                .Transient);
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, MkdChangeNotificationExport>("MkdChangeNotificationExport");
            this.Container.RegisterTransient<IAppealCitsExecutantDataExport, AppealCitsExecutantDataExport>();
            this.Container.RegisterTransient<IDataExportService, Protocol197DataExport>("Protocol197DataExport");
            this.Container.ReplaceTransient<IAppealCitsDataExport, AppealCitsDataExport, AppealCitsBaseChelyabinskDataExport>();
        }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }
    }
}