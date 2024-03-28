using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    public class CostLimitTypeWorkCrViewModel : BaseViewModel<CostLimitTypeWorkCr>
    {
        public override IDataResult List(IDomainService<CostLimitTypeWorkCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var costLimId = loadParams.Filter.GetAs<long>("costLimId");

            var data = domainService.GetAll()
                .Where(x => x.CostLimit.Id == costLimId)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.TypeWorkCr.ObjectCr.RealityObject.Address,
                    x.Cost,
                    x.Volume,
                    x.Year,
                    UnitMeasure = x.UnitMeasure.Name,
                    UnitCost = x.Volume != 0 ? decimal.Round(x.Cost / x.Volume, 2) : 0
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
