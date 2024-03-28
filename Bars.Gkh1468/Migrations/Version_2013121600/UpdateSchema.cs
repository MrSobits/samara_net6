namespace Bars.Gkh1468.Migrations.Version_2013121600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013121200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn(
                "GKH_OKI_PROV_PASSPORT",
                new Column("USER_NAME", DbType.String, ColumnProperty.Null));
            Database.AddColumn(
                "GKH_HOUSE_PROV_PASSPORT",
                new Column("USER_NAME", DbType.String, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OKI_PROV_PASSPORT", "USER_NAME");
            Database.RemoveColumn("GKH_HOUSE_PROV_PASSPORT", "USER_NAME");
        }
    }
}