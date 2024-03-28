namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017112300
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017112300"), MigrationDependsOn(typeof(Version_2017111000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "Regop", "DeleteFunctionPenaltyFormula");
            //ViewManager.Create(this.Database, "Regop", "CreateFunctionPenaltyFormula");
        }

        public override void Down()
        {
        }
    }
}