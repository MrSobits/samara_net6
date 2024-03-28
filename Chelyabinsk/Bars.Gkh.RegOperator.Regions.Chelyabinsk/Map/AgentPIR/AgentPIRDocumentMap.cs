namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;

    /// <summary>Маппинг для "Документа агент ПИР"</summary>
    public class AgentPIRDocumentMap : BaseEntityMap<AgentPIRDocument>
    {

        public AgentPIRDocumentMap() :
                base("Агент ПИР", "AGENT_PIR_DOCUMENT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Number, "Номер документа").Column("AP_DOC_NUMBER");
            this.Property(x => x.DocumentDate, "Дата документа").Column("AP_DOC_DATE");
            this.Property(x => x.DebtSum, "Сумма основного долга").Column("AP_DOC_DEBT_SUM");
            this.Property(x => x.PeniSum, "Сумма долга по пени").Column("AP_DOC_PENI_SUM");
            this.Property(x => x.Duty, "Сумма госпошлины").Column("AP_DOC_DUTY");
            this.Property(x => x.DocumentType, "Тип документа").Column("AP_DOC_TYPE");
            this.Reference(x => x.File, "Копия документа").Column("AP_DOC_FILE_INFO_ID");
            this.Reference(x => x.AgentPIRDebtor, "Должник агент ПИР").Column("AP_DOC_DEBTOR_ID");
            this.Property(x => x.Repaid, "Сумма погашения").Column("REPAID").DefaultValue(0);
            this.Property(x => x.YesNo, "Погашено").Column("YES_NO").DefaultValue(false);
        }
    }
}
