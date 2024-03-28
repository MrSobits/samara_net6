namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2016020300
{
    using System.Data;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016020300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			ViewManager.Drop(Database, "GkhGjiTomsk", "DeleteViewAppealCits");
			ViewManager.Create(Database, "GkhGjiTomsk", "CreateViewAppealCits");
		}

        public override void Down()
        {
        }
    }
}
