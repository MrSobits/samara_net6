using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class RealityObjectInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        public IDomainService<LongTermPrObject> LongPrObjectDomainService { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            if (entity.TypeHouse == TypeHouse.ManyApartments)
            {
                var obj = new LongTermPrObject { RealityObject = entity };
                LongPrObjectDomainService.Save(obj);
            }

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            if (entity.TypeHouse == TypeHouse.ManyApartments && !LongPrObjectDomainService.GetAll().Any(x => x.RealityObject.Id == entity.Id))
            {
                var obj = new LongTermPrObject { RealityObject = entity };
                LongPrObjectDomainService.Save(obj);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var longPrObjects =
                LongPrObjectDomainService.GetAll()
                    .Where(x => x.RealityObject.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

            longPrObjects.ForEach(x => LongPrObjectDomainService.Delete(x));

            return Success();
        }
    }
}
