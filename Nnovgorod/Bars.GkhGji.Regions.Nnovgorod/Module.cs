namespace Bars.GkhGji.Regions.Nnovgorod
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Report;
    using Bars.Gkh.TextValues;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.NumberRule;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;
    using Bars.GkhGji.Regions.Nnovgorod.Entities.AppealCits;
    using Bars.GkhGji.Regions.Nnovgorod.ExecutionAction;
    using Bars.GkhGji.Regions.Nnovgorod.InspectionRules;
    using Bars.GkhGji.Regions.Nnovgorod.Interceptors;
    using Bars.GkhGji.Regions.Nnovgorod.LogMap.Provider;
    using Bars.GkhGji.Regions.Nnovgorod.NumberRule;
    using Bars.GkhGji.Regions.Nnovgorod.Permissions;
    using Bars.GkhGji.Regions.Nnovgorod.Report;
    using Bars.GkhGji.Regions.Nnovgorod.StateChange;
    using Bars.GkhGji.Regions.Nnovgorod.ViewModel;
    using Bars.GkhGji.Regions.Nnovgorod.ViewModel.AppealCits;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.StateChange;
    using Bars.GkhGji.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.UsingForResolved<IMenuItemText>((container, menuItemText) =>
                {
                    menuItemText.Override("Проверки по обращениям граждан", "Проверки по обращениям и заявлениям");
                    menuItemText.Override("Инспекционные проверки", "Инспекционные обследования");
                });

            Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGji.Regions.Nnovgorod resources");

            Container.RegisterTransient<IViewModel<CheckTimeChange>, CheckTimeChangeViewModel>();

            Container.RegisterAltDataController<CheckTimeChange>();

            Container.RegisterDomainInterceptor<AppealCits, AppealCitsInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsAnswer, AppealCitsAnswerInterceptor>();

            Container.RegisterTransient<IRuleChangeStatus, AppealNumberValidationRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActSurveyValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, DisposalValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PresentationValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolProsValidationNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationNumberRule>();

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Nnovgorod.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Nnovgorod.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);

            Container.ReplaceComponent(
                typeof(IAppealCitsNumberRule),
                typeof(AppealCitsNumberRuleTat),
                Component
                    .For<IAppealCitsNumberRule>()
                    .ImplementedBy<AppealCitsNumberRuleNNovgorod>()
                    .LifestyleTransient());

            Container.Register(Component.For<IStateChangeHandler>().Named("GkhGji.Regions.Nnovgorod DocumentGjiStateChangeHandler").ImplementedBy<DocumentGjiStateChangeHandler>().LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiPermissionMap>());

            Container.RegisterExecutionAction<ClearFinalStateDocRemindersAction>();

            Container.ReplaceComponent<IDomainServiceInterceptor<InspectionGjiViol>>(
               typeof(GkhGji.Interceptors.InspectionGjiViolInterceptor),
               Component.For<IDomainServiceInterceptor<InspectionGjiViol>>().ImplementedBy<InspectionGjiViolInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Protocol>>(
               typeof(GkhGji.Interceptors.ProtocolServiceInterceptor),
               Component.For<IDomainServiceInterceptor<Protocol>>().ImplementedBy<ProtocolServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<Resolution>>(
               typeof(GkhGji.Interceptors.ResolutionServiceInterceptor),
               Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<ResolutionServiceInterceptor>().LifeStyle.Transient);

            Container.RegisterController<FileStorageDataController<DisposalControlMeasures>>();
            Container.Register(Component.For<IDomainService<DisposalControlMeasures>>().ImplementedBy<FileStorageDomainService<DisposalControlMeasures>>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<DisposalControlMeasures>>().ImplementedBy<DisposalControlMeasuresViewModel>().LifeStyle.Transient);

            Container.RegisterTransient<IPrintForm, AppealCitsJurnalAccounting>("GkhGji Report.AppealCitsJurnalAccounting");
            Container.RegisterTransient<IPrintForm, AdministrativeOffensesJurnalReport>("GkhGji Report.AdministrativeOffensesJurnalReport");
            Container.RegisterTransient<IPrintForm, ScheduledInspectionSurveysJournal>("GkhGji Report.ScheduledInspectionSurveysJournal");
            Container.RegisterTransient<IPrintForm, RegistrationOutgoingDocuments>("GkhGji Report.RegistrationOutgoingDocuments");
            Container.RegisterTransient<IPrintForm, AdministrativeOffensesResolution>("GkhGji Report.AdministrativeOffensesResolution");
            Container.RegisterTransient<IPrintForm, PrescriptionRegistrationJournal>("GkhGji Report.PrescriptionRegistrationJournal");

            RegisterInspectionRulesReplacement();

            RegisterReportReplacement();

            RegisterViewModelReplacement();

            RegisterServiceReplacement();

            this.RegisterAuditLogMap();
        }

        private void RegisterReportReplacement()
        {

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.DisposalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.DisposalGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGjiNotificationReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.PrescriptionGji.PrescriptionGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ResolutionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ResolutionGji.ResolutionGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ProtocolGji.ProtocolGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiDefinitionReport>().LifeStyle.Transient);

            Container.Register(Component.For<IPrintForm>().Named("GJI Report.NnovgorodJournalAppeals").ImplementedBy<NnovgorodJournalAppeals>().LifeStyle.Transient);
        }

        private void RegisterServiceReplacement()
        {
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
               typeof(ActCheckRealityObjectService),
               Component.For<IActCheckRealityObjectService>().ImplementedBy<DomainService.ActCheckRealityObjectService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Protocol>>(typeof(ProtocolDomainService),
                Component.For<IDomainService<Protocol>>().ImplementedBy<DomainService.ProtocolDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Resolution>>(typeof(ResolutionDomainService),
                Component.For<IDomainService<Resolution>>().ImplementedBy<DomainService.ResolutionDomainService>().LifeStyle.Transient);
        }

        private void RegisterViewModelReplacement()
        {
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
        }

        private void RegisterInspectionRulesReplacement()
        {
            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ResolutionToProtocolRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<NnResolutionToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(DisposalToActSurveyRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<NnDisposalToActSurveyRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ProtocolToResolutionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<NnProtocolToResolutionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IRuleChangeStatus>(typeof(GkhGji.StateChange.InspectionValidationRule),
                 Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.InspectionValidationRule>().LifeStyle.Transient);
        }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }
    }
}
