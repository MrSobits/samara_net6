/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Нарушение Админ деле ГЖИ"
///     /// </summary>
///     public class AdministrativeCaseViolationMap : SubclassMap<AdministrativeCaseViolation>
///     {
///         public AdministrativeCaseViolationMap()
///         {
///             Table("GJI_ADMINCASE_VIOLAT");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseViolation"</summary>
    public class AdministrativeCaseViolationMap : JoinedSubClassMap<AdministrativeCaseViolation>
    {
        
        public AdministrativeCaseViolationMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseViolation", "GJI_ADMINCASE_VIOLAT")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
