namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectCouncillorsViewModel : BaseViewModel<RealityObjectCouncillors>
    {
        public override IDataResult List(IDomainService<RealityObjectCouncillors> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("roId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.Fio,
                    x.Phone,
                    x.Email,
                    x.Post
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}