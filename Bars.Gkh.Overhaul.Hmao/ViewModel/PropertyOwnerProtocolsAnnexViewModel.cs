namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PropertyOwnerProtocolsAnnexViewModel : BaseViewModel<PropertyOwnerProtocolsAnnex>
    {
        public override IDataResult List(IDomainService<PropertyOwnerProtocolsAnnex> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var protocolId = baseParams.Params.GetAs<long>("protocolId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    x.FileInfo,
                    x.Description,
                    x.DocumentDate,
                    x.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}