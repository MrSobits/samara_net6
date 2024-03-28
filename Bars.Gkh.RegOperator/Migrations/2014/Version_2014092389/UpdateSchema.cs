namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092389
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092389")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092388.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("SS_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_SS", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("SS_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_SS", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("PWP_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_PWP", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("PWP_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_PWP", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "SS_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "SS_WALLET_ID");
            Database.RemoveColumn("REGOP_PERS_ACC", "PWP_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "PWP_WALLET_ID");
        }
    }
}
