namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;

    using Entities.Suggestion;

    public class SuggestionCommentFileViewModel : BaseViewModel<SuggestionCommentFiles>
    {
        public override IDataResult List(IDomainService<SuggestionCommentFiles> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var citSuggCommentId = loadParams.Filter.GetAs<long>("citSuggCommentId");
            var isAnswer = loadParams.Filter.Get("isAnswer", false);

            var data = domainService.GetAll()
                .Where(x => x.SuggestionComment.Id == citSuggCommentId && x.isAnswer == isAnswer)
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