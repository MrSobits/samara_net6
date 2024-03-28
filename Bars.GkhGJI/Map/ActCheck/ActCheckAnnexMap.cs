/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложения акта проверки ГЖИ"
///     /// </summary>
///     public class ActCheckAnnexMap : BaseGkhEntityMap<ActCheckAnnex>
///     {
///         public ActCheckAnnexMap() : base("Gji_ACTCHECK_ANNEX")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ActCheck, "ACTCHECK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения акта проверки ГЖИ"</summary>
    public class ActCheckAnnexMap : BaseEntityMap<ActCheckAnnex>
    {
        
        public ActCheckAnnexMap() : 
                base("Приложения акта проверки ГЖИ", "GJI_ACTCHECK_ANNEX")
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
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.SendFileToErknm, "SendFileToErknm").DefaultValue(YesNoNotSet.NotSet).Column("SEND_FILE_TO_ERKNM");
            this.Property(x => x.ErknmGuid, "Идентификатор в ЕРКНМ").Column("ERKNM_GUID").Length(36);
            this.Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
