namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class CourtVerdictGjiInterceptor : EmptyDomainInterceptor<CourtVerdictGji>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CourtVerdictGji> service, CourtVerdictGji entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CourtVerdictGji> service, CourtVerdictGji entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CourtVerdictGji> service, CourtVerdictGji entity)
        {
            if (Container.Resolve<IDomainService<ResolutionDispute>>().GetAll().Any(x => x.CourtVerdict.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Оспаривания в постановлении ГЖИ;");
            }

            return this.Success();
        }

        private IDataResult ValidateEntity(CourtVerdictGji entity)
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