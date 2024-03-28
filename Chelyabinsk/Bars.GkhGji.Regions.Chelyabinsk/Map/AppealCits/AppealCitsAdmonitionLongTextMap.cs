
namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Ответ по обращению"</summary>
    public class AppealCitsAdmonitionLongTextMap : BaseEntityMap<AppealCitsAdmonitionLongText>
    {

        public AppealCitsAdmonitionLongTextMap() :
                base("Ответ по обращению", "GJI_CH_APPCIT_ADMONITION_LTEXT")
        {

        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCitsAdmonition, "AppealCitsAdmonition").Column("ADMONITION_ID").NotNull();
            this.Property(x => x.Measures, "Victims").Column("MEASURES");
            this.Property(x => x.Violations, "Violations").Column("VIOLATION");
        }

        public class AppealCitsAdmonitionLongTextNHibernateMapping : ClassMapping<AppealCitsAdmonitionLongText>
        {
            public AppealCitsAdmonitionLongTextNHibernateMapping()
            {
                this.Property(
                    x => x.Measures,
                    mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
                this.Property(
                    x => x.Violations,
                    mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            }
        }
    }
}
