namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActSurveyRealityObjectViewModel : BaseViewModel<ActSurveyRealityObject>
    {
        public override IDataResult List(IDomainService<ActSurveyRealityObject> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActSurvey.Id == documentId)
                .Select(x => new
                    {
                        x.Id,
                        RealityObject = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name
                    })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
