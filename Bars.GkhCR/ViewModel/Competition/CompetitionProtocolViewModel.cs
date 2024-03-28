namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class CompetitionProtocolViewModel : BaseViewModel<CompetitionProtocol>
    {
        public override IDataResult List(IDomainService<CompetitionProtocol> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var competitionId = loadParams.Filter.GetAsId("competitionId");

            var data = domainService.GetAll()
                .Where(x => x.Competition.Id == competitionId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeProtocol,
                    x.SignDate,
                    x.IsCancelled,
                    x.File
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<CompetitionProtocol> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.TypeProtocol,
                    x.SignDate,
                    x.File,
                    ExecTime = x.ExecTime.HasValue ? x.ExecTime.Value.ToString("HH:mm") : "",
                    x.ExecDate,
                    x.Note,
                    x.IsCancelled,
                    Competition = x.Competition.Id
                })
                .FirstOrDefault();

            return new BaseDataResult(obj);

        }
    }
}