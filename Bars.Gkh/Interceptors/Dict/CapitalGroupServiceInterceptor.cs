namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class CapitalGroupServiceInterceptor : EmptyDomainInterceptor<CapitalGroup>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CapitalGroup> service, CapitalGroup entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CapitalGroup> service, CapitalGroup entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CapitalGroup> service, CapitalGroup entity)
        {
            if (Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.CapitalGroup.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр жилых домов;");
            }

            return Success();
        }

        private IDataResult CheckForm(CapitalGroup entity)
        {
            if (entity.Description.IsNotEmpty() && entity.Description.Length > 1000)
            {
                return Failure("Количество знаков в поле Описание не должно превышать 1000 символов");
            }

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
