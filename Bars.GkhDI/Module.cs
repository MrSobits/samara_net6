namespace Bars.GkhDi
{
    using B4.Modules.Reports;
    using B4;
    using B4.IoC;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using B4.Modules.FileStorage.DomainService;
    using B4.Modules.NHibernateChangeLog;
    using B4.Modules.States;
    using B4.Windsor;

    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Log;
    using Bars.GkhDi.Log;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.Modules.GkhDi.Entities;
    using Bars.GkhDi.ConfigSections;
    using Bars.GkhDi.DomainService.Dict.TemplateOtherService;
    using Bars.GkhDi.DomainService.Service;
    using Bars.GkhDi.Entities.Service;
    using Bars.GkhDi.FormatDataExport.ExportableEntities.Impl;
    using Bars.GkhDi.FormatDataExport.ProxySelectors.Impl;
    using Bars.GkhDi.Import.CommunalPay;
    using Bars.GkhDi.MetaValueConstructor;
    using Bars.GkhDi.Tasks;

    using Gkh.Entities;
    using Gkh.ExecutionAction;
    using GkhCR.Interceptors;
    using Controllers;
    using DomainService;
    using Entities;
    using ExecutionAction;
    using Export;
    using GroupAction;
    using Import.Sections;
    using Interceptors;
    using PercentCalculationProvider;
    using Permissions;
    using Report;
    using Services;
    using Services.Domain;
    using Castle.MicroKernel.Registration;
    using DomainService.Impl;
    using Gkh.ImportExport;
    using Gkh.Report;
    using Gkh.Utils;
    using Report.Fucking731;
    using Services.Impl;
    using ViewModels;
    
    using DisclosureInfo731 = Bars.GkhDi.Report.Fucking731.DisclosureInfo731;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IEntityExportProvider, EntityExportProvider>();

            this.Container.RegisterGkhConfig<DiConfig>();

            // Групповые операции
            this.Container.Register(Component.For<IDiGroupAction>().ImplementedBy<DiCopyServicePeriod>());
            this.Container.Register(Component.For<IDiGroupAction>().ImplementedBy<DiLoadWorkPprGroupAction>());

            // Регистрация класса для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.GkhDi dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.RegisterBundlers();

            this.RegisterCommonComponents();

            this.RegisterControllers();

            this.RegisterDomainServices();

            this.RegisterExports();

            this.RegisterImports();

            this.RegisterInterceptors();

            this.RegisterPrintForms();

            this.RegisterServices();

            this.RegisterViewModels();

            this.RegisterWebServices();

            this.RegisterAuditLogMap();

            this.Container.RegisterTransient<IPercentCalculation, PercentCalculationProvider.PercentCalculation>("PercentCalculation");
            this.Container.RegisterTransient<IPercentCalculation, PercentCalculation988>("PercentCalculation988");
            this.Container.RegisterTransient<IPercentCalculation, B3PercentCalculation>("B3PercentCalculation");

            this.Container.RegisterTransient<IServiceDi, ServiceDi>();

            this.Container.RegisterTaskExecutor<PercentCalculationTaskExecutor>(nameof(PercentCalculationTaskExecutor));

            this.Container.RegisterExecutionAction<SaveReport731ToDicrectoryAction>();

            this.Container.RegisterSingleton<IConstructorDataFillerMap, DiConstructorDataFillerMap>();
            this.Container.RegisterTransient<IAsyncLogger<DisclosureInfoEmptyFieldsBase>, DisclosureInfoEmptyFieldsLogger>();

            this.RegisterFormatDataExport();
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IPermissionSource, GkhDiPermissionMap>();
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhDi resources");
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhDi navigation");
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhDi statefulentity");
        }

        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, GkhDI.LogMap.Provider.AuditLogMapProvider>();
        }

        private void RegisterControllers()
        {
            #region FinActivity

            this.Container.RegisterController<FileStorageDataController<FinActivityAudit>>();
            this.Container.RegisterController<FileStorageDataController<FinActivityDocByYear>>();
            this.Container.RegisterController<FileStorageDataController<InfoAboutUseCommonFacilities>>();

            #endregion FinActivity

            #region Documents

            this.Container.RegisterController<FileStorageDataController<DocumentsRealityObjProtocol>>();

            #endregion Documents

            #region Service

            this.Container.RegisterController<FileStorageDataController<HousingService>>();

            #endregion Service

            this.Container.RegisterController<DiReportController>();

            // Меню
            this.Container.RegisterController<MenuDiController>();

            // Сущности
            this.Container.RegisterController<DisclosureInfoController>();
            this.Container.RegisterController<FileStorageDataController<DisclosureInfoLicense>>();
            this.Container.RegisterController<DisclosureInfoRealityObjController>();
            this.Container.RegisterController<TerminateContractController>();
            this.Container.RegisterController<MembershipUnionsController>();
            this.Container.RegisterAltDataController<FundsInfo>();
            this.Container.RegisterAltDataController<Actions>();
            this.Container.RegisterController<FinActivityController>();
            this.Container.RegisterController<AdminRespController>();
            this.Container.RegisterController<FinActivityManagRealityObjController>();
            this.Container.RegisterController<FinActivityRepairCategoryController>();
            this.Container.RegisterController<FinActivityRepairSourceController>();
            this.Container.RegisterController<FinActivityCommunalServiceController>();
            this.Container.RegisterController<FinActivityRealityObjCommunalServiceController>();
            this.Container.RegisterController<FinActivityManagCategoryController>();
            this.Container.RegisterController<DocumentsController>();
            this.Container.RegisterController<DocumentsRealityObjController>();
            this.Container.RegisterAltDataController<NonResidentialPlacement>();
            this.Container.RegisterAltDataController<NonResidentialPlacementMeteringDevice>();
            this.Container.RegisterAltDataController<InformationOnContracts>();
            this.Container.RegisterController<ServiceController>();
            this.Container.RegisterAltDataController<CommunalService>();
            this.Container.RegisterAltDataController<CapRepairService>();
            this.Container.RegisterAltDataController<RepairService>();
            this.Container.RegisterAltDataController<ControlService>();
            this.Container.RegisterAltDataController<AdditionalService>();
            this.Container.RegisterAltDataController<TariffForConsumers>();
            this.Container.RegisterAltDataController<TariffForConsumersOtherService>();
            this.Container.RegisterAltDataController<TariffForRso>();
            this.Container.RegisterAltDataController<ConsumptionNormsNpa>();
            this.Container.RegisterAltDataController<WorkCapRepair>();
            this.Container.RegisterAltDataController<CostItem>();
            this.Container.RegisterController<WorkRepairListController>();
            this.Container.RegisterController<WorkRepairDetailController>();
            this.Container.RegisterController<WorkRepairTechServController>();
            this.Container.RegisterController<PlanWorkServiceRepairController>();
            this.Container.RegisterAltDataController<PlanWorkServiceRepairWorks>();
            this.Container.RegisterController<PlanReductionExpenseController>();
            this.Container.RegisterAltDataController<PlanReductionExpenseWorks>();
            this.Container.RegisterController<InfoAboutReductionPaymentController>();
            this.Container.RegisterController<InfoAboutPaymentCommunalController>();
            this.Container.RegisterController<InfoAboutPaymentHousingController>();
            this.Container.RegisterAltDataController<OtherService>();
            this.Container.RegisterAltDataController<ProviderService>();
            this.Container.RegisterAltDataController<ProviderOtherService>();
            this.Container.RegisterController<GroupDiController>();
            this.Container.RegisterController<RealityObjGroupController>();
            this.Container.RegisterController<FinActivityDocsController>();
            this.Container.RegisterController<PercentCalculationController>();

            // Справочники
            this.Container.RegisterAltDataController<PeriodDi>();
            this.Container.RegisterAltDataController<TaxSystem>();
            this.Container.RegisterAltDataController<SupervisoryOrg>();
            this.Container.RegisterAltDataController<PeriodicityTemplateService>();
            this.Container.RegisterController<TemplateServiceController>();
            this.Container.RegisterAltDataController<TemplateOtherService>();
            this.Container.RegisterAltDataController<TemplateServiceOptionFields>();
            this.Container.RegisterAltDataController<GroupWorkTo>();
            this.Container.RegisterAltDataController<GroupWorkPpr>();
            this.Container.RegisterController<WorkToController>();
            this.Container.RegisterController<WorkPprController>();

            this.Container.RegisterAltDataController<DisclosureInfoEmptyFields>();
            this.Container.RegisterAltDataController<DisclosureInfoRealityObjEmptyFields>();
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterDomainService<DocumentsRealityObj, FileStorageDomainService<DocumentsRealityObj>>();
            this.Container.RegisterDomainService<Documents, FileStorageDomainService<Documents>>();
            this.Container.RegisterDomainService<DocumentsRealityObjProtocol, FileStorageDomainService<DocumentsRealityObjProtocol>>();
            this.Container.RegisterDomainService<HousingService, FileStorageDomainService<HousingService>>();
            this.Container.RegisterDomainService<DisclosureInfo, FileStorageDomainService<DisclosureInfo>>();
            this.Container.RegisterDomainService<FinActivityDocs, FileStorageDomainService<FinActivityDocs>>();
            this.Container.RegisterDomainService<FinActivityDocByYear, FileStorageDomainService<FinActivityDocByYear>>();
            this.Container.RegisterDomainService<FinActivityAudit, FileStorageDomainService<FinActivityAudit>>();
            this.Container.RegisterDomainService<AdminResp, FileStorageDomainService<AdminResp>>();
            this.Container.RegisterDomainService<DisclosureInfoRealityObj, FileStorageDomainService<DisclosureInfoRealityObj>>();
            this.Container.RegisterDomainService<DisclosureInfoLicense, FileStorageDomainService<DisclosureInfoLicense>>();
            this.Container.RegisterDomainService<InfoAboutUseCommonFacilities, FileStorageDomainService<InfoAboutUseCommonFacilities>>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, TemplateServiceDataExport>("TemplateServiceDataExport");
            this.Container.RegisterTransient<IDataExportService, WorkPprDataExport>("WorkPprDataExport");
            this.Container.RegisterTransient<IDataExportService, WorkToDataExport>("WorkToDataExport");
        }

        private void RegisterImports()
        {
            this.Container.RegisterImport<CommunalPayImport>();
            this.Container.RegisterTransient<ISectionImport, SectionImport1>();
            this.Container.RegisterTransient<ISectionImport, SectionImport2>();
            this.Container.RegisterTransient<ISectionImport, SectionImport3>();
            this.Container.RegisterTransient<ISectionImport, SectionImport4>();
            this.Container.RegisterTransient<ISectionImport, SectionImport5>();
            this.Container.RegisterTransient<ISectionImport, SectionImport6>();
            this.Container.RegisterTransient<ISectionImport, SectionImport7>();
            this.Container.RegisterTransient<ISectionImport, SectionImport8>();
            this.Container.RegisterTransient<ISectionImport, SectionImport9>();
            this.Container.RegisterTransient<ISectionImport, SectionImport10>();
            this.Container.RegisterTransient<ISectionImport, SectionImport12>();
            this.Container.RegisterTransient<ISectionImport, SectionImport13>();
            this.Container.RegisterTransient<ISectionImport, SectionImport14>();
            this.Container.RegisterTransient<ISectionImport, SectionImport15>();
            this.Container.RegisterTransient<ISectionImport, SectionImport16>();
            this.Container.RegisterTransient<ISectionImport, SectionImport17>();
            this.Container.RegisterTransient<ISectionImport, SectionImport18>();
            this.Container.RegisterTransient<ISectionImport, SectionImport19>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<TemplateService, TemplateServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ProviderService, ProviderServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<AdditionalService, AdditionalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ControlService, ControlServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<HousingService, HousingServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<CommunalService, CommunalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<RepairService, RepairServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<CapRepairService, CapRepairServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<DisclosureInfo, DisclosureInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<TariffForConsumers, TariffForConsumersInterceptor>();
            this.Container.RegisterDomainInterceptor<WorkRepairList, WorkRepairListInterceptor>();
            this.Container.RegisterDomainInterceptor<AdminResp, AdminRespInterceptor>();

            this.Container.RegisterDomainInterceptor<DisclosureInfoRealityObj, DisclosureInfoRealityObjInterceptor>();
            this.Container.RegisterDomainInterceptor<GroupWorkPpr, GroupWorkPprInterceptor>();
            this.Container.RegisterDomainInterceptor<GroupWorkTo, GroupWorkToInterceptor>();
            this.Container.RegisterDomainInterceptor<NonResidentialPlacement, NonResidentialPlacementInterceptor>();

            this.Container.RegisterDomainInterceptor<PeriodDi, PeriodDiInterceptor>();
            this.Container.RegisterDomainInterceptor<PeriodicityTemplateService, PeriodicityTemplateServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<PlanReductionExpense, PlanReductionExpenseInterceptor>();
            this.Container.RegisterDomainInterceptor<PlanWorkServiceRepair, PlanWorkServiceRepairInterceptor>();

            this.Container.RegisterDomainInterceptor<SupervisoryOrg, SupervisoryOrgInterceptor>();
            this.Container.RegisterDomainInterceptor<TaxSystem, TaxSystemInterceptor>();
            this.Container.RegisterDomainInterceptor<WorkRepairDetail, WorkRepairDetailInterceptor>();
            this.Container.RegisterDomainInterceptor<WorkTo, WorkToInterceptor>();

            this.Container.RegisterDomainInterceptor<RealityObjectResOrgService, RealityObjectResOrgServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<DocumentsRealityObj, DocumentsRealityObjInterceptor>();
        }

        private void RegisterPrintForms()
        {
            this.Container.RegisterTransient<IPrintForm, PercentCalculationReport>("DI Report.PercentCalculation");
            this.Container.RegisterTransient<IPrintForm, B3PercentCalculationReport>("DI Report.B3PercentCalculation");
            this.Container.RegisterTransient<IPrintForm, WeeklyDisclosureInfoReport>("DI Report.WeeklyDi");
            this.Container.RegisterTransient<IPrintForm, FillingDataForRankingManOrgReport>("DI Report.FillingDataForRankingManOrg");

            this.Container.RegisterTransient<IPrintForm, FillGeneralDataRatingCr>("DI Report.FillGeneralDataRatingCR");
            this.Container.RegisterTransient<IPrintForm, FillFinanceDataRatingCr>("DI Report.FillFinanceDataRatingCr");
            this.Container.RegisterTransient<IPrintForm, FillingTechPassportForManOrgRatingReport>("DI Report.FillingTechPassportForManOrgRatingReport");
            this.Container.RegisterTransient<IPrintForm, FillGenInformationHousesRankingYK>("DI Report.FillGenInformationHousesRankingYK");
            this.Container.RegisterTransient<IPrintForm, ChangeCommunalServicesTariffReport>("DI Report.ChangeCommunalServicesTariffReport");

            this.Container.RegisterTransient<IGkhBaseReport, DisclosureInfo731>("DisclosureInfo731");
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IAdminRespService, AdminRespService>();
            this.Container.RegisterTransient<ITemplateServService, TemplateServService>();
            this.Container.RegisterTransient<IDocumentsRealityObjService, DocumentsRealityObjService>();
            this.Container.RegisterTransient<IDocumentsService, DocumentsService>();
            this.Container.RegisterTransient<IFinActivityCommunalServService, FinActivityCommunalServService>();
            this.Container.RegisterTransient<IFinActivityService, FinActivityService>();
            this.Container.RegisterTransient<IFinActivityDocsService, FinActivityDocsService>();
            this.Container.RegisterTransient<IFinActivityManagCatService, FinActivityManagCatService>();
            this.Container.RegisterTransient<IFinActivityManagRealityObjService, FinActivityManagRealityObjService>();
            this.Container.RegisterTransient<IFinActivityRealityObjCommunalServService, FinActivityRealityObjCommunalServService>();
            this.Container.RegisterTransient<IFinActivityRepairCategoryService, FinActivityRepairCategoryService>();
            this.Container.RegisterTransient<IFinActivityRepairSourceService, FinActivityRepairSourceService>();
            this.Container.RegisterTransient<IInfoAboutPaymentCommunalService, InfoAboutPaymentCommunalService>();
            this.Container.RegisterTransient<IInfoAboutPaymentHousingService, InfoAboutPaymentHousingService>();
            this.Container.RegisterTransient<IInfoAboutReductionPaymentService, InfoAboutReductionPaymentService>();
            this.Container.RegisterTransient<IPlanReductionExpenseService, PlanReductionExpenseService>();
            this.Container.RegisterTransient<IPlanWorkServiceRepairService, PlanWorkServiceRepairService>();
            this.Container.RegisterTransient<IServService, ServService>();
            this.Container.RegisterTransient<IWorkRepairDetailService, WorkRepairDetailService>();
            this.Container.RegisterTransient<IWorkRepairListService, WorkRepairListService>();
            this.Container.RegisterTransient<IWorkRepairTechServService, WorkRepairTechServService>();
            this.Container.RegisterTransient<IDisclosureInfoService, DisclosureInfoService>();
            this.Container.RegisterTransient<IDisclosureInfoRealityObjService, DisclosureInfoRealityObjService>();
            this.Container.RegisterTransient<IRealityObjGroupService, RealityObjGroupService>();
            this.Container.RegisterTransient<IGroupDiService, GroupDiService>();

            this.Container.RegisterTransient<IServicePprService, ServicePprService>();
            this.Container.RegisterTransient<IDiRealityObjectViewModelService, DiRealityObjectViewModelService>();
            this.Container.RegisterTransient<IRealtyObjectLiftService, RealtyObjectLiftService>();
            this.Container.RegisterTransient<IDisclosureInfoHouseManagingMoneys, DisclosureInfoHouseManagingMoneys>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<AdminResp, AdminRespViewModel>();
            this.Container.RegisterViewModel<Actions, ActionsViewModel>();
            this.Container.RegisterViewModel<TemplateServiceOptionFields, OptionFieldsViewModel>();
            this.Container.RegisterViewModel<TemplateService, TemplateServiceViewModel>();
            this.Container.RegisterViewModel<TemplateOtherService, TemplateOtherServiceViewModel>();
            this.Container.RegisterViewModel<GroupWorkPpr, GroupWorkPprViewModel>();
            this.Container.RegisterViewModel<GroupWorkTo, GroupWorkToViewModel>();
            this.Container.RegisterViewModel<PeriodDi, PeriodDiViewModel>();
            this.Container.RegisterViewModel<TaxSystem, TaxSystemViewModel>();
            this.Container.RegisterViewModel<WorkPpr, WorkPprViewModel>();
            this.Container.RegisterViewModel<WorkTo, WorkToViewModel>();
            this.Container.RegisterViewModel<DisclosureInfo, DisclosureInfoViewModel>();
            this.Container.RegisterViewModel<DisclosureInfoRealityObj, DisclosureInfoRealityObjectViewModel>();
            this.Container.RegisterViewModel<DocumentsRealityObjProtocol, DocumentsRealityObjProtocolViewModel>();
            this.Container.RegisterViewModel<Documents, DocumentsViewModel>();
            this.Container.RegisterViewModel<FinActivityAudit, FinActivityAuditViewModel>();
            this.Container.RegisterViewModel<FinActivityDocByYear, FinActivityDocByYearViewModel>();
            this.Container.RegisterViewModel<FinActivityCommunalService, FinActivityCommunalViewModel>();
            this.Container.RegisterViewModel<FinActivityManagCategory, FinActivityManagCatViewModel>();
            this.Container.RegisterViewModel<FinActivityDocs, FinActivityDocsViewModel>();
            this.Container.RegisterViewModel<FinActivityManagRealityObj, FinActivityManagRealityObjViewModel>();
            this.Container.RegisterViewModel<FinActivityRealityObjCommunalService, FinActivityRealityObjCommunalViewModel>();
            this.Container.RegisterViewModel<FinActivityRepairCategory, FinActivityRepairCategoryViewModel>();
            this.Container.RegisterViewModel<FinActivityRepairSource, FinActivityRepairSourceViewModel>();
            this.Container.RegisterViewModel<FundsInfo, FundsInfoViewModel>();
            this.Container.RegisterViewModel<GroupDi, GroupDiViewModel>();
            this.Container.RegisterViewModel<RealityObjGroup, RealityObjGroupViewModel>();
            this.Container.RegisterViewModel<InfoAboutPaymentCommunal, InfoAboutPaymentCommunalViewModel>();
            this.Container.RegisterViewModel<InfoAboutPaymentHousing, InfoAboutPaymentHousingViewModel>();
            this.Container.RegisterViewModel<InfoAboutReductionPayment, InfoAboutReductionPaymentViewModel>();
            this.Container.RegisterViewModel<InfoAboutUseCommonFacilities, InfoAboutUseCommonFacilitiesViewModel>();
            this.Container.RegisterViewModel<InformationOnContracts, InformationOnContractsViewModel>();
            this.Container.RegisterViewModel<ManagingOrgMembership, MembershipUnionsViewModel>("MembershipUnions");
            this.Container.RegisterViewModel<NonResidentialPlacement, NonResidentialPlacementViewModel>();
            this.Container.RegisterViewModel<NonResidentialPlacementMeteringDevice, MeteringDeviceViewModel>();
            this.Container.RegisterViewModel<PlanReductionExpenseWorks, PlanRedExpWorksViewModel>();
            this.Container.RegisterViewModel<PlanReductionExpense, PlanReductionExpenseViewModel>();
            this.Container.RegisterViewModel<PlanWorkServiceRepair, PlanWorkServiceRepairViewModel>();
            this.Container.RegisterViewModel<PlanWorkServiceRepairWorks, PlanWorkServiceRepairWorksViewModel>();
            this.Container.RegisterViewModel<CostItem, CostItemViewModel>();
            this.Container.RegisterViewModel<OtherService, OtherServiceViewModel>();
            this.Container.RegisterViewModel<ProviderService, ProviderServViewModel>();
            this.Container.RegisterViewModel<ProviderOtherService, ProviderOtherServiceViewModel>();
            this.Container.RegisterViewModel<BaseService, ServiceViewModel>();
            this.Container.RegisterViewModel<TariffForConsumers, TariffForConsumersViewModel>();
            this.Container.RegisterViewModel<TariffForConsumersOtherService, TariffForConsumersOtherServiceViewModel>();
            this.Container.RegisterViewModel<WorkCapRepair, WorkCapRepairViewModel>();
            this.Container.RegisterViewModel<WorkRepairDetail, WorkRepairDetailViewModel>();
            this.Container.RegisterViewModel<WorkRepairList, WorkRepairListViewModel>();
            this.Container.RegisterViewModel<WorkRepairTechServ, WorkRepairTechServViewModel>();
            this.Container.RegisterViewModel<ManOrgContractRealityObject, TerminateContractViewModel>("TerminateContract");
            this.Container.RegisterViewModel<TariffForRso, TariffForRsoViewModel>();
            this.Container.RegisterViewModel<FinActivity, FinActivityViewModel>();
            this.Container.RegisterViewModel<ConsumptionNormsNpa, ConsumptionNormsNpaViewModel>();
            this.Container.RegisterViewModel<DisclosureInfoLicense, DisclosureInfoLicenseViewModel>();
            this.Container.RegisterViewModel<DisclosureInfoEmptyFields, DisclosureInfoEmptyFieldsViewModel>();
            this.Container.RegisterViewModel<DisclosureInfoRealityObjEmptyFields, DisclosureInfoRealityObjEmptyFieldsViewModel>();
        }

        private void RegisterWebServices()
        {
            // TODO: wcf
            // Component.For<IService>().ImplementedBy<Service>().AsWcfSecurityService().RegisterIn(this.Container);
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.RegisterExportableEntity<DogPoiExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DogPoiFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DogPoiProtocolossExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<TarifExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<TarifRsoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<TarifOktmoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<TarifUslugaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<TarifResExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<WorkExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<WorkListExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<WorkRequiredExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<WorkUslugaExportableEntity>(this.Container);

            ContainerHelper.RegisterProxySelectorService<DogPoiProxy, DogPoiSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<TarifProxy, TarifSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<WorkProxy, WorkSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<WorkUslugaProxy, WorkUslugaSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<WorkListProxy, WorkListSelectorService>(this.Container);
        }
    }
}
