namespace Bars.GkhGji.Regions.Zabaykalye
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Report;
    using Bars.Gkh.TextValues;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Navigation;
    using Bars.GkhGji.Regions.Zabaykalye.Controllers;
    using Bars.GkhGji.Regions.Zabaykalye.Controllers.ActCheck;
    using Bars.GkhGji.Regions.Zabaykalye.DomainService;
    using Bars.GkhGji.Regions.Zabaykalye.DomainService.Impl;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCits;
    using Bars.GkhGji.Regions.Zabaykalye.ExecutionAction;
    using Bars.GkhGji.Regions.Zabaykalye.InspectionRules;
    using Bars.GkhGji.Regions.Zabaykalye.InspectionRules.DocumentRules;
    using Bars.GkhGji.Regions.Zabaykalye.Interceptors;
    using Bars.GkhGji.Regions.Zabaykalye.Interceptors.Dict;
    using Bars.GkhGji.Regions.Zabaykalye.LogMap.Provider;
    using Bars.GkhGji.Regions.Zabaykalye.Permissions;
    using Bars.GkhGji.Regions.Zabaykalye.Report;
    using Bars.GkhGji.Regions.Zabaykalye.Report.ActRemovalGJI;
    using Bars.GkhGji.Regions.Zabaykalye.Report.ProtocolGji;
    using Bars.GkhGji.Regions.Zabaykalye.StateChange;
    using Bars.GkhGji.Regions.Zabaykalye.ViewModel;
    using Bars.GkhGji.Regions.Zabaykalye.ViewModel.AppealCits;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.TextValues;
    using Bars.GkhGji.ViewModel;

    using Castle.MicroKernel.Registration;

    using ActCheckGjiReport = Bars.GkhGji.Report.ActCheckGjiReport;
    using ActCheckRealityObjectInterceptor = Bars.GkhGji.Regions.Zabaykalye.Interceptors.ActCheckRealityObjectInterceptor;
    using ActCheckRealityObjectService = Bars.GkhGji.DomainService.ActCheckRealityObjectService;
    using ActCheckServiceInterceptor = Bars.GkhGji.Interceptors.ActCheckServiceInterceptor;
    using ActCheckViolationViewModel = Bars.GkhGji.ViewModel.ActCheckViolationViewModel;
    using ActRemovalServiceInterceptor = Bars.GkhGji.Interceptors.ActRemovalServiceInterceptor;
    using ActRemovalViolationViewModel = Bars.GkhGji.ViewModel.ActRemovalViolationViewModel;
    using ActSurveyGjiReport = Bars.GkhGji.Report.ActSurveyGjiReport;
    using AppealCitsService = Bars.GkhGji.DomainService.AppealCitsService;
    using AppealCitsServiceInterceptor = Bars.GkhGji.Interceptors.AppealCitsServiceInterceptor;
    using AppealCitsValidationRule = Bars.GkhGji.StateChange.AppealCitsValidationRule;
    using DisposalServiceInterceptor = Bars.GkhGji.Regions.Zabaykalye.Interceptors.DisposalServiceInterceptor;
    using GkhGjiActSurveyOwnerPermissionMap = Bars.GkhGji.Permissions.GkhGjiActSurveyOwnerPermissionMap;
    using InspectionGjiViolInterceptor = Bars.GkhGji.Interceptors.InspectionGjiViolInterceptor;
    using JurPersonService = Bars.GkhGji.DomainService.JurPersonService;
    using PersonInspectionService = Bars.GkhGji.DomainService.PersonInspectionService;
    using PrescriptionInterceptor = Bars.GkhGji.Interceptors.PrescriptionInterceptor;
    using PrescriptionViolViewModel = Bars.GkhGji.ViewModel.PrescriptionViolViewModel;
    using ProtocolDomainService = Bars.GkhGji.DomainService.ProtocolDomainService;
    using ProtocolGjiReport = Bars.GkhGji.Report.ProtocolGjiReport;
    using ProtocolServiceInterceptor = Bars.GkhGji.Interceptors.ProtocolServiceInterceptor;
    using ProtocolViewModel = Bars.GkhGji.Regions.Zabaykalye.ViewModel.ProtocolViewModel;
    using ProtocolViolationViewModel = Bars.GkhGji.ViewModel.ProtocolViolationViewModel;
    using ResolutionDomainService = Bars.GkhGji.DomainService.ResolutionDomainService;
    using ResolutionServiceInterceptor = Bars.GkhGji.Interceptors.ResolutionServiceInterceptor;
    using ResolutionViewModel = Bars.GkhGji.DomainService.ResolutionViewModel;
    using TypeSurveyGjiViewModel = Bars.GkhGji.ViewModel.TypeSurveyGjiViewModel;
    using TypeSurveyGoalInspGjiViewModel = Bars.GkhGji.ViewModel.TypeSurveyGoalInspGjiViewModel;
    using TypeSurveyInspFoundationGjiViewModel = Bars.GkhGji.ViewModel.TypeSurveyInspFoundationGjiViewModel;
    using TypeSurveyKindInspGjiViewModel = Bars.GkhGji.ViewModel.TypeSurveyKindInspGjiViewModel;
    using TypeSurveyTaskInspGjiViewModel = Bars.GkhGji.ViewModel.TypeSurveyTaskInspGjiViewModel;
    using Gkh.Utils;

    using ResolProsDefinition = Bars.GkhGji.Regions.Zabaykalye.Entities.ResolProsDefinition;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>()
                         .Named("GkhGji.Regions.Zabaykalye resources")
                         .ImplementedBy<ResourceManifest>()
                         .LifeStyle.Transient);

            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Zabaykalye statefulentity");

            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Zabaykalye.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Zabaykalye.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);

            Container.UsingForResolved<IMenuItemText>((container, menuItemText) =>
            {
                menuItemText.Override("Проверки по обращениям граждан", "Проверки по обращениям и заявлениям");
                menuItemText.Override("Инспекционные проверки", "Инспекционные обследования");
            });

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiPermissionMap>());

            Container.ReplaceComponent<IPermissionSource>(typeof(GkhGjiActSurveyOwnerPermissionMap),
                Component.For<IPermissionSource>().ImplementedBy<Permissions.GkhGjiActSurveyOwnerPermissionMap>().LifeStyle.Transient);
            
            Container.RegisterExecutionAction<ClearFinalStateDocRemindersAction>();

            Container.RegisterExecutionAction<SetFixedForDpkrStatusAction>();

            Container.RegisterNavigationProvider<ZabaykalyeDocGjiRegisterMenuProvider>();

            ReplaceComponents();

            RegisterReports();

            RegisterDomainServices();

            RegisterStateChangeRules();

            RegisterControllers();

            RegisterService();

            RegisterInterceptors();

            RegisterViewModels();

            RegisterInspectionRules();

            this.RegisterAuditLogMap();
        }

        private void RegisterControllers()
        {
            Container.RegisterController<DocumentViolGroupController>();
            Container.RegisterController<DisposalControlMeasuresController>();
            Container.RegisterController<FileStorageDataController<ActSurveyConclusion>>();
            Container.RegisterController<AppealCitsExecutantController>();    
            Container.RegisterController<DisposalSurveySubjectController>();
            Container.RegisterAltDataController<CheckTimeChange>();
            Container.RegisterAltDataController<ResolProsDefinition>();

            // справочники

            // Заменяем контроллер потому что сущность перекрыта новой сущностью
            Container.ReplaceController<ZabaykalyeActSurveyController>("actsurvey");
            Container.ReplaceController<ZabaykalyeProtocolDefinitionController>("protocoldefinition");
            Container.ReplaceController<ZabaykalyeResolutionDefinitionController>("resolutiondefinition");

            Container.ReplaceController<ActCheckController>("actcheck");
            Container.ReplaceController<ZabaykalyeProtocolController>("protocol");
            Container.ReplaceController<ActCheckRealityObjectController>("actcheckrealityobject");
            Container.ReplaceController<ZabaykalyeResolutionController>("resolution");
        }

        private void RegisterService()
        {
            Container.RegisterTransient<IAppealCitsExecutantService, AppealCitsExecutantService>();
            Container.RegisterTransient<IDefinitionService, DefinitionService>();
            Container.RegisterTransient<IDisposalSurveySubjectService, DisposalSurveySubjectService>();
            Container.RegisterTransient<IDisposalControlMeasuresService, DisposalControlMeasuresService>();
            Container.RegisterTransient<IViolationGroupService, ViolationGroupService>();
            
            Container.ReplaceComponent<IActCheckService>(typeof(ActCheckService),
                      Component.For<IActCheckService, IZabaykalyeActCheckService>().ImplementedBy<ZabaykalyeActCheckService>().LifeStyle.Transient);
        }

        private void RegisterInterceptors()
        {
            Container.Register(Component.For<IDomainServiceInterceptor<AppealCitsExecutant>>().ImplementedBy<AppealCitsExecutantInterceptor>().LifeStyle.Transient);
            Container.RegisterDomainInterceptor<Disposal, DisposalServiceInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsAnswer, ZabaykalyeAppealCitsAnswerInterceptor>();
            Container.RegisterDomainInterceptor<ZabaykalyeResolutionDefinition, ResolutionDefInterceptor>();
            Container.RegisterDomainInterceptor<ZabaykalyeProtocolDefinition, ProtocolDefInterceptor>();
            Container.RegisterDomainInterceptor<ResolProsDefinition, ResolProsDefInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckDefinition, ActCheckDefInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMhcDefinition, ProtocolMhcDefInterceptor>();
            Container.RegisterDomainInterceptor<ZabaykalyeActSurvey, ZabaykalyeActSurveyInterceptor>();
            Container.RegisterDomainInterceptor<DisposalTypeSurvey, DisposalTypeSurveyInterceptor>();
            Container.RegisterDomainInterceptor<DocumentViolGroup, DocumentViolGroupInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolArticleLaw, ProtocolArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckRealityObject, ActCheckRealityObjectInterceptor>();
        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<ActSurveyConclusion, ActSurveyConclusionViewModel>();
            Container.RegisterViewModel<DisposalControlMeasures, DisposalControlMeasuresViewModel>();
            Container.RegisterViewModel<AppealCitsExecutant, AppealCitsExecutantViewModel>();
            Container.RegisterViewModel<ResolProsDefinition, ResolProsDefinitionViewModel>();
            Container.RegisterViewModel<ZabaykalyeActSurvey, ZabaykalyeActSurveyViewModel>();
            Container.RegisterViewModel<DisposalSurveySubject, DisposalSurveySubjectViewModel>();
            Container.RegisterTransient<IViewModel<CheckTimeChange>, CheckTimeChangeViewModel>();
			
            Container.RegisterViewModel<DocumentViolGroup, DocumentViolGroupViewModel>();

            Container.RegisterViewModel<ZabaykalyeProtocolDefinition, ZabaykalyeProtocolDefinitionViewModel>();
            Container.RegisterViewModel<ZabaykalyeResolutionDefinition, ZabaykalyeResolutionDefinitionViewModel>();
        }
        
        private void RegisterStateChangeRules()
        {
            Container.Register(Component.For<IStateChangeHandler>().Named("GkhGji.Regions.Zabaykalye DocumentGjiStateChangeHandler").ImplementedBy<DocumentGjiStateChangeHandler>().LifeStyle.Transient);

            Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActSurveyValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, DisposalValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PresentationValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolProsValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, AppealCitsValidationNumberRule>();

            Container.ReplaceComponent<IRuleChangeStatus>(typeof(AppealCitsValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.AppealCitsValidationRule>().LifeStyle.Transient);
        }

        private void RegisterDomainServices()
        {
            Container.Register(Component.For<IDomainService<ZabaykalyeProtocolDefinition>>().ImplementedBy<FileStorageDomainService<ZabaykalyeProtocolDefinition>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ZabaykalyeResolutionDefinition>>().ImplementedBy<FileStorageDomainService<ZabaykalyeResolutionDefinition>>().LifeStyle.Transient);

            Container.Register(Component.For<IDomainService<ActSurveyConclusion>>().ImplementedBy<FileStorageDomainService<ActSurveyConclusion>>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Protocol>>(typeof(ProtocolDomainService),
                Component.For<IDomainService<Protocol>>().ImplementedBy<DomainService.ProtocolDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Resolution>>(typeof(ResolutionDomainService),
                Component.For<IDomainService<Resolution>>().ImplementedBy<DomainService.ResolutionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IJurPersonService>(typeof (JurPersonService),
                Component.For<IJurPersonService>().ImplementedBy<DomainService.JurPersonService>().LifeStyle.Transient);

            Container.ReplaceComponent<IPersonInspectionService>(typeof(PersonInspectionService),
                Component.For<IPersonInspectionService>().ImplementedBy<DomainService.PersonInspectionService>().LifeStyle.Transient);
        }

        private void 
            RegisterReports()
        {
            Container.RegisterTransient<IPrintForm, AppealCitsJurnalAccounting>("GkhGji Report.AppealCitsJurnalAccounting");
            Container.RegisterTransient<IPrintForm, AdministrativeOffensesJurnalReport>("GkhGji Report.AdministrativeOffensesJurnalReport");
            Container.RegisterTransient<IPrintForm, ScheduledInspectionSurveysJournal>("GkhGji Report.ScheduledInspectionSurveysJournal");
            Container.RegisterTransient<IPrintForm, RegistrationOutgoingDocuments>("GkhGji Report.RegistrationOutgoingDocuments");
            Container.RegisterTransient<IPrintForm, AdministrativeOffensesResolution>("GkhGji Report.AdministrativeOffensesResolution");
            Container.RegisterTransient<IPrintForm, PrescriptionRegistrationJournal>("GkhGji Report.PrescriptionRegistrationJournal");
            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ResolutionNotificationGjiStimulReport>().LifeStyle.Transient);
            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ResolProsDefinitionReport>().LifeStyle.Transient);
            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ActViewGjiReport>().LifeStyle.Transient);
            Container.ReplaceTransient<IGkhBaseReport, ActRemovalGjiReport, ActRemovalGjiStimulReport>();

            Container.RegisterTransient<IGkhBaseReport, ActCheckNotificationReport>();
            Container.RegisterTransient<IGkhBaseReport, ActRemovalNotificationReport>();
            Container.RegisterTransient<IGkhBaseReport, AppealCitsAnswerStimulReport>();
        }

        private void ReplaceComponents()
        {
            Container.ReplaceComponent<IDomainService<ActSurvey>>(
              typeof(ActSurveyDomainService),
              Component.For<IDomainService<ActSurvey>>().ImplementedBy<ReplacementActSurveyDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ProtocolDefinition>>(
              typeof(ProtocolDefinitionDomainService),
              Component.For<IDomainService<ProtocolDefinition>>().ImplementedBy<ReplacementProtocolDefenitionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ResolutionDefinition>>(
              typeof(ResolutionDefinitionDomainService),
              Component.For<IDomainService<ResolutionDefinition>>().ImplementedBy<ReplacementResolutionDefenitionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IActSurveyService>(
                typeof(ActSurveyService),
                Component.For<IActSurveyService>().ImplementedBy<ZabaykalyeActSurveyService>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizens>, AppealCitsService, DomainService.AppealCitsService>();

            Container.ReplaceComponent<IDomainServiceInterceptor<AppealCits>>(
                typeof(AppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<Interceptors.AppealCitsServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.DisposalGjiStateToProsecReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGjiStateToProsecStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<InspectionGjiViol>>(
              typeof(InspectionGjiViolInterceptor),
              Component.For<IDomainServiceInterceptor<InspectionGjiViol>>().ImplementedBy<Interceptors.InspectionGjiViolInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Protocol>>(
               typeof(ProtocolServiceInterceptor),
               Component.For<IDomainServiceInterceptor<Protocol>>().ImplementedBy<Interceptors.ProtocolServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Resolution>>(
               typeof(ResolutionServiceInterceptor),
               Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<Interceptors.ResolutionServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<ActCheck>>(
               typeof(ActCheckServiceInterceptor),
               Component.For<IDomainServiceInterceptor<ActCheck>>().ImplementedBy<Interceptors.ActCheckServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<ActRemoval>>(
               typeof(ActRemovalServiceInterceptor),
               Component.For<IDomainServiceInterceptor<ActRemoval>>().ImplementedBy<Interceptors.ActRemovalServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Prescription>>(
               typeof(PrescriptionInterceptor),
               Component.For<IDomainServiceInterceptor<Prescription>>().ImplementedBy<Interceptors.PrescriptionInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiDefinitionStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ResolutionGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiDefinitionStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.PrescriptionGji.PrescriptionGjiNotificationReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ResolutionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiDefinitionStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IDisposalText>(
                typeof(DisposalText),
                Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

            Container.ReplaceComponent<IDisposalService>(
                typeof(DisposalService),
                Component.For<IDisposalService>().ImplementedBy<DomainService.Disposal.Impl.DisposalService>());

            Container.ReplaceComponent<IBaseStatementService>(
                typeof(BaseStatementService),
                Component.For<IBaseStatementService>().ImplementedBy<DomainService.Inspection.Impl.BaseStatementService>());

            Container.ReplaceComponent<IActCheckRealityObjectService>(
               typeof(ActCheckRealityObjectService),
               Component.For<IActCheckRealityObjectService>().ImplementedBy<DomainService.ActCheckRealityObjectService>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<TypeSurveyGoalInspGji>>(
                typeof(TypeSurveyGoalInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyGoalInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyGoalInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyInspFoundationGji>>(
                typeof(TypeSurveyInspFoundationGjiViewModel),
                Component.For<IViewModel<TypeSurveyInspFoundationGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyInspFoundationGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyKindInspGji>>(
                typeof(TypeSurveyKindInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyKindInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyKindInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyTaskInspGji>>(
                typeof(TypeSurveyTaskInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyTaskInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyTaskInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<ActCheckViolation>>(
                typeof(ActCheckViolationViewModel),
                Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ViewModel.ActCheckViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<PrescriptionViol>>(
                typeof(PrescriptionViolViewModel),
                Component.For<IViewModel<PrescriptionViol>>().ImplementedBy<ViewModel.PrescriptionViolViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ProtocolViolation>>(
                typeof(ProtocolViolationViewModel),
                Component.For<IViewModel<ProtocolViolation>>().ImplementedBy<ViewModel.ProtocolViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<Protocol>>(
                typeof(ProtocolViolationViewModel),
                Component.For<IViewModel<Protocol>>().ImplementedBy<ProtocolViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<Resolution>>(
                typeof(ResolutionViewModel),
                Component.For<IViewModel<Resolution>>().ImplementedBy<ViewModel.ResolutionViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ActRemovalViolation>>(
                typeof(ActRemovalViolationViewModel),
                Component.For<IViewModel<ActRemovalViolation>>().ImplementedBy<ViewModel.ActRemovalViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof (ProtocolGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<NotificationProtocolStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof (ProtocolGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<NotificationOfVerificationStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiNotificationStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
            typeof(ActSurveyGjiReport),
            Component.For<IGkhBaseReport>().ImplementedBy<Report.ActSurveyGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<TypeSurveyGji>>(
                typeof(TypeSurveyGjiViewModel),
                Component.For<IViewModel<TypeSurveyGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyGjiViewModel>().LifestyleTransient());

            Container.ReplaceComponent<IDomainServiceInterceptor<TypeSurveyGji>>(
                typeof(TypeSurveyGjiInterceptor),
                Component.For<IDomainServiceInterceptor<TypeSurveyGji>>().ImplementedBy<Interceptors.Dict.TypeSurveyGjiInterceptor>().LifestyleTransient());

            Container.ReplaceTransient<IReminderService, ReminderService, DomainService.Reminder.Impl.ReminderService>();

            Container.ReplaceTransient<IProtocolDefinitionService, ProtocolDefinitionService, ZabaykalyeProtocolDefinitionService>();

            Container.ReplaceTransient<IResolutionDefinitionService, ResolutionDefinitionService, ZabaykalyeResolutionDefinitionService>();
        }

        private void RegisterInspectionRules()
        {
            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ResolutionToProtocolRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeResolutionToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ProtocolToResolutionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeProtocolToResolutionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseDispHeadToDisposalRule),
                Component.For<IInspectionGjiRule>().ImplementedBy<ZabaykalyeBaseDispHeadToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseJurPersonToDisposalRule),
                Component.For<IInspectionGjiRule>().ImplementedBy<ZabaykalyeBaseJurPersonToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseProsClaimToDisposalRule),
                           Component.For<IInspectionGjiRule>().ImplementedBy<ZabaykalyeBaseProsClaimToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseStatementToDisposalRule),
                           Component.For<IInspectionGjiRule>().ImplementedBy<ZabaykalyeBaseStatementToDisposalRule>().LifeStyle.Transient);

            Container.RegisterTransient<IDocumentGjiRule, ZabaykalyeDisposalToActViewRule>();

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActCheckByRoRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeDisposalToActCheckByRoRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActCheckRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeDisposalToActCheckRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActCheckToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeActCheckToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActCheckToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeActCheckToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActRemovalToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeActRemovalToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActRemovalToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeActRemovalToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActCheckPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyeDisposalToActCheckPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(PrescriptionToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<ZabaykalyePrescriptionToProtocolRule>().LifeStyle.Transient);

            Container.RegisterTransient<IInspectionGjiRule, ZabaykalyeBasePlanActionToActCheckRule>();
        }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }
    }
}