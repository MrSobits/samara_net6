namespace Bars.GkhGji.Regions.Khakasia
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
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Navigation;
    using Bars.GkhGji.Regions.Khakasia.Controllers;
    using Bars.GkhGji.Regions.Khakasia.Controllers.ActCheck;
    using Bars.GkhGji.Regions.Khakasia.DomainService;
    using Bars.GkhGji.Regions.Khakasia.DomainService.Impl;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    using Bars.GkhGji.Regions.Khakasia.Entities.AppealCits;
    using Bars.GkhGji.Regions.Khakasia.ExecutionAction;
    using Bars.GkhGji.Regions.Khakasia.InspectionRules;
    using Bars.GkhGji.Regions.Khakasia.InspectionRules.DocumentRules;
    using Bars.GkhGji.Regions.Khakasia.Interceptors;
    using Bars.GkhGji.Regions.Khakasia.Permissions;
    using Bars.GkhGji.Regions.Khakasia.Report;
    using Bars.GkhGji.Regions.Khakasia.Report.ActRemovalGJI;
    using Bars.GkhGji.Regions.Khakasia.Report.ProtocolGji;
    using Bars.GkhGji.Regions.Khakasia.StateChange;
    using Bars.GkhGji.Regions.Khakasia.ViewModel;
    using Bars.GkhGji.Regions.Khakasia.ViewModel.AppealCits;
    using Bars.GkhGji.Regions.Khakasia.Import;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.StateChange;
    using Bars.GkhGji.ViewModel;

    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Khakasia.LogMap.Provider;

    using Castle.MicroKernel.Registration;

    using DisposalTypeSurveyInterceptor = Bars.GkhGji.Regions.Khakasia.Interceptors.DisposalTypeSurveyInterceptor;
    using ProtocolArticleLawInterceptor = Bars.GkhGji.Regions.Khakasia.Interceptors.ProtocolArticleLawInterceptor;
    using ResolProsDefinition = Bars.GkhGji.Regions.Khakasia.Entities.ResolProsDefinition;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>()
                         .Named("GkhGji.Regions.Khakasia resources")
                         .ImplementedBy<ResourceManifest>()
                         .LifeStyle.Transient);

            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Khakasia statefulentity");

            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Khakasia.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Khakasia.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);

            Container.RegisterImport<AppealsImport>(AppealsImport.Id);

            Container.UsingForResolved<IMenuItemText>((container, menuItemText) =>
            {
                menuItemText.Override("Проверки по обращениям граждан", "Проверки по обращениям и заявлениям");
                menuItemText.Override("Инспекционные проверки", "Инспекционные обследования");
            });

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiPermissionMap>());

            Container.ReplaceComponent<IPermissionSource>(typeof(Bars.GkhGji.Permissions.GkhGjiActSurveyOwnerPermissionMap),
                Component.For<IPermissionSource>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Permissions.GkhGjiActSurveyOwnerPermissionMap>().LifeStyle.Transient);
            
            Container.RegisterExecutionAction<ClearFinalStateDocRemindersAction>();

            Container.RegisterExecutionAction<SetFixedForDpkrStatusAction>();

            Container.RegisterNavigationProvider<KhakasiaDocGjiRegisterMenuProvider>();

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
            //Container.RegisterAltDataController<SurveySubject>();

            // Заменяем контроллер потому что сущность перекрыта новой сущностью
            Container.ReplaceController<KhakasiaActSurveyController>("actsurvey");
            Container.ReplaceController<KhakasiaProtocolDefinitionController>("protocoldefinition");
            Container.ReplaceController<KhakasiaResolutionDefinitionController>("resolutiondefinition");

            Container.ReplaceController<ActCheckController>("actcheck");
            Container.ReplaceController<KhakasiaProtocolController>("protocol");
            Container.ReplaceController<ActCheckRealityObjectController>("actcheckrealityobject");
            Container.ReplaceController<KhakasiaResolutionController>("resolution");
        }

        private void RegisterService()
        {
            Container.RegisterTransient<IAppealCitsExecutantService, AppealCitsExecutantService>();
            Container.RegisterTransient<IDefinitionService, DefinitionService>();
            Container.RegisterTransient<IDisposalSurveySubjectService, DisposalSurveySubjectService>();
            Container.RegisterTransient<IDisposalControlMeasuresService, DisposalControlMeasuresService>();
            Container.RegisterTransient<IViolationGroupService, ViolationGroupService>();
            
            Container.ReplaceComponent<IActCheckService>(typeof(ActCheckService),
                      Component.For<IActCheckService, IKhakasiaActCheckService>().ImplementedBy<KhakasiaActCheckService>().LifeStyle.Transient);
        }

        private void RegisterInterceptors()
        {
            Container.Register(Component.For<IDomainServiceInterceptor<AppealCitsExecutant>>().ImplementedBy<AppealCitsExecutantInterceptor>().LifeStyle.Transient);
            Container.RegisterDomainInterceptor<Disposal, Interceptors.DisposalServiceInterceptor>();
            Container.RegisterDomainInterceptor<KhakasiaResolutionDefinition, ResolutionDefInterceptor>();
            Container.RegisterDomainInterceptor<KhakasiaProtocolDefinition, ProtocolDefInterceptor>();
            Container.RegisterDomainInterceptor<ResolProsDefinition, ResolProsDefInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckDefinition, ActCheckDefInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMhcDefinition, ProtocolMhcDefInterceptor>();
            Container.RegisterDomainInterceptor<KhakasiaActSurvey, KhakasiaActSurveyInterceptor>();
            Container.RegisterDomainInterceptor<DisposalTypeSurvey, DisposalTypeSurveyInterceptor>();
            Container.RegisterDomainInterceptor<DocumentViolGroup, DocumentViolGroupInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolArticleLaw, ProtocolArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckRealityObject, Bars.GkhGji.Regions.Khakasia.Interceptors.ActCheckRealityObjectInterceptor>();
        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<ActSurveyConclusion, ActSurveyConclusionViewModel>();
            Container.RegisterViewModel<DisposalControlMeasures, DisposalControlMeasuresViewModel>();
            Container.RegisterViewModel<AppealCitsExecutant, AppealCitsExecutantViewModel>();
            Container.RegisterViewModel<ResolProsDefinition, ResolProsDefinitionViewModel>();
            Container.RegisterViewModel<KhakasiaActSurvey, KhakasiaActSurveyViewModel>();
            Container.RegisterViewModel<DisposalSurveySubject, DisposalSurveySubjectViewModel>();
            Container.RegisterTransient<IViewModel<CheckTimeChange>, CheckTimeChangeViewModel>();
			
            Container.RegisterViewModel<DocumentViolGroup, DocumentViolGroupViewModel>();

            Container.RegisterViewModel<KhakasiaProtocolDefinition, KhakasiaProtocolDefinitionViewModel>();
            Container.RegisterViewModel<KhakasiaResolutionDefinition, KhakasiaResolutionDefinitionViewModel>();
        }
        
        private void RegisterStateChangeRules()
        {
            Container.Register(Component.For<IStateChangeHandler>().Named("GkhGji.Regions.Khakasia DocumentGjiStateChangeHandler").ImplementedBy<DocumentGjiStateChangeHandler>().LifeStyle.Transient);

            Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActSurveyValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, DisposalValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PresentationValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolProsValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationNumberRule>();

            Container.ReplaceComponent<IRuleChangeStatus>(typeof(Bars.GkhGji.StateChange.AppealCitsValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.StateChange.AppealCitsValidationRule>().LifeStyle.Transient);
        }

        private void RegisterDomainServices()
        {
            Container.Register(Component.For<IDomainService<KhakasiaProtocolDefinition>>().ImplementedBy<FileStorageDomainService<KhakasiaProtocolDefinition>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<KhakasiaResolutionDefinition>>().ImplementedBy<FileStorageDomainService<KhakasiaResolutionDefinition>>().LifeStyle.Transient);

            Container.Register(Component.For<IDomainService<ActSurveyConclusion>>().ImplementedBy<FileStorageDomainService<ActSurveyConclusion>>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Protocol>>(typeof(GkhGji.DomainService.ProtocolDomainService),
                Component.For<IDomainService<Protocol>>().ImplementedBy<DomainService.ProtocolDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Resolution>>(typeof(GkhGji.DomainService.ResolutionDomainService),
                Component.For<IDomainService<Resolution>>().ImplementedBy<DomainService.ResolutionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IJurPersonService>(typeof (GkhGji.DomainService.JurPersonService),
                Component.For<IJurPersonService>().ImplementedBy<DomainService.JurPersonService>().LifeStyle.Transient);

            Container.ReplaceComponent<IPersonInspectionService>(typeof(GkhGji.DomainService.PersonInspectionService),
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
        }

        private void ReplaceComponents()
        {
            Container.ReplaceComponent<IDomainService<ActSurvey>>(
              typeof(GkhGji.DomainService.ActSurveyDomainService),
              Component.For<IDomainService<ActSurvey>>().ImplementedBy<ReplacementActSurveyDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ProtocolDefinition>>(
              typeof(GkhGji.DomainService.ProtocolDefinitionDomainService),
              Component.For<IDomainService<ProtocolDefinition>>().ImplementedBy<ReplacementProtocolDefenitionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ResolutionDefinition>>(
              typeof(GkhGji.DomainService.ResolutionDefinitionDomainService),
              Component.For<IDomainService<ResolutionDefinition>>().ImplementedBy<ReplacementResolutionDefenitionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IActSurveyService>(
                typeof(Bars.GkhGji.DomainService.ActSurveyService),
                Component.For<IActSurveyService>().ImplementedBy<DomainService.KhakasiaActSurveyService>().LifeStyle.Transient);

             this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizens>, GkhGji.DomainService.AppealCitsService, DomainService.AppealCitsService>();

            Container.ReplaceComponent<IDomainServiceInterceptor<AppealCits>>(
                typeof(GkhGji.Interceptors.AppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<Interceptors.AppealCitsServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<InspectionGjiViol>>(
              typeof(GkhGji.Interceptors.InspectionGjiViolInterceptor),
              Component.For<IDomainServiceInterceptor<InspectionGjiViol>>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Interceptors.InspectionGjiViolInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Protocol>>(
               typeof(GkhGji.Interceptors.ProtocolServiceInterceptor),
               Component.For<IDomainServiceInterceptor<Protocol>>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Interceptors.ProtocolServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Resolution>>(
               typeof(GkhGji.Interceptors.ResolutionServiceInterceptor),
               Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Interceptors.ResolutionServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<ActCheck>>(
               typeof(GkhGji.Interceptors.ActCheckServiceInterceptor),
               Component.For<IDomainServiceInterceptor<ActCheck>>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Interceptors.ActCheckServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<ActRemoval>>(
               typeof(GkhGji.Interceptors.ActRemovalServiceInterceptor),
               Component.For<IDomainServiceInterceptor<ActRemoval>>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Interceptors.ActRemovalServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Prescription>>(
               typeof(GkhGji.Interceptors.PrescriptionInterceptor),
               Component.For<IDomainServiceInterceptor<Prescription>>().ImplementedBy<Bars.GkhGji.Regions.Khakasia.Interceptors.PrescriptionInterceptor>().LifeStyle.Transient);

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
                typeof(Bars.GkhGji.Report.DisposalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.PrescriptionGji.PrescriptionGjiNotificationReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ResolutionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ResolutionGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiDefinitionReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IDisposalText>(
                typeof(GkhGji.TextValues.DisposalText),
                Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

            Container.ReplaceComponent<IDisposalService>(
                typeof(DisposalService),
                Component.For<IDisposalService>().ImplementedBy<DomainService.Disposal.Impl.DisposalService>());

            Container.ReplaceComponent<IBaseStatementService>(
                typeof(BaseStatementService),
                Component.For<IBaseStatementService>().ImplementedBy<DomainService.Inspection.Impl.BaseStatementService>());

            Container.ReplaceComponent<IActCheckRealityObjectService>(
               typeof(GkhGji.DomainService.ActCheckRealityObjectService),
               Component.For<IActCheckRealityObjectService>().ImplementedBy<DomainService.ActCheckRealityObjectService>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<TypeSurveyGoalInspGji>>(
                typeof(Bars.GkhGji.ViewModel.TypeSurveyGoalInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyGoalInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyGoalInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyInspFoundationGji>>(
                typeof(Bars.GkhGji.ViewModel.TypeSurveyInspFoundationGjiViewModel),
                Component.For<IViewModel<TypeSurveyInspFoundationGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyInspFoundationGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyKindInspGji>>(
                typeof(Bars.GkhGji.ViewModel.TypeSurveyKindInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyKindInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyKindInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyTaskInspGji>>(
                typeof(Bars.GkhGji.ViewModel.TypeSurveyTaskInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyTaskInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyTaskInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<ActCheckViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActCheckViolationViewModel),
                Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ViewModel.ActCheckViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<PrescriptionViol>>(
                typeof(Bars.GkhGji.ViewModel.PrescriptionViolViewModel),
                Component.For<IViewModel<PrescriptionViol>>().ImplementedBy<ViewModel.PrescriptionViolViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ProtocolViolation>>(
                typeof(Bars.GkhGji.ViewModel.ProtocolViolationViewModel),
                Component.For<IViewModel<ProtocolViolation>>().ImplementedBy<ViewModel.ProtocolViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<Protocol>>(
                typeof(Bars.GkhGji.ViewModel.ProtocolViolationViewModel),
                Component.For<IViewModel<Protocol>>().ImplementedBy<ViewModel.ProtocolViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<Resolution>>(
                typeof(Bars.GkhGji.DomainService.ResolutionViewModel),
                Component.For<IViewModel<Resolution>>().ImplementedBy<ViewModel.ResolutionViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ActRemovalViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActRemovalViolationViewModel),
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
            typeof(GkhGji.Report.ActSurveyGjiReport),
            Component.For<IGkhBaseReport>().ImplementedBy<Report.ActSurveyGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<TypeSurveyGji>>(
                typeof(Bars.GkhGji.ViewModel.TypeSurveyGjiViewModel),
                Component.For<IViewModel<TypeSurveyGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyGjiViewModel>().LifestyleTransient());

            Container.ReplaceComponent<IDomainServiceInterceptor<TypeSurveyGji>>(
                typeof(TypeSurveyGjiInterceptor),
                Component.For<IDomainServiceInterceptor<TypeSurveyGji>>().ImplementedBy<Interceptors.Dict.TypeSurveyGjiInterceptor>().LifestyleTransient());

            Container.ReplaceTransient<IReminderService, ReminderService, DomainService.Reminder.Impl.ReminderService>();

            Container.ReplaceTransient<IProtocolDefinitionService, ProtocolDefinitionService, DomainService.KhakasiaProtocolDefinitionService>();

            Container.ReplaceTransient<IResolutionDefinitionService, ResolutionDefinitionService, DomainService.KhakasiaResolutionDefinitionService>();
        }

        private void RegisterInspectionRules()
        {
            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ResolutionToProtocolRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaResolutionToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ProtocolToResolutionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaProtocolToResolutionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseDispHeadToDisposalRule),
                Component.For<IInspectionGjiRule>().ImplementedBy<KhakasiaBaseDispHeadToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseJurPersonToDisposalRule),
                Component.For<IInspectionGjiRule>().ImplementedBy<KhakasiaBaseJurPersonToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseProsClaimToDisposalRule),
                           Component.For<IInspectionGjiRule>().ImplementedBy<KhakasiaBaseProsClaimToDisposalRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IInspectionGjiRule>(typeof(BaseStatementToDisposalRule),
                           Component.For<IInspectionGjiRule>().ImplementedBy<KhakasiaBaseStatementToDisposalRule>().LifeStyle.Transient);

            Container.RegisterTransient<IDocumentGjiRule, KhakasiaDisposalToActViewRule>();

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActCheckByRoRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaDisposalToActCheckByRoRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActCheckRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaDisposalToActCheckRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActCheckToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaActCheckToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActCheckToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaActCheckToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActRemovalToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaActRemovalToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActRemovalToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaActRemovalToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActCheckPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaDisposalToActCheckPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(PrescriptionToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<KhakasiaPrescriptionToProtocolRule>().LifeStyle.Transient);

            Container.RegisterTransient<IInspectionGjiRule, KhakasiaBasePlanActionToActCheckRule>();
        }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }
    }
}