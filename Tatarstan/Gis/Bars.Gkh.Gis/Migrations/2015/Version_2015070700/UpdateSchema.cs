namespace Bars.Gkh.Gis.Migrations._2015.Version_2015070700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Gis.Enum;

    [Migration("2015070700")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_NAME");
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_RESULT");

            this.Database.AddEntityTable(
                "GIS_OPEN_TATARSTAN",
                new RefColumn("B4_FILE_INFO_ID", "OPEN_TAT_FILEINFO", "B4_FILE_INFO", "ID"),
                new RefColumn("B4_LOG_INFO_ID", "OPEN_TAT_LOG", "B4_FILE_INFO", "ID"),
                new RefColumn("B4_USER_ID", "OPEN_TAT_USER", "B4_USER", "ID"),
                new Column("IMPORT_RESULT", DbType.Int32, ColumnProperty.NotNull, (int)ImportResult.Default),
                new Column("IMPORT_NAME", DbType.String, 200),
                new Column("RESPONSE_INFO", DbType.String, 1000),
                new Column("RESPONSE_CODE", DbType.String, 50));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_OPEN_TATARSTAN");

            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_RESULT", DbType.Int32, ColumnProperty.NotNull, (int)ImportResult.Default);
            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_NAME", DbType.String, 200);
        }
    }
}