namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015080400
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015080400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2015072200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_REALITY_OBJECT_FANTOM",
                new RefColumn("MU_FANTOM_ID", "GJI_RO_FANTOM_MU", "GKH_DICT_MUNICIPALITY", "ID"));
            this.Database.AddRefColumn("GJI_REALITY_OBJECT_FANTOM",
                new RefColumn("SETTLEMENT_FANTOM_ID", "GJI_RO_FANTOM_SETTL", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_REALITY_OBJECT_FANTOM", "MU_FANTOM_ID");
            this.Database.RemoveColumn("GJI_REALITY_OBJECT_FANTOM", "SETTLEMENT_FANTOM_ID");
        }
    }
}