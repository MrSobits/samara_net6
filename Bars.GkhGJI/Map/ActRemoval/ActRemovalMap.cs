/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт устранения"
///     /// </summary>
///     public class ActRemovalMap : SubclassMap<ActRemoval>
///     {
///         public ActRemovalMap()
///         {
///             Table("Gji_ACTREMOVAL");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.TypeRemoval, "TYPE_REMOVAL").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.Area, "AREA");
///             Map(x => x.Flat, "FLAT").Length(250);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Акт проверки устранения нарушений В данном документе по нарушениям идет устранение и если нарушение устранено то выставляется дата устранения"</summary>
    public class ActRemovalMap : JoinedSubClassMap<ActRemoval>
    {
        
        public ActRemovalMap() : 
                base("Акт проверки устранения нарушений В данном документе по нарушениям идет устранени" +
                        "е и если нарушение устранено то выставляется дата устранения", "GJI_ACTREMOVAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeRemoval, "Признак устранено или неустранено нарушение").Column("TYPE_REMOVAL").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.Area, "Площадь").Column("AREA");
            Property(x => x.Flat, "Квартира").Column("FLAT").Length(250);

        }
    }
}
