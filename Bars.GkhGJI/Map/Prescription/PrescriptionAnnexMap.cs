
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения предписания ГЖИ"</summary>
    public class PrescriptionAnnexMap : BaseEntityMap<PrescriptionAnnex>
    {
        
        public PrescriptionAnnexMap() : 
                base("Приложения предписания ГЖИ", "GJI_PRESCRIPTION_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypePrescriptionAnnex, "Тип приложения").Column("ANNEX_TYPE");
            Property(x => x.Number, "Номер документа").Column("NUMBER").Length(50);
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.TypeAnnex, "TypeAnnex").Column("TYPE_ANNEX").NotNull();
            Property(x => x.DocumentSend, "Дата документа").Column("DOCUMENT_SEND");
            Property(x => x.DocumentDelivered, "Дата документа").Column("DOCUMENT_DELIV");
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.SendFileToErknm, "SendFileToErknm").DefaultValue(YesNoNotSet.NotSet).Column("SEND_FILE_TO_ERKNM");
            this.Property(x => x.ErknmGuid, "Идентификатор в ЕРКНМ").Column("ERKNM_GUID").Length(36);
            this.Reference(x => x.Prescription, "Предписание").Column("PRESCRIPTION_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
