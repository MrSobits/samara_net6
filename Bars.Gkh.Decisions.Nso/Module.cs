namespace Bars.Gkh.Decisions.Nso
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NHibernateChangeLog;

    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Decisions.Nso.CollectionFilters;
    using Bars.Gkh.Decisions.Nso.Controllers;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Domain.Decision.Impl;
    using Bars.Gkh.Decisions.Nso.Domain.Decisions;
    using Bars.Gkh.Decisions.Nso.Domain.Impl;
    using Bars.Gkh.Decisions.Nso.DomainServices;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Decisions.Nso.ExecutionAction;
    using Bars.Gkh.Decisions.Nso.Interceptors;
    using Bars.Gkh.Decisions.Nso.LogMap.Provider;
    using Bars.Gkh.Decisions.Nso.Permissions;
    using Bars.Gkh.Decisions.Nso.Report;
    using Bars.Gkh.Decisions.Nso.States;
    using Bars.Gkh.Decisions.Nso.ViewModel;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Navigation;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;
    using Overhaul.Domain;
    using Overhaul.Domain.Impl;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.RegisterAuditLogMap();

            this.RegisterControllers();

            this.RegisterCommonComponents();

            this.RegisterCustomQueryFilters();

            this.RegisterDomainServices();

            this.RegisterInterceptors();

            this.RegisterNavigations();

            this.RegisterReports();

            this.RegisterServices();

            this.RegisterStateRoutines();

            this.RegisterViewModels();

            this.RegisterExecutionActions();

            this.RegisterDataProviders();

            this.RegisterFormatDataExport();
        }

        private void RegisterDataProviders()
        {
            this.Container.RegisterTransient<ITypeOfFormingCrProvider, IRealityObjectDecisionProtocolProxyService, RealityObjectDecisionProtocolProxyService>();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<RecreateProtocolFileForDecisionNotificationsAction>();
        }

        private void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<DecisionStraightForwardController>();
            this.Container.RegisterController<DecisionNotificationController>();
            this.Container.RegisterAltDataController<MonthlyFeeAmountDecHistory>();
            this.Container.RegisterController<MonthlyFeeAmountDecisionController>();
            this.Container.RegisterController<FileStorageDataController<RealityObjectDecisionProtocol>>();
            this.Container.RegisterController<DecisionController>();
            this.Container.RegisterAltDataController<GenericDecision>();
            this.Container.RegisterController<FileStorageDataController<GovDecision>>();
        }

        private void RegisterCommonComponents()
        {
            this.Container.Register(
                Component.For<IModuleDependencies>()
                    .Named(string.Format("{0} dependencies", this.AssemblyName))
                    .LifeStyle.Singleton.UsingFactoryMethod(
                        () => new ModuleDependencies(this.Container).Init()));
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<DecisionsNsoPermissionMap>());
            this.Container.Register(Component.For<IFieldRequirementSource>().ImplementedBy<DecisionsNsoFieldRequirementMap>());
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IMenuModificator, NsoOverhaulMenuModificator>(RealityObjMenuKey.Key);
        }

        private void RegisterCustomQueryFilters()
        {
            this.Container.RegisterTransient<ICustomQueryFilter<Contragent>, ContragentFilter>();
            this.Container.RegisterTransient<ICustomQueryFilter<ManagingOrganization>, ManOrgFilter>();
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterDomainService<DecisionNotification, FileStorageDomainService<DecisionNotification>>();
            this.Container.RegisterDomainService<RealityObjectDecisionProtocol, FileStorageDomainService<RealityObjectDecisionProtocol>>();
            this.Container.RegisterDomainService<GovDecision, FileStorageDomainService<GovDecision>>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<GenericDecision, GenericDecisionInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectDecisionProtocol, RealityObjectDecisionProtocolInterceptor>();
            this.Container.RegisterDomainInterceptor<DecisionNotification, DecisionNotificationInterceptor>();
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IPrintForm, RepairAbovePaymentReport>("RepairAbovePaymentReport");
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IRealityObjectDecisionProtocolService, RealityObjectDecisionProtocolService>();
            this.Container.RegisterTransient<IRealityObjectDecisionsService, RealityObjectDecisionService>();
            this.Container.RegisterTransient<IMonthlyFeeAmountDecisionService, MonthlyFeeAmountDecisionService>();
            this.Container.RegisterTransient<IUltimateDecisionService, UltimateDecisionService>();

            this.Container.RegisterTransient<IDecisionHistoryService, DecisionHistoryService>();

            this.Container.RegisterTransient<IDecisionNotificationService, DecisionNotificationService>();

            this.Container.RegisterTransient<IRobjectDecisionService, RobjectDecisionService>();

            this.Container.ReplaceTransient<IPaysizeRepository, PaysizeRepository, PaysizeRepositoryDecisions>();

            this.Container.RegisterTransient<IDecisionStraightForwardService, DecisionStraightForwardService>();

            this.Container.RegisterTransient<ILogMonthlyFeeAmountDecisionService, LogMonthlyFeeAmountDecisionServiceService>();

        }

        private void RegisterStateRoutines()
        {
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>();
            this.Container.RegisterTransient<IRuleChangeStatus, DecisionNotificationStatetransferRule>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<RealityObjectDecisionProtocol, RealityObjectDecisionProtocolViewModel>();
            this.Container.RegisterViewModel<GenericDecision, RealityObjectGenericDecisionViewModel>();
            this.Container.RegisterViewModel<GovDecision, GovDecisionViewModel>();

            this.Container.RegisterViewModel<MonthlyFeeAmountDecHistory, MonthlyFeeAmountDecHistoryViewModel>();
        }

        private void RegisterFormatDataExport()
        {
        }
    }
}