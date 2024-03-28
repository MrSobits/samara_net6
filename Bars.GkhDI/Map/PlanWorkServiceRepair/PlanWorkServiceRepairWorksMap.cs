/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы по плану работ по содержанию и ремонту"
///     /// </summary>
///     public class PlanWorkServiceRepairWorksMap : BaseGkhEntityMap<PlanWorkServiceRepairWorks>
///     {
///         public PlanWorkServiceRepairWorksMap()
///             : base("DI_DISINFO_RO_SRVREP_WORK")
///         {
///             Map(x => x.DataComplete, "DATA_COMPLETE").Length(300);
///             Map(x => x.DateComplete, "DATE_COMPLETE");
///             Map(x => x.Cost, "COST");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.FactCost, "FACT_COST");
///             Map(x => x.ReasonRejection, "REASON_REJECTION").Length(500);
/// 
/// 
///             References(x => x.PlanWorkServiceRepair, "DISINFO_RO_SRVREP_ID").Not.Nullable().Fetch.Join();
///             References(x => x.WorkRepairList, "REPAIR_WORK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.PeriodicityTemplateService, "PERIODICITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.PlanWorkServiceRepairWorks"</summary>
    public class PlanWorkServiceRepairWorksMap : BaseImportableEntityMap<PlanWorkServiceRepairWorks>
    {
        
        public PlanWorkServiceRepairWorksMap() : 
                base("Bars.GkhDi.Entities.PlanWorkServiceRepairWorks", "DI_DISINFO_RO_SRVREP_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DataComplete, "DataComplete").Column("DATA_COMPLETE").Length(300);
            Property(x => x.DateComplete, "DateComplete").Column("DATE_COMPLETE");
            Property(x => x.Cost, "Cost").Column("COST");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.FactCost, "FactCost").Column("FACT_COST");
            Property(x => x.ReasonRejection, "ReasonRejection").Column("REASON_REJECTION").Length(500);
            Reference(x => x.PlanWorkServiceRepair, "PlanWorkServiceRepair").Column("DISINFO_RO_SRVREP_ID").NotNull().Fetch();
            Reference(x => x.WorkRepairList, "WorkRepairList").Column("REPAIR_WORK_ID").NotNull().Fetch();
            Reference(x => x.PeriodicityTemplateService, "PeriodicityTemplateService").Column("PERIODICITY_ID").Fetch();
        }
    }
}
