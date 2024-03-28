namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2014120900")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_HOUSE_REGISTER", new Column("MUNICIPALIY_ID", DbType.Int64, null));
            this.Database.AddForeignKey("FK_GIS_HOUSE_REGISTER_MUNICIPALIY_ID_GKH_DICT_MUNICIPALITY_ID", "GIS_HOUSE_REGISTER",
                "MUNICIPALIY_ID", "GKH_DICT_MUNICIPALITY", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GIS_HOUSE_REGISTER", "FK_GIS_HOUSE_REGISTER_MUNICIPALIY_ID_GKH_DICT_MUNICIPALITY_ID");
            this.Database.RemoveColumn("GIS_HOUSE_REGISTER", "MUNICIPALIY_ID");
        }
    }
}