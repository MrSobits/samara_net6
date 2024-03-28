namespace Bars.Gkh1468.Migrations.Version_2014112600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_HOUSE_PROV_PASS_ROW", new Column("S_GROUP_KEY", DbType.String, ColumnProperty.Null));
            Database.AddColumn("GKH_HOUSE_PROV_PASS_ROW", new Column("INT_CODE", DbType.String, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_HOUSE_PROV_PASS_ROW", "S_GROUP_KEY");
            Database.RemoveColumn("GKH_HOUSE_PROV_PASS_ROW", "INT_CODE");
        }
    }
}
