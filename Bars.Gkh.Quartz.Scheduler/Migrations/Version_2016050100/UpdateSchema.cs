namespace Bars.Gkh.Quartz.Scheduler.Migrations.Version_2016050100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016050100")]
    [MigrationDependsOn(typeof(Version_2016010500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("SCHDLR_TRIGGER",
                new Column("CLASS_NAME", DbType.String, 200),
                new Column("QRTZ_TRIGGER_KEY", DbType.String, 200),
                new Column("START_PARAMS", DbType.Binary, ColumnProperty.Null),
                new Column("REPEAT_COUNT", DbType.Int32),
                new Column("INTERVAL", DbType.Int32),
                new Column("START_TIME", DbType.DateTime),
                new Column("END_TIME", DbType.DateTime),
                new Column("USER_NAME", DbType.String, 200));

            this.Database.AddEntityTable("SCHDLR_JOURNAL",
                new RefColumn("TRIGGER_ID", "SCHDLR_TRIGGER_JOURNAL", "SCHDLR_TRIGGER", "ID"),
                new Column("START_TIME", DbType.DateTime),
                new Column("END_TIME", DbType.DateTime),
                new Column("RESULT", DbType.Binary, ColumnProperty.Null),
                new Column("PROTOCOL", DbType.Binary, ColumnProperty.Null),
                new Column("MESSAGE", DbType.String, 500),
                new Column("INTERRUPTED", DbType.Boolean));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("SCHDLR_JOURNAL");
            this.Database.RemoveTable("SCHDLR_TRIGGER");
        }
    }
}
