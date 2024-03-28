/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     public class UserRoleExternalMap : BaseGkhEntityMap<UserRoleExternal>
///     {
///         public UserRoleExternalMap() : base("CONVERTER_UROLE_EXTERNAL")
///         {
///             Map(x => x.UserRoleId, "USER_ROLE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.UserRoleExternal"</summary>
    public class UserRoleExternalMap : BaseImportableEntityMap<UserRoleExternal>
    {
        
        public UserRoleExternalMap() : 
                base("Bars.Gkh.Entities.UserRoleExternal", "CONVERTER_UROLE_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.UserRoleId, "Id роли пользователя в новой").Column("USER_ROLE_ID");
        }
    }
}
