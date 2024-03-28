namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>Маппинг для "Протокол о выборе формы управления многоквартирным домом"</summary>
    public class MkdManagementTypeDecisionProtocolMap : JoinedSubClassMap<MkdManagementTypeDecisionProtocol>
    {
        public MkdManagementTypeDecisionProtocolMap() : base("Протокол о выборе формы управления многоквартирным домом", "DEC_MKDMANAGE_TYPE_PROTOCOL")
        {
        }

        protected override void Map()
        {
        }
    }
}