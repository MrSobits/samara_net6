namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class BasePropertyOwnerDecisionViewModel : BaseViewModel<BasePropertyOwnerDecision>
    {
        public override IDataResult List(IDomainService<BasePropertyOwnerDecision> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var protocolId = loadParams.Filter.GetAs<long>("protocolId");

            var data = domainService.GetAll()
                .Where(x => x.PropertyOwnerProtocol.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate,
                    Decision = x.PropertyOwnerDecisionType,
                    x.MethodFormFund,
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}