namespace Bars.Gkh.Migrations._2016.Version_2016011500
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 12.01.2016
    /// </summary>
    [Migration("2016011500")]
    [MigrationDependsOn(typeof(Version_2016011200.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_PERSON_CERTIFICATE", new Column("APPLICATION_DATE", DbType.DateTime));
            this.Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("FAIL_NOTIFICATION_OF_EXAM_RESULTS_ID", "FAIL_NOTIFICATION_OF_EXAM_RESULTS", "B4_FILE_INFO", "ID"));
            this.Database.AddRefColumn("GKH_PERSON_CERTIFICATE", new RefColumn("FILE_ISSUE_APPLICATION_ID", "FILE_ISSUE_APPLICATION", "B4_FILE_INFO", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "FAIL_NOTIFICATION_OF_EXAM_RESULTS_ID");
            this.Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "FILE_ISSUE_APPLICATION_ID");
            this.Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "APPLICATION_DATE");
        }
    }
}
