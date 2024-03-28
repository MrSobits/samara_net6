using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Gkh.Interceptors;

    public class RealityObjectInterceptor : Gkh.Interceptors.RealityObjectInterceptor
    {
        public IDomainService<LongTermPrObject> LongPrObjectDomainService { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            if (entity.TypeHouse == TypeHouse.ManyApartments
                || entity.TypeHouse == TypeHouse.BlockedBuilding
                || entity.TypeHouse == TypeHouse.SocialBehavior)
            {
                var obj = new LongTermPrObject { RealityObject = entity };
                LongPrObjectDomainService.Save(obj);
            }

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            if ((entity.TypeHouse == TypeHouse.ManyApartments
                || entity.TypeHouse == TypeHouse.BlockedBuilding
                || entity.TypeHouse == TypeHouse.SocialBehavior)
                && !LongPrObjectDomainService.GetAll().Any(x => x.RealityObject.Id == entity.Id))
            {
                var obj = new LongTermPrObject { RealityObject = entity };
                LongPrObjectDomainService.Save(obj);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObject> service, RealityObject entity)
        {
                LongPrObjectDomainService.GetAll()
                    .Where(x => x.RealityObject.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => LongPrObjectDomainService.Delete(x));
            
            return base.BeforeDeleteAction(service, entity);
        }
    }
}
