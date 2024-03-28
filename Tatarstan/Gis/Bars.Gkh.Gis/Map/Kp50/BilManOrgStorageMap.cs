namespace Bars.Gkh.Gis.Map.Kp50
{
    using B4.Modules.Mapping.Mappers;
    using Entities.Kp50;

    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Kp50.BilManOrgStorage"</summary>
    public class BilManOrgStorageMap : PersistentObjectMap<BilManOrgStorage>
    {

        public BilManOrgStorageMap() :
            base("Bars.Gkh.Gis.Entities.Kp50.BilManOrgStorage", "BIL_MANORG_STORAGE")
        {
        }

        protected override void Map()
        {
            Property(x => x.ManOrgCode, "ManOrgCode").Column("ManOrg_Code").NotNull();
            Property(x => x.ManOrgName, "ManOrgName").Column("ManOrg_Name").Length(150);

            Property(x => x.ManOrgInn, "ManOrgInn").Column("ManOrg_Inn").Length(150);
            Property(x => x.ManOrgKpp, "ManOrgKpp").Column("ManOrg_Kpp").Length(150);
            Property(x => x.ManOrgOgrn, "ManOrgOgrn").Column("ManOrg_Ogrn").Length(150);
            Property(x => x.ManOrgAddress, "ManOrgAddress").Column("ManOrg_Address").Length(150);

            Reference(x => x.Schema, "Schema").Column("bil_dict_schema_id").NotNull().Fetch();
        }
    }
}