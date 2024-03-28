/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Этап нарушения проверки ГЖИ"
///     /// </summary>
///     public class InspectionGjiViolStageMap : BaseGkhEntityMap<InspectionGjiViolStage>
///     {
///         public InspectionGjiViolStageMap()
///             : base("GJI_INSPECTION_VIOL_STAGE")
///         {
///             Map(x => x.TypeViolationStage, "TYPE_VIOL_STAGE").Not.Nullable().CustomType<TypeViolationStage>();
///             Map(x => x.DatePlanRemoval, "DATE_PLAN_REMOVAL");
///             Map(x => x.DateFactRemoval, "DATE_FACT_REMOVAL");
///             Map(x => x.SumAmountWorkRemoval, "SUM_AMOUNT_REMOVAL");
/// 
///             References(x => x.Document, "DOCUMENT_ID").Not.Nullable().LazyLoad();
///             References(x => x.InspectionViolation, "INSPECTION_VIOL_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Этап нарушения"</summary>
    public class InspectionGjiViolStageMap : BaseEntityMap<InspectionGjiViolStage>
    {
        
        public InspectionGjiViolStageMap() : 
                base("Этап нарушения", "GJI_INSPECTION_VIOL_STAGE")
        {
        }
        
        protected override void Map()
        { 
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeViolationStage, "Тип этапа нарушения").Column("TYPE_VIOL_STAGE").NotNull();
            this.Property(x => x.DatePlanRemoval, "Плановая дата устранения").Column("DATE_PLAN_REMOVAL");
            this.Property(x => x.DateFactRemoval, "фактическая дата устранения").Column("DATE_FACT_REMOVAL");
            this.Property(x => x.SumAmountWorkRemoval, "сумма работ по устранению нарушений").Column("SUM_AMOUNT_REMOVAL");
            this.Reference(x => x.Document, "Документ ГЖИ").Column("DOCUMENT_ID").NotNull();
            this.Reference(x => x.InspectionViolation, "Нарушение проверки").Column("INSPECTION_VIOL_ID").NotNull().Fetch();
            this.Property(x => x.ErpGuid, nameof(InspectionGjiViolStage.ErpGuid)).Column("ERP_GUID");
        }
    }
}
