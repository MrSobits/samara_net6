namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019112100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Migrations;

    [Migration("2019112100")]
    [MigrationDependsOn(typeof(Version_2019111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_CONTROL_TYPE", new GuidColumn("TOR_ID", ColumnProperty.Null));
            this.Database.ChangeColumn("GJI_DICT_MANDATORY_REQS", new GuidColumn("TOR_ID", ColumnProperty.Null));

            this.Database.RemoveColumn("GJI_TAT_DISPOSAL", "TOR_ID");
            this.Database.AddColumn("GJI_TAT_DISPOSAL", new GuidColumn("RESULT_TOR_ID"));
        }

        public override void Down()
        {

            this.Database.AddColumn("GJI_TAT_DISPOSAL", new GuidColumn("TOR_ID"));
            this.Database.RemoveColumn("GJI_TAT_DISPOSAL", "RESULT_TOR_ID");
        }
    }
}