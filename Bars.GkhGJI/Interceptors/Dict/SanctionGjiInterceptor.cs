namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class SanctionGjiInterceptor : EmptyDomainInterceptor<SanctionGji>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SanctionGji> service, SanctionGji entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SanctionGji> service, SanctionGji entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SanctionGji> service, SanctionGji entity)
        {
            if (Container.Resolve<IDomainService<Resolution>>().GetAll().Any(x => x.Sanction.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Постановления ГЖИ;");
            }

            return this.Success();
        }

        private IDataResult ValidateEntity(SanctionGji entity)
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