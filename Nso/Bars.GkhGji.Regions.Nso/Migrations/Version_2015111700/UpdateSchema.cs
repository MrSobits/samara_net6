namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015111700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015102300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
	    public override void Up()
	    {
	        this.Database.AddColumn("GJI_ACTCHECK_LTEXT", new Column("VIOL_DESC", DbType.Binary));

            ViewManager.DropAll(this.Database);

            ViewManager.CreateAll(this.Database);
        }

	    public override void Down()
        {
	        this.Database.RemoveColumn("GJI_ACTCHECK_LTEXT", "VIOL_DESC");
        }
    }
}