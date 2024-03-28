namespace Bars.Gkh.ViewModel.Suggestion
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Entities.Suggestion;
    using Domain;
    using Enums;

    public class CitizenSuggestionHistoryViewModel : BaseViewModel<CitizenSuggestionHistory>
    {
        public override IDataResult List(IDomainService<CitizenSuggestionHistory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var suggId = baseParams.Params.GetAsId("suggId");

            var histories = domainService.GetAll()
                .Where(x => x.CitizenSuggestion.Id == suggId);

            var muIds =
                histories
                    .Where(x => x.ExecutorMunicipality != null)
                    .Select(x => x.ExecutorMunicipality.Id)
                    .ToArray();

            var localGovs = Container.ResolveDomain<LocalGovernmentMunicipality>().GetAll()
                .Where(x => muIds.Contains(x.Municipality.Id))
                .Select(x => new
                {
                    x.Municipality.Id,
                    x.LocalGovernment.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.First().Name);

            var data = histories
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.RecordDate,
                    ExecutorName = x.TargetExecutorType == ExecutorType.Mu
                        ? localGovs.Get(x.ExecutorMunicipality.Id)
                        : x.GetExecutorDisplayName(),
                    x.ExecutionDeadline
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}