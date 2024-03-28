namespace Bars.Gkh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    class LicenseProvidedDocInterceptor : EmptyDomainInterceptor<LicenseProvidedDoc>
    {
        public override IDataResult BeforeCreateAction(IDomainService<LicenseProvidedDoc> service, LicenseProvidedDoc entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<LicenseProvidedDoc> service, LicenseProvidedDoc entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(LicenseProvidedDoc entity)
        {

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 100)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
