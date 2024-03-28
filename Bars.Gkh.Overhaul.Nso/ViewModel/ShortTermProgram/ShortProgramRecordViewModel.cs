namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class ShortProgramRecordViewModel : BaseViewModel<ShortProgramRecord>
    {
        public override IDataResult List(IDomainService<ShortProgramRecord> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    CeoName = x.Stage2.CommonEstateObject.Name,
                    PlanYear = x.Year,
                    x.Stage2.Sum,
                    BudgetOwners = x.OwnerSumForCr,
                    x.BudgetRegion,
                    x.BudgetMunicipality,
                    x.BudgetFcr,
                    x.BudgetOtherSource
                })
                .OrderBy(x => x.PlanYear)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}