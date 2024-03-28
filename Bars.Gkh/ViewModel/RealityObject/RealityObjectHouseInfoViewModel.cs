namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectHouseInfoViewModel : BaseViewModel<RealityObjectHouseInfo>
    {
        public override IDataResult List(IDomainService<RealityObjectHouseInfo> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.NumObject,
                    x.NumRegistrationOwner,
                    x.DateRegistration,
                    x.DateRegistrationOwner,
                    x.TotalArea,
                    x.Owner,
                    x.Name,
                    x.KindRight,
                    UnitMeasure = x.UnitMeasure.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}