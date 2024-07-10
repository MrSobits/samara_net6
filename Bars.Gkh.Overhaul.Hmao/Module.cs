namespace Bars.Gkh.Overhaul.Hmao
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Params;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Import.RoImport;
    using Bars.Gkh.Overhaul.CommonParams;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Domain.Impl;
    using Bars.Gkh.Overhaul.Domain.RealityObjectServices;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.CommonParams;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Controllers;
    using Bars.Gkh.Overhaul.Hmao.DataProviders;
    using Bars.Gkh.Overhaul.Hmao.Domain.Impl;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Impl;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.Overhaul.Hmao.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.Export;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.Overhaul.Hmao.Import;
    using Bars.Gkh.Overhaul.Hmao.Import.ImportPublishYear;
    using Bars.Gkh.Overhaul.Hmao.Import.ImportDpkr;
    using Bars.Gkh.Overhaul.Hmao.Import.Program;
    using Bars.Gkh.Overhaul.Hmao.Import.ReformGkh;
    using Bars.Gkh.Overhaul.Hmao.Interceptors;
    using Bars.Gkh.Overhaul.Hmao.Navigation;
    using Bars.Gkh.Overhaul.Hmao.PriorityParams;
    using Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;
    using Bars.Gkh.Overhaul.Hmao.Reports;
    using Bars.Gkh.Overhaul.Hmao.Services.Impl;
    using Bars.Gkh.Overhaul.Hmao.StateChange;
    using Bars.Gkh.Overhaul.Hmao.SystemDataTransfer;
    using Bars.Gkh.Overhaul.Hmao.ValueResolver;
    using Bars.Gkh.Overhaul.Hmao.ViewModel;
    using Bars.Gkh.Overhaul.Hmao.ViewModel.Version;
    using Bars.Gkh.Report;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Castle.MicroKernel.Registration;

    using CommonEstateObjectService = Bars.Gkh.Overhaul.DomainService.Impl.CommonEstateObjectService;
    using IDpkrService = Bars.Gkh.Overhaul.Hmao.DomainService.IDpkrService;
    using RealEstateTypeInterceptor = Bars.Gkh.Interceptors.RealEstateType.RealEstateTypeInterceptor;
    using RoomAreaControlReport = Bars.Gkh.Report.RoomAreaControlReport;
    using TypeWorkCrRemovalInterceptor = Bars.GkhCr.Interceptors.TypeWorkCrRemovalInterceptor;
    using WorkPriceDomainService = Bars.Gkh.Overhaul.DomainService.WorkPriceDomainService;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.Domain.Impl;
    using Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets;
    using Bars.Gkh.Overhaul.ViewModel;

    using RealityObjectStructuralElementViewModel = Bars.Gkh.Overhaul.Hmao.ViewModel.RealityObject.RealityObjectStructuralElementViewModel;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Gkh.Overhaul.Hmao statefulentity");
            this.Container.RegisterTransient<IPublishedProgramReportDataProvider, PublishedProgramReportDataProvider>("PublishedProgramReportDataProvider");

            this.Container.RegisterGkhConfig<OverhaulHmaoConfig>();

            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<SubsidyList>().Named("SubsidyList").LifestyleTransient());

            this.Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient());
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());

            this.Container.RegisterTransient<IPriorityParams, WearoutPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, YearExploitationPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, DensityLivingPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, RoPercentDebtPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, LastOverhaulYearPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, BuildYearPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, LastOverhaulYearCeoPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, LivingAreaPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, NeedOverhaulCeoPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, PhysicalWearCeoPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, YearCommissioningPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, CollectionByCrPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, CollectionByHcsPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, PaySizeCrPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, StructElementComplexity>();
            this.Container.RegisterTransient<IPriorityParams, StructElementUsageParams>();
            this.Container.RegisterTransient<IPriorityParams, PhysicalWearout>();
            this.Container.RegisterTransient<IPriorityParams, ExceedStructuralElementLifeTimePriorityParam>();

            this.Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<CapitalGroupPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<ComplexGroupPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<CeoPointPriorityParam>().LifestyleTransient());

            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<DecisionSetMinAmountPriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<EnergyPassportPriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<NeedOverhaulStructElemPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<ProjDocsPriorityParam>().LifestyleTransient());
            this.Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<WorkDocsPriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<AvailabilityCouncilHousePriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<UrbanSettlCtrlMethodRoPriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<RuralSettlCtrlMethodRoPriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<NeededRepairOnPrivatizationDatePriorityParam>().LifestyleTransient());
            this.Container.Register(
                Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<LiftWearoutPriorityParam>().LifestyleTransient());
            this.Container.RegisterTransient<IRealityObjectsInPrograms, RealityObjectsInPrograms>();

            this.RegisterBundlers();

            this.RegisterCommonParams();

            this.RegisterControllers();

            this.RegisterDomainServices();

            this.RegisterExecutionActions();

            this.RegisterExports();

            this.RegisterInterceptors();

            this.RegisterImports();

            this.RegisterNavigations();

            this.RegisterProgramPriorityParams();

            this.RegisterReports();

            this.RegisterServices();

            this.RegisterValueResolvers();

            this.RegisterViewModels();

            this.RegisterDataProviders();

            this.RegisterStateChangeRules();

            this.AddEntityGroupComponent();

            // Регистрация классов для получения информации о зависимостях
            this.Container.Register(
                Component.For<IModuleDependencies>()
                    .Named(string.Format("{0} dependencies", this.AssemblyName))
                    .LifeStyle.Singleton
                    .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            EnumRegistry.Add(
                new B4.Modules.Analytics.Reports.Params.Enum
                {
                    Id = "DpkrDataSource",
                    Display = "Источник сборки данных по ДПКР",
                    EnumJsClass = "B4.enums.DpkrDataSource"
                });

            this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();

            this.RegisterFormatDataExport();
        }

        private void RegisterDataProviders()
        {
            this.Container.RegisterTransient<IDataProvider, DpkrDataProvider>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportReport, RealityObjectInProgramStage3Report>("RealObjStructElemlnProgStg3DataExport");
            this.Container.RegisterTransient<IDataExportService, DpkrCorrectionStage2Export>("DpkrCorrectionStage2Export");
            this.Container.RegisterTransient<IDataExportService, ShortProgramRecordExport>("ShortProgramRecordExport");
            this.Container.RegisterTransient<IDataExportService, PublishedProgramRecordExport>("PublishedProgramRecordExport");
            this.Container.RegisterTransient<IDataExportService, DecisionNoticeExport>("DecisionNoticeExport");
            this.Container.RegisterTransient<IDataExportService, VersionRecordsExport>("VersionRecordsExport");
            this.Container.RegisterTransient<IDataExportService, HmaoWorkPriceDataExport>("HmaoWorkPriceDataExport");
            this.Container.RegisterTransient<IDataExportService, VersionActualizeLogRecordExport>("VersionActualizeLogRecordExport");

            //Container.ReplaceComponent<IDataExportService>(typeof(WorkPriceDataExport),
            //   Component.For<IDataExportService>().ImplementedBy<HmaoWorkPriceDataExport>());
        }

        private void RegisterControllers()
        {
            this.Container.ReplaceController<HmaoWorkPriceController>("workprice");
            this.Container.RegisterController<MultiPriorityParamController>();
            this.Container.RegisterAltDataController<QualityPriorityParam>();
            this.Container.RegisterAltDataController<QuantPriorityParam>();
            this.Container.RegisterController<PriorityParamController>();
            this.Container.RegisterController<LongTermObjectLoanController>();
            this.Container.RegisterController<VersionActualizeLogRecordController>();

            this.Container.RegisterController<DpkrDocumentProgramVersionController>();
            this.Container.RegisterController<DpkrDocumentRealityObjectController>();
            this.Container.RegisterController<RealityObjectStructuralElementInProgrammController>();
            this.Container.RegisterController<RealityObjectStructuralElementInProgrammStage3Controller>();
            this.Container.RegisterController<SubsidyRecordController>();
            this.Container.RegisterController<FileStorageDataController<PropertyOwnerProtocols>>();
            this.Container.RegisterController<PropertyOwnerDecisionWorkController>();
            this.Container.RegisterController<FileStorageDataController<SpecialAccountDecision>>();
            this.Container.RegisterAltDataController<BasePropertyOwnerDecision>();
            this.Container.RegisterAltDataController<RealityObjectStructuralElementInProgrammStage2>();
            this.Container.RegisterAltDataController<RegOpAccountDecision>();

            this.Container.RegisterController<AccountOperationController>();

            this.Container.RegisterAltDataController<CurrentPrioirityParams>();
            this.Container.RegisterAltDataController<DpkrGroupedYear>();
            this.Container.RegisterController<DpkrCorrectionStage2Controller>();
            this.Container.RegisterController<LongTermPrObjectController>();
            this.Container.RegisterAltDataController<RealEstateTypeCommonParam>();
            this.Container.RegisterAltDataController<RealEstateTypeStructElement>();
            this.Container.RegisterController<ProgramVersionController>();
            this.Container.RegisterAltDataController<VersionRecord>();
            this.Container.RegisterAltDataController<VersionParam>();
            this.Container.RegisterAltDataController<RealEstateTypePriorityParam>();
            this.Container.RegisterController<RealEstateTypeRateController>();
            this.Container.RegisterController<RealEstateTypeMunicipalityController>();

            this.Container.RegisterAltDataController<ShareFinancingCeo>();

            this.Container.RegisterController<CommonParamsController>();

            //Краткосрочная программа
            this.Container.RegisterController<ShortProgramRecordController>();
            this.Container.RegisterController<ShortProgramDeficitController>();

            // Сервисы для портала
            this.Container.RegisterController<DpkrServiceController>();

            // Ручная загрузка ДПКР
            this.Container.RegisterAltDataController<LoadProgram>();

            this.Container.RegisterController<LongProgramController>();

            this.Container.RegisterController<BillingController>();

            this.Container.RegisterController<PublishedProgramRecordController>();

            this.Container.RegisterController<OverhaulMenuController>();
            this.Container.RegisterController<PublishedProgramController>();

            this.Container.RegisterAltDataController<MinAmountDecision>();
            this.Container.RegisterController<SpecialAccountDecisionNoticeController>();

            this.Container.RegisterController<OverhaulHmaoScriptsController>();

            this.Container.RegisterController<RealityObjMenuController>();
            this.Container.RegisterController<DefaultPlanCollectionInfoController>();
            this.Container.RegisterAltDataController<SubsidyRecordVersion>();
            this.Container.RegisterController<PriorityParamAdditionController>();

            this.Container.ReplaceController<RealEstateTypeController>("realestatetype");
            this.Container.RegisterAltDataController<VersionRecordStage1>();
            this.Container.RegisterAltDataController<VersionActualizeLog>();

            this.Container.RegisterAltDataController<ChangeYearOwnerDecision>();
            this.Container.RegisterAltDataController<DpkrDocument>();

            this.Container.RegisterAltDataController<DPKRActualCriterias>();
            this.Container.RegisterController<CostLimitController>();
            this.Container.RegisterAltDataController<CostLimit>();
            this.Container.RegisterAltDataController<CostLimitTypeWorkCr>();
            this.Container.RegisterAltDataController<CostLimitOOI>();
            this.Container.RegisterAltDataController<OwnerProtocolType>();
            this.Container.RegisterAltDataController<OwnerProtocolTypeDecision>();
            this.Container.RegisterController<OwnerProtocolTypeDecisionsController>();
            this.Container.RegisterAltDataController<CriteriaForActualizeVersion>();
            this.Container.RegisterAltDataController<CrPeriod>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<HmaoWorkPrice, HmaoWorkPriceViewModel>();

            this.Container.RegisterViewModel<CurrentPrioirityParams, CurrentPrioirityParamsViewModel>();

            this.Container.RegisterViewModel<RealityObjectStructuralElementInProgramm, RealityObjectStructuralElementInProgrammViewModel>();
            this.Container.RegisterViewModel<RealityObjectStructuralElementInProgrammStage2, RealityObjectStructuralElementInProgrammStage2ViewModel>();
            this.Container.RegisterViewModel<RealityObjectStructuralElementInProgrammStage3, RealityObjectStructuralElementInProgrammStage3ViewModel>();
            this.Container.RegisterViewModel<SubsidyRecordVersion, SubsidyRecordVersionViewModel>();
            
            this.Container.ReplaceComponent(
                typeof(IViewModel<RealityObjectStructuralElement>),
                typeof(Overhaul.ViewModel.RealityObjectStructuralElementViewModel),
                Component.For<IViewModel<RealityObjectStructuralElement>>().ImplementedBy<RealityObjectStructuralElementViewModel>().LifestyleTransient());

            this.Container.RegisterViewModel<DpkrGroupedYear, DpkrGroupedYearModel>();
            this.Container.RegisterViewModel<DpkrCorrectionStage2, DpkrCorrectionViewModel>();

            this.Container.RegisterViewModel<ShortProgramRecord, ShortProgramRecordViewModel>();

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
            this.Container.RegisterViewModel<RealEstateTypeMunicipality, RealEstateTypeMunicipalityViewModel>();

            this.Container.RegisterViewModel<VersionRecord, VersionRecordViewModel>();
            this.Container.RegisterViewModel<VersionParam, VersionParamViewModel>();
            this.Container.RegisterViewModel<ProgramVersion, ProgramVersionViewModel>();

            this.Container.RegisterViewModel<ShareFinancingCeo, ShareFinancingCeoViewModel>();

            this.Container.RegisterViewModel<ShortProgramDifitsit, ShortProgramDeficitViewModel>();

            this.Container.RegisterViewModel<PublishedProgramRecord, PublishedProgramRecordViewModel>();

            this.Container.RegisterViewModel<QualityPriorityParam, QualityPriorityParamViewModel>();
            this.Container.RegisterViewModel<QuantPriorityParam, QuantPriorityParamViewModel>();
            this.Container.RegisterViewModel<MultiPriorityParam, MultiPriorityParamViewModel>();
            this.Container.RegisterViewModel<SpecialAccountDecisionNotice, SpecialAccountDecisionNoticeViewModel>();
            this.Container.RegisterViewModel<MinAmountDecision, MinAmountDecisionViewModel>();

            this.Container.RegisterViewModel<LongTermObjectLoan, LongTermObjectLoanViewModel>();

            this.Container.RegisterViewModel<DefaultPlanCollectionInfo, DefaultPlanCollectionInfoViewModel>();
            this.Container.RegisterViewModel<VersionRecordStage1, VersionRecordSt1ViewModel>();
            this.Container.RegisterViewModel<VersionActualizeLog, VersionActualizeLogViewModel>();
            this.Container.RegisterViewModel<VersionActualizeLogRecord, VersionActualizeLogRecordViewModel>();
            this.Container.RegisterViewModel<ChangeYearOwnerDecision, ChangeYearOwnerDecisionViewModel>();
            this.Container.RegisterViewModel<DpkrDocument, DpkrDocumentViewModel>();
            this.Container.RegisterViewModel<FormatDataExportEntity, FormatDataExportEntityViewModel>();
            this.Container.RegisterViewModel<DpkrDocumentProgramVersion, DpkrDocumentProgramVersionViewModel>();
            this.Container.RegisterViewModel<DpkrDocumentRealityObject, DpkrDocumentRealityObjectViewModel>();

            Container.RegisterViewModel<DPKRActualCriterias, DPKRActualCriteriasViewModel>();
            Container.RegisterViewModel<CostLimit, CostLimitViewModel>();
            Container.RegisterViewModel<CostLimitOOI, CostLimitOOIViewModel>();
            Container.RegisterViewModel<CostLimitTypeWorkCr, CostLimitTypeWorkCrViewModel>();
            this.Container.RegisterViewModel<OwnerProtocolType, OwnerProtocolTypeViewModel>();
            this.Container.RegisterViewModel<OwnerProtocolTypeDecision, OwnerProtocolTypeDecisionViewModel>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IDpkrDocumentProgramVersionService, DpkrDocumentProgramVersionService>();
            this.Container.RegisterTransient<IDpkrDocumentRealityObjectService, DpkrDocumentRealityObjectService>();
            this.Container.RegisterTransient<IObjectCrIntegrationService, ObjectCrIntegrationService>();

            this.Container.RegisterTransient<IObjectCrDpkrDataService, ObjectCrDpkrDataService>();
            this.Container.RegisterTransient<IWorkPriceService<HmaoWorkPrice>, HmaoWorkPriceService>();

            this.Container.RegisterTransient<ILongProgramService, LongProgramService>();
            this.Container.RegisterTransient<ISubsidyRecordService, SubsidyRecordService>();
            this.Container.RegisterTransient<IPropertyOwnerDecisionWorkService, PropertyOwnerDecisionWorkService>();
            this.Container.RegisterTransient<IRealEstateTypeRateService, RealEstateTypeRateService>();

            this.Container.RegisterTransient<IAccountOperationService, AccountOperationService>();

            //Container.RegisterTransient<IDpkrParamsService, DbDpkrParamsService>();
            this.Container.RegisterTransient<IDpkrParamsService, ConfigDpkrParamsService>();

            this.Container.RegisterTransient<IShortProgramDeficitService, ShortProgramDeficitService>();

            // домен сервис для портальных экшенов
            this.Container.RegisterTransient<IDpkrService, DpkrService>();

            this.Container.RegisterTransient<IDpkrCorrectionService, DpkrCorrectionService>();

            this.Container.RegisterTransient<IPublishProgramService, PublishedProgramService>();

            this.Container.RegisterTransient<IPriorityParamService, PriorityParamService>();
            this.Container.RegisterTransient<ILongTermPrObjectService, LongTermPrObjectService>();
            this.Container.RegisterTransient<ILongTermObjectLoanService, LongTermObjectLoanService>();
            this.Container.RegisterTransient<IDecisionNoticeService, DecisionNoticeService>();

            this.Container.RegisterTransient<IOverhaulHmaoScriptsService, OverhaulHmaoScriptsService>();
            this.Container.RegisterTransient<Overhaul.DomainService.IDpkrService, HmaoDpkrService>();
            this.Container.RegisterTransient<IProgramVersionService, ProgramVersionService>();

            this.Container.RegisterTransient<IStage3Service, Stage3Service>();
            this.Container.RegisterTransient<IRealityObjectStructElementService, RealityObjectStructElementService>();

            this.Container.RegisterTransient<IPriorityService, PriorityService>();

            this.Container.RegisterTransient<IVersionDateCalcService, VersionDateCalcService>();
            this.Container.RegisterTransient<IRealityObjectDpkrDataService, HmaoRealityObjectDpkrDataService>();

            this.Container.RegisterTransient<IPublishProgramWcfService, HmaoPublishProgramWcfService>();
            this.Container.RegisterTransient<IDefaultPlanCollectionInfoService, DefaultPlanCollectionInfoService>();
            this.Container.RegisterTransient<IRealEstateTypeMunicipalityService, RealEstateTypeMunicipalityService>();
            this.Container.RegisterTransient<IRepairKonstWcfService, HmaoRepairKonstWcfService>();
            this.Container.RegisterTransient<IActualizeVersionService, ActualizeVersionService>();
            this.Container.RegisterTransient<IMaxCostExceededService, HmaoMaxCostExceededService>();
            this.Container.RegisterTransient<IRealityObjectsProgramVersion, RealityObjectsProgramVersion>();
            this.Container.RegisterTransient<IProgramCrImportService, ProgramCrImportService>();

            Component.For<IRealEstateTypeService>()
                .Forward<IHmaoRealEstateTypeService>()
                .ImplementedBy<HmaoRealEstateTypeService>()
                .LifestyleTransient()
                .RegisterIn(this.Container);

            this.Container.RegisterTransient<IDpkrRealityObjectService, HmaoDpkrRealityObjectService>();
            this.Container.RegisterTransient<IPriorityParamAdditionService, PriorityParamAdditionService>();
            this.Container.RegisterTransient<IDpkrTypeWorkService, DpkrTypeWorkService>();
            this.Container.RegisterTransient<IRealObjOverhaulDataService, HmaoRealObjOverhaulDataService>();
            this.Container.RegisterTransient<ITypeWorkStage1Service, TypeWorkStage1Service>();
            this.Container.RegisterTransient<IOverhaulViewModels, OverhaulViewModels>();
            this.Container.RegisterTransient<IActualizeVersionLogService<ActualizeVersionLogRecord, ActualizeVersionLogReport>, ActualizeVersionLogService<ActualizeVersionLogRecord, ActualizeVersionLogReport>>();
            this.Container.RegisterTransient<IActualizeVersionLogService<ImportPublishYearLogRecord, ImportPublishYearLogReport>, ActualizeVersionLogService<ImportPublishYearLogRecord, ImportPublishYearLogReport>>();
            this.Container.RegisterTransient<IChangeVersionSt1Service, ChangeVersionSt1Service>();
            this.Container.RegisterTransient<IHouseTypesConfigService, HouseTypesConfigServices>();
            this.Container.RegisterTransient<BaseRealObjOverhaulDataObject, HmaoRealObjOverhaulDataObject>("RealObjOverhaulDataObject");

            this.Container.ReplaceComponent(Component.For<IDefectService>().ImplementedBy<DefectService>().LifestyleTransient());

            this.Container.RegisterTransient<IOwnerProtocolTypeDecisionsService, OwnerProtocolTypeDecisionsService>();
        }

        private void RegisterDomainServices()
        {
            this.Container.ReplaceComponent<IDomainService<WorkPrice>>(
                typeof(WorkPriceDomainService),
                Component.For<IDomainService<WorkPrice>>().ImplementedBy<DomainService.WorkPriceDomainService>());

            this.Container.RegisterTransient<IDomainService<ProgramVersion>, ProgramVersionDomainService>();
            this.Container.RegisterTransient<IDomainService<PropertyOwnerProtocols>, FileStorageDomainService<PropertyOwnerProtocols>>();
            this.Container.RegisterTransient<IProgramCrRealityObjectService, ProgramCrRealityObjectService>();

            this.Container.RegisterDomainService<SpecialAccountDecision, FileStorageDomainService<SpecialAccountDecision>>();
            this.Container.RegisterDomainService<VersionActualizeLog, GkhFileStorageDomainService<VersionActualizeLog>>();
            this.Container.RegisterDomainService<SpecialAccountDecisionNotice, FileStorageDomainService<SpecialAccountDecisionNotice>>();
            this.Container.RegisterDomainService<LongTermObjectLoan, FileStorageDomainService<LongTermObjectLoan>>();

            this.Container.ReplaceComponent(Component.For<IListCeoService>().ImplementedBy<HmaoListCeoService>().LifestyleTransient());

            this.Container.ReplaceComponent<ICommonEstateObjectService>(
                typeof(CommonEstateObjectService),
                Component.For<ICommonEstateObjectService>().ImplementedBy<DomainService.Impl.CommonEstateObjectService>());

            this.Container.RegisterFileStorageDomainService<ChangeYearOwnerDecision>();
            this.Container.RegisterFileStorageDomainService<DpkrDocument>();
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
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterTransient<IDomainServiceInterceptor<HmaoWorkPrice>, HmaoWorkPriceInterceptor>();
            this.Container.ReplaceComponent<IDomainServiceInterceptor<RealEstateType>, RealEstateTypeInterceptor>(
                Component.For<IDomainServiceInterceptor<RealEstateType>>()
                    .ImplementedBy<Interceptors.RealEstateTypeInterceptor>()
                    .LifestyleTransient());
            this.Container.RegisterTransient<IDomainServiceInterceptor<ProgramVersion>, ProgramVersionInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RealityObject>, RealityObjectInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<SubsidyRecord>, SubsidyRecordInterceptor>();

            this.Container.RegisterDomainInterceptor<LongTermPrObject, LongTermPrObjectInterceptor>();

            this.Container.RegisterDomainInterceptor<SpecialAccountDecision, SpecialAccountDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<SpecialAccountDecisionNotice, SpecialAccountDecisionNoticeInterceptor>();
            this.Container.RegisterDomainInterceptor<MinAmountDecision, MinAmountDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<ShareFinancingCeo, ShareFinancingCeoInterceptor>();

            this.Container.RegisterDomainInterceptor<DefectList, DefectListInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeWorkCr, TypeWorkCrInterceptor>();
            this.Container.RegisterDomainInterceptor<DpkrDocument, DpkrDocumentInterceptor>();

            this.Container.ReplaceComponent<IDomainServiceInterceptor<TypeWorkCrRemoval>>(
                typeof(TypeWorkCrRemovalInterceptor),
                Component.For<IDomainServiceInterceptor<TypeWorkCrRemoval>>()
                    .ImplementedBy<Interceptors.TypeWorkCrRemovalInterceptor>()
                    .LifeStyle.Transient);
            this.Container.RegisterDomainInterceptor<FormatDataExportEntity, FormatDataExportEntityInterceptor>();

            this.Container.RegisterDomainInterceptor<DPKRActualCriterias, DPKRActualCriteriasInterceptor>();
            this.Container.RegisterDomainInterceptor<CostLimit, CostLimitInterceptor>();
            this.Container.RegisterDomainInterceptor<CostLimitOOI, CostLimitOOIInterceptor>();
            this.Container.RegisterDomainInterceptor<CostLimitTypeWorkCr, CostLimitTypeWorkCrInterceptor>();
        }

        private void RegisterImports()
        {
            this.Container.RegisterTransient<IStructElementImport, StructElementImport>();

            this.Container.RegisterImport<RealtyObjectImport>();
            this.Container.RegisterImport<DpkrLoadImport>();
            this.Container.RegisterImport<FundImportPart3>();
            this.Container.RegisterImport<FundImportPart5>();
            this.Container.RegisterImport<StructElemWorksImport>();
            this.Container.RegisterImport<DpkrImport>();
            this.Container.RegisterImport<Dpkr1cImport>();
            this.Container.RegisterImport<PublishYearsImport>();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<StructElementCopyAction>();
            this.Container.RegisterExecutionAction<CopyAreaToStructElementAction>();
            this.Container.RegisterExecutionAction<ConstructiveElementCopyAction>();
            this.Container.RegisterExecutionAction<CrEqualYearV3WithV1Action>();
            this.Container.RegisterExecutionAction<SubsidyCopyToVersionAction>();

            //Container.RegisterExecutionAction<VersionPriorityFixAction>();
            this.Container.RegisterExecutionAction<ProgramCrChangesCreationAction>();
            this.Container.RegisterExecutionAction<TypeWorkCrSetYearRepairAction>();
            this.Container.RegisterExecutionAction<OverhaulHmaoConfigMigrationAction>();
            this.Container.RegisterExecutionAction<VersionRecordActualizeFieldUpdateAction>();
            this.Container.RegisterExecutionAction<FillPublishedProgramRecordRoReference>();
            this.Container.RegisterExecutionAction<CorrectionPeriodAction>();
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
            this.Container.RegisterTransient<IFormulaParameter, CalculateByVolumeResolver>(CalculateByVolumeResolver.Id);
        }

        private void RegisterCommonParams()
        {
            this.Container.RegisterTransient<ICommonParam, NumberEntrancesCommonParam>(NumberEntrancesCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, PhysicalWearCommonParam>(PhysicalWearCommonParam.Code);
        }

        private void RegisterProgramPriorityParams()
        {
            this.Container.RegisterTransient<IProgrammPriorityParam, StructuralElementWearoutParam>(StructuralElementWearoutParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, RoBuildYearParam>(RoBuildYearParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, WorkVolumeParam>(WorkVolumeParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, NeedOverhaulParam>(NeedOverhaulParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, LastOverhaulYearParam>(LastOverhaulYearParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, CountWorksParam>(CountWorksParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, RoWearoutParam>(RoWearoutParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, DateInventoryParam>(DateInventoryParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, RoLifetimeParam>(RoLifetimeParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, PointsParam>(PointsParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, PrivatizationWithWeightParam>(PrivatizationWithWeightParam.Code);
            this.Container.RegisterTransient<IProgrammPriorityParam, DensityLivingParam>(DensityLivingParam.Code);
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IPrintForm, RegisterMkdToBeRepairedOverhaul>("RegisterMkdToBeRepairedOverhaul");
            this.Container.RegisterTransient<IPrintForm, ListByManyApartmentsHousesOverhaul>("ListByManyApartmentsHousesOverhaul");
            this.Container.RegisterTransient<IPrintForm, ControlCertificationOfBuild>("ControlCertificationOfBuild");
            this.Container.RegisterTransient<IPrintForm, PublishedDpkrReport>("PublishedDpkrReport");
            this.Container.RegisterTransient<IPrintForm, PublishedDpkrExtendedReport>("PublishedDpkrExtendedReport");
            this.Container.RegisterTransient<IPrintForm, SpecialAccountDecisionReport>("SpecialAccountDecisionReport");
            this.Container.RegisterTransient<IPrintForm, FormFundNotSetMkdInfoReport>("FormFundNotSetMkdInfoReport");
            this.Container.RegisterTransient<IPrintForm, CtrlCertOfBuildConsiderMissingCeo>("CtrlCertOfBuildConsiderMissingCeo");
            this.Container.RegisterTransient<IPrintForm, LongtermProgOverhaulInfo>("LongtermProgOverhaulInfo");
            this.Container.RegisterTransient<IPrintForm, MarginalCostKr1LivingSpace>("MarginalCostKr1LivingSpace");
            this.Container.RegisterTransient<IPrintForm, CertificationControlValues>("CertificationControlValues");
            this.Container.RegisterTransient<IPrintForm, CertificationControlValuesWithQuality>("CertificationControlValuesWithQuality");
            this.Container.RegisterTransient<IPrintForm, DpkrGroupedByPeriod>("DpkrGroupedByPeriod");
            this.Container.RegisterTransient<IPrintForm, DpkrGroupedByPeriodPublish>("DpkrGroupedByPeriodPublish");
            this.Container.RegisterTransient<IPrintForm, CountCeoByMuInPeriod>("CountCeoByMuInPeriod");
            this.Container.RegisterTransient<IPrintForm, CountRoByMuInPeriod>("CountRoByMuInPeriod");
            this.Container.RegisterTransient<IPrintForm, CountRoByCeoInPublProgram>("CountRoByCeoInPublProgram");
            this.Container.RegisterTransient<IPrintForm, PublishedDpkrByWorkReport>("PublishedDpkrByWorkReport");
            this.Container.RegisterTransient<IPrintForm, PublishedDpkrByWorkAndAddressReport>("PublishedDpkrByWorkAndAddressReportReport");
            this.Container.RegisterTransient<IPrintForm, ProgramVersionReport>("ProgramVersionReport");
            this.Container.RegisterTransient<IPrintForm, HousesExcessMargSumReport>("HousesExcessMargSumReport");
            this.Container.RegisterTransient<IPrintForm, HousesHaveNotCollectSumReport>("HousesHaveNotCollectSumReport");
            this.Container.RegisterTransient<IPrintForm, LongProgInfoByStructEl>("LongProgInfoByStructEl");
            this.Container.RegisterTransient<IPrintForm, CtrlFillDataForFormLongProg>("CtrlFillDataForFormLongProg");
            this.Container.RegisterTransient<IPrintForm, PublishedDpkrPeriodByWorkReport>("PublishedDpkrPeriodByWorkReport");
            this.Container.RegisterTransient<IPrintForm, FinanceModelDpkrReport>("FinanceModelDpkrReport");
            this.Container.RegisterTransient<IPrintForm, ImportedDpkrReport>("ImportedDpkrReport");
            this.Container.RegisterTransient<IPrintForm, CrShortTermPlanReport>("CrShortTermPlanReport");
            this.Container.RegisterTransient<IPrintForm, SubsidyInfoReport>("SubsidyInfoReport");
            this.Container.RegisterTransient<IPrintForm, ReasonableRateReport>("ReasonableRateReport");
            this.Container.RegisterTransient<IPrintForm, NotIncludedWorksInProgram>("NotIncludedWorksInProgram");
            this.Container.RegisterTransient<IPrintForm, PlanOwnerCollectionReport>("PlanOwnerCollectionReport");
            this.Container.RegisterTransient<IPrintForm, OverhaulMkdIndicatorsReport>("OverhaulMkdIndicatorsReport");
            this.Container.RegisterTransient<IPrintForm, DpkrDataAnalysisReport>("DpkrDataAnalysisReport");
            this.Container.RegisterTransient<IPrintForm, DpkrStructuralElements>("DpkrStructuralElements");

            this.Container.RegisterTransient<IPrintForm, SubsidyBudget>("SubsidyBudget");
            this.Container.RegisterTransient<IPrintForm, SubsidyBudgetSourcesFinancing>("SubsidyBudgetSourcesFinancing");
            this.Container.RegisterTransient<IPrintForm, SummaryCharacteristicsOfHouse>("SummaryCharacteristicsOfHouse");
            this.Container.RegisterTransient<IPrintForm, LongProgramReport>("LongProgramReport");
            this.Container.RegisterTransient<IPrintForm, ImportPublishYearLogReport>("ImportPublishYearLogReport");
            this.Container.RegisterTransient<IGkhBaseReport, StructElementListHmao>("StructElementListHmao");
            this.Container.RegisterTransient<IPrintForm, LongProgramByTypeWork>("LongProgramByTypeWork");
            this.Container.RegisterTransient<IPrintForm, HouseInformationCeReport>("HouseInformationCeReport");

            this.Container.RegisterTransient<IPrintForm, RealtiesOutOfDpkr>("RealtiesOutOfDpkr");
            this.Container.RegisterTransient<IPrintForm, PublishProgramByStructEl>("PublishProgramByStructEl");

            this.Container.RegisterTransient<IPrintForm, ActualizeVersionLogReport>("ActualizeVersionLogReport");

            this.Container.ReplaceComponent<IPrintForm>(
                typeof(RoomAreaControlReport),
                Component
                    .For<IPrintForm>()
                    .ImplementedBy<Reports.RoomAreaControlReport>()
                    .LifestyleTransient()
                    .Named("Report Bars.Gkh.Overhaul.Hmao RoomAreaControl"));

            this.Container.ReplaceComponent<IPrintForm>(
                typeof(Bars.Gkh.Overhaul.Reports.RealtyObjectDataReport),
                Component.For<IPrintForm>().ImplementedBy<Reports.RealtyObjectDataReport>().LifeStyle.Transient);
            
            this.Container.RegisterTransient<IDataExportReport, CrBudgetingReport>("CrBudgetingReport");
            this.Container.RegisterTransient<IDataExportReport, HousesWithMissingParamsReport>("HousesWithMissingParamsReport");
            this.Container.RegisterTransient<IDataExportReport, WorksNotIncludedPublishProgramReport>("WorksNotIncludedPublishProgramReport");
            this.Container.RegisterTransient<IDataExportReport, NotIncludedInCrHousesReport>("NotIncludedInCrHousesReport");
            this.Container.RegisterTransient<IDataExportReport, CostOfWorksInStructuralElementContextReport>("CostOfWorksInStructuralElementContextReport");
            this.Container.RegisterTransient<IDataExportReport, IncludedInCrHousesByYearsReport>("IncludedInCrHousesByYearsReport");
            this.Container.RegisterTransient<IDataExportReport, CrCeoWorkReport>("CrCeoWorkReport");
            this.Container.RegisterTransient<IDataExportReport, HousesWithNotFilledFiasReport>("HousesWithNotFilledFiasReport");
        }

        private void RegisterStateChangeRules()
        {
            this.Container.RegisterTransient<IRuleChangeStatus, PublishProgramDeleteDateTransferRule>();
            this.Container.Register(
                Component.For<IRuleChangeStatus, IGkhRuleChangeStatus>().ImplementedBy<PublishProgramAddDateTransferRule>().LifestyleTransient());

            this.Container.Register(Component.For<IStateChangeHandler>().ImplementedBy<DefectListStateChangeHandler>().LifeStyle.Transient);
            this.Container.RegisterTransient<IRuleChangeStatus, PerfWorkActStateChangeRule>();
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.RegisterProxySelectorService<PkrProxy, PkrSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PkrDomProxy, PkrDomSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PkrDomWorkProxy, PkrDomWorkSelectorService>();
            ContainerHelper.RegisterProxySelectorService<AddContragentProxy, AddContragentSelectorService>();
            ContainerHelper.RegisterProxySelectorService<PkrDocProxy, PkrDocSelectorService>();
            ContainerHelper.ReplaceProxySelectorService<WorkKprTypeProxy,
                Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl.WorkKprTypeSelectorService,
                WorkKprTypeSelectorService>(this.Container);

            ContainerHelper.ReplaceFormatDataExportRepository<ObjectCr, 
                GkhCr.FormatDataExport.Domain.Impl.FormatDataExportObjectCrRepository, 
                FormatDataExportObjectCrRepository>();
        }

        private void AddEntityGroupComponent()
        {
            this.AddEntityGroupComponent("KprEntityGroup",
                "Программы капитального ремонта",
                new List<string>
                {
                    "PKRDOCFILES",
                    "ADDCONTRAGENT"
                },
                FormatDataExportType.Pkr);
        }

        private void AddEntityGroupComponent(string code, string description, IList<string> inheritedEtities, FormatDataExportType exportType)
        {
            this.Container.Register(Component.For<IExportableEntityGroup>()
                .Instance(new ExportableEntityGroup(code, description, inheritedEtities, exportType))
                .LifestyleSingleton());
        }
    }
}