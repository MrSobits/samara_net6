namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using ViewModel;
    using Controllers;
    using Domain;
    using Entities;
    using Interceptors;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.Impl;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService;
    using Bars.B4.Modules.FileStorage;
    using Castle.MicroKernel.Registration;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.Gkh.Report;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Report;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Utils;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports;
    using Bars.Gkh.RegOperator.ViewModels;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.ReferenceCalculation;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.ReferenceCalculation.Impl;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterPermissionMap<PermissionMap>();
            this.Container.RegisterResourceManifest<ResourceManifest>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhRegopChelyabinsk navigation");
            this.RegisterControllers();
            this.RegisterDomainServices();
            this.RegisterViewModel();
            this.RegisterInterceptors();
            this.RegisterImports();

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<IGkhBaseReport, AgentPirLsReport>();
            Container.RegisterTransient<IGkhBaseReport, AgentPirDebtorsDocs>();
            Container.RegisterTransient<IGkhBaseReport, AgentPirDebtorReport>();
            Container.RegisterTransient<IGkhBaseReport, AgentPirOperationsByPeriodReport>();
        }

        private void RegisterControllers()
        {

            this.Container.RegisterAltDataController<LawSuitDebtWorkSSP>();
            this.Container.RegisterAltDataController<LawSuitDebtWorkSSPDocument>();
            this.Container.RegisterController<MassDebtWorkSSPController>();
            this.Container.RegisterController<RosRegExtractOperationsController>();
            this.Container.RegisterController<ArchivedClaimWorkController>();
            this.Container.RegisterAltDataController<AgentPIR>();
            this.Container.RegisterAltDataController<AgentPIRDebtor>();
            this.Container.RegisterAltDataController<DebtorReferenceCalculation>();
            this.Container.RegisterController<AgentPIRExecuteController>();
            this.Container.RegisterAltDataController<AgentPIRDocument>();
            this.Container.RegisterController<FileStorageDataController<AgentPIRDocument>>();
            this.Container.RegisterAltDataController<AgentPIRDebtorCredit>();
            this.Container.RegisterController<OrderingController>();
        }

        private void RegisterImports()
        {
            this.Container.RegisterImport<AgentPIRDocumentImport>(AgentPIRDocumentImport.Id);
            this.Container.RegisterImport<AgentPIRFileImport>(AgentPIRFileImport.Id);
            this.Container.RegisterImport<AgentPIRDebtorCreditImport>(AgentPIRDebtorCreditImport.Id);
            this.Container.RegisterImport<AgentPIRDutyImport>(AgentPIRDutyImport.Id);
            this.Container.RegisterImport<AgentPIRDebtorOrderingImport>(AgentPIRDebtorOrderingImport.Id);
        }

        private void RegisterInterceptors()
        {           
            this.Container.RegisterDomainInterceptor<LawSuitDebtWorkSSP, LawSuitDebtWorkSSPInterceptor>();          
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterTransient<IRosRegExtractOperationsService, RosRegExtractOperationsService>();
            this.Container.RegisterTransient<IAgentPIRExecuteService, AgentPIRExecuteService>();
            this.Container.Register(Component.For<IDomainService<AgentPIRDocument>>().ImplementedBy<FileStorageDomainService<AgentPIRDocument>>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDebtorService>(
          typeof(Gkh.RegOperator.DomainService.PersonalAccount.Debtor.Impl.DebtorService),
          Component.For<IDebtorService>().ImplementedBy<DomainService.PersonalAccount.Debtor.Impl.DebtorService>());

            this.Container.RegisterTransient<IDebtorReferenceCalculationService, DebtorEarlyCumulative>();
            this.Container.RegisterTransient<IDebtorReferenceCalculationService, DebtorEarlyMonthly>();
            this.Container.RegisterTransient<IDebtorReferenceCalculationService, DebtorLateCumylative>();
            this.Container.RegisterTransient<IDebtorReferenceCalculationService, DebtorLateMonthly>();
        }

        private void RegisterViewModel()
        {
            this.Container.RegisterViewModel<LawSuitDebtWorkSSP, LawSuitDebtWorkSSPViewModel>();
            this.Container.RegisterViewModel<LawSuitDebtWorkSSPDocument, LawSuitDebtWorkSSPDocumentViewModel>();
            this.Container.RegisterViewModel<AgentPIR, AgentPIRViewModel>();
            this.Container.RegisterViewModel<AgentPIRDebtor, AgentPIRDebtorViewModel>();
            this.Container.RegisterViewModel<DebtorReferenceCalculation, DebtorReferenceCalculationViewModel>();
            this.Container.RegisterViewModel<AgentPIRDocument, AgentPIRDocumentViewModel>();
            this.Container.RegisterViewModel<AgentPIRDebtorCredit, AgentPIRDebtorCreditViewModel>();

            this.Container.ReplaceComponent<IPersonalAccountDistributionViewModel>(
         typeof(Bars.Gkh.RegOperator.ViewModels.Impl.PersonalAccountDistributionViewModel),
         Component.For<IPersonalAccountDistributionViewModel>().ImplementedBy<Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModels.Impl.ChelyabinskPersonalAccountDistributionViewModel>());
        }
    }
}