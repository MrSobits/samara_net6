/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smolensk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Smolensk.Entities;
///     using NHibernate.Type;
///     public class ResolutionLongDescriptionMap : BaseEntityMap<ResolutionLongDescription>
///     {
///         public ResolutionLongDescriptionMap()
///             : base("GJI_RESOL_LONGDESC")
///         {
///             References(x => x.Resolution, "RESOLUTION_ID", ReferenceMapConfig.NotNull);
///             Property(
///                 x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.ResolutionLongDescription"</summary>
    public class ResolutionLongDescriptionMap : BaseEntityMap<ResolutionLongDescription>
    {
        
        public ResolutionLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.ResolutionLongDescription", "GJI_RESOL_LONGDESC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Resolution, "Resolution").Column("RESOLUTION_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ResolutionLongDescriptionNHibernateMapping : ClassMapping<ResolutionLongDescription>
    {
        public ResolutionLongDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
