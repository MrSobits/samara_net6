namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112800
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112800")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GIS_HOUSE_SERVICE_REGISTER", new RefColumn("LOADEDFILE", "GIS_HS_SRV_REG_LDFILE", "GIS_LOADED_FILE_REGISTER", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "LOADEDFILE");
        }
    }
}