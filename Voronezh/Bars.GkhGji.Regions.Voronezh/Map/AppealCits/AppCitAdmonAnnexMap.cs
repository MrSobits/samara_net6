namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Приложения в постановлении прокуратуры"</summary>
    public class AppCitAdmonAnnexMap : BaseEntityMap<AppCitAdmonAnnex>
    {
        
        public AppCitAdmonAnnexMap() : 
                base("Обращениям граждан - Предостережение - Приложения", "GJI_CH_APPCIT_ADMON_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.AppealCitsAdmonition, "Предостережение").Column("APPCIT_ADMONITION_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
        }
    }
}
