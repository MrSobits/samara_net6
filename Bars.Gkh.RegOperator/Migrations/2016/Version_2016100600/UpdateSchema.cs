namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016100600
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016100600
    /// </summary>
    [Migration("2016100600")]
    [MigrationDependsOn(typeof(Version_2016100400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "PENALTY_CHANGE");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
