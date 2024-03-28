/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess;
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Запись акта выполненных работ мониторинга СМР"
///     /// </summary>
///     public class PerformedWorkActRecordMap : BaseEstimateMap<PerformedWorkActRecord>
///     {
///         public PerformedWorkActRecordMap()
///             : base("CR_OBJ_PERFOMED_WACT_REC")
///         {
///             References(x => x.PerformedWorkAct, "PERFOMED_ACT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Запись акта выполненных работ мониторинга СМР"</summary>
    public class PerformedWorkActRecordMap : BaseImportableEntityMap<PerformedWorkActRecord>
    {
        
        public PerformedWorkActRecordMap() : 
                base("Запись акта выполненных работ мониторинга СМР", "CR_OBJ_PERFOMED_WACT_REC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Property(x => x.Number, "Number").Column("DOCUMENT_NUM").Length(250);
            Property(x => x.Name, "Name").Column("DOCUMENT_NAME").Length(2000);
            Property(x => x.Reason, "Reason").Column("REASON").Length(1000);
            Property(x => x.MechanicSalary, "MechanicSalary").Column("MECH_SALARY");
            Property(x => x.BaseSalary, "BaseSalary").Column("BASE_SALARY");
            Property(x => x.MechanicWork, "MechanicWork").Column("MECH_WORK");
            Property(x => x.BaseWork, "BaseWork").Column("BASE_WORK");
            Property(x => x.TotalCount, "TotalCount").Column("TOTAL_COUNT");
            Property(x => x.TotalCost, "TotalCost").Column("TOTAL_COST");
            Property(x => x.OnUnitCount, "OnUnitCount").Column("ON_UNIT_COUNT");
            Property(x => x.OnUnitCost, "OnUnitCost").Column("ON_UNIT_COST");
            Property(x => x.MaterialCost, "MaterialCost").Column("MAT_COST");
            Property(x => x.MachineOperatingCost, "MachineOperatingCost").Column("MACHINE_OPERATING_COST");
            Property(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE").Length(300);
            Reference(x => x.PerformedWorkAct, "Акт выполненных работ мониторинга СМР").Column("PERFOMED_ACT_ID").NotNull().Fetch();
        }
    }
}
