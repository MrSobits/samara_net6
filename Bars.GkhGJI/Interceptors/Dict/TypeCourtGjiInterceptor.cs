namespace Bars.GkhGji.Interceptors
{
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class TypeCourtGjiInterceptor : EmptyDomainInterceptor<TypeCourtGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<TypeCourtGji> service, TypeCourtGji entity)
        {
            if (Container.Resolve<IDomainService<ResolutionDispute>>().GetAll().Any(x => x.Court.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Оспаривания в постановлении ГЖИ;");
            }

            return this.Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<TypeCourtGji> service, TypeCourtGji entity)
        {
            return CheckFields(entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<TypeCourtGji> service, TypeCourtGji entity)
        {
            return CheckFields(entity);
        }

        private IDataResult CheckFields(TypeCourtGji entity)
        {
            if (!entity.Name.IsEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (!entity.Code.IsEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            return Success();
        }
    }
}