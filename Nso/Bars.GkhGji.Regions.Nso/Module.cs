namespace Bars.GkhGji.Regions.Nso
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.NumberRule;
    using Bars.GkhGji.Regions.Nso.Controllers;
    using Bars.GkhGji.Regions.Nso.Controllers.Disposal;
    using Bars.GkhGji.Regions.Nso.DomainService;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    using Bars.GkhGji.Regions.Nso.Entities.Protocol;
    using Bars.GkhGji.Regions.Nso.InspectionRules;
    using Bars.GkhGji.Regions.Nso.Interceptors;
    using Bars.GkhGji.Regions.Nso.Interceptors.Prescription;
    using Bars.GkhGji.Regions.Nso.NumberRule;
    using Bars.GkhGji.Regions.Nso.Permissions;
    using Bars.GkhGji.Regions.Nso.Report;
    using Bars.GkhGji.Regions.Nso.Report.DisposalGji;
    using Bars.GkhGji.Regions.Nso.ViewModel;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.StateChange;
    using Bars.GkhGji.ViewModel;
    using Bars.GkhGjiCr.Report;

    using Castle.MicroKernel.Registration;
    using DomainService.Impl;
    using Gkh.Domain;

    using AppealCitsService = GkhGji.DomainService.AppealCitsService;
	using Bars.GkhGji.Regions.Nso.ViewModel.Disposal;
	using Bars.B4.Modules.FileStorage.DomainService;
	using Bars.B4.Modules.DataExport.Domain;
	using Bars.GkhGji.Regions.Nso.Export;
	using Bars.Gkh;
    using Gkh.ExecutionAction;
    using StateChange;
	using Bars.GkhGji.Regions.Nso.Navigation;
	using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Nso.ExecutionAction;
    using Bars.GkhGji.Regions.Nso.LogMap.Provider;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterResources<ResourceManifest>();

            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Nso statefulentity");

            Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiNsoPermissionMap>());
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            Container.RegisterTransient<IRuleChangeStatus, DisposalNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActCheckNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, ProtocolNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, PrescriptionNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActRemovalNumberValidationNsoRule>();

            Container.RegisterTransient<IRuleChangeStatus, SahalinDisposalNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, SahalinActCheckNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, SahalinProtocolNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, SahalinPrescriptionNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, SahalinActRemovalNumberValidationNsoRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActCheckAddDateStateRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActCheckRemoveDateStateRule>();
            Container.RegisterTransient<IRuleChangeStatus, CheckActTotalDurationRule>();
			Container.RegisterTransient<IViewCollection, GkhGjiNsoViewCollection>("GkhGjiNsoViewCollection");

            Container.RegisterTransient<IRuleChangeStatus, PrescriptionStateChangeNsoRule>();

            Container.RegisterExecutionAction<ActCheckAddDateAction>();
			Container.RegisterExecutionAction<MigrateActCheckRemovalToActRemovalAction>();

			RegisterBundlers();

            ReplaceComponents();

            RegisterControllers();

            RegisterInterceptors();

            RegisterService();

            RegisterDomainServices();

            RegisterViewModels();

            RegisterReports();

            RegisterInstectionRules();

			RegisterExports();

            this.RegisterAuditLogMap();
        }

        private void RegisterInstectionRules()
        {
            Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<PrescriptionToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
               typeof(GkhGji.InspectionRules.DisposalToActCheckWithoutRoRule),
               Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.DisposalToActCheckWithoutRoRule>().LifeStyle.Transient);
            
            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckByRoRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.DisposalToActCheckByRoRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckPrescriptionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.DisposalToActRemovalPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.DisposalToActCheckRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.DisposalToActCheckRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.ActCheckToDisposalBaseRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.ActCheckToDisposalBaseRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(GkhGji.InspectionRules.ActCheckToDisposalRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.ActCheckToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
               typeof(GkhGji.InspectionRules.ActCheckToPrescriptionRule),
               Component.For<IDocumentGjiRule>().ImplementedBy<InspectionRules.ActCheckToPrescriptionRule>().LifeStyle.Transient);
        }

        private void ReplaceComponents()
        {
            Container.ReplaceController<Bars.GkhGji.Regions.Nso.Controllers.ReminderController>("reminder");

            Container.ReplaceComponent<IHeatSeasonService>(
                typeof(Bars.GkhGji.DomainService.HeatSeasonService),
                Component.For<IHeatSeasonService>().ImplementedBy<DomainService.HeatSeasonService>().LifeStyle.Transient);

            Container.ReplaceComponent<IReminderService>(
                typeof(Bars.GkhGji.DomainService.ReminderService),
                Component.For<IReminderService>().ImplementedBy<DomainService.ReminderService>().LifeStyle.Transient);

            Container.ReplaceComponent<IHeatSeasonDocService>(
                typeof(Bars.GkhGji.DomainService.HeatSeasonDocService),
                Component.For<IHeatSeasonDocService>().ImplementedBy<DomainService.HeatSeasonDocService>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ActCheckViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActCheckViolationViewModel),
                Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ViewModel.ActCheckViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ActRemovalViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActRemovalViolationViewModel),
                Component.For<IViewModel<ActRemovalViolation>>().ImplementedBy<ViewModel.ActRemovalViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<PrescriptionViol>>(
                typeof(Bars.GkhGji.ViewModel.PrescriptionViolViewModel),
                Component.For<IViewModel<PrescriptionViol>>().ImplementedBy<ViewModel.PrescriptionViolViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ProtocolViolation>>(
                typeof(Bars.GkhGji.ViewModel.ProtocolViolationViewModel),
                Component.For<IViewModel<ProtocolViolation>>().ImplementedBy<ViewModel.ProtocolViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiNotificationStimulReport>().LifeStyle.Transient);

            Container.ReplaceTransient<IGkhBaseReport, GkhGji.Report.DisposalGjiReport, NsoDisposalStimulReport>();

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiStateToProsecReport),
                Component.For<IGkhBaseReport>().ImplementedBy<NsoDisposalGjiStateToProsecReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ActCheckGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActRemovalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ActRemovalStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IDisposalText>(
                typeof(GkhGji.TextValues.DisposalText),
                Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>().LifeStyle.Transient);

            Container.ReplaceComponent<IAppealCitsNumberRule>(
                typeof(AppealCitsNumberRuleTat),
                Component.For<IAppealCitsNumberRule>().ImplementedBy<AppealCitsNumberRuleNso>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<BaseJurPerson>>(
                typeof(GkhGji.Interceptors.BaseJurPersonServiceInterceptor),
                Component.For<IDomainServiceInterceptor<BaseJurPerson>>().ImplementedBy<Interceptors.BaseJurPersonServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<AppealCits>>(
                typeof(GkhGji.Interceptors.AppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<Interceptors.AppealCitsServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<HeatSeasonDoc>>(
                typeof(GkhGji.Interceptors.HeatSeasonDocServiceInterceptor),
                Component.For<IDomainServiceInterceptor<HeatSeasonDoc>>().ImplementedBy<Interceptors.HeatSeasonDocServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.NsoProtocolStimulReport>().LifeStyle.Transient);

            // Имена нужны именно такие (не вздумайте поменять)
            Container.ReplaceComponent<IPrintForm>(
                typeof(Form1StateHousingInspection),
                Component.For<IPrintForm>()
                .ImplementedBy(typeof(NsoForm1StateHousingInspection))
                .Named("GkhCr.Regions.Nso Report.NsoForm1StateHousingInspection")
                .LifestyleTransient());

            // Имена нужны именно такие (не вздумайте поменять)
            Container.ReplaceComponent<IPrintForm>(
                typeof(HouseTechPassportReport),
                Component.For<IPrintForm>()
                .ImplementedBy(typeof(NosHouseTechPassportReport))
                .Named("GkhCr.Regions.Nso Report.TechPassport")
                .LifestyleTransient());

            Container.ReplaceComponent<IPrescriptionViolService>(
                typeof(GkhGji.DomainService.PrescriptionViolService),
                Component.For<IPrescriptionViolService>().ImplementedBy<DomainService.Impl.PrescriptionViolService>());

			Container.ReplaceComponent<INavigationProvider>(
				typeof(GkhGji.Navigation.DocumentsGjiRegisterMenuProvider),
				Component.For<INavigationProvider>().ImplementedBy<DocumentsGjiRegisterMenuProvider>().LifeStyle.Transient);

			Container.ReplaceComponent<IInspectionMenuService>(
				typeof(GkhGji.DomainService.InspectionMenuService),
				Component.For<IInspectionMenuService>().ImplementedBy<DomainService.Impl.InspectionMenuService>().LifeStyle.Transient);
        }

        private void RegisterControllers()
        {
            // заменяем контроллеры поскольку в НСО появились новые сущности котоыре заменили старые 
            Container.ReplaceController<NsoDisposalController>("disposal");
            Container.ReplaceController<NsoProtocolController>("protocol");
            Container.ReplaceController<ActCheckRealityObjectController>("actcheckrealityobject");
            Container.ReplaceController<NsoActCheckController>("actcheck");
            Container.ReplaceController<NsoPrescriptionController>("prescription");
            Container.RegisterAltDataController<PrescriptionBaseDocument>();
            Container.RegisterAltDataController<PrescriptionActivityDirection>();
            Container.RegisterController<AppealCitsExecutantController>();
            Container.RegisterAltDataController<DisposalVerificationSubject>();
            Container.RegisterAltDataController<DisposalDocConfirm>();
            Container.RegisterController<StoreContragentController>();
            Container.RegisterController<NsoDocumentGjiController>();
            Container.ReplaceController<ActCheckViolationController>("actcheckviolation");
            Container.RegisterAltDataController<ProtocolBaseDocument>();
			Container.ReplaceController<NsoDisposalProvidedDocController>("disposalprovideddoc");
			Container.RegisterController<MkdChangeNotificationController>();
			Container.RegisterFileStorageDataController<MkdChangeNotificationFile>();
			Container.RegisterController<Protocol197Controller>();
			Container.RegisterController<Protocol197ArticleLawController>();
			Container.RegisterController<FileStorageDataController<Protocol197Annex>>();
			Container.RegisterController<Protocol197ViolationController>();
			Container.ReplaceController<NsoActRemovalController>("actremoval");
			Container.RegisterController<ActRemovalProvidedDocController>();
			Container.RegisterController<ActRemovalInspectedPartController>();
			Container.RegisterAltDataController<ActRemovalPeriod>();
			Container.RegisterAltDataController<ActRemovalWitness>();
			Container.RegisterAltDataController<ActRemovalDefinition>();
			Container.RegisterFileStorageDataController<ActRemovalAnnex>();
			Container.RegisterAltDataController<DisposalSurveyObjective>();
			Container.RegisterAltDataController<DisposalAdminRegulation>();
			Container.RegisterAltDataController<DisposalInspFoundation>();
			Container.RegisterAltDataController<DisposalInspFoundationCheck>();
        }

        private void RegisterInterceptors()
        {
            Container.RegisterDomainInterceptor<AppealCitsExecutant, AppealCitsExecutantInterceptor>();
            Container.RegisterDomainInterceptor<NsoDisposal, NsoDisposalServiceInterceptor>();
            Container.RegisterDomainInterceptor<NsoPrescription, NsoPrescriptionInterceptor>();
            Container.RegisterDomainInterceptor<NsoProtocol, NsoProtocolInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckRealityObject, ActCheckRealityObjectInterceptor>();
            Container.RegisterDomainInterceptor<NsoActCheck, NsoActCheckServiceInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckViolation, ActCheckViolationInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionViol, PrescriptionViolInterceptor>();
			Container.RegisterDomainInterceptor<MkdChangeNotification, MkdChangeNotificationInterceptor>();
			Container.RegisterDomainInterceptor<Protocol197, Protocol197ServiceInterceptor>();
			Container.RegisterDomainInterceptor<NsoActRemoval, NsoActRemovalServiceInterceptor>();
			Container.RegisterDomainInterceptor<ActRemovalPeriod, ActRemovalPeriodServiceInterceptor>();
			Container.RegisterDomainInterceptor<ActCheckPeriod, NsoActCheckPeriodServiceInterceptor>();
        }

        private void RegisterDomainServices()
        {
            // Заменяю ДоменСервис для Disposal поскольку теперь сущность Disposal расширена новой сущностью subclass NsoDisposal 
            // что бы при Disposal.Save выполнилоссь сохранение NsoDisposal требудется так заменит ьи переопределит ьсвои методы SaveInternal и DeleteInternal
            Container.ReplaceComponent<IDomainService<Disposal>>(
                typeof(GkhGji.DomainService.DisposalDomainService),
                Component.For<IDomainService<Disposal>>()
                         .ImplementedBy<DomainService.ReplacementDisposalDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Protocol>>(
                typeof(GkhGji.DomainService.ProtocolDomainService),
                Component.For<IDomainService<Protocol>>()
                         .ImplementedBy<DomainService.ReplacementProtocolDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ActCheck>>(
                typeof(GkhGji.DomainService.ActCheckDomainService),
                Component.For<IDomainService<ActCheck>>()
                         .ImplementedBy<DomainService.ReplacementActCheckDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Prescription>>(
                typeof(GkhGji.DomainService.PrescriptionDomainService),
                Component.For<IDomainService<Prescription>>()
                         .ImplementedBy<DomainService.ReplacementPrescriptionDomainService>()
                         .LifeStyle.Transient);

			Container.ReplaceComponent<IDisposalProvidedDocService>(
				typeof(DisposalProvidedDocService),
				Component.For<IDisposalProvidedDocService>()
					.ImplementedBy<NsoDisposalProvidedDocService>()
					.LifeStyle.Transient);

			Container.RegisterDomainService<MkdChangeNotificationFile, FileStorageDomainService<MkdChangeNotificationFile>>();
			Container.RegisterDomainService<Protocol197Annex, FileStorageDomainService<Protocol197Annex>>();

	        Container.ReplaceComponent<IDomainService<ActRemoval>>(
		        typeof (GkhGji.DomainService.ActRemovalDomainService),
		        Component.For<IDomainService<ActRemoval>>()
			        .ImplementedBy<DomainService.ReplacementActRemovalDomainService>()
			        .LifeStyle.Transient);
        }

        private void RegisterService()
        {
            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Nso.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Nso.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);

            Container.RegisterTransient<INsoReminderService, NsoReminderService>();
            Container.RegisterTransient<IAppealCitsExecutantService, AppealCitsExecutantService>();
            Container.RegisterTransient<INumberRuleNso, NumberRuleNso>();
            Container.RegisterTransient<IPrescriptionActivityDirectionService, PrescriptionActivityDirectionService>();

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizens>, AppealCitsService, DomainService.Impl.AppealCitsService>();

            Container.ReplaceComponent<IPrescriptionService>(
                typeof(PrescriptionService),
                Component
                    .For<IPrescriptionService>()
                    .ImplementedBy<DomainService.Impl.NsoPrescriptionService>()
                    .LifestyleTransient());

            Container.ReplaceComponent<IProtocolService>(
                typeof(IProtocolService),
                Component
                    .For<IProtocolService, INsoProtocolService>()
                    .ImplementedBy<DomainService.Impl.NsoProtocolService>()
                    .LifestyleTransient());
            
            Container.RegisterTransient<IDisposalFactViolationService, DisposalFactViolationService>();

            Container.RegisterTransient<IActCheckRoNsoService, ActCheckRoNsoService>();
            Container.RegisterTransient<INsoActCheckService, NsoActCheckService>();
            Container.RegisterTransient<INsoActRemovalService, NsoActRemovalService>();
			
			Container.RegisterTransient<IMkdChangeNotificationService, MkdChangeNotificationService>();
			Container.RegisterTransient<IProtocol197Service, Protocol197Service>();
			Container.RegisterTransient<IProtocol197ArticleLawService, Protocol197ArticleLawService>();
			Container.RegisterTransient<IProtocol197ViolationService, Protocol197ViolationService>();
			Container.RegisterTransient<IActRemovalInspectedPartService, ActRemovalInspectedPartService>();
			Container.RegisterTransient<IActRemovalProvidedDocService, ActRemovalProvidedDocService>();
        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<AppealCitsExecutant, AppealCitsExecutantViewModel>();

            Container.RegisterViewModel<NsoDisposal, NsoDisposalViewModel>();
            Container.RegisterViewModel<DisposalDocConfirm, DisposalDocConfirmViewModel>();
            Container.RegisterViewModel<NsoProtocol, NsoProtocolViewModel>();
            Container.RegisterViewModel<NsoActCheck, NsoActCheckViewModel>();
            Container.RegisterViewModel<NsoPrescription, NsoPrescriptionViewModel>();

            Container.RegisterViewModel<PrescriptionBaseDocument, PrescriptionBaseDocViewModel>();
			
            Container.RegisterViewModel<ProtocolBaseDocument, ProtocolBaseDocViewModel>();

			Container.ReplaceComponent<IViewModel<NsoDisposalProvidedDoc>>(
				typeof(DisposalProvidedDocViewModel),
				Component.For<IViewModel<NsoDisposalProvidedDoc>>()
					.ImplementedBy<NsoDisposalProvidedDocViewModel>()
					.LifeStyle.Transient);

			Container.RegisterViewModel<MkdChangeNotification, MkdChangeNotificationViewModel>();
			Container.RegisterViewModel<MkdChangeNotificationFile, MkdChangeNotificationFileViewModel>();
			Container.RegisterViewModel<Protocol197ArticleLaw, Protocol197ArticleLawViewModel>();
			Container.RegisterViewModel<Protocol197Annex, Protocol197AnnexViewModel>();
			Container.RegisterViewModel<Protocol197Violation, Protocol197ViolationViewModel>();
			Container.RegisterViewModel<NsoActRemoval, NsoActRemovalViewModel>();
			Container.RegisterViewModel<ActRemovalAnnex, ActRemovalAnnexViewModel>();
			Container.RegisterViewModel<ActRemovalDefinition, ActRemovalDefinitionViewModel>();
			Container.RegisterViewModel<ActRemovalInspectedPart, ActRemovalInspectedPartViewModel>();
			Container.RegisterViewModel<ActRemovalPeriod, ActRemovalPeriodViewModel>();
			Container.RegisterViewModel<ActRemovalProvidedDoc, ActRemovalProvidedDocViewModel>();
			Container.RegisterViewModel<ActRemovalWitness, ActRemovalWitnessViewModel>();
        }

        private void RegisterReports()
        {
            Container.RegisterTransient<IGkhBaseReport, DisposalGjiMotivatedRequestReport>();
            Container.RegisterTransient<IPrintForm, JurPersonInspectionPlanReport>("Reports.GJI.JurPersonInspectionPlan");
            Container.RegisterTransient<IPrintForm, ActReviseInspectionHalfYearReport>("Reports.GJI.ActReviseInspectionHalfYear");
            Container.RegisterTransient<IPrintForm, NoActionsMadeListPrescriptionsReport>("Reports.GJI.NoActionsMadeListPrescriptions");
            Container.RegisterTransient<IPrintForm, NsoBusinessActivityReport>("GJI Report.NsoBusinessActivityReport");
			Container.RegisterTransient<IGkhBaseReport, Protocol197StimulReport>();
        }

	    private void RegisterExports()
	    {
			Container.RegisterTransient<IDataExportService, MkdChangeNotificationExport>("MkdChangeNotificationExport");
			Container.RegisterTransient<IDataExportService, Protocol197DataExport>("Protocol197DataExport");
	    }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

    }
}