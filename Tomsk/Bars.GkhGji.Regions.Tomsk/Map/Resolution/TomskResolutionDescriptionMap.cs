/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class TomskResolutionDescriptionMap : BaseEntityMap<TomskResolutionDescription>
///     {
///         public TomskResolutionDescriptionMap()
///             : base("GJI_TOMSK_RESOLUTION_DESCR")
///         {
///             this.References(x => x.Resolution, "RESOLUTION_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.ResolutionText,
///                 mapper =>
///                     {
///                         mapper.Column("RESOLUTION_TEXT");
///                         mapper.Type<BinaryBlobType>();
///                     });
/// 
///             this.Property(
///                 x => x.PetitionText,
///                 mapper =>
///                 {
///                     mapper.Column("PETITION_TEXT");
///                     mapper.Type<BinaryBlobType>();
///                 });
/// 
///             this.Property(
///                 x => x.ExplanationText,
///                 mapper =>
///                 {
///                     mapper.Column("EXPLANATION_TEXT");
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.TomskResolutionDescription"</summary>
    public class TomskResolutionDescriptionMap : BaseEntityMap<TomskResolutionDescription>
    {
        
        public TomskResolutionDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.TomskResolutionDescription", "GJI_TOMSK_RESOLUTION_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Resolution, "Resolution").Column("RESOLUTION_ID").NotNull();
            Property(x => x.ResolutionText, "ResolutionText").Column("RESOLUTION_TEXT");
            Property(x => x.PetitionText, "PetitionText").Column("PETITION_TEXT");
            Property(x => x.ExplanationText, "ExplanationText").Column("EXPLANATION_TEXT");
        }
    }

    public class TomskResolutionDescriptionNHibernateMapping : ClassMapping<TomskResolutionDescription>
    {
        public TomskResolutionDescriptionNHibernateMapping()
        {
            Property(
                x => x.ResolutionText,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            Property(
                x => x.PetitionText,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

            Property(
                x => x.ExplanationText,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
