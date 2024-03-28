namespace Bars.Gkh.Interceptors.Dict
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    class ConstructiveElementGroupInterceptor : EmptyDomainInterceptor<ConstructiveElementGroup>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ConstructiveElementGroup> service, ConstructiveElementGroup entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ConstructiveElementGroup> service, ConstructiveElementGroup entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(ConstructiveElementGroup entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                return Failure("Поле Наименование обязательно для заполнения");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
