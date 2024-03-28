namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RoofingMaterialServiceInterceptor : EmptyDomainInterceptor<RoofingMaterial>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RoofingMaterial> service, RoofingMaterial entity)
        {

            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RoofingMaterial> service, RoofingMaterial entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RoofingMaterial> service, RoofingMaterial entity)
        {
            if (Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.RoofingMaterial.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр жилых домов;");
            }

            return Success();
        }

        private IDataResult CheckForm(RoofingMaterial entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
