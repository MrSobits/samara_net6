namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018050300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018050300")]
    [MigrationDependsOn(typeof(Version_2018042301.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {

            this.Database.AddJoinedSubclassTable("GJI_TASK_DISPOSAL", "GJI_DISPOSAL", "GJI_TASK_DISPOSAL_DISP");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_TASK_DISPOSAL");
        }
    }
}