namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;

    using Entities.Suggestion;

    public class SuggestionCommentViewModel : BaseViewModel<SuggestionComment>
    {
        public override IDataResult List(IDomainService<SuggestionComment> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var citSuggId = loadParams.Filter.GetAs<long>("citSuggId");

            var data = domainService.GetAll()
                .Where(x => x.CitizenSuggestion.Id == citSuggId)
                .Select(x => new
                {
                    x.Id,
                    x.CreationDate,
                    x.Question
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}