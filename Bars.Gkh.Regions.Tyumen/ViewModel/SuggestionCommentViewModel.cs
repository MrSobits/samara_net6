namespace Bars.Gkh.Regions.Tyumen.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Enums;
    using Entities.Suggestion;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.Gkh.Domain;

    public class SuggestionCommentViewModel : Bars.Gkh.ViewModel.SuggestionCommentViewModel
    {
        public override IDataResult List(IDomainService<SuggestionComment> domainService, BaseParams baseParams)
        {
            var commentFilesDomain = Container.ResolveDomain<SuggestionCommentFiles>();

            var loadParams = GetLoadParam(baseParams);
            var citSuggId = loadParams.Filter.GetAs<long>("citSuggId");

            try
            {
                var commentFiles = commentFilesDomain.GetAll()
                    .Where(x => x.SuggestionComment.CitizenSuggestion.Id == citSuggId)
                    .Select(x => x.SuggestionComment.Id)
                    .ToArray();

                var comments = domainService.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == citSuggId)
                    .Filter(loadParams, this.Container);

                var data = comments.Order(loadParams).Paging(loadParams).ToArray()
                    .Select(x => new
                    {
                        x.Id,
                        x.CreationDate,
                        Question = x.IsFirst ? x.Description : x.Question,
                        x.Description,
                        x.ProblemPlace,
                        x.Answer,
                        x.IsFirst,
                        HasFiles = commentFiles.Contains(x.Id),
                        x.ExecutorCrFund,
                        x.ExecutorManagingOrganization,
                        x.ExecutorMunicipality,
                        x.ExecutorZonalInspection,
                        ExecutorType = x.GetCurrentExecutorType(),
                        Executor = x.GetCurrentExecutorType() != ExecutorType.None ? x.GetExecutor(x.GetCurrentExecutorType()).Name : string.Empty
                    });

                return new ListDataResult(data, comments.Count());
            }
            finally
            {
                Container.Release(commentFilesDomain);
            }
        }


        public override IDataResult Get(IDomainService<SuggestionComment> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.Get(id);

            var lastCommentId = domainService.GetAll()
                .Where(x => x.CitizenSuggestion.Id == value.CitizenSuggestion.Id)
                .Select(x => x.Id)
                .OrderByDescending(x => x).FirstOrDefault();

            var executorType = value.GetCurrentExecutorType();

            var result = new
            {
                value.Id,
                value.CreationDate,
                value.Question,
                value.Description,
                value.ProblemPlace,
                value.Answer,
                value.IsFirst,
                value.AnswerDate,
                value.ExecutorCrFund,
                value.ExecutorManagingOrganization,
                value.ExecutorMunicipality,
                value.ExecutorZonalInspection,
                value.CitizenSuggestion.Rubric,
                ExecutorType = executorType,
                Executor = value.GetCurrentExecutorType() != ExecutorType.None ? value.GetExecutor(value.GetCurrentExecutorType()).Name : string.Empty,
                IsLast = lastCommentId == value.Id
            };

            return new BaseDataResult(result);
        }
    }
}