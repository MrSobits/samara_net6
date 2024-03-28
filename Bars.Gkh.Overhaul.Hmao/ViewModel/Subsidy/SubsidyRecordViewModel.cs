namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    
    using Entities;
    using Gkh.Utils;

    public class SubsidyRecordVersionViewModel : BaseViewModel<SubsidyRecordVersion>
    {
        public override IDataResult List(IDomainService<SubsidyRecordVersion> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var moId = 0M;
            var versionId = loadParam.Filter.GetAs<long>("versionId");
            if (versionId == 0)
            {
                moId = baseParams.Params.GetAs<long>("mo_id");
            }

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var shortTermPeriod = config.ShortTermProgPeriod;

            var data = domainService.GetAll()
                .WhereIf(versionId > 0, x => x.Version.Id == versionId)
                .WhereIf(moId > 0, x => x.Version.Municipality.Id == moId && x.Version.IsMain)
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
                    IsShortTerm = true,
                    x.BalanceAfterCr,
                    IsSummary = false,
                    x.Version,
                    x.SubsidyRecord,
                    x.DateCalcOwnerCollection,
                    x.SaldoBallance,
                    x.AdditionalExpences
                })
                .Filter(loadParam, Container)
                .OrderBy(x => x.SubsidyYear)
                .ToList();

            var rec = new
            {
                Id = 0,
                SubsidyYear = 0,
                BudgetRegion = 0m,
                BudgetMunicipality = 0m,
                BudgetFcr = 0m,
                BudgetOtherSource = 0m,
                PlanOwnerCollection = data.Where(x => x.IsShortTerm).Sum(x => x.PlanOwnerCollection),
                PlanOwnerPercent = 0m,
                NotReduceSizePercent = 0m,
                OwnerSumForCr = data.Sum(x => x.OwnerSumForCr),
                BudgetCr = data.Sum(x => x.BudgetCr) - data.Sum(x => x.BalanceAfterCr) + data.Select(x => x.BalanceAfterCr).LastOrDefault(),
                CorrectionFinance = data.Sum(x => x.CorrectionFinance),
                IsShortTerm = false, // Выставляем признак является ли строка входяйщей в краткосрочный период
                BalanceAfterCr = data.Sum(x => x.BalanceAfterCr),
                IsSummary = true,
                SaldoBallance = 0,
                AdditionalExpences = data.Sum(x => x.AdditionalExpences)
            };

            var list = new List<dynamic>();
            list.AddRange(data);
            list.Add(rec);

            return new ListDataResult(list, list.Count());
        }
    }
}