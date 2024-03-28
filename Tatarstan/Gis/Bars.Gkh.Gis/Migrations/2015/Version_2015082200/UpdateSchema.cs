namespace Bars.Gkh.Gis.Migrations._2015.Version_2015082200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015082200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015081901.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveColumn("GIS_INTEGR_LOG", "PORTION_OBJECTS");
            this.Database.RemoveColumn("GIS_INTEGR_LOG", "RESULT_TEXT");
            this.Database.RemoveColumn("GIS_INTEGR_LOG", "TYPE_OPERATION");
            this.Database.RemoveColumn("GIS_INTEGR_LOG", "TYPE_COMPLETE");
            this.Database.RemoveColumn("GIS_INTEGR_LOG", "PROCESSED_PORTIONS");

            this.Database.AddRefColumn("GIS_INTEGR_LOG", new RefColumn("FILELOG_ID", "GIS_FILE_LOG", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_INTEGR_LOG", "FILELOG_ID");

            this.Database.AddColumn("GIS_INTEGR_LOG", new Column("PROCESSED_PORTIONS", DbType.Int32));
            this.Database.AddColumn("GIS_INTEGR_LOG", new Column("TYPE_COMPLETE", DbType.Int16, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("GIS_INTEGR_LOG", new Column("TYPE_OPERATION", DbType.Int16, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("GIS_INTEGR_LOG", new Column("RESULT_TEXT", DbType.String, 2000));
            this.Database.AddColumn("GIS_INTEGR_LOG", new Column("PORTION_OBJECTS", DbType.Int32, ColumnProperty.NotNull, 0));
        }
    }
}