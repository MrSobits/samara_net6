namespace Bars.Gkh.RegOperator
{
    #region usings
    using System.Text;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.ViewModels.Pretension;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Params;
    using Bars.B4.Modules.Analytics.Reports.Web.DomainService;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.B4.Windsor;
    using Bars.Gkh.ClaimWork.Controllers;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.ClaimWork.Interceptors;
    using Bars.Gkh.ClaimWork.ViewModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Domain.EntityHistory;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.FormatDataExport.Domain.Impl;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.Import;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Controllers;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Export;
    using Bars.Gkh.Modules.ClaimWork.Interceptors;
    using Bars.Gkh.Modules.Reforma;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.AccountNumberGenerator;
    using Bars.Gkh.RegOperator.AccountNumberGenerator.Impl;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.Config.Validators;
    using Bars.Gkh.RegOperator.Controllers;
    using Bars.Gkh.RegOperator.Controllers.Config;
    using Bars.Gkh.RegOperator.Controllers.Distribution;
    using Bars.Gkh.RegOperator.Controllers.Import;
    using Bars.Gkh.RegOperator.Controllers.Period;
    using Bars.Gkh.RegOperator.Controllers.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.EntityHistory;
    using Bars.Gkh.RegOperator.Domain.Impl;
    using Bars.Gkh.RegOperator.Domain.ImportExport;
    using Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export;
    using Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export.Impl;
    using Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Serializers;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl;
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.CashPaymentCenter;
    using Bars.Gkh.RegOperator.DomainService.Impl;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.DomainService.Interface;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor.Impl;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Assembly;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.DomainService.PersonalOperationAccount;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectDecisionProtocolService.Impl;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectProtocol;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectProtocol.Impl;
    using Bars.Gkh.RegOperator.DomainService.RegoperatorParams;
    using Bars.Gkh.RegOperator.DomainService.RegOperator;
    using Bars.Gkh.RegOperator.DomainService.TransferFunds.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Decisions;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Views;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.ExecutionAction;
    using Bars.Gkh.RegOperator.Export;
    using Bars.Gkh.RegOperator.Export.ExportToEbir;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.RegOperator.FormatDataExport.Domain.Impl;
    using Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.RegOperator.GeneralState;
    using Bars.Gkh.RegOperator.Imports;
    using Bars.Gkh.RegOperator.Imports.Account;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.RegOperator.Imports.DebtorClaimWork;
    using Bars.Gkh.RegOperator.Imports.DecisionProtocol;
    using Bars.Gkh.RegOperator.Imports.FsGorod;
    using Bars.Gkh.RegOperator.Imports.ImportRkc;
    using Bars.Gkh.RegOperator.Imports.OwnerRoom;
    using Bars.Gkh.RegOperator.Imports.Room;
    using Bars.Gkh.RegOperator.Imports.SocialSupport;
    using Bars.Gkh.RegOperator.Imports.SocialSupport.Impl;
    using Bars.Gkh.RegOperator.Interceptors;
    using Bars.Gkh.RegOperator.Interceptors.Decision;
    using Bars.Gkh.RegOperator.Interceptors.FsGorod;
    using Bars.Gkh.RegOperator.Interceptors.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.LogMap;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Controllers;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Impl;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Export;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Repository;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Repository.Impl;
    using Bars.Gkh.RegOperator.Modules.Reforma;
    using Bars.Gkh.RegOperator.Modules.ViewModel;
    using Bars.Gkh.RegOperator.Navigation;
    using Bars.Gkh.RegOperator.Permissions;
    using Bars.Gkh.RegOperator.Quartz;
    using Bars.Gkh.RegOperator.Report;
    using Bars.Gkh.RegOperator.Report.PaymentDocument;
    using Bars.Gkh.RegOperator.Report.PersonalAccountChargeReports;
    using Bars.Gkh.RegOperator.Services.Impl;
    using Bars.Gkh.RegOperator.Services.ServiceContracts;
    using Bars.Gkh.RegOperator.StateChange;
    using Bars.Gkh.RegOperator.Tasks.Charges.Callbacks;
    using Bars.Gkh.RegOperator.Tasks.Charges.Executors;
    using Bars.Gkh.RegOperator.Tasks.Debtors;
    using Bars.Gkh.RegOperator.Tasks.Loans;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments.Snapshoted;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2;
    using Bars.Gkh.RegOperator.Tasks.Period.Callbacks;
    using Bars.Gkh.RegOperator.Tasks.Period.Executors;
    using Bars.Gkh.RegOperator.Tasks.UnacceptedPayment;
    using Bars.Gkh.RegOperator.ViewManagerViews;
    using Bars.Gkh.RegOperator.ViewModel;
    using Bars.Gkh.RegOperator.ViewModels;
    using Bars.Gkh.RegOperator.ViewModels.Decisions;
    using Bars.Gkh.RegOperator.ViewModels.Dict;
    using Bars.Gkh.RegOperator.ViewModels.FsGorod;
    using Bars.Gkh.RegOperator.ViewModels.Owner;
    using Bars.Gkh.RegOperator.ViewModels.PersonalAccount;
    using Bars.Gkh.RegOperator.ViewModels.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.ViewModels.UnacceptedCharge;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;
    using Bars.Gkh.RegOperator.Wcf.Services;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.DomainService;
    using Bars.GkhRf.Entities;

    using Castle.MicroKernel.Registration;
    using DataProviders;
    using DomainService.BankDocumentImport;
    using DomainService.BankDocumentImport.Impl;
    using NHibernate;
    using NHibernate.Event;
    using Tasks.BankDocumentImport;
    using Tasks.CorrectTransfers.OrigName;
    using ViewModels.Impl;
    using BankDocumentImport = Bars.Gkh.RegOperator.Entities.BankDocumentImport;
    using CitizenSuggestionService = Bars.Gkh.DomainService.CitizenSuggestionService;
    using ContragentBankViewModel = Bars.Gkh.ViewModel.ContragentBankViewModel;
    using GovDecisionViewModel = Bars.Gkh.Decisions.Nso.ViewModel.GovDecisionViewModel;
    using IPersonalAccountService = Bars.Gkh.RegOperator.Wcf.Services.IPersonalAccountService;
    using RealEstateTypeInterceptor = Bars.Gkh.Interceptors.RealEstateType.RealEstateTypeInterceptor;
    using RealityObjectService = Bars.Gkh.DomainService.RealityObjectService;
    using RoomViewModel = Bars.Gkh.ViewModel.RealityObject.RoomViewModel;
    using SuspenseAccount = Bars.Gkh.RegOperator.Entities.SuspenseAccount;
    using SuspenseAccountImport = Bars.Gkh.RegOperator.Imports.SuspenseAccount;
    using Gkh.Domain.PaymentDocumentNumber;
    using Domain.PaymentDocumentNumber.Impl;
    using Bars.Gkh.RegOperator.Imports.VTB24;
    using Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin;
    using Bars.Gkh.RegOperator.Interceptors.Contragent;
    using Bars.Gkh.RegOperator.Interceptors.Debtor;
    using Bars.Gkh.RegOperator.Tasks.Rounding;

    using Bars.Gkh.RegOperator.Interceptors.Period;
    using Bars.Gkh.RegOperator.Interceptors.PersonalAccount;
    using Bars.Gkh.RegOperator.NSO.ExecutionAction;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;
    using Bars.Gkh.RegOperator.Report.ChesImport;
    using Bars.Gkh.RegOperator.SystemDataTransfer;
    using Bars.Gkh.RegOperator.Tasks.Distribution;
    using Bars.Gkh.RegOperator.Utils;
    using Bars.Gkh.RegOperator.ViewModels.Import;
    using Bars.Gkh.RegOperator.ViewModels.Period;
    using Bars.Gkh.SystemDataTransfer.Meta;

    using Controllers.PersonalAccount;
    using DomainService.PersonalAccountPrivilegedCategory;
    using RealityObjectDecisionProtocolService = Bars.Gkh.Decisions.Nso.Domain.Impl.RealityObjectDecisionProtocolService;
    using Dto;
    using Controllers.Owner;

    using ChesImport = Bars.Gkh.RegOperator.Imports.Ches.ChesImport;
    using ChesImportReport = Bars.Gkh.RegOperator.Report.ChesImportReport;
    using RealityObjectFieldsServiceFromGkh = Bars.Gkh.DomainService.Impl.RealityObjectFieldsService;
    using Tasks.PaymentDocuments;

    using ContragentRschetSelectorService = Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl.ContragentRschetSelectorService;
    #endregion

    /// <summary>
    /// Класс модуля
    /// </summary>
    public partial class Module : AssemblyDefinedModule
    {
        /// <inheritdoc />
        public override void Install()
        {
            this.Container.RegisterGkhConfig<DebtorClaimWorkConfig>();

            #region tmp
            this.Container.RegisterController<TestController>();

            this.Container.RegisterController<BankDocumentResolverController>();
            this.Container.RegisterTransient<BankDocumentResolver, BankDocumentResolver>();

            this.Container.RegisterTransient<StringBuilder, StringBuilder>();
           
            #endregion tmp

            this.Container.RegisterTransient<IInterceptor, NhInterceptor>();
            this.Container.RegisterSessionScoped<LogsHolder, LogsHolder>();

            this.SetPredecessor<B4.Modules.NH.Module>();

            this.Container.RegisterSingleton<IDatabaseMutexManager, DatabaseMutexManager>();

            this.Container.RegisterTransient<INhibernateConfigModifier, NhConfigModifier>();

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, RegOperatorMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, DeliveryAgentMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, CashPaymentCenterMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, ObjectCrMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, SpecialObjectCrMenuProvider>();
            
            this.Container.RegisterTransient<INavigationProvider, LegalClaimWorkMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, IndividualClaimWorkMenuProvider>();

            this.Container.RegisterTransient<INavigationProvider, ChesImportMenuProvider>();

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.RegOperator resources");

            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>();
            this.Container.RegisterTransient<IGeneralStatefulEntityManifest, GeneralStatefulEntityManifest>("Gkh.RegOperator generalstate");
            this.Container.RegisterTransient<IFieldRequirementSource, RegOperatorFieldRequirementMap>();
            this.Container.RegisterPermissionMap<RegOperatorPermissionMap>();
            this.Container.RegisterTransient<IRealityObjectCollectionService, RealityObjectCollectionService>();

            this.Container.RegisterTransient<IChargePeriodCloseService, ChargePeriodCloseService>();

            this.Container.ReplaceComponent<IViewModel<ContragentBank>, ContragentBankViewModel>(Component.For<IViewModel<ContragentBank>>().ImplementedBy<ViewModel.ContragentBankViewModel>());

            this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();

            this.RegisterInterceptors();

            this.RegisterControllers();

            this.RegisterViewModels();

            this.RegisterEntityServices();

            this.RegisterVersionedEntities();

            this.RegisterImports();

            this.RegisterDomainServices();

            this.RegisterEntityHistoryServices();

            this.RegisterReports();

            this.RegisterExecutionActions();

            this.RegisterEventHandlers();

            this.RegisterAuditLogMap();

            this.RegisterReplacements();

            this.RegisterExports();

            this.RegisterStateRoutines();

            this.RegisterCodedReports();

            this.RegisterAccountNumberGenerators();

            this.RegisterPersonalAccountOperations();

            this.RegisterCatalogs();

            this.RegisterSources();

            this.Container.RegisterTransient<IPersonalAccountService, NullPersonalAccountService>();
            this.Container.RegisterTransient<IClientBankService, ClientBankService>();

            this.Container.RegisterTransient<IRuleChangeStatus, RealityObjectLoanStatusRuleChangeStatus>();
            this.Container.RegisterTransient<IRuleChangeStatus, PerfWorkActStateChangeRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, TransferCtrStatetransferRule>();

            this.Container.RegisterTransient<IUnacceptedPaymentService, UnacceptedPaymentService>();
            this.Container.RegisterSessionScoped<ICancellableSourceProvider, IBankDocumentImportService, BankDocumentImportService>();

            this.Container.RegisterSessionScoped<ITariffCache, TariffCache>();
            this.Container.RegisterSessionScoped<IEntityLogCache, EntityLogCache>();
            this.Container.RegisterSessionScoped<ICalculationGlobalCache, CalculationGlobalCache>();
            this.Container.RegisterSessionScoped<ICalculatedParameterCache, CalculatedParameterCache>();
            this.Container.RegisterSessionScoped<ITariffAreaCache, TariffAreaCache>();

            // TODO: WCF
           // Component.For<IService>().ImplementedBy<Service>().AsWcfSecurityService().RegisterIn(this.Container);

            // Регистрация класса для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>().Named("Bars.GkhRegoperator dependencies").LifeStyle.Singleton.UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

            //ApplicationContext.Current.Events.GetEvent<AppStartEvent>().Subscribe<DistributionNameGlossaryInitializer>();

            this.RegisterDDD();

            this.Container.RegisterTransient<IViewCollection, RegopViewCollection>("RegopViewCollection");

            this.Container.RegisterTransient<IGroupManager, FormedInOpenPeriodSystemGroupManager>("FormedInOpenPeriodSystemGroup");

            #region ПИР
            this.Container.RegisterController<BaseClaimWorkController<LegalClaimWork>>();
            this.Container.RegisterController<BaseClaimWorkController<IndividualClaimWork>>();

            this.Container.RegisterController<RestructDebtScheduleController>();
            this.Container.RegisterDomainService<DebtorClaimWork, FileStorageDomainService<DebtorClaimWork>>();
            
            this.Container.RegisterTransient<IBaseClaimWorkExport<IndividualClaimWork>, BaseClaimWorkExport<IndividualClaimWork>>();
            this.Container.RegisterTransient<IBaseClaimWorkExport<LegalClaimWork>, BaseClaimWorkExport<LegalClaimWork>>();

            this.Container.RegisterTransient<IClaimWorkPermission, DebtorClaimWorkPermission>();
            this.Container.RegisterTransient<IClaimWorkNavigation, RegOpClaimWorkNavigation>();
            this.Container.RegisterTransient<IClaimWorkService, RegopClaimWorkService>();
            this.Container.RegisterTransient<IDebtorClaimworkRepository, DebtorClaimworkRepository>();
            this.Container.RegisterSessionScoped<IDebtorStateCache, DebtorStateCache>();
            this.Container.RegisterSessionScoped<IClwStateProvider, DebtorClwStateProvider>();
            this.Container.Register(
                Component.For<IBaseClaimWorkService>()
                    .Forward<IBaseClaimWorkService<LegalClaimWork>>()
                    .ImplementedBy<LegalClaimWorkService>()
                    .DependsOn(Dependency.OnComponent<IClwStateProvider, DebtorClwStateProvider>())
                    .LifeStyle.Scoped());
            this.Container.Register(
                Component.For<IBaseClaimWorkService>()
                    .Forward<IBaseClaimWorkService<IndividualClaimWork>>()
                    .ImplementedBy<IndividualClaimWorkService>()
                    .DependsOn(Dependency.OnComponent<IClwStateProvider, DebtorClwStateProvider>())
                    .LifeStyle.Scoped());
            this.Container.Register(
                Component.For<IBaseClaimWorkService>()
                    .Forward<IBaseClaimWorkService<DebtorClaimWork>>()
                    .ImplementedBy<DebtorClaimWorkService>()
                    .DependsOn(Dependency.OnComponent<IClwStateProvider, DebtorClwStateProvider>())
                    .LifeStyle.Scoped());

            this.Container.RegisterTransient<IClaimWorkInfoService, DebtorClaimWorkInfoService>();

            #endregion

            #region Реформа
            this.Container.RegisterTransient<IRealityObjectBothProtocolProvider, RealityObjectBothProtocolProvider>();

            #endregion

            this.RegisterTasks();

            this.RegisterConfigs();

            this.ExecuteAutoMapperConfiguration();

            this.RegistrationQuartz();

            this.RegisterFormatDataExport();
        }

        private void ExecuteAutoMapperConfiguration()
        {
            this.Container.RegisterSingleton<IAutoMapperConfigProvider, Bars.Gkh.RegOperator.Utils.AutoMapperConfigProvider>();
        }

        private void RegisterConfigs()
        {
            this.Container.RegisterTransient<IConfigValueValidator, AccountNumberTypeValidator>(AccountNumberTypeValidator.Id);

            this.Container.Register(
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleInn>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleMonth>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleYear>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleNumber>().LifestyleSingleton());
        }

        private void RegisterTasks()
        {
            #region tasks
            this.Container.RegisterTaskExecutor<PersonalAccountChargeExecutor>(PersonalAccountChargeExecutor.Id);
            this.Container.RegisterTaskExecutor<UnacceptedPaymentTaskExecutor>(UnacceptedPaymentTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<BankDocumentImportTaskExecutor>(BankDocumentImportTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<BankDocumentImportSelectedTaskExecutor>(BankDocumentImportSelectedTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<BankDocumentImportCancelSelectedTaskExecutor>(BankDocumentImportCancelSelectedTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<BankDocumentImportTaskCancelExecutor>(BankDocumentImportTaskCancelExecutor.Id);
            this.Container.RegisterTaskExecutor<BankDocumentImportTaskCheckExecutor>(BankDocumentImportTaskCheckExecutor.Id);
            this.Container.RegisterTaskExecutor<ChargesExportTaskExecutor>(ChargesExportTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<AcceptedChargesTaskExecutor>(AcceptedChargesTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<DebtorsTaskExecutor>(DebtorsTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<DebtorsStateTaskExecutor>(DebtorsStateTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<DebtorClaimWorkTaskExecutor>(DebtorClaimWorkTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<CreateDocumentsTaskExecutor>(CreateDocumentsTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<LoanTakerTaskExecutor>(LoanTakerTaskExecutor.Id);

            this.Container.RegisterTaskExecutor<PeriodCloseTaskExecutor_Step1>(PeriodCloseTaskExecutor_Step1.Id);
            this.Container.RegisterTaskExecutor<PeriodCloseTaskExecutor_Step2>(PeriodCloseTaskExecutor_Step2.Id);
            this.Container.RegisterTaskExecutor<PeriodCloseTaskExecutor_Step3>(PeriodCloseTaskExecutor_Step3.Id);

            this.Container.RegisterTaskExecutor<PhysicalOwnerDocumentSnapshotExecutor>(PhysicalOwnerDocumentSnapshotExecutor.Id);
            this.Container.RegisterTaskExecutor<LegalOwnerDocumentSnapshotExecutor>(LegalOwnerDocumentSnapshotExecutor.Id);

            this.Container.RegisterTaskExecutor<PhysicalOwnerDocumentExecutor>(PhysicalOwnerDocumentExecutor.Id);
            this.Container.RegisterTaskExecutor<LegalOwnerDocumentExecutor>(LegalOwnerDocumentExecutor.Id);
            this.Container.RegisterTaskExecutor<LegalOneHouseOwnerDocumentExecutor>(LegalOneHouseOwnerDocumentExecutor.Id);
            this.Container.RegisterTaskExecutor<LegalPartiallyOwnerDocumentExecutor>(LegalPartiallyOwnerDocumentExecutor.Id);

            this.Container.RegisterTaskExecutor<AccountSnapshotDocExecutor>(AccountSnapshotDocExecutor.Id);
            this.Container.RegisterTaskExecutor<OwnerSnapshotDocExecutor>(OwnerSnapshotDocExecutor.Id);

            this.Container.RegisterTaskExecutor<CorrectPaymentOriginatorNameTaskExecutor>(CorrectPaymentOriginatorNameTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<CorrectPwaOriginatorNameTaskExecutor>(CorrectPwaOriginatorNameTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<CorrectTransferCtrOriginatorNameTaksExecutor>(CorrectTransferCtrOriginatorNameTaksExecutor.Id);
            this.Container.RegisterTaskExecutor<RoundingValuesTasks>(RoundingValuesTasks.Id);
            this.Container.RegisterTaskExecutor<PeriodCloseCheckTaskExecutor>(PeriodCloseCheckTaskExecutor.Id);

            this.Container.RegisterTaskExecutor<DistributionApplyTaskExecutor>(DistributionApplyTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<DistributionUndoTaskExecutor>(DistributionUndoTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<DistributionUndoPartiallyTaskExecutor>(DistributionUndoPartiallyTaskExecutor.Id);

            this.Container.RegisterTaskExecutor<PaymentDocumentEmailSendTaskExecutor>(PaymentDocumentEmailSendTaskExecutor.Id);

            this.Container.RegisterTaskExecutor<MassDebtStartDateCalculateTaskExecutor>(MassDebtStartDateCalculateTaskExecutor.Id);

            #endregion

            #region callbacks
            this.Container.RegisterTaskCallback<PeriodCloseSuccessCallback_Step1>(PeriodCloseSuccessCallback_Step1.Id);
            this.Container.RegisterTaskCallback<PeriodCloseSuccessCallback_Step2>(PeriodCloseSuccessCallback_Step2.Id);
            this.Container.RegisterTaskCallback<PeriodCloseFailCallback>(PeriodCloseFailCallback.Id);
            this.Container.RegisterTaskCallback<LogFileMergeCallback>(LogFileMergeCallback.Id);
            this.Container.RegisterTaskCallback<PersonalAccountChargeSuccessCallback>(PersonalAccountChargeSuccessCallback.Id);
            this.Container.RegisterTaskCallback<PersonalAccountChargeFailCallback>(PersonalAccountChargeFailCallback.Id);

            this.Container.RegisterTaskCallback<PeriodCloseCheckSuccessCallback>(PeriodCloseCheckSuccessCallback.Id);

            this.Container.RegisterTaskCallback<BankDocumentImportAcceptFailureCallback>(BankDocumentImportAcceptFailureCallback.Id);

            this.Container.RegisterTaskCallback<DebtorTaskSuccessCallback>(DebtorTaskSuccessCallback.Id);
            this.Container.RegisterTaskCallback<DebtorClaimWorkTaskSuccessCallback>(DebtorClaimWorkTaskSuccessCallback.Id);

            #endregion
        }

        private void RegisterCodedReports()
        {
            this.Container.RegisterTransient<ICodedReport, ChesImportReport>();
            this.Container.RegisterTransient<ICodedReport, BaseInvoiceReport>();
            this.Container.RegisterTransient<ICodedReport, InvoiceAndActReport>();
            this.Container.RegisterTransient<ICodedReport, InvoiceRegistryAndActReport>();
            this.Container.RegisterTransient<ICodedReport, DecisionContractReport>();
            this.Container.RegisterTransient<ICodedReport, LoanDisposal>();
            this.Container.Register(
                Component.For<ICodedReport, IPersonalAccountCodedReport>()
                .ImplementedBy<PersonalAccountReport>()
                .LifestyleTransient()
                .Named("PersonalAccountReport"));
            this.Container.Register(
                Component.For<ICodedReport, IPersonalAccountCodedReport>()
                    .ImplementedBy<PersonalAccountClaimworkReport>()
                    .LifestyleTransient()
                    .Named("PersonalAccountClaimworkReport"));
            this.Container.RegisterTransient<IDataProvider, AccInfoByLocalityDataProvider>();
            this.Container.RegisterTransient<IPrintForm, FundFormationStimulReport>();

            this.Container.RegisterTransient<IDataProvider, PaymentDocSnapshotDataProvider>();
            this.Container.RegisterTransient<IDataProvider, AccountTransferCtrPaymentDataProvider>();
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnerReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnerDeclarationReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnerDeclaration185Report>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnerDeclaration512Report>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<CourtOrderAccountDeclaration185Report>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<CourtOrderAccountDeclaration512Report>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitAccountReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<AccountPretensionReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<AccountNotificationReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnersPetition>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnerClaimStatementReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitOwnerApplicationIssuanceCourtOrderReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitDeclarationAccountReport>().LifestyleTransient());

            this.Container.RegisterTransient<ICodedReport, CalcDebtExportReport>();
            this.Container.RegisterTransient<ICodedReport, CalcDebtReport>();
        }

        private void RegisterStateRoutines()
        {
            this.Container.RegisterTransient<IStateChangeHandler, DecisionProtocolStateChangeHandler>();
        }

        private void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterEventHandlers()
        {
            this.Container.RegisterTransient<IUnacceptedChargesExportService, UnacceptedChargesExportService>();
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterTransient<IChesImportService, ChesImportService>();
            this.Container.RegisterTransient<IChesComparingService, ChesComparingService>();
            this.Container.RegisterTransient<IChesAccountOwnerComparingService, ChesAccountOwnerComparingService>();
            this.Container.RegisterTransient<IChargeReportData, ChargeReportData>();
            this.Container.RegisterTransient<ITransitAccountService, TransitAccountService>();
            this.Container.RegisterTransient<IContragentListForTypeJurOrg, RegOpContragentListForTypeJurOrg>();
            this.Container.RegisterFileStorageDomainService<BasePersonalAccount>();
            this.Container.RegisterTransient<IDecisionService, DecisionService>();
            this.Container.RegisterTransient<IDebtorService, DebtorService>();
            this.Container.RegisterTransient<IPersonalAccountBenefitsService, PersonalAccountBenefitsService>();
            this.Container.RegisterTransient<IPrivilegedCategoryService, PrivilegedCategoryService>();
            this.Container.RegisterDomainService<CalcAccountCredit, FileStorageDomainService<CalcAccountCredit>>();
            this.Container.RegisterTransient<IRealtyObjectRegopOperationService, RealtyObjectRegopOperationService>();
            this.Container.RegisterDomainService<FundFormationContract, FileStorageDomainService<FundFormationContract>>();
            this.Container.RegisterDomainService<RegopServiceLog, FileStorageDomainService<RegopServiceLog>>();
            this.Container.RegisterDomainService<CalcAccountRealityObject, CalcAccountRealityObjectDomainService>();
            this.Container.RegisterDomainService<TransferCtr, FileStorageDomainService<TransferCtr>>();
            this.Container.RegisterTransient<IExportToEbirService, ExportToEbirService>();
            this.Container.RegisterTransient<IDomainService<PersonalAccountPrivilegedCategory>, PersonalAccountPrivilegedCategoryDomainService>();
            this.Container.RegisterTransient<IRealtyObjectAccountFormationService, RealtyObjectAccountFormationService>();
            this.Container.RegisterTransient<IOwnersService, OwnersService>();
            this.Container.RegisterTransient<IPersonalAccountOwnerService, PersonalAccountOwnerService>();
            this.Container.RegisterTransient<IPersonalAccountOperationService, PersonalAccountOperationService>();

            this.Container.RegisterTransient<IAccountOwnershipHistoryService, AccountOwnershipHistoryService>();
            this.Container.RegisterFileStorageDomainService<PersonalAccountOwnerInformation>();
            this.Container.RegisterTransient<IPersonalAccountGroupService, PersonalAccountGroupService>();
            this.Container.RegisterTransient<IPersonalAccountSystemGroupService, PersonalAccountSystemGroupService>();
            this.Container.RegisterTransient<IPersonalAccountCorrectPaymentsService, PersonalAccountCorrectPaymentsService>();
            this.Container.RegisterTransient<IPersonalAccountCancelChargeService, PersonalAccountCancelChargeService>();
            this.Container.RegisterTransient<IPersonalAccountRepaymentService, PersonalAccountRepaymentService>();
            this.Container.RegisterTransient<IAccountSaldoChangeService, IAccountMassSaldoExportService, AccountSaldoChangeService>();
            this.Container.RegisterTransient<IRealityObjectPersonalAccountStateProvider, RealityObjectPersonalAccountStateProvider>();
            this.Container.RegisterTransient<IRealityObjectLoanViewService, RealityObjectLoanViewService>();
            this.Container.RegisterTransient<IMassPersonalAccountDtoService, MassPersonalAccountDtoService>();
            this.Container.RegisterTransient<IPersonalAccountMerger, PersonalAccountMerger>();
            this.Container.RegisterTransient<IPersonalAccountSplitService, PersonalAccountSplitService>();
            this.Container.RegisterFileStorageDomainService<PeriodCloseCheckResult>();

            this.Container.RegisterTransient<IPeriodCloseCheckService, PeriodCloseCheckService>();
            this.Container.RegisterTransient<IPeriodCloseChecker, IncomingOutcomingSaldoChecker_Active>(IncomingOutcomingSaldoChecker_Active.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, IncomingOutcomingSaldoChecker_Others>(IncomingOutcomingSaldoChecker_Others.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, OutcomingSaldoInconstistenceChecker_Active>(OutcomingSaldoInconstistenceChecker_Active.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, OutcomingSaldoInconstistenceChecker_Others>(OutcomingSaldoInconstistenceChecker_Others.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, TransferInconsistencyChecker>(TransferInconsistencyChecker.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, DebtSumSaldoInChecker>(DebtSumSaldoInChecker.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, NotExistingCopiesTransfersChecker>(NotExistingCopiesTransfersChecker.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, ChargesInconsistencyChecker>(ChargesInconsistencyChecker.Id);
            this.Container.RegisterTransient<IPeriodCloseChecker, TransferWalletIncosistencyChecker>(TransferWalletIncosistencyChecker.Id);

            this.Container.RegisterTransient<IStimulService, PayDocStimulService>(PayDocStimulService.Code);
            this.Container.RegisterDomainService<PaymentDocumentTemplate, PaymentDocumentTemplateDomainService>();

            this.Container.RegisterTransient<ICompositionService, AccountCompositionService>(CompositionType.Account.ToString());
            this.Container.RegisterTransient<ICompositionService, DeliveryAgentCompositionService>(CompositionType.DeliveryAgent.ToString());

            this.Container.ReplaceTransient<IRealityObjectFieldsService, RealityObjectFieldsServiceFromGkh, RealityObjectFieldsService>();

            this.Container.RegisterSessionScoped<IDomainService<Transfer>, ITransferDomainService, TransferDomainService>();
            this.Container.RegisterSessionScoped<IRepository<Transfer>, InMemoryRepository<Transfer>>();

            this.Container.RegisterTransient<IDebtorClaimWorkUpdateService, DebtorClaimWorkUpdateService>();
            this.Container.RegisterTransient<IDebtPeriodCalcService, DebtPeriodCalcService>();
            this.Container.RegisterTransient<IRestructDebtScheduleService, RestructDebtScheduleService>();
            this.Container.RegisterTransient<IPersonalAccountCalcDebtService, PersonalAccountCalcDebtService>();
            this.Container.RegisterTransient<IChesTempDataProviderBuilder, ChesTempDataProviderBuilder>();
            this.Container.RegisterTransient<IDebtorStateProvider, DebtorStateProvider>();
            this.Container.RegisterDomainService<PersonalAccountCalcDebt, FileStorageDomainService<PersonalAccountCalcDebt>>();

            this.Container.RegisterTransient<IViewAccOwnershipHistoryRepository,
                IRepository<ViewAccountOwnershipHistory>,
                ViewAccOwnershipHistoryRepository>();
        }

        private void RegisterEntityHistoryServices()
        {
            this.Container.RegisterTransient<IEntityHistoryService<GovDecision>, GovDecisionHistory>();
            this.Container.RegisterTransient<IEntityHistoryService<RealityObjectDecisionProtocol>, RealityObjectDecisionHistory>();
        }

        private void RegisterVersionedEntities()
        {
            this.Container.RegisterTransient<IVersionedEntity, PaymentSizeCrVersionMap>();
            this.Container.RegisterTransient<IVersionedEntity, PersonalAccountVersionMap>();
            this.Container.RegisterTransient<IVersionedEntity, RoomAreaVersionMap>();
            this.Container.RegisterTransient<IVersionedEntity, RoomNumVersionMap>();
            this.Container.RegisterTransient<IVersionedEntity, RoomOwnershipTypeVersionMap>();
            this.Container.RegisterTransient<IVersionedEntity, PersonalAccountOpenCloseDatesMap>();
            this.Container.RegisterTransient<IVersionedEntity, PersonalAccountBenefitsMap>();
            this.Container.RegisterTransient<IVersionedEntity, RoomChamberNumVersionMap>();

            Component
                .For<IPostInsertEventListener>()
                .Forward<IPostDeleteEventListener>()
                .ImplementedBy<NhVersionedListener>()
                .LifestyleTransient()
                .RegisterIn(this.Container);
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<FsGorodImportInfo, FsGorodImportInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<PaymentDocumentSnapshot, PaymentDocumentSnapshotInterceptor>();
            this.Container.RegisterDomainInterceptor<PaymentDocumentTemplate, PaymentDocumentTemplateInterceptor>();

            this.Container.RegisterDomainInterceptor<PaymentAgent, PaymentAgentInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<CalcAccount>, CalcAccountInterceptor<CalcAccount>>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RegopCalcAccount>, CalcAccountInterceptor<RegopCalcAccount>>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<BasePersonalAccount>, BasePersonalAccountDomainInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<GovDecision>, GovDecisionAccountInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<ChargePeriod>, ChargePeriodInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<PaymentPenalties>, PaymentPenaltiesInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<FixedPeriodCalcPenalties>, FixedPeriodCalcPenaltiesInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<PersonalAccountOwner>, PersonalAccountOwnerInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<IndividualAccountOwner>, IndividualAccountOwnerInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<LegalAccountOwner>, LegalAccountOwnerInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<Room>, RoomInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RealityObject>, RealityObjectInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<PerformedWorkActPayment>, PerformedWorkActPaymentInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RealityObjectPaymentAccountOperation>, RealityObjectPaymentAccountOperationInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RealityObjectDecisionProtocol>, RealityObjectDecisionProtocolInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<MonthlyFeeAmountDecision>, MonthlyFeeAmountDecisionInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<PerformedWorkAct>, PerformedWorkActInterceptor>();
            this.Container.RegisterDomainInterceptor<SuspenseAccount, SuspenseAccountInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectLoan, RealityObjectLoanInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectChargeAccount, RealityObjectChargeAccountInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectPaymentAccount, RealityObjectPaymentAccountInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectSupplierAccount, RealityObjectSupplierAccountInterceptor>();
            this.Container.RegisterTransient<IDomainServiceInterceptor<RegOperator>, RegOperatorInterceptor>();
            this.Container.RegisterDomainInterceptor<DeliveryAgentRealObj, DeliveryAgentRealObjInterceptor>();
            this.Container.RegisterDomainInterceptor<DeliveryAgent, DeliveryAgentInterceptor>();
            this.Container.RegisterDomainInterceptor<CashPaymentCenter, CashPaymentCenterInterceptor>();
            this.Container.RegisterDomainInterceptor<CashPaymentCenterRealObj, CashPaymentCenterRealObjInterceptor>();
            this.Container.RegisterDomainInterceptor<CashPaymentCenterManOrg, CashPaymentCenterManOrgInterceptor>();
            this.Container.RegisterDomainInterceptor<CashPaymentCenterManOrgRo, CashPaymentCenterManOrgRoInterceptor>();
            this.Container.RegisterDomainInterceptor<PaymentDocInfo, PaymentDocInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<FederalStandardFeeCr, FederalStandardFeeCrInterceptor>();
            this.Container.RegisterDomainInterceptor<CreditOrgServiceCondition, CreditOrgServiceConditionInterceptor>();
            this.Container.RegisterDomainInterceptor<GovDecision, GovDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<PrivilegedCategory, PrivilegedCategoryInterceptor>();
            this.Container.RegisterDomainInterceptor<RegopCalcAccount, RegopCalcAccountInterceptor>();
            this.Container.RegisterDomainInterceptor<CalcAccountCredit, CalcAccountCreditInterceptor>();
            this.Container.RegisterDomainInterceptor<CalcAccountOverdraft, CalcAccountOverdraftInterceptor>();
            this.Container.RegisterDomainInterceptor<SpecialCalcAccount, SpecialCalcAccountInterceptor>();

            this.Container.RegisterDomainInterceptor<DecisionNotification, DecisionNotificationInterceptor>();
            this.Container.RegisterDomainInterceptor<CalcAccountRealityObject, CalcAccountRealityObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<TransferCtr, TransferCtrInterceptor>();
            this.Container.RegisterDomainInterceptor<PenaltiesWithDeferred, PenaltiesWithDeferredInterceptor>();
            this.Container.RegisterDomainInterceptor<PersonalAccountBanRecalc, PersonalAccountBanRecalcInterceptor>();
            this.Container.RegisterDomainInterceptor<Municipality, MunicipalityInterceptor>();

            this.Container.RegisterDomainInterceptor<BankAccountStatement, BankStatementInterceptor>();

            this.Container.Register(
                Component.For<IDomainServiceInterceptor<UltimateDecision>>()
                    .ImplementedBy<UltimateDecisionInterceptor<UltimateDecision>>()
                         .LifeStyle.Transient);

            this.Container.Register(
                Component.For<IDomainServiceInterceptor<CreditOrgDecision>>()
                    .ImplementedBy<UltimateDecisionInterceptor<CreditOrgDecision>>()
                         .LifeStyle.Transient);

            this.Container.Register(
                Component.For<IDomainServiceInterceptor<AccountOwnerDecision>>()
                    .ImplementedBy<UltimateDecisionInterceptor<AccountOwnerDecision>>()
                         .LifeStyle.Transient);

            this.Container.Register(
                Component.For<IDomainServiceInterceptor<AccountManagementDecision>>()
                    .ImplementedBy<UltimateDecisionInterceptor<AccountManagementDecision>>()
                         .LifeStyle.Transient);

            this.Container.Register(
                Component.For<IDomainServiceInterceptor<AccumulationTransferDecision>>()
                    .ImplementedBy<UltimateDecisionInterceptor<AccumulationTransferDecision>>()
                         .LifeStyle.Transient);

            this.Container.Register(
               Component.For<IDomainServiceInterceptor<CrFundFormationDecision>>()
                   .ImplementedBy<UltimateDecisionInterceptor<CrFundFormationDecision>>()
                        .LifeStyle.Transient);

            this.Container.Register(
               Component.For<IDomainServiceInterceptor<JobYearDecision>>()
                   .ImplementedBy<UltimateDecisionInterceptor<JobYearDecision>>()
                        .LifeStyle.Transient);

            this.Container.Register(
               Component.For<IDomainServiceInterceptor<MinFundAmountDecision>>()
                   .ImplementedBy<UltimateDecisionInterceptor<MinFundAmountDecision>>()
                        .LifeStyle.Transient);

            this.Container.Register(
               Component.For<IDomainServiceInterceptor<MkdManagementDecision>>()
                   .ImplementedBy<UltimateDecisionInterceptor<MkdManagementDecision>>()
                        .LifeStyle.Transient);

            this.Container.Register(
               Component.For<IDomainServiceInterceptor<MonthlyFeeAmountDecision>>()
                   .ImplementedBy<UltimateDecisionInterceptor<MonthlyFeeAmountDecision>>()
                        .LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<RealEstateType>, RealEstateTypeInterceptor>(
                Component.For<IDomainServiceInterceptor<RealEstateType>>()
                    .ImplementedBy<Interceptors.RealEstateTypeInterceptor>()
                    .LifestyleTransient());

            this.Container.RegisterDomainInterceptor<Entrance, EntranceInterceptor>();

            this.Container.RegisterDomainInterceptor<PersonalAccountPrivilegedCategory, Interceptors.PersonalAccount.PersonalAccountPrivilegedCategoryInterceptor>();
            this.Container.RegisterDomainInterceptor<SubsidyIncomeDetail, SubsidyIncomeDetailInterceptor>();

            this.Container.RegisterDomainInterceptor<CashPaymentCenterPersAcc, CashPaymentCenterPersAccInterceptor>();

            this.Container.RegisterDomainInterceptor<PersAccGroup, PersAccGroupInterceptor>();

            this.Container.RegisterDomainInterceptor<PeriodCloseCheck, PeriodCloseCheckInterceptor>();
            this.Container.RegisterDomainInterceptor<Petition, PetitionInterceptor>();

            this.Container.RegisterDomainInterceptor<Contragent, RegOperatorContragentServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<AddressMatch, AddressMatchInterceptor>();
            this.Container.RegisterDomainInterceptor<ChesMatchAccountOwner, ChesMatchAccountOwnerInterceptor<ChesMatchAccountOwner>>();
            this.Container.RegisterDomainInterceptor<ChesMatchIndAccountOwner, ChesMatchAccountOwnerInterceptor<ChesMatchIndAccountOwner>>();
            this.Container.RegisterDomainInterceptor<ChesMatchLegalAccountOwner, ChesMatchAccountOwnerInterceptor<ChesMatchLegalAccountOwner>>();

            this.Container.RegisterDomainInterceptor<LawsuitIndividualOwnerInfo, LawsuitIndividualOwnerInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<LawsuitLegalOwnerInfo, LawsuitLegalOwnerInfoInterceptor>();

            this.Container.RegisterDomainInterceptor<PretensionClw, DocumentClwInterceptor<PretensionClw>>();
            this.Container.RegisterDomainInterceptor<Lawsuit, DocumentClwInterceptor<Lawsuit>>();
            this.Container.RegisterDomainInterceptor<Petition, DocumentClwInterceptor<Petition>>();
            this.Container.RegisterDomainInterceptor<CourtOrderClaim, DocumentClwInterceptor<CourtOrderClaim>>();
            this.Container.RegisterDomainInterceptor<RestructDebt, RestructDebtInterceptor>();

            this.Container.RegisterDomainInterceptor<IndividualClaimWork, DebtorClaimWorkInterceptor<IndividualClaimWork>>();
            this.Container.RegisterDomainInterceptor<LegalClaimWork, DebtorClaimWorkInterceptor<LegalClaimWork>>();
        }

        private void RegisterEntityServices()
        {
            this.Container.RegisterTransient<IPaymentSrcFinanceDetailsService, PaymentSrcFinanceDetailsService>();
            this.Container.RegisterTransient<IAccountPeriodSummaryService, AccountPeriodSummaryService>();
            this.Container.RegisterTransient<IPersonalAccountCharger, PersonalAccountCharger>();
            this.Container.RegisterTransient<ILawsuitOwnerInfoService, LawsuitOwnerInfoService>();
            this.Container.RegisterTransient<IPaymentDocumentService, PaymentDocumentService>();
            this.Container.RegisterTransient<IPersonalAccountPrivilegedCategoryService, PersonalAccountPrivilegedCategoryService>();

            this.Container.RegisterSessionScoped<IChargePeriodService, ChargePeriodService>();

            this.Container.RegisterTransient<IChargeCalculationImplFactory, ChargeCalculationImplFactory>();
            this.Container.RegisterTransient<IParameterTracker, ParameterTracker>();
            this.Container.RegisterTransient<IParameterTracker, OnDateParameterTracker>("OnDateParameterTracker");
            this.Container.RegisterTransient<DomainService.PersonalAccount.IPersonalAccountService, PersonalAccountService>();
            this.Container.RegisterTransient<IPersonalAccountReportService, PersonalAccountReportService>();
            this.Container.RegisterTransient<ILoanService, LoanService>();
            this.Container.RegisterTransient<IRealityObjectAccountGenerator, RealityObjectAccountGenerator>();

            this.Container.RegisterTransient<IRealityObjectPaymentService, RealityObjectPaymentService>();

            this.Container.RegisterTransient<IUnacceptedPaymentProvider, UnacceptedPaymentProvider>();

            this.Container.RegisterTransient<IPersonalAccountInfoService, PersonalAccountInfoService>();
            this.Container.RegisterTransient<IPersonalAccountOperationLogService, PersonalAccountOperationLogService>();

            this.Container.RegisterTransient<IPersonalAccountSummaryService, PersonalAccountSummaryService>();
            this.Container.RegisterTransient<IRegoperatorParamsService, RegoperatorParamsService>();
            this.Container.RegisterTransient<IRealityObjectAccountService, RealityObjectAccountService>();
            this.Container.RegisterTransient<IRegOperatorMunicipalityService, RegOperatorMunicipalityService>();
            this.Container.RegisterTransient<IBankAccountDataProvider, BankAccountDataProvider>();

            this.Container.RegisterTransient<ITransferObjectService, TransferObjectService>();
            this.Container.RegisterTransient<ITransferObjectDataService, TransferObjectDataService>();
            this.Container.RegisterTransient<IDeliveryAgentService, DeliveryAgentService>();
            this.Container.RegisterTransient<ICashPaymentCenterService, CashPaymentCenterService>();

            this.Container.RegisterTransient<ICashPaymentCenterObjectsService, CashPaymentCenterRealityObjService>("CachPaymentCenterConnectionType.ByRealty");
            this.Container.RegisterTransient<ICashPaymentCenterObjectsService, CashPaymentCenterPersAccService>("CachPaymentCenterConnectionType.ByAccount");

            this.Container.RegisterTransient<IRealityObjectSubsidyService, RealityObjectSubsidyService>();

            this.Container.RegisterService<IRegopService, RegopService>();
            this.Container.RegisterTransient<ISpecialCalcAccountService, SpecialCalcAccountService>();
            this.Container.RegisterTransient<ICalcAccountService, CalcAccountService>();
            this.Container.RegisterTransient<ICalcAccountRealityObjectService, CalcAccountRealityObjectService>();
            this.Container.RegisterTransient<ICalcAccountOverdraftService, CalcAccountOverdraftService>();
            this.Container.RegisterTransient<ICalcAccountCreditService, CalcAccountCreditService>();
            this.Container.RegisterTransient<IRegopCalcAccountService, RegopCalcAccountService>();
            this.Container.RegisterTransient<IPerformedWorkActPaymentService, PerformedWorkActPaymentService>();
            this.Container.RegisterTransient<IRkcImportService, RkcImportService>();

            this.Container.RegisterTransient<IRealityObjectLoanRepository, RealityObjectLoanRepository>();
            this.Container.RegisterTransient<IRealtyObjectNeedLoanService, RealtyObjectNeedLoanByPerformedWorkActService>(nameof(RealtyObjectNeedLoanByPerformedWorkActService));
            this.Container.RegisterTransient<IRealtyObjectNeedLoanService, RealtyObjectNeedLoanByTransferCtrService>(nameof(RealtyObjectNeedLoanByTransferCtrService));
            this.Container.RegisterTransient<ITransferService, TransferService>();

            this.Container.RegisterTransient<IGovDecisionAccountService, GovDecisionAccountService>();
            this.Container.RegisterTransient<IOverhaulToGasuExportService, RegopOverhaulToGasuExportService>();
            this.Container.RegisterTransient<ITransferCtrService, TransferCtrService>();
            this.Container.RegisterTransient<IRealityObjectTariffService, RealityObjectTariffService>();
            this.Container.RegisterTransient<IRealObjDecInfoService, RealObjDecInfoService>();
            this.Container.RegisterTransient<IRealityObjectRealEstateTypeService, RealityObjectRealEstateTypeService>();
            
            this.Container.RegisterTransient<IExportVtscpService, ExportVtscpService>();

            this.Container.RegisterTransient<IBankAccountStatementService, BankAccountStatementService>();
            this.Container.RegisterTransient<IDebtorCalcService, DebtorCalcService>();
            this.Container.RegisterTransient<IDebtorJurInstitutionCache, DebtorJurInstitutionCache>();

            this.Container.RegisterTransient<IPaymentPenaltiesService, PaymentPenaltiesService>();
            this.Container.RegisterTransient<IRealityObjectBothProtocolService, RealityObjectBothProtocolService>();

            this.Container.RegisterTransient<IJurJournalDebtorService, JurJournalDebtorService>();
            this.Container.RegisterTransient<ISubsidyIncomeService, SubsidyIncomeService>();

            this.Container.RegisterTransient<ILawsuitAutoSelector, DebtorLawsuitAutoSelector>(DebtorLawsuitAutoSelector.Id);
            this.Container.RegisterTransient<IExportPenaltyService, ExportPenaltyService>();
            this.Container.RegisterTransient<IImportedPaymentService, ImportedPaymentService>();
            this.Container.RegisterTransient<IBankDocumentImportProvider, BankDocumentImportProvider>();

            this.Container.Register(Component.For<IPerformedWorkDistribution>()
                .UsingFactoryMethod(() => PerfWorkDistribServiceFactory.Create(this.Container))
                .LifeStyle.Scoped());

            this.Container.Register(Component.For<IPerformedWorkDistribution>()
                .ImplementedBy<PerformedWorkDistribution>()
                .Named(nameof(PerformedWorkDistribution))
                .LifeStyle.Scoped());

            this.Container.Register(Component
                .For<IPerformedWorkDistribution>()
                .Forward<IPerformedWorkDetailService>()
                .ImplementedBy<PerformedWorkDeferredDistribution>()
                .Named(nameof(PerformedWorkDeferredDistribution))
                .LifeStyle.Scoped());

            //this.Container.RegisterTransient<IPetitionService, PetitionService>();

            this.Container.RegisterTransient<ISourceConfigService, SourceConfigService>();
        }

        private void RegisterViewModels()
        {
            #region Owner
            this.Container.RegisterViewModel<LegalAccountOwner, LegalAccountOwnerViewModel>();
            this.Container.RegisterViewModel<LawsuitReferenceCalculation, LawsuitReferenceCalculationViewModel>();

            #endregion Owner

            this.Container.RegisterViewModel<AccountPaymentInfoSnapshot, AccountPaymentInfoSnapshotViewModel>();
            this.Container.RegisterViewModel<PaymentDocumentSnapshot, PaymentDocumentSnapshotViewModel>();
            this.Container.RegisterViewModel<PaymentDocumentTemplate, PaymentDocumentTemplateViewModel>();

            this.Container.RegisterViewModel<FsGorodMapItem, FsGorodMapItemViewModel>();

            this.Container.RegisterViewModel<PeriodPaymentDocuments, PeriodPaymentDocumentsViewModel>();
            this.Container.RegisterViewModel<BankAccountStatement, BankAccountStatementViewModel>();
            this.Container.RegisterViewModel<Debtor, DebtorViewModel>();
            this.Container.RegisterViewModel<PaymentSrcFinanceDetails, PaymentSrcFinanceDetailsViewModel>();
            this.Container.RegisterViewModel<PersonalAccountOwner, PersonalAccountOwnerViewModel>();

            this.Container.RegisterCacheableViewModel<BasePersonalAccount, BasePersonalAccountViewModel>();
            this.Container.RegisterViewModel<PersonalAccountCharge, PersonalAccountChargeViewModel>();
            this.Container.RegisterViewModel<UnacceptedCharge, UnacceptedChargeViewModel>();
            this.Container.RegisterViewModel<LocationCode, LocationCodeViewModel>();
            this.Container.RegisterViewModel<CreditOrgServiceCondition, CreditOrgServiceConditionViewModel>();

            this.Container.RegisterViewModel<RealityObjectChargeAccountOperation, RealityObjectChargeAccountOperationViewModel>();
            this.Container.RegisterViewModel<RealityObjectPaymentAccount, RealityObjectPaymentAccountViewModel>();
            this.Container.RegisterViewModel<RealityObjectPaymentAccountOperation, RealityObjectPaymentAccountOperationViewModel>();
            this.Container.RegisterViewModel<PersonalAccountPeriodSummary, PersonalAccountPeriodSummaryViewModel>();
            this.Container.RegisterViewModel<RealityObjectLoan, RealityObjectLoanViewModel>();
            this.Container.RegisterViewModel<RealityObjectSupplierAccountOperation, RealityObjectSupplierAccountOperationViewModel>();
            this.Container.RegisterViewModel<BankDocumentImport, BankDocumentImportViewModel>();
            this.Container.RegisterViewModel<ImportedPayment, ImportedPaymentViewModel>();
            this.Container.RegisterViewModel<ChargePeriod, ChargePeriodViewModel>();
            this.Container.RegisterViewModel<UnacceptedPayment, UnacceptedPaymentViewModel>();
            this.Container.RegisterViewModel<ComputingProcess, ComputingProcessViewModel>();
            this.Container.RegisterViewModel<PersonalAccountChange, PersonalAccountChangeViewModel>();
            //Container.RegisterViewModel<RealityObjectSpecialOrRegOperatorAccount, RealityObjectSpecialOrRegOperatorAccountViewModel>();

            this.Container.RegisterViewModel<RegOperator, RegOperatorViewModel>();
            this.Container.RegisterViewModel<RegOperatorMunicipality, RegOperatorMunicipalityViewModel>();
            //Container.RegisterViewModel<RegOpCalcAccount, RegOpCalcAccountViewModel>();
            //Container.RegisterViewModel<RegopCalcAccountRealityObject, RegopCalcAccountRealityObjectViewModel>();
            this.Container.RegisterViewModel<RegOpPersAccMunicipality, RegOpPersAccMuViewModel>();
            this.Container.RegisterViewModel<DecisionNotification, DecisionNotificationViewModel>();
            this.Container.RegisterViewModel<FundFormationContract, FundFormationContractViewModel>();

            this.Container.RegisterViewModel<UnacceptedPaymentPacket, UnacceptedPaymentPacketViewModel>();
            this.Container.RegisterViewModel<UnacceptedChargePacket, UnacceptedChargePacketViewModel>();

            this.Container.RegisterViewModel<TransferObject, TransferObjectViewModel>();
            this.Container.RegisterViewModel<DeliveryAgent, DeliveryAgentViewModel>();
            this.Container.RegisterViewModel<DeliveryAgentMunicipality, DeliveryAgentMuViewModel>();
            this.Container.RegisterViewModel<DeliveryAgentRealObj, DeliveryAgentRealObjViewModel>();
            this.Container.RegisterViewModel<CashPaymentCenter, CashPaymentCenterViewModel>();
            this.Container.RegisterViewModel<CashPaymentCenterMunicipality, CashPaymentCenterMuViewModel>();
            this.Container.RegisterViewModel<CashPaymentCenterRealObj, CashPaymentCenterRealObjViewModel>();
            this.Container.RegisterViewModel<CashPaymentCenterManOrg, CashPaymentCenterManOrgViewModel>();
            this.Container.RegisterViewModel<CashPaymentCenterManOrgRo, CashPaymentCenterManOrgRoViewModel>();

            this.Container.RegisterViewModel<SuspenseAccount, SuspenseAccountViewModel>();

            this.Container.RegisterViewModel<PaymentDocInfo, PaymentDocInfoViewModel>();
            this.Container.RegisterViewModel<RealityObjectSubsidyAccountOperation, RealityObjectSubsidyAccountOperationViewModel>();

            this.Container.RegisterViewModel<SpecialCalcAccount, SpecialCalcAccountViewModel>();
            this.Container.RegisterViewModel<RegopCalcAccount, RegopCalcAccountViewModel>();
            this.Container.RegisterViewModel<CalcAccount, CalcAccountViewModel>();
            this.Container.RegisterViewModel<CalcAccountCredit, CalcAccountCreditViewModel>();
            this.Container.RegisterViewModel<CalcAccountOverdraft, CalcAccountOverdraftViewModel>();
            this.Container.RegisterViewModel<CalcAccountRealityObject, CalcAccountRealityObjectViewModel>();

            this.Container.RegisterViewModel<Paysize, PaysizeViewModel>();

            this.Container.RegisterViewModel<RegopServiceLog, RegopServiceLogViewModel>();
            this.Container.ReplaceComponent(
                typeof(IViewModel<TransferRfRecord>),
                typeof(TransferRfRecordViewModel),
                Component.For<IViewModel<TransferRfRecord>>().ImplementedBy<DomainService.TransferRf.TransferRfRecordViewModel>().LifestyleTransient());

            this.Container.RegisterViewModel<CoreDecision, CoreDecisionViewModel>();
            this.Container.RegisterViewModel<BaseDecisionProtocol, BaseDecisionProtocolViewModel>();
            this.Container.ReplaceTransient<IViewModel<GovDecision>, GovDecisionViewModel, ViewModels.GovDecisionViewModel>();

            this.Container.ReplaceComponent(
              typeof(IViewModel<PretensionClw>),
              typeof(Gkh.Modules.ClaimWork.ViewModel.PretensionViewModel),
              Component.For<IViewModel<PretensionClw>>().ImplementedBy<PretensionViewModel>().LifestyleTransient());
            this.Container.RegisterViewModel<DistributionDetail, DistributionDetailViewModel>();

            this.Container.RegisterViewModel<PaymentOrderDetail, PaymentOrderDetailViewModel>();
            this.Container.RegisterViewModel<TransferCtr, TransferCtrViewModel>();
            this.Container.RegisterViewModel<TransferCtrPaymentDetail, TransferCtrPaymentDetailViewModel>();
            this.Container.RegisterAltDataController<ProgramCrType>();
            this.Container.RegisterAltDataController<LawsuitReferenceCalculation>();
            this.Container.RegisterViewModel<CashPaymentCenterPersAcc, CashPaymentCenterPersAccViewModel>();
            
            this.Container.RegisterViewModel<LegalClaimWork, LegalClaimWorkViewModel>();
            this.Container.RegisterViewModel<IndividualClaimWork, IndividualClaimWorkViewModel>();
            this.Container.RegisterViewModel<ClaimWorkAccountDetail, ClaimWorkAccountDetailViewModel>();

            this.Container.RegisterViewModel<RestructDebtSchedule, RestructDebtScheduleViewModel>();

            this.Container.RegisterViewModel<PersonalAccountPrivilegedCategory, PersonalAccountPrivilegedCategoryViewModel>();
            this.Container.RegisterViewModel<PersonalAccountOwnerInformation, PersonalAccountOwnerInformationViewModel>();
            this.Container.RegisterViewModel<LawsuitOwnerInfo, LawsuitOwnerInfoViewModel>();
            this.Container.RegisterViewModel<PaymentPenaltiesExcludePersAcc, PaymentPenaltiesExcludePersAccViewModel>();
            this.Container.RegisterViewModel<SubsidyIncomeDetail, SubsidyIncomeDetailViewModel>();
            this.Container.RegisterViewModel<SubsidyIncome, SubsidyIncomeViewModel>();
            this.Container.RegisterViewModel<PersonalAccountBenefits, PersonalAccountBenefitsViewModel>();
            this.Container.RegisterViewModel<PersAccGroup, PersAccGroupViewModel>();
            this.Container.RegisterViewModel<PersonalAccountBanRecalc, PersonalAccountBanRecalcViewModel>();

            this.Container.RegisterTransient<IJurJournalType, JurJournalDebtorType>();

            this.Container.ReplaceComponent(
                typeof(IViewModel<Contragent>),
                typeof(ContragentViewModel),
                Component.For<IViewModel<Contragent>>().ImplementedBy<ViewModels.Contragent.ContragentViewModel>().LifestyleTransient());

            this.Container.ReplaceComponent(
                typeof(IViewModel<EntityLogLight>),
                typeof(Gkh.ViewModel.EntityLogLightViewModel),
                Component.For<IViewModel<EntityLogLight>>().ImplementedBy<EntityLogLightViewModel>().LifestyleTransient());

            this.Container.RegisterTransient<IPersonalAccountDistributionViewModel, PersonalAccountDistributionViewModel>();

            this.Container.RegisterTransient<IPersonalAccountCustomViewModel, PersonalAccountCustomViewModel>();

            this.Container.RegisterViewModel<PeriodCloseCheckHistory, PeriodCloseCheckHistoryViewModel>();
            this.Container.RegisterViewModel<PeriodCloseCheckResult, PeriodCloseCheckResultViewModel>();

            this.Container.RegisterViewModel<Entities.Import.Ches.ChesImport, ChesImportViewModel>();
            this.Container.RegisterViewModel<Entities.Import.Ches.ChesImportFile, ChesImportFileViewModel>();
            this.Container.RegisterViewModel<Entities.Import.Ches.ChesNotMatchAddress, ChesNotMatchAddressViewModel>();
            this.Container.RegisterViewModel<Entities.Import.Ches.ChesMatchIndAccountOwner, ChesMatchIndAccountOwnerViewModel>();
            this.Container.RegisterViewModel<Entities.Import.Ches.ChesMatchLegalAccountOwner, ChesMatchLegalAccountOwnerViewModel>();
            this.Container.RegisterViewModel<Entities.Import.Ches.ChesNotMatchLegalAccountOwner, ChesNotMatchLegalAccountOwnerViewModel>();
            this.Container.RegisterViewModel<Entities.Import.Ches.ChesNotMatchIndAccountOwner, ChesNotMatchIndAccountOwnerViewModel>();
            this.Container.RegisterViewModel<ContragentBankCreditOrg, ContragentBankCreditOrgViewModel>();
            this.Container.RegisterViewModel<DocumentClwAccountDetail, DocumentClwAccountDetailViewModel>();
            this.Container.RegisterViewModel<ViewDocumentClw, ViewDocumentClwViewModel>();
        }

        private void RegisterControllers()
        {
            #region Owner
            this.Container.RegisterController<PersonalAccountOwnerController>();
            this.Container.RegisterAltDataController<LegalAccountOwner>();
            this.Container.RegisterAltDataController<IndividualAccountOwner>();
            this.Container.RegisterAltDataController<PersonalAccountOwnerInformation>();

            #endregion Owner

            #region Distribution
            this.Container.RegisterController<DistributionController>();

            #endregion Distribution

            #region Контроллеры для мапов импортов фс город
            this.Container.RegisterAltDataController<FsGorodMapItem>();
            this.Container.RegisterController<FsImportSetupController>();

            #endregion

            #region ПИР
            this.Container.RegisterController<DebtorClaimworkRegoperatorController>();
            this.Container.RegisterController<JurJournalDebtorController>();
            this.Container.RegisterAltDataController<ViewDebtor>();
            this.Container.RegisterAltDataController<ViewIndividualClaimWork>();
            this.Container.RegisterAltDataController<ClaimWorkAccountDetail>();

            #endregion

            this.Container.RegisterController<PaymentDocumentLogController>();
            this.Container.RegisterController<PaymentDocumentSnapshotController>();
            this.Container.RegisterController<AccountPaymentInfoSnapshotController>();
            this.Container.RegisterAltDataController<PaymentDocumentTemplate>();
            this.Container.RegisterController<PaymentDocReportManagerController>();

            this.Container.RegisterController<RealityObjectAccountUpdaterController>();

            this.Container.RegisterController<PrivilegedCategoryController>();
            this.Container.RegisterController<PeriodPaymentDocumentsController>();
            this.Container.RegisterController<TransitAccountController>();
            this.Container.RegisterController<ImportController>();
            this.Container.RegisterController<DebtorController>();
            this.Container.RegisterController<FsGorodImportInfoController>();
            this.Container.RegisterController<PaymentSrcFinanceDetailsController>();
            this.Container.RegisterController<LocalityAddressByMoController>();
            this.Container.RegisterController<BankAccountStatementController>();

            this.Container.RegisterAltDataController<PersonalAccountCharge>();
            this.Container.RegisterAltDataController<BankAccountStatementGroup>();
            this.Container.RegisterController<Bars.B4.Alt.BaseDataController<PersonalAccountCharge>>();
            this.Container.RegisterAltDataController<UnacceptedCharge>();
            this.Container.RegisterAltDataController<UnacceptedChargePacket>();
            this.Container.RegisterAltDataController<RealityObjectSupplierAccount>();
            this.Container.RegisterAltDataController<RealityObjectSupplierAccountOperation>();
            this.Container.RegisterAltDataController<RealityObjectChargeAccount>();
            this.Container.RegisterAltDataController<RealityObjectChargeAccountOperation>();
            this.Container.RegisterController<RealityObjectPaymentAccountController>();
            this.Container.RegisterAltDataController<RealityObjectPaymentAccountOperation>();
            this.Container.RegisterController<RealityObjectSubsidyAccountController>();
            this.Container.RegisterAltDataController<RealityObjectSubsidyAccountOperation>();
            this.Container.RegisterController<ImportedPaymentController>();
            this.Container.RegisterAltDataController<CreditOrgServiceCondition>();
            this.Container.RegisterController<PersonalAccountChargePaymentController>();
            this.Container.RegisterController<PersonalAccountBenefitsController>();
            this.Container.RegisterController<PersonAccountGroupController>();
            this.Container.RegisterController<PersonAccountDetalizationController>();
            this.Container.RegisterController<PersonAccountOperationController>();

            this.Container.RegisterController<TransferObjectController>();
            this.Container.RegisterController<DecisProtController>();

            this.Container.RegisterController<PersonalAccountPeriodSummaryController>();
            this.Container.RegisterController<ChargePeriodController>();
            this.Container.RegisterController<ChargeController>();
            this.Container.RegisterController<LawsuitOwnerInfoController>();
            this.Container.RegisterController<BasePersonalAccountController>();
            this.Container.RegisterController<RealityObjectAccountController>();
            this.Container.RegisterController<SuspenseAccountController>();

            this.Container.RegisterController<UnacceptedPaymentController>();
            this.Container.RegisterAltDataController<UnacceptedPaymentPacket>();
            this.Container.RegisterController<BankDocumentImportController>();

            // Справочник МО для кодов
            this.Container.RegisterAltDataController<LocationCode>();

            this.Container.RegisterController<LocationCodeFiasController>();
            this.Container.RegisterController<RealityObjectLoanController>();
            //Container.RegisterController<RealityObjectSpecialOrRegOperatorAccountController>();

            this.Container.RegisterController<PaymentPenaltiesController>();
            this.Container.RegisterController<PaymentPenaltiesBasePersonalAccountController>();
            this.Container.RegisterAltDataController<BankDocumentImport>();
            this.Container.RegisterAltDataController<PersonalAccountChange>();
            this.Container.RegisterController<RegoperatorParamsController>();

            this.Container.RegisterController<PaymentDocSourceController>();

            this.Container.RegisterController<UnacceptedChargesExportController>();
            this.Container.RegisterController<RegOpAccountController>();
            this.Container.RegisterController<RegOperatorController>();
            this.Container.RegisterController<RegOperatorMunicipalityController>();

            this.Container.RegisterAltDataController<RegOpPersAccMunicipality>();
            this.Container.RegisterController<RegopCalcAccountRealityObjectController>();
            this.Container.RegisterController<RealityObjectDecisionsController>();
            this.Container.RegisterController<FileStorageDataController<FundFormationContract>>();
            this.Container.RegisterAltDataController<PaymentDocInfo>();

            this.Container.RegisterController<DeliveryAgentController>();
            this.Container.RegisterAltDataController<DeliveryAgentMunicipality>();
            this.Container.RegisterAltDataController<DeliveryAgentRealObj>();
            this.Container.RegisterController<CashPaymentCenterController>();
            this.Container.RegisterAltDataController<CashPaymentCenterMunicipality>();
            this.Container.RegisterAltDataController<CashPaymentCenterRealObj>();
            this.Container.RegisterAltDataController<CashPaymentCenterManOrg>();
            this.Container.RegisterAltDataController<CashPaymentCenterManOrgRo>();
            this.Container.RegisterController<RegOperatorMenuController>();

            this.Container.RegisterController<RegOpReportController>();
            this.Container.RegisterAltDataController<FederalStandardFeeCr>();

            this.Container.RegisterController<CalcAccountController>();
            this.Container.RegisterController<RegopCalcAccountController>();
            this.Container.RegisterController<SpecialCalcAccountController>();
            this.Container.RegisterController<FileStorageDataController<CalcAccountCredit>>();
            this.Container.RegisterController<CalcAccountOverdraftController>();
            this.Container.RegisterController<CalcAccountRealityObjectController>();
            this.Container.RegisterController<PerfWorkActPaymentController>();

            this.Container.RegisterController<FileStorageDataController<RegopServiceLog>>();

            this.Container.RegisterController<FileStorageDataController<CoreDecision>>();

            //#decisionProtocol удалить после допиливания протоколов решений
            this.Container.RegisterController<RealityObjectBothProtocolController>();

            this.Container.RegisterController<TransferController>();
            this.Container.RegisterController<PaymentOrderDetailController>();
            this.Container.RegisterController<TransferCtrController>();

            this.Container.RegisterAltDataController<TransferCtrPaymentDetail>();

            this.Container.RegisterController<CashPaymentCenterPersAccController>();

            this.Container.RegisterAltDataController<DistributionDetail>();
            this.Container.RegisterAltDataController<PersonalAccountPrivilegedCategory>();
            this.Container.RegisterAltDataController<PaymentPenaltiesExcludePersAcc>();
            this.Container.RegisterController<SubsidyIncomeController>();
            this.Container.RegisterAltDataController<SubsidyIncomeDetail>();
            this.Container.RegisterAltDataController<PenaltiesWithDeferred>();
            this.Container.RegisterAltDataController<FixedPeriodCalcPenalties>();

            this.Container.RegisterAltDataController<PersAccGroup>();
            this.Container.RegisterAltDataController<PersAccGroupRelation>();

            this.Container.RegisterController<PeriodCloseCheckController>();
            this.Container.RegisterAltDataController<PeriodCloseCheckHistory>();
            this.Container.RegisterController<PeriodCloseCheckResultController>();
            this.Container.RegisterAltDataController<PersonalAccountBanRecalc>();

            this.Container.ReplaceController<RoomController>("Room");

            this.Container.RegisterController<ChesImportController>();
            this.Container.RegisterController<ChesNotMatchAccountOwnerController>();
            this.Container.RegisterAltDataController<ChesNotMatchAddress>();
            this.Container.RegisterAltDataController<ChesMatchAccountOwner>();
            this.Container.RegisterAltDataController<ChesMatchLegalAccountOwner>();
            this.Container.RegisterAltDataController<ChesMatchIndAccountOwner>();
            this.Container.RegisterAltDataController<ChesNotMatchAccountOwner>();
            this.Container.RegisterAltDataController<ChesNotMatchLegalAccountOwner>();
            this.Container.RegisterAltDataController<ChesNotMatchIndAccountOwner>();
            this.Container.RegisterAltDataController<PeriodCloseRollbackHistory>();

            this.Container.RegisterAltDataController<LawsuitIndividualOwnerInfo>();
            this.Container.RegisterAltDataController<LawsuitLegalOwnerInfo>();
            this.Container.RegisterAltDataController<DocumentClwAccountDetail>();
            this.Container.RegisterAltDataController<ViewDocumentClw>();
            this.Container.RegisterController<PersonalAccountCalcDebtController>();
            this.Container.RegisterAltDataController<CalcDebtDetail>();

            this.Container.RegisterController<MenuRegopController>();
            ContainerHelper.RegisterFileDataController<ChesImportFile>();
        }

        private void RegisterImports()
        {
            #region Информация, необходимая для сериализации данных при импорте/экспорте
            this.Container.RegisterSingleton<ImportExportMapperHolder, ImportExportMapperHolder>();
            this.Container.RegisterSingleton<ImportExportDataProvider, ImportExportDataProvider>();

            this.Container.RegisterTransient<IImportExportSerializer<PaymentInProxy>, DefaultImportExportSerializer<PaymentInProxy>>();
            this.Container.RegisterTransient<IImportExportSerializer<PersonalAccountPaymentInfoIn>, PersonalAccountPaymentInfoInDeserializer>();
            this.Container.RegisterTransient<IImportExportSerializer<PersonalAccountPaymentInfoIn>, PersonalAccountPaymentInfoInAltDeserializer>();
            this.Container.RegisterTransient<IImportExportSerializer<ChargeOutProxy>, ChargeOutProxyImportSerializer>();
            this.Container.RegisterTransient<IImportExportSerializer<PersonalAccountInfoProxy>, PersonalAccountInfoProxySerializer>();
            this.Container.RegisterTransient<IImportExportSerializer<BenefitsCategoryInfoProxy>, BenefitsCategoryInfoProxySerializer>();
            this.Container.RegisterTransient<IImportExportSerializer<PersonalAccountPaymentInfoIn>, PersonalAccountPaymentInfoInSpbDeserializer>();
            this.Container.RegisterTransient<IImportExportSerializer<VtscpExportRecord>, DefaultImportExportSerializer<VtscpExportRecord>>();
            this.Container.RegisterTransient<IImportExportSerializer<PrivilegedCategoryInfoProxy>, PrivilegedCategoryInfoProxySerializer>();
            this.Container.RegisterTransient<IImportExportSerializer<PersonalAccountPaymentInfoIn>, VtbMessageSerializer>();

            #region Maps

            Component.For<IImportMap, IExportDataProvider>().ImplementedBy<ChargeOutDefaultJsonImportFormat>().LifestyleTransient().RegisterIn(this.Container);
            Component.For<IImportMap, IExportDataProvider>().ImplementedBy<ChargeOutVersion1TxtImportFormat>().LifestyleTransient().RegisterIn(this.Container);
            Component.For<IImportMap, IExportDataProvider>().ImplementedBy<ChargeOutFsGorodImportFormat>().LifestyleTransient().RegisterIn(this.Container);
            this.Container.RegisterTransient<IImportMap, PersonalAccountPaymentInfoInDbfImportMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountPaymentInfoInDbf2ImportMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountPaymentInfoInDefaultXmlImportMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountPaymentInfoInDefaultJsonImportMap>();
            this.Container.RegisterTransient<IImportMap, PaPaymentInfoInFsGorodImportMap>();
            this.Container.RegisterTransient<IImportMap, VtbImportMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountInfoSberProxyMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountSberProxyMap>();
            this.Container.RegisterTransient<IImportMap, BenefitsCategoryProxyMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountInfoSberNoFioMap>();
            this.Container.RegisterTransient<IImportMap, PersonalAccountPaymentInfoInDbfSpbImportMap>();
            this.Container.RegisterTransient<IImportMap, VtscpExportRecordDbfImportMap>();
            this.Container.RegisterTransient<IImportMap, PrivilegedCategoryProxyMap>();
            this.Container.RegisterTransient<IExportDataProvider, SberbankExportProvider>();
            this.Container.RegisterTransient<IExportDataProvider, SberbankExportTambovProvider>();
            this.Container.RegisterTransient<IExportDataProvider, BenefitsCategoryExportProvider>();
            this.Container.RegisterTransient<IExportDataProvider, SberbankNoFioExporter>();
            this.Container.RegisterTransient<IExportDataProvider, PrivilegedCategoryExportProvider>();

            #endregion

            #endregion

            this.Container.RegisterImport<FsGorodImportInfoSettingsImport>(FsGorodImportInfoSettingsImport.Id);
            this.Container.RegisterImport<OwnersImport>(OwnersImport.Id);
            this.Container.RegisterImport<PersonalAccountImport>(PersonalAccountImport.Id);
            this.Container.RegisterImport<RkcImport>(RkcImport.Id);
            this.Container.RegisterImport<Imports.BankDocumentImport>(Imports.BankDocumentImport.Id);
            this.Container.RegisterImport<BankArchiveImport>(BankArchiveImport.Id);
            this.Container.RegisterImport<BankAccountStatementImport>();

            this.Container.RegisterImport<RoomImport>(RoomImport.Id);
            this.Container.RegisterImport<OwnerRoomImport>(OwnerRoomImport.Id);

            this.Container.RegisterImport<SocialSupportImport>(SocialSupportImport.Id); // 
            this.Container.RegisterImport<DecisionProtocolImport>(DecisionProtocolImport.Id);
            this.Container.RegisterImport<OwnerDecisionProtocolImport>(OwnerDecisionProtocolImport.Id);
            this.Container.RegisterImport<SuspenseAccountImport>(SuspenseAccountImport.Id);

            this.Container.RegisterTransient<IBillingFileImporter, PersonalAccountImporter>();
            this.Container.RegisterTransient<IBillingFileImporter, SocialSupportImporter>("SocialSupport");
            this.Container.RegisterImport<PersonalAccountPaymentImport>(PersonalAccountPaymentImport.Id);
            this.Container.RegisterImport<PersonalAccountPaymentImportDbf2>(PersonalAccountPaymentImportDbf2.Id);
            this.Container.RegisterImport<BenefitsCategoryImport>(BenefitsCategoryImport.Id);
            this.Container.RegisterImport<BenefitsCategoryImportVersion2>();
            this.Container.RegisterImport<OfflinePaymentAndChargeImport>();
            this.Container.RegisterImport<SubsidyIncomeImport>();

            this.Container.RegisterImport<ChargesToClosedPeriodsImport>();
            this.Container.RegisterImport<PaymentsToClosedPeriodsImport>();
            this.Container.RegisterImport<BenefitsCategoryPersAccSumImport>();
            this.Container.RegisterImport<PersonalAccountSaldoImport>();

            this.Container.RegisterImport<DebtorClaimWorkImport>();

            this.Container.RegisterImport<ChesImport>(ChesImport.Id);
            this.Container.RegisterTransient<IChesImporter<CalcFileInfo>, CalcImporter>(CalcImporter.Id);
            this.Container.RegisterTransient<IChesImporter<AccountFileInfo>, AccountImporter>(AccountImporter.Id);
            this.Container.RegisterTransient<IChesImporter<CalcProtFileInfo>, CalcProtImporter>(CalcProtImporter.Id);
            this.Container.RegisterTransient<IChesImporter<RecalcFileInfo>, RecalcImporter>(RecalcImporter.Id);
            this.Container.RegisterTransient<IChesImporter<PayFileInfo>, PayImporter>(PayImporter.Id);
            this.Container.RegisterTransient<IChesImporter<SaldoChangeFileInfo>, SaldoChangeImporter>(SaldoChangeImporter.Id);
            this.Container.RegisterTransient<IBillingFilterService, BillingFilterService>();
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IPrintForm, PaymentSrcFinanceDetailsReport>("PaymentSrcFinanceDetailsReport");
            this.Container.RegisterTransient<IPrintForm, LoanReport>("LoanReport");
            this.Container.RegisterTransient<IPrintForm, OwnersRoNotInLongTermPr>("OwnersRoNotInLongTermPr");
            this.Container.RegisterTransient<IPrintForm, OwnersRoInLongTermPr>("OwnersRoInLongTermPr");
            this.Container.RegisterTransient<IPrintForm, CheckMkdAndOwnersPremises>("CheckMkdAndOwnersPremises");
            this.Container.RegisterTransient<IPrintForm, TurnoverBalance>("TurnoverBalance");
            this.Container.RegisterTransient<IPrintForm, TurnoverBalanceByMkd>("TurnoverBalanceByMkd");
            this.Container.RegisterTransient<IPrintForm, ChargeReport>("ChargeReport");
            this.Container.RegisterTransient<IPrintForm, FundFormationReport>("FundFormationReport");
            this.Container.RegisterTransient<IPrintForm, PaymentAndBalanceReport>("PaymentAndBalanceReport");
            this.Container.RegisterTransient<IPrintForm, MkdRoomAbonentReport>("MkdRoomAbonentReport");
            this.Container.RegisterTransient<IPrintForm, MkdRoomAbonentReportOld>("MkdRoomAbonentReportOld");
            this.Container.RegisterTransient<IPrintForm, TransferFundReport>("TransferFundReport");
            this.Container.RegisterTransient<IPrintForm, MkdChargePaymentReport>("MkdChargePaymentReport");
            this.Container.RegisterTransient<IPrintForm, MkdLoanReport>("MkdLoanReport");
            //Container.RegisterTransient<IBaseReport, Report.PersonalAccountReport>("PersonalAccountReport");
            this.Container.RegisterTransient<IPrintForm, AccrualAccountStateReport>("AccrualAccountStateReport");
            this.Container.RegisterTransient<IPrintForm, NotificationFormFundReport>("NotificationFormFundReport");
            this.Container.RegisterTransient<IPrintForm, RepairPaymentAmountReport>("RepairPaymentAmountReport");
            this.Container.RegisterTransient<IPrintForm, AccountOperationsReport>("AccountOperationsReport");
            this.Container.RegisterTransient<IPrintForm, RepairPaymentByOwnerReport>("RepairPaymentByOwnerReport");
            this.Container.RegisterTransient<IPrintForm, RequestsGisuReport>("RequestsGisuReport");
            this.Container.RegisterTransient<IPrintForm, SocialSupportReport>("SocialSupportReport");
            this.Container.RegisterTransient<IPrintForm, CollectionPercentReport>("CollectionPercentReport");
            this.Container.RegisterTransient<IPrintForm, RealtiesOutOfOverhaul>("RealtiesOutOfOverhaul");
            this.Container.RegisterTransient<IPrintForm, CalendarCostPlaningReport>("CalendarCostPlaningReport");
            this.Container.RegisterTransient<IPrintForm, OwnerAndGovernmentDecisionReport>("OwnerAndGovernmentDecisionReport");
            this.Container.RegisterTransient<IPrintForm, RepairContributionInfoReport>("RepairContributionInfoReport");
            this.Container.RegisterTransient<IPrintForm, BalanceReport>("BalanceReport");
            this.Container.RegisterTransient<IPrintForm, RoomAndAccOwnersReport>("RoomAndAccOwnersReport");
            this.Container.RegisterTransient<IBaseReport, Receipt>();
            this.Container.RegisterTransient<IPrintForm, PersonalAccountChargeReport>("PersonalAccountChargeReport");
            this.Container.RegisterTransient<IPrintForm, OwnerChargeReport>("OwnerChargeReport");
            this.Container.RegisterTransient<IPrintForm, MunicipalityChargeReport>("MunicipalityChargeReport");
            this.Container.RegisterTransient<IPrintForm, RaionChargeReport>("RaionChargeReport");
            this.Container.RegisterTransient<IPrintForm, RegionChargeReport>("RegionChargeReport");
            this.Container.RegisterTransient<IGkhBaseReport, TransferCtrForm>("TransferCtrForm");
            this.Container.RegisterTransient<IBaseReport, BankDocumentImportCheckReport>("BankDocumentImportCheckReport");

            this.Container.RegisterTransient<IGkhBaseReport, ActivePersonalAccountCheckReport>(nameof(ActivePersonalAccountCheckReport));
            this.Container.RegisterTransient<IGkhBaseReport, ChargeInconsistentlyCheckReport>(nameof(ChargeInconsistentlyCheckReport));
            this.Container.RegisterTransient<IGkhBaseReport, InvalidChargesMkdCheckReport>(nameof(InvalidChargesMkdCheckReport));
            this.Container.RegisterTransient<IGkhBaseReport, PersonalAccountChangeCheckReport>(nameof(PersonalAccountChangeCheckReport));
            this.Container.RegisterTransient<IGkhBaseReport, SaldoInSaldoOutCheckReport>(nameof(SaldoInSaldoOutCheckReport));
            this.Container.RegisterTransient<IGkhBaseReport, SaldoOutCheckReport>(nameof(SaldoOutCheckReport));
        }

        private void RegisterExecutionActions()
        {
            // Отключенные действия
            //Container.RegisterExecutionAction<RestoreUnacceptedPaymentAction>(RestoreUnacceptedPaymentAction.Code);
            //Container.RegisterExecutionAction<UpdateFiasPlaceNameLengthAction>(UpdateFiasPlaceNameLengthAction.Code);
            //Container.RegisterExecutionAction<RoomAndAccountOwnerMigrationAction>(RoomAndAccountOwnerMigrationAction.Code);
            //Container.RegisterExecutionAction<SetRealtyAccountNonActiveAction>(SetRealtyAccountNonActiveAction.Code);
            //Container.RegisterExecutionAction<RegenerationIncorrectPersonalAccountsAction>(RegenerationIncorrectPersonalAccountsAction.Code);
            //Container.RegisterExecutionAction<UpdateRoSpecialAccountsAfterProtocolDeletionAction>(UpdateRoSpecialAccountsAfterProtocolDeletionAction.Code);
            //Container.RegisterExecutionAction<GuidSuspenseAccountGeneratorAction>(GuidSuspenseAccountGeneratorAction.Code);
            //Container.RegisterExecutionAction<RestoreLogLightAction>("RestoreLogLightAction");
            //Container.RegisterExecutionAction<OldConfigMigrationAction>(OldConfigMigrationAction.Id);
            //this.Container.RegisterExecutionAction<TransferCorrectionAction>();

            this.Container.RegisterExecutionAction<RealityObjectChargeAccountNumbererAction>();
            this.Container.RegisterExecutionAction<BasePersonalAccountStateFixAction>();
            this.Container.RegisterExecutionAction<RoAccountsGeneratorAction>();
            this.Container.RegisterExecutionAction<EntityLogLightPopulateAction>();
            this.Container.RegisterExecutionAction<RoCalcAccountFixAction>();
            this.Container.RegisterExecutionAction<DeleteDoubleRealObjInTransferAction>();
            this.Container.RegisterExecutionAction<AccountOwnerDuplicateDeleteAction>();
            this.Container.RegisterExecutionAction<UndoPaymentFixAction>();
            this.Container.RegisterExecutionAction<RoPaymentAccFixAction>();
            this.Container.RegisterExecutionAction<CurrPeriodPersAccSummarySaldoFixAction>();
            this.Container.RegisterExecutionAction<RecalculateTotalRoChargeAccount>();
            this.Container.RegisterExecutionAction<InputSaldoReculcAction>();
            this.Container.RegisterExecutionAction<ChargeAccountOperationCreateAction>();
            this.Container.RegisterExecutionAction<ChargeAccountOperationRestoreAction>();
            this.Container.RegisterExecutionAction<EntityLogLightComparsionAction>();
            this.Container.RegisterExecutionAction<CashPaymentPersAccCreateAction>();
            this.Container.RegisterExecutionAction<PersAccChangesRestoreAction>();
            this.Container.RegisterExecutionAction<UnacceptedPaymentPacketSumFixAction>();
            this.Container.RegisterExecutionAction<SaldoRoChargeAccountRecalcAction>();
            this.Container.RegisterExecutionAction<MigrateSuspenseAccountToBankAccountStatementAction>();
            this.Container.RegisterExecutionAction<FillAccountFormationVariantAction>();
            this.Container.RegisterExecutionAction<RoBaseTarifWalltetBalanceFix>();
            this.Container.RegisterExecutionAction<RoSubsideyWalltetBalanceFix>();
            this.Container.RegisterExecutionAction<ChelyabRoundingValuesAccrualAction>();
            this.Container.RegisterExecutionAction<TransferCtrCreateMoneyLocks>();
            this.Container.RegisterExecutionAction<ClaimWorkTypeBaseFixAction>();
            this.Container.RegisterExecutionAction<BankStatementSetAction>();
            this.Container.RegisterExecutionAction<MigrateInfoToBankDocumentImport>();
            this.Container.RegisterExecutionAction<MigrateManuallyRecToBankAccountStatementAction>();
            this.Container.RegisterExecutionAction<FillImportedPaymentAction>();
            this.Container.RegisterExecutionAction<PersAccChargesBalanceRecalcAction>();
            this.Container.RegisterExecutionAction<FixPersonalAccTransfersIsAffectedParameter>();
            this.Container.RegisterExecutionAction<CopyPrivilegedCategory>();
            this.Container.RegisterExecutionAction<DeleteDuplicatesTransferObjects>();
            this.Container.RegisterExecutionAction<UpdateSubsidyWalletTransfersAction>();
            this.Container.RegisterExecutionAction<FillingPerformedWorkActOfDebit>();
            this.Container.RegisterExecutionAction<UpdateTransferDateForCancelledBankStatementAction>();
            this.Container.RegisterExecutionAction<SetStateDebtorClaimwok>();
            this.Container.RegisterExecutionAction<MigrateDistributionsAction>();
            this.Container.RegisterExecutionAction<RecalcRoDebtAndCreditAction>();
            this.Container.RegisterExecutionAction<SetAreaSumAndPercentDebtAction>();
            this.Container.RegisterExecutionAction<FileInfoReportAction>();
            this.Container.RegisterExecutionAction<WalletsCreateAction>();
            this.Container.RegisterExecutionAction<CreateFakePersonalAccountsAction>(CreateFakePersonalAccountsAction.Id);
            this.Container.RegisterExecutionAction<RealityObjectProtocolDecisionCreateAction>();
            this.Container.RegisterExecutionAction<RealityObjectToIndividaulAccRecalculateAction>();

            this.Container.RegisterExecutionAction<RecalcPeriodSummarySaldoOutAction>();
            this.Container.RegisterExecutionAction<RecalcAllPeriodsSummarySaldoOutAction>();
            this.Container.RegisterExecutionAction<RoundTransferAmountAction>();
            this.Container.RegisterExecutionAction<OriginatorNameForTransferAction>();
            this.Container.RegisterExecutionAction<RoundingValuesAccrualAction>();
            this.Container.RegisterExecutionAction<RecalcOwnerAccountsCountAction>();
            this.Container.RegisterExecutionAction<FixInvalidBankDocumentPayments>();
            this.Container.RegisterExecutionAction<IsAffectAction>();
            this.Container.RegisterExecutionAction<CreatePartitionsForAcountChargeAction>();
            this.Container.RegisterExecutionAction<CreatePartitionsForAcountChangeAction>();
            this.Container.RegisterExecutionAction<CreatePartitionsForAcountPeriodSummaryAction>();
            this.Container.RegisterExecutionAction<CreatePartitionsForCalcParamTraceAction>();
            this.Container.RegisterExecutionAction<CreatePartitionsForRecalcHistoryAction>();
            this.Container.RegisterExecutionAction<CreatePartitionsForTransferAction>();
            this.Container.RegisterExecutionAction<CreateViewForRecalcHistoryCharge>();
            this.Container.RegisterExecutionAction<DeleteNotFixedChargesAction>();
            this.Container.RegisterExecutionAction<PerfWorkDistrTargetCoefAction>();
            this.Container.RegisterExecutionAction<CreatePeriodRefenenceInMoneyOperation>();
            this.Container.RegisterExecutionAction<FixChangeBalanceAction>();
            this.Container.RegisterExecutionAction<ValidateTransfersBeforeSplittingAction>();
            this.Container.RegisterExecutionAction<SetWalletOwnerInfoAction>();
            this.Container.RegisterExecutionAction<FillOwnershipHistoryAction>();
            this.Container.RegisterExecutionAction<CreateTransfersWithOwnersAction>();
            this.Container.RegisterExecutionAction<SetAcceptableStatesForBankDocImportAction>();
            this.Container.RegisterExecutionAction<MirateDebtorConfiguration>();
            this.Container.RegisterExecutionAction<CreateDebtorsAction>();
            this.Container.RegisterExecutionAction<SetImportedPaymentPaNotDeterminationStateReasonAction>();
            this.Container.RegisterExecutionAction<DebtorSumPermissionAction>();
            this.Container.RegisterExecutionAction<FillDecisionHistoryAction>();
        }

        private void RegisterReplacements()
        {
            this.Container.ReplaceComponent<IViewModel<Room>>(
                typeof(RoomViewModel), 
                Component.For<IViewModel<Room>>().ImplementedBy<ViewModels.RoomViewModel>().LifestyleTransient());
            this.Container.ReplaceComponent<IRealityObjectService>(
                typeof(RealityObjectService), 
                Component.For<IRealityObjectService>().ImplementedBy<Domain.Impl.RealityObjectService>());
            this.Container.ReplaceComponent<ICitizenSuggestionService>(
                typeof(CitizenSuggestionService),
                Component.For<ICitizenSuggestionService>().ImplementedBy<Domain.Impl.CitizenSuggestionService>());
            this.Container.ReplaceComponent<IRealityObjectDecisionProtocolService>(
                typeof(RealityObjectDecisionProtocolService),
                Component.For<IRealityObjectDecisionProtocolService>().ImplementedBy<DomainService.RealityObjectDecisionProtocolService.Impl.RealityObjectDecisionProtocolService>());
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IEbirExport, XmlExport>();
            this.Container.RegisterTransient<IEbirExport, XlsExport>();

            this.Container.RegisterTransient<IDataExportService, RegOperatorExport>("RegOperatorExport");
            this.Container.RegisterTransient<IDataExportService, RegOpAccountExport>("RegOpAccountExport");
            this.Container.RegisterTransient<IDataExportService, DecisionNotificationExport>("DecisionNotificationExport");
            this.Container.RegisterTransient<IDataExportService, RegopCalcAccountExport>("RegopCalcAccountExport");
            this.Container.RegisterTransient<IDataExportService, DebtorExport>("DebtorDataExport");
            this.Container.RegisterTransient<IDataExportService, BasePersonalAccountExport>("BasePersonalAccountExport");
            this.Container.RegisterTransient<IDataExportService, TransferCtrExport>("TransferCtrDataExport");
            this.Container.RegisterTransient<IDataExportService, JurJournalDebtorExport>("JurJournalDebtorExport");
            this.Container.RegisterTransient<IDataExportService, RealityObjectDataExport>("Regop RealityObjectDataExport");
            this.Container.RegisterTransient<IDataExportService, RealityObjectLoanDataExport>("RealityObjectLoanDataExport");
            this.Container.RegisterTransient<IDataExportService, ChesPaymentsExport>("ChesPaymentsExport");
            this.Container.RegisterTransient<IDataExportService, ChesSaldoExport>("ChesSaldoExport");
        }

        private void RegisterAccountNumberGenerators()
        {
            this.Container.RegisterTransient<IAccountNumberService, AccountNumberService>();

            this.Container.RegisterTransient<IAccountNumberGenerator, ShortAccountNumberGenerator>(ShortAccountNumberGenerator.TypeAccountNumber + "AccountNumberGenerator");
            this.Container.RegisterTransient<IAccountNumberGenerator, LongAccountNumberGenerator>(LongAccountNumberGenerator.TypeAccountNumber + "AccountNumberGenerator");
        }

        private void RegisterPersonalAccountOperations()
        {
            //this.Container.RegisterTransient<IPersonalAccountOperation, CancelChargeOperation>(CancelChargeOperation.Key);
            //this.Container.RegisterTransient<IPersonalAccountOperation, PenaltyCancelOperation>(PenaltyCancelOperation.Key);

            this.Container.RegisterTransient<IAccountOperationProvider, AccountOperationProvider>();

            this.Container.RegisterTransient<IPersonalAccountOperation, SetNewOwnerAccountOperation>(SetNewOwnerAccountOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, SetPenaltyAccountOperation>(SetPenaltyAccountOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, SetBalanceAccountOperation>(SetBalanceAccountOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, CloseAccountOperation>(CloseAccountOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, ReOpenAccountOperation>(ReOpenAccountOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, MergeAccountOperation>(MergeAccountOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, MassCancelChargeOperation>(MassCancelChargeOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, ManuallyRecalcOperation>(ManuallyRecalcOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, DistributeFundsForPerformedWorkOperation>(DistributeFundsForPerformedWorkOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, MassPersAccGroupOperation>(MassPersAccGroupOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, BanRecalcOperation>(BanRecalcOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, CorrectPaymentsOperation>(CorrectPaymentsOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, MassSaldoChangeOperation>(MassSaldoChangeOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, TurnOffLockFromCalculationOperation>(TurnOffLockFromCalculationOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, RepaymentOperation>(RepaymentOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, PersonalAccountSplitOperation>(PersonalAccountSplitOperation.Key);
            this.Container.RegisterTransient<IPersonalAccountOperation, CalcDebtOperation>(CalcDebtOperation.Key);
        }

        private void RegisterCatalogs()
        {
            CatalogRegistry.Add(new Catalog
            {
                Display = "Справочник периодов начислений",
                Id = "ChargePeriodMulti",
                SelectFieldClass = "B4.catalogs.ChargePeriodMultiSelectField"
            });

            CatalogRegistry.Add(new Catalog
            {
                Display = "Справочник периодов начислений",
                Id = "ChargePeriod",
                SelectFieldClass = "B4.catalogs.ChargePeriodSelectField"
            });

            CatalogRegistry.Add(new Catalog
            {
                Display = "Населенные пункты",
                Id = "Locality",
                SelectFieldClass = "B4.catalogs.LocalitySelectField"
            });

            CatalogRegistry.Add(new Catalog
            {
                Display = "Агент доставки",
                Id = "DeliveryAgent",
                SelectFieldClass = "B4.catalogs.DeliveryAgentSelectField"
            });

            CatalogRegistry.Add(new Catalog
            {
                Display = "Адрес помещения",
                Id = "RoomAddress",
                SelectFieldClass = "B4.catalogs.RoomAddressSelectField"
            });
        }

        private void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
            {
                Component.For<IBalanceUpdateChecker>()
                    .Forward<ITask>()
                    .ImplementedBy<BalanceUpdateChecker>()
                    .LifestyleTransient().RegisterIn(this.Container);

                this.Container.RegisterTransient<ITask, UnactivatePersonalAccountTask>();
                this.Container.RegisterTransient<ITask, UpdateConditionHouseAndPersAccsTask>();
                this.Container.RegisterTransient<ITask, SyncEmergencyObjectsTask>();
                ApplicationContext.Current.Events.GetEvent<AppStartEvent>().Subscribe<InitRegopQuarz>();
            }
        }

        private void RegisterFormatDataExport()
        {
            //ContainerHelper.RegisterExportableEntity<DuVotproExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<IndExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<RegopExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<RegopSchetExportableEntity>(this.Container);
            //ContainerHelper.RegisterExportableEntity<UstavVotprotExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<EpdExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<OplataPackExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ProtocolossExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ProtocolossFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<EpdCapitalExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KvarExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KvaraccomExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KvisolExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<KvisolServiceExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<OplataExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<CrFundSizeExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<CrFundSizePremisesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<CreditContractExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<CreditContractFilesExportableEntity>(this.Container);

            ContainerHelper.RegisterProxySelectorService<RegopProxy, RegopSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<RegopSchetProxy, RegopSchetSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<KapremDecisionsProxy, KapremDecisionsSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<OplataPackProxy, OplataPackSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<EpdProxy, EpdSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<IndProxy, IndSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<KvarProxy, KvarSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<NpaProxy, NpaSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<OplataProxy, OplataSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<ProtocolossProxy, ProtocolossSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<PayDogovProxy, PayDogovSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<CrFundSizeProxy, CrFundSizeSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<CrFundSizePremisesProxy, CrFundSizePremisesSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<CreditContractProxy, CreditContractSelectorService>(this.Container);

            ContainerHelper.RegisterFormatDataExportRepository<BankAccountStatement, FormatDataExportBankAccountStatementRepository>();
            ContainerHelper.RegisterFormatDataExportRepository<BankDocumentImport, FormatDataExportBankDocumentImportRepository>();
            ContainerHelper.RegisterFormatDataExportRepository<PersonalAccountPaymentTransfer, FormatDataExportPersonalAccountPaymentTransferRepository>();
            ContainerHelper.RegisterFormatDataExportRepository<AccountPaymentInfoSnapshot, FormatDataExportAccountPaymentInfoSnapshotRepository>();

            ContainerHelper.ReplaceProxySelectorService<HouseProxy,
                Gkh.FormatDataExport.ProxySelectors.Impl.HouseSelectorService,
                HouseSelectorService>(this.Container);

            ContainerHelper.RegisterProxySelectorService<ContragentRschetProxy, ContragentRschetSelectorService>();

            ContainerHelper.ReplaceFormatDataExportRepository<RealityObject,
                FormatDataExportRealityObjectRepository,
                FormatDataExportRealityObjectRegopRepository>();
        }

        private void RegisterSources()
        {
            //регистрация источников для физ лиц
            this.Container.RegisterDocumentSource<MainInfoBuilder>(PaymentDocumentType.Individual, MainInfoBuilder.Id);
            this.Container.RegisterDocumentSource<IndividualInfoBuilder>(PaymentDocumentType.Individual, IndividualInfoBuilder.Id);

            this.Container.RegisterDocumentSource<ChargesAndPaymentsBuilder>(PaymentDocumentType.Individual, ChargesAndPaymentsBuilder.Id);
            this.Container.RegisterDocumentSource<CrPaymentsBuilder>(PaymentDocumentType.Individual, CrPaymentsBuilder.Id);
            this.Container.RegisterDocumentSource<DocNumberBuilder>(PaymentDocumentType.Individual, DocNumberBuilder.Id);
            this.Container.RegisterDocumentSource<TariffAreaBuilder>(PaymentDocumentType.Individual, TariffAreaBuilder.Id);
            //this.Container.RegisterDocumentSource<DebtorInfoBuilder>(PaymentDocumentType.Individual, DebtorInfoBuilder.Id);
            this.Container.RegisterDocumentSource<LastPaymentDateBuilder>(PaymentDocumentType.Individual, LastPaymentDateBuilder.Id);
            this.Container.RegisterDocumentSource<BenefitsBuilder>(PaymentDocumentType.Individual, BenefitsBuilder.Id);
            this.Container.RegisterDocumentSource<FiasAdressToIndividualAccountOwnerBuilder>(PaymentDocumentType.Individual, FiasAdressToIndividualAccountOwnerBuilder.Id);

            //регистрация источников для юр лиц
            this.Container.RegisterDocumentSource<MainInfoBuilder>(PaymentDocumentType.Legal, MainInfoBuilder.Id);
            this.Container.RegisterDocumentSource<LegalInfoBuilder>(PaymentDocumentType.Legal, LegalInfoBuilder.Id);

            this.Container.RegisterDocumentSource<ChargesAndPaymentsBuilder>(PaymentDocumentType.Legal, ChargesAndPaymentsBuilder.Id);
            this.Container.RegisterDocumentSource<CrPaymentsBuilder>(PaymentDocumentType.Legal, CrPaymentsBuilder.Id);
            this.Container.RegisterDocumentSource<DocNumberBuilder>(PaymentDocumentType.Legal, DocNumberBuilder.Id);
            this.Container.RegisterDocumentSource<TariffAreaBuilder>(PaymentDocumentType.Legal, TariffAreaBuilder.Id);
            //this.Container.RegisterDocumentSource<DebtorInfoBuilder>(PaymentDocumentType.Legal, DebtorInfoBuilder.Id);
            this.Container.RegisterDocumentSource<LastPaymentDateBuilder>(PaymentDocumentType.Legal, LastPaymentDateBuilder.Id);

            //регистрация источников для юр лиц с реестром
            this.Container.RegisterDocumentSource<MainInfoBuilder>(PaymentDocumentType.RegistryLegal, MainInfoBuilder.Id);
            this.Container.RegisterDocumentSource<LegalInfoBuilder>(PaymentDocumentType.RegistryLegal, LegalInfoBuilder.Id);

            this.Container.RegisterDocumentSource<GroupedChargesAndPaymentsBuilder>(PaymentDocumentType.RegistryLegal, GroupedChargesAndPaymentsBuilder.Id);
            this.Container.RegisterDocumentSource<DocNumberBuilder>(PaymentDocumentType.RegistryLegal, DocNumberBuilder.Id);
            this.Container.RegisterDocumentSource<TariffAreaBuilder>(PaymentDocumentType.RegistryLegal, TariffAreaBuilder.Id);
            this.Container.RegisterDocumentSource<LastPaymentDateBuilder>(PaymentDocumentType.RegistryLegal, LastPaymentDateBuilder.Id);
        }
    }
}