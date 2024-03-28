namespace Bars.Gkh.Map.Administration.EmailMessage
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration;

    public class EmailMessageMap : BaseEntityMap<EmailMessage>
    {
        public EmailMessageMap():
            base("Отправленное письмо", "EMAIL_MESSAGE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.EmailMessageType, "Тип отправляемого сообщения").Column("EMAIL_MESSAGE_TYPE").NotNull();
            this.Property(x => x.EmailAddress, "Адрес электронной почты получателя").Column("EMAIL_ADDRESS");
            this.Property(x => x.AdditionalInfo, "Дополнительные сведения").Column("ADDITIONAL_INFO");
            this.Property(x => x.SendingTime, "Время отправки").Column("SENDING_TIME");
            this.Property(x => x.SendingStatus, "Статус отправки").Column("SENDING_STATUS").NotNull();
            this.Reference(x => x.RecipientContragent, "Получатель письма").Column("RECIPIENT_CONTRAGENT_ID").Fetch();
            this.Reference(x => x.LogFile, "Файл лога").Column("LOG_FILE_ID").Fetch();
        }
    }
}