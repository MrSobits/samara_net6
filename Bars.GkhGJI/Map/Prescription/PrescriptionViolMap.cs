/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Нарушение в Предписании"
///     /// </summary>
///     public class PrescriptionViolMap : SubclassMap<PrescriptionViol>
///     {
///         public PrescriptionViolMap()
///         {
///             Table("GJI_PRESCRIPTION_VIOLAT");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.Action, "ACTION").Length(2000);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>Маппинг для "Этап указания к устранению нарушения в предписании"</summary>
    public class PrescriptionViolMap : JoinedSubClassMap<PrescriptionViol>
    {
        public PrescriptionViolMap() : base("Этап указания к устранению нарушения в предписании", "GJI_PRESCRIPTION_VIOLAT") { } 

        protected override void Map()
        {
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.Action, "Мероприятие по устранению").Column("ACTION").Length(2000);
        }
    }
}
