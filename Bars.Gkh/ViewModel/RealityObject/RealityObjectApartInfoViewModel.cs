namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectApartInfoViewModel : BaseViewModel<RealityObjectApartInfo>
    {
        public override IDataResult List(IDomainService<RealityObjectApartInfo> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.NumApartment,
                    x.AreaLiving,
                    x.AreaTotal,
                    x.CountPeople,
                    x.Phone,
                    x.FioOwner,
                    x.Privatized
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}