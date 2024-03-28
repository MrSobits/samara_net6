namespace Bars.GkhGji.Map.Email
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Email;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Entities.EmailGjiLongText"</summary>
    public class EmailGjiLongTextMap : BaseEntityMap<EmailGjiLongText>
    {
        public EmailGjiLongTextMap() :
                base("Bars.GkhGji.Entities.EmailGjiLongText", "GJI_EMAIL_LTEXT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.EmailGji, "Родительское письмо").Column("EMAILGJI_ID").NotNull();
            this.Property(x => x.Content, "Содержание").Column("CONTENT");
        }
    }

    public class EmailGjiLongTextNHibernateMapping : ClassMapping<EmailGjiLongText>
    {
        public EmailGjiLongTextNHibernateMapping()
        {
            this.Property(
                x => x.Content,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });
        }
    }
}
