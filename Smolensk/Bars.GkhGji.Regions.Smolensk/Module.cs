namespace Bars.GkhGji.Regions.Smolensk
{
    using System;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;

    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.AddressPattern;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Smolensk.Controllers;
    using Bars.GkhGji.Regions.Smolensk.Controllers.Disposal;
    using Bars.GkhGji.Regions.Smolensk.DomainService;
    using Bars.GkhGji.Regions.Smolensk.DomainService.Disposal;
    using Bars.GkhGji.Regions.Smolensk.DomainService.Disposal.Impl;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Smolensk.Entities.Disposal;
    using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;
    using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;
    using Bars.GkhGji.Regions.Smolensk.ExecutionAction;
    using Bars.GkhGji.Regions.Smolensk.InspectionRules;
    using Bars.GkhGji.Regions.Smolensk.Interceptors;
    using Bars.GkhGji.Regions.Smolensk.Interceptors.Prescription;
    using Bars.GkhGji.Regions.Smolensk.Report;
    using Bars.GkhGji.Regions.Smolensk.Report.Prescription;
    using Bars.GkhGji.Regions.Smolensk.Report.Protocol;
    using Bars.GkhGji.Regions.Smolensk.Report.ProtocolGji;
    using Bars.GkhGji.Regions.Smolensk.StateChange;
    using Bars.GkhGji.Regions.Smolensk.ViewModel;
    using Bars.GkhGji.Regions.Smolensk.ViewModel.AppealCits;
    using Bars.GkhGji.Regions.Smolensk.ViewModel.Disposal;
    using Bars.GkhGji.Regions.Smolensk.ViewModel.Protocol;
    using Bars.GkhGji.Regions.Smolensk.ViewModel.Resolution;
    using Bars.GkhGji.Report;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            Container.RegisterSingleton<IPermissionSource, PermissionMap>();

            RegisterControllers();

            RegisterReports();

            RegisterViewModels();

            RegisterReplacements();

            RegisterInspectionRules();

            RegisterDomainService();

            RegisterInterceptors();

            this.RegisterAuditLogMap();

            Container.RegisterExecutionAction<ClearFinalStateDocRemindersAction>();

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Smolensk.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Smolensk.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);

            Container.RegisterTransient<IRuleChangeStatus, ActCheckSmolenskNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ActRemovalSmolenskNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, DisposalSmolenskNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, PrescriptionSmolenskNumberRule>();
            Container.RegisterTransient<IRuleChangeStatus, ProtocolSmolenskNumberTatRule>();
            Container.RegisterTransient<IRuleChangeStatus, ResolutionSmolenskNumberTatRule>();

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

            Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(GkhGji.StateChange.InspectionValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.InspectionValidationRule>().LifeStyle.Transient);

            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<INavigationProvider, NavigationProvider>();
        }

        private void RegisterInterceptors()
        {
            Container.RegisterDomainInterceptor<AppealCits, AppealCitsInterceptor>();
            Container.RegisterDomainInterceptor<AppealCitsAnswer, AppealCitsAnswerInterceptor>();
            Container.RegisterTransient<IDomainServiceInterceptor<ProtocolSmol>, ProtocolSmolInterceptor>();
            Container.RegisterTransient<IDomainServiceInterceptor<ProtocolDefinitionSmol>, ProtocolDefinitionSmolInterceptor>();
            Container.RegisterTransient<IDomainServiceInterceptor<ResolutionDefinitionSmol>, ResolutionDefinitionSmolInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckSmol, ActCheckSmolServiceInterceptor>();
            Container.RegisterDomainInterceptor<ActSurveySmol, ActSurveySmolInterceptor>();

            Container.RegisterDomainInterceptor<ActCheckRealityObject, ActCheckRealityObjectInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionViol, PrescriptionViolInterceptor>();

            Container
                .ReplaceTransient<
                    IDomainServiceInterceptor<ActRemoval>,
                    GkhGji.Interceptors.ActRemovalServiceInterceptor,
                    ActRemovalServiceInterceptor>();


            Container.ReplaceComponent<IDomainServiceInterceptor<Prescription>>(
               typeof(GkhGji.Interceptors.PrescriptionInterceptor),
               Component.For<IDomainServiceInterceptor<Prescription>>().ImplementedBy<Bars.GkhGji.Regions.Smolensk.Interceptors.PrescriptionInterceptor>().LifeStyle.Transient);

            Container.RegisterDomainInterceptor<DocumentViolGroup, DocumentViolGroupInterceptor>();

            Container.RegisterDomainInterceptor<ActRemovalSmol, ActRemovalSmolInterceptor>();

        }

        private void RegisterDomainService()
        {
            Container.RegisterTransient<IViolationGroupService, ViolationGroupService>();

            Container.ReplaceComponent<IDomainService<PrescriptionCancel>>(
                typeof(GkhGji.DomainService.PrescriptionCancelDomainService),
                Component.For<IDomainService<PrescriptionCancel>>()
                         .ImplementedBy<DomainService.PrescriptionCancelDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ProtocolDefinition>>(
                typeof(GkhGji.DomainService.ProtocolDefinitionDomainService),
                Component.For<IDomainService<ProtocolDefinition>>()
                         .ImplementedBy<DomainService.ProtocolDefinitionDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ResolutionDefinition>>(
                typeof(GkhGji.DomainService.ResolutionDefinitionDomainService),
                Component.For<IDomainService<ResolutionDefinition>>()
                         .ImplementedBy<DomainService.ResolutionDefinitionDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Disposal>>(
                typeof(GkhGji.DomainService.DisposalDomainService),
                Component.For<IDomainService<Disposal>>()
                         .ImplementedBy<DomainService.DisposalSmolDomainService>()
                         .LifeStyle.Transient);


            Container.ReplaceComponent<IDomainService<Protocol>>(
                typeof(GkhGji.DomainService.ProtocolDomainService),
                Component.For<IDomainService<Protocol>>()
                         .ImplementedBy<DomainService.ProtocolSmolDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ActSurvey>>(
                typeof(GkhGji.DomainService.ActSurveyDomainService),
                Component.For<IDomainService<ActSurvey>>()
                         .ImplementedBy<DomainService.ActSurveySmolDomainService>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<ActRemoval>>(
              typeof(GkhGji.DomainService.ActRemovalDomainService),
              Component.For<IDomainService<ActRemoval>>()
                       .ImplementedBy<DomainService.ActRemovalSmolDomainService>()
                       .LifeStyle.Transient);

            Container.RegisterDomainService<ProtocolSmol, ProtocolSmolenskDomainService>();

            Container.ReplaceComponent<IDomainService<ActCheck>>(
                typeof(GkhGji.DomainService.ActCheckDomainService),
                Component.For<IDomainService<ActCheck>>()
                         .ImplementedBy<DomainService.ActCheckSmolDomainService>()
                         .LifeStyle.Transient);
            
            Container.RegisterTransient<IDisposalSurveySubjectService, DisposalSurveySubjectService>();

        }

        private void RegisterInspectionRules()
        {
            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.ProtocolToResolutionRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskProtocolToResolutionRule>().LifeStyle.Transient);
            
            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.ResolutionToProtocolRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskResolutionToProtocolRule>().LifeStyle.Transient);
            
            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.DisposalToActCheckWithoutRoRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskDisposalToActCheckWithoutRoRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.DisposalToActCheckByRoRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskDisposalToActCheckByRoRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.ActCheckToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskActCheckToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.ActRemovalToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskActRemovalToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(Bars.GkhGji.InspectionRules.DisposalToActCheckPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<SmolenskDisposalToActCheckPrescriptionRule>().LifeStyle.Transient);
        }

        private void RegisterReplacements()
        {
            Container.ReplaceComponent<IViewModel<TypeSurveyGoalInspGji>>(
                typeof(Bars.GkhGji.ViewModel.TypeSurveyGoalInspGjiViewModel),
                Component.For<IViewModel<TypeSurveyGoalInspGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyGoalInspGjiViewModel>());

            Container.ReplaceComponent<IViewModel<TypeSurveyInspFoundationGji>>(
               typeof(Bars.GkhGji.ViewModel.TypeSurveyInspFoundationGjiViewModel),
               Component.For<IViewModel<TypeSurveyInspFoundationGji>>().ImplementedBy<ViewModel.Dict.TypeSurveyInspFoundationGjiViewModel>());
            
            Container.ReplaceComponent<IViewModel<ActCheckViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActCheckViolationViewModel),
                Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ViewModel.ActCheckViolationViewModel>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<ActRemovalViolation>>(
                typeof(Bars.GkhGji.ViewModel.ActRemovalViolationViewModel),
                Component.For<IViewModel<ActRemovalViolation>>().ImplementedBy<ViewModel.ActRemovalViolationViewModel>().LifeStyle.Transient);
        
            Container.ReplaceComponent<IDisposalText>(
                typeof(GkhGji.TextValues.DisposalText),
                Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

            Container.ReplaceComponent(
                typeof(IAppealCitsNumberRule),
                typeof(Bars.GkhGji.NumberRule.AppealCitsNumberRuleTat),
                Component
                    .For<IAppealCitsNumberRule>()
                    .ImplementedBy<Bars.GkhGji.Regions.Smolensk.NumberRule.AppealCitsNumberRuleEmpty>()
                    .LifestyleTransient());
            
            Container.ReplaceComponent<IDomainServiceInterceptor<Resolution>>(
              typeof(GkhGji.Interceptors.ResolutionServiceInterceptor),
              Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<ResolutionServiceInterceptor>().LifeStyle.Transient);
            
            Container.ReplaceComponent<IDomainServiceInterceptor<InspectionGjiViol>>(
               typeof(GkhGji.Interceptors.InspectionGjiViolInterceptor),
               Component.For<IDomainServiceInterceptor<InspectionGjiViol>>().ImplementedBy<InspectionGjiViolInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IActCheckRealityObjectService>(
               typeof(Bars.GkhGji.DomainService.ActCheckRealityObjectService),
               Component.For<IActCheckRealityObjectService>().ImplementedBy<DomainService.ActCheckRealityObjectService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDisposalService>(
               typeof(Bars.GkhGji.DomainService.DisposalService),
               Component.For<IDisposalService>().ImplementedBy<DomainService.Disposal.Impl.DisposalService>());

            Container.ReplaceComponent<IBaseStatementService>(
                typeof(Bars.GkhGji.DomainService.BaseStatementService),
                Component.For<IBaseStatementService>().ImplementedBy<DomainService.Inspection.Impl.BaseStatementService>());

            Container.ReplaceComponent<IDomainService<Resolution>>(
                typeof(Bars.GkhGji.DomainService.ResolutionDomainService),
               Component.For<IDomainService<Resolution>>().ImplementedBy<DomainService.ResolutionDomainService>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ProtocolGjiReport), 
                Component.For<IGkhBaseReport>().ImplementedBy<Report.Protocol.ProtocolGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.DisposalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGji.DisposalGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheck.ActCheckGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActSurveyGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ActSurveyGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ResolutionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<NoticeOfProtocolStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActRemovalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActRemoval.ActRemovalStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ActCheckGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiDefinitionReport>().LifeStyle.Transient);

            Replace<IGkhBaseReport, ProtocolGjiDefinitionReport, ProtocolDefinitionStimulReport>(x => x.LifestyleTransient());
            Replace<IGkhBaseReport, PrescriptionGjiCancelReport, PrescriptionCancelReport>(x => x.LifestyleTransient());
            Replace<IGkhBaseReport, ResolutionGjiDefinitionReport, ResolutionDefinitionStimulReport>(x => x.LifestyleTransient());

            Container.ReplaceComponent<IViewModel<PrescriptionViol>>(
                typeof(GkhGji.ViewModel.PrescriptionViolViewModel),
                Component.For<IViewModel<PrescriptionViol>>().ImplementedBy<ViewModel.PrescriptionViolViewModel>().LifestyleTransient());

            Container.ReplaceComponent<IViewModel<ActCheckRealityObject>>(
                typeof(GkhGji.ViewModel.ActCheckRealityObjectViewModel),
                Component.For<IViewModel<ActCheckRealityObject>>().ImplementedBy<ViewModel.ActCheckRealityObjectViewModel>().LifestyleTransient());

            Container.ReplaceTransient<IReminderService, ReminderService, DomainService.Reminder.Impl.ReminderService>();
            Container.ReplaceTransient<IProtocolDefinitionService, Bars.GkhGji.DomainService.ProtocolDefinitionService, DomainService.ProtocolDefinitionService>();
        }

        private void RegisterControllers()
        {
            Container.RegisterController<DocumentViolGroupController>();

            // данные контроллеры заменяет базовый поскольку сущности расширились новыми иполями
            Container.ReplaceController<ProtocolSmolController>("protocol");
            Container.ReplaceController<PrescriptionCancelController>("prescriptioncancel");
            Container.ReplaceController<ProtocolDefinitionController>("protocoldefinition");
            Container.ReplaceController<ResolutionDefinitionController>("resolutiondefinition");
            Container.ReplaceController<DisposalSmolController>("disposal");
            Container.ReplaceController<ActCheckSmolController>("actcheck");
            Container.ReplaceController<SmolenskActSurveyController>("actsurvey");
            Container.ReplaceController<SmolenskResolutionController>("resolution");

            Container.ReplaceController<ActCheckRealityObjectController>("actcheckrealityobject");
            Container.ReplaceController<PrescriptionViolController>("prescriptionviol");
            //Container.RegisterAltDataController<SurveySubject>();
            Container.RegisterController<DisposalSurveySubjectController>();
        }

        private void RegisterReports()
        {
            Container.RegisterTransient<IGkhBaseReport, PrescriptionCancelStimulReport>();
            Container.RegisterTransient<IGkhBaseReport, ActCheckSignNotificationReport>();
            Container.RegisterTransient<IGkhBaseReport, ActRemovalSignNotificationReport>();
            Container.RegisterTransient<IGkhBaseReport, ConsiderationAnAdministrativeCaseNotificationReport>();

            Container.RegisterTransient<IPrintForm, AppealCitsJurnalAccounting>("GkhGji Report.AppealCitsJurnalAccounting");
            Container.RegisterTransient<IPrintForm, AdministrativeOffensesJurnalReport>("GkhGji Report.AdministrativeOffensesJurnalReport");
            Container.RegisterTransient<IPrintForm, ScheduledInspectionSurveysJournal>("GkhGji Report.ScheduledInspectionSurveysJournal");
            Container.RegisterTransient<IPrintForm, RegistrationOutgoingDocuments>("GkhGji Report.RegistrationOutgoingDocuments");
            Container.RegisterTransient<IPrintForm, AdministrativeOffensesResolution>("GkhGji Report.AdministrativeOffensesResolution");
            Container.RegisterTransient<IPrintForm, PrescriptionRegistrationJournal>("GkhGji Report.PrescriptionRegistrationJournal");
        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<DocumentViolGroup, DocumentViolGroupViewModel>();
            Container.RegisterTransient<IViewModel<CheckTimeChange>, CheckTimeChangeViewModel>();
            Container.RegisterViewModel<PrescriptionCancelSmol, PrescriptionCancelSmolViewModel>();
            Container.RegisterViewModel<ProtocolDefinitionSmol, ProtocolDefinitionSmolViewModel>();
            Container.RegisterViewModel<ResolutionDefinitionSmol, ResolutionDefinitionSmolViewModel>();

            Replace<IViewModel<Resolution>, GkhGji.DomainService.ResolutionViewModel, ViewModel.ResolutionViewModel>(x => x.LifestyleTransient());

            Container.RegisterViewModel<ProtocolSmol, ProtocolSmolViewModel>();
            Container.RegisterViewModel<DisposalSmol, DisposalSmolViewModel>();
            Container.RegisterViewModel<ActCheckSmol, ActCheckSmolViewModel>();
            Container.RegisterViewModel<DisposalSurveySubject, DisposalSurveySubjectViewModel>();
            Container.RegisterViewModel<ActSurveySmol, ActSurveySmolViewModel>();
            Container.RegisterViewModel<ActRemovalSmol, ActRemovalSmolViewModel>();
        }

        /*private static ComponentRegistration<T> RegisterTransient<T, TImpl>() where TImpl : class, T where T : class
        {
            return Component.For<T>().ImplementedBy<TImpl>().LifestyleTransient();
        }*/

        private void Replace<TInterface, TImpl1, TImpl2>(Func<ComponentRegistration<TInterface>, ComponentRegistration<TInterface>> lifestyle = null)
            where TImpl2 : class, TInterface
            where TImpl1 : class, TInterface
            where TInterface : class
        {
            var registration = Component.For<TInterface>().ImplementedBy<TImpl2>();

            if (lifestyle != null)
            {
                registration = lifestyle(registration);
            }

            Container.ReplaceComponent<TInterface>(typeof (TImpl1), registration);
        }

        public void RegisterAuditLogMap()
        {
        }

    }
}