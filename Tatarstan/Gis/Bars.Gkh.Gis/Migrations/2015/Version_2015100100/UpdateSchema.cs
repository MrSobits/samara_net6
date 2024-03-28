namespace Bars.Gkh.Gis.Migrations._2015.Version_2015100100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015100100")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015092300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", new Column("CALCULATION_DATE", DbType.Date));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "CALCULATION_DATE");
        }
    }
}