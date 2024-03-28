namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class ObjectCrCompetitionService : IObjectCrCompetitionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<CompetitionLot> CompetitionLotDomain { get; set; }

        public IDomainService<CompetitionLotTypeWork> CompetitionLotTypeWorkDomain { get; set; }

        public IDomainService<CompetitionProtocol> CompetitionProtocolDomain { get; set; }

        public IDomainService<CompetitionLotBid> CompetitionLotBidDomain { get; set; }

        /// <summary>
        ///     Метод получения информации по конкурсам и лотам в разрезе Объекта КР
        /// </summary>
        public IDataResult ListCompetitions(BaseParams baseParams)
        {
            var loadParam = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            var objectId = loadParam.Filter.GetAs("objectId", 0l);

            var queryTypeWorks = CompetitionLotTypeWorkDomain.GetAll()
                .Where(x => x.TypeWork.ObjectCr.Id == objectId);

            // получаем все виды работ по конкурсу через запятую
            var worksByLot = queryTypeWorks.Select(x => new
            {
                x.TypeWork.Work.Name,
                lotId = x.Lot.Id
            })
                .ToList()
                .GroupBy(x => x.lotId)
                .ToDictionary(x => x.Key,
                    y => y.Select(z => z.Name).Aggregate((str, res) => string.IsNullOrEmpty(res) ? str : res + ", " + str));

            // Поулчаем дату протоколов с типами "Протокол рассмотрения заявок"
            var protocolByCompetition = CompetitionProtocolDomain.GetAll()
                .Where(x => queryTypeWorks.Any(y => y.Lot.Competition.Id == x.Competition.Id))
                .Where(x => x.TypeProtocol == TypeCompetitionProtocol.ReviewBids)
                .Select(x => new
                {
                    x.Id,
                    cmId = x.Competition.Id,
                    x.SignDate
                })
                .AsEnumerable()
                .GroupBy(x => x.cmId)
                .ToDictionary(x => x.Key, y => y.Select(z => z.SignDate).Max());

            // поулчаем участников из заявок по лотам
            var winnerByLot = CompetitionLotBidDomain.GetAll()
                .Where(x => queryTypeWorks.Any(y => y.Lot.Id == x.Lot.Id))
                .Where(x => x.IsWinner)
                .Select(x => new
                {
                    x.Id,
                    lotId = x.Lot.Id,
                    ContragentName = x.Builder.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.lotId)
                .ToDictionary(x => x.Key,
                    y =>
                        y.Select(z => z.ContragentName).Aggregate((str, res) => string.IsNullOrEmpty(res) ? str : res + ", " + str));

            var data = CompetitionLotDomain.GetAll()
                .Where(x => queryTypeWorks.Any(y => y.Lot.Id == x.Id))
                .Select(x => new
                {
                    x.Id,
                    CompetitionId = x.Competition.Id,
                    CompetitionState = x.Competition.State,
                    CompetitionNotifNumber = x.Competition.NotifNumber,
                    CompetitionNotifDate = x.Competition.NotifDate,
                    LotContractNumber = x.ContractNumber,
                    LotContractDate = x.ContractDate
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.CompetitionId,
                    x.CompetitionState,
                    x.CompetitionNotifNumber,
                    x.CompetitionNotifDate,
                    x.LotContractNumber,
                    x.LotContractDate,
                    TypeWorks = worksByLot.ContainsKey(x.Id) ? worksByLot[x.Id] : string.Empty,
                    ProtocolSignDate = protocolByCompetition.ContainsKey(x.CompetitionId) ? (DateTime?) protocolByCompetition[x.CompetitionId] : null,
                    Winner = winnerByLot.ContainsKey(x.Id) ? winnerByLot[x.Id] : null
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}