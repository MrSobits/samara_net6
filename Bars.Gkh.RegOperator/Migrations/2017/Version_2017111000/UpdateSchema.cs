namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017111000
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017111000"), MigrationDependsOn(typeof(Version_2017103000.UpdateSchema))]
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