namespace Bars.Gkh.Map.Dict
{
	using B4.Modules.Mapping.Mappers;
	using Gkh.Entities.Dicts;

	/// <summary>Маппинг для "Справочники - ГЖИ - Приложения к обращению за выдачей лицензии"</summary>
	public class AnnexToAppealForLicenseIssuanceMap : BaseImportableEntityMap<AnnexToAppealForLicenseIssuance>
    {
        
        public AnnexToAppealForLicenseIssuanceMap() : 
                base("Справочники - ГЖИ - Приложения к обращению за выдачей лицензии", "GKH_DICT_ANNEX_APPEAL_LIC_ISS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(500);
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
