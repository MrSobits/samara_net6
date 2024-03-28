namespace Bars.Gkh.Decisions.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RealityObjectDecisionProtocolViewModel : BaseViewModel<RealityObjectDecisionProtocol>
    {
        public override IDataResult List(IDomainService<RealityObjectDecisionProtocol> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("roId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}