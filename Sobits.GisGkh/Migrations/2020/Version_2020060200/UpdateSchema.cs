namespace Sobits.GisGkh.Migrations._2020.Version_2020060200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Map;

    [Migration("2020060200")]
    [MigrationDependsOn(typeof(Version_2020032400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GIS_GKH_REQUESTS", new RefColumn("logfile", "GIS_GKH_REQUESTS_LOG_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GIS_GKH_REQUESTS", "logfile");
        }
    }
}