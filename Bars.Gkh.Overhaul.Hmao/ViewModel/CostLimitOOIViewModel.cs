using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    public class CostLimitOOIViewModel : BaseViewModel<CostLimitOOI>
    {
        public override IDataResult List(IDomainService<CostLimitOOI> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    CommonEstateObject = x.CommonEstateObject.Name,
                    x.Cost,
                    x.DateStart,
                    x.DateEnd,
                    x.FloorStart,
                    x.FloorEnd,
                    Municipality = x.Municipality != null ? x.Municipality.Name : "Все"
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
