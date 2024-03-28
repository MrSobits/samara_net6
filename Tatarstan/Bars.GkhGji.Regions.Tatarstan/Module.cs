namespace Bars.GkhGji.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.NH.Events;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    // TODO: Расскоментировать после перевода  GisIntegration
        /* using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Tor.Entities;
    using Bars.GisIntegration.Tor.Service.SendData;*/
    using Bars.Gkh;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Region.Tatarstan;
    using Bars.GkhGji.Regions.Tatarstan.ConfigSections;
    using Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal;
    using Bars.GkhGji.Regions.Tatarstan.Controller;
    using Bars.GkhGji.Regions.Tatarstan.Controller.ActCheck;
    using Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Controller.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Controller.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Controller.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Controller.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Controller.MotivatedPresentation;
    using Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Controller.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.Controller.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.Controller.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ConfigSections;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Decision;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Dict;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Disposal;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Inspection.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionActionIsolated.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionPreventiveAction.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji.Impl;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.View;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ConfigSections;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;
    using Bars.GkhGji.Regions.Tatarstan.Export;
    using Bars.GkhGji.Regions.Tatarstan.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules;
    using Bars.GkhGji.Regions.Tatarstan.Integration;
    using Bars.GkhGji.Regions.Tatarstan.Integration.Impl;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.ConfigSections;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Permissions;
    using Bars.GkhGji.Regions.Tatarstan.Quartz;
    using Bars.GkhGji.Regions.Tatarstan.Quartz.Impl;
    using Bars.GkhGji.Regions.Tatarstan.Report;
    using Bars.GkhGji.Regions.Tatarstan.Report.ResolutionGji;
    using Bars.GkhGji.Regions.Tatarstan.Rules;
    using Bars.GkhGji.Regions.Tatarstan.Services;
    using Bars.GkhGji.Regions.Tatarstan.Services.Impl;
    using Bars.GkhGji.Regions.Tatarstan.SchedulerTasks;
    using Bars.GkhGji.Regions.Tatarstan.StateChange;
    using Bars.GkhGji.Regions.Tatarstan.StateChange.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.StateChange.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.StateChange.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.StateChange.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.TorIntegration.Controllers;
    using Bars.GkhGji.Regions.Tatarstan.TorIntegration.Service.SendData.Impl;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.ControlList;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.Decision;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.IntegrationTor;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanResolutionGji;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.Rules;

    using Castle.MicroKernel.Registration;

    using ActCheckService = Bars.GkhGji.DomainService.ActCheckService;
    using ActCheckServiceInterceptor = Bars.GkhGji.Interceptors.ActCheckServiceInterceptor;
    using ActCheckToDisposalRule = Bars.GkhGji.InspectionRules.ActCheckToDisposalRule;
    using ActRemovalValidationRule = Bars.GkhGji.StateChange.ActRemovalValidationRule;
    using BaseStatementService = Bars.GkhGji.DomainService.BaseStatementService;
    using BaseStatementServiceInterceptor = Bars.GkhGji.Interceptors.BaseStatementServiceInterceptor;
    using DisposalDomainService = Bars.GkhGji.DomainService.DisposalDomainService;
    using DisposalService = Bars.GkhGji.DomainService.DisposalService;
    using GjiWorkReport = Bars.GkhGji.Regions.Tatarstan.Report.GjiWorkReport.GjiWorkReport;
    using InspectionValidationRule = Bars.GkhGji.StateChange.InspectionValidationRule;
    using MainViewModel = Bars.GkhGji.ViewModel;
    using NavigationProvider = Bars.GkhGji.Regions.Tatarstan.Navigation.NavigationProvider;
    using NormativeDocInterceptor = Bars.GkhGji.Interceptors.NormativeDocInterceptor;
    using PrescriptionCancelInterceptor = Bars.GkhGji.Interceptors.PrescriptionCancelInterceptor;
    using PrescriptionViolInterceptor = Bars.GkhGji.Interceptors.PrescriptionViolInterceptor;
    using ProtocolMvdServiceInterceptor = Bars.GkhGji.Regions.Tatarstan.Interceptors.ProtocolMvdServiceInterceptor;
    using ProtocolResponsibility = Bars.GkhGji.Report.ProtocolResponsibility;
    using HeatSeasonReceivedDocumentsReport = Bars.GkhGji.Report.HeatSeasonReceivedDocumentsReport;
    using ReminderService = Bars.GkhGji.DomainService.ReminderService;
    using ResolutionGjiReport = Bars.GkhGji.Report.ResolutionGjiReport;
    using ResolutionService = Bars.GkhGji.DomainService.ResolutionService;
    using ResolutionServiceInterceptor = Bars.GkhGji.Regions.Tatarstan.Interceptors.Resolution.ResolutionServiceInterceptor;
    using SurveySubjectViewModel = Bars.GkhGji.ViewModel.Dict.SurveySubjectViewModel;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IReminderRule, AppealCitsReminderRule>("GkhGji.Regions.Tatarstan.Reminder.AppealCitsReminderRule");
            this.Container.RegisterTransient<IReminderRule, InspectionReminderRule>("GkhGji.Regions.Tatarstan.Reminder.InspectionReminderRule");
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Bars.GkhGji.Regions.Tatarstan statefulentity");

            this.Container.ReplaceTransient<IReminderService, ReminderService, DomainService.ReminderService>();
            this.Container.ReplaceTransient<IBaseStatementService, BaseStatementService, DomainService.BaseStatementService>();
            this.Container.ReplaceTransient<INavigationProvider, GkhGji.Navigation.DocumentsGjiRegisterMenuProvider, DocumentsGjiRegisterMenuProvider>();

            this.Container.ReplaceTransient<IGkhConfigSection, GkhGji.ConfigSections.HousingInspection, HousingInspection>();
            this.Container.RegisterGkhConfig<AppealConfig>();

            this.RegisterCommonComponents();

            this.RegisterControllers();

            this.RegisterDomainService();

            this.RegisterStateChange();

            this.RegisterInterceptors();

            this.RegistrationInspectionRules();

            this.RegisterIntegrations();

            this.RegisterViewModels();

            this.RegistrationQuartz();

            this.RegisterReports();

            this.RegisterServices();

            this.RegisterExports();

            this.RegisterTypeCheckRules();
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGji.Regions.Tatarstan resources");
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IGjiTatParamService, GjiTatParamService>();
            this.Container.RegisterTransient<IFieldRequirementSource, GkhGjiFieldRequirementMap>();
            
            this.Container.RegisterTransient<IViewCollection, GkhGjiTatarstanViewCollection>("GkhGjiTatarstanViewCollection");

            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiPermissionMap>());
            this.RegisterNavigations();
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterAltDataController<GisGmpPatternDict>();
            this.Container.RegisterAltDataController<WarningDocViolations>();
            this.Container.RegisterAltDataController<WarningDocBasis>();
            this.Container.RegisterAltDataController<WarningBasis>();
            this.Container.RegisterAltDataController<InspectionBasis>();
            this.Container.RegisterAltDataController<TatarstanResolutionPayFine>();
            this.Container.RegisterAltDataController<WarningDocRealObj>();
            this.Container.RegisterAltDataController<EffectivenessAndPerformanceIndex>();
            this.Container.RegisterAltDataController<EffectivenessAndPerformanceIndexValue>();
            this.Container.RegisterAltDataController<MandatoryReqs>();
            this.Container.RegisterAltDataController<ControlOrganization>();
            this.Container.RegisterAltDataController<ControlOrganizationControlTypeRelation>();
            this.Container.RegisterAltDataController<ControlListTypicalAnswer>();
            this.Container.RegisterAltDataController<TatarstanControlListQuestion>();
            this.Container.RegisterAltDataController<ConfigurationReferenceInformationKndTor>();
            this.Container.RegisterAltDataController<TatarstanProtocolGjiContragent>();
            this.Container.RegisterAltDataController<TatarstanDocumentWitness>();
            this.Container.RegisterAltDataController<ProsecutorOfficeDict>();
            this.Container.RegisterAltDataController<BaseTatProtocolGji>();
            this.Container.RegisterAltDataController<TatarstanResolutionGji>();
            this.Container.RegisterAltDataController<ActCheckActionCarriedOutEvent>();
            this.Container.RegisterAltDataController<ActCheckActionFile>();
            this.Container.RegisterAltDataController<ActCheckActionInspector>();
            this.Container.RegisterAltDataController<ActCheckActionRemark>();
            this.Container.RegisterAltDataController<InstrExamAction>();
            this.Container.RegisterAltDataController<InstrExamActionNormativeDoc>();
            this.Container.RegisterAltDataController<ExplanationAction>();
            this.Container.RegisterAltDataController<SurveyAction>();
            this.Container.RegisterAltDataController<SurveyActionQuestion>();
            this.Container.RegisterAltDataController<InspectionAction>();
            this.Container.RegisterAltDataController<PreventiveActionTaskPlannedAction>();
            this.Container.RegisterAltDataController<DocRequestAction>();
            this.Container.RegisterAltDataController<DocRequestActionRequestInfo>();
            this.Container.RegisterAltDataController<ObjectivesPreventiveMeasure>();
            this.Container.RegisterAltDataController<PreventiveActionItems>();
            this.Container.RegisterAltDataController<TasksPreventiveMeasures>();
            this.Container.RegisterAltDataController<InspectorPositions>();
            this.Container.RegisterAltDataController<VisitSheetInfo>();
            this.Container.RegisterAltDataController<VisitSheetAnnex>();
            this.Container.RegisterAltDataController<MotivatedPresentationRealityObject>();
            this.Container.RegisterAltDataController<PreventiveActionTaskConsultingQuestion>();
            this.Container.RegisterAltDataController<ActActionIsolatedDefinition>();
            this.Container.RegisterAltDataController<TaskActionIsolatedAnnex>();
            this.Container.RegisterAltDataController<MotivatedPresentationAnnex>();
            this.Container.RegisterAltDataController<GjiValidityDocPeriod>();
            this.Container.RegisterAltDataController<ControlTypeInspectorPositions>();
            this.Container.RegisterAltDataController<TatRiskCategory>();
            this.Container.RegisterAltDataController<ControlObjectKind>();
            this.Container.RegisterAltDataController<ControlObjectType>();
            this.Container.RegisterAltDataController<ControlTypeRiskIndicators>();
            this.Container.RegisterAltDataController<KnmReason>();
            this.Container.RegisterAltDataController<VisitSheetViolationInfo>();
            this.Container.RegisterAltDataController<TaskActionIsolatedKnmAction>();
            this.Container.RegisterAltDataController<RapidResponseSystemAppealDetails>();

            this.Container.RegisterController<GisChargeController>();
            this.Container.RegisterController<GisGmpParamsController>();
            this.Container.RegisterController<GmpInnCheckerController>();
            this.Container.RegisterController<GisGmpPatternController>();
            this.Container.RegisterController<WarningInspectionController>();
            this.Container.RegisterController<WarningDocController>();
            this.Container.RegisterController<MotivationConclusionController>();
            // TODO: Расскоментировать после перевода  GisIntegration
            //this.Container.RegisterController<RisTaskController>();
            this.Container.RegisterController<TorIntegrationController>();
            this.Container.RegisterController<ControlListTypicalQuestionController>();
            this.Container.RegisterController<MandatoryReqsNormativeDocController>();
            this.Container.RegisterController<TatarstanProtocolGjiController>();
            this.Container.RegisterController<TatarstanProtocolGjiArticleLawController>();
            this.Container.RegisterController<TatarstanProtocolGjiRealityObjectController>();
            this.Container.RegisterController<TatarstanProtocolGjiViolationController>();
            this.Container.RegisterController<BudgetClassificationCodeController>();
            this.Container.RegisterController<ActCheckActionController>();
            this.Container.RegisterController<TaskActionIsolatedRealityObjectController>();
            this.Container.RegisterController<ActActionIsolatedController>();
            this.Container.RegisterController<TaskActionIsolatedController>();
            this.Container.RegisterController<TaskActionIsolatedItemController>();
            this.Container.RegisterController<InspectionActionIsolatedController>();
            this.Container.RegisterController<TaskOfPreventiveActionTaskController>();
            this.Container.RegisterController<MotivatedPresentationController>();
            this.Container.RegisterController<MotivatedPresentationViolationController>();
            this.Container.RegisterController<PreventiveActionTaskRegulationController>();
            this.Container.RegisterController<PreventiveActionTaskObjectiveController>();
            this.Container.RegisterController<PreventiveActionTaskItemController>();
            this.Container.RegisterController<TaskActionIsolatedSurveyPurposeController>();
            this.Container.RegisterController<TaskActionIsolatedArticleLawController>();
            this.Container.RegisterController<ActCheckActionViolationController>();
            this.Container.RegisterController<TatarstanDecisionController>();
            this.Container.RegisterController<KnmCharacterController>();
            this.Container.RegisterController<KnmTypesController>();
            this.Container.RegisterController<ErknmTypeDocumentController>();
            this.Container.RegisterController<DecisionControlObjectInfoController>();
            this.Container.RegisterController<DecisionInspectionBaseController>();
            this.Container.RegisterController<KnmActionController>();
            this.Container.RegisterController<VisitSheetController>();
            this.Container.RegisterController<VisitSheetViolationController>();
            this.Container.RegisterController<InspectionPreventiveActionController>();
            this.Container.RegisterController<RapidResponseSystemAppealController>();
            this.Container.RegisterController<PreventiveActionController>();
            this.Container.RegisterController<PreventiveActionTaskController>();
            this.Container.RegisterController<MotivatedPresentationAppealCitsController>();

            ContainerHelper.RegisterFileDataController<TatarstanProtocolGjiAnnex>();
            ContainerHelper.RegisterFileDataController<WarningDocAnnex>();
            ContainerHelper.RegisterFileDataController<MotivationConclusionAnnex>();
            ContainerHelper.RegisterFileDataController<TatarstanControlList>();
            ContainerHelper.RegisterFileDataController<TatDisposalAnnex>();
            ContainerHelper.RegisterFileDataController<MotivatedPresentationAppealCitsAnnex>();
            ContainerHelper.RegisterFileDataController<RapidResponseSystemAppealResponse>();

            this.Container.ReplaceController<ResolutionController>("resolution");
            this.Container.ReplaceController<TatarstanProtocolMvdController>("protocolmvd");
            this.Container.ReplaceController<TatarstanDisposalController>("disposal");
            this.Container.ReplaceController<TatarstanZonalInspectionController>("zonalinspection");
            this.Container.ReplaceController<PrescriptionCancelTatarstanController>("prescriptioncancel");
            this.Container.ReplaceController<Bars.GkhGji.Regions.Tatarstan.Controller.ResolPros.ResolProsTatarstanController>("resolpros");
            this.Container.ReplaceController<TatProtocolController>("protocol");
        }

        private void RegisterDomainService()
        {
            this.Container.RegisterDomainService<ViewWarningInspection, WarningInspectionDomainService>();
            this.Container.RegisterDomainService<WarningDocViolations, WarningDocViolationsDomainService>();
            this.Container.RegisterDomainService<ActCheckAction, ActCheckActionDomainService>();
            this.Container.RegisterDomainService<TatarstanDisposal, TatarstanDisposalDomainService>();
            this.Container.RegisterDomainService<InspectionBaseType, InspectionBaseTypeDomainService>();
            this.Container.RegisterDomainService<KnmTypes, KnmTypesDomainService>();
            this.Container.RegisterDomainService<KnmCharacter, KnmCharacterDomainService>();
            this.Container.RegisterDomainService<KnmAction, KnmActionDomainService>();
            this.Container.RegisterDomainService<ErknmTypeDocument, ErknmTypeDocumentService>();

            this.Container.RegisterFileStorageDomainService<WarningInspection>();
            this.Container.RegisterFileStorageDomainService<WarningDoc>();
            this.Container.RegisterFileStorageDomainService<TaskActionIsolated>();
            this.Container.RegisterFileStorageDomainService<ActCheckActionFile>();
            this.Container.RegisterFileStorageDomainService<PreventiveAction>();
            this.Container.RegisterFileStorageDomainService<ExplanationAction>();
            this.Container.RegisterFileStorageDomainService<PreventiveActionTask>();
            this.Container.RegisterFileStorageDomainService<DocRequestActionRequestInfo>();
            this.Container.RegisterFileStorageDomainService<VisitSheetAnnex>();
            this.Container.RegisterFileStorageDomainService<TaskActionIsolatedAnnex>();
            this.Container.RegisterFileStorageDomainService<MotivatedPresentationAnnex>();
            this.Container.RegisterFileStorageDomainService<KnmReason>();

            this.Container.RegisterTransient<IWarningInspectionService, WarningInspectionService>();
            this.Container.RegisterTransient<IWarningDocService, WarningDocService>();
            this.Container.RegisterTransient<IMandatoryReqsNormativeDocService, MandatoryReqsNormativeDocService>();
            this.Container.RegisterTransient<IControlListTypicalQuestionService, ControlListTypicalQuestionService>();
            this.Container.RegisterTransient<ITatarstanDisposalService, TatarstanDisposalService>();
            this.Container.RegisterTransient<ITatarstanProtocolMvdService, TatarstanProtocolMvdService>();
            this.Container.RegisterTransient<IActCheckActionService, ActCheckActionService>();
            this.Container.RegisterTransient<IInspectionActionIsolatedService, InspectionActionIsolatedService>();
            this.Container.RegisterTransient<ITaskOfPreventiveActionTaskService, TaskOfPreventiveActionTaskService>();
            this.Container.RegisterTransient<IMotivatedPresentationService, MotivatedPresentationService>();
            this.Container.RegisterTransient<ITaskActionIsolatedSurveyPurposeService, TaskActionIsolatedSurveyPurposeService>();
            this.Container.RegisterTransient<IDecisionInspBaseService, DecisionInspBaseService>();
            this.Container.RegisterTransient<IInspectionPreventiveActionService, InspectionPreventiveActionService>();

            this.Container.ReplaceComponent<IDomainService<Resolution>>(
                typeof(ResolutionDomainService),
                Component.For<IDomainService<Resolution>>()
                    .ImplementedBy<ReplacementResolutionDomainService>()
                    .LifeStyle.Transient);
            this.Container.ReplaceComponent<IDomainService<ProtocolMvd>>(
                typeof(ResolutionDomainService),
                Component.For<IDomainService<ProtocolMvd>>()
                    .ImplementedBy<ReplacementProtocolMvdDomainService>()
                    .LifeStyle.Transient);
            this.Container.ReplaceComponent<IDomainService<ResolutionPayFine>>(
                typeof(ReplacementResolutionPayFineService),
                Component.For<IDomainService<ResolutionPayFine>>()
                    .ImplementedBy<ReplacementResolutionPayFineService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<Disposal>>(
                typeof(DisposalDomainService),
                Component.For<IDomainService<Disposal>>()
                    .ImplementedBy<Bars.GkhGji.Regions.Tatarstan.DomainService.Disposal.DisposalDomainService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<ZonalInspection>>(
                typeof(ZonalInspectionService),
                Component.For<IDomainService<ZonalInspection>>()
                    .ImplementedBy<TatarstanZonalInspectionDomainService>()
                    .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainService<Protocol>>(
                typeof(ProtocolDomainService),
                Component.For<IDomainService<Protocol>>()
                    .ImplementedBy<Bars.GkhGji.Regions.Tatarstan.DomainService.Protocol.ProtocolDomainService>()
                    .LifeStyle.Transient);
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<TatarstanDisposal, TatDisposalInterceptor>();
            this.Container.RegisterDomainInterceptor<TaskDisposal, DisposalServiceInterceptor<TaskDisposal>>();
            this.Container.RegisterDomainInterceptor<WarningInspection, WarningInspectionInterceptor>();
            this.Container.RegisterDomainInterceptor<WarningDoc, WarningDocInterceptor>();
            this.Container.RegisterDomainInterceptor<MotivationConclusion, MotivationConclusionInterceptor>();
            this.Container.RegisterDomainInterceptor<WarningDocBasis, WarningDocBasisInterceptor>();
            this.Container.RegisterDomainInterceptor<TatarstanProtocolMvd, ProtocolMvdServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<TatarstanResolution, ResolutionServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<TatarstanZonalInspection, TatarstanZonalInspectionInterceptor>();
            this.Container.RegisterDomainInterceptor<MandatoryReqs, MandatoryReqsInterceptor>();
            this.Container.RegisterDomainInterceptor<ControlOrganization, ControlOrganizationInterceptor>();
            this.Container.RegisterDomainInterceptor<ControlType, ControlTypeInterceptor>();
            this.Container.RegisterDomainInterceptor<ControlListTypicalQuestion, ControlListTypicalQuestionInterceptor>();
            this.Container.RegisterDomainInterceptor<ControlListTypicalAnswer, ControlListTypicalAnswerInterceptor>();
            this.Container.RegisterDomainInterceptor<EffectivenessAndPerformanceIndex, EffectivenessAndPerformanceIndexInterceptor>();
            this.Container.RegisterDomainInterceptor<ConfigurationReferenceInformationKndTor, ConfigurationReferenceInformationKndTorInterceptor>();
            this.Container.RegisterDomainInterceptor<TatarstanProtocolGjiContragent, TatarstanProtocolGjiContragentInterceptor>();
            this.Container.RegisterDomainInterceptor<TatarstanResolutionGji, TatarstanResolutionGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<BudgetClassificationCode, BudgetClassificationCodeInterceptor>();
            this.Container.RegisterDomainInterceptor<PreventiveAction, PreventiveActionInterceptor>();
            this.Container.RegisterDomainInterceptor<PreventiveActionTask, PreventiveActionTaskInterceptor>();
            this.Container.RegisterDomainInterceptor<TaskActionIsolated, TaskActionIsolatedInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckAction, ActCheckActionInterceptor>();
            this.Container.RegisterDomainInterceptor<ActActionIsolated, ActActionIsolatedInterceptor>();
            this.Container.RegisterDomainInterceptor<InstrExamAction, InstrExamActionInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionActionIsolated, InspectionActionIsolatedInterceptor>();
            this.Container.RegisterDomainInterceptor<ExplanationAction, ExplanationActionInterceptor>();
            this.Container.RegisterDomainInterceptor<SurveyAction, SurveyActionInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionAction, InspectionActionInterceptor>();
            this.Container.RegisterDomainInterceptor<DocRequestAction, DocRequestActionInterceptor>();
            this.Container.RegisterDomainInterceptor<PreventiveActionItems, PreventiveActionItemsInterceptor>();
            this.Container.RegisterDomainInterceptor<ObjectivesPreventiveMeasure, ObjectivesPreventiveMeasuresInterceptor>();
            this.Container.RegisterDomainInterceptor<TasksPreventiveMeasures, TasksPreventiveMeasuresInterceptor>();
            this.Container.RegisterDomainInterceptor<MotivatedPresentation, MotivatedPresentationInterceptor>();
            this.Container.RegisterDomainInterceptor<VisitSheet, VisitSheetInterceptor>();
            this.Container.RegisterDomainInterceptor<VisitSheetInfo, VisitSheetInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckActionRemark, ActCheckActionRemarkInterceptor>();
            this.Container.RegisterDomainInterceptor<SurveyActionQuestion, SurveyActionQuestionInterceptor>();
            this.Container.RegisterDomainInterceptor<GjiValidityDocPeriod, GjiValidityDocPeriodInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionBaseType, InspectionBaseTypeInterceptor>();
            this.Container.RegisterDomainInterceptor<KnmTypes, KnmTypesInterceptor>();
            this.Container.RegisterDomainInterceptor<TatarstanDecision, DecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<KnmCharacter, KnmCharacterInterceptor>();
            this.Container.RegisterDomainInterceptor<TatRiskCategory, TatRiskCategoryInterceptor>();            
            this.Container.RegisterDomainInterceptor<KnmAction, KnmActionInterceptor>();
            this.Container.RegisterDomainInterceptor<ErknmTypeDocument, ErknmTypeDocumentInterceptor>();
            this.Container.RegisterDomainInterceptor<TatProtocol, TatProtocolInterceptor>();
            this.Container.RegisterDomainInterceptor<MotivatedPresentationAppealCits, MotivatedPresentationAppealCitsInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionPreventiveAction, InspectionPreventiveActionInterceptor>();
            this.Container.RegisterDomainInterceptor<VisitSheetViolationInfo, VisitSheetViolationInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<RapidResponseSystemAppeal, RapidResponseSystemAppealInterceptor>();
            this.Container.RegisterDomainInterceptor<RapidResponseSystemAppealDetails, RapidResponseSystemAppealDetailsInterceptor>();
            this.Container.RegisterDomainInterceptor<DocumentGjiPdfSignInfo, DocumentGjiPdfSignInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<Day, DayInterceptor>();
            this.Container.RegisterDomainInterceptor<DocumentGji, Interceptors.DocumentGjiInterceptor>();

            this.Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionCancel>>(
                typeof(PrescriptionCancelInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionCancel>>().ImplementedBy<Interceptors.PrescriptionCancelInterceptor>().LifeStyle
                    .Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionViol>>(
                typeof(PrescriptionViolInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionViol>>().ImplementedBy<Interceptors.PrescriptionViolInterceptor>()
                    .LifestyleTransient());

            this.Container.ReplaceComponent<IDomainServiceInterceptor<ActCheck>>(
                typeof(ActCheckServiceInterceptor),
                Component.For<IDomainServiceInterceptor<ActCheck>>().ImplementedBy<Interceptors.ActCheck.ActCheckServiceInterceptor>().LifestyleTransient());

            this.Container.ReplaceComponent<IDomainServiceInterceptor<BaseStatement>, BaseStatementServiceInterceptor>(
                Component.For<IDomainServiceInterceptor<BaseStatement>>().ImplementedBy<Interceptors.BaseStatementServiceInterceptor>()
                    .LifestyleTransient());

            this.Container.ReplaceComponent<IDomainServiceInterceptor<NormativeDoc>, NormativeDocInterceptor>(
                Component.For<IDomainServiceInterceptor<NormativeDoc>>().ImplementedBy<Interceptors.NormativeDocInterceptor>().LifestyleTransient());
        }

        private void RegisterIntegrations()
        {
            this.Container.RegisterTransient<IGisGmpIntegration, GisGmpIntegration>();
        }

        private void RegisterStateChange()
        {
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionGisChargeRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionGisChargeChangeRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionGisChargeAnnulRule>();

            this.Container.ReplaceTransient<IRuleChangeStatus, InspectionValidationRule, StateChange.InspectionValidationRule>();
            this.Container.ReplaceTransient<IRuleChangeStatus, ActRemovalValidationRule, StateChange.ActRemovalValidationRule>();

            this.Container.RegisterTransient<IRuleChangeStatus, TatarstanProtocolGjiValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, TatarstanProtocolGjiChargeSendRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, TatarstanProtocolGjiChargeChangeRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, TatarstanProtocolGjiChargeAnnulRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, TatarstanResolutionGjiValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, GjiProtocolPatternValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, GjiResolutionPatternValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, TaskActionIsolatedDocNumberValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActActionIsolatedDocNumberValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, MotivatedPresentationDocNumberValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PreventiveActionDocNumberValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PreventiveActionTaskDocNumberValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, VisitSheetDocNumberValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, WarningDocValidationNumberRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, MotivatedPresentationAppealCitsDocNumberValidationRule>();
        }

        public void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
            {
                this.Container.RegisterTransient<IReminderResolution, ReminderResolution>("Reminder Resolution Quartz integration");
                this.Container.RegisterTransient<ITask, ResolutionQuartzTask>();

                ApplicationContext.Current.Events.GetEvent<NhStartEvent>().Subscribe<InitQuartz>();
                ApplicationContext.Current.Events.GetEvent<AppStartEvent>().Subscribe<SchedulerInitiator>();
            }
        }

        public void RegistrationInspectionRules()
        {
            this.Container.RegisterTransient<IDocumentGjiRule, TatActCheckToWarningDocRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, TatWarningDocToMotivationConclusionRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, TatProtocolGjiToTatResolutionGjiRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, ActCheckToDecisionRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, DecisionToActCheckPrescriptionRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, DecisionToActCheckByRoRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, TaskActionToActActionRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, ActCheckToWarningDocRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, MotivatedPresentationToWarningDocRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, PreventiveActionToTaskRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, PreventiveActionTaskToVisitSheetRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, ActActionToMotivatedPresentationRule>();
            this.Container.RegisterTransient<IDocumentGjiRule, VisitSheetToMotivatedPresentationRule>();

            this.Container.RegisterTransient<IInspectionGjiRule, InspectionActionIsolatedToDisposalRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, TatGjiWarningToWarningDocRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, BaseDispHeadToDecisionRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, BaseJurPersonToDecisionRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, BaseProsClaimToDecisionRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, Bars.GkhGji.Regions.Tatarstan.InspectionRules.BaseStatementToDecisionRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, InspectionActionIsolatedToDecisionRule>();
            this.Container.RegisterTransient<IInspectionGjiRule, InspectionPreventiveActionToDecisionRule>();

            this.Container.ReplaceTransient<IDocumentGjiRule, DisposalToActSurveyRule, TatDisposalToActSurveyRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, DisposalToActCheckRule, TatDisposalToActCheckRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, DisposalToActCheckByRoRule, TatDisposalToActCheckByRoRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, ActCheckToDisposalRule, ActCheckToTatDisposalRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, ActCheckToProtocolRule, ActCheckToTatProtocolRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, ResolutionToProtocolRule, TatResolutionToTatProtocolRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, ProtocolToResolutionRule, TatProtocolToTatResolutionRule>();

            this.Container.ReplaceTransient<IInspectionGjiRule, BaseDispHeadToDisposalRule, BaseDispHeadToTatDisposalRule>();
            this.Container.ReplaceTransient<IInspectionGjiRule, BaseJurPersonToDisposalRule, BaseJurPersonToTatDisposalRule>();
            this.Container.ReplaceTransient<IInspectionGjiRule, BaseProsClaimToDisposalRule, BaseProsClaimToTatDisposalRule>();
            this.Container.ReplaceTransient<IInspectionGjiRule, BaseStatementToDisposalRule, BaseStatementToTatDisposalRule>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<GisChargeToSend, GisChargeViewModel>();
            this.Container.RegisterViewModel<GisGmpPattern, GisGmpPatternViewModel>();
            this.Container.RegisterViewModel<GisGmpPatternDict, GisGmpPatternDictViewModel>();
            this.Container.RegisterViewModel<WarningInspection, WarningInspectionViewModel>();
            this.Container.RegisterViewModel<WarningDoc, WarningDocViewModel>();
            this.Container.RegisterViewModel<WarningDocAnnex, WarningDocAnnexViewModel>();
            this.Container.RegisterViewModel<WarningDocBasis, WarningDocBasisViewModel>();
            this.Container.RegisterViewModel<WarningDocViolations, WarningDocViolationsViewModel>();
            this.Container.RegisterViewModel<MotivationConclusion, MotivationConclusionViewModel>();
            this.Container.RegisterViewModel<MotivationConclusionAnnex, MotivationConclusionAnnexViewModel>();
            this.Container.RegisterViewModel<TatarstanResolution, TatarstanResolutionViewModel>();
            this.Container.RegisterViewModel<TatarstanProtocolMvd, TatarstanProtocolMvdViewModel>();
            this.Container.RegisterViewModel<TatarstanResolutionPayFine, TatarstanResolutionPayFineViewModel>();
            this.Container.RegisterViewModel<TatarstanDisposal, TatarstanDisposalViewModel>();
            // TODO: 
           // this.Container.RegisterViewModel<RisTask, RisTaskViewModel>();
            this.Container.RegisterViewModel<EffectivenessAndPerformanceIndexValue, EffectivenessAndPerformanceIndexValueViewModel>();
            this.Container.RegisterViewModel<InspectionBaseType, InspectionBaseTypeViewModel>();
            this.Container.RegisterViewModel<TatarstanZonalInspection, TatarstanZonalInspectionViewModel>();
            this.Container.RegisterViewModel<ControlOrganization, ControlOrganizationViewModel>();
            this.Container.RegisterViewModel<ControlOrganizationControlTypeRelation, ControlOrganizationControlTypeRelationViewModel>();
            //this.Container.RegisterViewModel<TorTask, TorTaskViewModel>();
            this.Container.RegisterViewModel<TatarstanControlList, ControlListViewModel>();
            this.Container.RegisterViewModel<TatarstanControlListQuestion, ControlListQuestionViewModel>();
            this.Container.RegisterViewModel<ControlListTypicalQuestion, ControlListTypicalQuestionViewModel>();
            this.Container.RegisterViewModel<MandatoryReqsNormativeDoc, MandatoryReqsNormativeDocViewModel>();
            this.Container.RegisterViewModel<MandatoryReqs, MandatoryReqsViewModel>();
            this.Container.RegisterViewModel<TatarstanProtocolGjiContragent, TatarstanProtocolGjiContragentViewModel>();
            this.Container.RegisterViewModel<TatarstanProtocolGjiArticleLaw, TatarstanProtocolGjiArticleLawViewModel>();
            this.Container.RegisterViewModel<TatarstanProtocolGjiRealityObject, TatarstanProtocolGjiRealityObjectViewModel>();
            this.Container.RegisterViewModel<TatarstanProtocolGjiAnnex, TatarstanProtocolGjiAnnexViewModel>();
            this.Container.RegisterViewModel<TatarstanProtocolGjiViolation, TatarstanProtocolGjiViolationViewModel>();
            this.Container.RegisterViewModel<TatarstanDocumentWitness, TatarstanDocumentWitnessViewModel>();
            this.Container.RegisterViewModel<TatarstanResolutionGji, TatarstanResolutionGjiViewModel>();
            this.Container.RegisterViewModel<BudgetClassificationCode, BudgetClassificationCodeViewModel>();
            this.Container.RegisterViewModel<PrescriptionCancelTatarstan, PrescriptionCancelTatarstanViewModel>();
            this.Container.RegisterViewModel<TaskActionIsolated, TaskActionIsolatedViewModel>();
            this.Container.RegisterViewModel<TaskActionIsolatedItem, TaskActionIsolatedItemViewModel>();
            this.Container.RegisterViewModel<ActActionIsolated, ActActionIsolatedViewModel>();
            this.Container.RegisterViewModel<ActCheckAction, ActCheckActionViewModel>();
            this.Container.RegisterViewModel<PreventiveAction, PreventiveActionViewModel>();
            this.Container.RegisterViewModel<PreventiveActionTask, PreventiveActionTaskViewModel>();
            this.Container.RegisterViewModel<PreventiveActionTaskPlannedAction, PreventiveActionTaskPlannedActionViewModel>();
            this.Container.RegisterViewModel<PreventiveActionTaskRegulation, PreventiveActionTaskRegulationViewModel>();
            this.Container.RegisterViewModel<InspectionActionIsolated, InspectionActionIsolatedViewModel>();
            this.Container.RegisterViewModel<TaskActionIsolatedRealityObject, TaskActionIsolatedRealityObjectViewModel>();
            this.Container.RegisterViewModel<ActCheckActionCarriedOutEvent, ActCheckActionCarriedOutEventViewModel>();
            this.Container.RegisterViewModel<ActCheckActionFile, ActCheckActionFileViewModel>();
            this.Container.RegisterViewModel<ActCheckActionInspector, ActCheckActionInspectorViewModel>();
            this.Container.RegisterViewModel<ActCheckActionRemark, ActCheckActionRemarkViewModel>();
            this.Container.RegisterViewModel<ActCheckActionViolation, ActCheckActionViolationViewModel>();
            this.Container.RegisterViewModel<InstrExamAction, InstrExamActionViewModel>();
            this.Container.RegisterViewModel<InstrExamActionNormativeDoc, InstrExamActionNormativeDocViewModel>();
            this.Container.RegisterViewModel<ExplanationAction, ExplanationActionViewModel>();
            this.Container.RegisterViewModel<SurveyAction, SurveyActionViewModel>();
            this.Container.RegisterViewModel<SurveyActionQuestion, SurveyActionQuestionViewModel>();
            this.Container.RegisterViewModel<InspectionAction, InspectionActionViewModel>();
            this.Container.RegisterViewModel<DocRequestAction, DocRequestActionViewModel>();
            this.Container.RegisterViewModel<DocRequestActionRequestInfo, DocRequestActionRequestInfoViewModel>();
            this.Container.RegisterViewModel<TaskOfPreventiveActionTask, TaskOfPreventiveActionTaskViewModel>();
            this.Container.RegisterViewModel<VisitSheet, VisitSheetViewModel>();
            this.Container.RegisterViewModel<VisitSheetInfo, VisitSheetInfoViewModel>();
            this.Container.RegisterViewModel<MotivatedPresentation, MotivatedPresentationViewModel>();
            this.Container.RegisterViewModel<PreventiveActionTaskConsultingQuestion, PreventiveActionTaskConsultingQuestionViewModel>();
            this.Container.RegisterViewModel<MotivatedPresentationViolation, MotivatedPresentationViolationViewModel>();
            this.Container.RegisterViewModel<PreventiveActionTaskObjective, PreventiveActionTaskObjectiveViewModel>();
            this.Container.RegisterViewModel<VisitSheetAnnex, VisitSheetAnnexViewModel>();
            this.Container.RegisterViewModel<NormativeDoc, NormativeDocViewModel>();
            this.Container.RegisterViewModel<PreventiveActionTaskItem, PreventiveActionTaskItemViewModel>();
            this.Container.RegisterViewModel<ActActionIsolatedDefinition, ActActionIsolatedDefinitionViewModel>();
            this.Container.RegisterViewModel<TaskActionIsolatedAnnex, TaskActionIsolatedAnnexViewModel>();
            this.Container.RegisterViewModel<TaskActionIsolatedSurveyPurpose, TaskActionIsolatedSurveyPurposeViewModel>();
            this.Container.RegisterViewModel<TaskActionIsolatedArticleLaw, TaskActionIsolatedArticleLawViewModel>();
            this.Container.RegisterViewModel<MotivatedPresentationAnnex, MotivatedPresentationAnnexViewModel>();
            this.Container.RegisterViewModel<KnmTypes, KnmTypesViewModel>();
            this.Container.RegisterViewModel<TatarstanDecision, DecisionViewModel>();
            this.Container.RegisterViewModel<KindCheckGji, KindCheckGjiViewModel>();
            this.Container.RegisterViewModel<KnmCharacter, KnmCharacterViewModel>();
            this.Container.RegisterViewModel<KnmAction, KnmActionViewModel>();
            this.Container.RegisterViewModel<ControlTypeInspectorPositions, ControlTypeInspectorPosViewModel>();
            this.Container.RegisterViewModel<DecisionInspectionBase, DecisionInspectionBaseViewModel>();
            this.Container.RegisterViewModel<ErknmTypeDocument, ErknmTypeDocumentViewModel>();
            this.Container.RegisterViewModel<KnmReason, KnmReasonViewModel>();
            this.Container.RegisterViewModel<TatDisposalAnnex, TatDisposalAnnexViewModel>();
            this.Container.RegisterViewModel<ControlTypeRiskIndicators, ControlTypeRiskIndicatorsViewModel>();
            this.Container.RegisterViewModel<DecisionControlObjectInfo, DecisionControlObjectInfoViewModel>();
            this.Container.RegisterViewModel<TatProtocol, TatProtocolViewModel>();
            this.Container.RegisterViewModel<InspectionPreventiveAction, InspectionPreventiveActionViewModel>();
            this.Container.RegisterViewModel<VisitSheetViolation, VisitSheetViolationViewModel>();
            this.Container.RegisterViewModel<VisitSheetViolationInfo, VisitSheetViolationInfoViewModel>();
            this.Container.RegisterViewModel<MotivatedPresentationAppealCits, MotivatedPresentationAppealCitsViewModel>();
            this.Container.RegisterViewModel<MotivatedPresentationAppealCitsAnnex, MotivatedPresentationAppealCitsAnnexViewModel>();
            this.Container.RegisterViewModel<RapidResponseSystemAppealDetails, RapidResponseSystemAppealDetailsViewModel>();
            this.Container.RegisterViewModel<RapidResponseSystemAppealResponse, RapidResponseSystemAppealResponseViewModel>();
            this.Container.RegisterViewModel<RapidResponseSystemAppeal, RapidResponseSystemAppealViewModel>();

            ContainerHelper.ReplaceViewModel<ActCheck, MainViewModel.ActCheckViewModel, ActCheckViewModel>(this.Container);
            ContainerHelper.ReplaceViewModel<SurveySubject, SurveySubjectViewModel, ViewModel.Dict.SurveySubjectViewModel>(this.Container);
            ContainerHelper.ReplaceViewModel<HeatSeason, MainViewModel.HeatSeasonViewModel, HeatSeasonViewModel>(this.Container);
            ContainerHelper.ReplaceViewModel<ResolPros, MainViewModel.ResolProsViewModel, ViewModel.ResolPros.ResolProsTatarstanViewModel>(this.Container);
            ContainerHelper.ReplaceViewModel<Presentation, MainViewModel.PresentationViewModel, ViewModel.Presentation.PresentationViewModel>(this.Container);

            this.Container.ReplaceComponent
            (
                Component
                    .For<IViewModel<Inspector>>()
                    .ImplementedBy<Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict.InspectorViewModel>()
                    .LifestyleTransient()
            );
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IGkhBaseReport, WarningDocGjiReport>();
            this.Container.RegisterTransient<IGkhBaseReport, MotivatedRequestGjiReport>();
            this.Container.RegisterTransient<IGkhBaseReport, MotivationConclusionGjiReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ActIsolatedGjiReport>();
            this.Container.RegisterTransient<IGkhBaseReport, TatProtocolGjiReport>();
            this.Container.RegisterTransient<IGkhBaseReport, CourtResolutionGjiReport>();

            this.Container.ReplaceTransient<IPrintForm, ProtocolResponsibility, Report.ProtocolResponsibility>();
            this.Container.RegisterTransient<IPrintForm, GjiWorkReport>("GJI Tatarstan.Report.GjiWorkReport");

            this.Container.ReplaceTransient<IGkhBaseReport, ResolutionGjiReport, Report.ResolutionGji.ResolutionGjiReport>();
            this.Container.ReplaceTransient<IGkhBaseReport, ProtocolGjiReport, TatProtocolReport>();

            this.Container.RegisterTransient<IGkhBaseReport, DecisionReport>();
            this.Container.RegisterTransient<IGkhBaseReport, TaskActionIsolatedReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ActActionIsolatedReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ActActionReport>();
            this.Container.RegisterTransient<IGkhBaseReport, PreventiveActionVisitSheetReport>();
            this.Container.RegisterTransient<IGkhBaseReport, PreventiveActionTaskSheetReport>();
            this.Container.RegisterTransient<IGkhBaseReport, MotivatedPresentationReport>();
            this.Container.RegisterTransient<IGkhBaseReport, PreventiveActionVisitNotificationReport>();
            this.Container.RegisterTransient<IGkhBaseReport, PreventiveActionTaskConsultationReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ActCheckReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ProtocolNoticeReport>();

            this.Container.ReplaceComponent<IPrintForm>(
                typeof(HeatSeasonReceivedDocumentsReport),
                Component
                    .For<IPrintForm>()
                    .ImplementedBy<Report.HeatSeasonReceivedDocumentsReport>()
                    .LifestyleTransient()
                    .Named("GJI Tatarstan.Report.HeatSeasonReceivedDocumentsReport"));
        }

        private void RegisterServices()
        {
            // TODO: Расскоментировать после реализации
            /*this.Container.RegisterTransient<ISendDataService<TatarstanDisposal>, DisposalSendDataService>();
            this.Container
                .RegisterTransient<ISendDataService<EffectivenessAndPerformanceIndexValue>, EffectivenessAndPerformanceIndexValueSendDataService>();
            this.Container.RegisterTransient<ISendDataService<MandatoryReqs>, MandatoryReqsSendDataService>();
            this.Container.RegisterTransient<ISendDataService<Contragent>, ContragentSendDataService>();*/
            this.Container.RegisterTransient<ITatarstanProtocolGjiArticleLawService, TatarstanProtocolGjiArticleLawService>();
            this.Container.RegisterTransient<ITatarstanProtocolGjiRealityObjectService, TatarstanProtocolGjiRealityObjectService>();
            this.Container.RegisterTransient<ITatarstanProtocolGjiService, TatarstanProtocolGjiService>();
            this.Container.RegisterTransient<ITatarstanProtocolGjiViolationService, TatarstanProtocolGjiViolationService>();
            this.Container.RegisterTransient<IBudgetClassificationCodeService, BudgetClassificationCodeService>();
            this.Container.RegisterTransient<IResolProsAndResolutionService, ResolProsAndResolutionService>();
            this.Container.RegisterTransient<ITaskActionIsolatedRealityObjectService, TaskActionIsolatedRealityObjectService>();
            this.Container.RegisterTransient<IActActionIsolatedService, ActActionIsolatedService>();
            this.Container.RegisterTransient<ITaskActionIsolatedService, TaskActionIsolatedService>();
            this.Container.RegisterTransient<ITaskActionIsolatedItemService, TaskActionIsolatedItemService>();
            this.Container.RegisterTransient<IPreventiveActionTaskRegulationService, PreventiveActionTaskRegulationService>();
            this.Container.RegisterTransient<IPreventiveActionTaskItemService, PreventiveActionTaskItemService>();
            this.Container.RegisterTransient<ITaskActionIsolatedArticleLawService, TaskActionIsolatedArticleLawService>();
            this.Container.RegisterTransient<IActCheckActionViolationService, ActCheckActionViolationService>();
            this.Container.RegisterTransient<IGjiValidityDocPeriodService, GjiValidityDocPeriodService>();
            this.Container.RegisterTransient<IDecisionControlObjectInfoService, DecisionControlObjectInfoService>();
            this.Container.RegisterTransient<IErknmTypeDocumentsService, ErknmTypeDocumentsService>();
            this.Container.RegisterTransient<Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.IDecisionService, Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.Impl.DecisionService>();
            this.Container.RegisterTransient<IKnmActionService, KnmActionService>();
            this.Container.RegisterTransient<IVisitSheetService, VisitSheetService>();
            this.Container.RegisterTransient<IVisitSheetViolationService, VisitSheetViolationService>();
            this.Container.RegisterTransient<IRapidResponseSystemAppealService, RapidResponseSystemAppealService>();
            this.Container.RegisterTransient<IRapidResponseSystemAppealMailProvider, RapidResponseSystemAppealMailProvider>();
            this.Container.RegisterTransient<IPreventiveActionTaskService, PreventiveActionTaskService>();
            this.Container.RegisterTransient<IMotivatedPresentationAppealCitsService, MotivatedPresentationAppealCitsService>();

            this.Container.ReplaceComponent<IActCheckService>(
                typeof(ActCheckService),
                Component.For<IActCheckService>().ImplementedBy<DomainService.ActCheck.ActCheckService>().LifestyleTransient());
            this.Container.ReplaceComponent<IActRemovalService>(
                typeof(ActRemovalService),
                Component.For<IActRemovalService>().ImplementedBy<DomainService.ActRemoval.ActRemovalService>().LifestyleTransient());
            this.Container.ReplaceComponent<IDisposalService>(
                typeof(DisposalService),
                Component.For<IDisposalService>().ImplementedBy<DomainService.Disposal.DisposalService>().LifestyleTransient());
            this.Container.ReplaceComponent<IPrescriptionService>(
                typeof(PrescriptionService),
                Component.For<IPrescriptionService>().ImplementedBy<DomainService.Prescription.PrescriptionService>().LifestyleTransient());
            this.Container.ReplaceComponent<IProtocolService>(
                typeof(ProtocolService),
                Component.For<IProtocolService>().ImplementedBy<DomainService.Protocol.ProtocolService>().LifestyleTransient());
            this.Container.ReplaceComponent<IResolutionService>(
                typeof(ResolutionService),
                Component.For<IResolutionService>().ImplementedBy<DomainService.Resolution.ResolutionService>().LifestyleTransient());

            this.Container.ReplaceTransient<IPresentationService, PresentationService, DomainService.Presentation.PresentationService>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, TatarstanProtocolGjiExporter>("TatarstanProtocolGjiExporter");
            this.Container.Register(Component.For<IDataExportService>().Named("TatarstanProtocolMvdDataExport").ImplementedBy<TatarstanProtocolMvdDataExport>().LifeStyle.Transient);
            this.Container.RegisterTransient<IDataExportService, ResolProsTatarstanDataExport>("ResolProsTatarstanDataExport");
            this.Container.Register(Component.For<IDataExportService>().Named("TatarstanDisposalDataExport").ImplementedBy<TatarstanDisposalDataExport>().LifeStyle.Transient);
            this.Container.RegisterTransient<IDataExportService, InspectionActionIsolatedExport>("InspectionActionIsolatedExport");
            this.Container.RegisterTransient<IDataExportService, TaskActionIsolatedExport>("TaskActionIsolatedExport");
            this.Container.RegisterTransient<IDataExportService, InspectionPreventiveActionExport>("InspectionPreventiveActionExport");
            this.Container.RegisterTransient<IDataExportService, RapidResponseSystemAppealDetailsExport>("RapidResponseSystemAppealDetailsExport");
            this.Container.RegisterTransient<IDataExportService, VisitSheetDataExport>("VisitSheetDataExport");
            this.Container.RegisterTransient<IDataExportService, ActActionIsolatedExport>("ActActionIsolatedExport");
            this.Container.RegisterTransient<IDataExportService, PreventiveActionExport>("PreventiveActionExport");
            this.Container.RegisterTransient<IDataExportService, PreventiveActionTaskExport>("PreventiveActionTaskExport");
            this.Container.RegisterTransient<IDataExportService, MotivatedPresentationDataExport>("MotivatedPresentationDataExport");
            this.Container.RegisterTransient<IDataExportService, MotivatedPresentationAppealCitsExport>("MotivatedPresentationAppealCitsExport");

            this.Container.ReplaceTransient<IDataExportService, Bars.GkhGji.Export.ActCheckDataExport, ActCheckDataExport>("Tat ActCheckDataExport");
            this.Container.ReplaceTransient<IDataExportService, Bars.GkhGji.Export.ActRemovalDataExport, ActRemovalDataExport>("Tat ActRemovalDataExport");
            this.Container.ReplaceTransient<IDataExportService, Bars.GkhGji.Export.PresentationDataExport, PresentationDataExport>("Tat PresentationDataExport");
            this.Container.ReplaceTransient<IDataExportService, Bars.GkhGji.Export.PrescriptionDataExport, PrescriptionDataExport>("Tat PrescriptionDataExport");
            this.Container.ReplaceTransient<IDataExportService, Bars.GkhGji.Export.ProtocolDataExport, ProtocolDataExport>("Tat ProtocolDataExport");

            this.Container.RegisterTransient<IDataExportReport, ErknmIntegrationRegistryReport>("ErknmIntegrationRegistryReport");
        }

        private void RegisterTypeCheckRules()
        {
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<ActionIsolatedDocumentaryRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<ActionIsolatedExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<ActionIsolatedInspectionVisitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DecisionPrescriptionCode1279Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DecisionPrescriptionCode34Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DecisionPrescriptionCode5Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DecisionPrescriptionCode6Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DecisionPrescriptionCode8Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DecisionPrescriptionCode1011Rule>().LifeStyle.Transient);
            this.Container.RegisterTransient<IKindCheckRule, InspectionPrevActionDocumentaryRule>();
            this.Container.RegisterTransient<IKindCheckRule, InspectionPrevActionExitRule>();
            this.Container.RegisterTransient<IKindCheckRule, InspectionPrevActionInspectionVisitRule>();
        }
    }
}