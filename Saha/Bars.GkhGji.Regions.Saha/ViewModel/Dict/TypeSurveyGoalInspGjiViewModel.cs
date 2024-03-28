namespace Bars.GkhGji.Regions.Saha.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TypeSurveyGoalInspGjiViewModel : BaseViewModel<TypeSurveyGoalInspGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyGoalInspGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                ? baseParams.Params["typeSurveyId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => new
                {
                    x.Id,
                    SurveyPurpose = x.SurveyPurpose.Name,
                    x.Code,
                    TypeSurveyGji = x.TypeSurvey
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}