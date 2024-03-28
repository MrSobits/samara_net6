namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    public class CompetitionLotInterceptor : EmptyDomainInterceptor<CompetitionLot>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CompetitionLot> service, CompetitionLot entity)
        {
            return Validation(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CompetitionLot> service, CompetitionLot entity)
        {
            return Validation(service, entity);
        }

        private IDataResult Validation(IDomainService<CompetitionLot> service, CompetitionLot entity)
        {
            if (entity.LotNumber <= 0)
            {
                return Failure("Необходимо указанть номер лота");
            }

            if (service.GetAll().Any(x => x.Id != entity.Id && entity.Competition.Id == x.Competition.Id && x.LotNumber == entity.LotNumber))
            {
                return Failure("Лот с таким номером в текущем конкурсе уже существует");
            }
            
            return Success();
        }
    }
}