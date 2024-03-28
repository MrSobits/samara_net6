namespace Bars.Gkh.Regions.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Nso.Entities;

    public class RealityObjectDocumentViewModel : BaseViewModel<RealityObjectDocument>
    {
        public override IDataResult List(IDomainService<RealityObjectDocument> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    CreateDate = x.ObjectCreateDate,
                    File = x.File.Id
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}