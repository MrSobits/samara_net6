/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class RevenueFormCompareEdoMap : BaseGkhEntityMap<RevenueFormCompareEdo>
///     {
///         public RevenueFormCompareEdoMap()
///             : base("INTGEDO_REVENUEFORM")
///         {
///             References(x => x.RevenueForm, "REVENUEFORM_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.CodeEdo, "CODE_EDO").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.RevenueFormCompareEdo"</summary>
    public class RevenueFormCompareEdoMap : BaseEntityMap<RevenueFormCompareEdo>
    {
        
        public RevenueFormCompareEdoMap() : 
                base("Bars.GkhEdoInteg.Entities.RevenueFormCompareEdo", "INTGEDO_REVENUEFORM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CodeEdo, "CodeEdo").Column("CODE_EDO").NotNull();
            Reference(x => x.RevenueForm, "RevenueForm").Column("REVENUEFORM_ID").NotNull().Fetch();
        }
    }
}
