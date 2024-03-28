namespace Bars.Gkh.Ris.Migrations.Version_2016080300
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016080300")]
    [MigrationDependsOn(typeof(Version_2016072600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("RIS_CHARTER", new Column("IS_MANAGED_BY_CONTRACT", DbType.Boolean));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_CHARTER", "IS_MANAGED_BY_CONTRACT");
        }
    }
}