/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Stavropol.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Stavropol.Entities;
/// 
///     class ActCheckTimeMap : BaseEntityMap<ActCheckTime>
///     {
///         public ActCheckTimeMap()
///             : base("GJI_STAVROPOL_ACTCHECK_TIME")
///         {
///             Map(x => x.Hour, "HOUR");
///             Map(x => x.Minute, "MINUTE");
/// 
///             References(x => x.ActCheck, "ACT_CHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Stavropol.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Stavropol.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Stavropol.Entities.ActCheckTime"</summary>
    public class ActCheckTimeMap : BaseEntityMap<ActCheckTime>
    {
        
        public ActCheckTimeMap() : 
                base("Bars.GkhGji.Regions.Stavropol.Entities.ActCheckTime", "GJI_STAVROPOL_ACTCHECK_TIME")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Hour, "Hour").Column("HOUR");
            Property(x => x.Minute, "Minute").Column("MINUTE");
            Reference(x => x.ActCheck, "ActCheck").Column("ACT_CHECK_ID").NotNull().Fetch();
        }
    }
}
