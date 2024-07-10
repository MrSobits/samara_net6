namespace Bars.GkhGji
{
    using B4.Modules.Reports;
    using ConfigSections;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.Pivot;

    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.B4.Windsor;
    using Bars.Gkh;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Gji.Controllers;
    using Bars.Gkh.Gji.DomainService;
    using Bars.Gkh.ImportExport;
    using Bars.Gkh.Interceptors;
    using Bars.Gkh.Report;
    using Bars.GkhGji.CodedReport;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Controllers.Dict;
    using Bars.GkhGji.Controllers.HeatingSeason;
    using Bars.GkhGji.Controllers.Prescription;
    using Bars.GkhGji.Controllers.SurveyPlan;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.DomainService.SurveyPlan;
    using Bars.GkhGji.DomainService.SurveyPlan.Impl;
    using Bars.GkhGji.DomainService.SurveyPlan.Impl.Strategies;
    using Bars.GkhGji.DomainService.View;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.BoilerRooms;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Entities.SurveyPlan;
    using Bars.GkhGji.ExecutionAction;
    using Bars.GkhGji.Export;
    using Bars.GkhGji.Import.Appeal;
    using Bars.GkhGji.InspectionAction;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Interceptors.BoilerRooms;
    using Bars.GkhGji.Interceptors.SurveyPlan;
    using Bars.GkhGji.Navigation;
    using Bars.GkhGji.NumberRule;
    using Bars.GkhGji.NumberValidation;
    using Bars.GkhGji.Permissions;
    using Bars.GkhGji.Report;
    using Bars.GkhGji.Report.Form123;
    using Bars.GkhGji.Report.Form123Extended;
    using Bars.GkhGji.Report.Form1Control_v2;
    using Bars.GkhGji.Report.ReviewAppealsCits;
    using Bars.GkhGji.Rules;
    using Bars.GkhGji.StateChange;
    using Bars.GkhGji.StateChange.SurveyPlan;
    using Bars.GkhGji.Tasks.SurveyPlan;
    using Bars.GkhGji.TextValues;
    using Bars.GkhGji.ViewModel;
    using Bars.GkhGji.ViewModel.BoilerRooms;
    using Bars.GkhGji.ViewModel.SurveyPlan;

    using Castle.MicroKernel.Registration;
    using Controllers.Integration;
    using Gkh.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Interceptors.Dict;
    using Bars.GkhGji.Controllers.FuelInfo;
    using Bars.GkhGji.Controllers.ResolutionRospotrebnadzor;
    using Bars.GkhGji.DomainService.Dict;
    using Bars.GkhGji.DomainService.Dict.Impl;
    using Bars.GkhGji.DomainService.FuelInfo;
    using Bars.GkhGji.DomainService.FuelInfo.Impl;
    using Bars.GkhGji.DomainService.Impl;
    using Bars.GkhGji.FormatDataExport.ExportableEntities.Impl;
    using Bars.GkhGji.FormatDataExport.ProxySelectors.Impl;
    using Bars.GkhGji.Interceptors.FuelInfo;
    using Bars.GkhGji.Interceptors.ResolutionRospotrebnadzor;
    using Bars.GkhGji.LogMap.Provider;
    using Bars.GkhGji.ViewModel.Dict;
    using Bars.GkhGji.ViewModel.FuelInfo;
    using Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor;
    using Bars.GkhGji.ViewModel.Email;
    using Bars.GkhGji.Entities.Email;
    using Bars.GkhGji.DomainService.GisGkhRegional;
    using Bars.GkhGji.DomainService.GisGkhRegional.Impl;
    using Bars.GkhGji.DataExport;
    using Bars.GkhGji.DomainService.Inspection.Impl;
    using Bars.GkhGji.FormatDataExport.Domain.Impl;
    using Bars.GkhGji.Tasks;
    
    public partial class Module : AssemblyDefinedModule
    {
         /// <inheritdoc />
        public override void Install()
        {
            this.Container.RegisterImport<ImportAppeal>();
            this.Container.RegisterTransient<IEntityExportProvider, EntityExportProvider>();
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGji resources");
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji statefulentity");

            this.Container.RegisterGkhConfig<HousingInspection>();

            // Регистрация классов для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.GkhGji dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            // Роутинг для Панели задача инспектора и руководителя
            this.Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ReminderRouteMapRegistrar>().LifeStyle.Transient);

            this.Container.RegisterTransient<IFieldRequirementSource, GjiFieldRequirementMap>();
            this.Container.RegisterTransient<IPermissionSource, GkhGjiPermissionMap>();
            this.Container.RegisterTransient<IPermissionSource, GkhGjiActSurveyOwnerPermissionMap>();
            this.Container.RegisterTransient<IPermissionSource, GjiPermissionSource>();
            this.Container.RegisterTransient<IGjiPermission, GjiPermission>();

            this.Container.Register(Component.For<IViewCollection>().Named("GkhGjiViewCollection").ImplementedBy<GkhGjiViewCollection>().LifeStyle.Transient);

            this.Container.Register(Component.For<INavigationProvider>().Named("ActivityTsj navigation").ImplementedBy<ActivityTsjMenuProvider>()
                  .LifeStyle.Transient);

            this.Container.Register(Component.For<INavigationProvider>().Named("GkhGJI navigation").ImplementedBy<NavigationProvider>()
                    .LifeStyle.Transient);

            this.Container.Register(Component.For<INavigationProvider>().Named("DocumentsGjiRegister navigation").ImplementedBy<DocumentsGjiRegisterMenuProvider>()
                    .LifeStyle.Transient);
            
            this.Container.RegisterSingleton<IAutoMapperConfigProvider, Bars.GkhGji.Utils.AutoMapperConfigProvider>();

            // Регистрация правил формирования документов
            this.RegisterInspectionRules();

            this.RegisterStates();

            this.RegisterControllers();

            this.RegisterDomainService();

            this.RegisterViewModels();

            this.RegisterService();

            this.RegisterExports();

            this.RegisterInterceptors();

            this.RegisterReports();

            this.RegisterBundlers();

            this.RegisterAuditLogMap();

            this.RegisterExecutionActions();

            this.RegisterCodedReports();

            this.RegisterTasks();

            this.RegisterPV();

            this.RegisterFormatDataExport();
        }

        private void RegisterPV()
        {
            //профилактический визит
            Container.RegisterAltDataController<PreventiveVisit>();
            Container.RegisterAltDataController<PreventiveVisitAnnex>();
            Container.RegisterAltDataController<PreventiveVisitWitness>();
            Container.RegisterAltDataController<PreventiveVisitResult>();
            Container.RegisterAltDataController<PreventiveVisitPeriod>();
            Container.RegisterAltDataController<PreventiveVisitRealityObject>();
            //профвизит
            Container.Register(Component.For<IViewModel<PreventiveVisit>>().ImplementedBy<PreventiveVisitViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<PreventiveVisitAnnex>>().ImplementedBy<PreventiveVisitAnnexViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<PreventiveVisitWitness>>().ImplementedBy<PreventiveVisitWitnessViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<PreventiveVisitRealityObject>>().ImplementedBy<PreventiveVisitRealityObjectViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<PreventiveVisitPeriod>>().ImplementedBy<PreventiveVisitPeriodViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<PreventiveVisitResult>>().ImplementedBy<PreventiveVisitResultViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<PreventiveVisitResultViolation>>().ImplementedBy<PreventiveVisitResultViolationViewModel>().LifeStyle.Transient);

            Container.Register(Component.For<IDomainServiceInterceptor<PreventiveVisit>>().ImplementedBy<PreventiveVisitServiceInterceptor>().LifeStyle.Transient);
            Container.RegisterDomainInterceptor<PreventiveVisitPeriod, PreventiveVisitPeriodServiceInterceptor>();
            Container.RegisterTransient<IGkhBaseReport, PreventiveVisitReport>();
            Container.RegisterTransient<IRuleChangeStatus, PreventiveVisitValidationRule>();

            Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseStatementToPreventiveActRule>().LifeStyle.Transient);
            Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BasePlanActionToPreventiveActRule>().LifeStyle.Transient);
            Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseDispHeadToPreventiveActRule>().LifeStyle.Transient);
        }

        private void RegisterCodedReports()
        {
            this.Container.RegisterTransient<ICodedReport, ActCheckReport>();
            this.Container.RegisterTransient<ICodedReport, DisposalReport>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<SedAppealsController>();

            // контролелер для проведения какихто операций ГЖИ (которые нельзя выполнить скриптов в SQL)
            this.Container.RegisterController<GjiScriptController>();

            #region Справочники
			Container.RegisterAltDataController<InspectionReasonERKNM>();
            this.Container.RegisterAltDataController<SSTUTransferOrg>();
            this.Container.RegisterAltDataController<DirectoryERKNM>();
            this.Container.RegisterAltDataController<RecordDirectoryERKNM>();
            this.Container.RegisterAltDataController<ControlActivity>();
            this.Container.RegisterAltDataController<MKDLicTypeRequest>();
            this.Container.RegisterAltDataController<PlanActionGji>();
            this.Container.RegisterAltDataController<PlanJurPersonGji>();
            this.Container.RegisterAltDataController<ExpertGji>();
            this.Container.RegisterController<FileStorageDataController<MKDLicRequest>>();
            this.Container.RegisterAltDataController<MKDLicRequestRealityObject>();
            this.Container.RegisterController<MKDLicRequestFileController>();
            this.Container.RegisterController<MKDLicRequestQueryController>();
            this.Container.RegisterController<MKDLicRequestQueryAnswerController>();
            this.Container.RegisterAltDataController<ProsecutorOffice>();
            this.Container.RegisterAltDataController<ProvidedDocGji>();
            this.Container.RegisterController<InspectedPartGjiController>();
            this.Container.RegisterAltDataController<ExecutantDocGji>();
            this.Container.RegisterAltDataController<SanctionGji>();
            this.Container.RegisterAltDataController<ArticleLawGji>();
            this.Container.RegisterAltDataController<TypeCourtGji>();
            this.Container.RegisterAltDataController<CourtVerdictGji>();
            this.Container.RegisterAltDataController<InstanceGji>();
            this.Container.RegisterAltDataController<KindStatementGji>();
            this.Container.RegisterAltDataController<PlanInsCheckGji>();
            this.Container.RegisterAltDataController<ResolveGji>();
            this.Container.RegisterAltDataController<RevenueFormGji>();
            this.Container.RegisterAltDataController<RevenueSourceGji>();
            this.Container.RegisterAltDataController<ActionsRemovViol>();
            this.Container.RegisterAltDataController<DecisionMakingAuthorityGji>();
            this.Container.RegisterController<HeatSeasonPeriodGjiController>();
            this.Container.RegisterController<HeatingSeasonResolutionController>();
            this.Container.RegisterController<ViolationFeatureGjiController>();
            this.Container.RegisterController<ViolationNormativeDocItemGjiController>();
            this.Container.RegisterController<ViolationActionsRemovGjiController>();
            this.Container.RegisterAltDataController<KindWorkNotifGji>();
            this.Container.RegisterController<ServiceJuridicalGjiController>();
            this.Container.RegisterAltDataController<ArticleTsj>();
            this.Container.RegisterAltDataController<KindProtocolTsj>();
            this.Container.RegisterAltDataController<AnswerContentGji>();
            this.Container.RegisterController<CompetentOrgGjiController>();
            this.Container.RegisterAltDataController<RedtapeFlagGji>();
            this.Container.RegisterAltDataController<AuditPurposeGji>();
            this.Container.RegisterController<AuditPurposeSurveySubjectGjiController>();
            this.Container.RegisterController<ViolationGjiController>();

            this.Container.RegisterController<TypeSurveyGjiController>();
            this.Container.RegisterAltDataController<TypeSurveyGoalInspGji>();
            this.Container.RegisterAltDataController<TypeSurveyInspFoundationGji>();
            this.Container.RegisterAltDataController<TypeSurveyInspFoundationCheckGji>();
            this.Container.RegisterAltDataController<TypeSurveyTaskInspGji>();
            this.Container.RegisterAltDataController<TypeSurveyAdminRegulationGji>();
            this.Container.RegisterAltDataController<TypeSurveyKindInspGji>();
            this.Container.RegisterAltDataController<TypeSurveyProvidedDocumentGji>();

            this.Container.RegisterAltDataController<StatSubjectSubsubjectGji>();
            this.Container.RegisterAltDataController<StatSubsubjectFeatureGji>();
            this.Container.RegisterController<StatSubjectGjiController>();
            this.Container.RegisterController<StatSubsubjectGjiController>();

            this.Container.RegisterAltDataController<KindCheckGji>();

            this.Container.RegisterAltDataController<KindCheckRuleReplace>();

            this.Container.RegisterController<DocNumValidationRuleController>();
            this.Container.RegisterController<FeatureViolGjiController>();
            this.Container.RegisterAltDataController<SocialStatus>();

            this.Container.RegisterAltDataController<SurveyPurpose>();
            this.Container.RegisterAltDataController<SurveyObjective>();
            this.Container.RegisterAltDataController<ActivityDirection>();
            this.Container.RegisterAltDataController<DocumentCode>();
            this.Container.RegisterAltDataController<TypeFactViolation>();
            this.Container.RegisterAltDataController<KindBaseDocument>();
            this.Container.RegisterAltDataController<SurveySubjectRequirement>();
            this.Container.RegisterAltDataController<ResolveViolationClaim>();
            this.Container.RegisterAltDataController<NotificationCause>();
            this.Container.RegisterAltDataController<MkdManagementMethod>();
            this.Container.RegisterAltDataController<ApplicantCategory>();
            this.Container.RegisterController<B4.Alt.BaseDataController<Citizenship>>();
            this.Container.RegisterController<B4.Alt.BaseDataController<QuestionKind>>();
            this.Container.RegisterController<B4.Alt.BaseDataController<ConcederationResult>>();
            this.Container.RegisterController<B4.Alt.BaseDataController<FactCheckingType>>();

            this.Container.RegisterController<ControlTypeController>();
            
            this.Container.RegisterAltDataController<OSP>();

            this.Container.RegisterAltDataController<PhysicalPersonDocType>();

            #endregion

            #region контроллеры Инспекции
            this.Container.RegisterController<BasePlanActionController>();
            this.Container.RegisterController<InspectionGjiController>();
            this.Container.RegisterController<BaseJurPersonController>();
            this.Container.RegisterController<BaseDispHeadController>();
            this.Container.RegisterController<BaseInsCheckController>();
            this.Container.RegisterController<BaseProsClaimController>();
            this.Container.RegisterController<BaseStatementController>();
            this.Container.RegisterController<BaseLicenseApplicantsController>();
            this.Container.RegisterController<InspectionGjiRealityObjectController>();
            this.Container.RegisterController<InspectionGjiInspectorController>();
            this.Container.RegisterController<InspectionGjiZonalInspectionController>();
            this.Container.RegisterAltDataController<BaseProsResol>();
            this.Container.RegisterAltDataController<BaseHeatSeason>();
            this.Container.RegisterAltDataController<BaseProtocolMvd>();
            this.Container.RegisterAltDataController<BaseProtocolMhc>();
            this.Container.RegisterAltDataController<InspectionRisk>();
            this.Container.RegisterAltDataController<BaseProtocolRSO>();
            this.Container.RegisterAltDataController<ProdCalendar>();
            this.Container.RegisterAltDataController<EDSInspection>();
            this.Container.RegisterAltDataController<MKDLicRequestQuery>();
            this.Container.RegisterAltDataController<MKDLicRequestQueryAnswer>();

            this.Container.RegisterController<AppealCitsController>();
            this.Container.RegisterController<AppealCitsRealObjectController>();
            this.Container.RegisterController<AppealCitsStatSubjectController>();
            this.Container.RegisterController<FileStorageDataController<Disposal>>();
            this.Container.RegisterController<FileStorageDataController<AppealCitsRequest>>();
            this.Container.RegisterController<FileStorageDataController<AppealCitsRequestAnswer>>();
            this.Container.RegisterController<AppealCitsCategoryController>();
            this.Container.RegisterController<AppealCitsQuestionController>();
            this.Container.RegisterController<AppealCitsHeadInspectorController>();
            this.Container.RegisterController<AppealCitsAnswerStatSubjectController>();
            this.Container.RegisterAltDataController<AppealCitsSource>();
            this.Container.RegisterAltDataController<AppealCitsAttachment>();
            this.Container.RegisterAltDataController<AppealCitsAnswerAttachment>();
            this.Container.RegisterController<AppealCitsRequestController>();
            this.Container.RegisterController<AppealCitsRequestAnswerController>();

            //Внесение изменений в реестр лицензий 

            //this.Container.RegisterFileStorageDataController<MKDLicRequestQuery>();
            //this.Container.RegisterFileStorageDataController<MKDLicRequestQueryAnswer>();
            //лог обращений граждан
            this.Container.RegisterAltDataController<AppealCitsInfo>();

            this.Container.RegisterAltDataController<BaseDefault>();
            this.Container.RegisterAltDataController<BaseActivityTsj>();
            this.Container.RegisterController<InspectionGjiViolController>();
            this.Container.RegisterController<ReminderController>();

            this.Container.RegisterController<InspectionController>();
            this.Container.RegisterController<InspectionBaseContragentController>();
            #endregion

            this.Container.RegisterAltDataController<InspectionGjiViolStage>();

            this.Container.RegisterController<AppealCitsDecisionController>();

            this.Container.RegisterAltDataController<EntityChangeLogRecord>();



            //документ гжи
            this.Container.RegisterController<DocumentGjiController>();
            this.Container.RegisterController<DocumentGjiInspectorController>();
            this.Container.RegisterAltDataController<DocumentGjiChildren>();

            Container.RegisterAltDataController<SpecialAccountRow>();
            this.Container.RegisterController<SpecialAccountReportController>();

            //распоряжение
            this.Container.RegisterController<DisposalController>();
            this.Container.RegisterController<DisposalAnnexController>();
            this.Container.RegisterAltDataController<ViewDisposalWidget>();
            this.Container.RegisterController<DisposalExpertController>();
            this.Container.RegisterController<DisposalTypeSurveyController>();
            this.Container.RegisterController<DisposalProvidedDocController>();
            //this.Container.RegisterController<FileStorageDataController<DisposalAnnex>>();
            this.Container.RegisterController<DisposalViolController>();
            this.Container.RegisterController<UKDocumentController>();

            //решение
            this.Container.RegisterController<DecisionGjiController>();
            this.Container.RegisterController<DecisionAnnexController>();
            this.Container.RegisterAltDataController<DecisionExpert>();
            this.Container.RegisterAltDataController<DecisionControlList>();
            this.Container.RegisterAltDataController<DecisionInspectionReason>();
            this.Container.RegisterAltDataController<DecisionControlMeasures>();
            this.Container.RegisterAltDataController<DecisionAdminRegulation>();
            this.Container.RegisterAltDataController<DecisionControlSubjects>();
            this.Container.RegisterAltDataController<DecisionProvidedDoc>();
            this.Container.RegisterAltDataController<DecisionVerificationSubject>();

            //справочники для решения
            this.Container.RegisterAltDataController<InspectionReason>();
            this.Container.RegisterAltDataController<ControlList>();
            this.Container.RegisterAltDataController<ControlListQuestion>();


            //акт проверки
            this.Container.RegisterController<ActCheckController>();
            this.Container.RegisterController<ActCheckAnnexController>();
            this.Container.RegisterController<ActCheckRealityObjectController>();
            this.Container.RegisterController<ActCheckInspectedPartController>();
            //Container.RegisterAltDataController<ActCheckViolation>();
            this.Container.RegisterAltDataController<ActCheckPeriod>();
            this.Container.RegisterController<ActCheckWitnessController>();
            this.Container.RegisterAltDataController<ActCheckDefinition>();
            this.Container.RegisterController<FileStorageDataController<ActCheckAnnex>>();
            this.Container.RegisterAltDataController<ActCheckControlListAnswer>();
            this.Container.RegisterAltDataController<ActCheckWitness>();
            this.Container.RegisterAltDataController<ActCheckControlMeasures>();
            this.Container.RegisterController<ActCheckDefinitionController>();
            this.Container.RegisterController<ResolProsDefinitionController>();
            this.Container.RegisterController<FileStorageDataController<AppealCitsTypeOfFeedback>>();

            //акт обследования
            this.Container.RegisterController<ActSurveyController>();
            this.Container.RegisterController<ActSurveyAnnexController>();
            this.Container.RegisterAltDataController<ActSurveyRealityObject>();
            this.Container.RegisterController<ActSurveyInspectedPartController>();
            this.Container.RegisterAltDataController<ActSurveyOwner>();
            this.Container.RegisterController<FileStorageDataController<ActSurveyAnnex>>();
            this.Container.RegisterController<FileStorageDataController<ActSurveyPhoto>>();

            //акт устранения
            this.Container.RegisterController<ActRemovalController>();
            this.Container.RegisterAltDataController<ActRemovalViolation>();

            //предписание
            this.Container.RegisterController<PrescriptionController>();
            this.Container.RegisterController<PrescriptionAnnexController>();
            this.Container.RegisterAltDataController<ViewPrescriptionWidget>();
            this.Container.RegisterController<PrescriptionViolController>();
            this.Container.RegisterController<PrescriptionArticleLawController>();
            this.Container.RegisterController<PrescriptionCancelController>();
            this.Container.RegisterController<PrescriptionCancelViolReferenceController>();
            this.Container.RegisterController<FileStorageDataController<PrescriptionAnnex>>();
            this.Container.RegisterAltDataController<PrescriptionOfficialReport>();
            this.Container.RegisterAltDataController<PrescriptionOfficialReportViolation>();

            //протокол
            //ReplaceController чтобы избежать конфликта с Bars.KP60.Protocol.Controllers.ProtocolController при роутинге
            this.Container.ReplaceController<ProtocolController>("protocol");
            this.Container.RegisterController<ProtocolAnnexController>();
            this.Container.RegisterController<ProtocolViolationController>();
            this.Container.RegisterController<ProtocolArticleLawController>();
            this.Container.RegisterController<ProtocolDefinitionController>();
            this.Container.RegisterController<FileStorageDataController<ProtocolAnnex>>();

            //постановление
            this.Container.RegisterController<ResolutionController>();
            this.Container.RegisterController<ResolutionAnnexController>();
            this.Container.RegisterAltDataController<ResolutionPayFine>();
            this.Container.RegisterController<ResolutionDefinitionController>();
            this.Container.RegisterAltDataController<ResolutionDecision>();
            this.Container.RegisterController<FileStorageDataController<ResolutionAnnex>>();
            this.Container.RegisterController<FileStorageDataController<ResolutionDispute>>();

            //постановление прокуратуры
            this.Container.RegisterController<ResolProsController>();
            this.Container.RegisterController<ResolProsAnnexController>();
            this.Container.RegisterController<ResolProsArticleLawController>();
            this.Container.RegisterController<ResolProsRealityObjectController>();
            //this.Container.RegisterController<FileStorageDataController<ResolProsAnnex>>();

            // Постановление Роспотребнадзора
            this.Container.RegisterController<ResolutionRospotrebnadzorController>();
            this.Container.RegisterController<ResolutionRospotrebnadzorAnnexController>();
            this.Container.RegisterFileStorageDataController<ResolutionRospotrebnadzorAnnex>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorDefinition>();
            this.Container.RegisterFileStorageDataController<ResolutionRospotrebnadzorDispute>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorPayFine>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorViolation>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorArticleLaw>();

            //протокол МВД
            this.Container.RegisterController<ProtocolMvdController>();
            this.Container.RegisterController<ProtocolMvdAnnexController>();
            this.Container.RegisterController<ProtocolMvdArticleLawController>();
            this.Container.RegisterController<ProtocolMvdRealityObjectController>();
            this.Container.RegisterController<FileStorageDataController<ProtocolMvdAnnex>>();

            //протокол РСО
            Container.RegisterController<ProtocolRSOController>();
            Container.RegisterController<ProtocolRSOAnnexController>();
            Container.RegisterController<ProtocolRSOArticleLawController>();
            Container.RegisterController<ProtocolRSORealityObjectController>();
            Container.RegisterController<ProtocolRSODefinitionController>();
            Container.RegisterController<FileStorageDataController<ProtocolRSOAnnex>>();

            //протокол МЖК
            this.Container.RegisterController<ProtocolMhcController>();
            this.Container.RegisterController<ProtocolMhcAnnexController>();
            this.Container.RegisterController<ProtocolMhcArticleLawController>();
            this.Container.RegisterController<ProtocolMhcRealityObjectController>();
            this.Container.RegisterController<ProtocolMhcDefinitionController>();
            this.Container.RegisterController<FileStorageDataController<ProtocolMhcAnnex>>();

            //представление
            this.Container.RegisterController<PresentationController>();
            this.Container.RegisterController<PresentationAnnexController>();
            this.Container.RegisterController<FileStorageDataController<PresentationAnnex>>();

            //отопительный сезон
            this.Container.RegisterController<HeatSeasonController>();
            this.Container.RegisterController<HeatSeasonDocController>();

            //деятельность тсж
            this.Container.RegisterController<ActivityTsjController>();

            this.Container.RegisterController<ActivityTsjArticleController>();
            this.Container.RegisterController<ActivityTsjRealObjController>();
            this.Container.RegisterController<ActivityTsjProtocolRealObjController>();
            this.Container.RegisterController<FileStorageDataController<ActivityTsjProtocol>>();
            this.Container.RegisterController<FileStorageDataController<ActivityTsjStatute>>();
            this.Container.RegisterController<FileStorageDataController<ActivityTsjMember>>();
            this.Container.RegisterController<FileStorageDataController<PrescriptionCloseDoc>>();

            this.Container.RegisterController<MenuGjiController>();

            this.Container.RegisterController<MKDLicRequestInspectorController>();

            this.Container.RegisterController<BusinessActivityController>();

            this.Container.RegisterController<ActCheckViolationController>();

            // котельные
            this.Container.RegisterAltDataController<BoilerRoom>();
            this.Container.RegisterAltDataController<HeatingPeriod>();
            this.Container.RegisterAltDataController<UnactivePeriod>();

            // подача тепла
            this.Container.RegisterAltDataController<HeatInputPeriod>();
            this.Container.RegisterController<HeatInputInformationController>();
            this.Container.RegisterController<WorkWinterConditionController>();
            this.Container.RegisterController<GjiParamsController>();
            this.Container.RegisterAltDataController<ContragentAuditPurpose>();
            this.Container.RegisterAltDataController<SurveySubject>();
            this.Container.RegisterAltDataController<SurveySubjectLicensing>();

            // план проверки
            this.Container.RegisterController<SurveyPlanController>();
            this.Container.RegisterAltDataController<SurveyPlanCandidate>();
            this.Container.RegisterAltDataController<SurveyPlanContragent>();
            this.Container.RegisterController<FileStorageDataController<SurveyPlanContragentAttachment>>();

            this.Container.RegisterAltDataController<LegalReason>();
            this.Container.RegisterController<TypeSurveyContragentTypeController>();
            this.Container.RegisterController<TypeSurveyLegalReasonController>();

            this.Container.RegisterAltDataController<OrganMvd>();

            this.Container.RegisterController<FuelInfoPeriodController>();

            this.Container.RegisterAltDataController<FuelAmountInfo>();
            this.Container.RegisterAltDataController<FuelExtractionDistanceInfo>();
            this.Container.RegisterAltDataController<FuelContractObligationInfo>();
            this.Container.RegisterAltDataController<FuelEnergyDebtInfo>();

            this.Container.RegisterController<BaseJurPersonContragentController>();
            this.Container.RegisterController<ActCheckProvidedDocController>();

            this.Container.RegisterAltDataController<InspectionBaseType>();

            this.Container.RegisterAltDataController<DisposalAdminRegulation>();
            this.Container.RegisterAltDataController<DisposalInspFoundationCheck>();
            this.Container.RegisterAltDataController<DisposalInspFoundation>();
            this.Container.RegisterAltDataController<DisposalInspFoundCheckNormDocItem>();
            this.Container.RegisterAltDataController<DecisionVerificationSubjectNormDocItem>();
            this.Container.RegisterAltDataController<DisposalSurveyObjective>();
            this.Container.RegisterAltDataController<DisposalSurveyPurpose>();
            this.Container.RegisterAltDataController<DisposalVerificationSubject>();
            
            this.Container.RegisterController<DisposalInspFoundationCheckController>();

            //акт без взаимодействия
            this.Container.RegisterController<ActIsolatedController>();
            this.Container.RegisterController<ActIsolatedInspectedPartController>();
            this.Container.RegisterController<ActIsolatedProvidedDocController>();

            this.Container.RegisterAltDataController<ActIsolatedRealObj>();
            this.Container.RegisterAltDataController<ActIsolatedPeriod>();
            this.Container.RegisterAltDataController<ActIsolatedWitness>();
            this.Container.RegisterAltDataController<ActIsolatedDefinition>();
            this.Container.RegisterController<FileStorageDataController<ActIsolatedAnnex>>();
            this.Container.RegisterAltDataController<ActIsolatedRealObjViolation>();
            this.Container.RegisterAltDataController<ActIsolatedRealObjMeasure>();
            this.Container.RegisterAltDataController<ActIsolatedRealObjEvent>();

			this.Container.RegisterController<AppealCitsAnswerController>();
			this.Container.RegisterAltDataController<AppealCitsDefinition>();
			this.Container.RegisterAltDataController<AppealCitsTypeOfFeedback>();
            this.Container.RegisterController<EntityChangeLogRecordController>();

            //Владелец спецсчета
            Container.RegisterAltDataController<SpecialAccountOwner>();
            Container.RegisterAltDataController<SPAccOwnerRealityObject>();

            this.Container.RegisterAltDataController<TypeOfFeedback>();
            this.Container.RegisterController<EmailGjiController>();
            this.Container.RegisterAltDataController<EmailGjiAttachment>();
            //this.Container.RegisterAltDataController<ConcederationResult>();
        }

        private void RegisterDomainService()
        {
            //акт проверки
            this.Container.Register(Component.For<GkhGji.DomainService.ISignature<SpecialAccountReport>>().ImplementedBy<SpecialAccountReportSignature>().LifestyleTransient());
            this.Container.RegisterDomainService<ActCheckAnnex, FileStorageDomainService<ActCheckAnnex>>();
            this.Container.RegisterDomainService<AppealCitsTypeOfFeedback, FileStorageDomainService<AppealCitsTypeOfFeedback>>();
            this.Container.RegisterDomainService<ActCheckDefinition, FileStorageDomainService<ActCheckDefinition>>();
            this.Container.RegisterDomainService<ResolProsDefinition, FileStorageDomainService<ResolProsDefinition>>();
            this.Container.Register(Component.For<IDomainService<ActivityTsjProtocol>>().ImplementedBy<FileStorageDomainService<ActivityTsjProtocol>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActivityTsjStatute>>().ImplementedBy<FileStorageDomainService<ActivityTsjStatute>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActivityTsjMember>>().ImplementedBy<FileStorageDomainService<ActivityTsjMember>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActSurveyAnnex>>().ImplementedBy<FileStorageDomainService<ActSurveyAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActSurveyPhoto>>().ImplementedBy<FileStorageDomainService<ActSurveyPhoto>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCits>>().ImplementedBy<FileStorageDomainService<AppealCits>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCitsAnswer>>().ImplementedBy<FileStorageDomainService<AppealCitsAnswer>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<Disposal>>().ImplementedBy<FileStorageDomainService<Disposal>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCitsRequest>>().ImplementedBy<FileStorageDomainService<AppealCitsRequest>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCitsRequestAnswer>>().ImplementedBy<FileStorageDomainService<AppealCitsRequestAnswer>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BusinessActivity>>().ImplementedBy<FileStorageDomainService<BusinessActivity>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<DisposalAnnex>>().ImplementedBy<FileStorageDomainService<DisposalAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<DecisionAnnex>>().ImplementedBy<FileStorageDomainService<DecisionAnnex>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<SpecialAccountReport>>().ImplementedBy<FileStorageDomainService<SpecialAccountReport>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<UKDocument>>().ImplementedBy<FileStorageDomainService<UKDocument>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<MKDLicRequest>>().ImplementedBy<FileStorageDomainService<MKDLicRequest>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<MKDLicRequestFile>>().ImplementedBy<FileStorageDomainService<MKDLicRequestFile>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<MKDLicRequestQuery>>().ImplementedBy<FileStorageDomainService<MKDLicRequestQuery>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<MKDLicRequestQueryAnswer>>().ImplementedBy<FileStorageDomainService<MKDLicRequestQueryAnswer>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<HeatSeasonDoc>>().ImplementedBy<FileStorageDomainService<HeatSeasonDoc>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BaseDispHead>>().ImplementedBy<FileStorageDomainService<BaseDispHead>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BaseInsCheck>>().ImplementedBy<FileStorageDomainService<BaseInsCheck>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BaseProsClaim>>().ImplementedBy<FileStorageDomainService<BaseProsClaim>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<PrescriptionAnnex>>().ImplementedBy<FileStorageDomainService<PrescriptionAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<PresentationAnnex>>().ImplementedBy<FileStorageDomainService<PresentationAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolProsAnnex>>().ImplementedBy<FileStorageDomainService<ResolProsAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<PrescriptionOfficialReport>>().ImplementedBy<FileStorageDomainService<PrescriptionOfficialReport>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolutionAnnex>>().ImplementedBy<FileStorageDomainService<ResolutionAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolutionDefinition>>().ImplementedBy<FileStorageDomainService<ResolutionDefinition>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCitsDefinition>>().ImplementedBy<FileStorageDomainService<AppealCitsDefinition>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolDefinition>>().ImplementedBy<FileStorageDomainService<ProtocolDefinition>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolutionDispute>>().ImplementedBy<FileStorageDomainService<ResolutionDispute>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolMvdAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolMvdAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolMhcAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolMhcAnnex>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ProtocolRSOAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolRSOAnnex>>().LifeStyle.Transient);
            this.Container.Register(
                Component.For<IPersonInspectionService>().ImplementedBy<PersonInspectionService>().LifeStyle.Transient);
            this.Container.Register(
                Component.For<IJurPersonService>().ImplementedBy<JurPersonService>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDomainService<ActCheckWitness>>().ImplementedBy<ActCheckWitnessDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActCheckRealityObject>>().ImplementedBy<ActCheckRealityObjectDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActCheckViolation>>().ImplementedBy<ActCheckViolationDomainService>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDomainService<InspectionGjiRealityObject>>().ImplementedBy<InspectionGjiRealityObjectDomainService>().LifeStyle.Transient);
            
            
            // отопительный сезон
            this.Container.Register(Component.For<IDomainService<HeatSeason>>().ImplementedBy<HeatSeasonDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<HeatingSeasonResolution>>().ImplementedBy<FileStorageDomainService<HeatingSeasonResolution>>().LifeStyle.Transient);

            // обращения граждан
            this.Container.RegisterFileStorageDomainService<AppealCitsAttachment>();
            this.Container.RegisterFileStorageDomainService<AppealCitsAnswerAttachment>();

            //Типы обратной связи

            this.Container.RegisterViewModel<AppealCitsTypeOfFeedback, AppealCitsTypeOfFeedbackViewModel>();

            //вьюхи
            this.Container.Register(Component.For<IDomainService<ViewBaseDefault>>().ImplementedBy<ViewBaseDefaultDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ViewBaseDispHead>>().ImplementedBy<ViewBaseDispHeadDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ViewBaseInsCheck>>().ImplementedBy<ViewBaseInsCheckDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ViewBaseJurPerson>>().ImplementedBy<ViewBaseJurPersonDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ViewBaseProsClaim>>().ImplementedBy<ViewBaseProsClaimDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ViewBaseStatement>>().ImplementedBy<ViewBaseStatementDomainService>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDomainService<ActSurvey>>().ImplementedBy<ActSurveyDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActRemoval>>().ImplementedBy<ActRemovalDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActCheck>>().ImplementedBy<ActCheckDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<Prescription>>().ImplementedBy<PrescriptionDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<Disposal>>().ImplementedBy<DisposalDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<Protocol>>().ImplementedBy<ProtocolDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<Resolution>>().ImplementedBy<ResolutionDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolDefinition>>().ImplementedBy<ProtocolDefinitionDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolutionDefinition>>().ImplementedBy<ResolutionDefinitionDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelDomainService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolPros>>().ImplementedBy<ResolProsDomainService>().LifeStyle.Transient);

            this.Container.RegisterDomainService<ResolutionRospotrebnadzor, ResolutionRospotrebnadzorDomainService>();
            this.Container.RegisterFileStorageDomainService<ResolutionRospotrebnadzorAnnex>();
            this.Container.RegisterFileStorageDomainService<ResolutionRospotrebnadzorDispute>();

            this.Container.Register(Component.For<IDomainService<PrescriptionCloseDoc>>().ImplementedBy<FileStorageDomainService<PrescriptionCloseDoc>>().LifeStyle.Transient);

            this.Container.RegisterTransient<IViolationNormativeDocItemService, ViolationNormativeDocItemService>();
            this.Container.RegisterTransient<IMKDLicRequestInspectorService, MKDLicRequestInspectorService>();

            this.Container.RegisterDomainService<SurveyPlanContragentAttachment, FileStorageDomainService<SurveyPlanContragentAttachment>>();

            this.Container.RegisterTransient<IBaseJurPersonContragentService, BaseJurPersonContragentService>();
            this.Container.RegisterTransient<IDisposalAdminRegulationService, DisposalAdminRegulationService>();
            this.Container.RegisterTransient<IDisposalInsFoundationCheckService, DisposalInsFoundationCheckService>();
            this.Container.RegisterTransient<IDisposalInsFoundationService, DisposalInsFoundationService>();
            this.Container.RegisterTransient<IDisposalSurveyObjectiveService, DisposalSurveyObjectiveService>();
            this.Container.RegisterTransient<IDisposalSurveyPurposeService, DisposalSurveyPurposeService>();
            this.Container.RegisterTransient<IDisposalVerificationSubjectService, DisposalVerificationSubjectService>();
            this.Container.RegisterTransient<IEmailGjiService, EmailGjiService>();
            this.Container.RegisterTransient<IViewInspectionRepository, ViewInspectionRepository>();
            
            this.Container.RegisterDomainService<DisposalInspFoundationCheck, DisposalInspFoundationCheckDomainService>();

            this.Container.Register(Component.For<ISignature<AppealCitsRequest>>().ImplementedBy<AppealCitsRequestSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<AppealCitsRequestAnswer>>().ImplementedBy<AppealCitsRequestAnswerSignature>().LifestyleTransient());

            this.Container.Register(Component.For<ISignature<ActCheckAnnex>>().ImplementedBy<ActCheckAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<UKDocument>>().ImplementedBy<UKDocumentSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ActCheckDefinition>>().ImplementedBy<ActCheckDefinitionSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ActSurveyAnnex>>().ImplementedBy<ActSurveyAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<DisposalAnnex>>().ImplementedBy<DisposalAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<DecisionAnnex>>().ImplementedBy<DecisionAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<PrescriptionAnnex>>().ImplementedBy<PrescriptionAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<PresentationAnnex>>().ImplementedBy<PresentationAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ProtocolAnnex>>().ImplementedBy<ProtocolAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ProtocolMhcAnnex>>().ImplementedBy<ProtocolMhcAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ProtocolMvdAnnex>>().ImplementedBy<ProtocolMvdAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ProtocolRSOAnnex>>().ImplementedBy<ProtocolRSOAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ResolProsAnnex>>().ImplementedBy<ResolProsAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ResolutionAnnex>>().ImplementedBy<ResolutionAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<ResolutionRospotrebnadzorAnnex>>().ImplementedBy<ResolutionRospotrebnadzorAnnexSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<MKDLicRequestFile>>().ImplementedBy<MKDLicRequestFileSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<MKDLicRequestQuery>>().ImplementedBy<MKDLicRequestQuerySignature> ().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<MKDLicRequestQueryAnswer>>().ImplementedBy<MKDLicRequestQueryAnswerSignature>().LifestyleTransient());

            this.Container.Register(Component.For<ISignature<AppealCitsAnswer>>().ImplementedBy<AppealCitsAnswerSignature>().LifestyleTransient());
        }

        private void RegisterViewModels()
        {
            this.Container.Register(Component.For<IViewModel<InspectionGjiViolStage>>().ImplementedBy<InspectionViolStageViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<BasePlanAction>>().ImplementedBy<BasePlanActionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCitsDecision>>().ImplementedBy<AppealCitsDecisionViewModel>().LifeStyle.Transient);

            //документ гжи
            this.Container.Register(Component.For<IViewModel<DocumentGji>>().ImplementedBy<DocumentGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DocumentGjiChildren>>().ImplementedBy<DocumentGjiChildrenViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<Reminder>>().ImplementedBy<ReminderViewModel>().LifeStyle.Transient);

            Container.Register(Component.For<IViewModel<SpecialAccountReport>>().ImplementedBy<SpecialAccountReportViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<SpecialAccountRow>>().ImplementedBy<SpecialAccountRowViewModel>().LifeStyle.Transient);

            this.Container.RegisterViewModel<RecordDirectoryERKNM, RecordDirectoryERKNMViewModel>();
            this.Container.RegisterViewModel<DirectoryERKNM, DirectoryERKNMViewModel>();
            this.Container.RegisterViewModel<EntityChangeLogRecord, EntityChangeLogRecordViewModel>();

            this.Container.Register(Component.For<IViewModel<ActCheck>>().ImplementedBy<ActCheckViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckAnnex>>().ImplementedBy<ActCheckAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<UKDocument>>().ImplementedBy<UKDocumentViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckDefinition>>().ImplementedBy<ActCheckDefinitionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckInspectedPart>>().ImplementedBy<ActCheckInspectedPartViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckPeriod>>().ImplementedBy<ActCheckPeriodViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckControlListAnswer>>().ImplementedBy<ActCheckControlListAnswerViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckRealityObject>>().ImplementedBy<ActCheckRealityObjectViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ActCheckViolationViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckWitness>>().ImplementedBy<ActCheckWitnessViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolProsDefinition>>().ImplementedBy<ResolProsDefinitionViewModel>().LifeStyle.Transient);

            //деятельность тсж
            this.Container.Register(Component.For<IViewModel<ActivityTsjArticle>>().ImplementedBy<ActivityTsjArticleViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActivityTsj>>().ImplementedBy<ActivityTsjViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActivityTsjProtocol>>().ImplementedBy<ActivityTsjProtocolViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActivityTsjProtocolRealObj>>().ImplementedBy<ActivityTsjProtocolRealObjViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActivityTsjRealObj>>().ImplementedBy<ActivityTsjRealObjViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActivityTsjStatute>>().ImplementedBy<ActivityTsjStatuteViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActivityTsjMember>>().ImplementedBy<ActivityTsjMemberViewModel>().LifeStyle.Transient);

            //акт обследования
            this.Container.Register(Component.For<IViewModel<ActSurveyAnnex>>().ImplementedBy<ActSurveyAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActSurvey>>().ImplementedBy<ActSurveyViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActSurveyPhoto>>().ImplementedBy<ActSurveyPhotoViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActSurveyOwner>>().ImplementedBy<ActSurveyOwnerViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActSurveyInspectedPart>>().ImplementedBy<ActSurveyInspectedPartViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActSurveyRealityObject>>().ImplementedBy<ActSurveyRealityObjectViewModel>().LifeStyle.Transient);

            //акт устранения
            this.Container.Register(Component.For<IViewModel<ActRemoval>>().ImplementedBy<ActRemovalViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActRemovalViolation>>().ImplementedBy<ActRemovalViolationViewModel>().LifeStyle.Transient);


            //распоряжение
            this.Container.Register(Component.For<IViewModel<Disposal>>().ImplementedBy<DisposalViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DisposalAnnex>>().ImplementedBy<DisposalAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DisposalExpert>>().ImplementedBy<DisposalExpertViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DisposalTypeSurvey>>().ImplementedBy<DisposalTypeSurveyViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DisposalProvidedDoc>>().ImplementedBy<DisposalProvidedDocViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DisposalViolation>>().ImplementedBy<DisposalViolViewModel>().LifeStyle.Transient);

            //уведомление о начале предпринимательской деятельности
            this.Container.Register(Component.For<IViewModel<BusinessActivity>>().ImplementedBy<BusinessActivityViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ServiceJuridicalGji>>().ImplementedBy<ServiceJuridalGjiViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<Prescription>>().ImplementedBy<PrescriptionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionAnnex>>().ImplementedBy<PrescriptionAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionOfficialReport>>().ImplementedBy<PrescriptionOfficialReportViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionOfficialReportViolation>>().ImplementedBy<PrescriptionOfficialReportViolationViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionCloseDoc>>().ImplementedBy<PrescriptionCloseDocViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionArticleLaw>>().ImplementedBy<PrescriptionArticleLawViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionViol>>().ImplementedBy<PrescriptionViolViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<PrescriptionCancelViolReference>>().ImplementedBy<PrescriptionCancelViolReferenceViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<ProtocolAnnex>>().ImplementedBy<ProtocolAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolDefinition>>().ImplementedBy<ProtocolDefinitionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolArticleLaw>>().ImplementedBy<ProtocolArticleLawViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<Protocol>>().ImplementedBy<ProtocolViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolViolation>>().ImplementedBy<ProtocolViolationViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<PresentationAnnex>>().ImplementedBy<PresentationAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<Presentation>>().ImplementedBy<PresentationViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<ResolutionAnnex>>().ImplementedBy<ResolutionAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolutionPayFine>>().ImplementedBy<ResolutionPayFineViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolutionDispute>>().ImplementedBy<ResolutionDisputeViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolutionDecision>>().ImplementedBy<ResolutionDecisionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolutionDefinition>>().ImplementedBy<ResolutionDefinitionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCitsDefinition>>().ImplementedBy<AppealCitsDefinitionViewModel>().LifeStyle.Transient);

            //постановление прокуратуры
            this.Container.Register(Component.For<IViewModel<ResolPros>>().ImplementedBy<ResolProsViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolProsAnnex>>().ImplementedBy<ResolProsAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolProsArticleLaw>>().ImplementedBy<ResolProsArticleLawViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ResolProsRealityObject>>().ImplementedBy<ResolProsRealityObjectViewModel>().LifeStyle.Transient);

            //протокол МВД
            this.Container.Register(Component.For<IViewModel<ProtocolMvd>>().ImplementedBy<ProtocolMvdViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMvdAnnex>>().ImplementedBy<ProtocolMvdAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMvdArticleLaw>>().ImplementedBy<ProtocolMvdArticleLawViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMvdRealityObject>>().ImplementedBy<ProtocolMvdRealityObjectViewModel>().LifeStyle.Transient);

            //Протокол РСО
            Container.Register(Component.For<IViewModel<ProtocolRSO>>().ImplementedBy<ProtocolRSOViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ProtocolRSOAnnex>>().ImplementedBy<ProtocolRSOAnnexViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ProtocolRSOArticleLaw>>().ImplementedBy<ProtocolRSOArticleLawViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ProtocolRSODefinition>>().ImplementedBy<ProtocolRSODefinitionViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<ProtocolRSORealityObject>>().ImplementedBy<ProtocolRSORealityObjectViewModel>().LifeStyle.Transient);

            //Протокол МЖК
            this.Container.Register(Component.For<IViewModel<ProtocolMhc>>().ImplementedBy<ProtocolMhcViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMhcAnnex>>().ImplementedBy<ProtocolMhcAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMhcArticleLaw>>().ImplementedBy<ProtocolMhcArticleLawViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMhcDefinition>>().ImplementedBy<ProtocolMhcDefinitionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProtocolMhcRealityObject>>().ImplementedBy<ProtocolMhcRealityObjectViewModel>().LifeStyle.Transient);

            //обращения граждан
            this.Container.Register(Component.For<IViewModel<AppealCitsRealityObject>>().ImplementedBy<AppealCitsRealObjectViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCitsStatSubject>>().ImplementedBy<AppealCitsStatSubjectViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCitsSource>>().ImplementedBy<AppealCitsSourceViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCits>>().ImplementedBy<AppealCitsViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCitsAnswer>>().ImplementedBy<AppealCitsAnswerViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<AppealCitsRequest>>().ImplementedBy<AppealCitsRequestViewModel>().LifeStyle.Transient);
            this.Container.RegisterViewModel<AppealCitsCategory, AppealCitsCategoryViewModel>();
            this.Container.RegisterViewModel<AppealCitsQuestion, AppealCitsQuestionViewModel>();
            this.Container.RegisterViewModel<AppealCitsHeadInspector, AppealCitsHeadInspectorViewModel>();
            this.Container.RegisterViewModel<AppealCitsAttachment, AppealCitsAttachmentViewModel>();
            this.Container.RegisterViewModel<AppealCitsAnswerAttachment, AppealCitsAnswerAttachmentViewModel>();
            this.Container.RegisterViewModel<AppealCitsAnswerStatSubject, AppealCitsAnswerStatSubjectViewModel>();
            this.Container.RegisterViewModel<ProdCalendar, ProdCalendarViewModel>();
            this.Container.RegisterViewModel<EDSInspection, EDSInspectionViewModel>();

            //основания проверок
            this.Container.RegisterViewModel<BaseActivityTsj, BaseActivityTsjViewModel>();
            this.Container.RegisterViewModel<BaseHeatSeason, BaseHeatSeasonViewModel>();
            this.Container.RegisterViewModel<BaseDispHead, BaseDispHeadViewModel>();
            this.Container.RegisterViewModel<BaseProsClaim, BaseProsClaimViewModel>();
            this.Container.RegisterViewModel<BaseDefault, BaseDefaultViewModel>();
            this.Container.RegisterViewModel<BaseJurPerson, BaseJurPersonViewModel>();
            this.Container.RegisterViewModel<BaseInsCheck, BaseInsCheckViewModel>();
            this.Container.RegisterViewModel<BaseStatement, BaseStatementViewModel>();
            this.Container.RegisterViewModel<InspectionRisk, InspectionRiskViewModel>();

            this.Container.Register(Component.For<IViewModel<InspectionGjiRealityObject>>().ImplementedBy<InspectionGjiRealityObjectViewModel>().LifeStyle.Transient);
            this.Container.RegisterViewModel<InspectionBaseContragent, InspectionBaseContragentViewModel>();

            //отопительный сезон
            this.Container.Register(Component.For<IViewModel<HeatSeasonDoc>>().ImplementedBy<HeatSeasonDocViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<HeatSeason>>().ImplementedBy<HeatSeasonViewModel>().LifeStyle.Transient);

            //Справочники
            Container.RegisterViewModel<InspectionReasonERKNM, InspectionReasonERKNMViewModel>();
            this.Container.Register(Component.For<IViewModel<TypeSurveyGji>>().ImplementedBy<TypeSurveyGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<TypeSurveyGoalInspGji>>().ImplementedBy<TypeSurveyGoalInspGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<TypeSurveyKindInspGji>>().ImplementedBy<TypeSurveyKindInspGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<TypeSurveyProvidedDocumentGji>>().ImplementedBy<TypeSurveyProvidedDocumentGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<TypeSurveyInspFoundationGji>>().ImplementedBy<TypeSurveyInspFoundationGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<TypeSurveyInspFoundationCheckGji>>().ImplementedBy<TypeSurveyInspFoundationCheckGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<TypeSurveyTaskInspGji>>().ImplementedBy<TypeSurveyTaskInspGjiViewModel>().LifeStyle.Transient);
            this.Container.RegisterViewModel<TypeSurveyAdminRegulationGji, TypeSurveyAdminRegulationGjiViewModel>();
            this.Container.Register(Component.For<IViewModel<ViolationGji>>().ImplementedBy<ViolationGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ViolationFeatureGji>>().ImplementedBy<ViolationFeatureGjiViewModel>().LifeStyle.Transient);
            this.Container.RegisterViewModel<ViolationNormativeDocItemGji, ViolationNormativeDocItemGjiViewModel>();
            this.Container.Register(Component.For<IViewModel<StatSubsubjectGji>>().ImplementedBy<StatSubsubjectGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<StatSubjectSubsubjectGji>>().ImplementedBy<StatSubjectSubsubjectGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<StatSubsubjectFeatureGji>>().ImplementedBy<StatSubsubjectFeatureGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ViolationActionsRemovGji>>().ImplementedBy<ViolationActionsRemovGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ProvidedDocGji>>().ImplementedBy<ProvidedDocGjiViewModel>().LifeStyle.Transient);
            this.Container.RegisterViewModel<SurveySubject, SurveySubjectViewModel>();
            this.Container.RegisterViewModel<SurveySubjectRequirement, SurveySubjectRequirementViewModel>();
            this.Container.RegisterViewModel<TypeFactViolation, TypeFactViolationViewModel>();
            this.Container.RegisterViewModel<SSTUTransferOrg, SSTUTransferOrgViewModel>();
            this.Container.RegisterViewModel<PhysicalPersonDocType, PhysicalPersonDocTypeViewModel>();
            this.Container.RegisterViewModel<MKDLicTypeRequest, MKDLicTypeRequestViewModel>();

            //Внесение изменений в реестр лицензий 
            this.Container.RegisterViewModel<MKDLicRequest, MKDLicRequestViewModel>();
            this.Container.RegisterViewModel<MKDLicRequestRealityObject, MKDLicRequestRealityObjectViewModel>();
            this.Container.RegisterViewModel<MKDLicRequestFile, MKDLicRequestFileViewModel>();
            this.Container.RegisterViewModel<MKDLicRequestQuery, MKDLicRequestQueryViewModel>();
            this.Container.RegisterViewModel<MKDLicRequestQueryAnswer, MKDLicRequestQueryAnswerViewModel>();

            //Решение о проведении проверки
            this.Container.RegisterViewModel<Decision, DecisionGjiViewModel>();
            this.Container.RegisterViewModel<DecisionAdminRegulation, DecisionAdminRegulationViewModel>();
            this.Container.RegisterViewModel<DecisionAnnex, DecisionAnnexViewModel>();
            this.Container.RegisterViewModel<DecisionControlMeasures, DecisionControlMeasuresViewModel>();
            this.Container.RegisterViewModel<DecisionExpert, DecisionExpertViewModel>();
            this.Container.RegisterViewModel<DecisionControlList, DecisionControlListViewModel>();
            this.Container.RegisterViewModel<ActCheckControlMeasures, ActCheckControlMeasuresViewModel>();
            this.Container.RegisterViewModel<DecisionInspectionReason, DecisionInspectionReasonViewModel>();
            this.Container.RegisterViewModel<DecisionControlSubjects, DecisionControlSubjectsViewModel>();
            this.Container.RegisterViewModel<DecisionVerificationSubject, DecisionVerificationSubjectViewModel>();
            this.Container.RegisterViewModel<DecisionProvidedDoc, DecisionProvidedDocViewModel>();
            this.Container.RegisterViewModel<ControlListQuestion, ControlListQuestionViewModel>();


            //лог обращений граждан
            this.Container.RegisterViewModel<AppealCitsInfo, AppealCitsInfoViewModel>();
            this.Container.RegisterViewModel<AppealCitsRequestAnswer, AppealCitsRequestAnswerViewModel>();

            this.Container.RegisterViewModel<ArticleLawGji, ArticleLawViewModel>();
            this.Container.RegisterViewModel<AuditPurposeGji, AuditPurposeGjiViewModel>();
            this.Container.RegisterViewModel<AuditPurposeSurveySubjectGji, AuditPurposeSurveySubjectGjiViewModel>();

            this.Container.RegisterViewModel<ControlType, ControlTypeViewModel>();

            //правила проставления видов проверок
            this.Container.Register(Component.For<IViewModel<KindCheckRuleReplace>>().ImplementedBy<KindCheckRuleReplaceViewModel>().LifeStyle.Transient);
            //правила проставления номера документов
            this.Container.Register(Component.For<IViewModel<DocNumValidationRule>>().ImplementedBy<DocNumValidationRuleViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<ViewDisposalWidget>>().ImplementedBy<ViewDisposalWidgetViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ViewPrescriptionWidget>>().ImplementedBy<ViewPrescriptionWidgetViewModel>().LifeStyle.Transient);

            Component.For<IViewModel<Resolution>>()
                     .ImplementedBy<ResolutionViewModel>()
                     .LifestyleTransient()
                     .RegisterIn(this.Container);

            // котельные
            this.Container.Register(Component.For<IViewModel<BoilerRoom>>().ImplementedBy<BoilerRoomViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<HeatingPeriod>>().ImplementedBy<HeatingPeriodViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<UnactivePeriod>>().ImplementedBy<UnactivePeriodViewModel>().LifeStyle.Transient);

            // подача тепла
            this.Container.RegisterViewModel<HeatInputPeriod, HeatInputPeriodViewModel>();
            this.Container.RegisterViewModel<HeatInputInformation, HeatInputInformationViewModel>();
            this.Container.RegisterViewModel<WorkWinterCondition, WorkWinterConditionViewModel>();
            this.Container.RegisterViewModel<BaseLicenseApplicants, BaseLicenseApplicantsViewModel>();
            this.Container.RegisterViewModel<ContragentAuditPurpose, ContragentAuditPurposeViewModel>();

            // план проверки
            this.Container.RegisterViewModel<SurveyPlanCandidate, SurveyPlanCandidateViewModel>();
            this.Container.RegisterViewModel<SurveyPlanContragent, SurveyPlanContragentViewModel>();
            this.Container.RegisterViewModel<SurveyPlanContragentAttachment, SurveyPlanContragentAttachmentViewModel>();

            this.Container.RegisterViewModel<TypeSurveyLegalReason, TypeSurveyLegalReasonViewModel>();
            this.Container.RegisterViewModel<TypeSurveyContragentType, TypeSurveyContragentTypeViewModel>();
            this.Container.RegisterViewModel<OrganMvd, OrganMvdViewModel>();

            // Постановление Роспотребнадзора
            this.Container.RegisterViewModel<ResolutionRospotrebnadzor, ResolutionRospotrebnadzorViewModel>();
            this.Container.RegisterViewModel<ResolutionRospotrebnadzorAnnex, ResolutionRospotrebnadzorAnnexViewModel>();
            this.Container.RegisterViewModel<ResolutionRospotrebnadzorDefinition, ResolutionRospotrebnadzorDefinitionViewModel>();
            this.Container.RegisterViewModel<ResolutionRospotrebnadzorDispute, ResolutionRospotrebnadzorDisputeViewModel>();
            this.Container.RegisterViewModel<ResolutionRospotrebnadzorPayFine, ResolutionRospotrebnadzorPayFineViewModel>();
            this.Container.RegisterViewModel<ResolutionRospotrebnadzorViolation, ResolutionRospotrebnadzorViolationViewModel>();
            this.Container.RegisterViewModel<ResolutionRospotrebnadzorArticleLaw, ResolutionRospotrebnadzorArticleLawViewModel>();

            this.Container.RegisterViewModel<FuelAmountInfo, FuelAmountInfoViewModel>();
            this.Container.RegisterViewModel<FuelExtractionDistanceInfo, FuelExtractionDistanceInfoViewModel>();
            this.Container.RegisterViewModel<FuelContractObligationInfo, FuelContractObligationInfoViewModel>();
            this.Container.RegisterViewModel<FuelEnergyDebtInfo, FuelEnergyDebtInfoViewModel>();

            this.Container.RegisterViewModel<RevenueSourceGji, RevenueSourceGjiViewModel>();

            this.Container.RegisterViewModel<BaseJurPersonContragent, BaseJurPersonContragentViewModel>();
            this.Container.RegisterViewModel<DisposalAdminRegulation, DisposalAdminRegulationViewModel>();
            this.Container.RegisterViewModel<DisposalInspFoundationCheck, DisposalInspFoundationCheckViewModel>();
            this.Container.RegisterViewModel<DisposalInspFoundation, DisposalInspFoundationViewModel>();
            this.Container.RegisterViewModel<DisposalInspFoundCheckNormDocItem, DisposalInspFoundCheckNormDocItemViewModel>();
            this.Container.RegisterViewModel<DecisionVerificationSubjectNormDocItem, DecisionVerificationSubjectNormDocItemViewModel>();
            this.Container.RegisterViewModel<DisposalSurveyObjective, DisposalSurveyObjectiveViewModel>();
            this.Container.RegisterViewModel<DisposalSurveyPurpose, DisposalSurveyPurposeViewModel>();
            this.Container.RegisterViewModel<DisposalVerificationSubject, DisposalVerificationSubjectViewModel>();
            this.Container.RegisterViewModel<ActCheckProvidedDoc, ActCheckProvidedDocViewModel>();
            
            //Акт без взаимодействия
            this.Container.RegisterViewModel<ActIsolated, ActIsolatedViewModel>();
            this.Container.RegisterViewModel<ActIsolatedAnnex, ActIsolatedAnnexViewModel>();
            this.Container.RegisterViewModel<ActIsolatedDefinition, ActIsolatedDefinitionViewModel>();
            this.Container.RegisterViewModel<ActIsolatedInspectedPart, ActIsolatedInspectedPartViewModel>();
            this.Container.RegisterViewModel<ActIsolatedPeriod, ActIsolatedPeriodViewModel>();
            this.Container.RegisterViewModel<ActIsolatedProvidedDoc, ActIsolatedProvidedDocViewModel>();
            this.Container.RegisterViewModel<ActIsolatedRealObj, ActIsolatedRealObjViewModel>();
            this.Container.RegisterViewModel<ActIsolatedRealObjEvent, ActIsolatedRealObjEventViewModel>();
            this.Container.RegisterViewModel<ActIsolatedRealObjMeasure, ActIsolatedRealObjMeasureViewModel>();
            this.Container.RegisterViewModel<ActIsolatedRealObjViolation, ActIsolatedRealObjViolationViewModel>();
            this.Container.RegisterViewModel<ActIsolatedWitness, ActIsolatedWitnessViewModel>();

            this.Container.RegisterViewModel<SpecialAccountOwner, SpecialAccountOwnerViewModel>();
            this.Container.RegisterViewModel<SPAccOwnerRealityObject, SPAccOwnerRealityObjectViewModel>();
			this.Container.RegisterViewModel<EmailGji, EmailGjiViewModel>();
            this.Container.RegisterViewModel<EmailGjiAttachment, EmailGjiAttachmentViewModel>();
			this.Container.RegisterViewModel<OSP, OSPViewModel>();
        }

        private void RegisterService()
        {
            // документ гжи
            this.Container.Register(Component.For<IDocumentGjiService>().ImplementedBy<DocumentGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiInspectorService>().ImplementedBy<DocumentGjiInspectorService>().LifeStyle.Transient);

            // уведомление о начале предпринимательской деятельности
            this.Container.Register(Component.For<IServiceJuridalGjiService>().ImplementedBy<ServiceJuridalGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBusinessActivityService>().ImplementedBy<BusinessActivityService>().LifeStyle.Transient);

            // справочники

            this.Container.Register(Component.For<IViolationFeatureGjiService>().ImplementedBy<ViolationFeatureGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IStatSubsubjectGjiService>().ImplementedBy<StatSubsubjectGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IStatSubjectGjiService>().ImplementedBy<StatSubjectGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITypeSurveyGjiService>().ImplementedBy<TypeSurveyGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IHeatSeasonPeriodGjiService>().ImplementedBy<HeatSeasonPeriodGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViolationActionsRemovGjiService>().ImplementedBy<ViolationActionsRemovGjiService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IAuditPurposeSurveySubjectGjiService>().ImplementedBy<AuditPurposeSurveySubjectGjiService>().LifeStyle.Transient);

            // Отопительный сезон
            this.Container.Register(Component.For<IHeatSeasonService>().ImplementedBy<HeatSeasonService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IHeatSeasonDocService>().ImplementedBy<HeatSeasonDocService>().LifeStyle.Transient);

            // Решения
            this.Container.Register(Component.For<IDecisionService>().ImplementedBy<DecisionService>().LifeStyle.Transient);

            // Распоряжение
            this.Container.Register(Component.For<IDisposalService>().ImplementedBy<DisposalService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDisposalExpertService>().ImplementedBy<DisposalExpertService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDisposalTypeSurveyService>().ImplementedBy<DisposalTypeSurveyService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDisposalProvidedDocService>().ImplementedBy<DisposalProvidedDocService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDisposalViolService>().ImplementedBy<DisposalViolService>().LifeStyle.Transient);

            // Протокол
            this.Container.Register(Component.For<IProtocolArticleLawService>().ImplementedBy<ProtocolArticleLawService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IProtocolService>().ImplementedBy<ProtocolService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IProtocolViolationService>().ImplementedBy<ProtocolViolationService>().LifeStyle.Transient);

            // Предписание
            this.Container.Register(Component.For<IPrescriptionArticleLawService>().ImplementedBy<PrescriptionArticleLawService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IPrescriptionService>().ImplementedBy<PrescriptionService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IPrescriptionViolService>().ImplementedBy<PrescriptionViolService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IPrescriptionCancelViolReferenceService>().ImplementedBy<PrescriptionCancelViolReferenceService>().LifeStyle.Transient);

            // Представление
            this.Container.Register(Component.For<IPresentationService>().ImplementedBy<PresentationService>().LifeStyle.Transient);

            // обращения граждан
            this.Container.RegisterTransient<IAppealCitsService<ViewAppealCitizens>, AppealCitsService<ViewAppealCitizens>>();
            Container.Register(Component.For<IAppealCitsRealObjService>().ImplementedBy<AppealCitsRealObjService>().LifeStyle.Transient);
            Container.Register(Component.For<IAppealCitsStatSubjectService>().ImplementedBy<AppealCitsStatSubjectService>().LifeStyle.Transient);
            this.Container.RegisterService<IExportSuggestionService, ExportSuggestionService>();

            // Акт проверки
            this.Container.Register(Component.For<IActCheckService>().ImplementedBy<ActCheckService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IActCheckRealityObjectService>().ImplementedBy<ActCheckRealityObjectService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IActCheckInspectedPartService>().ImplementedBy<ActCheckInspectedPartService>().LifeStyle.Transient);
            this.Container.RegisterTransient<IActCheckProvidedDocService, ActCheckProvidedDocService>();
            
            // Акт обследования
            this.Container.Register(Component.For<IActSurveyService>().ImplementedBy<ActSurveyService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IActSurveyInspectedPartService>().ImplementedBy<ActSurveyInspectedPartService>().LifeStyle.Transient);

            // Акт устранения
            this.Container.Register(Component.For<IActRemovalService>().ImplementedBy<ActRemovalService>().LifeStyle.Transient);

            // Постановление
            this.Container.Register(Component.For<IResolutionService>().ImplementedBy<ResolutionService>().LifeStyle.Transient);

            // Постановление прокуратуры
            this.Container.Register(Component.For<IResolProsArticleLawService>().ImplementedBy<ResolProsArticleLawService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IResolProsRealityObjectService>().ImplementedBy<ResolProsRealityObjectService>().LifeStyle.Transient);

            // Постановление Роспотребнадзора
            this.Container.RegisterService<IResolutionRospotrebnadzorService, ResolutionRospotrebnadzorService>();

            // Акт без взаимодействия
            this.Container.RegisterService<IActIsolatedService, ActIsolatedService>();
            this.Container.RegisterService<IActIsolatedInspectedPartService, ActIsolatedInspectedPartService>();
            this.Container.RegisterService<IActIsolatedProvidedDocService, ActIsolatedProvidedDocService>();
            
            // Протокол МВД
            this.Container.Register(Component.For<IProtocolMvdArticleLawService>().ImplementedBy<ProtocolMvdArticleLawService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IProtocolMvdRealityObjectService>().ImplementedBy<ProtocolMvdRealityObjectService>().LifeStyle.Transient);

            // Протокол МЖК
            this.Container.Register(Component.For<IProtocolMhcArticleLawService>().ImplementedBy<ProtocolMhcArticleLawService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IProtocolMhcRealityObjectService>().ImplementedBy<ProtocolMhcRealityObjectService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IProtocolMhcService>().ImplementedBy<ProtocolMhcService>().LifeStyle.Transient);

            // Протокол РСО
            Container.Register(Component.For<IProtocolRSOArticleLawService>().ImplementedBy<ProtocolRSOArticleLawService>().LifeStyle.Transient);
            Container.Register(Component.For<IProtocolRSORealityObjectService>().ImplementedBy<ProtocolRSORealityObjectService>().LifeStyle.Transient);
            Container.Register(Component.For<IProtocolRSOService>().ImplementedBy<ProtocolRSOService>().LifeStyle.Transient);

            // Деятельность ТСЖ
            this.Container.Register(Component.For<IActivityTsjArticleService>().ImplementedBy<ActivityTsjArticleService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IActivityTsjRealObjService>().ImplementedBy<ActivityTsjRealObjService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IActivityTsjProtocolRealObjService>().ImplementedBy<ActivityTsjProtocolRealObjService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IActivityTsjService>().ImplementedBy<ActivityTsjService>().LifeStyle.Transient);

            // Основания проверок
            this.Container.Register(Component.For<IBasePlanActionService>().ImplementedBy<BasePlanActionService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBaseInsCheckService>().ImplementedBy<BaseInsCheckService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBaseJurPersonService>().ImplementedBy<BaseJurPersonService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBaseProsClaimService>().ImplementedBy<BaseProsClaimService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBaseDispHeadService>().ImplementedBy<BaseDispHeadService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBaseStatementService>().ImplementedBy<BaseStatementService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiInspectorService>().ImplementedBy<InspectionGjiInspectorService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiZonalInspectionService>().ImplementedBy<InspectionGjiZonalInspectionService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRealityObjectService>().ImplementedBy<InspectionGjiRealityObjectService>().LifeStyle.Transient);
            this.Container.RegisterTransient<IInspectionBaseContragentService, InspectionBaseContragentService>();

            this.Container.Register(Component.For<IReminderService>().ImplementedBy<ReminderService>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDisposalText>()
                .ImplementedBy<DisposalText>()
                .LifeStyle.Transient);

            // Сервисы для нумерации
            this.Container.Register(Component.For<INumberValidationRule>().ImplementedBy<BaseTatValidationRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<INumberValidationRule>().ImplementedBy<BasePermValidationRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IBaseStatementNumberRule>().ImplementedBy<BaseStatementNumberRuleTat>().LifeStyle.Transient);
            this.Container.Register(Component.For<IAppealCitsNumberRule>().ImplementedBy<AppealCitsNumberRuleTat>().LifeStyle.Transient);

            this.Container.Register(Component.For<IBaseStatementAction>().ImplementedBy<BaseStatementAction>().LifeStyle.Transient);

            this.Container.Register(Component.For<IGjiScriptService>().ImplementedBy<GjiScriptService>().LifeStyle.Transient);

            this.Container.Register(Component.For<IInspectionMenuService>().ImplementedBy<InspectionMenuService>().LifeStyle.Transient);

            // подача тепла
            this.Container.Register(Component.For<IHeatInputService>().ImplementedBy<HeatInputService>().LifeStyle.Transient);

            #region Правила проставления видов проверок
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseActTsjRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadCourtDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadCourtExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadCourtDocExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadPrescrDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadPrescrExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadPrescrDocExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseHeatSeasonRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseInsCheckRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseJurPersDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseJurPersExitDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseJurPersExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseProsClaimDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseProsClaimExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseStatDocRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseStatExitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseStatVisualRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DispPrescrCode1279Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DispPrescrCode34Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DispPrescrCode5Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DispPrescrCode6Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<DispPrescrCode8Rule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseJurPersVisitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseDispHeadVisitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseProsClaimVisitRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IKindCheckRule>().ImplementedBy<BaseStatVisitRule>().LifeStyle.Transient);
            #endregion

            this.Container.Register(Component.For<IProtocolDefinitionService>().ImplementedBy<ProtocolDefinitionService>().LifeStyle.Transient);
            this.Container.Register(Component.For<IResolutionDefinitionService>().ImplementedBy<ResolutionDefinitionService>().LifeStyle.Transient);

            this.Container.Register(Component.For<IForm1ContolServiceData>().ImplementedBy<Form1ContolServiceData>().LifeStyle.Transient);

            this.Container.Register(Component.For<IMonthlyProsecutorsOfficeServiceData>().ImplementedBy<MonthlyProsecutorsOfficeServiceData>().LifeStyle.Transient);
            this.Container.RegisterTransient<IGjiParamsService, GjiParamsService>();
            this.Container.RegisterTransient<IBaseLicenseApplicantsService, BaseLicenseApplicantsService>();

            // план проверки
            this.Container.RegisterTransient<ISurveyPlanService, SurveyPlanService>();
            this.Container.RegisterTransient<ILogEntityHistoryService, LogEntityHistoryService>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy01>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy02>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy03>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy04>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy05>();

            this.Container.RegisterTransient<ITypeSurveyLegalReasonService, TypeSurveyLegalReasonService>();
            this.Container.RegisterTransient<ITypeSurveyContragentTypeService, TypeSurveyContragentTypeService>();

            this.Container.RegisterService<IFuelInfoService, FuelInfoService>();

            this.Container.RegisterService<IGisGkhRegionalService, GisGkhRegionalService>();
        }

        private void RegisterTasks()
        {
            this.Container.RegisterTaskExecutor<CreateSurveyPlanCandidatesTaskExecutor>(CreateSurveyPlanCandidatesTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<EmailGjiTaskExecutor>(EmailGjiTaskExecutor.Id);
        }

        private void RegisterExports()
        {
            this.Container.Register(Component.For<IDataExportService>().Named("ActCheckDataExport").ImplementedBy<ActCheckDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActivityTsjDataExport").ImplementedBy<ActivityTsjDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActRemovalDataExport").ImplementedBy<ActRemovalDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActSurveyDataExport").ImplementedBy<ActSurveyDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseDispHeadDataExport").ImplementedBy<BaseDispHeadDataExport>().LifeStyle.Transient);
            this.Container.RegisterTransient<IDataExportService, WarningInspectionDataExport>("WarningInspectionDataExport");
            this.Container.RegisterTransient<IDataExportService, WarningDocDataExport>("WarningDocDataExport");
            this.Container.RegisterTransient<IDataExportService, MotivationConclusionDataExport>("MotivationConclusionDataExport");
            this.Container.Register(Component.For<IDataExportService>().Named("MKDLicRequestExport").ImplementedBy<MKDLicRequestExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("RequestDataExport").ImplementedBy<RequestDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("MKDLicRequestQueryDataExport").ImplementedBy<MKDLicRequestQueryDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseInsCheckDataExport").ImplementedBy<BaseInsCheckDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseJurPersonDataExport").ImplementedBy<BaseJurPersonDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseProsClaimDataExport").ImplementedBy<BaseProsClaimDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BusinessActivityDataExport").ImplementedBy<BusinessActivityDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("DisposalDataExport").ImplementedBy<DisposalDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("HeatSeasonDataExport").ImplementedBy<HeatSeasonDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("PrescriptionDataExport").ImplementedBy<PrescriptionDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("PresentationDataExport").ImplementedBy<PresentationDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ProtocolDataExport").ImplementedBy<ProtocolDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ResolProsDataExport").ImplementedBy<ResolProsDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ResolutionDataExport").ImplementedBy<ResolutionDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseStatementDataExport").ImplementedBy<BaseStatementDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseLicenseApplicantsDataExport").ImplementedBy<BaseLicenseApplicantsDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("DisposalNullInspectionDataExport").ImplementedBy<DisposalNullInspectionDataExport>().LifeStyle.Transient);
            this.Container.RegisterTransient<IAppealCitsDataExport, AppealCitsDataExport>();
            this.Container.Register(Component.For<IDataExportService>().Named("InspectedPartGjiDataExport").ImplementedBy<InspectedPartGjiDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ViolationGjiDataExport").ImplementedBy<ViolationGjiDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ProtocolMvdDataExport").ImplementedBy<ProtocolMvdDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ProtocolMhcDataExport").ImplementedBy<ProtocolMhcDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("ProtocolRSODataExport").ImplementedBy<ProtocolRSODataExport>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDataExportService>().Named("ReminderOfInspectorDataExport").ImplementedBy<ReminderOfInspectorDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ReminderOfHeadDataExport").ImplementedBy<ReminderOfHeadDataExport>().LifeStyle.Transient);

            this.Container.RegisterTransient<IDataExportService, ResolutionRospotrebnadzorDataExport>(ResolutionRospotrebnadzorDataExport.Id);
        }

        private void RegisterInterceptors()
        {
            this.Container.Register(Component.For<IDomainServiceInterceptor<EmailGji>>().ImplementedBy<EmailGjiServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<PrescriptionOfficialReport>>().ImplementedBy<PrescriptionOfficialReportInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActCheck>>().ImplementedBy<ActCheckServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActCheckPeriod>>().ImplementedBy<ActCheckPeriodServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActCheckViolation>>().ImplementedBy<ActCheckViolationServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActSurvey>>().ImplementedBy<ActSurveyServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActRemoval>>().ImplementedBy<ActRemovalServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActRemovalViolation>>().ImplementedBy<ActRemovalViolationServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Disposal>>().ImplementedBy<DisposalServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Decision>>().ImplementedBy<DecisionServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Prescription>>().ImplementedBy<PrescriptionInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<DecisionInspectionReason>>().ImplementedBy<DecisionInspectionReasonInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<PrescriptionViol>>().ImplementedBy<PrescriptionViolInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Protocol>>().ImplementedBy<ProtocolServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Resolution>>().ImplementedBy<ResolutionServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ResolPros>>().ImplementedBy<ResolProsServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseJurPerson>>().ImplementedBy<BaseJurPersonServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseDispHead>>().ImplementedBy<BaseDispHeadServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BasePlanAction>>().ImplementedBy<BasePlanActionServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseInsCheck>>().ImplementedBy<BaseInsCheckServiceInterceptor>().LifeStyle.Transient);
         
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseProsClaim>>().ImplementedBy<BaseProsClaimServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseHeatSeason>>().ImplementedBy<BaseHeatSeasonServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<HeatSeasonDoc>>().ImplementedBy<HeatSeasonDocServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseActivityTsj>>().ImplementedBy<BaseActivityTsjInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BusinessActivity>>().ImplementedBy<BusinessActivityServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActivityTsjStatute>>().ImplementedBy<ActivityTsjStatuteInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActivityTsjMember>>().ImplementedBy<ActivityTsjMemberInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Presentation>>().ImplementedBy<PresentationServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<Institutions>>().ImplementedBy<InstitutionsServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<KindCheckGji>>().ImplementedBy<KindCheckInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<AppealCitsServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<DocNumValidationRule>>().ImplementedBy<DocNumValidationRuleInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<HeatSeason>>().ImplementedBy<HeatSeasonInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<InspectionAppealCits>>().ImplementedBy<BaseStatementAppealCitsServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BaseDefault>>().ImplementedBy<BaseDefaultInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActivityTsj>>().ImplementedBy<ActivityTsjInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActivityTsjProtocol>>().ImplementedBy<ActivityTsjProtocolInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<InspectionGjiViol>>().ImplementedBy<InspectionGjiViolInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<AnswerContentGji>>().ImplementedBy<AnswerContentGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ArticleLawGji>>().ImplementedBy<ArticleLawGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<CompetentOrgGji>>().ImplementedBy<CompetentOrgGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ExecutantDocGji>>().ImplementedBy<ExecutantDocGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ExpertGji>>().ImplementedBy<ExpertGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<FeatureViolGji>>().ImplementedBy<FeatureViolGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<HeatSeasonPeriodGji>>().ImplementedBy<HeatSeasonPeriodGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<InspectedPartGji>>().ImplementedBy<InspectedPartGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<InstanceGji>>().ImplementedBy<InstanceGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<KindProtocolTsj>>().ImplementedBy<KindProtocolTsjInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<KindStatementGji>>().ImplementedBy<KindStatementGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<KindWorkNotifGji>>().ImplementedBy<KindWorkNotifGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<PlanInsCheckGji>>().ImplementedBy<PlanInsCheckGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<PlanJurPersonGji>>().ImplementedBy<PlanJurPersonGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ProvidedDocGji>>().ImplementedBy<ProvidedDocGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<RedtapeFlagGji>>().ImplementedBy<RedtapeFlagGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ResolveGji>>().ImplementedBy<ResolveGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<RevenueFormGji>>().ImplementedBy<RevenueFormGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<RevenueSourceGji>>().ImplementedBy<RevenueSourceGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<SanctionGji>>().ImplementedBy<SanctionGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<StatSubjectGji>>().ImplementedBy<StatSubjectGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<StatSubsubjectGji>>().ImplementedBy<StatSubsubjectGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<TypeCourtGji>>().ImplementedBy<TypeCourtGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<TypeSurveyGji>>().ImplementedBy<TypeSurveyGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ViolationGji>>().ImplementedBy<ViolationGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<CourtVerdictGji>>().ImplementedBy<CourtVerdictGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ProtocolMvd>>().ImplementedBy<ProtocolMvdServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<PlanActionGji>>().ImplementedBy<PlanActionGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<BoilerRoom>>().ImplementedBy<BoilerRoomInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<InspectionGji>>().ImplementedBy<InspectionGjiInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<HeatInputPeriod>>().ImplementedBy<HeatInputPeriodInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ProtocolMhc>>().ImplementedBy<ProtocolMhcServiceInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<ProtocolRSO>>().ImplementedBy<ProtocolRSOServiceInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<ActCheckRealityObject>>().ImplementedBy<ActCheckRealityObjectInterceptor>().LifeStyle.Transient);
            this.Container.RegisterDomainInterceptor<AppealCitsAnswer, AppealCitsAnswerInterceptor>();
            this.Container.RegisterDomainInterceptor<RelatedAppealCits, RelatedAppealCitsInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsQuestion, AppealCitsQuestionInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsHeadInspector, AppealCitsHeadInspectorInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsCategory, AppealCitsCategoryInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsDefinition, AppealCitsDefinitionInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsAttachment, AppealCitsAttachmentInterceptor>();
            this.Container.RegisterDomainInterceptor<OrganMvd, OrganMvdInterceptor>();
            this.Container.RegisterDomainInterceptor<ResolutionDecision, ResolutionDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsDecision, AppealCitsDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsStatSubject, AppealCitsStatSubjectInterceptor>();
            this.Container.RegisterDomainInterceptor<MKDLicRequest, MKDLicRequestInterceptor>();

            this.Container.Register(Component.For<IDomainServiceInterceptor<NormativeDoc>>().ImplementedBy<Bars.GkhGji.Interceptors.NormativeDocInterceptor>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainServiceInterceptor<NormativeDocItem>>().ImplementedBy<Bars.GkhGji.Interceptors.NormativeDocItemInterceptor>().LifeStyle.Transient);
            this.Container.RegisterDomainInterceptor<BaseLicenseApplicants, BaseLicenseApplicantsInterceptor>();
            this.Container.Register(Component.For<IDomainServiceInterceptor<AuditPurposeGji>>().ImplementedBy<AuditPurposeGjiInterceptor>().LifeStyle.Transient);

            this.Container.RegisterDomainInterceptor<SurveyPlan, SurveyPlanInterceptor>();
            this.Container.RegisterDomainInterceptor<SurveyPlanContragent, SurveyPlanContragentInterceptor>();
            this.Container.RegisterDomainInterceptor<ActionsRemovViol, ActionsRemovViolInterceptor>();

            this.Container.RegisterDomainInterceptor<ResolutionRospotrebnadzor, ResolutionRospotrebnadzorServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<FuelInfoPeriod, FuelInfoPeriodInterceptor>();
            
            //акт без взаимодействия
            this.Container.RegisterDomainInterceptor<ActIsolated, ActIsolatedInterceptor>();
            this.Container.RegisterDomainInterceptor<ActIsolatedPeriod, ActIsolatedPeriodInterceptor>();
            this.Container.RegisterDomainInterceptor<ActIsolatedRealObj, ActIsolatedRealObjInterceptor>();
            this.Container.RegisterDomainInterceptor<ActIsolatedRealObjViolation, ActIsolatedRealObjViolationInterceptor>();
            
            this.Container.RegisterDomainInterceptor<AppealCitsSource, AppealCitsSourceInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsRealityObject, AppealCitsRealityObjectInterceptor>();

            Container.RegisterDomainInterceptor<SpecialAccountReport, SpecialAccountReportInterceptor>();
            Container.RegisterDomainInterceptor<SpecialAccountRow, SpecialAccountRowInterceptor>();

            Container.RegisterDomainInterceptor<ActCheckAnnex, ActCheckAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckControlListAnswer, ActCheckControlListAnswerInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckControlMeasures, ActCheckControlMeasuresInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckDefinition, ActCheckDefinitionInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckInspectedPart, ActCheckInspectedPartInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckProvidedDoc, ActCheckProvidedDocInterceptor>();
            Container.RegisterDomainInterceptor<ActCheckWitness, ActCheckWitnessInterceptor>();

            Container.RegisterDomainInterceptor<ActSurveyAnnex, ActSurveyAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ActSurveyInspectedPart, ActSurveyInspectedPartInterceptor>();
            Container.RegisterDomainInterceptor<ActSurveyOwner, ActSurveyOwnerInterceptor>();
            Container.RegisterDomainInterceptor<ActSurveyPhoto, ActSurveyPhotoInterceptor>();
            Container.RegisterDomainInterceptor<ActSurveyRealityObject, ActSurveyRealityObjectInterceptor>();

            Container.RegisterDomainInterceptor<DecisionAdminRegulation, DecisionAdminRegulationInterceptor>();
            Container.RegisterDomainInterceptor<DecisionAnnex, DecisionAnnexInterceptor>();
            Container.RegisterDomainInterceptor<DecisionControlList, DecisionControlListInterceptor>();
            Container.RegisterDomainInterceptor<DecisionControlMeasures, DecisionControlMeasuresInterceptor>();
            Container.RegisterDomainInterceptor<DecisionControlSubjects, DecisionControlSubjectsInterceptor>();
            Container.RegisterDomainInterceptor<DecisionExpert, DecisionExpertInterceptor>();
            Container.RegisterDomainInterceptor<DecisionProvidedDoc, DecisionProvidedDocInterceptor>();
            Container.RegisterDomainInterceptor<DecisionVerificationSubject, DecisionVerificationSubjectInterceptor>();

            Container.RegisterDomainInterceptor<DisposalAdminRegulation, DisposalAdminRegulationInterceptor>();
            Container.RegisterDomainInterceptor<DisposalExpert, DisposalExpertInterceptor>();
            Container.RegisterDomainInterceptor<DisposalInspFoundationCheck, DisposalInspFoundationCheckInterceptor>();
            Container.RegisterDomainInterceptor<DisposalInspFoundation, DisposalInspFoundationInterceptor>();
            Container.RegisterDomainInterceptor<DisposalInspFoundCheckNormDocItem, DisposalInspFoundCheckNormDocItemInterceptor>();
            Container.RegisterDomainInterceptor<DisposalProvidedDoc, DisposalProvidedDocInterceptor>();
            Container.RegisterDomainInterceptor<DisposalSurveyObjective, DisposalSurveyObjectiveInterceptor>();
            Container.RegisterDomainInterceptor<DisposalSurveyPurpose, DisposalSurveyPurposeInterceptor>();
            Container.RegisterDomainInterceptor<DisposalTypeSurvey, DisposalTypeSurveyInterceptor>();
            Container.RegisterDomainInterceptor<DisposalVerificationSubject, DisposalVerificationSubjectInterceptor>();

            Container.RegisterDomainInterceptor<PrescriptionArticleLaw, PrescriptionArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<PrescriptionCloseDoc, PrescriptionCloseDocInterceptor>();

            Container.RegisterDomainInterceptor<PresentationAnnex, PresentationAnnexInterceptor>();

            Container.RegisterDomainInterceptor<PreventiveVisitAnnex, PreventiveVisitAnnexInterceptor>();
            Container.RegisterDomainInterceptor<PreventiveVisitRealityObject, PreventiveVisitRealityObjectInterceptor>();
            Container.RegisterDomainInterceptor<PreventiveVisitResult, PreventiveVisitResultInterceptor>();
            Container.RegisterDomainInterceptor<PreventiveVisitWitness, PreventiveVisitWitnessInterceptor>();

            Container.RegisterDomainInterceptor<ProtocolArticleLaw, ProtocolArticleLawInterceptor>();

            Container.RegisterDomainInterceptor<ProtocolMhcAnnex, ProtocolMhcAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMhcArticleLaw, ProtocolMhcArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMhcDefinition, ProtocolMhcDefinitionInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMhcRealityObject, ProtocolMhcRealityObjectInterceptor>();

            Container.RegisterDomainInterceptor<ProtocolMvdAnnex, ProtocolMvdAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMvdArticleLaw, ProtocolMvdArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolMvdRealityObject, ProtocolMvdRealityObjectInterceptor>();

            Container.RegisterDomainInterceptor<ProtocolRSOAnnex, ProtocolRSOAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolRSOArticleLaw, ProtocolRSOArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolRSODefinition, ProtocolRSODefinitionInterceptor>();
            Container.RegisterDomainInterceptor<ProtocolRSORealityObject, ProtocolRSORealityObjectInterceptor>();

            Container.RegisterDomainInterceptor<ResolProsAnnex, ResolProsAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ResolProsArticleLaw, ResolProsArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ResolProsRealityObject, ResolProsRealityObjectInterceptor>();

            Container.RegisterDomainInterceptor<ResolutionDispute, ResolutionDisputeInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionPayFine, ResolutionPayFineInterceptor>();

            
            Container.RegisterDomainInterceptor<ResolutionRospotrebnadzorAnnex, ResolutionRospotrebnadzorAnnexInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionRospotrebnadzorArticleLaw, ResolutionRospotrebnadzorArticleLawInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionRospotrebnadzorDefinition, ResolutionRospotrebnadzorDefinitionInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionRospotrebnadzorDispute, ResolutionRospotrebnadzorDisputeInterceptor>();
            Container.RegisterDomainInterceptor<ResolutionRospotrebnadzorPayFine, ResolutionRospotrebnadzorPayFineInterceptor>();
        }

        private void RegisterReports()
        {
            // Печатные формы
            Container.RegisterTransient<IGkhBaseReport, EmailGjiReport>();
            Container.RegisterTransient<IGkhBaseReport, EmailGjiPOSReport>();
            Container.RegisterTransient<IGkhBaseReport, EmailGjiAcceptedReport>();
            Container.RegisterTransient<IGkhBaseReport, EmailGjiPOSAcceptedReport>();
            Container.RegisterTransient<IGkhBaseReport, GisGkhAppealReport>();
            Container.RegisterTransient<IGkhBaseReport, AppealCitsDecisionReport>();
            Container.RegisterTransient<IGkhBaseReport, ResolutionDecisionReport>();
            Container.RegisterTransient<IGkhBaseReport, RenewalApplicationReport>();
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<SpecialAccountReportReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiNotificationReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiStateToProsecReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ActCheckGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ActCheckGjiDefinitionReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ActSurveyGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ActRemovalGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiNotificationReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiDefinitionReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiNotificationReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<ResolutionGjiDefinitionReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiCancelReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<PresentationGjiReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<BusinessActivityReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<NotificationAttendanceAtProtocolReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<NotificationAttendanceByRepresentativeCheckResultsReport>().LifeStyle.Transient);

            Container.RegisterTransient<IGkhBaseReport, DecisionNortificationReport>();

            // отчеты
            this.Container.RegisterTransient<IPrintForm, FillDocumentsGjiReport>("GJI Report.FillDocumentsGji");
            this.Container.RegisterTransient<IPrintForm, ActProtocolReport>("GJI Report.ActProtocolReport");
            this.Container.RegisterTransient<IPrintForm, ActResolutionReport>("GJI Report.ActResolutionReport");
            this.Container.RegisterTransient<IPrintForm, ActPresentationReport>("GJI Report.ActPresentationReport");
            this.Container.RegisterTransient<IPrintForm, ActPrescriptionReport>("GJI Report.ActPrescriptionReport");
            this.Container.RegisterTransient<IPrintForm, Form123Report>("GJI Report.Form123Report");
            this.Container.RegisterTransient<IPrintForm, ProtocolTotalTable>("GJI Report.ProtocolTotalTable");
            this.Container.RegisterTransient<IPrintForm, ControlDocGjiExecution>("GJI Report.ControlDocGjiExecution");
            this.Container.RegisterTransient<IPrintForm, ProtocolResponsibility>("GJI Report.ProtocolResponsibility");
            this.Container.RegisterTransient<IPrintForm, MonthlyReportToProsecutors>("GJI Report.MonthlyReportToProsecutors");
            this.Container.RegisterTransient<IPrintForm, PrepareHeatSeasonReport>("GJI Report.PrepareHeatSeasonReport");
            this.Container.RegisterTransient<IPrintForm, HeatInputInformationReport>("GJI Report.HeatInputInformationReport");
            this.Container.RegisterTransient<IPrintForm, WorkWinterInfoReport>("GJI Report.WorkWinterInfoReport");
            this.Container.RegisterTransient<IPrintForm, HeatSeasonReadinessReport>("GJI Report.HeatSeasonReadinessReport");
            this.Container.RegisterTransient<IPrintForm, Form1ControllReport>("GJI Report.Form1ControllReport");
            this.Container.RegisterTransient<IPrintForm, GjiWorkReport>("GJI Report.GjiWorkReport");
            this.Container.RegisterTransient<IPrintForm, JournalAppeals>("GJI Report.JournalAppeals");
            this.Container.RegisterTransient<IPrintForm, ReviewAppealsCitsReport>("GJI Report.ReviewAppealsCitsReport");
            this.Container.RegisterTransient<IPrintForm, SubjectRequestsReport>("GJI Report.SubjectRequestsReport");
            this.Container.RegisterTransient<IPrintForm, StatisticsAppealsCitsReport>("GJI Report.StatisticsAppealsCitsReport");
            this.Container.RegisterTransient<IPrintForm, Form1Control_v2Report>("GJI Report.Form1Control_v2Report");
            this.Container.RegisterTransient<IPrintForm, MonthlyProsecutorsOfficeReport>("GJI Report.MonthlyProsecutorsOfficeReport");
            this.Container.RegisterTransient<IPrintForm, ReportOnCourseOfHeatingSeason>("GJI Report.ReportOnCourseOfHeatingSeason");
            this.Container.RegisterTransient<IPrintForm, SubjectRequestsUkTsj>("GJI Report.SubjectRequestsUkTsj");
            this.Container.RegisterTransient<IPrintForm, CheckingExecutionOfPrescriptionReport>("GJI Report.CheckingExecutionOfPrescriptionReport");
            this.Container.RegisterTransient<IPrintForm, RegistryNotificationCommencementBusiness>("GJI Report.RegistryNotificationCommencementBusiness");
            this.Container.RegisterTransient<IPrintForm, Form123ExtendedReport>("GJI Report.Form123ExtendedReport");
            this.Container.RegisterTransient<IPrintForm, HeatSeasonReceivedDocumentsReport>("GJI Report.HeatSeasonReceivedDocumentsReport");
            this.Container.RegisterTransient<IPrintForm, InformationOfManagOrg>("GJI Report.InformationOfManagOrg");
            this.Container.RegisterTransient<IPrintForm, ManagementSysReport>("GJI Report Bars.Gkh ManagementSysExport");
            this.Container.RegisterTransient<IPrintForm, AnalyticalReportBySubject>("GJI Report.AnalyticalReportBySubject");
            this.Container.Register(
                Component.For<IPrintForm, IPivotModel>()
                         .Named("Report.OlapByInspectionReport")
                         .ImplementedBy<OlapByInspectionReport>()
                         .LifeStyle.Transient);
        }

        private void RegisterStates()
        {
            this.Container.RegisterTransient<IStateChangeHandler, DocumentGjiStateChangeHandler>("GkhGji DocumentGjiStateChangeHandler");
            this.Container.RegisterTransient<IStateChangeHandler, BusinessActivityStateChangeHandler>("GkhGji BusinessActivityStateChangeHandler");

            this.Container.RegisterTransient<IStateChangeHandler, SurveyPlanStateChangeHandler>();

            this.Container.RegisterTransient<IRuleChangeStatus, DocumentGjiValidationRule>();

            //РТ
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActSurveyValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PresentationValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolProsValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationNumberTatRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DocGjiValidationNumberTatRule>();

            // Пермь
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalValidationNumberPermRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationNumberPermRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationNumberPermRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationNumberPermRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PresentationValidationNumberPermRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationNumberPermRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationNumberPermRule>();

            //проверка заполненности карточки документов
            this.Container.RegisterTransient<IRuleChangeStatus, ActCheckValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActRemovalValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ActSurveyValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PrescriptionValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, PresentationValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolProsValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, AppealCitsValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolMvdValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ProtocolMhcValidationRule>();
            Container.RegisterTransient<IRuleChangeStatus, ProtocolRSOValidationRule>();

            this.Container.RegisterTransient<IRuleChangeStatus, InspectionValidationRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, DisposalProsClaimRule>();

            this.Container.RegisterTransient<IRuleChangeStatus, ManOrgLicenseAddDateStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, ManOrgLicenseRemoveDateStateRule>();

            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionRospotrebnadzorValidationRule>();
        }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterInspectionRules()
        {
            // регистрируем провайдер для правил
            this.Container.RegisterTransient<IInspectionGjiProvider, InspectionGjiProvider>();

            // Правила создания из оснований проверок -> Распоряжение
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseJurPersonToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseDispHeadToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseActivityTsjToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseHeatSeasonToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseInsCheckToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseProsClaimToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseStatementToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseStatementToDecisionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseDispHeadToDecisionlRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BasePlanActionToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IInspectionGjiRule>().ImplementedBy<BaseLicenseApplicantsToDisposalRule>().LifeStyle.Transient);

            // Правила для распоряжения
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckByRoRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckPrescriptionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActSurveyRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<DisposalToActCheckWithoutRoRule>().LifeStyle.Transient);

            // акт проверки
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToPrescriptionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToProtocolRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToDisposalRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActCheckToDisposalBaseRule>().LifeStyle.Transient);
            this.Container.RegisterTransient<IDocumentGjiRule, ActCheckToResolutionRospotrebnadzorRule>();

            // акт устранения нарушений
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActRemovalToPrescriptionRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActRemovalToProtocolRule>().LifeStyle.Transient);

            // предписание
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<PrescriptionToProtocolRule>().LifeStyle.Transient);

            // протокол
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ProtocolToResolutionRule>().LifeStyle.Transient);
            // постановление прокуратуры
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ResolProsToResolutionRule>().LifeStyle.Transient);
            // постановление
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ResolutionToPresentationRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ResolutionToProtocolRule>().LifeStyle.Transient);
            // протокол МВД
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ProtocolMvdToResolutionRule>().LifeStyle.Transient);

            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ProtocolMhcToResolutionRule>().LifeStyle.Transient);
            Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ProtocolRSOToResolutionRule>().LifeStyle.Transient);

            this.Container.Register(Component.For<ISMEVRule>().ImplementedBy<SMEVRule>().LifeStyle.Transient);

            //акт без взаимодействия
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<TaskDisposalToActIsolatedRule>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDocumentGjiRule>().ImplementedBy<ActIsolatedToWarningDocRule>().LifeStyle.Transient);
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<AdditionalAppealsDeleteAction>();
            this.Container.RegisterExecutionAction<WorkWinterMarkFillAction>();
            this.Container.RegisterExecutionAction<NormativeDocInitializationAction>();
            this.Container.RegisterExecutionAction<FeautureViolUpdateAction>();
            this.Container.RegisterExecutionAction<ViolationUpdateByNormativeDocAction>();
            this.Container.RegisterExecutionAction<GenerateReminderAction>();
            this.Container.RegisterExecutionAction<RemoveViolationsWithDeletedRealtyObjAction>();
            this.Container.RegisterExecutionAction<FillAppealSortGjiNumberValueAction>();
            this.Container.RegisterExecutionAction<TypeSurveyDictMigrationAction>();
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.RegisterExportableEntity<AuditResultFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditEventExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditObjectExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditPlaceExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditPlanExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditResultExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<AuditFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<FrguFuncExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<GjiExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PreceptAuditExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PreceptHouseExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ProtocolAuditExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PreceptFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ProtocolFilesExportableEntity>(this.Container);

            ContainerHelper.RegisterProxySelectorService<AuditPlanProxy, AuditPlanSelectorService>();
            ContainerHelper.RegisterProxySelectorService<AuditPlaceProxy, AuditPlaceSelectorService>();
            ContainerHelper.RegisterProxySelectorService<AuditProxy, AuditSelectorService>();
            ContainerHelper.RegisterProxySelectorService<AuditResultProxy, AuditResultSelectorService>();
            ContainerHelper.RegisterProxySelectorService<AuditResultFilesProxy, AuditResultFilesSelectorService>();
            ContainerHelper.RegisterProxySelectorService<AuditFilesProxy, AuditFilesSelectorService>();
            ContainerHelper.RegisterProxySelectorService<ProtocolAuditProxy, ProtocolAuditSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PreceptAuditProxy, PreceptAuditSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PreceptHouseProxy, PreceptHouseSelectorService>();
            ContainerHelper.RegisterProxySelectorService<GjiProxy, GjiSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<PreceptFilesProxy, PreceptFilesSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<ProtocolFilesProxy, ProtocolFilesSelectorService>(this.Container);
            
            ContainerHelper.RegisterFormatDataExportRepository<ViewFormatDataExportInspection, FormatDataExportInspectionRepository>();
            ContainerHelper.RegisterFormatDataExportRepository<Prescription, FormatDataExportPrescriptionRepository>();
        }
    }
}
