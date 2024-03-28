namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.RealityObj;

    public class RealityObjectBuildingFeatureViewModel : BaseViewModel<RealityObjectBuildingFeature>
    {
        public override IDataResult List(IDomainService<RealityObjectBuildingFeature> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.BuildingFeature.Code,
                    x.BuildingFeature.Name,
                    RealityObject = x.RealityObject.Id,
                    x.Id
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
