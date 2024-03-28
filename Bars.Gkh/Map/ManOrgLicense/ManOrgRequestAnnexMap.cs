namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Приложение заявки на лицензию"</summary>
    public class ManOrgRequestAnnexMap : BaseImportableEntityMap<ManOrgRequestAnnex>
    {
        
        public ManOrgRequestAnnexMap() : 
                base("Приложение заявки на лицензию", "GKH_MANORG_REQ_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicRequest, "Заявка на лицензию").Column("LIC_REQUEST_ID").NotNull();
            Property(x => x.Name, "Наименование").Column("LIC_ANNEX_NAME").Length(200);
            Property(x => x.DocumentNumber, "Номер документа").Column("LIC_ANNEX_NUMBER").Length(100);
            Property(x => x.DocumentDate, "Дата документа").Column("LIC_ANNEX_DATE");
            Property(x => x.Description, "Описание").Column("LIC_ANNEX_DESCR").Length(2000);
            Reference(x => x.File, "Файл").Column("LIC_ANNEX_FILE_ID").Fetch();
            Reference(x => x.DocumentType, "Тип документа").Column("DOCUMENT_TYPE_ID").Fetch();
        }
    }
}
