namespace Bars.Gkh.Gis.Migrations._2015.Version_2015081200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015081200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015072200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("NAME", DbType.String, 500));
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("ACTION_CODE", DbType.String, 500));
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("REGISTRY_NUMBER", DbType.String, 50));
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "GIS_DICT_ID");
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "GIS_DICT_NAME");
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "GKH_DICT_NAME");
        }

        public override void Down()
        {
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("GIS_DICT_ID", DbType.String, 500, ColumnProperty.NotNull));
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("GIS_DICT_NAME", DbType.String, 500, ColumnProperty.NotNull));
            this.Database.AddColumn("GIS_INTEGR_DICT", new Column("GKH_DICT_NAME", DbType.String, 50, ColumnProperty.NotNull));
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "NAME");
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "ACTION_CODE");
            this.Database.RemoveColumn("GIS_INTEGR_DICT", "REGISTRY_NUMBER");
        }
    }
}