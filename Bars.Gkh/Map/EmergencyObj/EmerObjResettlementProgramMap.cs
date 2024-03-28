/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.EmergencyObj
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Программа переселения"
///     /// </summary>
///     public class EmerObjResettlementProgramMap : BaseGkhEntityMap<EmerObjResettlementProgram>
///     {
///         public EmerObjResettlementProgramMap() : base("GKH_EMERGENCY_RESETPROG")
///         {
///             Map(x => x.Area, "AREA");
///             Map(x => x.Cost, "COST");
///             Map(x => x.CountResidents, "COUNT_RESIDENTS");
/// 
///             References(x => x.EmergencyObject, "EMERGENCY_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ResettlementProgramSource, "RESETTLEMENT_SOURCE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Программа переселения"</summary>
    public class EmerObjResettlementProgramMap : BaseImportableEntityMap<EmerObjResettlementProgram>
    {
        
        public EmerObjResettlementProgramMap() : 
                base("Программа переселения", "GKH_EMERGENCY_RESETPROG")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Area, "Площадь").Column("AREA");
            this.Property(x => x.Cost, "Плановая cтоимость").Column("COST");
            this.Property(x => x.ActualCost, "Фактическая cтоимость").Column("ACTUAL_COST");
            this.Property(x => x.CountResidents, "Количество жителей").Column("COUNT_RESIDENTS");
            this.Reference(x => x.EmergencyObject, "Аварийность жилого дома").Column("EMERGENCY_OBJ_ID").NotNull().Fetch();
            this.Reference(x => x.ResettlementProgramSource, "Источник по программе переселения").Column("RESETTLEMENT_SOURCE_ID").NotNull().Fetch();
        }
    }
}
