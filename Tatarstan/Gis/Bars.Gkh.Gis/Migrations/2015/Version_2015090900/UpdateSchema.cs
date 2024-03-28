namespace Bars.Gkh.Gis.Migrations._2015.Version_2015090900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015090900")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015083000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("GIS_INDICATOR_GROUPING");

            this.Database.AddEntityTable("GIS_INDICATOR_SERVICE_COMP",
                new RefColumn("SERVICE", "INDICATOR_SERVICE", "GIS_SERVICE_DICTIONARY", "ID"),
                new Column("TYPE_INDICATORS", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_INDICATOR_SERVICE_COMP");

            this.Database.AddEntityTable("GIS_INDICATOR_GROUPING",
                new Column("TYPE_GROUP_INDICATORS", DbType.Int32, ColumnProperty.NotNull),
                new Column("TYPE_INDICATORS", DbType.Int32, ColumnProperty.NotNull));
        }
    }
}