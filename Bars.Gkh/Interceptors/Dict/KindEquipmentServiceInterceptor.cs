namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class KindEquipmentServiceInterceptor : EmptyDomainInterceptor<KindEquipment>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<KindEquipment> service, KindEquipment entity)
        {
            if (Container.Resolve<IDomainService<BuilderProductionBase>>().GetAll().Any(x => x.KindEquipment.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Производственные базы подрядной организации;");
            }

            return Success();
        }
    }
}
