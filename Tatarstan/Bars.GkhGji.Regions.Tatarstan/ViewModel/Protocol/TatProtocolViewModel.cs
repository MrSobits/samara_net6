namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Protocol
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;
    using Bars.GkhGji.ViewModel;

    /// <summary>
    /// View-модель для <see cref="TatProtocol"/>
    /// </summary>
    public class TatProtocolViewModel : ProtocolViewModel<TatProtocol>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<TatProtocol> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            dynamic protocol = domainService.Get(id);
            protocol.CitizenshipType = protocol.CitizenshipType ?? CitizenshipType.RussianFederation;

            return new BaseDataResult(protocol);
        }
    }
}