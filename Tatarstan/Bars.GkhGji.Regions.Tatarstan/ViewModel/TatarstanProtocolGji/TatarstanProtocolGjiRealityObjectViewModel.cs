namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanProtocolGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiRealityObjectViewModel : BaseViewModel<TatarstanProtocolGjiRealityObject>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanProtocolGjiRealityObject> domainService, BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("documentId");
            return domainService.GetAll()
                .WhereIf(protocolId != default(long), x => x.TatarstanProtocolGji.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    Address = x.RealityObject.Address,
                    Area = x.RealityObject.AreaMkd
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
