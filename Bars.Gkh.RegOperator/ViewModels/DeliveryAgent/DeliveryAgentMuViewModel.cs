using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;

    public class DeliveryAgentMuViewModel : BaseViewModel<DeliveryAgentMunicipality>
    {
        public override IDataResult List(IDomainService<DeliveryAgentMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var delAgentId = loadParams.Filter.GetAs<long>("delAgentId");

            var data = domain.GetAll()
                .Where(x => x.DeliveryAgent.Id == delAgentId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}