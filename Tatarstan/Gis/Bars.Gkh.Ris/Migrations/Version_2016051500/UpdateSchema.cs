namespace Bars.Gkh.Ris.Migrations.Version_2016051500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051500")]
    [MigrationDependsOn(typeof(Version_2016051300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("RIS_PACKAGE_TRIGGER", new Column("PROCESSING_RESULT", DbType.Binary, ColumnProperty.Null));
            //this.Database.RemoveColumn("RIS_PACKAGE_TRIGGER", "RESULTLOG_ID");

            //this.Database.AddColumn("RIS_TASK", new Column("CLASS_NAME", DbType.String, 200));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.AddColumn("RIS_PACKAGE_TRIGGER", new RefColumn("RESULTLOG_ID", "RIS_PACKAGE_TRIGGER_RESULTLOG", "B4_FILE_INFO", "ID"));
            //this.Database.RemoveColumn("RIS_PACKAGE_TRIGGER", "PROCESSING_RESULT");

            //this.Database.RemoveColumn("RIS_TASK", "CLASS_NAME");
        }
    }
}
