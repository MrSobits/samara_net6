namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Запись акта выполненных работ мониторинга СМР"</summary>
    public class SpecialPerformedWorkActRecordMap : BaseImportableEntityMap<SpecialPerformedWorkActRecord>
    {
        
        public SpecialPerformedWorkActRecordMap() : 
                base("Запись акта выполненных работ мониторинга СМР", "CR_SPECIAL_OBJ_PERFOMED_WACT_REC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            this.Property(x => x.Number, "Number").Column("DOCUMENT_NUM").Length(250);
            this.Property(x => x.Name, "Name").Column("DOCUMENT_NAME").Length(2000);
            this.Property(x => x.Reason, "Reason").Column("REASON").Length(1000);
            this.Property(x => x.MechanicSalary, "MechanicSalary").Column("MECH_SALARY");
            this.Property(x => x.BaseSalary, "BaseSalary").Column("BASE_SALARY");
            this.Property(x => x.MechanicWork, "MechanicWork").Column("MECH_WORK");
            this.Property(x => x.BaseWork, "BaseWork").Column("BASE_WORK");
            this.Property(x => x.TotalCount, "TotalCount").Column("TOTAL_COUNT");
            this.Property(x => x.TotalCost, "TotalCost").Column("TOTAL_COST");
            this.Property(x => x.OnUnitCount, "OnUnitCount").Column("ON_UNIT_COUNT");
            this.Property(x => x.OnUnitCost, "OnUnitCost").Column("ON_UNIT_COST");
            this.Property(x => x.MaterialCost, "MaterialCost").Column("MAT_COST");
            this.Property(x => x.MachineOperatingCost, "MachineOperatingCost").Column("MACHINE_OPERATING_COST");
            this.Property(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE").Length(300);

            this.Reference(x => x.PerformedWorkAct, "Акт выполненных работ мониторинга СМР").Column("PERFOMED_ACT_ID").NotNull().Fetch();
        }
    }
}
