/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class AdministrativeCaseDescriptionMap : BaseEntityMap<AdministrativeCaseDescription>
///     {
///         public AdministrativeCaseDescriptionMap()
///             : base("GJI_ADMINCASE_DESCR")
///         {
///             this.References(x => x.AdministrativeCase, "ADMIN_CASE_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.DescriptionSet,
///                 mapper =>
///                     {
///                         mapper.Column("DESCRIPTION_SET");
///                         mapper.Type<BinaryBlobType>();
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseDescription"</summary>
    public class AdministrativeCaseDescriptionMap : BaseEntityMap<AdministrativeCaseDescription>
    {
        
        public AdministrativeCaseDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseDescription", "GJI_ADMINCASE_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AdministrativeCase, "AdministrativeCase").Column("ADMIN_CASE_ID").NotNull();
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESCRIPTION_SET");
        }
    }

    public class AdministrativeCaseDescriptionNHibernateMapping : ClassMapping<AdministrativeCaseDescription>
    {
        public AdministrativeCaseDescriptionNHibernateMapping()
        {
            Property(
                x => x.DescriptionSet,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
