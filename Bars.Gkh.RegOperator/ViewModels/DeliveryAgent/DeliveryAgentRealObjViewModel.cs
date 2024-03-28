using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    public class DeliveryAgentRealObjViewModel : BaseViewModel<DeliveryAgentRealObj>
    {
        public override IDataResult List(IDomainService<DeliveryAgentRealObj> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var delAgentId = loadParams.Filter.GetAs<long>("delAgentId");
            var realityObjectId = loadParams.Filter.GetAs<long>("realityObjectId");
            var data = domain.GetAll()
                 .WhereIf(delAgentId > 0, x => x.DeliveryAgent.Id == delAgentId)
                 .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                 .Select(x => new
                 {
                     x.Id,
                     x.RealityObject.Address,
                     Municipality = x.RealityObject.Municipality.Name,
                     x.DeliveryAgent.Contragent.Name,
                     x.DeliveryAgent.Contragent.Inn,
                     x.DeliveryAgent.Contragent.Kpp,
                     x.DeliveryAgent.Contragent.ContragentState,
                     x.DateStart,
                     x.DateEnd
                 })
                 .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                 .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                 .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}