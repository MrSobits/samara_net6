using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class RealityObjectInterceptor : EmptyDomainInterceptor<RealityObject>
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
            //try
            //{
            //    if ((entity.TypeHouse == TypeHouse.ManyApartments
            //    || entity.TypeHouse == TypeHouse.BlockedBuilding
            //    || entity.TypeHouse == TypeHouse.SocialBehavior)
            //    && !LongPrObjectDomainService.GetAll().Any(x => x.RealityObject.Id == entity.Id))
            //    {
            //        var obj = new LongTermPrObject { RealityObject = entity };
             
            //            LongPrObjectDomainService.Save(obj);
              
            //    }
            //}
            //catch
            //{ }

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
