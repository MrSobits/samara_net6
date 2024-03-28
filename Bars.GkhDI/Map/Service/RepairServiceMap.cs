/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Enums;
/// 
///     using Entities;
///     using Enums;
///     using FluentNHibernate.Mapping;
/// 
///     public class RepairServiceMap : SubclassMap<RepairService>
///     {
///         public RepairServiceMap()
///         {
///             Table("DI_REPAIR_SERVICE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.SumWorkTo, "SUM_WORK_TO");
///             Map(x => x.SumFact, "SUM_FACT");
///             Map(x => x.ProgressInfo, "PROGRESS_INFO");
///             Map(x => x.RejectCause, "REJECT_CAUSE");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
/// 
///             Map(x => x.TypeOfProvisionService, "TYPE_OF_PROVISION_SERVICE").Not.Nullable().CustomType<TypeOfProvisionServiceDi>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.RepairService"</summary>
    public class RepairServiceMap : JoinedSubClassMap<RepairService>
    {
        
        public RepairServiceMap() : 
                base("Bars.GkhDi.Entities.RepairService", "DI_REPAIR_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SumWorkTo, "SumWorkTo").Column("SUM_WORK_TO");
            Property(x => x.SumFact, "SumFact").Column("SUM_FACT");
            Property(x => x.ProgressInfo, "ProgressInfo").Column("PROGRESS_INFO");
            Property(x => x.RejectCause, "RejectCause").Column("REJECT_CAUSE");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.TypeOfProvisionService, "TypeOfProvisionService").Column("TYPE_OF_PROVISION_SERVICE").NotNull();
        }
    }
}
