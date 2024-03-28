namespace Bars.GkhGji.Regions.Zabaykalye.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TypeSurveyKindInspGjiViewModel : BaseViewModel<TypeSurveyKindInspGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyKindInspGji> domainService, BaseParams baseParams)
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
                    x.Code,
                    KindCheck = x.KindCheck.Name
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<TypeSurveyKindInspGji> domainService, BaseParams baseParams)
        {
            var item = domainService.Get(baseParams.Params.GetAs<long>("id"));

            return new BaseDataResult(new { item.Id, item.Code, KindCheckName = item.KindCheck.Name });
        }
    }
}