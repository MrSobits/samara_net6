namespace Bars.Gkh.ClaimWork
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.States;
    using Bars.B4.ResourceBundling;
    using Bars.B4.Windsor;
    using Bars.Gkh.B4Events;
    using Bars.Gkh.ClaimWork.CodedReports;
    using Bars.Gkh.ClaimWork.CodedReports.ReportHandler;
    using Bars.Gkh.ClaimWork.Controllers;
    using Bars.Gkh.ClaimWork.Controllers.Document;
    using Bars.Gkh.ClaimWork.DocCreateRules;
    using Bars.Gkh.ClaimWork.DomainService;
    using Bars.Gkh.ClaimWork.DomainService.Document;
    using Bars.Gkh.ClaimWork.DomainService.Document.Impl;
    using Bars.Gkh.ClaimWork.DomainService.DocumentRegister;
    using Bars.Gkh.ClaimWork.DomainService.DocumentRegister.Impl;
    using Bars.Gkh.ClaimWork.DomainService.Impl;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.ClaimWork.ExecutionAction;
    using Bars.Gkh.ClaimWork.Exports;
    using Bars.Gkh.ClaimWork.Formulas;
    using Bars.Gkh.ClaimWork.Interceptors;
    using Bars.Gkh.ClaimWork.Services;
    using Bars.Gkh.ClaimWork.Services.Impl;
    using Bars.Gkh.ClaimWork.ViewModel;
    using Bars.Gkh.ClaimWork.ViewModel.Dict;
    using Bars.Gkh.ClaimWork.ViewModel.LawsuitClw;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Controllers.Document;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Bars.Gkh.Modules.ClaimWork.DomainService.ReportHandler;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.ViewModel;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterResourceManifest<ResourceManifest>("GkhClaimWork resources");
            this.Container.RegisterPermissionMap<PermissionMap>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhClaimWork navigation");
            this.Container.RegisterTransient<IPermissionSource, ClaimWorkPermissionSource>();

            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Gkh.ClaimWork statefulentity");
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            
            this.RegisterController();

            this.RegisterViewModel();

            this.RegisterDomainService();

            this.RegisterInterceptors();

            this.RegisterDocCreateRules();

            this.RegisterCodedReports();

            this.RegisterServices();

            this.RegisterExports();

            this.RegisterFormulaParameters();

            this.RegisterBundlers();

            this.RegisterExecutionActions();

            ApplicationContext.Current.Events.GetEvent<GkhInitializedEvent>().Subscribe(
                _ =>
                {
                    var provider = this.Container.Resolve<IGkhConfigProvider>();
                    provider.Get<ClaimWorkConfig>().Enabled = true;
                    provider.SaveChanges();
                });
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<SetPenaltyFormulaAction>();
        }

        private void RegisterBundlers()
        {
            var bundler = this.Container.Resolve<IResourceBundler>();
            bundler.RegisterCssBundle("b4-all", "~/content/css/b4ClwMain.css");
        }

        private void RegisterFormulaParameters()
        {
            this.Container.RegisterFormulaParameter<DebtAmountParameter>("StateDuty", DebtAmountParameter.Id);
            this.Container.RegisterFormulaParameter<PenaltyAmountParameter>("StateDuty", PenaltyAmountParameter.Id);
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, ActViolIdentificationExport>("ActViolIdentificationExport");
            this.Container.RegisterTransient<IDataExportService, NotificationExport>("NotificationExport");
            this.Container.RegisterTransient<IDataExportService, PretensionExport>("PretensionExport");
            this.Container.RegisterTransient<IDataExportService, LawsuitExport>("LawsuitExport");
        }

        private void RegisterController()
        {
            this.Container.RegisterController<DocumentRegisterController>();
            this.Container.RegisterController<PretensionClwController>();
            this.Container.RegisterController<ActViolIdentificationClwController>();
            this.Container.RegisterController<NotificationClwController>();
            this.Container.RegisterController<LawsuitController>();
            this.Container.RegisterController<PetitionController>();
            this.Container.RegisterController<CourtOrderClaimController>();
            this.Container.RegisterController<FileStorageDataController<LawsuitClwCourt>>();
            this.Container.RegisterController<FileStorageDataController<LawsuitClwDocument>>();
            this.Container.RegisterAltDataController<ViolClaimWork>();
            this.Container.RegisterFileStorageDataController<LawsuitDocument>();
            this.Container.RegisterController<MenuClaimWorkController>();
            this.Container.RegisterController<MenuBuildContractClaimWorkController>();
            this.Container.RegisterController<JurInstitutionController>();
            this.Container.RegisterAltDataController<JurInstitutionRealObj>();
            this.Container.RegisterAltDataController<PetitionToCourtType>();
            this.Container.RegisterAltDataController<StateDuty>();           
            this.Container.RegisterController<JurJournalController>();
            this.Container.RegisterAltDataController<StateDutyPetition>();
            ContainerHelper.RegisterFileDataController<RestructDebt>();
        }

        private void RegisterViewModel()
        {
            this.Container.RegisterViewModel<LawsuitClwCourt, LawsuitClwCourtViewModel>();
            this.Container.RegisterViewModel<LawsuitClwDocument, LawsuitClwDocumentViewModel>();
            this.Container.RegisterViewModel<JurInstitution, JurInstitutionViewModel>();
            this.Container.RegisterViewModel<JurInstitutionRealObj, JurInstitutionRealObjViewModel>();
            this.Container.RegisterViewModel<PetitionToCourtType, PetitionToCourtViewModel>();
            this.Container.RegisterViewModel<StateDuty, StateDutyViewModel>();
            this.Container.RegisterViewModel<StateDutyPetition, DutyPetitionViewModel>();
            this.Container.RegisterViewModel<ActViolIdentificationClw, ActViolIdentificationViewModel>();
            this.Container.RegisterViewModel<NotificationClw, NotificationViewModel>();
            this.Container.RegisterViewModel<Petition, PetitionViewModel>();
            this.Container.RegisterViewModel<LawsuitDocument, LawsuitDocumentViewModel>();
            this.Container.RegisterViewModel<RestructDebt, RestructDebtViewModel>();
            this.Container.RegisterViewModel<FlattenedClaimWork, FlattenedClaimWorkViewModel>();
        }

        private void RegisterDomainService()
        {
            this.Container.RegisterDomainService<NotificationClw, FileStorageDomainService<NotificationClw>>();
            this.Container.RegisterDomainService<PretensionClw, FileStorageDomainService<PretensionClw>>();
            this.Container.RegisterDomainService<LawsuitClwCourt, FileStorageDomainService<LawsuitClwCourt>>();
            this.Container.RegisterDomainService<LawsuitClwDocument, FileStorageDomainService<LawsuitClwDocument>>();
            this.Container.RegisterDomainService<Petition, FileStorageDomainService<Petition>>();
            this.Container.RegisterDomainService<CourtOrderClaim, FileStorageDomainService<CourtOrderClaim>>();
            this.Container.RegisterDomainService<Lawsuit, FileStorageDomainService<Lawsuit>>();
            this.Container.RegisterFileStorageDomainService<LawsuitDocument>();

            this.Container.RegisterTransient<IClwReportExportHandler, RegopPetitionExportHandler>();
            this.Container.RegisterTransient<IClwReportExportHandler, RegopCourtOrderClaimExportHandler>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<Petition, LawsuitInterceptor<Petition>>();
            this.Container.RegisterDomainInterceptor<CourtOrderClaim, LawsuitInterceptor<CourtOrderClaim>>();
            this.Container.RegisterDomainInterceptor<Lawsuit, LawsuitInterceptor<Lawsuit>>();
            this.Container.RegisterDomainInterceptor<PretensionClw, PretensionInterceptor>();
            this.Container.RegisterDomainInterceptor<NotificationClw, NotificationInterceptor>();
            this.Container.RegisterDomainInterceptor<ActViolIdentificationClw, ActViolIdentificationInterceptor>();
            this.Container.RegisterDomainInterceptor<JurInstitution, JurInstitutionInterceptor>();
            this.Container.RegisterDomainInterceptor<StateDuty, StateDutyInterceptor>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IClaimWorkReportService, ClaimWorkReportService>();
            this.Container.RegisterTransient<IJurInstitutionService, JurInstitutionService>();
            this.Container.RegisterTransient<IJurJournalService, JurJournalService>();
            this.Container.RegisterTransient<IDocumentRegisterService, DocumentRegisterService>();
            this.Container.RegisterTransient<IDocumentRegisterType, DocumentRegisterActViolIdentification>();
            this.Container.RegisterTransient<IActViolIdentificationService, ActViolIdentificationService>();
            this.Container.RegisterTransient<IDocumentRegisterType, DocumentRegisterNotification>();
            this.Container.RegisterTransient<INotificationService, NotificationService>();
            this.Container.RegisterTransient<IDocumentRegisterType, DocumentRegisterPretension>();
            this.Container.RegisterTransient<IPretensionService, PretensionService>();
            this.Container.RegisterTransient<IDocumentRegisterType, DocumentRegisterLawsuit>();
            this.Container.RegisterTransient<ILawsuitService, LawsuitService>();
        }

        private void RegisterDocCreateRules()
        {
            this.Container.RegisterTransient<IClaimWorkDocRule, NotificationRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, ActViolIdentificationRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, PretensionRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, LawSuitRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, CourtOrderClaimRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, RestructDebtRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, RestructDebtAmicArgRule>();
        }

        private void RegisterCodedReports()
        {
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<NotificationReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<PetitionReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<PretensionBuilderReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<PretensionReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitLegalAccountReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitDeclarationReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitDeclaration185Report>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<LawSuitDeclaration512Report>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<RestructDebtReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<RestructDebtAmicAgrReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<ApplicationToRenounceClaimsReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<OperManPretensionReport>().LifestyleTransient());
            this.Container.Register(Component.For<ICodedReport, IClaimWorkCodedReport>().ImplementedBy<OperManLawSuitLegalAccountReport>().LifestyleTransient());
        }
    }
}
