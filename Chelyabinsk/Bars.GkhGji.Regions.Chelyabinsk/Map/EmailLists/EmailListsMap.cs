
namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Исполнитель документа ГЖИ"</summary>
    public class EmailListsMap : BaseEntityMap<EmailLists>
    {
        
        public EmailListsMap() : 
                base("Список рассылок", "GJI_EMAIL_LIST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AnswerNumber, "Номер ответа").Column("ANSWER_NUMBER").NotNull();
            Property(x => x.AppealDate, "Дата обращения").Column("APPEAL_DATE").NotNull();
            Property(x => x.AppealNumber, "Номер обращения").Column("APPEAL_NUMBER");
            Property(x => x.MailTo, "Кому").Column("MAIL_TO");
            Property(x => x.SendDate, "Дата отправки").Column("SEND_DATE");
            Property(x => x.Title, "Заголовок").Column("TITLE");
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID");
        }
    }
}
