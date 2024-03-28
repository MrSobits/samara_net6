namespace Bars.Gkh1468.Interceptors.Passport
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh1468.Entities;

    public class PassportStructInterceptor : EmptyDomainInterceptor<PassportStruct>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PassportStruct> service, PassportStruct entity)
        {
            var entityIsNotUnique = service.GetAll()
                .Where(x => x.ValidFromMonth == entity.ValidFromMonth)
                .Where(x => x.ValidFromYear == entity.ValidFromYear)
                .Any(x => x.PassportType == entity.PassportType);

            if (entityIsNotUnique)
            {
                return Failure("Уже существует структура паспорта данного типа с аналогичным началом действия");
            }
            
            entity.Name = entity.Name ?? "Новая структура";

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PassportStruct> service, PassportStruct entity)
        {
            var entityIsNotUnique = service.GetAll()
                                           .Any(x => x.ValidFromMonth == entity.ValidFromMonth
                                                     && x.ValidFromYear == entity.ValidFromYear
                                                     && x.PassportType == entity.PassportType
                                                     && x.Id != entity.Id);

            if (entityIsNotUnique)
            {
                return Failure("Уже существует структура паспорта данного типа с аналогичным началом действия");
            }

            return base.BeforeUpdateAction(service, entity);
        }
    }
}