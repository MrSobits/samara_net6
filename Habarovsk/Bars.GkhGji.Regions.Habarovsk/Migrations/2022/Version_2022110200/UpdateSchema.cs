namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2022110200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022110200")]
    [MigrationDependsOn(typeof(Version_2022092800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_VDGO_VIOLATORS", new RefColumn("GJI_NOTIFICATION_FILE_ID", "GJI_VDGO_VIOLATORS_NOTIFICATION_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VDGO_VIOLATORS", "GJI_NOTIFICATION_FILE_ID");
        }
    }
}