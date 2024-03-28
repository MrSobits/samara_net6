namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Properties;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;

    using Entities;

    public class SubsidyList : GkhBaseReport
    {
        public SubsidyList()
            : base(new ReportTemplateBinary(Resources.Subsidy))
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
            var domainService = Container.Resolve<IDomainService<SubsidyRecord>>();

            var versionId = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain).Return(x => x.Id);

            var versionData = subsidyRecordVersionDomain.GetAll()
                                                        .Where(x => x.Version.Id == versionId)
                                                        .ToList();
            if (!versionData.Any())
            {
                return;
            }

            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            // Начало периода
            var periodStart = config.ProgrammPeriodStart;

            // Количество лет краткосрочной программы
            var shortTermPeriod = config.ShortTermProgPeriod;

            var data = domainService.GetAll()
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
                    x.OwnerSumForCr
                })
                .AsEnumerable()
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
                    BudgetCr = versionData.FirstOrDefault(y => y.SubsidyRecord.Id == x.Id).Return(y => y.BudgetCr),
                    CorrectionFinance = versionData.FirstOrDefault(y => y.SubsidyRecord.Id == x.Id).Return(y => y.CorrectionFinance),
                    IsShortTerm = (x.SubsidyYear < periodStart + shortTermPeriod), // Выставляем признак является ли строка входяйщей в краткосрочный период
                    BalanceAfterCr = versionData.FirstOrDefault(y => y.SubsidyRecord.Id == x.Id).Return(y => y.BalanceAfterCr),
                    IsSummary = false
                })
                .OrderBy(x => x.SubsidyYear)
                .ToList();

            var curYear = DateTime.Now.Year;
           
            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var i = 1;
            foreach (var record in data)
            {
                sect.ДобавитьСтроку();
                sect["Number"] = i;
                sect["SubsidyYear"] = record.SubsidyYear;

                if (record.SubsidyYear < curYear + shortTermPeriod)
                {
                    sect["PlanOwnerCollection"] = record.PlanOwnerCollection;
                    sect["PlanOwnerPercent"] = record.PlanOwnerPercent;
                    sect["NotReduceSizePercent"] = record.NotReduceSizePercent;
                    sect["OwnerSumForCR"] = record.OwnerSumForCr;
                    sect["BudgetRegion"] = record.BudgetRegion;
                    sect["BudgetMunicipality"] = record.BudgetMunicipality;
                    sect["BudgetFcr"] = record.BudgetFcr;
                    sect["BudgetOtherSource"] = record.BudgetOtherSource;
                }

                sect["BudgetCr"] = record.BudgetCr;
                sect["CorrectionFinance"] = record.CorrectionFinance;
                sect["BalanceAfterCR"] = record.BalanceAfterCr;

                i++;
            }

            reportParams.SimpleReportParams["AllBudgetCr"] = data.Sum(x => x.BudgetCr);
            reportParams.SimpleReportParams["AllCorrectionFinance"] = data.Sum(x => x.CorrectionFinance);
            reportParams.SimpleReportParams["AllBalanceAfterCR"] = data.Sum(x => x.BalanceAfterCr);
        }
    }
}