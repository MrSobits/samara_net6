namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023123101
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123101")]

    [MigrationDependsOn(typeof(Version_2023123100.UpdateSchema))]
    // Является Version_2018091900 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            //this.Database.RemoveExportId("REGOP_CALC_ACC");
        }
    }
}
