namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2017112200
{
    using B4.Modules.Ecm7.Framework;
    using Gkh;

    [Migration("2017112200")]
    [MigrationDependsOn(typeof(Version_2017050400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhGjiChelyabinsk");
            ViewManager.Create(this.Database, "GkhGjiChelyabinsk");
        }
    }
}