/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Мап для RoleExternal
///     /// </summary>
///     public class RoleExternalMap : BaseGkhEntityMap<RoleExternal>
///     {
///         public RoleExternalMap() : base("CONVERTER_ROLE_EXTERNAL")
///         {
///             Map(x => x.RoleId, "ROLE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.RoleExternal"</summary>
    public class RoleExternalMap : BaseImportableEntityMap<RoleExternal>
    {
        
        public RoleExternalMap() : 
                base("Bars.Gkh.Entities.RoleExternal", "CONVERTER_ROLE_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.RoleId, "Id роли в новой").Column("ROLE_ID");
        }
    }
}
