namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActivityTsjRealObjViewModel : BaseViewModel<ActivityTsjRealObj>
    {
        public override IDataResult List(IDomainService<ActivityTsjRealObj> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var activityTsjId = baseParams.Params.ContainsKey("activityTSJ")
                       ? baseParams.Params["activityTSJ"].ToLong()
                       : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActivityTsj.Id == activityTsjId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}