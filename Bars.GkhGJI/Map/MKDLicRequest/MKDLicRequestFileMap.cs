namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class MKDLicRequestFileMap : BaseEntityMap<MKDLicRequestFile>
    {
        
        public MKDLicRequestFileMap() : 
                base("Приложение", "GJI_MKD_LIC_STATEMENT_FILE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME");
            Property(x => x.Description, "Discription").Column("DESCRIPTION");
            Property(x => x.DocDate, "DocDate").Column("DOC_DATE");
            Property(x => x.LicStatementDocType, "LicStatementDocType").Column("DOC_TYPE");

            Reference(x => x.MKDLicRequest, "MKDLicRequest").Column("REQUEST_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Inspector").Column("FILE_ID").NotNull().Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
        }
    }
}
