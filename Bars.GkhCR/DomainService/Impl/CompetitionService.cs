namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Enums;
    
    using Castle.Windsor;

    /// <summary>
    /// 
    /// </summary>
    public class CompetitionService : ICompetitionService
    {
        private readonly IWindsorContainer _container;
        private readonly IDomainService<Competition> _competitionDomain;
        private readonly IDomainService<CompetitionLot> _competitionLotDomain;
        private readonly IDomainService<CompetitionProtocol> _competitionProtocolDomain;
        private readonly IDomainService<CompetitionLotBid> _competitionLotBidDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CompetitionService(
            IWindsorContainer container,
            IDomainService<Competition> competitionDomain,
            IDomainService<CompetitionLot> competitionLotDomain,
            IDomainService<CompetitionProtocol> competitionProtocolDomain,
            IDomainService<CompetitionLotBid> competitionLotBidDomain)
        {
            _container = container;
            _competitionDomain = competitionDomain;
            _competitionLotDomain = competitionLotDomain;
            _competitionProtocolDomain = competitionProtocolDomain;
            _competitionLotBidDomain = competitionLotBidDomain;
        }

        public IList GetList(BaseParams baseParams, bool isPaging,  ref int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();

            //Количество лотов по конкурсу
            var lots = _competitionLotDomain.GetAll()
                .GroupBy(x => x.Competition.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            //Количество победителей по конкурсу
            var winners = _competitionLotBidDomain.GetAll()
                .Where(x => x.IsWinner)
                .GroupBy(x => x.Lot.Competition.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            //Количество заявок по конкурсу
            var requests = _competitionLotBidDomain.GetAll()
                .GroupBy(x => x.Lot.Competition.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            //Количество договоров по конкурсу
            var contracts = _competitionLotDomain.GetAll()
                .Where(x => x.ContractDate.HasValue)
                .GroupBy(x => x.Competition.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            //дата подписания Протокола вскрытия
            var openEnvelopes = _competitionProtocolDomain.GetAll()
                .Where(x => x.TypeProtocol == TypeCompetitionProtocol.OpenEnvelopes)
                .Select(x => new
                {
                    cmId = x.Competition.Id,
                    x.SignDate
                })
                .AsEnumerable()
                .GroupBy(x => x.cmId)
                .ToDictionary(x => x.Key, y => y.Select(z => (DateTime?) z.SignDate).Max());

            //дата подписания Протокола рассмотрения
            var reviewBid = _competitionProtocolDomain.GetAll()
                .Where(x => x.TypeProtocol == TypeCompetitionProtocol.ReviewBids)
                .Select(x => new
                {
                    cmId = x.Competition.Id,
                    x.SignDate
                })
                .AsEnumerable()
                .GroupBy(x => x.cmId)
                .ToDictionary(x => x.Key, y => y.Select(z => (DateTime?)z.SignDate).Max());

            var data = _competitionDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.NotifNumber,
                    x.NotifDate
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.NotifNumber,
                    x.NotifDate,
                    LotCount = lots.Get(x.Id),
                    WinnersCount = winners.Get(x.Id),
                    LotBidCount = requests.Get(x.Id),
                    ContractCount = contracts.Get(x.Id),
                    OpenEnvelopes = openEnvelopes.Get(x.Id),
                    ReviewBid = reviewBid.Get(x.Id)
                })
                .AsQueryable()
                .Filter(loadParams, _container);

            totalCount = data.Count();

            if (isPaging)
            {
                return data.Order(loadParams).Paging(loadParams).ToList();
            }
            
            return data.Order(loadParams).ToList();
        }
    }
}