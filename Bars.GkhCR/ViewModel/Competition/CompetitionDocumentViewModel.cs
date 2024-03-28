namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Entities;
    using Gkh.Domain;

    public class CompetitionDocumentViewModel : BaseViewModel<CompetitionDocument>
    {
        public override IDataResult List(IDomainService<CompetitionDocument> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var competitionId = loadParams.Filter.GetAsId("competitionId");

            var data = domainService.GetAll()
                .Where(x => x.Competition.Id == competitionId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.File
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}