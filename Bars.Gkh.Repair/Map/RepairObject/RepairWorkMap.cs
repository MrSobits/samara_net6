/// <mapping-converter-backup>
/// namespace Bars.Gkh.Repair.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Repair.Entities;
/// 
///     public class RepairWorkMap : BaseEntityMap<RepairWork>
///     {
///         public RepairWorkMap()
///             : base("RP_TYPE_WORK")
///         {
///             References(x => x.RepairObject, "RP_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Work, "RP_WORK_CRP_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.Volume, "VOLUME");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.VolumeOfCompletion, "VOLUME_COMPLETION");
///             Map(x => x.PercentOfCompletion, "PERCENT_COMPLETION");
///             Map(x => x.CostSum, "COST_SUM");
///             Map(x => x.AdditionalDate, "ADD_DATE_END");
///             Map(x => x.Builder, "BUILDER");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Repair.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Repair.Entities.RepairWork"</summary>
    public class RepairWorkMap : BaseEntityMap<RepairWork>
    {
        
        public RepairWorkMap() : 
                base("Bars.Gkh.Repair.Entities.RepairWork", "RP_TYPE_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Volume, "Volume").Column("VOLUME");
            Property(x => x.Sum, "Sum").Column("SUM");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.VolumeOfCompletion, "VolumeOfCompletion").Column("VOLUME_COMPLETION");
            Property(x => x.PercentOfCompletion, "PercentOfCompletion").Column("PERCENT_COMPLETION");
            Property(x => x.CostSum, "CostSum").Column("COST_SUM");
            Property(x => x.AdditionalDate, "AdditionalDate").Column("ADD_DATE_END");
            Property(x => x.Builder, "Builder").Column("BUILDER");
            Reference(x => x.RepairObject, "RepairObject").Column("RP_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Work, "Work").Column("RP_WORK_CRP_ID").NotNull().Fetch();
        }
    }
}
