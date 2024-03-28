namespace Bars.Gkh1468.Migrations.Version_2013112700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013112500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_HOUSE_PROV_PASSPORT", new RefColumn("CERTIFICATE_ID", ColumnProperty.Null, "GKH_HPP_CERT", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GKH_OKI_PROV_PASSPORT", new RefColumn("CERTIFICATE_ID", ColumnProperty.Null, "GKH_OPP_CERT", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OKI_PROV_PASSPORT", "CERTIFICATE_ID");
            Database.RemoveColumn("GKH_HOUSE_PROV_PASSPORT", "CERTIFICATE_ID");
        }
    }
}