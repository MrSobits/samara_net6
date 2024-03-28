namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SubsidyMunicipalityRecordViewModel : BaseViewModel<SubsidyMunicipalityRecord>
    {
        public IDomainService<SubsidyRecordVersionData> SubsidyRecordVersionDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; } 

        public override IDataResult List(IDomainService<SubsidyMunicipalityRecord> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var versionId = ProgramVersionDomain.GetAll().FirstOrDefault(x => x.IsMain).Return(x => x.Id);

            var versionData =
                SubsidyRecordVersionDomain.GetAll()
                    .Where(x => x.Version.Id == versionId
                                && x.SubsidyRecordUnversioned.SubsidyMunicipality.Municipality.Id == municipalityId)
                    .ToList();

            var domainSubsidy = Container.Resolve<IDomainService<SubsidyMunicipality>>();
            var subsidy = domainSubsidy.GetAll()
                .Select(x => new
                {
                    x.Id,
                    MunicipalityId = x.Municipality.Id,
                    x.DpkrCorrected
                })
                .FirstOrDefault(x => x.MunicipalityId == municipalityId);

            var subsidyId = 0L;
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
        }
    }
}