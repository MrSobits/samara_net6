namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class RealityObjectStructuralElementInProgrammStage3ViewModel : BaseViewModel<RealityObjectStructuralElementInProgrammStage3>
    {
        public override IDataResult List(IDomainService<RealityObjectStructuralElementInProgrammStage3> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == municipalityId)
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

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}