namespace Bars.Gkh.Interceptors.Dict
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities.Dicts;

    public class BuildingFeatureInterceptor : EmptyDomainInterceptor<BuildingFeature>
    {
        public override IDataResult BeforeCreateAction(IDomainService<BuildingFeature> service, BuildingFeature entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<BuildingFeature> service, BuildingFeature entity)
        {
            return CheckForm(entity);
        }

        private IDataResult CheckForm(BuildingFeature entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 1000)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 1000 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 50)
            {
                return Failure("Количество знаков в поле Код не должно превышать 50 символов");
            }

            return Success();
        }
    }
}
