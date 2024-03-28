namespace Bars.Gkh1468.Migrations.Version_2015100100
{
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
	using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2015092900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_HOUSE_PROV_PASSPORT", new RefColumn("CONTRAGENT_ID", ColumnProperty.Null, "HS_PROV_PASP_CTRGNT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
        }
    }
}
