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
    using Bars.GkhGji.Controllers.FuelInfo;
    using Bars.GkhGji.Controllers.ResolutionRospotrebnadzor;
    using Bars.GkhGji.DomainService.Dict;
    using Bars.GkhGji.DomainService.Dict.Impl;
    using Bars.GkhGji.DomainService.FuelInfo;
    using Bars.GkhGji.DomainService.FuelInfo.Impl;
    using Bars.GkhGji.DomainService.Impl;
    using Bars.GkhGji.DomainService.Inspection.Impl;
    using Bars.GkhGji.FormatDataExport.Domain.Impl;
    using Bars.GkhGji.FormatDataExport.ExportableEntities.Impl;
    using Bars.GkhGji.FormatDataExport.ProxySelectors.Impl;
    using Bars.GkhGji.Interceptors.FuelInfo;
    using Bars.GkhGji.Interceptors.ResolutionRospotrebnadzor;
    using Bars.GkhGji.LogMap.Provider;
    using Bars.GkhGji.Utils;
    using Bars.GkhGji.ViewModel.Dict;
    using Bars.GkhGji.ViewModel.FuelInfo;
    using Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor;

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

            this.RegisterFormatDataExport();
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
            this.Container.RegisterAltDataController<ControlActivity>();
            this.Container.RegisterAltDataController<PlanActionGji>();
            this.Container.RegisterAltDataController<PlanJurPersonGji>();
            this.Container.RegisterAltDataController<ExpertGji>();
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

            this.Container.RegisterController<AppealCitsController>();
            this.Container.RegisterController<AppealCitsRealObjectController>();
            this.Container.RegisterController<AppealCitsStatSubjectController>();
            this.Container.RegisterController<FileStorageDataController<AppealCitsAnswer>>();
            this.Container.RegisterController<FileStorageDataController<AppealCitsRequest>>();
            this.Container.RegisterController<AppealCitsCategoryController>();
            this.Container.RegisterController<AppealCitsQuestionController>();
            this.Container.RegisterController<AppealCitsHeadInspectorController>();
            this.Container.RegisterController<AppealCitsAnswerStatSubjectController>();
            this.Container.RegisterAltDataController<AppealCitsSource>();
            this.Container.RegisterAltDataController<AppealCitsAttachment>();
            this.Container.RegisterAltDataController<AppealCitsAnswerAttachment>();

            this.Container.RegisterAltDataController<BaseDefault>();
            this.Container.RegisterAltDataController<BaseActivityTsj>();
            this.Container.RegisterController<InspectionGjiViolController>();
            this.Container.RegisterController<ReminderController>();

            this.Container.RegisterController<InspectionController>();
            this.Container.RegisterController<InspectionBaseContragentController>();
            #endregion

            this.Container.RegisterAltDataController<InspectionGjiViolStage>();

            //документ гжи
            this.Container.RegisterController<DocumentGjiController>();
            this.Container.RegisterController<DocumentGjiInspectorController>();
            this.Container.RegisterAltDataController<DocumentGjiChildren>();

            //распоряжение
            this.Container.RegisterController<DisposalController>();
            this.Container.RegisterAltDataController<ViewDisposalWidget>();
            this.Container.RegisterController<DisposalExpertController>();
            this.Container.RegisterController<DisposalTypeSurveyController>();
            this.Container.RegisterController<DisposalProvidedDocController>();
            this.Container.RegisterController<FileStorageDataController<DisposalAnnex>>();
            this.Container.RegisterController<DisposalViolController>();

            //акт проверки
            this.Container.RegisterController<ActCheckController>();
            this.Container.RegisterController<ActCheckRealityObjectController>();
            this.Container.RegisterController<ActCheckInspectedPartController>();
            this.Container.RegisterController<ActCheckProvidedDocController>();
            //Container.RegisterAltDataController<ActCheckViolation>();
            this.Container.RegisterAltDataController<ActCheckPeriod>();
            this.Container.RegisterController<ActCheckWitnessController>();
            this.Container.RegisterAltDataController<ActCheckDefinition>();
            this.Container.RegisterController<FileStorageDataController<ActCheckAnnex>>();

            //акт обследования
            this.Container.RegisterController<ActSurveyController>();
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
            this.Container.RegisterAltDataController<ViewPrescriptionWidget>();
            this.Container.RegisterController<PrescriptionViolController>();
            this.Container.RegisterController<PrescriptionArticleLawController>();
            this.Container.RegisterController<PrescriptionCancelController>();
            this.Container.RegisterController<PrescriptionCancelViolReferenceController>();
            this.Container.RegisterController<FileStorageDataController<PrescriptionAnnex>>();

            //протокол
            //ReplaceController чтобы избежать конфликта с Bars.KP60.Protocol.Controllers.ProtocolController при роутинге
            this.Container.ReplaceController<ProtocolController>("protocol");
            this.Container.RegisterController<ProtocolViolationController>();
            this.Container.RegisterController<ProtocolArticleLawController>();
            this.Container.RegisterController<ProtocolDefinitionController>();
            this.Container.RegisterController<FileStorageDataController<ProtocolAnnex>>();

            //постановление
            this.Container.RegisterController<ResolutionController>();
            this.Container.RegisterAltDataController<ResolutionPayFine>();
            this.Container.RegisterController<ResolutionDefinitionController>();
            this.Container.RegisterController<FileStorageDataController<ResolutionAnnex>>();
            this.Container.RegisterController<FileStorageDataController<ResolutionDispute>>();

            //постановление прокуратуры
            this.Container.RegisterController<ResolProsController>();
            this.Container.RegisterController<ResolProsArticleLawController>();
            this.Container.RegisterController<ResolProsRealityObjectController>();
            this.Container.RegisterController<FileStorageDataController<ResolProsAnnex>>();

            // Постановление Роспотребнадзора
            this.Container.RegisterController<ResolutionRospotrebnadzorController>();
            this.Container.RegisterFileStorageDataController<ResolutionRospotrebnadzorAnnex>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorDefinition>();
            this.Container.RegisterFileStorageDataController<ResolutionRospotrebnadzorDispute>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorPayFine>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorViolation>();
            this.Container.RegisterAltDataController<ResolutionRospotrebnadzorArticleLaw>();

            //протокол МВД
            this.Container.RegisterController<ProtocolMvdController>();
            this.Container.RegisterController<ProtocolMvdArticleLawController>();
            this.Container.RegisterController<ProtocolMvdRealityObjectController>();
            this.Container.RegisterController<FileStorageDataController<ProtocolMvdAnnex>>();

            //протокол МЖК
            this.Container.RegisterController<ProtocolMhcController>();
            this.Container.RegisterController<ProtocolMhcArticleLawController>();
            this.Container.RegisterController<ProtocolMhcRealityObjectController>();
            this.Container.RegisterController<ProtocolMhcDefinitionController>();
            this.Container.RegisterController<FileStorageDataController<ProtocolMhcAnnex>>();

            //представление
            this.Container.RegisterController<PresentationController>();
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

            this.Container.RegisterAltDataController<InspectionBaseType>();

            this.Container.RegisterAltDataController<DisposalAdminRegulation>();
            this.Container.RegisterAltDataController<DisposalInspFoundation>();
            this.Container.RegisterAltDataController<DisposalInspFoundCheckNormDocItem>();
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
        }

        private void RegisterDomainService()
        {
            //акт проверки
            this.Container.RegisterDomainService<ActCheckAnnex, FileStorageDomainService<ActCheckAnnex>>();
            this.Container.Register(Component.For<IDomainService<ActivityTsjProtocol>>().ImplementedBy<FileStorageDomainService<ActivityTsjProtocol>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActivityTsjStatute>>().ImplementedBy<FileStorageDomainService<ActivityTsjStatute>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActivityTsjMember>>().ImplementedBy<FileStorageDomainService<ActivityTsjMember>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActSurveyAnnex>>().ImplementedBy<FileStorageDomainService<ActSurveyAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ActSurveyPhoto>>().ImplementedBy<FileStorageDomainService<ActSurveyPhoto>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCits>>().ImplementedBy<GkhFileStorageDomainService<AppealCits>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCitsAnswer>>().ImplementedBy<FileStorageDomainService<AppealCitsAnswer>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<AppealCitsRequest>>().ImplementedBy<FileStorageDomainService<AppealCitsRequest>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BusinessActivity>>().ImplementedBy<FileStorageDomainService<BusinessActivity>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<DisposalAnnex>>().ImplementedBy<FileStorageDomainService<DisposalAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<HeatSeasonDoc>>().ImplementedBy<FileStorageDomainService<HeatSeasonDoc>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BaseDispHead>>().ImplementedBy<FileStorageDomainService<BaseDispHead>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BaseInsCheck>>().ImplementedBy<FileStorageDomainService<BaseInsCheck>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<BaseProsClaim>>().ImplementedBy<FileStorageDomainService<BaseProsClaim>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<PrescriptionAnnex>>().ImplementedBy<FileStorageDomainService<PrescriptionAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<PresentationAnnex>>().ImplementedBy<FileStorageDomainService<PresentationAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolProsAnnex>>().ImplementedBy<FileStorageDomainService<ResolProsAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolutionAnnex>>().ImplementedBy<FileStorageDomainService<ResolutionAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ResolutionDispute>>().ImplementedBy<FileStorageDomainService<ResolutionDispute>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolMvdAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolMvdAnnex>>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDomainService<ProtocolMhcAnnex>>().ImplementedBy<FileStorageDomainService<ProtocolMhcAnnex>>().LifeStyle.Transient);
            this.Container.RegisterDomainService<ActIsolatedAnnex, FileStorageDomainService<ActIsolatedAnnex>>();
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

            this.Container.RegisterDomainService<SurveyPlanContragentAttachment, FileStorageDomainService<SurveyPlanContragentAttachment>>();

            this.Container.RegisterTransient<IBaseJurPersonContragentService, BaseJurPersonContragentService>();
            this.Container.RegisterTransient<IDisposalAdminRegulationService, DisposalAdminRegulationService>();
            this.Container.RegisterTransient<IDisposalInsFoundationCheckService, DisposalInsFoundationCheckService>();
            this.Container.RegisterTransient<IDisposalInsFoundationService, DisposalInsFoundationService>();
            this.Container.RegisterTransient<IDisposalSurveyObjectiveService, DisposalSurveyObjectiveService>();
            this.Container.RegisterTransient<IDisposalSurveyPurposeService, DisposalSurveyPurposeService>();
            this.Container.RegisterTransient<IDisposalVerificationSubjectService, DisposalVerificationSubjectService>();
            this.Container.RegisterTransient<IViewInspectionRepository, ViewInspectionRepository>();

            this.Container.RegisterDomainService<DisposalInspFoundationCheck, DisposalInspFoundationCheckDomainService>();

        }

        private void RegisterViewModels()
        {
            this.Container.Register(Component.For<IViewModel<InspectionGjiViolStage>>().ImplementedBy<InspectionViolStageViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<BasePlanAction>>().ImplementedBy<BasePlanActionViewModel>().LifeStyle.Transient);

            //документ гжи
            this.Container.Register(Component.For<IViewModel<DocumentGji>>().ImplementedBy<DocumentGjiViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<DocumentGjiChildren>>().ImplementedBy<DocumentGjiChildrenViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<Reminder>>().ImplementedBy<ReminderViewModel>().LifeStyle.Transient);

            this.Container.Register(Component.For<IViewModel<ActCheck>>().ImplementedBy<ActCheckViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckAnnex>>().ImplementedBy<ActCheckAnnexViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckDefinition>>().ImplementedBy<ActCheckDefinitionViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckInspectedPart>>().ImplementedBy<ActCheckInspectedPartViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckPeriod>>().ImplementedBy<ActCheckPeriodViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckRealityObject>>().ImplementedBy<ActCheckRealityObjectViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckViolation>>().ImplementedBy<ActCheckViolationViewModel>().LifeStyle.Transient);
            this.Container.Register(Component.For<IViewModel<ActCheckWitness>>().ImplementedBy<ActCheckWitnessViewModel>().LifeStyle.Transient);
            
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
            this.Container.Register(Component.For<IViewModel<ResolutionDefinition>>().ImplementedBy<ResolutionDefinitionViewModel>().LifeStyle.Transient);

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

            this.Container.RegisterViewModel<BaseJurPersonContragent, BaseJurPersonContragentViewModel>();
            this.Container.RegisterViewModel<DisposalAdminRegulation, DisposalAdminRegulationViewModel>();
            this.Container.RegisterViewModel<DisposalInspFoundationCheck, DisposalInspFoundationCheckViewModel>();
            this.Container.RegisterViewModel<DisposalInspFoundation, DisposalInspFoundationViewModel>();
            this.Container.RegisterViewModel<DisposalInspFoundCheckNormDocItem, DisposalInspFoundCheckNormDocItemViewModel>();
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
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy01>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy02>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy03>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy04>();
            this.Container.RegisterTransient<ISurveyPlanStrategy, SurveyPlanStrategy05>();

            this.Container.RegisterTransient<ITypeSurveyLegalReasonService, TypeSurveyLegalReasonService>();
            this.Container.RegisterTransient<ITypeSurveyContragentTypeService, TypeSurveyContragentTypeService>();

            this.Container.RegisterService<IFuelInfoService, FuelInfoService>();
        }

        private void RegisterTasks()
        {
            this.Container.RegisterTaskExecutor<CreateSurveyPlanCandidatesTaskExecutor>(CreateSurveyPlanCandidatesTaskExecutor.Id);
        }

        private void RegisterExports()
        {
            this.Container.Register(Component.For<IDataExportService>().Named("ActCheckDataExport").ImplementedBy<ActCheckDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActivityTsjDataExport").ImplementedBy<ActivityTsjDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActRemovalDataExport").ImplementedBy<ActRemovalDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActSurveyDataExport").ImplementedBy<ActSurveyDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ActIsolatedDataExport").ImplementedBy<ActIsolatedDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("BaseDispHeadDataExport").ImplementedBy<BaseDispHeadDataExport>().LifeStyle.Transient);
            this.Container.RegisterTransient<IDataExportService, WarningInspectionDataExport>("WarningInspectionDataExport");
            this.Container.RegisterTransient<IDataExportService, WarningDocDataExport>("WarningDocDataExport");
            this.Container.RegisterTransient<IDataExportService, MotivationConclusionDataExport>("MotivationConclusionDataExport");
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

            this.Container.Register(Component.For<IDataExportService>().Named("ReminderOfInspectorDataExport").ImplementedBy<ReminderOfInspectorDataExport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IDataExportService>().Named("ReminderOfHeadDataExport").ImplementedBy<ReminderOfHeadDataExport>().LifeStyle.Transient);

            this.Container.RegisterTransient<IDataExportService, ResolutionRospotrebnadzorDataExport>(ResolutionRospotrebnadzorDataExport.Id);
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<ActCheck, ActCheckServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckPeriod, ActCheckPeriodServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActCheckViolation, ActCheckViolationServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActSurvey, ActSurveyServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActRemoval, ActRemovalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActRemovalViolation, ActRemovalViolationServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Disposal, DisposalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Prescription, PrescriptionInterceptor>();
            this.Container.RegisterDomainInterceptor<PrescriptionViol, PrescriptionViolInterceptor>();
            this.Container.RegisterDomainInterceptor<PrescriptionCancel, PrescriptionCancelInterceptor>();
            this.Container.RegisterDomainInterceptor<Protocol, ProtocolServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Resolution, ResolutionServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ResolPros, ResolProsServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseJurPerson, BaseJurPersonServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseDispHead, BaseDispHeadServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BasePlanAction, BasePlanActionServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseInsCheck, BaseInsCheckServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseStatement, BaseStatementServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseProsClaim, BaseProsClaimServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseHeatSeason, BaseHeatSeasonServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<HeatSeasonDoc, HeatSeasonDocServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseActivityTsj, BaseActivityTsjInterceptor>();
            this.Container.RegisterDomainInterceptor<BusinessActivity, BusinessActivityServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActivityTsjStatute, ActivityTsjStatuteInterceptor>();
            this.Container.RegisterDomainInterceptor<ActivityTsjMember, ActivityTsjMemberInterceptor>();
            this.Container.RegisterDomainInterceptor<Presentation, PresentationServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Institutions, InstitutionsServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionRisk, InspectionRiskInterceptor>();
            this.Container.RegisterDomainInterceptor<KindCheckGji, KindCheckInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCits, AppealCitsServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<DocNumValidationRule, DocNumValidationRuleInterceptor>();
            this.Container.RegisterDomainInterceptor<HeatSeason, HeatSeasonInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionAppealCits, BaseStatementAppealCitsServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseDefault, BaseDefaultInterceptor>();
            this.Container.RegisterDomainInterceptor<ActivityTsj, ActivityTsjInterceptor>();
            this.Container.RegisterDomainInterceptor<ActivityTsjProtocol, ActivityTsjProtocolInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionGjiViol, InspectionGjiViolInterceptor>();
            this.Container.RegisterDomainInterceptor<AnswerContentGji, AnswerContentGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ArticleLawGji, ArticleLawGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<CompetentOrgGji, CompetentOrgGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ExecutantDocGji, ExecutantDocGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ExpertGji, ExpertGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<FeatureViolGji, FeatureViolGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<HeatSeasonPeriodGji, HeatSeasonPeriodGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectedPartGji, InspectedPartGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<InstanceGji, InstanceGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<KindProtocolTsj, KindProtocolTsjInterceptor>();
            this.Container.RegisterDomainInterceptor<KindStatementGji, KindStatementGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<KindWorkNotifGji, KindWorkNotifGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<PlanInsCheckGji,PlanInsCheckGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<PlanJurPersonGji, PlanJurPersonGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ProvidedDocGji, ProvidedDocGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<RedtapeFlagGji, RedtapeFlagGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ResolveGji, ResolveGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<RevenueFormGji, RevenueFormGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<RevenueSourceGji, RevenueSourceGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<SanctionGji, SanctionGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<StatSubjectGji, StatSubjectGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<StatSubsubjectGji, StatSubsubjectGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeCourtGji, TypeCourtGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeSurveyGji, TypeSurveyGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ViolationGji, ViolationGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<CourtVerdictGji, CourtVerdictGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<ProtocolMvd, ProtocolMvdServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<PlanActionGji, PlanActionGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<BoilerRoom, BoilerRoomInterceptor>();
            this.Container.RegisterDomainInterceptor<InspectionGji, InspectionGjiInterceptor>();
            this.Container.RegisterDomainInterceptor<HeatInputPeriod, HeatInputPeriodInterceptor>();
            this.Container.RegisterDomainInterceptor<ProtocolMhc, ProtocolMhcServiceInterceptor>();
            this.Container.RegisterDomainInterceptor < ActCheckRealityObject,ActCheckRealityObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<AppealCitsAnswer, AppealCitsAnswerInterceptor>();
            this.Container.RegisterDomainInterceptor<OrganMvd, OrganMvdInterceptor>();
            this.Container.ReplaceComponent<IDomainServiceInterceptor<NormativeDoc>, Bars.Gkh.Interceptors.NormativeDocInterceptor>(
                Component.For<IDomainServiceInterceptor<NormativeDoc>>().ImplementedBy<Bars.GkhGji.Interceptors.NormativeDocInterceptor>().LifestyleTransient());
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
        }

        private void RegisterReports()
        {
            // Печатные формы
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiReport>().LifeStyle.Transient);
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

            // отчеты
            this.Container.RegisterTransient<IPrintForm, FillDocumentsGjiReport>("GJI Report.FillDocumentsGji");
            this.Container.RegisterTransient<IPrintForm, ActProtocolReport>("GJI Report.ActProtocolReport");
            this.Container.RegisterTransient<IPrintForm, ActResolutionReport>("GJI Report.ActResolutionReport");
            this.Container.RegisterTransient<IPrintForm, ActPresentationReport>("GJI Report.ActPresentationReport");
            this.Container.RegisterTransient<IPrintForm, ActPrescriptionReport>("GJI Report.ActPrescriptionReport");
            this.Container.RegisterTransient<IPrintForm, Form123Report>("GJI Report.Form123Report");
            this.Container.RegisterTransient<IPrintForm, ProtocolTotalTable>("GJI Report.ProtocolTotalTable");
            this.Container.RegisterTransient<IPrintForm, ControlDocGjiExecution>("GJI Report.ControlDocGjiExecution");
            this.Container.Register(
                Component.For<IPrintForm>().ImplementedBy<ProtocolResponsibility>().Named("Report Bars.GJI ProtocolResponsibility").LifestyleTransient());
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
            ContainerHelper.RegisterProxySelectorService<GjiProxy, GjiSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PreceptFilesProxy, PreceptFilesSelectorService>();
            ContainerHelper.RegisterProxySelectorService<ProtocolFilesProxy, ProtocolFilesSelectorService>();

            ContainerHelper.RegisterFormatDataExportRepository<ViewFormatDataExportInspection, FormatDataExportInspectionRepository>();
            ContainerHelper.RegisterFormatDataExportRepository<Prescription, FormatDataExportPrescriptionRepository>();
        }
    }
}