namespace Bars.Gkh.Gis
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.B4.Windsor;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.CommonParams;
    using Bars.Gkh.Gis.CommonParams.Impl;
    using Bars.Gkh.Gis.ConfigSections;
    using Bars.Gkh.Gis.Controllers;
    using Bars.Gkh.Gis.Controllers.Dict;
    using Bars.Gkh.Gis.Controllers.GisAddressMatching;
    using Bars.Gkh.Gis.Controllers.GisRealEstate;
    using Bars.Gkh.Gis.Controllers.House;
    using Bars.Gkh.Gis.Controllers.ImportData;
    using Bars.Gkh.Gis.Controllers.ImportExport;
    using Bars.Gkh.Gis.Controllers.ManOrg;
    using Bars.Gkh.Gis.Controllers.Olap;
    using Bars.Gkh.Gis.Controllers.PersonalAccount;
    using Bars.Gkh.Gis.Controllers.RealityObject;
    using Bars.Gkh.Gis.Controllers.Register.HouseRegisterRegister;
    using Bars.Gkh.Gis.Controllers.Register.HouseServiceRegister;
    using Bars.Gkh.Gis.Controllers.Register.ServiceSubsidyRegister;
    using Bars.Gkh.Gis.Controllers.Register.TenantSubsidyRegister;
    using Bars.Gkh.Gis.Controllers.Report;
    using Bars.Gkh.Gis.Controllers.Skap;
    using Bars.Gkh.Gis.Controllers.WasteCollection;
    using Bars.Gkh.Gis.Domain.IndicatorsOt;
    using Bars.Gkh.Gis.DomainService;
    using Bars.Gkh.Gis.DomainService.Analysis;
    using Bars.Gkh.Gis.DomainService.Analysis.Impl;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.DomainService.BilConnection.Impl;
    using Bars.Gkh.Gis.DomainService.CalcVerification.Impl;
    using Bars.Gkh.Gis.DomainService.CalcVerification.Intf;
    using Bars.Gkh.Gis.DomainService.CrpCryptoProvider;
    using Bars.Gkh.Gis.DomainService.CrpCryptoProvider.Impl;
    using Bars.Gkh.Gis.DomainService.Dict;
    using Bars.Gkh.Gis.DomainService.Dict.Impl;
    using Bars.Gkh.Gis.DomainService.GisAddressMatching;
    using Bars.Gkh.Gis.DomainService.GisAddressMatching.Impl;
    using Bars.Gkh.Gis.DomainService.Helper.HqlHelper;
    using Bars.Gkh.Gis.DomainService.Helper.HqlHelper.Impl;
    using Bars.Gkh.Gis.DomainService.House;
    using Bars.Gkh.Gis.DomainService.House.Claims;
    using Bars.Gkh.Gis.DomainService.House.Claims.Impl;
    using Bars.Gkh.Gis.DomainService.House.Impl;
    using Bars.Gkh.Gis.DomainService.ImportData;
    using Bars.Gkh.Gis.DomainService.ImportData.Impl;
    using Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData;
    using Bars.Gkh.Gis.DomainService.ImportExport;
    using Bars.Gkh.Gis.DomainService.ImportExport.Impl;
    using Bars.Gkh.Gis.DomainService.Indicator;
    using Bars.Gkh.Gis.DomainService.Indicator.Impl;
    using Bars.Gkh.Gis.DomainService.JExtractor;
    using Bars.Gkh.Gis.DomainService.JExtractor.Impl;
    using Bars.Gkh.Gis.DomainService.ManOrg;
    using Bars.Gkh.Gis.DomainService.ManOrg.Impl;
    using Bars.Gkh.Gis.DomainService.PersonalAccount;
    using Bars.Gkh.Gis.DomainService.PersonalAccount.Impl;
    using Bars.Gkh.Gis.DomainService.RealEstate;
    using Bars.Gkh.Gis.DomainService.RealEstate.Impl;
    using Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister;
    using Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister.Impl;
    using Bars.Gkh.Gis.DomainService.Register.HouseServiceRegister;
    using Bars.Gkh.Gis.DomainService.Register.HouseServiceRegister.Impl;
    using Bars.Gkh.Gis.DomainService.Register.ServiceSubsidyRegister;
    using Bars.Gkh.Gis.DomainService.Register.ServiceSubsidyRegister.Impl;
    using Bars.Gkh.Gis.DomainService.Register.TenantSubsidyRegister;
    using Bars.Gkh.Gis.DomainService.Register.TenantSubsidyRegister.Impl;
    using Bars.Gkh.Gis.DomainService.Report;
    using Bars.Gkh.Gis.DomainService.Report.Impl;
    using Bars.Gkh.Gis.DomainService.UicHouse;
    using Bars.Gkh.Gis.DomainService.UicHouse.Impl;
    using Bars.Gkh.Gis.Entities.Dict;
    using Bars.Gkh.Gis.Entities.IndicatorServiceComparison;
    using Bars.Gkh.Gis.Entities.Kp50;
    using Bars.Gkh.Gis.Entities.ManOrg;
    using Bars.Gkh.Gis.Entities.ManOrg.Contract;
    using Bars.Gkh.Gis.Entities.RealEstate;
    using Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;
    using Bars.Gkh.Gis.Entities.Register.MultipleAnalysis;
    using Bars.Gkh.Gis.Entities.Register.SupplierRegister;
    using Bars.Gkh.Gis.Entities.WasteCollection;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.Gis.ExecutionAction;
    using Bars.Gkh.Gis.Export;
    using Bars.Gkh.Gis.Interceptors.Analysis;
    using Bars.Gkh.Gis.Interceptors.Contragent;
    using Bars.Gkh.Gis.Interceptors.Dict;
    using Bars.Gkh.Gis.Interceptors.ManOrg;
    using Bars.Gkh.Gis.LogMap;
    using Bars.Gkh.Gis.NavigationMenu;
    using Bars.Gkh.Gis.Permissions;
    using Bars.Gkh.Gis.RabbitMQ;
    using Bars.Gkh.Gis.RabbitMQ.Impl;
    using Bars.Gkh.Gis.RepositoryDomain.Impl;
    using Bars.Gkh.Gis.ViewModel.Analysis;
    using Bars.Gkh.Gis.ViewModel.Dict.Normativ;
    using Bars.Gkh.Gis.ViewModel.Dict.Service;
    using Bars.Gkh.Gis.ViewModel.Dict.Tarif;
    using Bars.Gkh.Gis.ViewModel.GisRealEstateType;
    using Bars.Gkh.Gis.ViewModel.KpSettings;
    using Bars.Gkh.Gis.ViewModel.ManOrg;
    using Bars.Gkh.Gis.ViewModel.ManOrg.Contract;
    using Bars.Gkh.Gis.ViewModel.MultipleAnalysis;
    using Bars.Gkh.Gis.ViewModel.WasteCollection;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // маршруты
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.RegisterControllers();

            this.RegisterViewModels();

            this.RegisterDomainServices();

            this.RegisterNavigation();

            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            this.RegisterBundlers();

            this.RegisterServices();

            this.RegisterCommonParams();

            this.RegisterRabbitMq();

            // отчеты
            this.Container.RegisterController<BillingReportController>();

            //права доступа
            this.RegisterPermissions();

            this.RegisterInterceptors();

            // запуск фонового процесса для множественного анализа
            this.Container.Resolve<IMultipleAnalysisService>().StartBackgroundProcess();

            this.RegisterWcfRiaServices();

            this.RegisterExports();

            this.Container.RegisterGkhConfig<GisConfig>();

            this.RegisterExecutionActions();

            this.RegisterAuditLogMap();
        }

        public void RegisterControllers()
        {
            this.Container.RegisterController<PiwikController>();

            this.Container.RegisterController<HouseParamController>();
            this.Container.RegisterController<HouseServiceController>();
            this.Container.RegisterController<HouseAccrualsController>();
            this.Container.RegisterController<HouseCounterController>();
            this.Container.RegisterController<HouseClaimsController>();
            this.Container.RegisterController<PublicControlClaimsController>();

            this.Container.RegisterController<GisPersonalAccountController>();
            this.Container.RegisterController<PersonalAccountParamController>();
            this.Container.RegisterController<PersonalAccountServiceController>();
            this.Container.RegisterController<PersonalAccountAccrualsController>();
            this.Container.RegisterController<PersonalAccountCounterController>();

            this.Container.RegisterAltDataController<RealEstateIndicator>();
            this.Container.RegisterController<CommonParamsController>();
            this.Container.RegisterController<HostController>();
            this.Container.RegisterAltDataController<GisRealEstateTypeCommonParam>();
            this.Container.RegisterAltDataController<GisRealEstateTypeIndicator>();
            this.Container.RegisterAltDataController<GisRealEstateTypeGroup>();
            this.Container.RegisterController<GisRealEstateTypeController>();
            this.Container.RegisterController<ImportDataController>();
            this.Container.RegisterController<RegressionAnalysisController>();
            this.Container.RegisterAltDataController<HouseRegister>();
            this.Container.RegisterController<MultipleAnalysisTemplateController>();
            this.Container.RegisterController<HouseServiceRegisterController>();
            this.Container.RegisterController<CubeController>();
            this.Container.RegisterController<GisAddressMatchingController>();
            this.Container.RegisterAltDataController<SupplierRegister>();
            this.Container.RegisterController<BillingController>();
            this.Container.RegisterController<TenantSubsidyRegisterController>();
            this.Container.RegisterController<ServiceSubsidyRegisterController>();
            this.Container.RegisterController<SkapController>();
            this.Container.RegisterController<RealityObjectExtraController>();
            this.Container.RegisterController<GisAddressController>();
            this.Container.RegisterController<GisHouseRegisterController>();
            this.Container.RegisterAltDataController<GisDataBank>();
            this.Container.RegisterController<ImportDataOtController>();
            this.Container.RegisterController<WasteCollectionPlaceController>();
            this.Container.RegisterController<MenuWasteCollectionController>();
            this.Container.RegisterController<IndicatorServiceComparisonController>();
            this.Container.RegisterAltDataController<ServiceDictionary>();
            this.Container.RegisterController<GisNormativDictController>();
            this.Container.RegisterAltDataController<GisTariffDict>();
            this.Container.RegisterController<BilServiceDictionaryController>();
            this.Container.RegisterAltDataController<BilTarifStorage>();
            this.Container.RegisterAltDataController<BilNormativStorage>();
            this.Container.RegisterController<UnloadCounterValuesController>();
            this.Container.RegisterController<ManagingOrgMkdWorkController>();
            this.Container.RegisterAltDataController<ManOrgBilAdditionService>();
            this.Container.RegisterAltDataController<ManOrgBilCommunalService>();
            this.Container.RegisterAltDataController<ManOrgBilWorkService>();
            this.Container.RegisterAltDataController<ManOrgBilMkdWork>();
            this.Container.RegisterAltDataController<ContractOwnersAddService>();
            this.Container.RegisterAltDataController<ContractOwnersCommService>();
            this.Container.RegisterAltDataController<ContractOwnersWorkService>();
            this.Container.RegisterAltDataController<JskTsjContractAddService>();
            this.Container.RegisterAltDataController<JskTsjContractCommService>();
            this.Container.RegisterAltDataController<JskTsjContractWorkService>();
            this.Container.RegisterAltDataController<TransferContractAddService>();
            this.Container.RegisterAltDataController<TransferContractCommService>();
            this.Container.RegisterAltDataController<TransferContractWorkService>();
            this.Container.RegisterAltDataController<RelationContractAddService>();
            this.Container.RegisterAltDataController<RelationContractCommService>();
            this.Container.RegisterAltDataController<BilConnection>();
        }

        public void RegisterViewModels()
        {
            this.Container.RegisterTransient<IViewModel<GisRealEstateTypeIndicator>, GisRealEstateTypeIndicatorViewModel>();
            this.Container.RegisterViewModel<GisRealEstateType, GisRealEstateTypeViewModel>();
            this.Container.RegisterViewModel<GisRealEstateTypeCommonParam, GisRealEstateTypeCommonParamViewModel>();
            this.Container.RegisterViewModel<MultipleAnalysisTemplate, MultipleAnalysisTemplateViewModel>();
            this.Container.RegisterViewModel<WasteCollectionPlace, WasteCollectionPlaceViewModel>();
            this.Container.RegisterViewModel<IndicatorServiceComparison, IndicatorServiceComparisonViewModel>();
            this.Container.RegisterViewModel<GisNormativDict, GisNormativDictViewModel>();
            this.Container.RegisterViewModel<GisTariffDict, GisTariffDictViewModel>();
            this.Container.RegisterViewModel<BilServiceDictionary, BilServiceDictionaryViewModel>();
            this.Container.RegisterViewModel<BilTarifStorage, BilTarifStorageViewModel>();
            this.Container.RegisterViewModel<BilNormativStorage, BilNormativStorageViewModel>();
            this.Container.RegisterViewModel<GisDataBank, GisDataBankViewModel>();
            this.Container.RegisterViewModel<ManOrgBilAdditionService, ManOrgBilAdditionServiceViewModel>();
            this.Container.RegisterViewModel<ManOrgBilCommunalService, ManOrgBilCommunalServiceViewModel>();
            this.Container.RegisterViewModel<ManOrgBilWorkService, ManOrgBilWorkServiceViewModel>();
            this.Container.RegisterViewModel<ManOrgBilMkdWork, ManOrgBilMkdWorkViewModel>();
            this.Container.RegisterViewModel<ContractOwnersAddService, ContractOwnersAddServiceViewModel>();
            this.Container.RegisterViewModel<ContractOwnersCommService, ContractOwnersCommServiceViewModel>();
            this.Container.RegisterViewModel<ContractOwnersWorkService, ContractOwnersWorkServiceViewModel>();
            this.Container.RegisterViewModel<JskTsjContractAddService, JskTsjContractAddServiceViewModel>();
            this.Container.RegisterViewModel<JskTsjContractCommService, JskTsjContractCommServiceViewModel>();
            this.Container.RegisterViewModel<JskTsjContractWorkService, JskTsjContractWorkServiceViewModel>();
            this.Container.RegisterViewModel<TransferContractAddService, TransferContractAddServiceViewModel>();
            this.Container.RegisterViewModel<TransferContractCommService, TransferContractCommServiceViewModel>();
            this.Container.RegisterViewModel<TransferContractWorkService, TransferContractWorkServiceViewModel>();
            this.Container.RegisterViewModel<RelationContractAddService, RelationContractAddServiceViewModel>();
            this.Container.RegisterViewModel<RelationContractCommService, RelationContractCommServiceViewModel>();
            this.Container.RegisterViewModel<BilConnection, BilConnectionViewModel>();
        }

        public void RegisterDomainServices()
        {
            this.Container.RegisterDomainService<BilServiceDictionary, BilServiceDictionaryDomainService>();
        }

        public void RegisterCommonParams()
        {
            this.Container.RegisterTransient<IGisCommonParam, AreaLivingNotLivingMkdCommonParam>(AreaLivingNotLivingMkdCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, AreaMkdCommonParam>(AreaMkdCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, AreaOwnedCommonParam>(AreaOwnedCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, BuildYearCommonParam>(BuildYearCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, PrivatizationDateFirstApartment>(PrivatizationDateFirstApartment.Code);
            this.Container.RegisterTransient<IGisCommonParam, MaximumFloorsCommonParam>(MaximumFloorsCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, MinimumFloorsCommonParam>(MinimumFloorsCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, NumberLiftsCommonParam>(NumberLiftsCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, TypeHouseCommonParam>(TypeHouseCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, TypeRoofCommonParam>(TypeRoofCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, WallMaterialCommonParam>(WallMaterialCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, NumberApartmentsCommonParam>(NumberApartmentsCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, PhysicalWearCommonParam>(PhysicalWearCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, NumberEntrancesCommonParam>(NumberEntrancesCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, NumberLivingCommonParam>(NumberLivingCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, RoofingMaterialCommonParam>(RoofingMaterialCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, TypeProjectCommonParam>(TypeProjectCommonParam.Code);
            this.Container.RegisterTransient<IGisCommonParam, HeatingSystemCommonParam>(HeatingSystemCommonParam.Code);
        }

        public void RegisterServices()
        {
            this.Container.RegisterTransient<IBillingReportService, BillingReportService>();
            this.Container.RegisterTransient<IAnalysisReportService, AnalysisReportService>();
            this.Container.RegisterTransient<IImportDataService, ImportDataService>();
            this.Container.RegisterTransient<IRealEstateTypeService, RealEstateTypeService>();
            this.Container.RegisterTransient<IRealEstateTypeGroupService, RealEstateTypeGroupService>();
            this.Container.RegisterTransient<IIndicatorService, IndicatorService>();
            this.Container.RegisterTransient<IHouseServiceRegisterService, HouseServiceRegisterService>();
            this.Container.RegisterTransient<IHqlHelper, HqlHelper>();
            this.Container.RegisterTransient<IGisAddressMatchingService, GisAddressMatchingService>();
            this.Container.RegisterTransient<IJExtractorService, JExtractorService>();
            this.Container.RegisterTransient<ICrpCryptoProvider, CrpCryptoProvider>();

            this.Container.RegisterTransient<BaseImportDataHandler, ImportIncremetalDataForPgu>("IncrementalPguHandler2");
            this.Container.RegisterTransient<BaseImportDataHandler, ImportIncremetalDataForGis>("IncrementalGisHandler2");
            this.Container.RegisterTransient<BaseImportDataHandler, ImportSzDataForMinstroyReportsHandler>("SzDataForMinStroyReportHandler2");
            this.Container.RegisterTransient<BaseImportDataHandler, ImportGkhDataForMinstroyReportsHandler>("GkhDataForMinStroyReportHandler2");

            this.Container.RegisterTransient<IGisHouseService, GisHouseService>("GisHouseService");
            this.Container.RegisterTransient<IGisHouseService, KpHouseService>("KpHouseService");

            this.Container.RegisterTransient<IGisPersonalAccountService, GisPersonalAccountService>("GisPersonalAccount");
            this.Container.RegisterTransient<IGisPersonalAccountService, KpPersonalAccountService>("KpPersonalAccount");

            this.Container.RegisterTransient<IRealEstateTypeCommonParamService, RealEstateTypeCommonParamService>();
            this.Container.RegisterTransient<IMultipleAnalysisService, MultipleAnalysisService>();
            this.Container.RegisterTransient<ILoadFromBillingBasesService, LoadFromBillingBasesService>();
            this.Container.RegisterTransient<ITenantSubsidyRegisterService, TenantSubsidyRegisterService>();
            this.Container.RegisterTransient<IServiceSubsidyRegisterService, ServiceSubsidyRegisterService>();
            this.Container.RegisterTransient<IAddressMatcherService, AddressMatcherService>();
            this.Container.RegisterTransient<IAddressService, AddressService>();
            this.Container.RegisterTransient<IImportedAddressService, AddressService>("BillingImportedAddressService");
            this.Container.RegisterTransient<IUicHouseService, UicHouseService>();
            this.Container.RegisterTransient<IRegressionAnalysisService, RegressionAnalysisService>();
            this.Container.RegisterTransient<IHouseClaimsService, HouseClaimsService>();
            this.Container.RegisterTransient<IPublicControlClaimsService, PublicControlClaimsService>();
            this.Container.RegisterTransient<IHouseRegisterService, HouseRegisterService>();
            this.Container.RegisterTransient<IImportDataOtService, ImportDataOtService>();
            this.Container.RegisterTransient<IIndicatorDescriptor,
                IndicatorDescriptorMkdOdpuwithOdngt20Percent>("IndicatorDescriptorMKDODPUwithODNgt20Percent");
            this.Container.RegisterTransient<IIndicatorDescriptor,
                IndicatorDescriptorProblemMkdPercent>("IndicatorDescriptorProblemMkdPercent");
            this.Container.RegisterTransient<IDomainService<WasteCollectionPlace>, FileStorageDomainService<WasteCollectionPlace>>();
            this.Container.RegisterTransient<ICalcVerificationService, CalcVerificationService>();
            this.Container.RegisterTransient<IDomainService<GisNormativDict>, FileStorageDomainService<GisNormativDict>>();
            this.Container.RegisterTransient<IUnloadCounterValuesService, UnloadCounterValuesService>();
            this.Container.RegisterTransient<IManagingOrgMkdWorkService, ManagingOrgMkdWorkService>();
            this.Container.RegisterTransient<IBilServiceDictionaryService, BilServiceDictionaryService>();
            this.Container.RegisterTransient<IBilConnectionService, BilConnectionService>();
            this.Container.RegisterTransient<IEiasTariffImportService, EiasTariffImportService>();
        }

        public void RegisterRabbitMq()
        {
            this.Container.RegisterTransient<ITaskHandler<FileTask>, FileTaskHandler>();
            this.Container.RegisterSingleton<IProducerService, ProducerService>();
            this.Container.RegisterSingleton<IConsumerService, ConsumerService>();

            var appSettings = this.Container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.Gkh.Gis");

            if (appSettings == null || !appSettings[SettingsKeyStore.Enable].To<bool>())
            {
                return;
            }
            var consumerService = this.Container.Resolve<IConsumerService>();

            foreach (var task in (TypeQueueName[])System.Enum.GetValues(typeof(TypeQueueName)))
            {
                consumerService.StartConsuming(task.GetEnumMeta().Display);
            }
        }

        public void RegisterPermissions()
        {
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GisPermissionMap>());
            this.Container.Register(Component.For<IFieldRequirementSource>().ImplementedBy<GisFieldRequirementMap>());
        }

        public void RegisterNavigation()
        {
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, PersonalAccountInfoMenu>();
            this.Container.RegisterTransient<INavigationProvider, WasteCollectionMenuProvider>();
            this.Container.ReplaceComponent<INavigationProvider>(
                typeof(PrintFormService),
                Component.For<INavigationProvider>().ImplementedBy<RealityObjMenuProvider>());
            this.Container.RegisterTransient<INavigationProvider, ManOrgMenuProvider>();
        }

        public void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<IndicatorServiceComparison, IndicatorServiceComparisonInterceptor>();
            this.Container.RegisterDomainInterceptor<ServiceDictionary, ServiceDictionaryInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgBilWorkService, ManOrgBilWorkServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgBilCommunalService, ManOrgBilCommunalServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgBilAdditionService, ManOrgBilAdditionServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Contragent, ContragentGisServiceInterceptor>();
        }

        public void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, WasteCollectionDataExport>("WasteCollectionDataExport");
        }

        private void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<ImportIncrementalDataPermissionsAddAction>();
            this.Container.RegisterExecutionAction<ImportIncrementalDataPermissionsRemoveAction>();
            this.Container.RegisterExecutionAction<ExportDeviceReadingPermissionsAddAction>();
            this.Container.RegisterExecutionAction<ExportDeviceReadingPermissionsRemoveAction>();
            this.Container.RegisterExecutionAction<EiasTariffImportServiceAction>();
        }
    }
}