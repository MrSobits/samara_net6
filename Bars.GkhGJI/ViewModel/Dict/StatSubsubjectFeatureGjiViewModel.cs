namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class StatSubsubjectFeatureGjiViewModel : BaseViewModel<StatSubsubjectFeatureGji>
    {
        public override IDataResult List(IDomainService<StatSubsubjectFeatureGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var subsubjectId = baseParams.Params.ContainsKey("subsubjectId")
                                   ? baseParams.Params["subsubjectId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Subsubject.Id == subsubjectId)
                .Select(x => new
                {
                    x.Id,
                    x.FeatureViol.Name,
                    x.FeatureViol.Code
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}