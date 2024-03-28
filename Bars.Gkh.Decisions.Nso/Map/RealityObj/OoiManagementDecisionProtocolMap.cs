namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>Маппинг для "Протокол по вопросам, связанных с эксплуатацией и управлением общим имуществом здания"</summary>
    public class OoiManagementDecisionProtocolMap : JoinedSubClassMap<OoiManagementDecisionProtocol>
    {
        public OoiManagementDecisionProtocolMap() : base("Протокол по вопросам, связанных с эксплуатацией и управлением общим имуществом здания", "DEC_OOI_MANAGE_PROTOCOL")
        {
        }

        protected override void Map()
        {
        }
    }
}