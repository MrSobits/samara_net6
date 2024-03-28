namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    
    public class QualifyTestQuestionsAnswersViewModel : BaseViewModel<QualifyTestQuestionsAnswers>
    {
        public override IDataResult List(IDomainService<QualifyTestQuestionsAnswers> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

           // var questionId = baseParams.Params.GetAs<long>("questionId");
            var questionId = loadParams.Filter.GetAs("questionId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.QualifyTestQuestions.Id == questionId)
                .Select(x => new
                    {
                        x.Id,
                        x.Answer,
                        x.Code,
                        x.IsCorrect
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}