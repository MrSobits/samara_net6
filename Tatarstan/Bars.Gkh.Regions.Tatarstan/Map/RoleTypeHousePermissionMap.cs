/// <mapping-converter-backup>
/// namespace Bars.Gkh.Regions.Tatarstan.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Regions.Tatarstan.Entities;
/// 
///     public class RoleTypeHousePermissionMap : BaseEntityMap<RoleTypeHousePermission>
///     {
///         public RoleTypeHousePermissionMap()
///             : base("GKH_TAT_ROLE_TYPEHOUSE")
///         {
///             Map(x => x.TypeHouse, "TYPE_HOUSE").Not.Nullable().CustomType<TypeHouse>();
///             References(x => x.Role, "ROLE_ID").Not.Nullable().Fetch.Join(); 
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Tatarstan.Entities.RoleTypeHousePermission"</summary>
    public class RoleTypeHousePermissionMap : BaseEntityMap<RoleTypeHousePermission>
    {
        
        public RoleTypeHousePermissionMap() : 
                base("Bars.Gkh.Regions.Tatarstan.Entities.RoleTypeHousePermission", "GKH_TAT_ROLE_TYPEHOUSE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeHouse, "TypeHouse").Column("TYPE_HOUSE").NotNull();
            Reference(x => x.Role, "Role").Column("ROLE_ID").NotNull().Fetch();
        }
    }
}
