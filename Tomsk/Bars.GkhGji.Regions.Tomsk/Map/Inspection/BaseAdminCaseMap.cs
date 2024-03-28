/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки административного дела"
///     /// </summary>
///     public class BaseAdminCaseMap : SubclassMap<BaseAdminCase>
///     {
///         public BaseAdminCaseMap()
///         {
///             Table("GJI_INSPECTION_ADMINCASE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.BaseAdminCase"</summary>
    public class BaseAdminCaseMap : JoinedSubClassMap<BaseAdminCase>
    {
        
        public BaseAdminCaseMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.BaseAdminCase", "GJI_INSPECTION_ADMINCASE")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
