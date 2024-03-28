/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class RevenueSourceCompareEdoMap : BaseGkhEntityMap<RevenueSourceCompareEdo>
///     {
///         public RevenueSourceCompareEdoMap()
///             : base("INTGEDO_REVENSOURCE")
///         {
///             References(x => x.RevenueSource, "REVENUESOURCE_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.CodeEdo, "CODE_EDO").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.RevenueSourceCompareEdo"</summary>
    public class RevenueSourceCompareEdoMap : BaseEntityMap<RevenueSourceCompareEdo>
    {
        
        public RevenueSourceCompareEdoMap() : 
                base("Bars.GkhEdoInteg.Entities.RevenueSourceCompareEdo", "INTGEDO_REVENSOURCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CodeEdo, "CodeEdo").Column("CODE_EDO").NotNull();
            Reference(x => x.RevenueSource, "RevenueSource").Column("REVENUESOURCE_ID").NotNull().Fetch();
        }
    }
}
