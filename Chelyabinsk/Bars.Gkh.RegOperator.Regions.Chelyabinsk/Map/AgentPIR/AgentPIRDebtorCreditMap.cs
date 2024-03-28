namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;

    /// <summary>Маппинг для "Должника Агент ПИР"</summary>
    public class AgentPIRDebtorCreditMap : BaseEntityMap<AgentPIRDebtorCredit>
    {

        public AgentPIRDebtorCreditMap() :
                base("Зачтено агенту ПИР", "AGENT_PIR_DEBTOR_CREDITED")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Debtor, "Должник").Column("AGENT_PIR_DEBTOR_ID");
            this.Property(x => x.Credit, "Зачтено").Column("CREDIT");
            this.Property(x=> x.Date, "Дата").Column("DOC_DATE");
            this.Property(x => x.User, "Пользователь").Column("ACTIVE_USER");
            this.Reference(x => x.File, "Файл").Column("CREDITED_FILE_INFO_ID");
        }
    }
}
