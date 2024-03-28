namespace Bars.GkhRf.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhRf.Entities;

    public class LimitCheckInterceptor : EmptyDomainInterceptor<LimitCheck>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<LimitCheck> service, LimitCheck entity)
        {
            if (service.GetAll().Any(x => x.TypeProgram == entity.TypeProgram && (entity.Id == 0 || x.Id != entity.Id)))
            {
                throw new ValidationException("Запись с таким типом программы уже существует!");
            }

            return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<LimitCheck> service, LimitCheck entity)
        {
            return BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<LimitCheck> service, LimitCheck entity)
        {
            if (Container.Resolve<IDomainService<LimitCheckFinSource>>().GetAll().Any(x => x.LimitCheck.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Разрезы финансирования настройки проверки на наличие лимитов;");
            }

            return Success();
        }
    }
}