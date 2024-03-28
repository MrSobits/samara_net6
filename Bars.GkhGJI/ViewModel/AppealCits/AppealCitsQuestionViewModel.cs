namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsQuestionViewModel: BaseViewModel<AppealCitsQuestion>
    {
        public override IDataResult List(IDomainService<AppealCitsQuestion> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var appealCitizensId = loadParams.Filter.GetAsId("appealCitizensId");

            return domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.Id,
                    QuestionKind = x.QuestionKind.Name,
                    x.QuestionKind.QuestionType
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}