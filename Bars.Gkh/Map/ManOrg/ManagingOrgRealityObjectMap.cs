/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ManOrg
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Жилой дом управляющей организации"
///     /// </summary>
///     public class ManagingOrgRealityObjectMap : BaseGkhEntityMap<ManagingOrgRealityObject>
///     {
///         public ManagingOrgRealityObjectMap() : base("GKH_MAN_ORG_REAL_OBJ")
///         {
///             References(x => x.ManagingOrganization, "MANAG_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом управляющей организации"</summary>
    public class ManagingOrgRealityObjectMap : BaseImportableEntityMap<ManagingOrgRealityObject>
    {
        
        public ManagingOrgRealityObjectMap() : 
                base("Жилой дом управляющей организации", "GKH_MAN_ORG_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAG_ORG_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
