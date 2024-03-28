namespace Bars.Gkh1468
{
    using B4;
    using B4.IoC;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using B4.Modules.FileStorage.DomainService;
    using B4.Modules.NHibernateChangeLog;
    using B4.Modules.States;
    using B4.Windsor;

    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh1468.SystemDataTransfer;

    using Castle.MicroKernel.Registration;
    using Controllers;
    using Controllers.Passport;
    using DataFiller;
    using DataFiller.Fillers;
    using DataFiller.Service;
    using DataFiller.Service.Impl;
    using Domain.PassportImport.Impl;
    using Domain.PassportImport.Interfaces;
    using DomainService;
    using DomainService.Impl;
    using DomainService.Passport;
    using DomainService.Passport.Impl;
    using Entities;
    using Entities.Passport;
    using ExecutionAction;
    using Export;
    using Gkh.DomainService;
    using Gkh.Entities;
    using Gkh.ExecutionAction;
    using Gkh.Import.Organization;
    using Gkh.ImportExport;
    using Gkh.Utils;
    using Import.Organization;
    using Interceptors;
    using Interceptors.Passport;
    using LogMap.Provider;
    using Navigation;
    using PassportStructExport;
    using PassportStructExport.Impl;
    using Permissions;
    using StateChangeHandlers;
    using ViewModel;
    using Wcf;
    using IRealityObjectService = DomainService.IRealityObjectService;
    using RealityObjectService = DomainService.RealityObjectService;

    /// <summary>
    /// Module
    /// </summary>
    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterImport<DataImport1468>();
            Component.For<IDataProcessorFactory>().ImplementedBy<DataProcessorFactory>().LifestyleSingleton().RegisterIn(this.Container);

            this.Container.RegisterTransient<IEntityExportProvider, EntityExportContainer>();
            Component.For<IStatefulEntitiesManifest>().ImplementedBy<StatefulEntityManifest>().LifeStyle.Transient.RegisterIn(this.Container);

            this.Container.Register(Component.For<IResourceManifest>().Named("Gkh1468 resources").ImplementedBy<ResourceManifest>().LifeStyle.Transient);
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<Gkh1468PermissionMap>());

            this.Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>());

            Component.For<IPassportService>().ImplementedBy<PassportService>().RegisterIn(this.Container);

            // Регистрация классов для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.Gkh1468 dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();

            this.RegisterNavigationProviders();

            this.RegisterControllers();

            this.RegisterDomainServices();

            this.RegisterViewModels();

            this.RegisterInterceptors();

            this.RegisterBundlers();

            this.RegisterDataFillers();

            this.RegisterServices();

            this.RegisterAuditLogMap();

            this.RegisterExecuteActions();

            this.RegisterImports();

            this.RegisterExports();
        }

        /// <summary>
        /// DataFillers
        /// </summary>
        public void RegisterDataFillers()
        {
            Component.For<IDataFillerService>().ImplementedBy<DataFillerService>().LifeStyle.Transient.RegisterIn(this.Container);
            
            Component.For<IDataFiller>().ImplementedBy<BuidYearDataFiller>().Named(BuidYearDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<MaxFloorsDataFiller>().Named(MaxFloorsDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<MinFloorsDataFiller>().Named(MinFloorsDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<NumberEntrancesDataFiller>().Named(NumberEntrancesDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);

            Component.For<IDataFiller>().ImplementedBy<UniqueIdentifierDataFiller>().Named(UniqueIdentifierDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<PostalAddressDataFiller>().Named(PostalAddressDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<InventoryNumberDataFiller>().Named(InventoryNumberDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<CadastralNumberDataFiller>().Named(CadastralNumberDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ProjectTypeAndSeriesDataFiller>().Named(ProjectTypeAndSeriesDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<MailingAddressDataFiller>().Named(MailingAddressDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<JuridicalAddressDataFiller>().Named(JuridicalAddressDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<StartDateManagementDataFiller>().Named(StartDateManagementDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<NumberLivingDataFiller>().Named(NumberLivingDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ContragentNameDataFiller>().Named(ContragentNameDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ContragentOgrnDataFiller>().Named(ContragentOgrnDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ContragentKppDataFiller>().Named(ContragentKppDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<AreaMkdDataFiller>().Named(AreaMkdDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ContragentPhoneDataFiller>().Named(ContragentPhoneDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ContragentWebSiteDataFiller>().Named(ContragentWebSiteDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ContragentEmailDataFiller>().Named(ContragentEmailDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);

            Component.For<IDataFiller>().ImplementedBy<ManOrgInnDataFiller>().Named(ManOrgInnDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ManOrgManagerNameDataFiller>().Named(ManOrgManagerNameDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ManOrgManagerSurnameDataFiller>().Named(ManOrgManagerSurnameDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
            Component.For<IDataFiller>().ImplementedBy<ManOrgManagerPatronymicDataFiller>().Named(ManOrgManagerPatronymicDataFiller.Code).LifeStyle.Transient.RegisterIn(this.Container);
        }

        /// <summary>
        /// NavigationProviders
        /// </summary>
        public void RegisterNavigationProviders()
        {
            // Главное меню
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("Gkh1468 navigation");

            this.Container.RegisterTransient<INavigationProvider, RealityObj1468MenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, PublicServOrgMenuProvider>("PublicServOrg navigation");
        }

        /// <summary>
        /// ViewModels
        /// </summary>
        private void RegisterViewModels()
        {
            Component.For<IViewModel<PublicServiceOrg>>().ImplementedBy<PublicServiceOrgViewModel>().LifestyleTransient().RegisterIn(this.Container);
            Component.For<IViewModel<PublicServiceOrgMunicipality>>().ImplementedBy<PublicServiceOrgMunicipalityViewModel>().LifestyleTransient().RegisterIn(this.Container);
            Component.For<IViewModel<PublicServiceOrgRealtyObject>>().ImplementedBy<PublicServiceOrgRealtyObjectViewModel>().LifestyleTransient().RegisterIn(this.Container);

            this.Container.ReplaceComponent<IViewModel<RealityObject>>(typeof(RealityObjectViewModel), Component.For<IViewModel<RealityObject>>().ImplementedBy<RealityObjectViewModel>());
            this.Container.Register(Component.For<IViewModel<PublicServiceOrgContract>>().ImplementedBy<RealObjPublicServiceOrgViewModel>().LifeStyle.Transient);

            this.Container.RegisterViewModel<HousePassport, HousePassportViewModel>();
            this.Container.RegisterViewModel<HouseProviderPassport, HouseProviderPassportViewModel>();
            this.Container.RegisterViewModel<HouseProviderPassportRow, HouseProviderPassportRowViewModel>();

            this.Container.RegisterViewModel<OkiPassport, OkiPassportViewModel>();
            this.Container.RegisterViewModel<OkiProviderPassport, OkiProviderPassportViewModel>();
            this.Container.RegisterViewModel<OkiProviderPassportRow, OkiProviderPassportRowViewModel>();

            this.Container.RegisterViewModel<PassportStruct, PassportStructViewModel>();
        }

        /// <summary>
        /// Controllers
        /// </summary>
        public void RegisterControllers()
        {
            this.Container.RegisterController<Integration>();

            // Рабочий стол
            this.Container.RegisterController<DesktopMapController>();

            // Справочники
            this.Container.RegisterController<PublicServiceController>();
            this.Container.ReplaceController<FiasController>("fias");
            this.Container.ReplaceController<MunicipalityController>("municipality");

            // Сущности
            this.Container.RegisterController<PublicServiceOrgController>();
            this.Container.RegisterController<PublicServiceOrgMunicipalityController>();
            this.Container.RegisterController<PublicServiceOrgRealtyObjectController>();
            
            this.Container.RegisterController<OkiProviderPassportController>();
            this.Container.RegisterController<HouseProviderPassportController>();
            this.Container.RegisterAltDataController<OkiPassport>();
            this.Container.RegisterController<PassportStructController>();
            this.Container.RegisterController<FileStorageDataController<PublicServiceOrgContract>>();
            this.Container.RegisterController<HousePassportController>();
            
            // Структура
            this.Container.RegisterController<MetaAttributeController>();

            this.Container.RegisterController<BaseProviderPassportRowController<HouseProviderPassportRow>>();
            this.Container.RegisterController<BaseProviderPassportRowController<OkiProviderPassportRow>>();

            this.Container.RegisterController<RealityObject1468Controller>();
            this.Container.RegisterController<PartController>();

            this.Container.RegisterController<DataFillerController>();

            this.Container.RegisterController<HousePassportCombinedController>();
            this.Container.RegisterController<OkiPassportCombinedController>();

            this.Container.RegisterController<Temp1468Controller>();
            this.Container.RegisterController<PublicServiceOrgContractRealObjController>();
        }

        /// <summary>
        /// Interceptors
        /// </summary>
        public void RegisterInterceptors()
        {

            // Сущности
            this.Container.Register(Component.For<IDomainServiceInterceptor<PublicServiceOrg>>().ImplementedBy<PublicServOrgServiceInterceptor>().LifeStyle.Transient);
            Component.For<IDomainServiceInterceptor<PassportStruct>>().ImplementedBy<PassportStructInterceptor>().LifestyleTransient().RegisterIn(this.Container);

            Component.For<IDomainServiceInterceptor<HouseProviderPassport>>().ImplementedBy<HouseProviderPassportInterceptor>().LifestyleTransient().RegisterIn(this.Container);
            Component.For<IDomainServiceInterceptor<OkiProviderPassport>>().ImplementedBy<OkiProviderPassportInterceptor>().LifestyleTransient().RegisterIn(this.Container);


            this.Container.RegisterDomainInterceptor<PublicServiceOrgContract, PublicServiceOrgContractInterceptor>();
        }

        /// <summary>
        /// DomainServices
        /// </summary>
        public void RegisterDomainServices()
        {
            // ДоменСервисы
            // this.Container.Register(Component.For<IDomainService<PublicServiceOrg>>().ImplementedBy<PublicServiceOrgDomainService>().LifeStyle.Transient);

            // Дополнительные реализации контрактов к ДоменСервисам
            this.Container.RegisterTransient<IPublicServiceOrgMunicipalityService, PublicServiceOrgMunicipalityService>();
            this.Container.RegisterTransient<IPublicServiceOrgRealtyObjectService, PublicServiceOrgRealtyObjectService>();
            this.Container.RegisterTransient<IMunicipalityService1468, MunicipalityService1468>();

            this.Container.Register(Component.For<IOkiPassportService>().ImplementedBy<OkiPassportService>().LifestyleTransient());
            this.Container.Register(Component.For<IHousePassportService>().ImplementedBy<HousePassportService>().LifestyleTransient());

            this.Container.Register(Component.For<IRealityObjectService>().ImplementedBy<RealityObjectService>().LifestyleTransient());

            this.Container.Register(Component.For<ISignature<OkiProviderPassport>>().ImplementedBy<OkiPassportProvSignature>().LifestyleTransient());
            this.Container.Register(Component.For<ISignature<HouseProviderPassport>>().ImplementedBy<HousePassportProvSignature>().LifestyleTransient());

            this.Container.Register(Component.For<IMetaAttributeService>().ImplementedBy<MetaAttributeService>().LifestyleTransient());
            this.Container.Register(Component.For<IStructPartService>().ImplementedBy<StructPartService>().LifestyleTransient());

            this.Container.RegisterDomainService<PublicServiceOrgContract, FileStorageDomainService<PublicServiceOrgContract>>();

            this.Container.Register(Component.For<IHousePassportCombinedService>().ImplementedBy<HousePassportCombinedService>().LifestyleTransient());
            this.Container.Register(Component.For<IOkiPassportCombinedService>().ImplementedBy<OkiPassportCombinedService>().LifestyleTransient());
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IContragentListForTypeJurOrg, Gkh1468ContragentListForTypeJurOrg>();

            this.Container.RegisterTransient<IOkiPassportProviderService, OkiPassportProviderService>();
            this.Container.RegisterTransient<IHousePassportProviderService, HousePassportProviderService>();

            this.Container.RegisterTransient<IBaseProviderPassportRowService<HouseProviderPassportRow>, HouseProviderPassportRowService>();
            this.Container.RegisterTransient<IBaseProviderPassportRowService<OkiProviderPassportRow>, OkiProviderPassportRowService>();
            this.Container.RegisterTransient<IStateChangeHandler, PassportStateChangeHandler>();

            this.Container.RegisterTransient<IPassportStructExporter, PassportStructExporter>();

            Component.For(typeof(IPassportFillPercentService<,>))
                     .ImplementedBy(typeof(PassportFillPercentService<,>))
                     .LifeStyle.Transient.RegisterIn(this.Container);
        }

        /// <summary>
        /// AuditLogMap
        /// </summary>
        public void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterExecuteActions()
        {
            //this.Container.RegisterExecutionAction<DeletePassportRowDuplicates>(DeletePassportRowDuplicates.Code);
            this.Container.RegisterExecutionAction<PassportRestoreGroupKeys>();
            this.Container.RegisterExecutionAction<SetParentValueForPassportRows>();
        }

        private void RegisterImports()
        {
            // В данном модуле импорт организаций расширяется для импорта Поставщиков ресурсов
            this.Container.RegisterTransient<IOrganizationImportHelper, PublicServiceOrgImportHelper>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, PublicServiceOrgDataExport>("PublicServiceOrgDataExport");
        }
    }
}