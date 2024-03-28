namespace Bars.GkhGji.Regions.Stavropol
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Stavropol.Controller;
    using Bars.GkhGji.Regions.Stavropol.Controller.ResolPros;
    using Bars.GkhGji.Regions.Stavropol.DomainService.ActCheck;
    using Bars.GkhGji.Regions.Stavropol.DomainService.ActCheck.Impl;
    using Bars.GkhGji.Regions.Stavropol.DomainService.ResolPros;
    using Bars.GkhGji.Regions.Stavropol.DomainService.ResolPros.Impl;
    using Bars.GkhGji.Regions.Stavropol.Entities;
    using Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities;
    using Bars.GkhGji.Regions.Stavropol.Interceptors;
    using Bars.GkhGji.Regions.Stavropol.NumberRule;
    using Bars.GkhGji.Regions.Stavropol.NumberRule.Impl;
    using Bars.GkhGji.Regions.Stavropol.Report;
    using Bars.GkhGji.Regions.Stavropol.Report.DisposalGji;
    using Bars.GkhGji.Regions.Stavropol.Report.Notification;
    using Bars.GkhGji.Regions.Stavropol.Report.PrescriptionGji;
    using Bars.GkhGji.Regions.Stavropol.Report.ProtocolGji;
    using Bars.GkhGji.Regions.Stavropol.Report.ResolPros;
    using Bars.GkhGji.Regions.Stavropol.Report.ResolutionGji;
    using Bars.GkhGji.Regions.Stavropol.StateChange;
    using Bars.GkhGji.Regions.Stavropol.ViewModel;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.StateChange;

    using Castle.MicroKernel.Registration;

    using ActCheckDefinitionInterceptor = Bars.GkhGji.Regions.Stavropol.Interceptors.ActCheckDefinitionInterceptor;
    using PrescriptionCancelInterceptor = Bars.GkhGji.Regions.Stavropol.Interceptors.PrescriptionCancelInterceptor;
    using ResolProsDefinition = Bars.GkhGji.Regions.Stavropol.Entities.ResolProsDefinition;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.Register(
                Component.For<IResourceManifest>().Named("GkhGji.Regions.Stavropol resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

            this.Container.RegisterSingleton<IPermissionSource, PermissionMap>();

            this.RegisterControllers();

            this.ReplaceComponent();

            this.RegisterServices();

            this.RegisterRules();

            this.RegisterInterceptors();

            this.RegisterViewModels();

            this.RegisterReports();

            this.RegisterExecuteActions();

            this.RegisterFormatDataExport();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<ActCheckTimeController>();
            this.Container.RegisterController<ResolProsDefinitionController>();
            this.Container.ReplaceController<StavropolResolProsController>("resolpros");
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IActCheckTimeService, ActCheckTimeService>();
            this.Container.RegisterTransient<IResolProsDefinitionService, ResolProsDefinitionService>();
            Container.RegisterTransient<IViewCollection, GkhGjiStavropolViewCollection>("GkhGjiStavropolViewCollection");
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<StavropolResolPros, ResolProsStavropolServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckDefinition, ActCheckDefinitionInterceptor>();
            this.Container.ReplaceComponent<IDomainServiceInterceptor<ProtocolDefinition>>(
                typeof(GkhGji.Interceptors.ProtocolDefinitionInterceptor),
                Component.For<IDomainServiceInterceptor<ProtocolDefinition>>().ImplementedBy<Interceptors.ProtocolDefinitionInterceptor>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IDomainServiceInterceptor<ResolutionDefinition>>(
                typeof(GkhGji.Interceptors.ResolutionDefinitionInterceptor),
                Component.For<IDomainServiceInterceptor<ResolutionDefinition>>().ImplementedBy<Interceptors.ResolutionDefinitionInterceptor>().LifeStyle.Transient);

            this.Container.RegisterDomainReadInterceptor<LogImport, LogImportReadInterceptor>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<StavropolResolPros, ResolProsStavropolViewModel>();
            this.Container.RegisterViewModel<ResolProsDefinition, ResolProsDefinitionViewModel>();
            this.Container.ReplaceComponent<IViewModel<ActCheckDefinition>>(
                typeof(GkhGji.ViewModel.ActCheckDefinitionViewModel),
                Component.For<IViewModel<ActCheckDefinition>>().ImplementedBy<ActCheckDefinitionViewModel>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IViewModel<ProtocolDefinition>>(
                typeof(GkhGji.ViewModel.ProtocolDefinitionViewModel),
                Component.For<IViewModel<ProtocolDefinition>>().ImplementedBy<ProtocolDefinitionViewModel>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IViewModel<ResolutionDefinition>>(
                typeof(GkhGji.DomainService.ResolutionDefinitionViewModel),
                Component.For<IViewModel<ResolutionDefinition>>().ImplementedBy<ViewModel.ResolutionDefinitionViewModel>().LifeStyle.Transient);
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IGkhBaseReport, ActCheckNotificationStimulReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ResolutionNotificationReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ResolProsGjiDefinitionStimulReport>();
            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<StavropolActCheckGjiDefinitionReport>().LifeStyle.Transient);
        }

        private void RegisterRules()
        {
            this.Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Stavropol.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);
            this.Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Stavropol.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);
            this.Container.RegisterTransient<IDefinitionNumberRule, DefinitionNumberRule>();
        }

        private void RegisterExecuteActions()
        {
            this.Container.RegisterExecutionAction<StoredProcedureExecutionAction>();
        }

        private void ReplaceComponent()
        {
            this.Container.ReplaceComponent<IDomainService<ResolPros>>(
                typeof(ResolProsDomainService),
                Component.For<IDomainService<ResolPros>>()
                         .ImplementedBy<DomainService.ResolProsDomainService>()
                         .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDisposalText>(
               typeof(GkhGji.TextValues.DisposalText),
               Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

            this.Container.ReplaceComponent<IBaseStatementAction>(
                typeof(InspectionAction.BaseStatementAction),
                Component.For<IBaseStatementAction>().ImplementedBy<BaseStatementAction>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.DisposalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGji.DisposalGjiStimulReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.PrescriptionGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.PrescriptionNotificationReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.ProtocolGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiDefinitionStimulReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.PrescriptionGjiStimulReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.ResolutionGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiDefinitionStimulReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.DisposalGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiNotificationStimulReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.PrescriptionGjiCancelReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiCancelStimulReport>());

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.ActCheckGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheckGji.ActCheckGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(ActCheckValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<ActCheckValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(ActRemovalValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<ActRemovalValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(ActSurveyValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<ActSurveyValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(DisposalValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<DisposalValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(PrescriptionValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<PrescriptionValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(PresentationValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<PresentationValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(ProtocolValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<ProtocolValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(ResolProsValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<ResolProsValidationNumberRuleStavropol>().LifeStyle.Transient);
            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(ResolutionValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<ResolutionValidationNumberRuleStavropol>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IActCheckService>(typeof(ActCheckService),
                Component.For<IActCheckService>().ImplementedBy<DomainService.Impl.ActCheckService>().LifestyleTransient());

            this.Container.ReplaceComponent<IDocumentGjiRule>(
                typeof(DisposalToActSurveyRule),
                Component.For<IDocumentGjiRule>().ImplementedBy<StavrDisposalToActSurveyRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(GkhGji.StateChange.InspectionValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.StavropolInspectionValidationRule>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IProtocolDefinitionService>(
                typeof(Bars.GkhGji.DomainService.ProtocolDefinitionService),
                Component.For<IProtocolDefinitionService>().ImplementedBy<Bars.GkhGji.Regions.Stavropol.DomainService.ProtocolDefinitionService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IResolutionDefinitionService>(
                typeof(Bars.GkhGji.DomainService.ResolutionDefinitionService),
                Component.For<IResolutionDefinitionService>().ImplementedBy<Bars.GkhGji.Regions.Stavropol.DomainService.ResolutionDefinitionService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<ActCheck>>(
                typeof(GkhGji.ViewModel.ActCheckViewModel),
                Component.For<IViewModel<ActCheck>>().ImplementedBy<ActCheckViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionCancel>>(
                typeof(GkhGji.Interceptors.PrescriptionCancelInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelInterceptor>().LifeStyle.Transient);
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.RegisterExportableEntity<AddContragentExportableEntity>();

            ContainerHelper.ReplaceExportableEntity<KapremDecisionsExportableEntity, FormatDataExport.ExportableEntities.KapremDecisionsExportableEntity>();
            ContainerHelper.ReplaceExportableEntity<NpaExportableEntity, FormatDataExport.ExportableEntities.NpaExportableEntity>();
            ContainerHelper.ReplaceExportableEntity<KvarExportableEntity, FormatDataExport.ExportableEntities.KvarExportableEntity>();
            ContainerHelper.ReplaceExportableEntity<Bars.Gkh.FormatDataExport.ExportableEntities.Impl.PkrExportableEntity,
                FormatDataExport.ExportableEntities.PkrExportableEntity>();
            ContainerHelper.ReplaceExportableEntity<Bars.Gkh.FormatDataExport.ExportableEntities.Impl.PkrDocExportableEntity,
                FormatDataExport.ExportableEntities.PkrDocExportableEntity>();
            ContainerHelper.ReplaceExportableEntity<Bars.Gkh.FormatDataExport.ExportableEntities.Impl.PkrDocFilesExportableEntity,
                FormatDataExport.ExportableEntities.PkrDocFilesExportableEntity>();
        }
    }
}