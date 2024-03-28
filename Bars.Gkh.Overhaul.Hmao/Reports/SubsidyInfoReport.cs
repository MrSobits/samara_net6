using Bars.Gkh.Domain;
using Bars.Gkh.Domain.CollectionExtensions;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Castle.Windsor;
    using Entities;
    using Gkh.Utils;
    using BasePrintForm = B4.Modules.Reports.BasePrintForm;

    public class SubsidyInfoReport : BasePrintForm
    {
        public SubsidyInfoReport()
            : base(new ReportTemplateBinary(Properties.Resources.SubsidyInfo))
        {
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        public override string Name
        {
            get { return "Информация о субсидировании по МО"; }
        }

        public override string Desciption
        {
            get { return "Информация о субсидировании по МО"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.SubsidyInfo"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.SubsidyInfo"; }
        }

        private long[] municipalityIds = new long[0];


        private int year = 0;

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityIds = baseParams.Params.GetAs<string>("muIds").ToLongArray();

            year = baseParams.Params.GetAs("year", 0);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermPeriod = config.ShortTermProgPeriod;

            var isShortTerm = year < periodStart + shortTermPeriod;

           var versionIds = ProgramVersionDomain.GetAll()
                                .Where(x => x.IsMain)
                                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                                .Select(x => x.Id)
                                .ToList();

           var result = SubsidyRecordVersionDomain.GetAll()
                                                    .Where(x => versionIds.Contains(x.Version.Id))
                                                    .WhereIf(year > 0, x => x.SubsidyRecord.SubsidyYear == year)
                                                    .Select(x => new {
                                                           x.BudgetCr,
                                                           x.CorrectionFinance,
                                                           x.BalanceAfterCr,
                                                           x.Id,
                                                           x.SubsidyYear,
                                                           x.BudgetRegion,
                                                           x.BudgetMunicipality,
                                                           x.BudgetFcr,
                                                           x.BudgetOtherSource,
                                                           x.PlanOwnerCollection,
                                                           x.PlanOwnerPercent,
                                                           x.NotReduceSizePercent,
                                                           x.OwnerSumForCr,
                                                           MuName = x.Version.Municipality.Name
                                                        })
                                                    .OrderBy(x => x.MuName)
                                                    .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var number = 1;

            foreach (var rec in result)
            {
                section.ДобавитьСтроку();

                section["Number"] = number++;
                section["Municipality"] = rec.MuName;
                section["PlanOwnerCollection"] = isShortTerm ? rec.PlanOwnerCollection.ToStr() : " - ";
                section["PlanOwnerPercent"] = isShortTerm ? rec.PlanOwnerPercent.ToStr() : " - ";
                section["NotReduceSizePercent"] = isShortTerm ? rec.NotReduceSizePercent.ToStr() : " - ";
                section["OwnerSumForCr"] = rec.OwnerSumForCr;
                section["BudgetRegion"] = isShortTerm ? rec.BudgetRegion.ToStr() : " - ";
                section["BudgetMu"] = isShortTerm ? rec.BudgetMunicipality.ToStr() : " - ";
                section["BudgetFcr"] = isShortTerm ? rec.BudgetFcr.ToStr() : " - ";
                section["BudgetOtherSrc"] = isShortTerm ? rec.BudgetOtherSource.ToStr() : " - ";
                section["BudgetCr"] = rec.BudgetCr;
                section["CorrectionFinance"] = rec.CorrectionFinance;
                section["BalanceAfterCr"] = rec.BalanceAfterCr;
            }

            reportParams.SimpleReportParams["YearProgram"] = year;
            reportParams.SimpleReportParams["TotalPlanOwnerCollection"] = isShortTerm ? result.SafeSum(x => x.PlanOwnerCollection).ToStr() : " - ";
            reportParams.SimpleReportParams["TotalOwnerSumForCr"] = result.SafeSum(x => x.OwnerSumForCr);
            reportParams.SimpleReportParams["TotalBudgetRegion"] = isShortTerm ? result.SafeSum(x => x.BudgetRegion).ToStr() : " - ";
            reportParams.SimpleReportParams["TotalBudgetMu"] = isShortTerm ? result.SafeSum(x => x.BudgetMunicipality).ToStr() : " - ";
            reportParams.SimpleReportParams["TotalBudgetFcr"] = isShortTerm ? result.SafeSum(x => x.BudgetFcr).ToStr() : " - ";
            reportParams.SimpleReportParams["TotalBudgetOtherSrc"] = isShortTerm ? result.SafeSum(x => x.BudgetOtherSource).ToStr() : " - ";
            reportParams.SimpleReportParams["TotalBudgetCr"] = result.SafeSum(x => x.BudgetCr);
            reportParams.SimpleReportParams["TotalCorrectionFinance"] = result.SafeSum(x => x.CorrectionFinance);
            reportParams.SimpleReportParams["TotalBalanceAfterCr"] = result.SafeSum(x => x.BalanceAfterCr);
        }
    }
}