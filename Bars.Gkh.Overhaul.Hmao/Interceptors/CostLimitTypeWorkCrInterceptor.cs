using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    public class CostLimitTypeWorkCrInterceptor : EmptyDomainInterceptor<CostLimitTypeWorkCr>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<CostLimitTypeWorkCr> service, CostLimitTypeWorkCr entity)
        {
            var costLimitDomain = Container.ResolveDomain<CostLimit>();
            var costLimit = costLimitDomain.Get(entity.CostLimit.Id);

            var costWorkList = service.GetAll()
                .Where(x => x.CostLimit == costLimit)
                .Where(x => x != entity)
                .ToList();

            var costList = new List<decimal>();
            foreach (var costWork in costWorkList)
            {
                var costByWork = costWork.Volume != 0 ? costWork.Cost / costWork.Volume : 0;
                costList.Add(costByWork);
            }

            var costLim = costList.Count != 0 ?(costList.SafeSum() / costList.Count) * (1 + costLimit.Rate / 100) : 0;

            try
            {
                costLimit.Cost = decimal.Round(costLim, 2);
            }
            catch
            {
                costLimit.Cost = costLim;
            }

            costLimitDomain.Update(costLimit);

            return Success();
        }
    }
}
