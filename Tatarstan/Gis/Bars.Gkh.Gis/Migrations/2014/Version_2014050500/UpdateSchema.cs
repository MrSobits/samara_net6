namespace Bars.Gkh.Gis.Migrations._2014.Version_2014050500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014050500")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GIS_REAL_ESTATE_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "GIS_REAL_EST_TYPE_COMM_PARAM",
                new Column("MIN", DbType.String, 500),
                new Column("MAX", DbType.String, 500),
                new Column("PRECISION_VALUE", DbType.String, 500),
                new Column("COMMON_PARAM_CODE", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "GIS_COMM_PAR_REAL_EST_TYPE", "GIS_REAL_ESTATE_TYPE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_REAL_EST_TYPE_COMM_PARAM");
            this.Database.RemoveTable("GIS_REAL_ESTATE_TYPE");
        }
    }
}