namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;

    /// <summary>Маппинг для "Агент ПИР"</summary>
    public class AgentPIRMap : BaseEntityMap<AgentPIR>
    {

        public AgentPIRMap() :
                base("Агент ПИР", "AGENT_PIR")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DateStart, "Дата начала").Column("AP_DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания").Column("AP_DATE_END");
            this.Property(x => x.ContractDate, "Дата договора").Column("AP_CONTRACT_DATE");
            this.Property(x => x.ContractNumber, "Номер договора").Column("AP_CONTRACT_NUMBER");
            this.Reference(x => x.Contragent, "Контрагент").Column("AP_CONTRAGENT_ID");
            this.Reference(x => x.FileInfo, "Скан договора").Column("AP_FILE_INFO_ID");
        }
    }
}
