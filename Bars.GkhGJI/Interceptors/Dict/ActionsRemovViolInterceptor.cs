namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    class ActionsRemovViolInterceptor : EmptyDomainInterceptor<ActionsRemovViol>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActionsRemovViol> service, ActionsRemovViol entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActionsRemovViol> service, ActionsRemovViol entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(ActionsRemovViol entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнено обязательное поле : Наименование");
            }

            if (entity.Name.Length > 300)
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
