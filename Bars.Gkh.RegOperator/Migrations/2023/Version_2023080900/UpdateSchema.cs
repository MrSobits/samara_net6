namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023080900
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2023080900")]

    [MigrationDependsOn(typeof(_2023.Version_2023041900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "Regop", "CreateViewDebtorExport");
        }

        public override void Down()
        {
            ViewManager.Drop(this.Database, "Regop", "DeleteViewDebtorExport");
        }
    }
}
