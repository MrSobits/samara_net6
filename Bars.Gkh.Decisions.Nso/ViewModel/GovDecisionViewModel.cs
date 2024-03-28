namespace Bars.Gkh.Decisions.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Entities.Decisions;
    using Gkh.Domain;

    public class GovDecisionViewModel: BaseViewModel<GovDecision>
    {
        public override IDataResult List(IDomainService<GovDecision> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realityObjId = baseParams.Params.GetAsId("objectId");

            var result = domainService.GetAll()
                .Where(x => x.RealityObject.Id == realityObjId);

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
        }
    }
}