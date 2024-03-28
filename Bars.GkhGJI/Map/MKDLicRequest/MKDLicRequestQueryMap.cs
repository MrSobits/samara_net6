
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Внесение изменений в реестр лицензий - Запрос"</summary>
    public class MKDLicRequestQueryMap : BaseEntityMap<MKDLicRequestQuery>
    {
        
        public MKDLicRequestQueryMap() : 
                base("Запрос переоформлении лицензии запрос", "GJI_MKD_LIC_REQUEST_QUERY")
        {
        }
        
        protected override void Map()
        {
             Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.PerfomanceDate, "Дата исполнения").Column("PERFORMANCE_DATE");
            Property(x => x.PerfomanceFactDate, "Дата фактического исполнения").Column("PERFORMANCE_FACT_DATE");
            Reference(x => x.MKDLicRequest, "ЗапросЛицензии").Column("REQUEST_ID").NotNull().Fetch();
            Reference(x => x.CompetentOrg, "Компетентная организация").Column("COMPETENT_ORG_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_INFO_ID").Fetch();
           
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
        }
    }
}
