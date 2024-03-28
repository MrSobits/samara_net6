namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class SpecialtyServiceInterceptor : EmptyDomainInterceptor<Specialty>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<Specialty> service, Specialty entity)
        {
            if (Container.Resolve<IDomainService<BuilderWorkforce>>().GetAll().Any(x => x.Specialty.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Состав трудовых ресурсов подрядной организации;");
            }

            return Success();
        }
    }
}
