namespace Bars.Gkh.Overhaul.Nso
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Import.RoImport;
    using Bars.Gkh.Interceptors;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Domain.Impl;
    using Bars.Gkh.Overhaul.Domain.RealityObjectServices;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Controllers;
    using Bars.Gkh.Overhaul.Nso.Domain.Impl;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.DomainService.Impl;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.ExecutionAction;
    using Bars.Gkh.Overhaul.Nso.Export;
    using Bars.Gkh.Overhaul.Nso.Import;
    using Bars.Gkh.Overhaul.Nso.Import.Program;
    using Bars.Gkh.Overhaul.Nso.Import.ReformGkh;
    using Bars.Gkh.Overhaul.Nso.Interceptors;
    using Bars.Gkh.Overhaul.Nso.Navigation;
    using Bars.Gkh.Overhaul.Nso.Permissions;
    using Bars.Gkh.Overhaul.Nso.PriorityParams;
    using Bars.Gkh.Overhaul.Nso.PriorityParams.Impl;
    using Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams;
    using Bars.Gkh.Overhaul.Nso.Reports;
    using Bars.Gkh.Overhaul.Nso.Services.Impl;
    using Bars.Gkh.Overhaul.Nso.StateChange;
    using Bars.Gkh.Overhaul.Nso.ValueResolver;
    using Bars.Gkh.Overhaul.Nso.ViewModel;
    using Bars.Gkh.Report;
    using Bars.GkhCr.DomainService;
    using Castle.MicroKernel.Registration;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;
    using GkhCr.Entities;
    using CopyAreaToStructElementAction = ExecutionAction.CopyAreaToStructElementAction;
    using IDpkrService = DomainService.IDpkrService;
    using RealityObjectInterceptor = Bars.Gkh.Interceptors.RealityObjectInterceptor;
    using StructElementCopyAction = ExecutionAction.StructElementCopyAction;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Gkh.Overhaul.Nso statefulentity");
            Container.RegisterTransient<IPublishedProgramReportDataProvider, PublishedProgramReportDataProvider>("PublishedProgramReportDataProvider");

            Container.RegisterGkhConfig<OverhaulNsoConfig>();

            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<SubsidyList>().Named("SubsidyList").LifestyleTransient());

            Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient());
            Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());

            Component.For<IFormulaParameter>().ImplementedBy<RoBuildYearResolver>().Named(RoBuildYearResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<RoFirstPrivatizationYearResolver>().Named(RoFirstPrivatizationYearResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<RoWearoutResolver>().Named(RoWearoutResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<ElementWearoutResolver>().Named(ElementWearoutResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<ElementLastRepairYearResolver>().Named(ElementLastRepairYearResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<ElementVolumeResolver>().Named(ElementVolumeResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<ElementLifeTimeResolver>().Named(ElementLifeTimeResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<CurrentYearResolver>().Named(CurrentYearResolver.Id).LifeStyle.Transient.RegisterIn(Container);
            Component.For<IFormulaParameter>().ImplementedBy<DpkrEndYearResolver>().Named(DpkrEndYearResolver.Id).LifeStyle.Transient.RegisterIn(Container);

            Container.RegisterTransient<IProgrammPriorityParam, StructuralElementWearoutParam>(StructuralElementWearoutParam.Code);
            Container.RegisterTransient<IProgrammPriorityParam, RoBuildYearParam>(RoBuildYearParam.Code);
            Container.RegisterTransient<IProgrammPriorityParam, WorkVolumeParam>(WorkVolumeParam.Code);
            Container.RegisterTransient<IProgrammPriorityParam, NeedOverhaulParam>(NeedOverhaulParam.Code);
            Container.RegisterTransient<IProgrammPriorityParam, LastOverhaulYearParam>(LastOverhaulYearParam.Code);
            Container.RegisterTransient<IProgrammPriorityParam, PointsParam>(PointsParam.Code);
            Container.RegisterTransient<IProgrammPriorityParam, DensityLivingParam>(DensityLivingParam.Code);
            
            #region Reports
            Container.RegisterTransient<IPrintForm, ActualizeVersionLogReport>("ActualizeVersionLogReport");
            Container.Register(Component.For<IPrintForm>().ImplementedBy<ControlCertificationOfBuild>().Named("ControlCertificationOfBuild").LifestyleTransient());
            Container.Register(Component.For<IPrintForm>().ImplementedBy<PublishedDpkrReport>().Named("PublishedDpkrReport").LifestyleTransient());
            Container.Register(Component.For<IPrintForm>().ImplementedBy<PublishedDpkrExtendedReport>().Named("PublishedDpkrExtendedReport").LifestyleTransient());
            Container.Register(Component.For<IPrintForm>().ImplementedBy<LongtermProgOverhaulInfo>().Named("LongtermProgOverhaulInfo").LifestyleTransient());
            Container.RegisterTransient<IPrintForm, SpecialAccountDecisionReport>("SpecialAccountDecisionReport");
            Container.RegisterTransient<IPrintForm, FormFundNotSetMkdInfoReport>("FormFundNotSetMkdInfoReport");
            Container.RegisterTransient<IPrintForm, CtrlCertOfBuildConsiderMissingCeo>("CtrlCertOfBuildConsiderMissingCeo");
            Container.RegisterTransient<IPrintForm, CertificationControlValues>("CertificationControlValues");
            Container.RegisterTransient<IPrintForm, HouseInformationCeReport>("HouseInformationCeReport");
            Container.RegisterTransient<IPrintForm, PublishedDpkrByWorkReport>("PublishedDpkrByWorkReport");
            Container.RegisterTransient<IPrintForm, PublishedDpkrByWorkAndAddressReport>("PublishedDpkrByWorkAndAddressReport");
            Container.RegisterTransient<IPrintForm, ProgramVersionReport>("ProgramVersionReport");
            Container.RegisterTransient<IPrintForm, HousesExcessMargSumReport>("HousesExcessMargSumReport");
            Container.RegisterTransient<IPrintForm, LongProgInfoByStructEl>("LongProgInfoByStructEl");
            Container.RegisterTransient<IPrintForm, ReasonableRateReport>("ReasonableRateReport");
            Container.RegisterTransient<IGkhBaseReport, StructElementListNso>("StructElementListNso");
            Container.RegisterTransient<IPrintForm, NotIncludedWorksInProgram>("NotIncludedWorksInProgram");
            Container.RegisterTransient<IPrintForm, PlanOwnerCollectionReport>("PlanOwnerCollectionReport");
            Container.RegisterTransient<IPrintForm, CertificationControlValuesWithQuality>("CertificationControlValuesWithQuality");
            Container.RegisterTransient<IPrintForm, RealtiesOutOfDpkr>("RealtiesOutOfDpkr");
            Container.RegisterTransient<IPrintForm, HouseInformationCeReportFias>("HouseInformationCeFiasReport");

            Container.ReplaceComponent<IPrintForm>(
                typeof(Gkh.Report.RoomAreaControlReport),
                Component
                    .For<IPrintForm>()
                    .ImplementedBy<Reports.RoomAreaControlReport>()
                    .LifestyleTransient()
                    .Named("Report Bars.Gkh.Overhaul.Nso RoomAreaControl"));

            #endregion

            #region Imports

            Container.RegisterImport<RealtyObjectImport>();
            Container.RegisterTransient<IStructElementImport, StructElementImport>();
            Container.RegisterImport<DpkrLoadImport>();
            Container.RegisterImport<FundImportPart3>();
            Container.RegisterImport<FundImportPart5>();
            Container.RegisterImport<StructElemWorksImport>();
            Container.RegisterImport<KpkrProgramImport>();

            #endregion Imports

            Container.RegisterTransient<IPriorityParams, WearoutPriorityParam>();
            Container.RegisterTransient<IPriorityParams, YearExploitationPriorityParam>();
            Container.RegisterTransient<IPriorityParams, RoWearoutPriorityParam>();
            Container.RegisterTransient<IPriorityParams, YearCommissioningPriorityParam>();
            Container.RegisterTransient<IPriorityParams, StructElementUsageParams>();
            Container.RegisterTransient<IPriorityParams, PhysicalWearout>();
            Container.RegisterTransient<IPriorityParams, StructElementComplexity>();
            Container.RegisterTransient<IPriorityParams, CollectionByHcsPriorityParam>();
            Container.RegisterTransient<IPriorityParams, CollectionByCrPriorityParam>();
            Container.RegisterTransient<IPriorityParams, PaySizeCrPriorityParam>();

            Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<CapitalGroupPriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<ComplexGroupPriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IMultiPriorityParam>().ImplementedBy<CeoPointPriorityParam>().LifestyleTransient());

            Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<EnergyPassportPriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<ProjDocsPriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<WorkDocsPriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<AvailabilityCouncilHousePriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<UrbanSettlCtrlMethodRoPriorityParam>().LifestyleTransient());
            Container.Register(Component.For<IPriorityParams, IQualitPriorityParam>().ImplementedBy<RuralSettlCtrlMethodRoPriorityParam>().LifestyleTransient());

            Container.RegisterPermissionMap<OverhaulNsoPermissionMap>();

            RegisterBundlers();

            RegisterExports();

            RegisterControllers();

            RegisterInterceptors();

            RegisterNavigations();

            RegisterViewModels();

            RegisterServices();

            RegisterDomainServices();

            RegisterExecutionActions();

            RegisterStateChangeRules();
            
            // Регистрация классов для получения информации о зависимостях
            Container.Register(Component.For<IModuleDependencies>()
                .Named(string.Format("{0} dependencies", AssemblyName))
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(Container).Init()));
        }

        private void RegisterExports()
        {
            Container.RegisterTransient<IDataExportReport, RealityObjectInProgramStage3Report>("RealObjStructElemlnProgStg3DataExport");
            Container.RegisterTransient<IDataExportService, DpkrCorrectionStage2Export>("DpkrCorrectionStage2Export");
            Container.RegisterTransient<IDataExportService, ShortProgramRecordExport>("ShortProgramRecordExport");
            Container.RegisterTransient<IDataExportService, PublishedProgramRecordExport>("PublishedProgramRecordExport");
            Container.RegisterTransient<IDataExportService, DecisionNoticeExport>("DecisionNoticeExport");
            Container.RegisterTransient<IDataExportService, NsoWorkPriceDataExport>("NsoWorkPriceDataExport");
        }

        private void RegisterControllers()
        {
            Container.ReplaceController<NsoWorkPriceController>("workprice");

            Container.RegisterAltDataController<VersionActualizeLog>();
            Container.RegisterController<RealEstateTypeMunicipalityController>();
            Container.RegisterAltDataController<ListServiceDecisionWorkPlan>();
            Container.RegisterController<MultiPriorityParamController>();
            Container.RegisterAltDataController<QualityPriorityParam>();
            Container.RegisterAltDataController<QuantPriorityParam>();
            Container.RegisterController<PriorityParamController>();
            Container.RegisterController<LongTermObjectLoanController>();
                                   
            Container.RegisterController<RealityObjectStructuralElementInProgrammController>();
            Container.RegisterController<RealityObjectStructuralElementInProgrammStage2Controller>();
            Container.RegisterController<RealityObjectStructuralElementInProgrammStage3Controller>();
            Container.RegisterController<SubsidyRecordController>();
            Container.RegisterController<SubsidyMunicipalityController>();
            Container.RegisterController<SubsidyMunicipalityRecordController>();
            Container.ReplaceController<RealEstateTypeController>("realestatetype");
            Container.RegisterController<FileStorageDataController<PropertyOwnerProtocols>>();
            Container.RegisterController<PropertyOwnerDecisionWorkController>();
            Container.RegisterAltDataController<BasePropertyOwnerDecision>();
            Container.RegisterController<FileStorageDataController<SpecialAccountDecision>>();
            Container.RegisterAltDataController<RegOpAccountDecision>();

            Container.RegisterController<AccountOperationController>();

            Container.RegisterAltDataController<CurrentPrioirityParams>();
            Container.RegisterAltDataController<DpkrGroupedYear>();
            Container.RegisterController<DpkrCorrectionStage2Controller>();
            Container.RegisterController<LongTermPrObjectController>();
            Container.RegisterAltDataController<SpecialAccount>();
            Container.RegisterAltDataController<AccrualsAccount>();
            Container.RegisterAltDataController<PaymentAccount>();
            Container.RegisterAltDataController<BaseOperation>();
            Container.RegisterAltDataController<AccrualsAccountOperation>();
            Container.RegisterAltDataController<RealEstateTypeCommonParam>();
            Container.RegisterAltDataController<RealEstateTypeStructElement>();
            Container.RegisterController<ProgramVersionController>();
            Container.RegisterController<FileStorageDataController<VersionRecord>>();
            Container.RegisterAltDataController<VersionParam>();
            Container.RegisterAltDataController<RealEstateTypePriorityParam>();
            Container.RegisterController<RealEstateTypeRateController>();

            Container.RegisterController<CommonParamsController>();

            //Краткосрочная программа
            Container.RegisterController<ShortProgramRecordController>();
            Container.RegisterController<ShortProgramDeficitController>();

            // Сервисы для портала
            Container.RegisterController<DpkrServiceController>();

            // Ручная загрузка ДПКР
            Container.RegisterAltDataController<LoadProgram>();


            Container.RegisterController<BillingController>();

            Container.RegisterController<PublishedProgramRecordController>();

            Container.RegisterController<OverhaulMenuController>();
            Container.RegisterController<RealityObjMenuController>();
            Container.RegisterController<PublishedProgramController>();

            Container.RegisterAltDataController<MinAmountDecision>();
            Container.RegisterAltDataController<ListServicesDecision>();
            Container.RegisterAltDataController<MinFundSizeDecision>();
            Container.RegisterAltDataController<PrevAccumulatedAmountDecision>();
            Container.RegisterAltDataController<CreditOrganizationDecision>();
            Container.RegisterController<SpecialAccountDecisionNoticeController>();
            Container.RegisterAltDataController<AccBankStatement>();

            Container.RegisterController<LongTermProgramController>();
            Container.RegisterController<ExecutionLongTermProgramController>();

            Container.RegisterAltDataController<ContributionCollection>();

            Container.RegisterController<OwnerAccountDecisionController>();

            Container.RegisterController<PriorityParamAdditionController>();
        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<VersionActualizeLog, VersionActualizeLogViewModel>();
            Container.RegisterViewModel<NsoWorkPrice, NsoWorkPriceViewModel>();
            Container.RegisterViewModel<RealEstateTypeMunicipality, RealEstateTypeMunicipalityViewModel>();

            Container.RegisterViewModel<ListServiceDecisionWorkPlan, ListServiceDecisionWorkPlanViewModel>();

            Container.RegisterViewModel<CurrentPrioirityParams, CurrentPrioirityParamsViewModel>();

            Container.RegisterViewModel<RealityObjectStructuralElementInProgramm, RealityObjectStructuralElementInProgrammViewModel>();
            Container.RegisterViewModel<RealityObjectStructuralElementInProgrammStage2, RealityObjectStructuralElementInProgrammStage2ViewModel>();
            Container.RegisterViewModel<RealityObjectStructuralElementInProgrammStage3, RealityObjectStructuralElementInProgrammStage3ViewModel>();
            Container.RegisterViewModel<SubsidyMunicipalityRecord, SubsidyMunicipalityRecordViewModel>();
            Container.RegisterViewModel<SubsidyRecord, SubsidyRecordViewModel>();
            Container.RegisterViewModel<SpecialAccount, SpecialAccountViewModel>();
            Container.RegisterViewModel<AccrualsAccount, AccrualsAccountViewModel>();
            Container.RegisterViewModel<PaymentAccount, PaymentAccountViewModel>();
            Container.RegisterViewModel<AccrualsAccountOperation, AccrualsAccountOperationViewModel>();

            Container.RegisterViewModel<DpkrGroupedYear, DpkrGroupedYearModel>();
            Container.RegisterViewModel<DpkrCorrectionStage2, DpkrCorrectionViewModel>();

            Container.RegisterViewModel<ShortProgramRecord, ShortProgramRecordViewModel>();

            Container.RegisterViewModel<LongTermPrObject, LongTermPrObjViewModel>();
            Container.RegisterViewModel<PropertyOwnerProtocols, PropertyOwnerProtocolsViewModel>();
            Container.RegisterViewModel<BasePropertyOwnerDecision, BasePropertyOwnerDecisionViewModel>();
            Container.RegisterViewModel<SpecialAccountDecision, SpecialAccountDecisionViewModel>();
            Container.RegisterViewModel<PropertyOwnerDecisionWork, PropertyOwnerDecisionWorkViewModel>();
            Container.RegisterViewModel<OwnerAccountDecision, OwnerAccountDecisionViewModel>();
            Container.RegisterViewModel<RealEstateType, RealEstateTypeViewModel>();
            Container.RegisterViewModel<RealEstateTypeCommonParam, RealEstateTypeCommonParamViewModel>();
            Container.RegisterViewModel<RealEstateTypeStructElement, RealEstateTypeStructElementViewModel>();
            Container.RegisterViewModel<RealEstateTypePriorityParam, RealEstateTypePriorityParamViewModel>();
            Container.RegisterViewModel<RealEstateTypeRate, RealEstateTypeRateViewModel>();
     
            Container.RegisterViewModel<VersionRecord, VersionRecordViewModel>();
            Container.RegisterViewModel<VersionParam, VersionParamViewModel>();

            Container.RegisterViewModel<ShortProgramDifitsit, ShortProgramDeficitViewModel>();

            Container.RegisterViewModel<PublishedProgramRecord, PublishedProgramRecordViewModel>();

            Container.RegisterViewModel<QualityPriorityParam, QualityPriorityParamViewModel>();
            Container.RegisterViewModel<QuantPriorityParam, QuantPriorityParamViewModel>();
            Container.RegisterViewModel<MultiPriorityParam, MultiPriorityParamViewModel>();
            Container.RegisterViewModel<SpecialAccountDecisionNotice, SpecialAccountDecisionNoticeViewModel>();
            Container.RegisterViewModel<MinAmountDecision, MinAmountDecisionViewModel>();

            Container.RegisterViewModel<LongTermObjectLoan, LongTermObjectLoanViewModel>();

            Container.RegisterViewModel<BaseOperation, BaseOperationViewModel>();
            Container.RegisterViewModel<AccBankStatement, AccBankStatementViewModel>();
            Container.RegisterViewModel<ContributionCollection, ContributionCollectionViewModel>();
        }

        private void RegisterServices()
        {

            Container.RegisterTransient<IPriorityService, PriorityService>();

            Container.RegisterTransient<IRealityObjectStructElementService, RealityObjectStructElementService>();

            Container.RegisterTransient<IActualizeVersionService, ActualizeVersionService>();

            Container.RegisterTransient<IActualizeVersionLogService, ActualizeVersionLogService>();

            Container.RegisterTransient<IWorkPriceService<NsoWorkPrice>,  NsoWorkPriceService>();

            Container.RegisterTransient<IObjectCrIntegrationService, ObjectCrIntegrationService>();

            Container.RegisterTransient<IObjectCrDpkrDataService, ObjectCrDpkrDataService>();

            Container.RegisterTransient<ILongProgramService, LongProgramService>();
            Container.RegisterTransient<ISubsidyMunicipalityService, SubsidyMunicipalityService>();
            Container.RegisterTransient<ISubsidyRecordService, SubsidyRecordService>();
            Container.RegisterTransient<IPropertyOwnerDecisionWorkService, PropertyOwnerDecisionWorkService>();
            Container.RegisterTransient<IRealEstateTypeRateService, RealEstateTypeRateService>();

            Container.ReplaceComponent(Component.For<IListCeoService>().ImplementedBy<NsoListCeoService>().LifestyleTransient());

            Container.RegisterTransient<IAccountOperationService, AccountOperationService>();

            Container.RegisterTransient<IDpkrParamsService, ConfigDpkrParamsService>();

            Container.RegisterTransient<IShortProgramDeficitService, ShortProgramDeficitService>();

            // домен сервис для портальных экшенов
            Container.RegisterTransient<IDpkrService, DpkrService>();

            Container.RegisterTransient<IDpkrCorrectionService, DpkrCorrectionService>();

            Container.RegisterTransient<IPublishProgramService, PublishedProgramService>();

            Container.RegisterTransient<IPriorityParamService, PriorityParamService>();
            Container.RegisterTransient<ILongTermPrObjectService, LongTermPrObjectService>();
            Container.RegisterTransient<ILongTermObjectLoanService, LongTermObjectLoanService>();
            Container.RegisterTransient<IDecisionNoticeService, DecisionNoticeService>();

            Container.RegisterTransient<Overhaul.DomainService.IDpkrService, NsoDpkrService>();

            Container.RegisterTransient<ILongTermProgramService, LongTermProgramService>();
            Container.RegisterTransient<IExecutionLongTermProgramService, ExecutionLongTermProgramService>();

            Container.RegisterTransient<IPriorityParamAdditionService, PriorityParamAdditionService>();
            
            Container.RegisterTransient<IRealEstateTypeMunicipalityService, RealEstateTypeMunicipalityService>();

            Container.RegisterTransient<IRealityObjectDpkrDataService, NsoRealityObjectDpkrDataService>();
            Container.RegisterTransient<IOwnerAccountDecisionService, OwnerAccountDecisionService>();
            Container.RegisterTransient<IPublishProgramWcfService, NsoPublishProgramWcfService>();
            Container.RegisterTransient<IRepairKonstWcfService, NsoRepairKonstWcfService>();
            Container.RegisterTransient<IProgramVersionService, ProgramVersionService>();
            Container.RegisterTransient<IOverhaulToGasuExportService, OverhaulToGasuExportService>();

            Container.RegisterTransient<IRealityObjectsInPrograms, NsoRealityObjectsInPrograms>();
            Container.RegisterTransient<IProgramCrImportService, ProgramCrImportService>();
            Component.For<INsoRealEstateTypeService, IRealEstateTypeService>()
                .ImplementedBy<NsoRealEstateTypeService>()
                .LifestyleTransient()
                .RegisterIn(Container);

            Container.RegisterTransient<IDpkrRealityObjectService, NsoDpkrRealityObjectService>();

            Container.RegisterTransient<IDpkrTypeWorkService, DpkrTypeWorkService>();
            Container.RegisterTransient<IRealObjOverhaulDataService, NsoRealObjOverhaulDataService>();
            Container.RegisterTransient<IRealityObjectsProgramVersion, RealityObjectsProgramVersion>();
            Container.RegisterTransient<ITypeWorkStage1Service, TypeWorkStage1Service>();
            Container.RegisterTransient<IOverhaulViewModels, OverhaulViewModels>();
            Container.RegisterTransient<IChangeVersionSt1Service, ChangeVersionSt1Service>();
            this.Container.RegisterTransient<BaseRealObjOverhaulDataObject, NsoRealObjOverhaulDataObject>("RealObjOverhaulDataObject");
            Container.ReplaceComponent(Component.For<IDefectService>().ImplementedBy<DefectService>().LifestyleTransient());
        }

        private void RegisterDomainServices()
        {
            Container.ReplaceComponent<IDomainService<WorkPrice>>(typeof(Overhaul.DomainService.WorkPriceDomainService),
               Component.For<IDomainService<WorkPrice>>().ImplementedBy<Bars.Gkh.Overhaul.Nso.DomainService.WorkPriceDomainService>());

            Container.RegisterTransient<IDomainService<ProgramVersion>, ProgramVersionDomainService>();
            Container.RegisterTransient<IDomainService<PropertyOwnerProtocols>, FileStorageDomainService<PropertyOwnerProtocols>>();
            Container.RegisterTransient<IProgramCrRealityObjectService, ProgramCrRealityObjectService>();

            Container.RegisterDomainService<SpecialAccountDecision, FileStorageDomainService<SpecialAccountDecision>>();
            Container.RegisterDomainService<SpecialAccountDecisionNotice, FileStorageDomainService<SpecialAccountDecisionNotice>>();
            Container.RegisterDomainService<LongTermObjectLoan, FileStorageDomainService<LongTermObjectLoan>>();
            Container.RegisterDomainService<VersionRecord, FileStorageDomainService<VersionRecord>>();

            Container.ReplaceComponent<ICommonEstateObjectService>(typeof(Overhaul.DomainService.Impl.CommonEstateObjectService),
               Component.For<ICommonEstateObjectService>().ImplementedBy<CommonEstateObjectService>());
            
        }

        private void RegisterNavigations()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();

            Container.RegisterTransient<INavigationProvider, LongTermPrObjectFullMenuProvider>();
            Container.RegisterTransient<INavigationProvider, LongTermPrObjectMenuProvider>();
            Container.RegisterTransient<INavigationProvider, LongTermPrObjectRealMenuProvider>();
            Container.RegisterTransient<INavigationProvider, LongTermPrObjectSpecialMenuProvider>();
        }

        private void RegisterInterceptors()
        {
            Container.RegisterTransient<IDomainServiceInterceptor<NsoWorkPrice>, NsoWorkPriceInterceptor>();

            Container.RegisterTransient<IDomainServiceInterceptor<SubsidyMunicipality>, SubsidyMunicipalityInterceptor>();
            Container.ReplaceComponent<IDomainServiceInterceptor<RealEstateType>, Gkh.Interceptors.RealEstateType.RealEstateTypeInterceptor>(
                Component.For<IDomainServiceInterceptor<RealEstateType>>()
                    .ImplementedBy<RealEstateTypeInterceptor>()
                    .LifestyleTransient());
            Container.RegisterTransient<IDomainServiceInterceptor<ProgramVersion>, ProgramVersionInterceptor>();
            Container.ReplaceComponent<IDomainServiceInterceptor<RealityObject>, RealityObjectInterceptor>(
                Component.For<IDomainServiceInterceptor<RealityObject>>()
                    .ImplementedBy<Interceptors.RealityObjectInterceptor>()
                    .LifestyleTransient());
            Container.RegisterTransient<IDomainServiceInterceptor<SubsidyRecord>, SubsidyRecordInterceptor>();

            Container.RegisterDomainInterceptor<LongTermPrObject, LongTermPrObjectInterceptor>();

            Container.RegisterDomainInterceptor<SpecialAccountDecision, SpecialAccountDecisionInterceptor>();
            Container.RegisterDomainInterceptor<SpecialAccountDecisionNotice, SpecialAccountDecisionNoticeInterceptor>();
            Container.RegisterDomainInterceptor<MinAmountDecision, MinAmountDecisionInterceptor>();
            Container.RegisterDomainInterceptor<MinFundSizeDecision, MinFundSizeDecisionInterceptor>();
            Container.RegisterDomainInterceptor<CreditOrganizationDecision, CreditOrganizationDecisionInterceptor>();
            Container.RegisterDomainInterceptor<ListServicesDecision, ListServicesDecisionInterceptor>();
            Container.RegisterDomainInterceptor<OwnerAccountDecision, OwnerAccountDecisionInterceptor>();
            Container.RegisterDomainInterceptor<PrevAccumulatedAmountDecision, PrevAccumulatedAmountDecisionInterceptor>();
            Container.RegisterDomainInterceptor<BaseOperation, BaseOperationInterceptor>();
            Container.RegisterDomainInterceptor<AccBankStatement, AccBankStatementInterceptor>();

            Container.RegisterDomainInterceptor<ContributionCollection, ContributionCollectionInterceptor>();
            Container.RegisterDomainInterceptor<DefectList, DefectListInterceptor>();
            Container.RegisterDomainInterceptor<TypeWorkCr, TypeWorkCrInterceptor>();

            Container.ReplaceComponent<IDomainServiceInterceptor<TypeWorkCrRemoval>>(typeof(GkhCr.Interceptors.TypeWorkCrRemovalInterceptor),
                Component.For<IDomainServiceInterceptor<TypeWorkCrRemoval>>().ImplementedBy<Bars.Gkh.Overhaul.Nso.Interceptors.TypeWorkCrRemovalInterceptor>().LifeStyle.Transient);
        }

        private void RegisterExecutionActions()
        {
            Container.RegisterExecutionAction<StructElementCopyAction>();
            //Container.RegisterExecutionAction<CopyAreaToStructElementAction>();
            Container.RegisterExecutionAction<ConstructiveElementCopyAction>();
            Container.RegisterExecutionAction<ProgramCrChangesCreationAction>();
            Container.RegisterExecutionAction<TypeWorkCrSetYearRepairAction>();
            Container.RegisterExecutionAction<OverhaulNsoConfigMigrationAction>();
        }

        private void RegisterStateChangeRules()
        {
            Container.RegisterTransient<IRuleChangeStatus, PublishProgramDeleteDateTransferRule>();
            Container.Register(Component.For<IRuleChangeStatus, IGkhRuleChangeStatus>().ImplementedBy<PublishProgramAddDateTransferRule>().LifestyleTransient());

            Container.Register(Component.For<IStateChangeHandler>().ImplementedBy<DefectListStateChangeHandler>().LifeStyle.Transient);
            Container.RegisterTransient<IRuleChangeStatus, PerfWorkActStateChangeRule>();
        }
    }
}