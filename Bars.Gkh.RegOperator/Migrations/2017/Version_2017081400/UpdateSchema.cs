namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017081400
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017081400")]
    [MigrationDependsOn(typeof(Version_2017080400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            ViewManager.Drop(this.Database, "Regop");
            this.Database.ExecuteQuery("alter table regop_pers_acc alter column area_share type numeric(19,4);");
            //ViewManager.Create(this.Database, "Regop");
        }
    }
}