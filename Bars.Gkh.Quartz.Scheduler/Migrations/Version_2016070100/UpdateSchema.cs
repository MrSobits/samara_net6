namespace Bars.Gkh.Quartz.Scheduler.Migrations.Version_2016070100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016070100")]
    [MigrationDependsOn(typeof(Version_2016050100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("SCHDLR_JOURNAL", new Column("MESSAGE", DbType.String, 10000));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
        }
    }
}
