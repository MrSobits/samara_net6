/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     public class AppealCitsRealObjectMap : BaseGkhEntityMap<AppealCitsRealityObject>
///     {
///         public AppealCitsRealObjectMap()
///             : base("GJI_APPCIT_RO")
///         {
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.AppealCits, "APPCIT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.AppealCitsRealityObject"</summary>
    public class AppealCitsRealityObjectMap : BaseEntityMap<AppealCitsRealityObject>
    {
        
        public AppealCitsRealityObjectMap() : 
                base("Bars.GkhGji.Entities.AppealCitsRealityObject", "GJI_APPCIT_RO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
        }
    }
}
