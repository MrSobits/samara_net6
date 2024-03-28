namespace Bars.Gkh.Migrations._2015.Version_2015080500
{
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015080500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015072900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddRefColumn("GKH_CONTRAGENT", new RefColumn("MOSETTLEMENT_ID", "GKH_CONTR_MO", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
			Database.RemoveColumn("GKH_CONTRAGENT", "MOSETTLEMENT_ID");
        }
    }
}