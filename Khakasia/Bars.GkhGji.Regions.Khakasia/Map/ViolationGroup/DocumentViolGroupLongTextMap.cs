namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.DocumentViolGroupLongText"</summary>
    public class DocumentViolGroupLongTextMap : BaseEntityMap<DocumentViolGroupLongText>
    {
        
        public DocumentViolGroupLongTextMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.DocumentViolGroupLongText", "GJI_DOC_VIOLGROUP_LTEXT")
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
