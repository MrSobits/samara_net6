namespace Bars.Gkh.Ris.Migrations.Version_2016062500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016062500")]
    [MigrationDependsOn(typeof(Version_2016062400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddEntityTable("RIS_SERVICE_SETTINGS",
            //    new Column("INTEGRATION_SERVICE", DbType.Int32, ColumnProperty.NotNull),
            //    new Column("NAME", DbType.String, 255, ColumnProperty.NotNull),
            //    new Column("ADDRESS", DbType.String, 255),
            //    new Column("ASYNC_ADDRESS", DbType.String, 255));

            //// уникальность, чтобы настройки случайно не продублировались
            //this.Database.AddIndex("INTEGRATION_SERVICE", true, "RIS_SERVICE_SETTINGS", "INTEGRATION_SERVICE");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_SERVICE_SETTINGS");
        }
    }
}
