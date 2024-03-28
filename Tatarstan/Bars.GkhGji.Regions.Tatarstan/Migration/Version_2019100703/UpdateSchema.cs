namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019100703
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2019100703")]
    [MigrationDependsOn(typeof(Version_2019100702.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddForeignKey("GJI_TAT_DISPOSAL_PROSECUTOR", "GJI_TAT_DISPOSAL", "PROSECUTOR_ID", "GJI_DICT_PROSECUTOR_OFFICE", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GJI_TAT_DISPOSAL", "GJI_TAT_DISPOSAL_PROSECUTOR");
        }
    }
}