namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class TypeSurveyGjiIssueViewModel : BaseViewModel<TypeSurveyGjiIssue>
    {
        public override IDataResult List(IDomainService<TypeSurveyGjiIssue> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.GetAs<long>("typeSurveyId");

            var data = domainService.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    TypeSurveyGji = x.TypeSurvey
                })
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}