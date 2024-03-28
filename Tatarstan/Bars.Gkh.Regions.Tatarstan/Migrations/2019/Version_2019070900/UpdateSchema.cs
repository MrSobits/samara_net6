namespace Bars.Gkh.Regions.Tatarstan.Migrations._2019.Version_2019070900
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019070900")]
    [MigrationDependsOn(typeof(Version_2019060500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddForeignKey("FK_GKH_EGSO_INTEGRATION_VALUES_EGSO_INTEGRATION", "GKH_EGSO_INTEGRATION_VALUES", "EGSO_INTEGRATION_ID", "GKH_EGSO_INTEGRATION", "ID");
            this.Database.AddForeignKey("FK_GKH_EGSO_INTEGRATION_VALUES_MUNICIPALITY_DICT", "GKH_EGSO_INTEGRATION_VALUES", "MUNICIPALITY_DICT_ID", "GKH_EGSO_DICT_MUNICIPALITY", "ID");
        }
        public override void Down()
        {
            this.Database.RemoveConstraint("GKH_EGSO_INTEGRATION_VALUES", "FK_GKH_EGSO_INTEGRATION_VALUES_EGSO_INTEGR");
            this.Database.RemoveConstraint("GKH_EGSO_INTEGRATION_VALUES", "FK_GKH_EGSO_INTEGRATION_VALUES_MUNICIPALITY");
        }
    }
}
