/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class ArchiveDiRoPercentMap : SubclassMap<ArchiveDiRoPercent>
///     {
///         public ArchiveDiRoPercentMap()
///         {
///             Table("DI_ARCH_PERC_REAL_OBJ");
///             KeyColumn("ID");
///             References(x => x.DiRealityObject, "REAL_OBJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.ArchiveDiRoPercent"</summary>
    public class ArchiveDiRoPercentMap : JoinedSubClassMap<ArchiveDiRoPercent>
    {
        
        public ArchiveDiRoPercentMap() : 
                base("Bars.GkhDi.Entities.ArchiveDiRoPercent", "DI_ARCH_PERC_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DiRealityObject, "DiRealityObject").Column("REAL_OBJ_ID").NotNull().Fetch();
        }
    }
}
