namespace Bars.Gkh.Gis.Migrations._2014.Version_2014050600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014050600")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GIS_REAL_EST_TYPE_INDICATOR",
                new Column("REAL_EST_TYPE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REAL_EST_INDICATOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("MIN", DbType.String, 500),
                new Column("MAX", DbType.String, 500),
                new Column("PRECISION_VALUE", DbType.String, 500));

            this.Database.AddIndex("IND_GIS_REAL_EST_TI_TYPE", false, "GIS_REAL_EST_TYPE_INDICATOR", "REAL_EST_TYPE_ID");
            this.Database.AddIndex("IND_GIS_REAL_EST_TI_INDI", false, "GIS_REAL_EST_TYPE_INDICATOR", "REAL_EST_INDICATOR_ID");
            this.Database.AddForeignKey("FK_GIS_REAL_EST_TI_TYPE", "GIS_REAL_EST_TYPE_INDICATOR", "REAL_EST_TYPE_ID", "GIS_REAL_ESTATE_TYPE", "ID");
            this.Database.AddForeignKey("FK_GIS_REAL_EST_TI_INDI", "GIS_REAL_EST_TYPE_INDICATOR", "REAL_EST_INDICATOR_ID", "GIS_REAL_ESTATE_INDICATOR", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_REAL_EST_TYPE_INDICATOR");
        }
    }
}