/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Мап для UserExternal
///     /// </summary>
///     public class UserExternalMap : BaseGkhEntityMap<UserExternal>
///     {
///         public UserExternalMap() : base("CONVERTER_USER_EXTERNAL")
///         {
///             Map(x => x.UserId, "USER_ID");
///             Map(x => x.UserPasswordId, "USER_PASSWORD_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.UserExternal"</summary>
    public class UserExternalMap : BaseImportableEntityMap<UserExternal>
    {
        
        public UserExternalMap() : 
                base("Bars.Gkh.Entities.UserExternal", "CONVERTER_USER_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.UserId, "Id пользователя в новой").Column("USER_ID");
            Property(x => x.UserPasswordId, "UserPasswordId").Column("USER_PASSWORD_ID");
        }
    }
}
