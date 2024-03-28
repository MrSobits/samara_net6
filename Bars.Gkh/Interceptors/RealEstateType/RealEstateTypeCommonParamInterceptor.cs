namespace Bars.Gkh.Interceptors.RealEstateType
{
    using Bars.B4;
    using Entities.RealEstateType;


    class RealEstateTypeCommonParamInterceptor : EmptyDomainInterceptor<RealEstateTypeCommonParam>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealEstateTypeCommonParam> service, RealEstateTypeCommonParam entity)
        {
            return CheckEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RealEstateTypeCommonParam> service, RealEstateTypeCommonParam entity)
        {
            return CheckEntity(entity);
        }

        private IDataResult CheckEntity(RealEstateTypeCommonParam entity)
        {
            if (string.IsNullOrEmpty(entity.Min))
            {
                return Failure("Не заполнено обязательное поле Минимальное значение");
            }

            return Success();
        }
    }
}
