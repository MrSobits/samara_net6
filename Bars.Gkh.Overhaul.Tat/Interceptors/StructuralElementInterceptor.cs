namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using B4;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.RealEstateType;

    public class StructuralElementInterceptor : EmptyDomainInterceptor<StructuralElement>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<StructuralElement> service, StructuralElement entity)
        {
            if (Container.Resolve<IDomainService<RealEstateTypeStructElement>>().GetAll().Any(x => x.StructuralElement.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Конструктивные элементы типов домов;");
            }

            return Success();
        }
    }
}