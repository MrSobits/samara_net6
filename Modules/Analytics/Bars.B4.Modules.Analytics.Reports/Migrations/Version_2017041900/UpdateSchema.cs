namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2017041900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017041900")]
    [MigrationDependsOn(typeof(Version_2017020200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("AL_STORED_REPORT_HISTORY",
               new RefColumn("REPORT_ID", "AL_STORED_REPORT_HISTORY_REPORT", "AL_STORED_REPORT", "ID"),
               new RefColumn("FILE_ID", "AL_STORED_REPORT_HISTORY_FILE", "B4_FILE_INFO", "ID"),
               new RefColumn("USER_ID", "AL_STORED_REPORT_HISTORY_USER", "B4_USER", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("AL_STORED_REPORT_HISTORY");
        }
    }
}