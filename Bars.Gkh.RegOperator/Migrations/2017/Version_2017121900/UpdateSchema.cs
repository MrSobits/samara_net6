namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017121900
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017121900")]
    [MigrationDependsOn(typeof(Version_2017121301.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "Regop");
            this.Database.ExecuteQuery("alter table regop_pers_acc alter column area_share type numeric(19,7);");
            //ViewManager.Create(this.Database, "Regop");
        }
    }
}