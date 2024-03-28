namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Ведомость ресурсов"</summary>
    public class SpecialResourceStatementMap : BaseImportableEntityMap<SpecialResourceStatement>
    {
        public SpecialResourceStatementMap() : 
                base("Ведомость ресурсов", "CR_SPECIAL_EST_CALC_RES_STATEM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            this.Property(x => x.Number, "Номер").Column("DOCUMENT_NUM").Length(50);
            this.Property(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE").Length(250);
            this.Property(x => x.Name, "Наименование").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.Reason, "Обоснование").Column("REASON").Length(300);
            this.Property(x => x.TotalCount, "Количество всего").Column("TOTAL_COUNT");
            this.Property(x => x.TotalCost, "Общая стоимость").Column("TOTAL_COST");
            this.Property(x => x.OnUnitCost, "Стоимость на ед.").Column("ON_UNIT_COUNT");

            this.Reference(x => x.EstimateCalculation, "Сметный расчет по работе").Column("ESTIMATE_CALC_ID").NotNull().Fetch();
        }
    }
}
