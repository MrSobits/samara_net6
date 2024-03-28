namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022070500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Enums;

    [MigrationDependsOn(typeof(Version_2022062800.UpdateSchema))]
    [Migration("2022070500")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery($@"
                UPDATE GJI_DECISION
                SET using_means_remote_interaction = {(int)YesNoNotSet.NotSet}
                WHERE using_means_remote_interaction = 0"
            );
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery($@"
                UPDATE GJI_DECISION
                SET using_means_remote_interaction = 0
                WHERE using_means_remote_interaction = {(int)YesNoNotSet.NotSet}"
            );
        }
    }
}