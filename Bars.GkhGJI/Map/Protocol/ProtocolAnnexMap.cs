
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения протокола ГЖИ"</summary>
    public class ProtocolAnnexMap : BaseEntityMap<ProtocolAnnex>
    {
        
        public ProtocolAnnexMap() : 
                base("Приложения протокола ГЖИ", "GJI_PROTOCOL_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.TypeAnnex, "TypeAnnex").Column("ANNEX_TYPE").NotNull();
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
            Property(x => x.DocumentSend, "Дата документа").Column("DOCUMENT_SEND");
            Property(x => x.DocumentDelivered, "Дата документа").Column("DOCUMENT_DELIV");
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.SendFileToErknm, "SendFileToErknm").DefaultValue(YesNoNotSet.NotSet).Column("SEND_FILE_TO_ERKNM");
            this.Property(x => x.ErknmGuid, "Идентификатор в ЕРКНМ").Column("ERKNM_GUID").Length(36);
            this.Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
