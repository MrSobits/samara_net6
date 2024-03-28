namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092394
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092394")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092393.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT",
                new RefColumn("FSU_WALLET_ID", "REGOP_RO_PAYACC_FSUW", "REGOP_WALLET", "ID"));

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT",
                new RefColumn("RSU_WALLET_ID", "REGOP_RO_PAYACC_RSUW", "REGOP_WALLET", "ID"));

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT",
                new RefColumn("SSU_WALLET_ID", "REGOP_RO_PAYACC_SSUW", "REGOP_WALLET", "ID"));

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT",
                new RefColumn("TSU_WALLET_ID", "REGOP_RO_PAYACC_TSUW", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "FSU_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "RSU_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "SSU_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "TSU_WALLET_ID");
        }
    }
}
