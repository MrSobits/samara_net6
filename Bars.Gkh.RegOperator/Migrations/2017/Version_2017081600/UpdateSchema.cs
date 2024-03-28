namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017081600
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017081600")]
    [MigrationDependsOn(typeof(Version_2017080400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.RemoveColumn("GKH_ADDRESS_MATCH", "TYPE_ADDRESS");
        }
    }
}