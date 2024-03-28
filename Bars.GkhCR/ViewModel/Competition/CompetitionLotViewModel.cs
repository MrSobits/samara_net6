namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class CompetitionLotViewModel : BaseViewModel<CompetitionLot>
    {
        public override IDataResult List(IDomainService<CompetitionLot> domainService, BaseParams baseParams)
        {
            var lotTypeWorkDomain = Container.Resolve<IDomainService<CompetitionLotTypeWork>>();
            var lotBitDomain = Container.Resolve<IDomainService<CompetitionLotBid>>();

            try
            {
                var loadParams = GetLoadParam(baseParams);

                var competitionId = loadParams.Filter.GetAsId("competitionId");

                var query = domainService.GetAll()
                    .Where(x => x.Competition.Id == competitionId);

                var countObjects = lotTypeWorkDomain.GetAll()
                    .Where(x => query.Any(y => y.Id == x.Lot.Id))
                    .GroupBy(x => x.Lot.Id)
                    .ToDictionary(x => x.Key, y => y.Count());

                var countBid = lotBitDomain.GetAll()
                    .Where(x => query.Any(y => y.Id == x.Lot.Id))
                    .GroupBy(x => x.Lot.Id)
                    .ToDictionary(x => x.Key, y => y.Count());

                var winners = lotBitDomain.GetAll()
                    .Where(x => query.Any(y => y.Id == x.Lot.Id) && x.IsWinner)
                    .Select(x => new
                    {
                        LotId = x.Lot.Id,
                        WinnerName = x.Builder.Contragent.Name,
                        WinnerId = x.Builder.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.LotId)
                    .ToDictionary(x => x.Key, y => y.Select(z => new {z.WinnerName, z.WinnerId}).FirstOrDefault());

                var data = query
                    .Select(x => new
                    {
                        x.Id,
                        x.LotNumber,
                        x.Subject,
                        x.StartingPrice
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.LotNumber,
                        x.Subject,
                        x.StartingPrice,
                        ObjectsCount = countObjects.Get(x.Id),
                        BidCount = countBid.Get(x.Id),
                        Winner = winners.ContainsKey(x.Id) ? winners[x.Id].WinnerName : string.Empty 
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                Container.Release(lotTypeWorkDomain);
                Container.Release(lotBitDomain);
            }
        }

        public override IDataResult Get(IDomainService<CompetitionLot> domainService, BaseParams baseParams)
        {
            var lotBitDomain = Container.Resolve<IDomainService<CompetitionLotBid>>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");

                var winner = lotBitDomain.GetAll()
                    .Where(x => x.Lot.Id == id && x.IsWinner)
                    .Select(x => new
                    {
                        WinnerName = x.Builder.Contragent.Name,
                        WinnerId = x.Builder.Id
                    })
                    .FirstOrDefault();

                var obj = domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        Competition = x.Competition.Id,
                        x.LotNumber,
                        x.StartingPrice,
                        x.Subject,
                        x.ContractNumber,
                        x.ContractDate,
                        x.ContractFactPrice,
                        x.ContractFile,
                        Winner = winner != null ? winner.WinnerName : string.Empty,
                        WinnerId = winner != null ? winner.WinnerId : 0L
                    })
                    .FirstOrDefault();

                return new BaseDataResult(obj);

            }
            finally
            {
                Container.Release(lotBitDomain);
            }
        }
    }
}