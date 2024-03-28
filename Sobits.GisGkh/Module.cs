namespace Sobits.GisGkh
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.Utils;
    using Sobits.GisGkh.Controllers;
    using Sobits.GisGkh.DomainService;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.ExecutionAction;
    using Sobits.GisGkh.File;
    using Sobits.GisGkh.File.Impl;
    using Sobits.GisGkh.Interceptors;
    using Sobits.GisGkh.StateChange;
    using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
    using Sobits.GisGkh.ViewModel;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // настройки ограничений
            Container.RegisterTransient<IPermissionSource, PermissionMap>();

            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            Container.RegisterTransient<IGISGKHService, GISGKHService>();
            Container.RegisterTransient<IExportHouseDataService, ExportHouseDataService>();
            Container.RegisterTransient<IExportNsiListService, ExportNsiListService>();
            Container.RegisterTransient<IExportNsiItemsService, ExportNsiItemsService>();
            Container.RegisterTransient<IExportNsiPagingItemsService, ExportNsiPagingItemsService>();
            Container.RegisterTransient<IExportOrgRegistryService, ExportOrgRegistryService>();
            Container.RegisterTransient<IExportAccountDataService, ExportAccountDataService>();
            Container.RegisterTransient<IExportRegionalProgramService, ExportRegionalProgramService>();
            Container.RegisterTransient<IExportRegionalProgramWorkService, ExportRegionalProgramWorkService>();
            Container.RegisterTransient<IExportPlanService, ExportPlanService>();
            Container.RegisterTransient<IExportPlanWorkService, ExportPlanWorkService>();
            Container.RegisterTransient<IExportBriefApartmentHouseService, ExportBriefApartmentHouseService>();
            Container.RegisterTransient<IImportAccountDataService, ImportAccountDataService>();
            Container.RegisterTransient<IExportDecisionsFormingFundService, ExportDecisionsFormingFundService>();
            Container.RegisterTransient<IImportDecisionsFormingFundService, ImportDecisionsFormingFundService>();
            Container.RegisterTransient<IExportPaymentDocumentDataService, ExportPaymentDocumentDataService>();
            Container.RegisterTransient<IImportPaymentDocumentDataService, ImportPaymentDocumentDataService>();
            Container.RegisterTransient<IImportPlanService, ImportPlanService>();
            Container.RegisterTransient<IImportPlanWorkService, ImportPlanWorkService>();
            Container.RegisterTransient<IImportRegionalProgramService, ImportRegionalProgramService>();
            Container.RegisterTransient<IImportRegionalProgramWorkService, ImportRegionalProgramWorkService>();
            Container.RegisterTransient<IExportDataProviderNsiItemsService, ExportDataProviderNsiItemsService>();
            Container.RegisterTransient<IExportAppealService, ExportAppealService>();
            Container.RegisterTransient<IImportAnswerService, ImportAnswerService>();
            Container.RegisterTransient<IExportAppealCRService, ExportAppealCRService>();
            Container.RegisterTransient<IExportDRsService, ExportDRsService>();
            Container.RegisterTransient<IImportAnswerCRService, ImportAnswerCRService>();
            Container.RegisterTransient<IExportExaminationsService, ExportExaminationsService>();
            Container.RegisterTransient<IImportExaminationsService, ImportExaminationsService>();
            Container.RegisterTransient<IExportInspectionPlansService, ExportInspectionPlansService>();
            Container.RegisterTransient<IImportInspectionPlanService, ImportInspectionPlanService>();
            Container.RegisterTransient<IExportDecreesAndDocumentsDataService, ExportDecreesAndDocumentsDataService>();
            Container.RegisterTransient<IImportDecreesAndDocumentsDataService, ImportDecreesAndDocumentsDataService>();
            Container.RegisterTransient<IExportLicenseService, ExportLicenseService>();
            Container.RegisterTransient<IImportBuildContractService, ImportBuildContractService>();
            Container.RegisterTransient<IImportPerfWorkActService, ImportPerfWorkActService>();
            Container.RegisterTransient<Sobits.GisGkh.DomainService.IFileService, Sobits.GisGkh.DomainService.FileService>();

            Container.RegisterResources<ResourceManifest>();
            //Container.RegisterTransient<IPermissionSource, GisGkhPermissionMap>();
            //this.Container.RegisterImport<ExtractImport>();

            Container.RegisterTransient<IRuleChangeStatus, AppealChangeStateForGisGkhInWorkRule>();
            Container.RegisterTransient<IRuleChangeStatus, AppealChangeStateForGisGkhRollOverRule>();
            Container.RegisterTransient<IRuleChangeStatus, AppealChangeStateForGisGkhRedirectRule>();
            Container.RegisterTransient<IRuleChangeStatus, AppealChangeStateForGisGkhCloseRule>();
            Container.RegisterTransient<IRuleChangeStatus, CitizenSuggestionChangeStateForGisGkhCloseRule>();
            Container.RegisterTransient<IRuleChangeStatus, CitizenSuggestionChangeStateForGisGkhInWorkRule>();

            RegisterControllers();
            RegisterViewModels();
            RegisterExecutorTasks();
            RegisterInterceptors();
            RegisterTasks();
            RegisterServices();
            RegisterActions();
            RegisterBundlers();
        }

        private void RegisterControllers()
        {
            Container.RegisterController<GisGkhExecuteController>();
            Container.RegisterAltDataController<GisGkhRequests>();
            Container.RegisterAltDataController<NsiList>();
            Container.RegisterAltDataController<NsiItem>();
        }

        private void RegisterViewModels()
        {
            Container.RegisterViewModel<GisGkhRequests, GisGkhRequestsViewModel>();
            Container.RegisterViewModel<NsiList, NsiListViewModel>();
            Container.RegisterViewModel<NsiItem, NsiItemViewModel>();

        }

        private void RegisterExecutorTasks()
        {
        }
        private void RegisterInterceptors()
        {
            Container.RegisterDomainInterceptor<GisGkhRequests, GisGkhRequestsInterceptor>();
            Container.RegisterDomainInterceptor<NsiList, NsiListInterceptor>();
        }
        private void RegisterTasks()
        {
            Container.RegisterTaskExecutor<ProcessGisGkhAnswersTaskExecutor>(ProcessGisGkhAnswersTaskExecutor.Id);
            Container.RegisterTaskExecutor<ExportROTaskExecutor>(ExportROTaskExecutor.Id);
            Container.RegisterTaskExecutor<ImportBillsAccMassTaskExecutor>(ImportBillsAccMassTaskExecutor.Id);
            Container.RegisterTaskExecutor<ExportAccDataTaskExecutor>(ExportAccDataTaskExecutor.Id);
            Container.RegisterTaskExecutor<ImportAccDataTaskExecutor>(ImportAccDataTaskExecutor.Id);
            Container.RegisterTaskExecutor<ExportDecisionsFormingFundTaskExecutor>(ExportDecisionsFormingFundTaskExecutor.Id);
            Container.RegisterTaskExecutor<ExportLegalAccOwnersTaskExecutor>(ExportLegalAccOwnersTaskExecutor.Id);
            Container.RegisterTaskExecutor<ExportOrgTaskExecutor>(ExportOrgTaskExecutor.Id);
            Container.RegisterTaskExecutor<ImportPaymentDocumentsTaskExecutor>(ImportPaymentDocumentsTaskExecutor.Id);
            Container.RegisterTaskExecutor<ImportPersAccMassTaskExecutor>(ImportPersAccMassTaskExecutor.Id);
            Container.RegisterTaskExecutor<MatchAppealContragentTaskExecutor>(MatchAppealContragentTaskExecutor.Id);
            Container.RegisterTaskExecutor<ImportBuildContractTaskExecutor>(ImportBuildContractTaskExecutor.Id);
            Container.RegisterTaskExecutor<ImportPerfWorkActTaskExecutor>(ImportPerfWorkActTaskExecutor.Id);
        }
        private void RegisterServices()
        {
            this.Container.ReplaceTransient<IStatusPaymentDocumentHousesService, Bars.Gkh.RegOperator.DomainService.StatusPaymentDocumentHousesService,
                 Sobits.GisGkh.DomainService.StatusPaymentDocumentHousesService>();

            Container.RegisterSingleton<IFileUploader, SimpleFileUploader>("SimpleFileUploader");
            Container.RegisterSingleton<IFileUploader, FileUploader>("FileUploader");
            Container.RegisterSingleton<IFileDownloader, FileDownloader>();
        }
        private void RegisterActions()
        {
            this.Container.RegisterExecutionAction<ExportROAction>();
            this.Container.RegisterExecutionAction<ExportAccDataAction>();
            this.Container.RegisterExecutionAction<ExportDecisionsFormingFundAction>();
            this.Container.RegisterExecutionAction<ExportLegalAccOwnersAction>();
            this.Container.RegisterExecutionAction<ExportOrgAction>();
            this.Container.RegisterExecutionAction<ImportPersAccMassAction>();
            this.Container.RegisterExecutionAction<ImportBillsAccMassAction>();
            this.Container.RegisterExecutionAction<ImportAccDataAction>();
            this.Container.RegisterExecutionAction<MatchAppealContragentAction>();
            this.Container.RegisterExecutionAction<ImportPerfWorkActAction>();
            this.Container.RegisterExecutionAction<ImportBuildContractAction>();
        }
    }
}