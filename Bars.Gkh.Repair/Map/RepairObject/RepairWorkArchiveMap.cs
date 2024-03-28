/// <mapping-converter-backup>
/// namespace Bars.Gkh.Repair.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Repair.Entities;
/// 
///     public class RepairWorkArchiveMap : BaseEntityMap<RepairWorkArchive>
///     {
///         public RepairWorkArchiveMap() : base("RP_TYPE_WORK_ARCH")
///         {
///             Map(x => x.VolumeOfCompletion, "VOLUME_COMPLETION");
///             Map(x => x.PercentOfCompletion, "PERCENT_COMPLETION");
///             Map(x => x.CostSum, "COST_SUM");
///             Map(x => x.DateChangeRec, "DATE_CHANGE_REC");
/// 
///             References(x => x.RepairWork, "REPAIR_WORK_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Repair.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Repair.Entities.RepairWorkArchive"</summary>
    public class RepairWorkArchiveMap : BaseEntityMap<RepairWorkArchive>
    {
        
        public RepairWorkArchiveMap() : 
                base("Bars.Gkh.Repair.Entities.RepairWorkArchive", "RP_TYPE_WORK_ARCH")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.VolumeOfCompletion, "VolumeOfCompletion").Column("VOLUME_COMPLETION");
            Property(x => x.PercentOfCompletion, "PercentOfCompletion").Column("PERCENT_COMPLETION");
            Property(x => x.CostSum, "CostSum").Column("COST_SUM");
            Property(x => x.DateChangeRec, "DateChangeRec").Column("DATE_CHANGE_REC");
            Reference(x => x.RepairWork, "RepairWork").Column("REPAIR_WORK_ID").Fetch();
        }
    }
}
