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
///     public class BaseProtocolMhcMap : SubclassMap<BaseProtocolMhc>
///     {
///         public BaseProtocolMhcMap()
///         {
///             Table("GJI_INSPECTION_PROTMHC");
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
    
    
    /// <summary>Маппинг для "Основание протокола МЖК"</summary>
    public class BaseProtocolMhcMap : JoinedSubClassMap<BaseProtocolMhc>
    {
        
        public BaseProtocolMhcMap() : 
                base("Основание протокола МЖК", "GJI_INSPECTION_PROTMHC")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
