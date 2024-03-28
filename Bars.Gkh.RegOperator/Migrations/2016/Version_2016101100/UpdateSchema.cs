namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016101100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2016101100
    /// </summary>
    [Migration("2016101100")]
    [MigrationDependsOn(typeof(Version_2016100600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_PERS_ACC_CHARGE", new RefColumn("PERIOD_ID", "FK_CHARGE_PERIOD_ID", "REGOP_PERIOD", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "PERIOD_ID");
        }
    }
}
