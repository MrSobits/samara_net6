namespace Bars.GkhGji.Regions.Zabaykalye.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TypeSurveyTaskInspGjiViewModel : BaseViewModel<TypeSurveyTaskInspGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyTaskInspGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.GetAs<long>("typeSurveyId");

            var data = domainService.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => new
                {
                    x.Id,
                    SurveyObjective = x.SurveyObjective.Name,
                    x.Code,
                    TypeSurveyGji = x.TypeSurvey
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}