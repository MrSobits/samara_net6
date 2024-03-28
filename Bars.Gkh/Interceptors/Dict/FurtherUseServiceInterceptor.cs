namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    public class FurtherUseServiceInterceptor : EmptyDomainInterceptor<FurtherUse>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<FurtherUse> service, FurtherUse entity)
        {
            if (Container.Resolve<IDomainService<EmergencyObject>>().GetAll().Any(x => x.FurtherUse.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр аварийных домов;");
            }

            return Success();
        }
    }
}
