namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023123104
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123104")]

    [MigrationDependsOn(typeof(Version_2023123103.UpdateSchema))]
    // Является Version_2020090700 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddForeignKey("LAWSUIT_REFERENCE_CALCULATION_PERIOD", "REGOP_LAWSUIT_REFERENCE_CALCULATION", "PERIOD_ID", "REGOP_PERIOD", "ID");
            this.Database.AddForeignKey("LAWSUIT_REFERENCE_CALCULATION_ACCOUNT", "REGOP_LAWSUIT_REFERENCE_CALCULATION", "ACCOUNT_ID", "REGOP_PERS_ACC", "ID");
        }
        public override void Down()
        {

        }
    }
}
