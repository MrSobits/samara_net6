namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023092100
{
    using B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2023092100")]
    [MigrationDependsOn(typeof(Version_2023080100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGjiChelyabinsk", "CreateViewChERKNM");
        }

        public override void Down()
        {
            ViewManager.Drop(this.Database, "GkhGjiChelyabinsk", "DeleteViewChERKNM");
        }
    }
}