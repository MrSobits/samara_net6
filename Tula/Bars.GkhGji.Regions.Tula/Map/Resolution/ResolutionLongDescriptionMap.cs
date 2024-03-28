/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     using NHibernate.Type;
/// 
///     /// <summary>
///     /// Маппинг для длинных описаний постановления.
///     /// </summary>
///     public class ResolutionLongDescriptionMap : BaseEntityMap<ResolutionLongDescription>
///     {
///         /// <summary>
///         /// Конструктор.
///         /// </summary>
///         public ResolutionLongDescriptionMap()
///             : base("GJI_RESOLUTION_LONGDESC")
///         {
///             this.References(x => x.Resolution, "RESOLUTION_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.Description,
///                 mapper =>
///                     {
///                         mapper.Column("DESCRIPTION");
///                         mapper.Type<BinaryBlobType>();
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.ResolutionLongDescription"</summary>
    public class ResolutionLongDescriptionMap : BaseEntityMap<ResolutionLongDescription>
    {
        
        public ResolutionLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.ResolutionLongDescription", "GJI_RESOLUTION_LONGDESC")
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
