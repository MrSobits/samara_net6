namespace Bars.Gkh.Overhaul
{
    using B4;
    using B4.IoC;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using B4.Modules.FileStorage.DomainService;
    using B4.Modules.NHibernateChangeLog;
    using B4.Modules.Pivot;
    using B4.Modules.Reports;
    using B4.Modules.States;
    using B4.Windsor;

    using Bars.Gkh.DomainService;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities.Dict;
    using Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.Overhaul.SystemDataTransfer;
    using Bars.Gkh.SystemDataTransfer.Meta;

    using CommonParams;
    using Domain;
    using Domain.Impl;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.Dicts;
    using Controllers;
    using DomainService.Impl;
    using Entities;
    using ExecutionAction;
    using Export;
    using Gkh.Report;
    using Gkh.Utils;
    using Interceptors;
    using LogMap.Provider;
    using Navigation;
    using Reports;
    using Services.Impl;
    using Services.ServiceContracts;
    using ViewModel;

    using Castle.MicroKernel.Registration;
    using ConfigSections.Overhaul;

    using IWorkService = Bars.Gkh.Overhaul.DomainService.IWorkService;
    using WorkService = Bars.Gkh.Overhaul.DomainService.Impl.WorkService;

    public partial class Module : AssemblyDefinedModule
    {
        /// <inheritdoc />
        public override void Install()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>();

            this.Container.RegisterPermissionMap<PermissionMap>();
            this.Container.Register(Component.For<IFieldRequirementSource>().ImplementedBy<GkhOverhaulFieldRequirementMap>());

            this.Container.Register(
                Component.For<IModuleDependencies>()
                    .Named(string.Format("{0} dependencies", this.AssemblyName))
                    .LifeStyle.Singleton.UsingFactoryMethod(
                        () => new ModuleDependencies(this.Container).Init()));

            this.RegisterCommonParams();

            this.RegisterDataExports();

            this.RegisterBundlers();

            this.RegisterControllers();

            this.RegisterInterceptors();

            this.RegisterNavigations();

            this.RegisterServices();

            this.RegisterViewModels();

            this.RegisterReports();

            this.RegisterDomainServices();

            this.RegisterAuditLogMap();

            this.RegisterExecutionActions();

            this.RegisterImports();

            // TODO wcf 
            // Component.For<IService>().ImplementedBy<Service>().AsWcfSecurityService().RegisterIn(this.Container);

            this.Container.RegisterGkhConfig<OverhaulConfig>();
            this.Container.RegisterGkhConfig<BasisOverhaulDocConfig>();

            this.RegisterFormatDataExport();

            this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();
        }

        private void RegisterCommonParams()
        {
            this.Container.RegisterTransient<ICommonParam, AreaLivingNotLivingMkdCommonParam>(AreaLivingNotLivingMkdCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, AreaMkdCommonParam>(AreaMkdCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, AreaOwnedCommonParam>(AreaOwnedCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, BuildYearCommonParam>(BuildYearCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, FirstPrivatizationDateCommonParam>(FirstPrivatizationDateCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, MaximumFloorsCommonParam>(MaximumFloorsCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, MinimumFloorsCommonParam>(MinimumFloorsCommonParam.Code);
            this.Container.RegisterTransient<ICommonParam, NumberLiftsCommonParam>(NumberLiftsCommonParam.Code);
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<DpkrController>();

            #region CommonEstateObject
            this.Container.RegisterController<CommonEstateObjectController>();
            this.Container.RegisterController<StructuralElementGroupController>();
            this.Container.RegisterController<StructuralElementGroupAttributeController>();
            this.Container.RegisterController<StructuralElementController>();
            this.Container.RegisterAltDataController<StructuralElementWork>();
            this.Container.RegisterAltDataController<StructuralElementFeatureViol>();

            #endregion

            #region Dict
            this.Container.RegisterController<JobController>();
            this.Container.RegisterController<PaymentSizeMuRecordController>();
            this.Container.RegisterController<WorkPriceController>();
            this.Container.RegisterController<OvrhlWorkController>();

            this.Container.RegisterAltDataController<WorkTypeFinSource>();
            this.Container.RegisterAltDataController<PaymentSizeCr>();
            this.Container.RegisterAltDataController<GroupType>();
            this.Container.RegisterAltDataController<BasisOverhaulDocKind>();

            #endregion

            #region RealityObject
            this.Container.RegisterController<RealityObjectMissingCeoController>();
            this.Container.RegisterController<RealityObjectStructuralElementController>();

            this.Container.RegisterAltDataController<RealityObjectStructuralElementAttributeValue>();

            #endregion

            #region RealEstateType
            this.Container.RegisterAltDataController<Paysize>();
            this.Container.RegisterController<PaysizeRecordController>();
            this.Container.RegisterAltDataController<PaysizeRealEstateType>();

            #endregion RealEstateType

            this.Container.RegisterController<CreditOrgController>();
            this.Container.RegisterController<FileStorageDataController<ContragentBankCreditOrg>>();
            this.Container.ReplaceController<B4.Alt.DataController<OvrhlRealityObjectLift>>("RealityObjectLift");
        }

        private void RegisterInterceptors()
        {
            #region WorkPrice
            this.Container.RegisterDomainInterceptor<WorkPrice, WorkPriceInterceptor>();

            #endregion

            #region CommonEstateObject
            this.Container.RegisterDomainInterceptor<CommonEstateObject, CommonEstateObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<StructuralElementGroup, StructuralElementGroupInterceptor>();
            this.Container.RegisterDomainInterceptor<StructuralElement, StructuralElementInterceptor>();
            this.Container.RegisterDomainInterceptor<PaymentSizeMuRecord, PaymentSizeMuRecordInterceptor>();

            #endregion

            #region RealityObject
            this.Container.RegisterDomainInterceptor<RealityObjectStructuralElement, RealityObjectStructuralElementInterceptor>();

            #endregion

            this.Container.RegisterDomainInterceptor<PaymentSizeCr, PaymentSizeCrInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<CreditOrg>, CreditOrgInterceptor>();

            this.Container.RegisterDomainInterceptor<Paysize, PaysizeInterceptor>();
            this.Container.RegisterDomainInterceptor<PaysizeRecord, PaysizeRecordInterceptor>();

            this.Container.RegisterDomainInterceptor<Municipality, MunicipalityInterceptor>();
            this.Container.RegisterDomainInterceptor<Job, JobInterceptor>();
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterNavigationProvider<NavigationProvider>();
            this.Container.RegisterNavigationProvider<RealityObjMenuProvider>();
            this.Container.RegisterNavigationProvider<PaysizeNavigationProvider>();
            this.Container.RegisterNavigationProvider<ContragentMenuProvider>();
        }

        private void RegisterServices()
        {
            #region CommonEstateObject
            this.Container.RegisterTransient<IListCeoService, ListCeoService>();
            this.Container.RegisterTransient<IStructuralElementGroupAttributeService, StructuralElementGroupAttributeService>();
            this.Container.RegisterService<IStructuralElementService, StructuralElementService>();
            this.Container.RegisterTransient<ICommonEstateObjectService, CommonEstateObjectService>();

            #endregion

            #region Dict
            this.Container.RegisterTransient<IJobService, JobService>();
            this.Container.RegisterTransient<IWorkService, WorkService>();
            this.Container.RegisterTransient<IWorkPriceService<WorkPrice>, WorkPriceService>();
            this.Container.RegisterTransient<IPaymentSizeMuRecordService, PaymentSizeMuRecordService>();

            #endregion Dict

            #region RealityObject
            this.Container.RegisterTransient<IRealityObjectMissingCeoService, RealityObjectMissingCeoService>();

            #endregion

            this.Container.RegisterTransient<ICreditOrgService, CreditOrgService>();

            this.Container.RegisterTransient<IRealityObjectStructElService, RealityObjectStructElService>();
            this.Container.RegisterTransient<IRealityObjectStructuralElementService, RealityObjectStructuralElementService>();

            this.Container.RegisterTransient<IPaysizeRecordService, PaysizeRecordService>();

            this.Container.RegisterTransient<IPaysizeRepository, PaysizeRepository>();

            this.Container.RegisterTransient<Gkh.DomainService.IInheritEntityChangeLog, ContragentBankCreditOrgChangeLog>(ContragentBankCreditOrgChangeLog.Id);
        }

        private void RegisterViewModels()
        {
            #region CommonEstateObject
            this.Container.RegisterViewModel<CommonEstateObject, CommonEstateObjectViewModel>();
            this.Container.RegisterViewModel<StructuralElementGroup, StructuralElementGroupViewModel>();
            this.Container.RegisterViewModel<StructuralElementGroupAttribute, StructuralElementGroupAttributeViewModel>();
            this.Container.RegisterViewModel<StructuralElement, StructuralElementViewModel>();
            this.Container.RegisterViewModel<StructuralElementWork, StructuralElementWorkViewModel>();
            this.Container.RegisterViewModel<StructuralElementFeatureViol, StructuralElementFeatureViolViewModel>();

            #endregion

            #region Dict
            this.Container.RegisterViewModel<Job, JobViewModel>();
            this.Container.RegisterViewModel<PaymentSizeCr, PaymentSizeCrViewModel>();
            this.Container.RegisterViewModel<PaymentSizeMuRecord, PaymentSizeMuRecordViewModel>();
            this.Container.RegisterViewModel<WorkPrice, WorkPriceViewModel>();
            this.Container.RegisterViewModel<WorkTypeFinSource, WorkTypeFinSourceViewModel>();

            this.Container.ReplaceComponent(Component.For<IViewModel<Work>>().ImplementedBy<WorkViewModel>().LifestyleTransient());
            this.Container.ReplaceComponent(Component.For<IViewModel<AdditWork>>().ImplementedBy<AdditWorkViewModel>().LifestyleTransient());
            #endregion

            #region RealityObject
            this.Container.RegisterViewModel<RealityObjectMissingCeo, RealityObjectMissingCeoViewModel>();
            this.Container.RegisterViewModel<RealityObjectStructuralElement, RealityObjectStructuralElementViewModel>();
            this.Container.RegisterViewModel<OvrhlRealityObjectLift, RealityObjectLiftViewModel>();
            #endregion

            this.Container.RegisterViewModel<PaysizeRealEstateType, PaysizeRealEstateTypeViewModel>();

            
            this.Container.RegisterViewModel<CreditOrg, CreditOrgViewModel>();
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IPrintForm, RealtyObjectDataReport>("RealtyObjectPassport");
            this.Container.RegisterTransient<IPrintForm, LackOfRequiredStructEls>("LackOfRequiredStructEls");
            this.Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<StructElementList>().Named("StructElementList").LifestyleTransient());
            this.Container.Register(Component.For<IPrintForm, IPivotModel>()
                .Named("Report.PublishedProgramReport")
                .ImplementedBy<PublishedProgramReport>()
                .LifeStyle.Transient);
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterTransient<IDomainService<WorkPrice>, WorkPriceDomainService>();
            this.Container.RegisterDomainService<RealityObjectStructuralElement, RealityObjectStructuralElementDomainService>();
            this.Container.RegisterDomainService<ContragentBankCreditOrg, FileStorageDomainService<ContragentBankCreditOrg>>();
        }

        private void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<WorkPriceCopyByMoLevelAction>();
            this.Container.RegisterExecutionAction<StructuralElementAddAction>();
            this.Container.RegisterExecutionAction<RemovePaysizeMuDuplicatesAction>();
        }
        private void RegisterDataExports()
        {
            this.Container.RegisterTransient<IDataExportService, WorkPriceDataExport>("WorkPriceDataExport");
            this.Container.RegisterTransient<IDataExportService, CreditOrgExport>("CreditOrgExport");
        }

        private void RegisterImports()
        {
            this.Container.RegisterImport<Import.CommonRealtyObjectImport.RoImport>();
        }

        private void RegisterFormatDataExport()
        {
            ContainerHelper.RegisterExportableEntity<ActWorkDogovExportableEntity>();
            ContainerHelper.RegisterExportableEntity<ActWorkDogovFilesExportableEntity>();
            ContainerHelper.RegisterExportableEntity<ActWorkExportableEntity>();
            ContainerHelper.RegisterExportableEntity<BankExportableEntity>();
            ContainerHelper.RegisterExportableEntity<ContragentRschetExportableEntity>();
            ContainerHelper.RegisterExportableEntity<DogovorPkrExportableEntity>();
            ContainerHelper.RegisterExportableEntity<DogovorPkrFilesExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PayDogovExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PayDogovWorkExportableEntity>();
            ContainerHelper.RegisterExportableEntity<WorkKprTypeExportableEntity>();
            ContainerHelper.RegisterExportableEntity<WorkDogovExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PkrDocExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PkrDocFilesExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PkrDomExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PkrDomWorkExportableEntity>();
            ContainerHelper.RegisterExportableEntity<PkrExportableEntity>();

            ContainerHelper.RegisterProxySelectorService<ActWorkDogovProxy, ActWorkDogovSelectorService>();
            ContainerHelper.RegisterProxySelectorService<DogovorPkrProxy, DogovorPkrSelectorService>();
            ContainerHelper.RegisterProxySelectorService<WorkDogovProxy, WorkDogovSelectorService>();
            ContainerHelper.RegisterProxySelectorService<WorkKprTypeProxy, WorkKprTypeSelectorService>();
            ContainerHelper.RegisterProxySelectorService<ContragentProxy, ContragentSelectorService>();
            ContainerHelper.RegisterProxySelectorService<BankProxy, BankSelectorService>();
        }
    }
}