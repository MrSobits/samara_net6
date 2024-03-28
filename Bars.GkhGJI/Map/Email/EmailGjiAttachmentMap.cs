namespace Bars.GkhGji.Map.Email
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Email;


    /// <summary>Маппинг для вложения письма ГЖИ</summary>
    public class EmailGjiAttachmentMap : BaseEntityMap<EmailGjiAttachment>
    {
        
        public EmailGjiAttachmentMap() : 
                base("Вложение письма ГЖИ", "GJI_EMAIL_ATTACHMENT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Message, "Родительское письмо").Column("EMAILGJI_ID").NotNull();
            this.Reference(x => x.AttachmentFile, "Файл вложения").Column("ATT_FILE_ID").NotNull();
        }
    }
}