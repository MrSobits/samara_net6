/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки протокол МВД"
///     /// </summary>
///     public class BaseProtocolMvdMap : SubclassMap<BaseProtocolMvd>
///     {
///         public BaseProtocolMvdMap()
///         {
///             Table("GJI_INSPECTION_PROTMVD");
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
    
    
    /// <summary>Маппинг для "Основание Протокол МВД"</summary>
    public class BaseProtocolMvdMap : JoinedSubClassMap<BaseProtocolMvd>
    {
        
        public BaseProtocolMvdMap() : 
                base("Основание Протокол МВД", "GJI_INSPECTION_PROTMVD")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
