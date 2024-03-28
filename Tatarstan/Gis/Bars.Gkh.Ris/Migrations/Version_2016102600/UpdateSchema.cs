namespace Bars.Gkh.Ris.Migrations.Version_2016102600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102600")]
    [MigrationDependsOn(typeof(Version_2015102800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ChangeColumn("RIS_CHARTER", new Column("MANAGERS", DbType.String, 1000));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.ChangeColumn("RIS_CHARTER", new Column("MANAGERS", DbType.String));
        }
    }
}