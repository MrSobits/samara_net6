namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Report;
    using Entities;
    using Gkh.Utils;

    public class SubsidyList : GkhBaseReport
    {
        public SubsidyList()
            : base(new ReportTemplateBinary(Properties.Resources.Subsidy))
        {
        }

        public override string CodeForm
        {
            get { return "SubsidyList"; }
        }

        public override string Name
        {
            get { return "Субсидирование"; }
        }

        public override string Description
        {
            get { return "Субсидирование"; }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>();
        }

        public override string Id
        {
            get { return "SubsidyList"; }
        }

        protected override string CodeTemplate { get; set; }

        private long municipalityId;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var subsidyRecordVersionDomain = Container.Resolve<IDomainService<SubsidyRecordVersion>>();
            var programVersionDomain = Container.Resolve<IDomainService<ProgramVersion>>();

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermPeriod = config.ShortTermProgPeriod;

            var versionId = 
                programVersionDomain.GetAll()
                    .FirstOrDefault(x => x.IsMain && x.Municipality.Id == municipalityId)
                    .Return(x => x.Id);

            var versionData = subsidyRecordVersionDomain.GetAll()
                .Where(x => x.Version.Id == versionId)
                .Select(x => new
                {
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
                    x.BudgetCr,
                    x.CorrectionFinance,
                    IsShortTerm = (x.SubsidyYear <= periodStart + shortTermPeriod),
                    x.SaldoBallance,
                    // Выставляем признак является ли строка входяйщей в краткосрочный период
                    BalanceAfterCR = x.BalanceAfterCr,
                    IsSummary = false
                })
                .OrderBy(x => x.SubsidyYear)
                .ToList();

            if (!versionData.Any())
            {
                return;
            }

            var curYear = DateTime.Now.Year;
           
            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var i = 1;
            foreach (var record in versionData)
            {
                sect.ДобавитьСтроку();
                sect["Number"] = i;
                sect["SubsidyYear"] = record.SubsidyYear;    
                sect["PlanOwnerCollection"] = record.PlanOwnerCollection;
                sect["PlanOwnerPercent"] = record.PlanOwnerPercent;
                sect["NotReduceSizePercent"] = record.NotReduceSizePercent;
                sect["OwnerSumForCR"] = record.OwnerSumForCr;
                sect["BudgetRegion"] = record.BudgetRegion;
                sect["BudgetMunicipality"] = record.BudgetMunicipality;
                sect["BudgetFcr"] = record.BudgetFcr;
                sect["BudgetOtherSource"] = record.BudgetOtherSource;
                sect["SaldoBallance"] = record.SaldoBallance;
                sect["BudgetCr"] = record.BudgetCr;
                sect["CorrectionFinance"] = record.CorrectionFinance;
                sect["BalanceAfterCR"] = record.BalanceAfterCR;

                i++;
            }

            
            reportParams.SimpleReportParams["AllBudgetCr"] = versionData.Sum(x => x.BudgetCr) - versionData.Sum(x => x.BalanceAfterCR) + versionData.Select(x => x.BalanceAfterCR).LastOrDefault();
            reportParams.SimpleReportParams["AllCorrectionFinance"] = versionData.Sum(x => x.CorrectionFinance);
            reportParams.SimpleReportParams["AllBalanceAfterCR"] = versionData.Sum(x => x.BalanceAfterCR);
        }
    }
}