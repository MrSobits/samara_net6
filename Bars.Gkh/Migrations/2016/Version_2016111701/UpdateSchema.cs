namespace Bars.Gkh.Migrations._2016.Version_2016111701
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016171101
    /// </summary>
    [Migration("2016171101")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016110300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("LATEST_TECH_MONITORING", DbType.DateTime));
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_NOT_INVOLVED_CR_REASON", DbType.Int16, 0));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "LATEST_TECH_MONITORING");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_NOT_INVOLVED_CR_REASON");
        }
    }
}