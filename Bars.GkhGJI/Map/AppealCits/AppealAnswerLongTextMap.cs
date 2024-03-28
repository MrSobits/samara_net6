namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Описание ответа по обращению"</summary>
    public class AppealAnswerLongTextMap : BaseEntityMap<AppealAnswerLongText>
    {

        public AppealAnswerLongTextMap() :
                base("Описание ответа по обращению", "GJI_CH_APPCIT_ANSWER_LTEXT")
        {

        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCitsAnswer, "Ответ по обращению").Column("APPCIT_ANSWER_ID").NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Property(x => x.Description2, "Описание2").Column("DESCRIPTION2");
        }

        public class AppealAnswerLongTextNHibernateMapping : ClassMapping<AppealAnswerLongText>
        {
            public AppealAnswerLongTextNHibernateMapping()
            {
                this.Property(
                    x => x.Description,
                    mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });

                this.Property(
                    x => x.Description2,
                    mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            }
        }
    }
}
