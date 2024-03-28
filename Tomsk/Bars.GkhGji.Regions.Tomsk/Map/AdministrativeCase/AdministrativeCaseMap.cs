/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities;
///     using Bars.GkhGji.Regions.Tomsk.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Административное дело"
///     /// </summary>
///     public class AdministrativeCaseMap : SubclassMap<AdministrativeCase>
///     {
///         public AdministrativeCaseMap()
///         {
///             Table("GJI_ADMINCASE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.TypeAdminCaseBase, "TYPE_ADMIN_BASE").Not.Nullable().CustomType<TypeAdminCaseBase>();
///             Map(x => x.DescriptionQuestion, "DESC_QUESTION").Length(2000);
///             Map(x => x.DescriptionSet, "DESC_SET").Length(2000);
///             Map(x => x.DescriptionDefined, "DESC_DEFINED").Length(2000);
/// 
///             References(x => x.RealityObject, "RO_ID").Fetch.Join();
///             References(x => x.Contragent, "CTR_ID").Fetch.Join();
///             References(x => x.Inspector, "INSPECTOR_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCase"</summary>
    public class AdministrativeCaseMap : JoinedSubClassMap<AdministrativeCase>
    {
        
        public AdministrativeCaseMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCase", "GJI_ADMINCASE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeAdminCaseBase, "TypeAdminCaseBase").Column("TYPE_ADMIN_BASE").NotNull();
            Property(x => x.DescriptionQuestion, "DescriptionQuestion").Column("DESC_QUESTION").Length(2000);
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESC_SET").Length(2000);
            Property(x => x.DescriptionDefined, "DescriptionDefined").Column("DESC_DEFINED").Length(2000);
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").Fetch();
            Reference(x => x.Contragent, "Contragent").Column("CTR_ID").Fetch();
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID").Fetch();
        }
    }
}
