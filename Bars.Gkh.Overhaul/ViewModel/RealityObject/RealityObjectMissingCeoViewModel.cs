namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class RealityObjectMissingCeoViewModel : BaseViewModel<RealityObjectMissingCeo>
    {
        public override IDataResult List(IDomainService<RealityObjectMissingCeo> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var realObjId = baseParams.Params.GetAs<long>("realObjId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == realObjId)
                .Select(x => new
                {
                    x.Id,
                    MissingCommonEstateObject = x.MissingCommonEstateObject.Name,
                })
                .Filter(loadParam, Container)
                .Order(loadParam);

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }
    }
}