/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Gkh.Map;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Ведомость ресурсов"
///     /// </summary>
///     public class ResourceStatementMap : BaseGkhEntityByCodeMap<ResourceStatement>
///     {
///         public ResourceStatementMap()
///             : base("CR_EST_CALC_RES_STATEM")
///         {
///             Map(x => x.Number, "DOCUMENT_NUM", false, 50);
///             Map(x => x.Name, "DOCUMENT_NAME", false, 300);
///             Map(x => x.Reason, "REASON", false, 300);
///             Map(x => x.UnitMeasure, "UNIT_MEASURE", false);
///             Map(x => x.TotalCount, "TOTAL_COUNT");
///             Map(x => x.TotalCost, "TOTAL_COST");
///             Map(x => x.OnUnitCost, "ON_UNIT_COUNT");
/// 
///             References(x => x.EstimateCalculation, "ESTIMATE_CALC_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Ведомость ресурсов"</summary>
    public class ResourceStatementMap : BaseImportableEntityMap<ResourceStatement>
    {
        
        public ResourceStatementMap() : 
                base("Ведомость ресурсов", "CR_EST_CALC_RES_STATEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.EstimateCalculation, "Сметный расчет по работе").Column("ESTIMATE_CALC_ID").NotNull().Fetch();
            Property(x => x.Number, "Номер").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE").Length(250);
            Property(x => x.Name, "Наименование").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.Reason, "Обоснование").Column("REASON").Length(300);
            Property(x => x.TotalCount, "Количество всего").Column("TOTAL_COUNT");
            Property(x => x.TotalCost, "Общая стоимость").Column("TOTAL_COST");
            Property(x => x.OnUnitCost, "Стоимость на ед.").Column("ON_UNIT_COUNT");
        }
    }
}
