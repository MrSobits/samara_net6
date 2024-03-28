/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Группа конструктивного элемента"
///     /// </summary>
///     public class ConstructiveElementGroupMap : BaseGkhEntityMap<ConstructiveElementGroup>
///     {
///         public ConstructiveElementGroupMap()
///             : base("GKH_DICT_CONEL_GROUP")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Necessarily, "NECESSARILY");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Группа конструктивного элемента"</summary>
    public class ConstructiveElementGroupMap : BaseImportableEntityMap<ConstructiveElementGroup>
    {
        
        public ConstructiveElementGroupMap() : 
                base("Группа конструктивного элемента", "GKH_DICT_CONEL_GROUP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Necessarily, "Обязательность").Column("NECESSARILY");
        }
    }
}
