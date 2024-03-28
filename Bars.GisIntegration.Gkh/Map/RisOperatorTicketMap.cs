namespace Bars.GisIntegration.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Gkh.Entities;

    /// <summary>
    /// Маппинг для "RisOperatorToken"
    /// </summary>
    public class RisOperatorTicketMap : BaseEntityMap<RisOperatorTicket>
    {
        public RisOperatorTicketMap() :
            base("Bars.GisIntegration.Gkh.RisOperatorToken", "RIS_OPERATOR_TICKET")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Ticket, "Ticket").Column("TICKET");
            this.Reference(x => x.Operator, "Operator").Column("GKH_OPERATOR_ID").NotNull().Fetch();
        }
    }
}