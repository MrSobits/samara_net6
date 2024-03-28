/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки постановление прокуратуры"
///     /// </summary>
///     public class BaseProsResolMap : SubclassMap<BaseProsResol>
///     {
///         public BaseProsResolMap()
///         {
///             Table("GJI_INSPECTION_RESOLPROS");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание постановление прокуратуры ГЖИ"</summary>
    public class BaseProsResolMap : JoinedSubClassMap<BaseProsResol>
    {
        
        public BaseProsResolMap() : 
                base("Основание постановление прокуратуры ГЖИ", "GJI_INSPECTION_RESOLPROS")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
