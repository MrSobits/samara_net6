namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class TypeOwnershipServiceInterceptor : EmptyDomainInterceptor<TypeOwnership>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TypeOwnership> service, TypeOwnership entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TypeOwnership> service, TypeOwnership entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TypeOwnership> service, TypeOwnership entity)
        {
            if (Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.TypeOwnership.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр жилых домов;");
            }

            return Success();
        }

        private IDataResult CheckForm(TypeOwnership entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
