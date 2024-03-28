/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Тип проекта"
///     /// </summary>
///     public class TypeProjectMap : BaseGkhEntityMap<TypeProject>
///     {
///         public TypeProjectMap()
///             : base("GKH_DICT_TYPE_PROJ")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Not.Nullable().Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Тип проекта"</summary>
    public class TypeProjectMap : BaseImportableEntityMap<TypeProject>
    {
        
        public TypeProjectMap() : 
                base("Тип проекта", "GKH_DICT_TYPE_PROJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
