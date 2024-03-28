namespace Bars.GkhRf
{
    using B4;
    using B4.IoC;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage.DomainService;
    using B4.Modules.Reports;
    using B4.Modules.States;
    using B4.Windsor;
    using Gkh;

    using B4.Modules.Pivot;

    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.GkhRf.SystemDataTransfer;

    using Gkh.Import;
    using Gkh.Report;
    using Controllers;
    using DomainService;
    using DomainService.Impl;
    using Entities;
    using Export;
    using Gkh.Utils;
    using Import;
    using Interceptors;
    using Navigation;
    using Permissions;
    using Report;
    using Services.Impl;
    using Services.ServiceContracts;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>().Named("GkhRF resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IStatefulEntitiesManifest>().Named("GkhRF statefulentity").ImplementedBy<StatefulEntityManifest>()
                    .LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhRfPermissionMap>());

            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<TransferRfRecordReport>().LifeStyle.Transient);

            Container.Register(Component.For<IViewCollection>().Named("GkhRfViewCollection").ImplementedBy<GkhRfViewCollection>().LifeStyle.Transient);

            Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient());

            #region Интерсепторы
            Container.Register(Component.For<IDomainServiceInterceptor<TransferRfRecord>>().ImplementedBy<TransferRfRecordInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<RequestTransferRf>>().ImplementedBy<RequestTransferRfServiceInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<TransferRf>>().ImplementedBy<TransferRfInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<TransferFundsRf>>().ImplementedBy<TransferFundsRfInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<ContractRfObject>>().ImplementedBy<ContractRfObjectInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<LimitCheck>>().ImplementedBy<LimitCheckInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<PaymentItem>>().ImplementedBy<PaymentItemInterceptor>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainServiceInterceptor<Payment>>().ImplementedBy<PaymentInterceptor>().LifeStyle.Transient);
            #endregion

            #region Service

            Container.Register(Component.For<IContractRfObjectService>().ImplementedBy<ContractRfObjectService>().LifeStyle.Transient);
            Container.Register(Component.For<IContractRfService>().ImplementedBy<ContractRfService>().LifeStyle.Transient);
            Container.Register(Component.For<IRealObjForTransferService>().ImplementedBy<RealObjForTransferService>().LifeStyle.Transient);
            Container.Register(Component.For<ITransferFundsRfService>().ImplementedBy<TransferFundsRfService>().LifeStyle.Transient);
            Container.Register(Component.For<ILimitCheckFinSourceService>().ImplementedBy<LimitCheckFinSourceService>().LifeStyle.Transient);
            Container.Register(Component.For<ITransferRfService>().ImplementedBy<TransferRfService>().LifeStyle.Transient);

            #endregion

            #region DomainService
            Container.Register(Component.For<IDomainService<RequestTransferRf>>().ImplementedBy<FileStorageDomainService<RequestTransferRf>>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ContractRf>>().ImplementedBy<ContractRfDomainService>().LifeStyle.Transient);

            Component
                .For<IViewModel<ContractRf>>()
                .ImplementedBy<ContractRfViewModel>()
                .LifestyleTransient()
                .RegisterIn(Container);

            Container.Register(Component.For<IViewModel<ContractRfObject>>().ImplementedBy<ContractRfObjectViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<Payment>>().ImplementedBy<PaymentViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<PaymentItem>>().ImplementedBy<PaymentItemDomainService>().LifeStyle.Transient);

            Component
                .For<IViewModel<PaymentItem>>()
                .ImplementedBy<PaymentItemViewModel>()
                .LifestyleTransient()
                .RegisterIn(Container);

            Container.Register(Component.For<IViewModel<TransferRfRecord>>().ImplementedBy<TransferRfRecordViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<TransferRfRecord>>().ImplementedBy<FileStorageDomainService<TransferRfRecord>>().LifeStyle.Transient);

            Component.For<IViewModel<RequestTransferRf>>()
                .ImplementedBy<RequestTransferRfViewModel>()
                .LifestyleTransient()
                .RegisterIn(Container);

            Container.Register(Component.For<IViewModel<TransferFundsRf>>().ImplementedBy<TransferFundsRfViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<TransferRf>>().ImplementedBy<TransferRfViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<TransferRfRecObj>>().ImplementedBy<TransferRfRecObjViewModel>().LifeStyle.Transient);
            Container.Register(Component.For<IViewModel<LimitCheckFinSource>>().ImplementedBy<LimitCheckFinSourceViewModel>().LifeStyle.Transient);


            Container.Register(Component.For<IDomainService<ViewContractRf>>().ImplementedBy<ViewContractRfDomainService>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ViewPayment>>().ImplementedBy<ViewPaymentRfDomainService>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ViewRequestTransferRf>>().ImplementedBy<ViewRequestTransferRfDomainService>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ViewTransferRf>>().ImplementedBy<ViewTransferRfDomainService>().LifeStyle.Transient);
            Container.Register(Component.For<IDomainService<ViewTransferRfRecord>>().ImplementedBy<BaseDomainService<ViewTransferRfRecord>>().LifeStyle.Transient);



            #endregion

            #region NavigationProvider
            Container.Register(
                Component.For<INavigationProvider>().Named("Payment navigation").ImplementedBy<PaymentMenuProvider>()
                    .LifeStyle.Transient);
            #endregion

            #region Export

            Container.Register(Component.For<IDataExportService>().Named("RequestTransferRfDataExport").ImplementedBy<RequestTransferRfDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("ContractRfDataExport").ImplementedBy<ContractRfDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("ContractRfObjectDataExport").ImplementedBy<ContractRfObjectDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("TransferRfDataExport").ImplementedBy<TransferRfDataExport>().LifeStyle.Transient);
            Container.Register(Component.For<IDataExportService>().Named("PaymentDataExport").ImplementedBy<PaymentDataExport>().LifeStyle.Transient);          

            #endregion

            #region Сущности
            Container.RegisterAltDataController<PaymentItem>();
            Container.RegisterController<TransferRfController>();
            Container.RegisterController<RequestTransferRfController>();
            Container.RegisterAltDataController<TransferRfRecObj>();
            Container.RegisterController<TransferRfRecordController>();
            Container.RegisterController<ContractRfController>();
            Container.RegisterController<ContractRfObjectController>();
            Container.RegisterController<PaymentController>();
            Container.RegisterController<TransferFundsRfController>();
            Container.RegisterController<RealityObjectForTransferController>();
            Container.RegisterController<MenuRegFundController>();
            Container.RegisterAltDataController<LimitCheck>();
            Container.RegisterController<LimitCheckFinSourceController>();
            #endregion

            #region Отчеты
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.GisuRObjectContract").ImplementedBy<GisuRealObjContract>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.CrPaymentInformation").ImplementedBy<CrPaymentInformation>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.ProgramCrOwnersSpending").ImplementedBy<ProgramCrOwnersSpending>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.InformationRepublicProgramCr").ImplementedBy<InformationRepublicProgramCr>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.GisuOrderReport").ImplementedBy<GisuOrderReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.AnalisysProgramCr").ImplementedBy<AnalisysProgramCr>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.ExistHouseInContractAndPayment").ImplementedBy<ExistHouseInContractAndPayment>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.CreatedRealtyObject").ImplementedBy<CreatedRealtyObject>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.ContractsAvailabilityWithGisuReport").ImplementedBy<ContractsAvailabilityWithGisuReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.ListHousesByProgramCr").ImplementedBy<ListHousesByProgramCr>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.CountOfRequestInRf").ImplementedBy<CountOfRequestInRfReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.AllocationFundsToPeopleInCr").ImplementedBy<AllocationFundsToPeopleInCrReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.CalculationsBetweenGisuByManOrg").ImplementedBy<CalculationsBetweenGisuByManOrg>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.PaymentCrMonthByRealObj").ImplementedBy<PaymentCrMonthByRealObj>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.InformationCashReceivedRegionalFund").ImplementedBy<InformationCashReceivedRegionalFund>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.CitizenFundsIncomeExpense").ImplementedBy<CitizenFundsIncomeExpenseReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.InformationRepublicProgramCrPart2").ImplementedBy<InformationRepublicProgramCrPart2>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.CorrectionOfLimits").ImplementedBy<CorrectionOfLimits>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("RF Report.PeopleFundsTransferToGisuInfo").ImplementedBy<PeopleFundsTransferToGisuInfo>().LifeStyle.Transient);

            Container.Register(Component.For<IPrintForm, IPivotModel>().Named("RF Report.JournalAcPayment").ImplementedBy<JournalAcPayment>().LifeStyle.Transient);
            #endregion

            #region Веб-сервисы

            // TODO wcf
            // Component.For<IService>().ImplementedBy<Service>().AsWcfSecurityService().RegisterIn(Container);

            #endregion

            // Импорт
            Container.RegisterImport<ErcImport>();

            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<GisuRequestTransferForm>().LifeStyle.Transient);

            // Регистрация класса для получения информации о зависимостях
            Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.GkhRf dependencies")
                .LifeStyle.Singleton
                .UsingFactoryMethod(() => new ModuleDependencies(Container).Init()));

            RegisterBundlers();

            this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();
        }
    }
}