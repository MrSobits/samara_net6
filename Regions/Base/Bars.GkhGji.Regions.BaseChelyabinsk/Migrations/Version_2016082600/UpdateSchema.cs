namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016082600
{
    using Bars.Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016082600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016082500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhGjiChelyabinsk");
            ViewManager.Create(this.Database, "GkhGjiChelyabinsk");
        }

        public override void Down()
        {
            ViewManager.Drop(this.Database, "GkhGjiChelyabinsk");
        }
    }
}