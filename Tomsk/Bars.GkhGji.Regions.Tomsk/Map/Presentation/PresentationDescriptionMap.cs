/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class PresentationDescriptionMap : BaseEntityMap<PresentationDescription>
///     {
///         public PresentationDescriptionMap()
///             : base("GJI_PRESENT_DESCR")
///         {
///             this.References(x => x.Presentation, "PRESEN_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.DescriptionSet,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION_SET");
///                     mapper.Type<BinaryBlobType>();
///                 });
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.PresentationDescription"</summary>
    public class PresentationDescriptionMap : BaseEntityMap<PresentationDescription>
    {
        
        public PresentationDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.PresentationDescription", "GJI_PRESENT_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Presentation, "Presentation").Column("PRESEN_ID").NotNull();
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESCRIPTION_SET");
        }
    }

    public class PresentationDescriptionNHibernateMapping : ClassMapping<PresentationDescription>
    {
        public PresentationDescriptionNHibernateMapping()
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
