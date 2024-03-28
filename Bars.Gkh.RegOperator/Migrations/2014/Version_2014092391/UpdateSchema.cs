namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092391
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092391")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092390.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("AF_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_AF", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("AF_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_AF", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "AF_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "AF_WALLET_ID");
        }
    }
}
