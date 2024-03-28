namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    public class CompetitionLotBidInterceptor : EmptyDomainInterceptor<CompetitionLotBid>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CompetitionLotBid> service, CompetitionLotBid entity)
        {
            return Validation(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CompetitionLotBid> service, CompetitionLotBid entity)
        {
            return Validation(service, entity);
        }

        private IDataResult Validation(IDomainService<CompetitionLotBid> service, CompetitionLotBid entity)
        {
            if (entity.IsWinner && service.GetAll().Any(x => x.Id != entity.Id && x.IsWinner && x.Lot.Id == entity.Lot.Id))
            {
                return Failure("Победитель уже выбран! Уберите отметку 'Является победителем' у другого участника и после этого выберите нового победителя!");
            }
            
            return Success();
        }
    }
}