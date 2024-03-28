namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>Маппинг для "Протокол о формировании фонда капитального ремонта"</summary>
    public class CrFundDecisionProtocolMap : JoinedSubClassMap<CrFundDecisionProtocol>
    {
        public CrFundDecisionProtocolMap() : base("Протокол о формировании фонда капитального ремонта", "DEC_CRFUND_PROTOCOL")
        {
        }

        protected override void Map()
        {
        }
    }
}