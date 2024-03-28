namespace Bars.Gkh.RegOperator.Map.EmailNewsletter
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;


    /// <summary>Маппинг для "Лог рассылки"</summary>
    public class EmailNewsletterLogMap : BaseEntityMap<EmailNewsletterLog>
    {

        public EmailNewsletterLogMap() :
                base("Период начислений", "EMAIL_NEWSLETTER_LOG")
        {
        }

        protected override void Map()
        {
            Reference(x => x.EmailNewsletter, "Рассылка").Column("EMAIL_NEWSLETTER_ID").NotNull();
            Property(x => x.Destination, "Адресат").Column("DESTINATION");
            Property(x => x.Log, "Лог").Column("EMAIL_LOG").Length(1000);
            Property(x => x.Success, "Успех?").Column("SUCCESS");
        }
    }
}