namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using DataResult;
    using Entities;

    public class RealityObjectStructuralElementInProgrammStage3ViewModel : BaseViewModel<RealityObjectStructuralElementInProgrammStage3>
    {
        public override IDataResult List(IDomainService<RealityObjectStructuralElementInProgrammStage3> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var municipalityId = baseParams.Params.GetAs<long>("muId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == municipalityId || x.RealityObject.MoSettlement.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    x.CommonEstateObjects,
                    x.Year,
                    x.IndexNumber,
                    x.Point,
                    x.Sum
                })
                .Filter(loadParams, Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.IndexNumber);

            var summary = data.AsEnumerable().Sum(x => x.Sum);

            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count(), new { Sum = summary });
        }

    }
}