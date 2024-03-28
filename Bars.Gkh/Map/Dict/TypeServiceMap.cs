/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Тип обслуживания"
///     /// </summary>
///     public class TypeServiceMap : BaseGkhEntityMap<TypeService>
///     {
///         public TypeServiceMap() : base("GKH_DICT_TYPE_SERVICE")
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
    
    
    /// <summary>Маппинг для "Тип обслуживания"</summary>
    public class TypeServiceMap : BaseImportableEntityMap<TypeService>
    {
        
        public TypeServiceMap() : 
                base("Тип обслуживания", "GKH_DICT_TYPE_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
