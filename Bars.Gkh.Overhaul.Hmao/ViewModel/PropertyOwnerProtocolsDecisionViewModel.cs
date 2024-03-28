namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PropertyOwnerProtocolsDecisionViewModel : BaseViewModel<PropertyOwnerProtocolsDecision>
    {
        public override IDataResult List(IDomainService<PropertyOwnerProtocolsDecision> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var protocolId = loadParams.Filter.GetAs<long>("protocolId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentFile,
                    x.Description,
                    Decision = x.Decision.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}