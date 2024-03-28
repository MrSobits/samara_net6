
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения распоряжения ГЖИ"</summary>
    public class PreventiveVisitAnnexMap : BaseEntityMap<PreventiveVisitAnnex>
    {
        
        public PreventiveVisitAnnexMap() : 
                base("Приложения распоряжения ГЖИ", "GJI_PREVENTIVE_VISIT_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.TypeAnnex, "TypeAnnex").Column("ANNEX_TYPE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            Reference(x => x.PreventiveVisit, "Акт").Column("PREVENT_VISIT_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
        }
    }
}
