namespace Bars.Gkh.Gis.Migrations._2015.Version_2015070300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.Enum;

    [Migration("2015070300")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015070200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_RESULT", DbType.Int32, ColumnProperty.NotNull, (int)ImportResult.Default);
            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_NAME", DbType.String, 200);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_NAME");
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "IMPORT_RESULT");
        }
    }
}