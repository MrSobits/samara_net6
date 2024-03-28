namespace Bars.Gkh.Migrations.Version_2013092400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013091700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_EMERGENCY_OBJECT", new Column("DOCUMENT_NAME", DbType.String, 300));
            Database.ChangeColumn("GKH_CONTRAGENT_BANK", new Column("SETTL_ACCOUNT", DbType.String, 100));
        }

        public override void Down()
        {
        }
    }
}