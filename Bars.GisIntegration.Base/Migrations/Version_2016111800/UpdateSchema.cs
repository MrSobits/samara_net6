namespace Bars.GisIntegration.Base.Migrations.Version_2016111800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016111800")]
    [MigrationDependsOn(typeof(Version_2016111000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("RIS_CHARTER", new Column("DOCNUM", DbType.String, 1000));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}