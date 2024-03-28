namespace Bars.GkhGji.Migrations.Version_2013032800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_BUISNES_NOTIF", new Column("REG_NUMBER", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_BUISNES_NOTIF", "REG_NUMBER");
        }
    }
}