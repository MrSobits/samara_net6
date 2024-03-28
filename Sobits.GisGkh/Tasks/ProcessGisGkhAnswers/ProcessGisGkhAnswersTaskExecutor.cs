using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Sobits.GisGkh.DomainService;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using System;
using System.Reflection;
using System.Threading;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    /// <summary>
    /// Задача на опрос и обработку ответов из смэв
    /// </summary>
    public class ProcessGisGkhAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        private IExportHouseDataService _ExportHouseDataService;

        private IExportOrgRegistryService _ExportOrgRegistryService;

        private IExportNsiListService _ExportNsiListService;

        private IExportNsiItemsService _ExportNsiItemsService;

        private IExportNsiPagingItemsService _ExportNsiPagingItemsService;

        private IExportDataProviderNsiItemsService _ExportDataProviderNsiItemsService;

        private IExportRegionalProgramService _ExportRegionalProgramService;

        private IExportRegionalProgramWorkService _ExportRegionalProgramWorkService;

        private IExportPlanService _ExportPlanService;

        private IExportPlanWorkService _ExportPlanWorkService;

        private IExportBriefApartmentHouseService _ExportBriefApartmentHouseService;

        private IExportAccountDataService _ExportAccountDataService;

        private IImportAccountDataService _ImportAccountDataService;

        private IExportDecisionsFormingFundService _ExportDecisionsFormingFundService;

        private IImportDecisionsFormingFundService _ImportDecisionsFormingFundService;

        private IExportPaymentDocumentDataService _ExportPaymentDocumentDataService;

        private IImportPaymentDocumentDataService _ImportPaymentDocumentDataService;

        private IImportPlanService _ImportPlanService;

        private IImportPlanWorkService _ImportPlanWorkService;
        private IImportRegionalProgramService _ImportRegionalProgramService;
        private IImportRegionalProgramWorkService _ImportRegionalProgramWorkService;

        private IExportAppealService _ExportAppealService;

        private IImportAnswerService _ImportAnswerService;

        private IExportAppealCRService _ExportAppealCRService;
        private IExportDRsService _ExportDRsService;

        private IImportAnswerCRService _ImportAnswerCRService;

        private IExportExaminationsService _ExportExaminationsService;

        private IImportExaminationsService _ImportExaminationsService;
        
        private IExportInspectionPlansService _ExportInspectionPlansService;

        private IImportInspectionPlanService _ImportInspectionPlanService;

        private IExportDecreesAndDocumentsDataService _ExportDecreesAndDocumentsDataService;

        private IImportDecreesAndDocumentsDataService _ImportDecreesAndDocumentsDataService;

        private IExportLicenseService _ExportLicenseService;

        private IImportBuildContractService _ImportBuildContractService;

        private IImportPerfWorkActService _ImportPerfWorkActService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ProcessGisGkhAnswersTaskExecutor(IExportHouseDataService ExportHouseDataService, IExportOrgRegistryService ExportOrgRegistryService,
            IExportNsiListService ExportNsiListService, IExportNsiItemsService ExportNsiItemsService, IExportNsiPagingItemsService ExportNsiPagingItemsService,
            IExportDataProviderNsiItemsService ExportDataProviderNsiItemsService,
            IExportRegionalProgramService ExportRegionalProgramService, IExportRegionalProgramWorkService ExportRegionalProgramWorkService, IExportPlanService ExportPlanService,
            IExportBriefApartmentHouseService ExportBriefApartmentHouseService, IExportAccountDataService ExportAccountDataService,
            IImportAccountDataService ImportAccountDataService, IExportDecisionsFormingFundService ExportDecisionsFormingFundService,
            IImportDecisionsFormingFundService ImportDecisionsFormingFundService, IImportPlanService ImportPlanService, IImportPlanWorkService ImportPlanWorkService,
            IExportPlanWorkService ExportPlanWorkService,
            IExportPaymentDocumentDataService ExportPaymentDocumentDataService, IImportPaymentDocumentDataService ImportPaymentDocumentDataService,
            IImportRegionalProgramService ImportRegionalProgramService, IImportRegionalProgramWorkService ImportRegionalProgramWorkService,
            IExportAppealService ExportAppealService, IImportAnswerService ImportAnswerService,
            IExportAppealCRService ExportAppealCRService, IImportAnswerCRService ImportAnswerCRService,
            IExportExaminationsService ExportExaminationsService, IImportExaminationsService ImportExaminationsService,
            IExportInspectionPlansService ExportInspectionPlansService, IImportInspectionPlanService ImportInspectionPlanService,
            IExportDecreesAndDocumentsDataService ExportDecreesAndDocumentsDataService, IImportDecreesAndDocumentsDataService ImportDecreesAndDocumentsDataService,
            IExportLicenseService ExportLicenseService, IExportDRsService drs, IImportBuildContractService ImportBuildContractService, IImportPerfWorkActService ImportPerfWorkActService
            )
        {
            _ExportHouseDataService = ExportHouseDataService;
            _ExportOrgRegistryService = ExportOrgRegistryService;
            _ExportNsiListService = ExportNsiListService;
            _ExportNsiItemsService = ExportNsiItemsService;
            _ExportNsiPagingItemsService = ExportNsiPagingItemsService;
            _ExportDataProviderNsiItemsService = ExportDataProviderNsiItemsService;
            _ExportRegionalProgramService = ExportRegionalProgramService;
            _ExportRegionalProgramWorkService = ExportRegionalProgramWorkService;
            _ExportPlanService = ExportPlanService;
            _ExportPlanWorkService = ExportPlanWorkService;
            _ExportBriefApartmentHouseService = ExportBriefApartmentHouseService;
            _ExportAccountDataService = ExportAccountDataService;
            _ImportAccountDataService = ImportAccountDataService;
            _ExportDecisionsFormingFundService = ExportDecisionsFormingFundService;
            _ImportDecisionsFormingFundService = ImportDecisionsFormingFundService;
            _ImportPlanService = ImportPlanService;
            _ImportPlanWorkService = ImportPlanWorkService;
            _ExportPaymentDocumentDataService = ExportPaymentDocumentDataService;
            _ImportPaymentDocumentDataService = ImportPaymentDocumentDataService;
            _ExportAppealService = ExportAppealService;
            _ImportAnswerService = ImportAnswerService;
            _ExportAppealCRService = ExportAppealCRService;
            _ImportAnswerCRService = ImportAnswerCRService;
            _ImportRegionalProgramService = ImportRegionalProgramService;
            _ImportRegionalProgramWorkService = ImportRegionalProgramWorkService;
            _ExportExaminationsService = ExportExaminationsService;
            _ImportExaminationsService = ImportExaminationsService;
            _ExportInspectionPlansService = ExportInspectionPlansService;
            _ImportInspectionPlanService = ImportInspectionPlanService;
            _ExportDecreesAndDocumentsDataService = ExportDecreesAndDocumentsDataService;
            _ImportDecreesAndDocumentsDataService = ImportDecreesAndDocumentsDataService;
            _ExportLicenseService = ExportLicenseService;
            _ExportDRsService = drs;
            _ImportBuildContractService = ImportBuildContractService;
            _ImportPerfWorkActService = ImportPerfWorkActService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            //var processLog = new List<string>();
            try
            {
                GisGkhRequests req = GisGkhRequestsDomain.Get(long.Parse((string)@params.Params["reqId"]));
                switch (req.TypeRequest)
                {
                    case GisGkhTypeRequest.exportNsiItems:
                        _ExportNsiItemsService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportNsiPagingItems:
                        _ExportNsiPagingItemsService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportDataProviderNsiItem:
                        _ExportDataProviderNsiItemsService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportNsiList:
                        _ExportNsiListService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportNsiRaoList:
                        _ExportNsiListService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportRegionalProgram:
                        _ExportRegionalProgramService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportRegionalProgramWork:
                        _ExportRegionalProgramWorkService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportPlan:
                        _ExportPlanService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportPlanWork:
                        _ExportPlanWorkService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportBriefApartmentHouse:
                        _ExportBriefApartmentHouseService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportHouseData:
                        _ExportHouseDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportOrgRegistry:
                        _ExportOrgRegistryService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportAccountData:
                        _ExportAccountDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importAccountData:
                        _ImportAccountDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportExaminations:
                        _ExportExaminationsService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importExaminations:
                        _ImportExaminationsService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importCurrentExaminations:
                        _ImportExaminationsService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportInspectionPlans:
                        _ExportInspectionPlansService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importInspectionPlan:
                        _ImportInspectionPlanService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportDecisionsFormingFund:
                        _ExportDecisionsFormingFundService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importDecisionsFormingFund:
                        _ImportDecisionsFormingFundService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportPaymentDocumentData:
                        _ExportPaymentDocumentDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importPaymentDocumentData:
                        _ImportPaymentDocumentDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importPlan:
                        _ImportPlanService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importPlanWork:
                        _ImportPlanWorkService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importRegionalProgram:
                        _ImportRegionalProgramService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importRegionalProgramWork:
                        _ImportRegionalProgramWorkService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportDecreesAndDocumentsData:
                        _ExportDecreesAndDocumentsDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importDecreesAndDocumentsData:
                        _ImportDecreesAndDocumentsDataService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportLicense:
                        _ExportLicenseService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportAppeal:
                        _ExportAppealService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importAnswer:
                        _ImportAnswerService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportAppealCR:
                        _ExportAppealCRService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importAnswerCR:
                        _ImportAnswerCRService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.exportDebtRequests:
                        _ExportDRsService.ProcessAnswer(req, true);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importDebtRequests:
                        _ExportDRsService.ProcessAnswer(req, false);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importBuildContract:
                        _ImportBuildContractService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                    case GisGkhTypeRequest.importPerfWorkAct:
                        _ImportPerfWorkActService.ProcessAnswer(req);
                        return new BaseDataResult(true, "Ответ успешно обработан");
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
            while (true);
        }

        #endregion

        #region Private methods

        #endregion
    }
}
