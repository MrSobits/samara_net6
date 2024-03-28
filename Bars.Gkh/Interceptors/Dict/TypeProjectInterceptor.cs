namespace Bars.Gkh.Interceptors
{
    using B4;
    using B4.Utils;
    using Entities;

    class TypeProjectInterceptor : EmptyDomainInterceptor<TypeProject>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TypeProject> service, TypeProject entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TypeProject> service, TypeProject entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(TypeProject entity)
        {
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
