namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015080400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015080400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015072200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_REALITY_OBJECT_FANTOM",
                new RefColumn("MU_FANTOM_ID", "GJI_RO_FANTOM_MU", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddRefColumn("GJI_REALITY_OBJECT_FANTOM",
                new RefColumn("SETTLEMENT_FANTOM_ID", "GJI_RO_FANTOM_SETTL", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_REALITY_OBJECT_FANTOM", "MU_FANTOM_ID");
            Database.RemoveColumn("GJI_REALITY_OBJECT_FANTOM", "SETTLEMENT_FANTOM_ID");
        }
    }
}