namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class BasePropertyOwnerDecisionViewModel : BaseViewModel<BasePropertyOwnerDecision>
    {
        public override IDataResult List(IDomainService<BasePropertyOwnerDecision> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var protocolId = baseParams.GetLoadParam().Filter.GetAs<long>("protocolId");
            
            var data = domainService.GetAll()
                .WhereIf(protocolId > 0, x => x.PropertyOwnerProtocol.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    ProtocolNumber = x.PropertyOwnerProtocol.DocumentNumber,
                    ProtocolDate = x.PropertyOwnerProtocol.DocumentDate,
                    Decision = x.PropertyOwnerDecisionType,
                    x.MethodFormFund
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}