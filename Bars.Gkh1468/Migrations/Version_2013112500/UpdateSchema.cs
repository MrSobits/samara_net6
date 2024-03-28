namespace Bars.Gkh1468.Migrations.Version_2013112500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013111300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OKI_PROV_PASSPORT", new Column("SIGN_DATE", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GKH_HOUSE_PROV_PASSPORT", new Column("SIGN_DATE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OKI_PROV_PASSPORT", "SIGN_DATE");
            Database.RemoveColumn("GKH_HOUSE_PROV_PASSPORT", "SIGN_DATE");
        }
    }
}