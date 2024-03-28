namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>Маппинг для "Протокол о выборе управляющей компании для дома"</summary>
    public class ManagementOrganizationDecisionProtocolMap : JoinedSubClassMap<ManagementOrganizationDecisionProtocol>
    {
        public ManagementOrganizationDecisionProtocolMap() : base("Протокол о выборе управляющей компании для дома", "DEC_MKDORG_PROTOCOL")
        {
        }

        protected override void Map()
        {
        }
    }
}