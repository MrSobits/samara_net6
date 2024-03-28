namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016061600
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016061600")]
    [MigrationDependsOn(typeof(Version_2016052700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
         //   ViewManager.Drop(this.Database, "Regop");
         //   ViewManager.Create(this.Database, "Regop");
        }

        public override void Down()
        {
           // ViewManager.Drop(this.Database, "Regop");
        }
    }
}