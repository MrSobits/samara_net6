namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018011160
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018011160")]
    [MigrationDependsOn(typeof(_2017.Version_2017122100.UpdateSchema))]
    [MigrationDependsOn(typeof(_2017.Version_2017121800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2018011100.UpdateSchema))]
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