namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class QExamQuestionViewModel : BaseViewModel<QExamQuestion>
    {
        public override IDataResult List(IDomainService<QExamQuestion> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var personRequestToExam = loadParams.Filter.GetAs("personRequestToExam", 0L);

            var data = domain.GetAll()
                .Where(x => x.PersonRequestToExam.Id == personRequestToExam)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.QuestionText,
                    QualifyTestQuestionsAnswers = x.QualifyTestQuestionsAnswers!=null? x.QualifyTestQuestionsAnswers.Answer:"",
                    IsCorrect = x.QualifyTestQuestionsAnswers != null ? x.QualifyTestQuestionsAnswers.IsCorrect: Enums.YesNoNotSet.No
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}