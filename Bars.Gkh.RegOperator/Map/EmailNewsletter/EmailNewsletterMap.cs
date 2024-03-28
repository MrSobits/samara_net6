namespace Bars.Gkh.RegOperator.Map.EmailNewsletter
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;


    /// <summary>Маппинг для "Лог рассылки"</summary>
    public class EmailNewsletterMap : BaseEntityMap<EmailNewsletter>
    {

        public EmailNewsletterMap() :
                base("Период начислений", "EMAIL_NEWSLETTER")
        {
        }

        protected override void Map()
        {
            Property(x => x.Header, "Тема").Column("HEADER").Length(500);
            Property(x => x.Body, "Содержание").Column("BODY").Length(1500);
            Property(x => x.Destinations, "Адресаты через запятую").Column("DESTINATIONS").Length(1000);
            Property(x => x.Success, "Успешно?").Column("SUCCESS").NotNull();
            Reference(x => x.Attachment, "Вложение").Column("ATTACHMENT_ID");
            Property(x => x.Sender, "Оператор отправителя").Column("SENDER");
            Property(x => x.SendDate, "Дата отправления").Column("SEND_DATE");
        }
    }
}