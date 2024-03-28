namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017121301
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017121301")]
    [MigrationDependsOn(typeof(Version_2017121100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "Regop");
            //ViewManager.Create(this.Database, "Regop");
        }
    }
}
