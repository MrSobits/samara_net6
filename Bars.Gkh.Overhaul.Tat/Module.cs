namespace Bars.Gkh.Overhaul.Tat
{
    using B4;
    using B4.IoC;
    using B4.Modules.DataExport;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using B4.Modules.FileStorage.DomainService;
    using B4.Modules.NHibernateChangeLog;
    using B4.Modules.Reports;
    using B4.Modules.States;
    using B4.Windsor;
    using Bars.B4.Modules.Analytics.Reports.Params;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Modules.Reforma;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.Overhaul.Tat.Import.DpkrDocumentImport;
    using Bars.Gkh.Overhaul.Tat.Modules.Reforma;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Entities;
    using Castle.MicroKernel.Registration;
    using Controllers;
    using DomainService;
    using DomainService.FormingOfCr;
    using DomainService.Impl;
    using Entities;
    using ExecutionAction;
    using Export;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhCr.DomainService;
    using Import;
    using ImportExport;
    using Interceptors;
    using LogMap;
    using Navigation;
    using Overhaul.Domain;
    using Overhaul.DomainService;
    using Overhaul.Entities;
    using Overhaul.Navigation;
    using PriorityParams;
    using PriorityParams.Impl;
    using ProgrammPriorityParams;
    using Reports;
    using Services.Impl;
    using StateChange;
    using ValueResolver;
    using ViewModel;
    using ViewModel.PublishedProgram;
    using RealityObjMenuProvider = Navigation.RealityObjMenuProvider;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IEntityExportProvider, EntityExportProvider>();
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Gkh.Overhaul.Tat statefulentity");

            this.Container.RegisterGkhConfig<OverhaulTatConfig>();

            Component.For<IDpkrCorrectionDataProvider>().ImplementedBy<FakeDpkrCorrectionProvider>().LifestyleTransient().RegisterIn(this.Container);
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<SubsidyList>().Named("SubsidyList").LifestyleTransient());

            this.Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient());
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());

            Component.For<IProgrammPriorityParam>().ImplementedBy<StructuralElementWearoutParam>().Named(StructuralElementWearoutParam.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IProgrammPriorityParam>().ImplementedBy<RoBuildYearParam>().Named(RoBuildYearParam.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IProgrammPriorityParam>().ImplementedBy<WorkVolumeParam>().Named(WorkVolumeParam.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IProgrammPriorityParam>().ImplementedBy<NeedOverhaulParam>().Named(NeedOverhaulParam.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IProgrammPriorityParam>().ImplementedBy<LastOverhaulYearParam>().Named(LastOverhaulYearParam.Code).LifeStyle.Transient.RegisterIn(this.Container);

            #region Reports
            this.Container.RegisterTransient<IPrintForm, ActualizeVersionLogReport>("ActualizeVersionLogReport");

            this.Container.Register(Component.For<IPrintForm>().ImplementedBy<ControlCertificationOfBuild>().Named("ControlCertificationOfBuild").LifestyleTransient());
            this.Container.Register(Component.For<IPrintForm>().ImplementedBy<CtrlCertOfBuildConsiderMissingCeo>().Named("CtrlCertOfBuildConsiderMissingCeo").LifestyleTransient());
            this.Container.Register(Component.For<IPrintForm>().ImplementedBy<PublishedDpkrReport>().Named("PublishedDpkrReport").LifestyleTransient());

            this.Container.RegisterTransient<IPrintForm, LongProgramReport>("LongProgramReport");
            this.Container.RegisterTransient<IPrintForm, CountRoByMuInPeriod>("CountRoByMuInPeriod");

            this.Container.Register(Component.For<IPrintForm>().ImplementedBy<FillingControlRepairReport>().Named("FillingControlRepairReport").LifestyleTransient());
            this.Container.RegisterTransient<IPrintForm, ConsolidatedCertificationReport>("ConsolidatedCertificationReport");

            this.Container.RegisterTransient<IPrintForm, SpecialAccountDecisionReport>("SpecialAccountDecisionReport");
            this.Container.RegisterTransient<IPrintForm, FormFundNotSetMkdInfoReport>("FormFundNotSetMkdInfoReport");
            this.Container.RegisterTransient<IPrintForm, CertificationControlValues>("CertificationControlValues");
            this.Container.RegisterTransient<IPrintForm, LongProgramByTypeWork>("LongProgramByTypeWork");
            this.Container.RegisterTransient<IPrintForm, ShortProgramByTypeWork>("ShortProgramByTypeWork");
            this.Container.RegisterTransient<IPrintForm, OwnersProtocolsControlManOrgReport>("Report Bars.Gkh OwnersProtocolsControlManOrg");
            this.Container.RegisterTransient<IPrintForm, GisuRealObjContractWithFundDecision>("GJI Report.GisuRealObjContractWithFundDecision");

            this.Container.ReplaceComponent<IPrintForm>(
                typeof(Gkh.Report.RoomAreaControlReport),
                Component
                    .For<IPrintForm>()
                    .ImplementedBy<Reports.RoomAreaControlReport>()
                    .LifestyleTransient()
                    .Named("Report Bars.Gkh.Overhaul.Tat RoomAreaControl"));

            this.Container.ReplaceTransient<IPrintForm, GkhCr.Report.ListByManyApartmentsHouses, Reports.ListByManyApartmentsHouses>("CR Tat Report.ListByManyApartmentsHouses");
            this.Container.ReplaceTransient<IPrintForm, GkhCr.Report.PlanedProgramIndicatorsReport, Reports.PlanedProgramIndicatorsReport>("CR Tat Report.PlanedProgramIndicatorsReport");
            this.Container.ReplaceTransient<IPrintForm, GkhCr.Report.RegisterMkdToBeRepaired, Reports.RegisterMkdToBeRepaired>("CR Tat Report.RegisterMkdToBeRepaired");

            #endregion

            #region Imports
            this.Container.RegisterImport<RealtyObjectImport>();

            #endregion Imports

            this.Container.RegisterTransient<IPriorityParams, WearoutPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, YearExploitationPriorityParam>();

            this.Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<EnergyPassportPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<ProjDocsPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<WorkDocsPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<PaymentSizeCrPriorityParam>().LifestyleTransient());

            this.Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<CapitalGroupPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<ComplexGroupPriorityParam>().LifestyleTransient());

            this.Container.RegisterTransient<IRuleChangeStatus, ProgramVersionStateChangeRule>();

            this.Container.RegisterTransient<IRealityObjectsInPrograms, RealityObjectsInPrograms>();
            this.Container.RegisterTransient<IRealityObjectsProgramVersion, RealityObjectsProgramVersion>();
            this.Container.RegisterTransient<IRegOpAccountDecisionRo, RegOpAccountDecisionRo>();

            this.RegisterBundlers();

            this.RegisterControllers();

            this.RegisterDomainServices();

            this.RegisterExecuteActions();

            this.RegisterExports();

            this.RegisterImports();

            this.RegisterInterceptors();

            this.RegisterNavigations();

            this.RegisterServices();

            this.RegisterValueResolvers();

            this.RegisterViewModels();

            this.RegisterDataProviders();

            this.RegisterCatalogs();

            // Регистрация классов для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>()
                .Named(string.Format("{0} dependencies", this.AssemblyName))
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.RegisterAuditLogMap();

            this.RegisterFormatDataExport();

            this.Container.RegisterTransient<IViewCollection, GkhOvrhlViewCollection>("GkhOvrhlTatViewCollection");
        }

        private void RegisterDataProviders()
        {
            this.Container.ReplaceComponent(Component.For<IRealityObjectDecisionProtocolProxyService>()
                .ImplementedBy<RealityObjectDecisionProtocolProxyService>().LifestyleTransient());

            this.Container.ReplaceComponent(Component.For<ITypeOfFormingCrProvider>()
                    .UsingFactoryMethod(container => container.Resolve<IRealityObjectDecisionProtocolProxyService>()).LifestyleTransient());

            this.Container.RegisterTransient<IPropertyOwnerProtocolsProvider, PropertyOwnerProtocolsProvider>();
        }

        private void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogRegistrator>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterAltDataController<VersionActualizeLog>();
            this.Container.RegisterController<MultiPriorityParamController>();
            this.Container.RegisterAltDataController<QualityPriorityParam>();
            this.Container.RegisterAltDataController<QuantPriorityParam>();
            this.Container.RegisterController<PublishedProgramController>();
            this.Container.RegisterController<PublishedProgramRecordController>();
            this.Container.RegisterController<PriorityParamController>();
            this.Container.RegisterController<RealityObjectStructuralElementInProgrammController>();
            this.Container.RegisterAltDataController<RealityObjectStructuralElementInProgrammStage2>();
            this.Container.RegisterController<RealityObjectStructuralElementInProgrammStage3Controller>();
            this.Container.RegisterController<SubsidyMunicipalityController>();
            this.Container.RegisterController<SubsidyMunicipalityRecordController>();
            this.Container.RegisterController<OvrhlWorkController>();
            this.Container.RegisterController<FileStorageDataController<PropertyOwnerProtocols>>();
            this.Container.RegisterController<PropertyOwnerDecisionWorkController>();
            this.Container.RegisterController<VersionRecordController>();
            this.Container.RegisterAltDataController<BasePropertyOwnerDecision>();
            this.Container.RegisterController<FileStorageDataController<SpecialAccountDecision>>();
            this.Container.RegisterAltDataController<RegOpAccountDecision>();
            this.Container.RegisterAltDataController<ListServicesDecision>();
            this.Container.RegisterAltDataController<MinFundSizeDecision>();
            this.Container.RegisterAltDataController<AccountOperation>();
            this.Container.RegisterAltDataController<CurrentPrioirityParams>();
            this.Container.RegisterAltDataController<DpkrGroupedYear>();
            this.Container.RegisterController<DpkrCorrectionStage2Controller>();
            this.Container.RegisterController<LongTermPrObjectController>();
            this.Container.RegisterAltDataController<SpecialAccount>();
            this.Container.RegisterAltDataController<AccrualsAccount>();
            this.Container.RegisterAltDataController<RealAccount>();
            this.Container.RegisterAltDataController<RealAccountOperation>();
            this.Container.RegisterAltDataController<SpecialAccountOperation>();
            this.Container.RegisterAltDataController<AccrualsAccountOperation>();
            this.Container.RegisterAltDataController<PaymentAccount>();
            this.Container.RegisterAltDataController<AccBankStatement>();
            this.Container.RegisterAltDataController<BaseOperation>();
            this.Container.RegisterAltDataController<ListServiceDecisionWorkPlan>();
            this.Container.RegisterAltDataController<RealEstateTypeCommonParam>();
            this.Container.RegisterAltDataController<RealEstateTypeStructElement>();
            this.Container.RegisterAltDataController<VersionParam>();
            this.Container.RegisterAltDataController<RealEstateTypePriorityParam>();
            this.Container.RegisterController<RealEstateTypeRateController>();
            this.Container.RegisterAltDataController<MinAmountDecision>();
            this.Container.RegisterController<SpecialAccountDecisionNoticeController>();
            this.Container.RegisterController<ProgramVersionController>();
            this.Container.RegisterController<CommonParamsController>();
            this.Container.RegisterController<OverhaulScriptController>();
            this.Container.RegisterController<OverhaulMenuController>();
            this.Container.RegisterController<RealityObjMenuController>();
            this.Container.RegisterController<ShortProgramRecordController>();
            this.Container.RegisterAltDataController<ShortProgramRealityObject>();
            this.Container.RegisterController<ShortProgramDefectListController>();
            this.Container.RegisterController<ShortProgramProtocolController>();
            this.Container.RegisterAltDataController<YearCorrection>();
            this.Container.RegisterAltDataController<DpkrDocument>();
            this.Container.RegisterController<DpkrDocumentRealityObjectController>();
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterDomainService<ProgramVersion, ProgramVersionDomainService>();
            this.Container.RegisterDomainService<PropertyOwnerProtocols, FileStorageDomainService<PropertyOwnerProtocols>>();
            this.Container.RegisterDomainService<SpecialAccountDecision, FileStorageDomainService<SpecialAccountDecision>>();
            this.Container.RegisterDomainService<SpecialAccountDecisionNotice, FileStorageDomainService<SpecialAccountDecisionNotice>>();
            this.Container.RegisterDomainService<ShortProgramDefectList, FileStorageDomainService<ShortProgramDefectList>>();
            this.Container.RegisterDomainService<ShortProgramProtocol, FileStorageDomainService<ShortProgramProtocol>>();
            this.Container.RegisterTransient<IProgramCrRealityObjectService, ProgramCrRealityObjectService>();
            this.Container.RegisterTransient<ICalcAccountOwnerDecisionService, CalcAccountOwnerDecisionService>();
            this.Container.RegisterFileStorageDomainService<DpkrDocument>();
        }

        private void RegisterExecuteActions()
        {
            this.Container.RegisterExecutionAction<ProgramCrChangesCreationAction>();
            this.Container.RegisterExecutionAction<PublishedProgramMassiveCreationAction>();
            this.Container.RegisterExecutionAction<TypeWorkCrSetYearRepairAction>();
            this.Container.RegisterExecutionAction<OverhaulTatConfigMigrationAction>();
            this.Container.RegisterExecutionAction<SetTypeOfFormingCrRealityObjAction>();
            this.Container.RegisterExecutionAction<FillCalcAccountAction>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportReport, RealityObjectInProgramStage3Report>("RealObjStructElemlnProgStg3DataExport");
            this.Container.RegisterTransient<IDataExportService, DpkrCorrectionStage2Export>("DpkrCorrectionStage2Export");
            this.Container.RegisterTransient<IDataExportService, ShortProgramRecordExport>("ShortProgramRecordExport");
            this.Container.RegisterTransient<IDataExportService, VersionRecordDataExport>("VersionRecordDataExport");
            this.Container.RegisterTransient<IDataExportService, DecisionNoticeExport>("DecisionNoticeExport");
            this.Container.RegisterTransient<IDataExportService, PublishedProgramRecordExport>("PublishedProgramRecordExport");
        }

        private void RegisterImports()
        {
            this.Container.RegisterImport<DpkrDocumentImport>(DpkrDocumentImport.Id);
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<SpecialAccount, SpecialAccountInterceptor>();
            this.Container.RegisterDomainInterceptor<RealAccount, RealAccountInterceptor>();
            this.Container.RegisterDomainInterceptor<SubsidyMunicipality, SubsidyMunicipalityInterceptor>();
            this.Container.RegisterDomainInterceptor<ProgramVersion, ProgramVersionInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObject, RealityObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<SubsidyRecord, SubsidyRecordInterceptor>();
            this.Container.RegisterDomainInterceptor<ShortProgramRecord, ShortProgramRecordInterceptor>();
            this.Container.RegisterDomainInterceptor<ShortProgramDefectList, ShortProgramDefectListInterceptor>();
            this.Container.RegisterDomainInterceptor<LongTermPrObject, LongTermPrObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<SpecialAccountDecision, SpecialAccountDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<MinAmountDecision, MinAmountDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<SpecialAccountDecisionNotice, SpecialAccountDecisionNoticeInterceptor>();
            this.Container.RegisterDomainInterceptor<RegOpAccountDecision, RegOpAccountDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<ListServicesDecision, ListServicesDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<PropertyOwnerProtocols, PropertyOwnerProtocolsInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectStructuralElement, RealityObjectStructuralElementInterceptor>();
            this.Container.RegisterDomainInterceptor<YearCorrection, YearCorrectionInterceptor>();
            this.Container.RegisterDomainInterceptor<DpkrDocument, DpkrDocumentInterceptor>();
            this.Container.ReplaceComponent<IDomainServiceInterceptor<TypeWorkCrRemoval>>(
                typeof(GkhCr.Interceptors.TypeWorkCrRemovalInterceptor),
                Component.For<IDomainServiceInterceptor<TypeWorkCrRemoval>>()
                         .ImplementedBy<TypeWorkCrRemovalInterceptor>()
                         .LifeStyle.Transient);
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, LongTermPrObjectFullMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, LongTermPrObjectMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, LongTermPrObjectRealMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, LongTermPrObjectSpecialMenuProvider>();
            this.Container.ReplaceComponent(
                typeof(INavigationProvider),
                typeof(PaysizeNavigationProvider),
                Component
                    .For<INavigationProvider>()
                    .ImplementedBy<TatPaysizeNavigationProvider>()
                    .LifestyleTransient());
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IActualizeVersionLogService, ActualizeVersionLogService>();
            this.Container.RegisterTransient<IProgramCrCopByDpkr, ProgramCrCopByDpkr>();
            this.Container.RegisterTransient<IActualizeVersionService, ActualizeVersionService>();
            this.Container.RegisterTransient<IObjectCrIntegrationService, ObjectCrIntegrationService>();
            this.Container.RegisterTransient<IDpkrCorrectionService, DpkrCorrectionService>();
            this.Container.RegisterTransient<ILongProgramService, LongProgramService>();
            this.Container.RegisterTransient<ISubsidyMunicipalityService, SubsidyMunicipalityService>();
            this.Container.RegisterTransient<IPropertyOwnerDecisionWorkService, PropertyOwnerDecisionWorkService>();
            this.Container.RegisterTransient<IRealEstateTypeRateService, RealEstateTypeRateService>();
            this.Container.RegisterTransient<IPublishProgramService, PublishedProgramService>();
            this.Container.RegisterTransient<IDpkrParamsService, ConfigDpkrParamsService>();
            this.Container.ReplaceComponent(Component.For<IListCeoService>().ImplementedBy<TatListCeoService>().LifestyleTransient());
            this.Container.RegisterTransient<IRegOpAccountService, RegOpAccountService>();
            this.Container.RegisterTransient<IJobService, JobService>();
            this.Container.RegisterTransient<IOverhaulScriptService, OverhaulScriptService>();
            this.Container.RegisterTransient<IPriorityParamService, PriorityParamService>();
            this.Container.RegisterTransient<IShortProgramRecordService, ShortProgramRecordService>();
            this.Container.RegisterTransient<IStage2Service, Stage2Service>();
            this.Container.RegisterTransient<IProgramVersionService, ProgramVersionService>();
            this.Container.RegisterTransient<ILongTermPrObjectService, LongTermPrObjectService>();
            this.Container.RegisterTransient<IDecisionNoticeService, DecisionNoticeService>();
            this.Container.RegisterTransient<IRealObjStructElementService, RealityObjectStructElementService>();
            this.Container.RegisterTransient<IShortProgramDefectListService, ShortProgramDefectListService>();
            this.Container.RegisterTransient<IDpkrService, TatDpkrService>();
            this.Container.RegisterTransient<IRealityObjectDpkrDataService, TatRealityObjectDpkrDataService>();
            this.Container.RegisterTransient<IPublishProgramWcfService, TatPublishProgramWcfService>();
            this.Container.RegisterTransient<IRepairKonstWcfService, TatRepairKonstWcfService>();
            this.Container.RegisterTransient<IDpkrTypeWorkService, DpkrTypeWorkService>();
            this.Container.RegisterTransient<IOverhaulViewModels, OverhaulViewModels>();
            this.Container.RegisterTransient<ITypeWorkStage1Service, TypeWorkStage1Service>();
            this.Container.RegisterTransient<IChangeVersionSt1Service, ChangeVersionSt1Service>();
            this.Container.RegisterTransient<IGetProgramVersionService, GetProgramVersionService>();
            this.Container.RegisterTransient<IYearCorrectionConfigService, YearCorrectionConfigService>();
            this.Container.RegisterTransient<IRealObjectStructElementService, RealObjectStructElementService>();
            this.Container.RegisterTransient<IDpkrDocumentRealityObjectService, DpkrDocumentRealityObjectService>();
            this.Container.RegisterTransient<BaseRealObjOverhaulDataObject, GkhRealObjOverhaulDataObject>("RealObjOverhaulDataObject");

            this.Container.ReplaceComponent(Component.For<IDefectService>().ImplementedBy<DefectService>().LifestyleTransient());
            this.Container.ReplaceComponent(
                Component.For<IRealtyObjectAccountFormationService>().ImplementedBy<RealtyObjectAccountFormationService>().LifestyleTransient());
        }

        private void RegisterValueResolvers()
        {
            this.Container.RegisterTransient<IFormulaParameter, RoBuildYearResolver>(RoBuildYearResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, RoFirstPrivatizationYearResolver>(RoFirstPrivatizationYearResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, RoWearoutResolver>(RoWearoutResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, ElementWearoutResolver>(ElementWearoutResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, ElementLastRepairYearResolver>(ElementLastRepairYearResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, ElementVolumeResolver>(ElementVolumeResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, ElementLifeTimeResolver>(ElementLifeTimeResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, CurrentYearResolver>(CurrentYearResolver.Id);
            this.Container.RegisterTransient<IFormulaParameter, DpkrEndYearResolver>(DpkrEndYearResolver.Id);
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<VersionActualizeLog, VersionActualizeLogViewModel>();
            this.Container.RegisterViewModel<PublishedProgramRecord, PublishedProgramRecordViewModel>();
            this.Container.RegisterViewModel<CurrentPrioirityParams, CurrentPrioirityParamsViewModel>();
            this.Container.RegisterViewModel<RealityObjectStructuralElementInProgramm, RealityObjectStructuralElementInProgrammViewModel>();
            this.Container.RegisterViewModel<RealityObjectStructuralElementInProgrammStage2, RealityObjectStructuralElementInProgrammStage2ViewModel>();
            this.Container.RegisterViewModel<RealityObjectStructuralElementInProgrammStage3, RealityObjectStructuralElementInProgrammStage3ViewModel>();
            this.Container.RegisterViewModel<SubsidyMunicipalityRecord, SubsidyMunicipalityRecordViewModel>();
            this.Container.RegisterViewModel<SpecialAccount, SpecialAccountViewModel>();
            this.Container.RegisterViewModel<AccrualsAccount, AccrualsAccountViewModel>();
            this.Container.RegisterViewModel<RealAccount, RealAccountViewModel>();
            this.Container.RegisterViewModel<RealAccountOperation, RealAccountOperationViewModel>();
            this.Container.RegisterViewModel<SpecialAccountOperation, SpecialAccountOperationViewModel>();
            this.Container.RegisterViewModel<AccrualsAccountOperation, AccrualsAccountOperationViewModel>();
            this.Container.RegisterViewModel<SpecialAccountDecisionNotice, SpecialAccountDecisionNoticeViewModel>();
            this.Container.RegisterViewModel<MinFundSizeDecision, MinFundSizeDecisionViewModel>();
            this.Container.RegisterViewModel<DpkrGroupedYear, DpkrGroupedYearModel>();
            this.Container.RegisterViewModel<DpkrCorrectionStage2, DpkrCorrectionViewModel>();
            this.Container.RegisterViewModel<LongTermPrObject, LongTermPrObjViewModel>();
            this.Container.RegisterViewModel<PropertyOwnerProtocols, PropertyOwnerProtocolsViewModel>();
            this.Container.RegisterViewModel<BasePropertyOwnerDecision, BasePropertyOwnerDecisionViewModel>();
            this.Container.RegisterViewModel<SpecialAccountDecision, SpecialAccountDecisionViewModel>();
            this.Container.RegisterViewModel<RegOpAccountDecision, RegOpAccountDecisionViewModel>();
            this.Container.RegisterViewModel<PropertyOwnerDecisionWork, PropertyOwnerDecisionWorkViewModel>();
            this.Container.RegisterViewModel<RealEstateType, RealEstateTypeViewModel>();
            this.Container.RegisterViewModel<RealEstateTypeCommonParam, RealEstateTypeCommonParamViewModel>();
            this.Container.RegisterViewModel<RealEstateTypeStructElement, RealEstateTypeStructElementViewModel>();
            this.Container.RegisterViewModel<RealEstateTypePriorityParam, RealEstateTypePriorityParamViewModel>();
            this.Container.RegisterViewModel<RealEstateTypeRate, RealEstateTypeRateViewModel>();
            this.Container.RegisterViewModel<VersionRecord, VersionRecordViewModel>();
            this.Container.RegisterViewModel<VersionParam, VersionParamViewModel>();
            this.Container.RegisterViewModel<ProgramVersion, ProgramVersionViewModel>();
            this.Container.RegisterViewModel<QualityPriorityParam, QualityPriorityParamViewModel>();
            this.Container.RegisterViewModel<QuantPriorityParam, QuantPriorityParamViewModel>();
            this.Container.RegisterViewModel<MultiPriorityParam, MultiPriorityParamViewModel>();
            this.Container.RegisterViewModel<ShortProgramRealityObject, ShortProgramRealityObjectViewModel>();
            this.Container.RegisterViewModel<ShortProgramRecord, ShortProgramRecordViewModel>();
            this.Container.RegisterViewModel<ShortProgramDefectList, ShortProgramDefectListViewModel>();
            this.Container.RegisterViewModel<ShortProgramProtocol, ShortProgramProtocolViewModel>();
            this.Container.RegisterViewModel<MinAmountDecision, MinAmountDecisionViewModel>();
            this.Container.RegisterViewModel<PaymentAccount, PaymentAccountViewModel>();
            this.Container.RegisterViewModel<AccBankStatement, AccBankStatementViewModel>();
            this.Container.RegisterViewModel<BaseOperation, BaseOperationViewModel>();
            this.Container.RegisterViewModel<ListServiceDecisionWorkPlan, ListServiceDecisionWorkPlanViewModel>();
            this.Container.RegisterViewModel<DpkrDocument, DpkrDocumentViewModel>();
            this.Container.RegisterViewModel<DpkrDocumentRealityObject, DpkrDocumentRealityObjectViewModel>();
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.RegisterProxySelectorService<PkrProxy, PkrSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PkrDomProxy, PkrDomSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PkrDomWorkProxy, PkrDomWorkSelectorService>();

            ContainerHelper.ReplaceProxySelectorService<KapremDecisionsProxy,
                Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl.KapremDecisionsSelectorService,
                KapremDecisionsSelectorService>();

            ContainerHelper.ReplaceProxySelectorService<NpaProxy,
                Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl.NpaSelectorService,
                NpaSelectorService>();

            ContainerHelper.ReplaceProxySelectorService<ProtocolossProxy,
                Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl.ProtocolossSelectorService,
                ProtocolossSelectorService>();
        }

        private void RegisterCatalogs()
        {
            CatalogRegistry.Add(new Catalog
            {
                Display = "Справочник программ КР (множественный выбор)",
                Id = "CrProgramMulti",
                SelectFieldClass = "B4.catalogs.ProgramVersionsMultiSelectField"
            });
        }
    }
}