namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    public class ReasonInexpedientServiceInterceptor : EmptyDomainInterceptor<ReasonInexpedient>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ReasonInexpedient> service, ReasonInexpedient entity)
        {
            if (Container.Resolve<IDomainService<EmergencyObject>>().GetAll().Any(x => x.ReasonInexpedient.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр аварийных домов;");
            }

            return Success();
        }
    }
}
