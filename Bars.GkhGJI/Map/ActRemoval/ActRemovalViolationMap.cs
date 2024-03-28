/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Устранения нарушения в акте устранения ГЖИ"
///     /// </summary>
///     public class ActRemovalViolationMap : SubclassMap<ActRemovalViolation>
///     {
///         public ActRemovalViolationMap()
///         {
///             Table("GJI_ACTREMOVAL_VIOLAT");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 			Map(x => x.CircumstancesDescription, "CIRCUMSTANCES_DESCRIPTION").Length(500);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Этап устранения нарушения в акте проверки Данная таблица служит связтю между нарушением и Актом устранения Чтобы понимать какие Нарушения были устранены входе проверки Для устраненных нарушений ставится фактическая дата устранения"</summary>
    public class ActRemovalViolationMap : JoinedSubClassMap<ActRemovalViolation>
    {
        
        public ActRemovalViolationMap() : 
                base("Этап устранения нарушения в акте проверки Данная таблица служит связтю между нару" +
                        "шением и Актом устранения Чтобы понимать какие Нарушения были устранены входе пр" +
                        "оверки Для устраненных нарушений ставится фактическая дата устранения", "GJI_ACTREMOVAL_VIOLAT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CircumstancesDescription, "Описание обстоятельств").Column("CIRCUMSTANCES_DESCRIPTION").Length(500);
        }
    }
}
