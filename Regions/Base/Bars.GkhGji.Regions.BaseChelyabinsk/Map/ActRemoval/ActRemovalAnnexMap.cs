namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Приложения акта проверки предписания ГЖИ"</summary>
    public class ActRemovalAnnexMap : BaseEntityMap<ActRemovalAnnex>
    {

		public ActRemovalAnnexMap() : 
                base("Приложения акта проверки предписания ГЖИ", "GJI_NSO_ACTREMOVAL_ANNEX")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            this.Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
        }
    }
}
