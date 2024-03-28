namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для "Основание проверки лицензиата"</summary>
    public class BaseLicenseReissuanceMap : JoinedSubClassMap<BaseLicenseReissuance>
    {

        public BaseLicenseReissuanceMap() :
                base("Основание проверки соискателей лицензии", "GJI_INSPECTION_LIC_REISS")
        {
        }

        protected override void Map()
        {
            Property(x => x.FormCheck, "Форма проверки").Column("FORM_CHECK").NotNull();
            Reference(x => x.LicenseReissuance, "Обращение за переоформлением лицензии").Column("LIC_REISSUANCE_ID").Fetch();
        }
    }
}
