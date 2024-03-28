namespace Bars.Gkh1468.Migrations.Version_2014120500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2014112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_HOUSE_PROV_PASS_ROW", "S_GROUP_KEY");
            Database.RemoveColumn("GKH_HOUSE_PROV_PASS_ROW", "INT_CODE");

            Database.AddColumn("GKH_HOUSE_PROV_PASS_ROW", new Column("PARENT_VALUE", DbType.Int64, ColumnProperty.Null));
            Database.AddColumn("GKH_OKI_PROV_PASSPORT_ROW", new Column("PARENT_VALUE", DbType.Int64, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_HOUSE_PROV_PASS_ROW", "PARENT_VALUE_ID");
            Database.RemoveColumn("GKH_OKI_PROV_PASSPORT_ROW", "PARENT_VALUE_ID");

            Database.AddColumn("GKH_HOUSE_PROV_PASS_ROW", new Column("S_GROUP_KEY", DbType.String, ColumnProperty.Null));
            Database.AddColumn("GKH_HOUSE_PROV_PASS_ROW", new Column("INT_CODE", DbType.String, ColumnProperty.Null));
        }
    }
}
