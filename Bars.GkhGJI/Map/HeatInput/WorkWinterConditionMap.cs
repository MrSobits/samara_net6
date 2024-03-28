/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Информация о подготовке ЖКХ к работе в зимних условиях"
///     /// </summary>
///     public class WorkWinterConditionMap : BaseEntityMap<WorkWinterCondition>
///     {
///         public WorkWinterConditionMap()
///             : base("GJI_WORK_WINTER_CONDITION")
///         {
///             References(x => x.HeatInputPeriod, "HEATINPUTPERIOD_ID");
///             References(x => x.WorkInWinterMark, "WORKWINTERMARK_ID");
///             Map(x => x.Total, "WORKWINTER_TOTAL").Nullable();
///             Map(x => x.PreparationTask, "WORKWINTER_PREPTASK").Nullable();
///             Map(x => x.PreparedForWork, "WORKWINTER_PREPWORK").Nullable();
///             Map(x => x.FinishedWorks, "WORKWINTER_FINISHED").Nullable();
///             Map(x => x.Percent, "WORKWINTER_PERCENT").Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.WorkWinterCondition"</summary>
    public class WorkWinterConditionMap : BaseEntityMap<WorkWinterCondition>
    {
        
        public WorkWinterConditionMap() : 
                base("Bars.GkhGji.Entities.WorkWinterCondition", "GJI_WORK_WINTER_CONDITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Total, "Total").Column("WORKWINTER_TOTAL");
            Property(x => x.PreparationTask, "PreparationTask").Column("WORKWINTER_PREPTASK");
            Property(x => x.PreparedForWork, "PreparedForWork").Column("WORKWINTER_PREPWORK");
            Property(x => x.FinishedWorks, "FinishedWorks").Column("WORKWINTER_FINISHED");
            Property(x => x.Percent, "Percent").Column("WORKWINTER_PERCENT");
            Reference(x => x.HeatInputPeriod, "HeatInputPeriod").Column("HEATINPUTPERIOD_ID");
            Reference(x => x.WorkInWinterMark, "WorkInWinterMark").Column("WORKWINTERMARK_ID");
        }
    }
}
