
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Внесение изменений в реестр лицензий - Запрос ответ "</summary>
    public class MKDLicRequestQueryAnswerMap : BaseEntityMap<MKDLicRequestQueryAnswer>
    {
        
        public MKDLicRequestQueryAnswerMap() : 
                base("Обращениям граждан - Запрос", "GJI_MKD_LIC_REQUEST_QUERY_ANSWER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.MKDLicRequestQuery, "Запрос инфы").Column("REQUEST_QUERY_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").NotNull().Fetch();
           
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
        }
    }
}
