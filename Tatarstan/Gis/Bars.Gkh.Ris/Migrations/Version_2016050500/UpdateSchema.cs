namespace Bars.Gkh.Ris.Migrations.Version_2016050500
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016050500")]
    [MigrationDependsOn(typeof(Version_2016050200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddEntityTable("RIS_TASK_TRIGGER",
            //    new RefColumn("TASK_ID", "RIS_TASK_TRIGGER_TASK", "RIS_TASK", "ID"),
            //    new RefColumn("TRIGGER_ID", "RIS_TASK_TRIGGER_TRIGGER", "SCHDLR_TRIGGER", "ID"),
            //    new Column("TRIGGER_TYPE", DbType.Int16));

            //this.Database.AddEntityTable("RIS_PACKAGE_TRIGGER",
            //    new RefColumn("PACKAGE_ID", "RIS_PACKAGE_TRIGGER_PACKAGE", "RIS_PACKAGE", "ID"),
            //    new RefColumn("TRIGGER_ID", "RIS_PACKAGE_TRIGGER_TRIGGER", "SCHDLR_TRIGGER", "ID"),
            //    new Column("MESSAGE", DbType.String, 500),
            //    new RefColumn("RESULTLOG_ID", "RIS_PACKAGE_TRIGGER_RESULTLOG", "B4_FILE_INFO", "ID"),
            //    new Column("ACK_MESSAGE_GUID", DbType.String, 100));

            //this.Database.RemoveColumn("RIS_TASK", "CLASS_NAME");
            //this.Database.RemoveColumn("RIS_TASK", "QRTZ_TRIGGER_KEY");
            //this.Database.RemoveColumn("RIS_TASK", "MAX_REPEAT_COUNT");
            //this.Database.RemoveColumn("RIS_TASK", "INTERVAL");

            //this.Database.AddColumn("RIS_PACKAGE", new Column("STATE", DbType.Int16));

            //this.Database.RemoveTable("RIS_TASK_PACKAGE");
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.AddEntityTable("RIS_TASK_PACKAGE",
            //    new RefColumn("TASK_ID", "RIS_TASK_PACKAGE_TASK", "RIS_TASK", "ID"),
            //    new RefColumn("PACKAGE_ID", "RIS_TASK_PACKAGE_PACKAGE", "RIS_PACKAGE", "ID"),
            //    new Column("ACK_MESSAGE_GUID", DbType.String, 100),
            //    new RefColumn("RESULTLOG_ID", "RIS_TASK_PACKAGE_RESULTLOG", "B4_FILE_INFO", "ID"),
            //    new Column("STATE", DbType.Int16),
            //    new Column("MESSAGE", DbType.String, 500));

            //this.Database.RemoveColumn("RIS_PACKAGE", "STATE");

            //this.Database.AddColumn("RIS_TASK", new Column("CLASS_NAME", DbType.String, 200));
            //this.Database.AddColumn("RIS_TASK", new Column("QRTZ_TRIGGER_KEY", DbType.String, 200));
            //this.Database.AddColumn("RIS_TASK", new Column("MAX_REPEAT_COUNT", DbType.Int32));
            //this.Database.AddColumn("RIS_TASK", new Column("INTERVAL", DbType.Int32));

            //this.Database.RemoveTable("RIS_PACKAGE_TRIGGER");
            //this.Database.RemoveTable("RIS_TASK_TRIGGER");
        }
    }
}
