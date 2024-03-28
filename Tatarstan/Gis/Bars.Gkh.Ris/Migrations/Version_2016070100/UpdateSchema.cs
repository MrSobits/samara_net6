namespace Bars.Gkh.Ris.Migrations.Version_2016070100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016070100")]
    [MigrationDependsOn(typeof(Version_2016063001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ChangeColumn("RIS_PACKAGE_TRIGGER", new Column("MESSAGE", DbType.String, 10000));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}
