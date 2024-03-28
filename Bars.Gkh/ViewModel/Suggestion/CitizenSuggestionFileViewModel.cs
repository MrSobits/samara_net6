namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;

    using Entities.Suggestion;

    public class CitizenSuggestionFileViewModel : BaseViewModel<CitizenSuggestionFiles>
    {
        public override IDataResult List(IDomainService<CitizenSuggestionFiles> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var citSuggId = loadParams.Filter.GetAs<long>("citSuggId");
            var isAnswer = loadParams.Filter.Get("isAnswer", false);

            var data = domainService.GetAll()
                .Where(x => x.CitizenSuggestion.Id == citSuggId && x.isAnswer == isAnswer)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentFile.Name,
                    x.DocumentFile
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}