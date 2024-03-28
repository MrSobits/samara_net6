namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    public class SubsidyMunicipalityRecordViewModel : BaseViewModel<SubsidyMunicipalityRecord>
    {
        public IDomainService<SubsidyRecordVersionData> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<SubsidyMunicipality> SubsidyMunicipalityDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed. Suppression is OK here.")]
        public override IDataResult List(IDomainService<SubsidyMunicipalityRecord> domainService, BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var loadParam = GetLoadParam(baseParams);

            var subsidy = SubsidyMunicipalityDomain.GetAll().FirstOrDefault(x => x.Municipality.Id == municipalityId);

            if (subsidy == null)
                return new ListDataResult(null, 0);

            var versionId = ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == municipalityId).Return(x => x.Id);

            var versionData = SubsidyRecordVersionDomain.GetAll()
                .Where(x => x.Version.Id == versionId)
                .ToDictionary(x => x.SubsidyRecordUnversioned.Id);

            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            // Начало периода
            var periodStart = config.ProgrammPeriodStart;

            // Количество лет краткосрочной программы
            var shortTermPeriod = config.ShortTermProgPeriod;

            var data = domainService.GetAll()
                .Where(x => x.SubsidyMunicipality.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    x.SubsidyYear,
                    x.BudgetRegion,
                    x.BudgetMunicipality,
                    x.BudgetFcr,
                    BudgetCr = (x.BudgetRegion + x.BudgetMunicipality + x.BudgetFcr + x.OwnerSource),
                    x.OwnerSource,
                    SubsidyMunicipality = x.SubsidyMunicipality.Id
                })
                .Filter(loadParam, Container)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.SubsidyMunicipality,
                    x.SubsidyYear,
                    x.BudgetRegion,
                    x.BudgetMunicipality,
                    x.BudgetFcr,
                    x.BudgetCr,
                    x.OwnerSource,
                    NeedFinance = versionData[x.Id].Return(y => y.NeedFinance),
                    Deficit = versionData[x.Id].Return(y => y.Deficit),
                    CorrNeedFinance = versionData[x.Id].Return(y => y.CorrNeedFinance),
                    CorrDeficit = versionData[x.Id].Return(y => y.CorrDeficit),
                    IsShortTerm = (x.SubsidyYear < periodStart + shortTermPeriod),
                    // Выставляем признак является ли строка входяйщей в краткосрочный период
                    IsSummary = false
                })
                .OrderBy(x => x.SubsidyYear)
                .ToList();

            var rec = new
            {
                Id = 0L,
                SubsidyMunicipality = data.Select(y => y.SubsidyMunicipality).FirstOrDefault(),
                SubsidyYear = 0,
                BudgetRegion = data.Sum(x => x.BudgetRegion),
                BudgetMunicipality = data.Sum(x => x.BudgetMunicipality),
                BudgetFcr = data.Sum(x => x.BudgetFcr),
                BudgetCr = data.Sum(x => x.BudgetCr),
                OwnerSource = data.Sum(x => x.OwnerSource),
                NeedFinance = data.Sum(x => x.NeedFinance),
                Deficit = data.Sum(x => x.Deficit),
                CorrNeedFinance = data.Sum(x => x.CorrNeedFinance),
                CorrDeficit = data.Sum(x => x.CorrDeficit),
                IsShortTerm = false, // Выставляем признак является ли строка входяйщей в краткосрочный период
                IsSummary = true
            };

            data.Add(rec);

            return new ListDataResult(data, data.Count());
            /*
            var subsidyId = 0;
            var isDpkrCorrected = false;
            if (subsidy != null)
            {
                subsidyId = subsidy.Id;
                isDpkrCorrected = subsidy.DpkrCorrected;
            }
                
            var data = domainService.GetAll()
                             .Where(x => x.SubsidyMunicipality.Id == subsidyId)
                             .Filter(loadParam, Container);

            var corrDomain = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();
            var correctDpkr = corrDomain.GetAll().Where(x => x.RealityObject.Municipality.Id == municipalityId).ToList();
                
            var result = data.Order(loadParam).ToList()
                .Select(x => new
                {
                    x.Id,
                    x.SubsidyMunicipality,
                    x.SubsidyYear,
                    x.BudgetFund,
                    x.BudgetRegion,
                    x.BudgetMunicipality,
                    x.OtherSource,
                    x.CalculatedCollection,
                    x.ShareBudgetFund,
                    x.ShareBudgetRegion,
                    x.ShareBudgetMunicipality,
                    x.ShareOtherSource,
                    x.ShareOwnerFounds,
                    FinanceNeedBefore = versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.FinanceNeedBefore),
                    FinanceNeedAfter = versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.FinanceNeedAfter),
                    //FinanceNeedFromCorrect = isDpkrCorrected ? x.FinanceNeedAfter : versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.FinanceNeedBefore),
                    RecommendedTarif = versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.RecommendedTarif),
                    DeficitBefore = versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.DeficitBefore),
                    x.DeficitAfter,
                    DeficitFromCorrect = isDpkrCorrected ? x.DeficitAfter : versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.DeficitBefore),
                    x.StartRecommendedTarif,
                    RecommendedTarifCollection = versionData.FirstOrDefault(y => y.SubsidyRecordUnversioned.Id == x.Id).Return(y => y.RecommendedTarifCollection),
                    x.EstablishedTarif,
                    x.OwnersLimit,
                    OwnersMoneyBalance = correctDpkr.FirstOrDefault(y => x.SubsidyMunicipality.Municipality.Id == y.RealityObject.Municipality.Id && x.SubsidyYear == y.PlanYear).Return(z => z.OwnersMoneyBalance),
                    BudgetFundBalance = correctDpkr.FirstOrDefault(y => x.SubsidyMunicipality.Municipality.Id == y.RealityObject.Municipality.Id && x.SubsidyYear == y.PlanYear).Return(z => z.BudgetFundBalance),
                    BudgetRegionBalance = correctDpkr.FirstOrDefault(y => x.SubsidyMunicipality.Municipality.Id == y.RealityObject.Municipality.Id && x.SubsidyYear == y.PlanYear).Return(z => z.BudgetRegionBalance),
                    BudgetMunicipalityBalance = correctDpkr.FirstOrDefault(y => x.SubsidyMunicipality.Municipality.Id == y.RealityObject.Municipality.Id && x.SubsidyYear == y.PlanYear).Return(z => z.BudgetMunicipalityBalance),
                    OtherSourceBalance = correctDpkr.FirstOrDefault(y => x.SubsidyMunicipality.Municipality.Id == y.RealityObject.Municipality.Id && x.SubsidyYear == y.PlanYear).Return(z => z.OtherSourceBalance)
                });

            Container.Release(domainSubsidy);
            Container.Release(corrDomain);

            return new ListDataResult(result, result.Count());
             */
            return new ListDataResult(null, 0);
        }
    }
}