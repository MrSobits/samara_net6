namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;



    /// <summary>Маппинг для "Должностное лицо заявки на переоформление"</summary>
    public class LicenseReissuancePersonMap : BaseEntityMap<LicenseReissuancePerson>
    {
        
        public LicenseReissuancePersonMap() : 
                base("Должностное лицо заявки на переоформление", "GJI_CH_LICENSE_REISSUANCE_PERSON")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicenseReissuance, "Заявка на лицензию").Column("LIC_REISSUANCE_ID").NotNull();
            Reference(x => x.Person, "Должностное лицо").Column("PERSON_ID").NotNull();
        }
    }
}
