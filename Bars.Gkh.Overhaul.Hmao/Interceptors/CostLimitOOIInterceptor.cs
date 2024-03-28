using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    public class CostLimitOOIInterceptor : EmptyDomainInterceptor<CostLimitOOI>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CostLimitOOI> service, CostLimitOOI entity)
        {
            var costs = service.GetAll()
                .Where(x => x.CommonEstateObject.Id == entity.CommonEstateObject.Id)
                .WhereIf(entity.Municipality != null, x => x.Municipality == null || x.Municipality.Id == entity.Id)
                .WhereIf(entity.DateEnd != null, x => x.DateStart == null || x.DateStart <= entity.DateEnd)
                .WhereIf(entity.DateStart != null, x => x.DateEnd == null || x.DateEnd >= entity.DateStart)
                .WhereIf(entity.FloorEnd != null, x => x.FloorStart == null || x.FloorStart <= entity.FloorEnd)
                .WhereIf(entity.FloorStart != null, x => x.FloorEnd == null || x.FloorEnd >= entity.FloorStart)
                .Select(x => new
                {
                    x.Id,
                    x.Cost
                });

            foreach (var cost in costs)
                if (cost.Cost != entity.Cost)
                    return Failure($"Условия стоимости перекрывают стоимость c id {cost.Id}");

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CostLimitOOI> service, CostLimitOOI entity)
        {
            var costs = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Where(x => x.CommonEstateObject.Id == entity.CommonEstateObject.Id)
                .WhereIf(entity.Municipality != null, x => x.Municipality == null || x.Municipality.Id == entity.Id)
                .WhereIf(entity.DateEnd != null, x => x.DateStart == null || x.DateStart <= entity.DateEnd)
                .WhereIf(entity.DateStart != null, x => x.DateEnd == null || x.DateEnd >= entity.DateStart)
                .WhereIf(entity.FloorEnd != null, x => x.FloorStart == null || x.FloorStart <= entity.FloorEnd)
                .WhereIf(entity.FloorStart != null, x => x.FloorEnd == null || x.FloorEnd >= entity.FloorStart)
                .Select(x => new
                {
                    x.Id,
                    x.Cost
                });

            foreach (var cost in costs)
                if (cost.Cost != entity.Cost)
                    return Failure($"Условия стоимости перекрывают стоимость c id {cost.Id}");

            return Success();
        }
    }
}
