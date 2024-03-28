namespace Bars.GkhRf.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class PaymentInterceptor : EmptyDomainInterceptor<Payment>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Payment> service, Payment entity)
        {
            if (Container.Resolve<IDomainService<Payment>>().GetAll().Any(x => x.RealityObject.Id == entity.RealityObject.Id))
            {
                throw new ValidationException("Для указанного объекта недвижимости уже существует оплата.");
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Payment> service, Payment entity)
        {
            if (Container.Resolve<IDomainService<Payment>>().GetAll().Any(x => x.RealityObject.Id == entity.RealityObject.Id && x.Id != entity.Id))
            {
                throw new ValidationException("Для указанного объекта недвижимости уже существует оплата.");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Payment> service, Payment entity)
        {
            if (Container.Resolve<IDomainService<PaymentItem>>().GetAll().Any(x => x.Payment.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Оплата капитального ремонта;");
            }

            return Success();
        }

    }
}