namespace Bars.Gkh1468.Interceptors.Dict
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh1468.Entities;

    public class PublicServiceInterceptor : EmptyDomainInterceptor<PublicService>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PublicService> service, PublicService entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PublicService> service, PublicService entity)
        {
            return CheckForm(entity);
        }

        private IDataResult CheckForm(PublicService entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 500)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 500 символов");
            }

            return Success();
        }
    }
}
