namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016041200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016.04.12.00
    /// </summary>
    [Migration("2016041200")]
    [MigrationDependsOn(typeof(Version_2016040400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_PERS_ACC_RECALC_EVT", 
                new RefColumn("PERIOD_ID", "RECALC_EVT_PERIOD", "REGOP_PERIOD", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_RECALC_EVT", "PERIOD_ID");
        }
    }
}
