namespace Bars.GkhGji.Regions.Smolensk.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TypeSurveyInspFoundationGjiViewModel : BaseViewModel<TypeSurveyInspFoundationGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyInspFoundationGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.GetAs<long>("typeSurveyId");

            var data = domainService.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => new
                {
                    x.Id,
                    NormativeDoc = x.NormativeDoc.Name,
                    x.Code,
                    TypeSurveyGji = x.TypeSurvey
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}