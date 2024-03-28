namespace Bars.GkhGji.Regions.Tomsk
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.NumberRule;
    using Bars.GkhGji.Regions.Tomsk.Controller;
    using Bars.GkhGji.Regions.Tomsk.Controllers;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.DomainService.ActCheck;
    using Bars.GkhGji.Regions.Tomsk.DomainService.ActCheck.Impl;
    using Bars.GkhGji.Regions.Tomsk.DomainService.Impl;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
    using Bars.GkhGji.Regions.Tomsk.Export;
    using Bars.GkhGji.Regions.Tomsk.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Interceptors;
    using Bars.GkhGji.Regions.Tomsk.Interceptors.Dict;
    using Bars.GkhGji.Regions.Tomsk.Navigation;
    using Bars.GkhGji.Regions.Tomsk.NumberRule;
    using Bars.GkhGji.Regions.Tomsk.Report;
    using Bars.GkhGji.Regions.Tomsk.Report.ResolutionGji;
    using Bars.GkhGji.Regions.Tomsk.Rules;
    using Bars.GkhGji.Regions.Tomsk.StateChange;
    using Bars.GkhGji.Regions.Tomsk.ViewModel;
    using Bars.GkhGji.Regions.Tomsk.ViewModel.Protocol;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.Rules;
    using Bars.GkhGji.StateChange;
    using Castle.MicroKernel.Registration;
    using Gkh.Domain;
    using Report.ProtocolGji;
	using ActCheckRealityObjectInterceptor = Bars.GkhGji.Regions.Tomsk.Interceptors.ActCheckRealityObjectInterceptor;
	using ActCheckServiceInterceptor = Bars.GkhGji.Regions.Tomsk.Interceptors.ActCheckServiceInterceptor;
	using AppealCitsAnswerInterceptor = Bars.GkhGji.Regions.Tomsk.Interceptors.AppealCitsAnswerInterceptor;
	using AppealCitsServiceInterceptor = Bars.GkhGji.Regions.Tomsk.Interceptors.AppealCitsServiceInterceptor;
	using BaseStatementAppealCitsServiceInterceptor = Bars.GkhGji.Regions.Tomsk.Interceptors.BaseStatementAppealCitsServiceInterceptor;
	using PrescriptionInterceptor = Bars.GkhGji.Regions.Tomsk.Interceptors.PrescriptionInterceptor;
	using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Controller.AppealCits;
	using Bars.GkhGji.Regions.Tomsk.DomainService.AppealCits;
	using Bars.GkhGji.Regions.Tomsk.DomainService.AppealCits.Impl;
	using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;
	using Bars.GkhGji.Regions.Tomsk.ExecutionAction;
	using Bars.GkhGji.Regions.Tomsk.Interceptors.AppealCits;
    using Bars.GkhGji.Regions.Tomsk.Report.AdministrativeCase;
    using Bars.GkhGji.Regions.Tomsk.ViewModel.AppealCits;
    using Bars.GkhGji.ViewModel;

    using GkhGji.Entities.Dict;
    using Report.Licensing;

    using AppealCitsService = Bars.GkhGji.DomainService.AppealCitsService;
    using ResolProsDefinition = Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition;
    using ResolProsDefinitionViewModel = Bars.GkhGji.Regions.Tomsk.ViewModel.ResolPros.ResolProsDefinitionViewModel;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterPermissionMap<GkhGjiTomskPermissionMap>();
            this.Container.RegisterNavigationProvider<NavigationProvider>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGji.Regions.Tomsk resources");

            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>();
			this.Container.RegisterTransient<IViewCollection, GkhGjiTomskViewCollection>("GkhGjiTomskViewCollection");

            this.RegisterInspectionRules();

            this.RegisterControllers();

            this.RegisterDomainServices();

            this.RegisterExports();

            this.RegisterReplacements();

            this.RegisterServices();

            this.RegisterViewModels();

            this.RegisterInterceptors();

            this.RegisterOther();

            this.RegisterReports();
        }
        
        private void RegisterInspectionRules()
        {
            // Регистрируем провайдер правил
            this.Container.ReplaceComponent<IInspectionGjiProvider>(
                typeof(InspectionGjiProvider),
                Component.For<IInspectionGjiProvider>().ImplementedBy<TomskInspectionProvider>());

            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseJurPersonToDisposalRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseDispHeadToDisposalRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseProsClaimToDisposalRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseStatementToDisposalRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseStatementToDisposalLicensingRule>();

            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseStatementToActVisualRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseDispHeadActVisualRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseStatementToAdminCaseRule>();

            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseLicenseAppToActVisualRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseLicenseAppToAdminCaseRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TomskBaseLicenseAppToDisposalRule>();

            // правила распоряжения
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskDisposalToActCheckHaveNotViolRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskDisposalToActCheckHaveViolRule>().LifeStyle.Transient);
            // пока убрал это правило потом чт опеределаллся процесс гжи томска тепрь у них только одно правило на предписани
            //this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskDisposalToPrescriptionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskDisposalToPrescriptionByViolationRule>().LifeStyle.Transient);

            // Предписание
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskPrescriptionToDisposalRule>().LifeStyle.Transient);

            // Акт проверки
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskActCheckToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskActCheckToProtocolRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskActCheckToAdminCaseRule>().LifeStyle.Transient);

            // Протокол
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskProtocolToResolutionRule>().LifeStyle.Transient);

            // Постановление прокуратуры
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskResolProsToResolutionRule>().LifeStyle.Transient);

            // Постановление
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskResolutionToProtocolRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskResolutionToAdminCaseRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskResolutionToPresentationRule>().LifeStyle.Transient);

            // Акт визуального осмотра
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskActVisualToAdminCaseRule>().LifeStyle.Transient);

            // Административное дело
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskAdminCaseToPrescriptionlRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskAdminCaseToProtocolRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TomskAdminCaseToResolutionRule>().LifeStyle.Transient);

            // Правила создания напоминания
            this.Container.Register(Component.For<IReminderRule>().ImplementedBy<AppealCitsReminderRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IReminderRule>().ImplementedBy<InspectionReminderRule>().LifeStyle.Transient);
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<RequirementController>();
            this.Container.RegisterController<RequirementDocumentController>();
            this.Container.RegisterController<RequirementArticleLawController>();
            this.Container.RegisterAltDataController<TomskArticleLawGji>();
            this.Container.RegisterController<AdministrativeCaseController>();
            this.Container.RegisterController<AdministrativeCaseArticleLawController>();
            this.Container.RegisterController<AdministrativeCaseProvidedDocController>();
            this.Container.RegisterController<AdministrativeCaseViolController>();

            this.Container.RegisterController<FileStorageDataController<AdministrativeCaseAnnex>>();
            this.Container.RegisterAltDataController<AdministrativeCaseDoc>();

            this.Container.RegisterController<RegionSpecificController>();
            this.Container.RegisterController<ActCheckTimeController>();
            this.Container.RegisterController<DisposalProvidedDocNumController>();
            this.Container.RegisterController<KindCheckGjiSpecController>();
            
            this.Container.RegisterController<DisposalVerificationSubjectLicensingController>();
            this.Container.RegisterController<ActCheckVerificationResultController>();

            this.Container.RegisterAltDataController<TypeSurveyGjiIssue>();

            this.Container.RegisterAltDataController<PrescriptionRealityObject>();

            this.Container.RegisterController<ActVisualController>();

            this.Container.RegisterController<TypeRequirementController>();

            this.Container.RegisterAltDataController<FrameVerification>();

            this.Container.RegisterAltDataController<ResolProsDefinition>();

            this.Container.RegisterController<DisposalViolationsController>();

            this.Container.RegisterAltDataController<ActCheckFamiliarized>();

            this.Container.RegisterController<AppealCitsAnswerAddresseeController>();
			this.Container.RegisterController<AppealCitsExecutantController>();
			this.Container.ReplaceController<TomskAppealCitsController>("appealcits");
        }

        private void RegisterReplacements()
        {
            this.Container.ReplaceController<Controllers.ReminderController>("reminder");
            this.Container.ReplaceController<CompetentOrgGjiController>("competentorggji");
            this.Container.ReplaceController<ProtocolDefinitionController>("protocoldefinition");
            this.Container.ReplaceController<ResolutionController>("resolution");
            this.Container.ReplaceController<ViolationGjiController>("violationgji");

            this.Container.ReplaceController<ProtocolController>("protocol");
            this.Container.ReplaceController<Controller.ActCheckRealityObjectController>("actcheckrealityobject");
            this.Container.ReplaceController<PresentationController>("presentation");

            this.Container.ReplaceComponent<IViewModel<InspectionGjiViolStage>>(
               typeof(Bars.GkhGji.ViewModel.InspectionViolStageViewModel),
               Component.For<IViewModel<InspectionGjiViolStage>>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.ViewModel.InspectionViolStageViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IPrescriptionService>(
                typeof(Bars.GkhGji.DomainService.PrescriptionService),
                Component.For<IPrescriptionService>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.DomainService.PrescriptionService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IReminderService>(
                typeof(Bars.GkhGji.DomainService.ReminderService),
                Component.For<IReminderService>().ImplementedBy<DomainService.ReminderService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IForm1ContolServiceData>(
                typeof(Bars.GkhGji.Report.Form1ContolServiceData),
                Component.For<IForm1ContolServiceData>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.Report.Form1ControlServiceData>().LifeStyle.Transient);

            this.Container.ReplaceComponent<Bars.GkhGji.Report.IMonthlyProsecutorsOfficeServiceData>(
                typeof(Bars.GkhGji.Report.MonthlyProsecutorsOfficeServiceData),
                Component.For<Bars.GkhGji.Report.IMonthlyProsecutorsOfficeServiceData>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.Report.MonthlyProsecutorsOfficeServiceData>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IProtocolService>(typeof(ProtocolService), Component.For<IProtocolService>().ImplementedBy<TomskProtocolService>());

            this.Container.ReplaceComponent<IDisposalViolService>(
                typeof(GkhGji.DomainService.DisposalViolService),
                Component.For<IDisposalViolService>().ImplementedBy<DomainService.Impl.DisposalViolService>());

            this.Container.ReplaceComponent<IPrescriptionViolService>(
                typeof(GkhGji.DomainService.PrescriptionViolService),
                Component.For<IPrescriptionViolService>().ImplementedBy<DomainService.Impl.PrescriptionViolService>());

            this.Container.ReplaceComponent<IProtocolViolationService>(
                typeof(GkhGji.DomainService.ProtocolViolationService),
                Component.For<IProtocolViolationService>().ImplementedBy<DomainService.Impl.ProtocolViolationService>());


            this.Container.ReplaceComponent<IDisposalText>(typeof(GkhGji.TextValues.DisposalText), Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

            this.Container.ReplaceComponent<IBaseStatementAction>(typeof(InspectionAction.BaseStatementAction), Component.For<IBaseStatementAction>().ImplementedBy<BaseStatementAction>());

            this.Container.ReplaceComponent<IGkhBaseReport>(typeof(ResolutionGjiReport), Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiStimulReport>().LifeStyle.Transient);

            /*this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ProtocolGji.ProtocolGjiReport>().LifeStyle.Transient);*/

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.ProtocolGjiDefinitionReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiDefinitionStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ProtocolGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ProtocolGjiNotificationReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(typeof(DisposalGjiReport), Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGji.DisposalGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGji.DisposalGjiNotificationReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(DisposalGjiStateToProsecReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGji.DisposalGjiStateToProsecReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiNotificationReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.PrescriptionGji.PrescriptionGjiNotificationReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(typeof(PrescriptionGjiReport), Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(typeof(ProtocolGjiReport), Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IGkhBaseReport>(typeof(ActCheckGjiReport), Component.For<IGkhBaseReport>().ImplementedBy<ActCheckGjiStimulReport>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IAppealCitsNumberRule>(typeof(AppealCitsNumberRuleTat), Component.For<IAppealCitsNumberRule>().ImplementedBy<AppealCitsNumberRuleTomsk>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<InspectionAppealCits>>(
                typeof(GkhGji.Interceptors.BaseStatementAppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<InspectionAppealCits>>().ImplementedBy<BaseStatementAppealCitsServiceInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<AppealCitsAnswer>>(
                typeof(GkhGji.Interceptors.AppealCitsAnswerInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCitsAnswer>>().ImplementedBy<AppealCitsAnswerInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IBaseStatementService>(
                typeof(Bars.GkhGji.DomainService.BaseStatementService),
                Component.For<IBaseStatementService>().ImplementedBy<DomainService.Impl.BaseStatementService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<ActCheck>>(
                typeof(Bars.GkhGji.ViewModel.ActCheckViewModel),
                Component.For<IViewModel<ActCheck>>().ImplementedBy<ViewModel.ActCheckViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IViewModel<Disposal>>(
                typeof(Bars.GkhGji.ViewModel.DisposalViewModel),
                Component.For<IViewModel<Disposal>>().ImplementedBy<ViewModel.DisposalViewModel>().LifeStyle.Transient);

            this.Container.ReplaceComponent<INavigationProvider>(
                typeof(GkhGji.Navigation.DocumentsGjiRegisterMenuProvider),
                Component.For<INavigationProvider>().ImplementedBy<DocumentsGjiRegisterMenuProvider>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizens>, AppealCitsService, DomainService.AppealCitsService>();

            this.Container.ReplaceComponent<IDomainService<ViewBaseStatement>>(
                typeof(GkhGji.DomainService.ViewBaseStatementDomainService),
                Component.For<IDomainService<ViewBaseStatement>>().ImplementedBy<DomainService.ViewBaseStatementDomainService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<Protocol>>(
                typeof (GkhGji.DomainService.ProtocolDomainService),
                Component.For<IDomainService<TomskProtocol>>()
                    .ImplementedBy<DomainService.ProtocolDomainService>()
                    .LifestyleTransient());

            this.Container.ReplaceComponent<IViewModel<Protocol>>(
                typeof(GkhGji.ViewModel.ProtocolViewModel),
                Component.For<IViewModel<TomskProtocol>>()
                    .ImplementedBy<TomskProtocolViewModel>()
                    .LifestyleTransient());

            this.Container.ReplaceComponent<IResolutionDefinitionService>(
                typeof(Bars.GkhGji.DomainService.ResolutionDefinitionService),
                Component.For<IResolutionDefinitionService>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.DomainService.ResolutionDefinitionService>().LifeStyle.Transient);

            this.Container.ReplaceComponent<GkhGji.DomainService.IProtocolDefinitionService>(
                typeof(Bars.GkhGji.DomainService.ProtocolDefinitionService),
                Component.For<GkhGji.DomainService.IProtocolDefinitionService>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.DomainService.ProtocolDefinitionService>().LifeStyle.Transient);
        
          this.Container.ReplaceComponent<GkhGji.DomainService.IInspectionMenuService>(
                typeof(Bars.GkhGji.DomainService.InspectionMenuService),
                Component.For<GkhGji.DomainService.IInspectionMenuService>().ImplementedBy<Bars.GkhGji.Regions.Tomsk.DomainService.InspectionMenuService>().LifeStyle.Transient);

		  this.Container.ReplaceComponent<IDomainService<AppealCits>>(
			 typeof(FileStorageDomainService<AppealCits>),
			 Component.For<IDomainService<AppealCits>>()
					  .ImplementedBy<ReplacementDomainService<AppealCits, TomskAppealCits>>()
					  .LifeStyle.Transient);
		}

        private void RegisterServices()
        {
            
            this.Container.RegisterExecutionAction<ReminderRuleAction>();
			this.Container.RegisterExecutionAction<ActualizeDisposalViolationsAction>();
			this.Container.RegisterExecutionAction<MigrateAppealCitsExecutantInformationAction>();

            this.Container.RegisterTransient<ITomskReminderService, TomskReminderService>();
            this.Container.RegisterTransient<ICompetentOrgGjiService, CompetentOrgGjiService>();
            this.Container.RegisterTransient<IRegionSpecificService, RegionSpecificService>();
            this.Container.RegisterTransient<IActCheckTimeService, ActCheckTimeService>();
            this.Container.RegisterTransient<IDisposalProvidedDocNumService, DisposalProvidedDocNumService>();
            this.Container.RegisterTransient<IDisposalVerificationSubjectLicensingService, DisposalVerificationSubjectLicensingService>();
            this.Container.RegisterTransient<IActCheckVerificationResultService, ActCheckVerificationResultService>();
            this.Container.RegisterTransient<IKindCheckGjiService, KindCheckGjiService>();
            this.Container.RegisterTransient<IActVisualService, ActVisualService>();
            this.Container.RegisterTransient<IAdministrativeCaseArticleLawService, AdministrativeCaseArticleLawService>();
            this.Container.RegisterTransient<ITypeRequirementService, TypeRequirementService>();
            this.Container.RegisterTransient<IAdminCaseProvidedDocService, AdminCaseProvidedDocService>();
            this.Container.RegisterTransient<IAdminCaseService, AdminCaseService>();
            this.Container.RegisterTransient<IDisposalViolationsService, DisposalViolationsService>();
            this.Container.RegisterTransient<IRequirementDocumentService, RequirementDocumentService>();

            this.Container.RegisterTransient<IRequirementService, RequirementService>();
            this.Container.RegisterTransient<IRequirementArticleLawService, RequirementArticleLawService>();
            this.Container.RegisterTransient<IAppealCitsAnswerAddressee, AppealCitsAnswerAddresseeService>();
            this.Container.RegisterTransient<IProtocolDefinitionDefaultParamsService, ProtocolDefinitionDefaultParamsService>();

            /*this.Container.RegisterTransient<IProtocolDescriptionService, ProtocolDescriptionService>();
            this.Container.RegisterTransient<IActCheckRealityObjectDescriptionService, ActCheckRealityObjectDescriptionService>();
            this.Container.RegisterTransient<IResolutionDescriptionService, ResolutionDescriptionService>();*/

            this.Container.RegisterTransient<IDocumentPhysInfoService, DocumentPhysInfoService>();

            this.Container.RegisterTransient<IAdminCaseViolService, AdminCaseViolService>();

            // виды проверки
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<TomskDispNotPlanDocumentationRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<TomskDispPlanDocumentationRule>().LifeStyle.Transient);
        }
        
        private void RegisterDomainServices()
        {
            this.Container.RegisterDomainService<AdministrativeCaseAnnex, FileStorageDomainService<AdministrativeCaseAnnex>>();
            this.Container.RegisterDomainService<Requirement, FileStorageDomainService<Requirement>>();

            this.Container.ReplaceComponent<IDomainService<ProtocolDefinition>>(
               typeof(GkhGji.DomainService.ProtocolDefinitionDomainService),
               Component.For<IDomainService<ProtocolDefinition>>()
                        .ImplementedBy<GkhGji.Regions.Tomsk.DomainService.ReplacementProtocolDefinitionDomainService>()
                        .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<Resolution>>(
               typeof(GkhGji.DomainService.ResolutionDomainService),
               Component.For<IDomainService<Resolution>>()
                        .ImplementedBy<GkhGji.Regions.Tomsk.DomainService.ReplacementResolutionDomainService>()
                        .LifeStyle.Transient);

			this.Container.RegisterTransient<IAppealCitsExecutantService, AppealCitsExecutantService>();
        }

        private void RegisterOther()
        {
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActSurveyValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PresentationValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolProsValidationNumberRule>();
            
            // В томске номера постановлений заполняются в ручную
            // this.Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DocGjiValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActVisualValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, RequirementStateChangeRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AdminCaseValidationNumberRule>();

            //замена татарстанской проверки заполненности полей
            this.Container.ReplaceComponent(
                typeof (IRuleChangeStatus),
                typeof (GkhGji.StateChange.ActCheckValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.ActCheckValidationRule>().LifestyleTransient());
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<DisposalVerificationSubjectLicensing, DisposalVerificationSubjectLicensingViewModel>();
            this.Container.RegisterViewModel<ActCheckVerificationResult, ActCheckVerificationResultViewModel>();
            this.Container.RegisterViewModel<TypeSurveyGjiIssue, TypeSurveyGjiIssueViewModel>();
            this.Container.RegisterViewModel<ResolProsDefinition, ResolProsDefinitionViewModel>();
            this.Container.RegisterViewModel<Requirement, RequirementViewModel>();
            this.Container.RegisterViewModel<AdministrativeCase, AdministrativeCaseViewModel>();
            this.Container.RegisterViewModel<AdministrativeCaseAnnex, AdministrativeCaseAnnexViewModel>();
            this.Container.RegisterViewModel<AdministrativeCaseArticleLaw, AdministrativeCaseArticleLawViewModel>();
            this.Container.RegisterViewModel<AdministrativeCaseProvidedDoc, AdministrativeCaseProvidedDocViewModel>();
            this.Container.RegisterViewModel<AdministrativeCaseDoc, AdministrativeCaseDocViewModel>();
            this.Container.RegisterViewModel<ActCheckFamiliarized, ActCheckFamiliarizedViewModel>();
            this.Container.RegisterViewModel<AppealCitsAnswerAddressee, AppealCitsAnswerAddresseeViewModel>();
            this.Container.RegisterViewModel<TomskResolution, TomskResolutionViewModel>();
            this.Container.RegisterViewModel<TomskProtocolDefinition, TomskProtocolDefinitionViewModel>();
            this.Container.RegisterViewModel<AdministrativeCaseViolation, AdministrativeCaseViolViewModel>();
            this.Container.RegisterViewModel<TomskArticleLawGji, TomskArticleLawGjiViewModel>();
            this.Container.RegisterViewModel<TomskViolationGji, ViolationGjiViewModel<TomskViolationGji>>();
			this.Container.RegisterViewModel<TomskAppealCits, TomskAppealCitsViewModel>();
			this.Container.RegisterViewModel<AppealCitsExecutant, AppealCitsExecutantViewModel>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<ActCheck, ActCheckServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<ActVisual, ActVisualnterceptor>();

            this.Container.RegisterDomainInterceptor<Prescription, PrescriptionInterceptor>();

            this.Container.RegisterDomainInterceptor<Requirement, RequirementServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<AdministrativeCase, AdministrativeCaseInterceptor>();

            this.Container.RegisterDomainInterceptor<AdministrativeCaseDoc, AdministrativeCaseDocInterceptor>();

            this.Container.RegisterDomainInterceptor<PrescriptionViol, PrescriptionViolationServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<TomskProtocol, TomskProtocolServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<TomskResolution, TomskResolutionServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<Disposal, TomskDisposalServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<TomskProtocolDefinition, TomskProtocolDefinitionInterceptor>();

            this.Container.RegisterDomainInterceptor<ActCheckRealityObject, ActCheckRealityObjectInterceptor>();

            this.Container.RegisterDomainInterceptor<TomskViolationGji, TomskViolationGjiInterceptor>();

            this.Container.RegisterDomainInterceptor<DisposalViolation, DisposalViolInterceptor>();

            this.Container.RegisterDomainInterceptor<AdministrativeCaseViolation, AdministrativeCaseViolInterceptor>();

			this.Container.RegisterDomainInterceptor<TomskAppealCits, AppealCitsServiceInterceptor>();
			this.Container.RegisterDomainInterceptor<AppealCitsExecutant, AppealCitsExecutantInterceptor>();
			this.Container.RegisterDomainInterceptor<SurveySubjectLicensing, SurveySubjectgLicensinInterceptor>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, ActVisualDataExport>("ActVisualDataExport");
            this.Container.RegisterTransient<IDataExportService, AdminCaseDataExport>("AdminCaseDataExport");
        }

	    private void RegisterReports()
	    {
	        this.Container.RegisterTransient<ITomskDisposalReportData, TomskDisposalReportData>();

	        this.Container.RegisterTransient<IGkhBaseReport, RequirementGjiStimulReport>();
	        this.Container.RegisterTransient<IGkhBaseReport, AdministrativeCaseStimulReport>();
            this.Container.RegisterTransient<IGkhBaseReport, AdministrativeCaseStimulReportExtensionDate>();
            this.Container.RegisterTransient<IGkhBaseReport, AdministrativeCaseStimulReportReclamationInfo>();
            this.Container.RegisterTransient<IGkhBaseReport, AdministrativeCaseDocStimulReport>();
	        this.Container.RegisterTransient<IGkhBaseReport, ResolutionGjiDefinition>();
	        this.Container.RegisterTransient<IGkhBaseReport, AppealCitsAnswerReport>();
	        this.Container.RegisterTransient<IGkhBaseReport, ActVisualStimulReport>();

	        this.Container.RegisterTransient<IPrintForm, AppealCitsWorkingReport>("Report Bars.Gji AppealCitsWorkingReport");

	        this.Container.ReplaceTransient<IGkhBaseReport, PresentationGjiReport, PresentationStimulReport>();
	        this.Container.RegisterTransient<IGkhBaseReport, ReasonedOfferReport>();
	        this.Container.RegisterTransient<IGkhBaseReport, ClaimOfTerminationActionLicenseReport>();
	        this.Container.RegisterTransient<IGkhBaseReport, ListOfDocumentsReport>();
	    }
    }
}