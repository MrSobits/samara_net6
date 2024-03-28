namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActIsolatedRealObjEventViewModel : BaseViewModel<ActIsolatedRealObjEvent>
    {
        public override IDataResult List(IDomainService<ActIsolatedRealObjEvent> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.ActIsolatedRealObj.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Term,
                    x.DateStart,
                    x.DateEnd
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}