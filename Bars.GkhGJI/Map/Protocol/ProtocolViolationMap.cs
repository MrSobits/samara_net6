/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Нарушение в Протоколе"
///     /// </summary>
///     public class ProtocolViolationMap : SubclassMap<ProtocolViolation>
///     {
///         public ProtocolViolationMap()
///         {
///             Table("GJI_PROTOCOL_VIOLAT");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.Description, "DESCRIPTION").Length(1000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Этап наказания за нарушение в протоколе"</summary>
    public class ProtocolViolationMap : JoinedSubClassMap<ProtocolViolation>
    {
        
        public ProtocolViolationMap() : 
                base("Этап наказания за нарушение в протоколе", "GJI_PROTOCOL_VIOLAT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(1000);
        }
    }
}
