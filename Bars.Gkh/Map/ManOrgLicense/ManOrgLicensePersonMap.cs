namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    public class ManOrgLicensePersonMap : BaseImportableEntityMap<ManOrgLicensePerson>
    {
        public ManOrgLicensePersonMap():
            base("Должностное лицо лицензии", "GKH_MANORG_LICENSE_PERSON")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ManOrgLicense, "Лицензия").Column("LIC_ID").NotNull();
            this.Reference(x => x.Person, "Должностное лицо").Column("PERSON_ID").NotNull();
        }
    }
}