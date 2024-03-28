/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки деятельности ТСЖ"
///     /// </summary>
///     public class BaseActivityTsjMap : SubclassMap<BaseActivityTsj>
///     {
///         public BaseActivityTsjMap()
///         {
///             Table("GJI_INSPECTION_ACTIVITY");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             References(x => x.ActivityTsj, "ACTIVITY_TSJ_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание деятельность ТСЖ"</summary>
    public class BaseActivityTsjMap : JoinedSubClassMap<BaseActivityTsj>
    {
        
        public BaseActivityTsjMap() : 
                base("Основание деятельность ТСЖ", "GJI_INSPECTION_ACTIVITY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActivityTsj, "Деятельность ТСЖ").Column("ACTIVITY_TSJ_ID").NotNull();
        }
    }
}
