namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112500
{
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112500")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "INTERNAL_ID", DbType.Int64);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "INTERNAL_ID");
        }
    }
}