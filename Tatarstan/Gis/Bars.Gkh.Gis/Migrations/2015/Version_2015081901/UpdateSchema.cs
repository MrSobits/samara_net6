namespace Bars.Gkh.Gis.Migrations._2015.Version_2015081901
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015081901")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015081900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GIS_INTEGR_METHOD", new Column("DESCRIPTION", DbType.String, 1000));
            this.Database.AddColumn("GIS_INTEGR_METHOD", new Column("METHODCODE", DbType.String, 1000, ColumnProperty.NotNull));
            this.Database.AddColumn("GIS_INTEGR_METHOD", new Column("SERVICE_ADDRESS", DbType.String, 1000, ColumnProperty.NotNull));
        }

        public override void Down()
        {

            this.Database.RemoveColumn("GIS_INTEGR_METHOD", "SERVICE_ADDRESS");
            this.Database.RemoveColumn("GIS_INTEGR_METHOD", "METHODCODE");
            this.Database.ChangeColumn("GIS_INTEGR_METHOD", new Column("DESCRIPTION", DbType.String, 1000, ColumnProperty.NotNull));
        }
    }
}