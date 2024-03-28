using Bars.Gkh.Domain;

namespace Bars.Gkh.Regions.Voronezh
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.GisGkhRegional;
    using Bars.Gkh.Regions.Voronezh.DomainService;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Bars.Gkh.Regions.Voronezh.ExecutionAction.Impl;
    using Bars.Gkh.Regions.Voronezh.Tasks;
    using Bars.Gkh.Regions.Voronezh.ViewModel;
    using Bars.Gkh.RegOperator.Imports;
    using Bars.Gkh.RegOperator.ViewModels;
    using Bars.Gkh.Report;
    using Castle.MicroKernel.Registration;
    using Controllers;
    using Domain;
    using Entities;
    using Import;
    using Imports;
    using Interceptors;
    using Modules.ClaimWork.Entities;
    using RegOperator.Entities;
    using RegOperator.Entities.Owner;
    using Utils;

    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// Метод инициализации модуля
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();

            this.RegisterControllers();
            this.RegisterViewModel();
            this.RegisterServices();
            this.RegisterInterceptors();
            this.RegisterDomainServices();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("Voronezh menu");
            this.Container.RegisterImport<RosRegExtractImport>();
            RegisterExecuteActions();
            RegisterTaskExecutors();
            ReplaceComponents();

        }
        private void ReplaceComponents()
        {
            Container.ReplaceComponent<IGisGkhRegionalService>(
                typeof(Bars.Gkh.DomainService.GisGkhRegional.Impl.GisGkhRegionalService),
                Component.For<IGisGkhRegionalService>().ImplementedBy<Bars.Gkh.Regions.Voronezh.DomainService.GisGkhRegional.Impl.GisGkhRegionalService>().LifeStyle.Transient);
            Container.ReplaceTransient<IGkhImport, RegOperator.Imports.BankAccountStatementImport, Imports.BankAccountStatementImport>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterAltDataController<ZonalInspectionPrefix>();
            this.Container.RegisterController<RosRegExtractOperationsController>();
            this.Container.RegisterController<GenNumberController>();
            this.Container.RegisterController<RosRegExtractImportController>();
            this.Container.RegisterAltDataController<RosRegExtractDesc>();
            this.Container.RegisterAltDataController<RosRegExtractPers>();
            this.Container.RegisterAltDataController<RosRegExtractOrg>();
            this.Container.RegisterAltDataController<RosRegExtractGov>();
            this.Container.RegisterController<RosRegExtractViewController>();
            this.Container.RegisterController<DataAreaOwnerMergerController>();
            this.Container.RegisterController<ExtractPrinterController>();
            this.Container.RegisterController<ArchivedClaimWorkController>();
            this.Container.RegisterAltDataController<LawSuitDebtWorkSSP>();
            this.Container.RegisterAltDataController<LawSuitDebtWorkSSPDocument>();
            this.Container.RegisterAltDataController<LawsuitOwnerRepresentative>();
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterTransient<IRosRegExtractOperationsService, RosRegExtractOperationsService>();
        }

        private void RegisterViewModel()
        {
            this.Container.RegisterViewModel<ZonalInspectionPrefix, ZonalInspectionPrefixViewModel>();
            this.Container.RegisterViewModel<RosRegExtractDesc, RosRegExtractDescViewModel>();
            this.Container.RegisterViewModel<RosRegExtractPers, RosRegExtractPersViewModel>();
            this.Container.RegisterViewModel<RosRegExtractOrg, RosRegExtractOrgViewModel>();
            this.Container.RegisterViewModel<RosRegExtractGov, RosRegExtractGovViewModel>();

            this.Container.RegisterViewModel<LawSuitDebtWorkSSP, LawSuitDebtWorkSSPViewModel>();
            this.Container.RegisterViewModel<LawSuitDebtWorkSSPDocument, LawSuitDebtWorkSSPDocumentViewModel>();
            this.Container.RegisterViewModel<LawsuitOwnerRepresentative, LawsuitOwnerRepresentativeViewModel>();

            this.Container.ReplaceComponent<IPersonalAccountDistributionViewModel>(
       typeof(Bars.Gkh.RegOperator.ViewModels.Impl.PersonalAccountDistributionViewModel),
       Component.For<IPersonalAccountDistributionViewModel>().ImplementedBy<Bars.Gkh.Regions.Voronezh.ViewModels.Impl.VoronezhPersonalAccountDistributionViewModel>());
        }

        private void RegisterServices()
        {
            this.Container.RegisterService<IModifyEnumerableService, ModifyEnumerableService>();
            Container.Register(Component.For<IDomainService<LawSuitDebtWorkSSPDocument>>().ImplementedBy<FileStorageDomainService<LawSuitDebtWorkSSPDocument>>().LifeStyle.Transient);
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<Petition, PetitionInterceptor>();
            this.Container.RegisterDomainInterceptor<LawSuitDebtWorkSSP, LawSuitDebtWorkSSPInterceptor>();
            this.Container.RegisterDomainInterceptor<IndividualClaimWork,DebtorClaimWorkInterceptor<IndividualClaimWork>>();
            Container.RegisterDomainInterceptor<RosRegExtractDesc, RosRegExtractDescInterceptor>();
            Container.RegisterDomainInterceptor<Lawsuit, LawsuitInterceptor>();
            this.Container.RegisterDomainInterceptor<LawsuitOwnerInfo, LawsuitOwnerInfoInterceptor<LawsuitOwnerInfo>>();
            this.Container.RegisterDomainInterceptor<LawsuitIndividualOwnerInfo, LawsuitIndividualOwnerInfoInterceptor>();
        }

        private void RegisterExecuteActions()
        {
            this.Container.RegisterExecutionAction<SetSSPPaymentsAction>(); 
        }

        private void RegisterTaskExecutors()
        {
            this.Container.RegisterTaskExecutor<SetSSPPaymentsExecutor>(SetSSPPaymentsExecutor.Id);
        }
    }
}