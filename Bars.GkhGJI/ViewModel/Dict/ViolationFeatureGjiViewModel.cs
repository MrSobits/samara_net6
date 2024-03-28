namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class ViolationFeatureGjiViewModel : BaseViewModel<ViolationFeatureGji>
    {
        public override IDataResult List(IDomainService<ViolationFeatureGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var violationId = baseParams.Params.ContainsKey("violationId")
                                  ? baseParams.Params["violationId"].ToLong()
                                  : 0;

            var data = domainService.GetAll()
                .Where(x => x.ViolationGji.Id == violationId)
                .Select(x => new
                {
                    x.Id,
                    x.FeatureViolGji.Name,
                    x.FeatureViolGji.Code,
                    x.FeatureViolGji.Parent
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}