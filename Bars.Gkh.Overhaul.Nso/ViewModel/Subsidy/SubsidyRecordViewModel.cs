namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Entities;
    using Gkh.Utils;

    public class SubsidyRecordViewModel : BaseViewModel<SubsidyRecord>
    {
        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<SubsidyRecord> SubsidyRecordDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        public ISubsidyRecordService SubsidyRecordService { get; set; }

        public override IDataResult List(IDomainService<SubsidyRecord> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var versionId = ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain).Return(x => x.Id);

            var versionData = SubsidyRecordVersionDomain.GetAll()
                                                        .Where(x => x.Version.Id == versionId)
                                                        .ToList();

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
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
                .Filter(loadParam, Container)
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
                    CorrectionFinance =
                        versionData.FirstOrDefault(y => y.SubsidyRecord.Id == x.Id).Return(y => y.CorrectionFinance),
                    IsShortTerm = (x.SubsidyYear < periodStart + shortTermPeriod),
                    // Выставляем признак является ли строка входяйщей в краткосрочный период
                    BalanceAfterCr =
                        versionData.FirstOrDefault(y => y.SubsidyRecord.Id == x.Id).Return(y => y.BalanceAfterCr),
                    IsSummary = false
                })
                .OrderBy(x => x.SubsidyYear)
                .ToList();

            var rec = new
            {
                Id = 0L,
                SubsidyYear = 0,
                BudgetRegion = 0m,
                BudgetMunicipality = 0m,
                BudgetFcr = 0m,
                BudgetOtherSource = 0m,
                PlanOwnerCollection = data.Where(x => x.IsShortTerm).Sum(x => x.PlanOwnerCollection),
                PlanOwnerPercent = 0m,
                NotReduceSizePercent = 0m,
                OwnerSumForCr = data.Sum(x => x.OwnerSumForCr),
                BudgetCr = data.Sum(x => x.BudgetCr),
                CorrectionFinance = data.Sum(x => x.CorrectionFinance),
                IsShortTerm = false, // Выставляем признак является ли строка входяйщей в краткосрочный период
                BalanceAfterCr = data.Sum(x => x.BalanceAfterCr),
                IsSummary = true
            };

            data.Add(rec);

            return new ListDataResult(data, data.Count());
        }
    }
}