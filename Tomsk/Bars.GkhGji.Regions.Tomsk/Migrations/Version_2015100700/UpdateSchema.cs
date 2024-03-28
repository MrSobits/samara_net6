namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015100700
{
	using global::Bars.B4.Modules.Ecm7.Framework;
	using Gkh;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015100600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            ViewManager.Drop(Database, "GkhGjiTomsk");
            ViewManager.Create(Database, "GkhGjiTomsk");
		}

        public override void Down()
        {
	        ViewManager.Drop(Database, "GkhGjiTomsk");
        }
    }
}