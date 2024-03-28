/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smolensk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Smolensk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class DocumentViolGroupLongTextMap : BaseEntityMap<DocumentViolGroupLongText>
///     {
///         public DocumentViolGroupLongTextMap()
///             : base("GJI_DOC_VIOLGROUP_LTEXT")
///         {
///             this.References(x => x.ViolGroup, "VIOLGROUP_ID");
///             this.Property(
///                 x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///             this.Property(
///                 x => x.Action,
///                 mapper =>
///                 {
///                     mapper.Column("ACTION");
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

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.DocumentViolGroupLongText"</summary>
    public class DocumentViolGroupLongTextMap : BaseEntityMap<DocumentViolGroupLongText>
    {
        
        public DocumentViolGroupLongTextMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.DocumentViolGroupLongText", "GJI_DOC_VIOLGROUP_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolGroup, "ViolGroup").Column("VIOLGROUP_ID");
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.Action, "Action").Column("ACTION");
        }
    }

    public class DocumentViolGroupLongTextNHibernateMapping : ClassMapping<DocumentViolGroupLongText>
    {
        public DocumentViolGroupLongTextNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            Property(
                x => x.Action,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
