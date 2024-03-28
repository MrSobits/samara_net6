namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017082300
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017082300")]
    [MigrationDependsOn(typeof(Version_2017062100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.RemoveTable("REGOP_DEBTOR_UPDATE_CONFIG");
        }
    }
}