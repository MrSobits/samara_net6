namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092396
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092396")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092395.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("OS_WALLET_ID", "REGOP_RO_PAYACC_OSW", "REGOP_WALLET", "ID"));

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("BP_WALLET_ID", "REGOP_RO_PAYACC_BPW", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "OS_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "BP_WALLET_ID");
        }
    }
}
