namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2014120200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_HOUSE_REGISTER", new Column("AREA_OWNED", DbType.Decimal));
            this.Database.AddColumn("GIS_HOUSE_REGISTER", new Column("AREA_LIV_NOT_LIV_MKD", DbType.Decimal));
            this.Database.AddColumn("GIS_HOUSE_REGISTER", new Column("NUMBER_APARTMENTS", DbType.Int32));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_HOUSE_REGISTER", "AREA_OWNED");
            this.Database.RemoveColumn("GIS_HOUSE_REGISTER", "AREA_LIV_NOT_LIV_MKD");
            this.Database.RemoveColumn("GIS_HOUSE_REGISTER", "NUMBER_APARTMENTS");
        }
    }
}