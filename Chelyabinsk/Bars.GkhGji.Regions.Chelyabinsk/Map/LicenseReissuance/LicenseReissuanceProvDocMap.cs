namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Предоставляемый документ заявки на лицензию"</summary>
    public class LicenseReissuanceProvDocMap : BaseEntityMap<LicenseReissuanceProvDoc>
    {
        
        public LicenseReissuanceProvDocMap() : 
                base("Предоставляемый документ заявки на переоформление", "GJI_CH_LICENSE_REISSUANCE_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicenseReissuance, "Заявка на переоформление").Column("LIC_REISSUANCE_ID").NotNull();
            Reference(x => x.LicProvidedDoc, "Предосталвяемы документ заявки на переоформление").Column("LIC_PROVDOC_ID").NotNull();
            Property(x => x.Number, "Номер предоставляемого документа").Column("LIC_PROVDOC_NUMBER").Length(100);
            Property(x => x.Date, "Дата предоставляемого документа").Column("LIC_PROVDOC_DATE");
            Reference(x => x.File, "Файл предоставляемого документа").Column("LIC_PROVDOC_FILE_ID").Fetch();
        }
    }
}
