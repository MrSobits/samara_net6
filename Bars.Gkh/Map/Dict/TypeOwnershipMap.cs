/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Формы собственности"
///     /// </summary>
///     public class TypeOwnershipMap : BaseGkhEntityMap<TypeOwnership>
///     {
///         public TypeOwnershipMap() : base("GKH_DICT_TYPE_OWNERSHIP")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "форма собственности"</summary>
    public class TypeOwnershipMap : BaseImportableEntityMap<TypeOwnership>
    {
        
        public TypeOwnershipMap() : 
                base("форма собственности", "GKH_DICT_TYPE_OWNERSHIP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
