namespace Bars.Gkh.RegOperator.Controllers
{
    using Bars.Gkh.Domain.CollectionExtensions;
    using System.Threading;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.DataAccess;
    using B4.Modules.DataExport.Domain;

    using Bars.B4.IoC;
    using Entities;
    using Enums;
    using DomainModelServices;
    using DomainService;
    using Gkh.Domain;

    public class RealityObjectLoanController : B4.Alt.DataController<RealityObjectLoan>
    {
        private static readonly object syncRoot = new object();

        public RealityObjectLoanController(
            ILoanService loanService,
            IRealityObjectLoanRepayment repaymentService,
            ILoanSourceRepository loanSourceRepo)
        {
            this.loanService = loanService;
            this.repaymentService = repaymentService;
            this.loanSourceRepo = loanSourceRepo;
        }

        public ActionResult ListAvailableSources(BaseParams baseParams)
        {
            return this.loanSourceRepo.ListAvailableSources(baseParams).ToJsonResult();
        }

        public ActionResult ListLoansByProgramAndMunicipality(BaseParams baseParams)
        {
            var loanRepo = this.Container.Resolve<IRealityObjectLoanRepository>();
            using (this.Container.Using(loanRepo))
            {
                var result = loanRepo.ListRealtyObjectNeedLoan(baseParams);

                var proxyResult = result as ListRealtyObjectNeedLoanProxyResult;

                if (proxyResult == null)
                {
                    return result.ToJsonResult();
                }

                return new JsonNetResult(new
                {
                    success = true,
                    data = proxyResult.Data,
                    totalCount = proxyResult.TotalCount,
                    additionalData = proxyResult.AdditionalData
                });
            }
        }

        public ActionResult TakeLoan(BaseParams baseParams)
        {
            var loanType = baseParams.Params.GetAs<TypeLoanProcess>("typeLoanProcess");

            // Получаем кол-во уже созданных займов
            // для порядкового номера внутри печатки
            var loanCount = this.Container.ResolveDomain<RealityObjectLoan>().GetAll().SafeMax(x => x.DocumentNum);

            baseParams.Params.Add("loanCount", loanCount + 1);
            
            if (Monitor.TryEnter(RealityObjectLoanController.syncRoot))
            {
                try
                {
                    IDataResult result = new BaseDataResult();

                    if (loanType == TypeLoanProcess.Manual)
                    {
                        result = this.loanService.TakeLoanManually(baseParams);
                    }
                    else if (loanType == TypeLoanProcess.Auto)
                    {
                        result = this.loanService.TakeLoanAutomatically(baseParams);
                    }

                    return result.ToJsonResult();
                }
                finally
                {
                    Monitor.Exit(RealityObjectLoanController.syncRoot);
                }
            }

            return this.JsSuccess();
        }

        public ActionResult Repayment(BaseParams baseParams)
        {
            return this.repaymentService.Repayment(baseParams).ToJsonResult();
        }

        public ActionResult RepaymentAll(BaseParams baseParams)
        {
            return this.Js(this.repaymentService.RepaymentAll(baseParams));
        }

        public ActionResult GetRegoperatorSaldo(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAsId("muId");

            return this.JsSuccess(this.loanSourceRepo.GetRegoperatorSaldo(muId));
        }

        public ActionResult GetDisposal(BaseParams baseParams)
        {
            return new ReportStreamResult(this.loanService.DownloadDisposal(baseParams), "Файл.docx");
        }

        /// <summary>
        /// Экспортировать в Excel
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("RealityObjectLoanDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        private readonly ILoanService loanService;
        private readonly IRealityObjectLoanRepayment repaymentService;
        private readonly ILoanSourceRepository loanSourceRepo;
    }
}