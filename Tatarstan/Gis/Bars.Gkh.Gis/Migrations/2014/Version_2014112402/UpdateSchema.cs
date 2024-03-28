namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112402
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2014112402")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", new Column("ISPUBLISHED", DbType.Boolean, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "ISPUBLISHED");
        }
    }
}