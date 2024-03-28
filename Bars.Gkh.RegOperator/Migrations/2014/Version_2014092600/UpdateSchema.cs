namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "R_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "SS_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "PWP_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "AF_WALLET_ID");

            Database.RemoveColumn("REGOP_TRANSFER", "OP_GUID");
            Database.AddRefColumn("REGOP_TRANSFER", new RefColumn("OP_ID", ColumnProperty.NotNull, "REGOP_TR_OP", "REGOP_MONEY_OPERATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_TRANSFER", "OP_ID");
            Database.AddColumn("REGOP_TRANSFER", new Column("OP_GUID", DbType.String, ColumnProperty.Null));

            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("AF_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_R", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("PWP_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_R", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("SS_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_R", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("R_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_R", "REGOP_WALLET", "ID"));
        }
    }
}
