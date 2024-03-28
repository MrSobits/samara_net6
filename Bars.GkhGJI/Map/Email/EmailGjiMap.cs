namespace Bars.GkhGji.Map.Email
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Email;


    /// <summary>Маппинг для "Письмо ГЖИ"</summary>
    public class EmailGjiMap : BaseEntityMap<EmailGji>
    {
        
        public EmailGjiMap() : 
                base("Письмо ГЖИ", "GJI_EMAIL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.From, "От кого").Column("EMAIL_FROM");
            this.Property(x => x.SenderInfo, "Подпись отправителя").Column("SENDER_INFO");
            this.Property(x => x.Theme, "Тема").Column("LETTER_THEME");
            this.Property(x => x.EmailDate, "Дата письма").Column("EMAIL_DATE");
            this.Property(x => x.GjiNumber, "Номер ГЖИ").Column("GJI_NUMBER");
            this.Property(x => x.SystemNumber, "Номер ГЖИ").Column("SYS_NUMBER");
            this.Property(x => x.LivAddress, "Номер ГЖИ").Column("LIV_ADDRESS");
            this.Property(x => x.EmailType, "Тип письма").Column("EMAIL_TYPE").NotNull();
            this.Property(x => x.EmailGjiSource, "Источник письма").Column("APPEAL_SOURCE").NotNull();
            this.Property(x => x.Registred, "Зарегестрированно").Column("IS_REGISTRED").NotNull();
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            this.Reference(x => x.EmailPdf, "Pdf-файл письма").Column("EMAIL_PDF_ID");
            this.Property(x => x.EmailDenailReason, "Тип письма").Column("DECLINE_TYPE").NotNull();
            this.Property(x => x.DeclineReason, "Тип письма").Column("DECLINE_REASON");
        }
    }
}