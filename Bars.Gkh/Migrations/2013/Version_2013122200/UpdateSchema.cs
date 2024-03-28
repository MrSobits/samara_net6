namespace Bars.Gkh.Migrations.Version_2013122200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "PAYMENT_CODE");
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("PAYMENT_CODE", DbType.String));
        }
        
        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "PAYMENT_CODE");
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("PAYMENT_CODE", DbType.Int64, 22));
        }
    }
}