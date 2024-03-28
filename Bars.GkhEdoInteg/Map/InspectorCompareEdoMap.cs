/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class InspectorCompareEdoMap : BaseGkhEntityMap<InspectorCompareEdo>
///     {
///         public InspectorCompareEdoMap()
///             : base("INTGEDO_INSPECTOR")
///         {
///             References(x => x.Inspector, "INSPECTOR_ID").Not.Nullable().Fetch.Join();
///             Map(x => x.CodeEdo, "CODE_EDO").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.InspectorCompareEdo"</summary>
    public class InspectorCompareEdoMap : BaseEntityMap<InspectorCompareEdo>
    {
        
        public InspectorCompareEdoMap() : 
                base("Bars.GkhEdoInteg.Entities.InspectorCompareEdo", "INTGEDO_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CodeEdo, "CodeEdo").Column("CODE_EDO").NotNull();
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}
