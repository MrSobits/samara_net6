namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Castle.Windsor;
    using Entities;

    public class MkdLoanReport : BasePrintForm
    {
        public MkdLoanReport()
            : base(new ReportTemplateBinary(Properties.Resources.MkdLoanReport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectLoan> RealityObjectLoan { get; set; }

        public IDomainService<ContragentContact> ContrAgent { get; set; }


        // Входящие параметры
        private int loanStatus;

        private long[] programCrIds;

        private long[] municipalityIds;

        public override string Name
        {
            get
            {
                return "Отчет по займам в разрезе МКД";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по займам в разрезе МКД";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.MkdLoanReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhRegOp.MkdLoanReport";
            }
        }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            loanStatus = baseParams.Params.GetAs<int>("loanStatus");

            programCrIds = baseParams.Params.GetAs("programCrIds", string.Empty).ToLongArray();

            municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var loans =
                RealityObjectLoan.GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.LoanTaker.RealityObject.Municipality.Id))
                    .WhereIf(programCrIds.Length > 0, x => programCrIds.Contains(x.ProgramCr.Id))
                    .WhereIf(loanStatus != 0, x => x.State.Id == loanStatus)
                    .Select(x => new
                    {
                        x.Id,
                        Status = x.State.Name,
                        x.LoanDate,
                        ShortTermProgram = x.ProgramCr.Name,
                        LoanReceiver = x.LoanTaker.RealityObject.Address,
                        x.LoanSum,
                        x.LoanReturnedSum,
                        DebtSum = x.LoanSum - x.LoanReturnedSum,
                        x.FactEndDate
                    })
                    .OrderBy(x => x.Status)
                    .ThenBy(x => x.LoanDate)
                    .ThenBy(x => x.ShortTermProgram)
                    .AsEnumerable()
                    .Distinct(x => x.Id);

            reportParams.SimpleReportParams["CreationDate"] = DateTime.Now.ToShortDateString();

            reportParams.SimpleReportParams["Chief"] = ContrAgent.GetAll()
                .Where(x => x.Position.Code == "1")
                .Select(x => x.FullName)
                .FirstOrDefault();

            reportParams.SimpleReportParams["Accountant"] = ContrAgent.GetAll()
                .Where(x => x.Position.Code == "2")
                .Select(x => x.FullName)
                .FirstOrDefault();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СписокЗаймов");

            foreach (var loan in loans)
            {
                section.ДобавитьСтроку();
                section["Status"] = loan.Status;
                section["LoadDate"] = loan.LoanDate;
                section["ShortTermProgram"] = loan.ShortTermProgram;
                section["LoanReceiver"] = loan.LoanReceiver;
                section["LoanSum"] = loan.LoanSum;
                section["LoanReturnedSum"] = loan.LoanReturnedSum;
                section["DebtSum"] = loan.DebtSum;
                section["FactEndDate"] = loan.FactEndDate;
            }
        }
    }
}