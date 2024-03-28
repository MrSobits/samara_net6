/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "инспектора проверки"
///     /// </summary>
///     public class InspectionGjiInspectorMap : BaseGkhEntityMap<InspectionGjiInspector>
///     {
///         public InspectionGjiInspectorMap()
///             : base("GJI_INSPECTION_INSPECTOR")
///         {
///             LazyLoad();
///             References(x => x.Inspection, "INSPECTION_ID").Not.Nullable().LazyLoad();
///             References(x => x.Inspector, "INSPECTOR_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Инспектора в проверке"</summary>
    public class InspectionGjiInspectorMap : BaseEntityMap<InspectionGjiInspector>
    {
        
        public InspectionGjiInspectorMap() : 
                base("Инспектора в проверке", "GJI_INSPECTION_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Inspection, "Проверка ГЖИ").Column("INSPECTION_ID").NotNull();
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}
