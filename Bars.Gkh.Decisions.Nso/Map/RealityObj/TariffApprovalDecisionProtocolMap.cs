namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>Маппинг для "Протокол об утверждение тарифа на содержание и ремонт жилья"</summary>
    public class TariffApprovalDecisionProtocolMap : JoinedSubClassMap<TariffApprovalDecisionProtocol>
    {
        public TariffApprovalDecisionProtocolMap() : base("Протокол об утверждение тарифа на содержание и ремонт жилья", "DEC_TARIFF_APPROVAL_PROTOCOL")
        {
        }

        protected override void Map()
        {
        }
    }
}