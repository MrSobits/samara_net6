namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092387
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092387")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "CHARGED_SUM");
            Database.RemoveColumn("REGOP_PERS_ACC", "PAID_SUM");
            Database.RemoveColumn("REGOP_PERS_ACC", "PENALTY_SUM");

            Database.AddColumn("REGOP_PERS_ACC",
                new Column("TARIFF_CHARGE_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_PERS_ACC",
                new Column("DECISION_CHARGE_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0));

            UpdateAccountBalance();

            Database.AddEntityTable("REGOP_TRANSFER",
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("OP_GUID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, 1000),
                new Column("SOURCE_GUID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("TARGET_GUID", DbType.String, 40, ColumnProperty.NotNull),
                new RefColumn("ORIGINATOR_ID", ColumnProperty.Null, "REGOP_TRANSFER_TRANSFER", "REGOP_TRANSFER", "ID"));
            Database.AddIndex("IDX_REGOP_TRANSFER_OP", false, "REGOP_TRANSFER", "OP_GUID");
            //Database.AddIndex("IDX_REGOP_TRANSFER_SG", false, "REGOP_TRANSFER", "SOURCE_GUID");
            //Database.AddIndex("IDX_REGOP_TRANSFER_TG", false, "REGOP_TRANSFER", "TARGET_GUID");

            Database.AddEntityTable("REGOP_WALLET",
                new Column("BALANCE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("WALLET_GUID", DbType.String, 40, ColumnProperty.NotNull)
                );
            Database.AddIndex("IDX_REGOP_WALLET_WG", true, "REGOP_WALLET", "WALLET_GUID");

            Database.AddEntityTable("REGOP_WALLET_OPERATION",
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("OP_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("WALLET_GUID", DbType.String, 40, ColumnProperty.NotNull),
                new RefColumn("TRANSFER_ID", ColumnProperty.NotNull, "REGOP_WALLET_OP_TRANSFER", "REGOP_TRANSFER", "ID")
                );
            Database.AddIndex("IDX_REGOP_WALLET_OP_W", false, "REGOP_WALLET_OPERATION", "WALLET_GUID");

            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("BT_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_BT", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("DT_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_DT", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("P_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_P", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("R_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_R", "REGOP_WALLET", "ID"));

            Database.AddEntityTable("REGOP_MONEY_OPERATION",
                new Column("OP_GUID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("ORIGINATOR_GUID", DbType.String, 40, ColumnProperty.NotNull)
                );
            Database.AddIndex("IDX_REGOP_MONEY_OP_G", true, "REGOP_MONEY_OPERATION", "OP_GUID");
            Database.AddIndex("IDX_REGOP_MONEY_OP_O_G", false, "REGOP_MONEY_OPERATION", "ORIGINATOR_GUID");

            #region RO pay account

            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "PACC_BALANCE");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "SUBCIDY_BALANCE");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "BANK_ACC_ID");

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("BT_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_BT", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("DT_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_DT", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("P_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_P", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("R_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_R", "REGOP_WALLET", "ID"));

            #endregion
        }

        public override void Down()
        {
            #region RO pay account

            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "BT_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "DT_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "P_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "R_WALLET_ID");

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT",new RefColumn("BANK_ACC_ID", ColumnProperty.Null, "RO_PAY_ACC", "REGOP_BANK_ACCOUNT", "ID"));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("SUBCIDY_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("PACC_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0));

            #endregion

            Database.RemoveIndex("IDX_REGOP_MONEY_OP_O_G", "REGOP_MONEY_OPERATION");
            Database.RemoveIndex("IDX_REGOP_MONEY_OP_G", "REGOP_MONEY_OPERATION");
            Database.RemoveTable("REGOP_MONEY_OPERATION");

            Database.RemoveColumn("REGOP_PERS_ACC", "BT_WALLET_ID");
            Database.RemoveColumn("REGOP_PERS_ACC", "DT_WALLET_ID");
            Database.RemoveColumn("REGOP_PERS_ACC", "P_WALLET_ID");
            Database.RemoveColumn("REGOP_PERS_ACC", "R_WALLET_ID");

            Database.RemoveIndex("IDX_REGOP_WALLET_OP_W", "REGOP_WALLET_OPERATION");
            Database.RemoveTable("REGOP_WALLET_OPERATION");

            Database.RemoveIndex("IDX_REGOP_WALLET_WG", "REGOP_WALLET");
            Database.RemoveTable("REGOP_WALLET");

            //Database.RemoveIndex("IDX_REGOP_TRANSFER_TG", "REGOP_TRANSFER");
            //Database.RemoveIndex("IDX_REGOP_TRANSFER_SG", "REGOP_TRANSFER");
            Database.RemoveIndex("IDX_REGOP_TRANSFER_OP", "REGOP_TRANSFER");
            Database.RemoveTable("REGOP_TRANSFER");

            Database.RemoveColumn("REGOP_PERS_ACC", "TARIFF_CHARGE_BALANCE");
            Database.RemoveColumn("REGOP_PERS_ACC", "DECISION_CHARGE_BALANCE");

            Database.AddColumn("REGOP_PERS_ACC", new Column("PENALTY_SUM", DbType.AnsiString));
            Database.AddColumn("REGOP_PERS_ACC", new Column("PAID_SUM", DbType.AnsiString));
            Database.AddColumn("REGOP_PERS_ACC", new Column("CHARGED_SUM", DbType.AnsiString));
        }

        private void UpdateAccountBalance()
        {
            string chargeBalanceSql = "update REGOP_PERS_ACC acc set {0} = " +
                                      " (select coalesce(sum({1}), 0) from REGOP_PERS_ACC_CHARGE ch where acc.ID = ch.PERS_ACC_ID and ch.IS_FIXED = {2})";
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(chargeBalanceSql.FormatUsing("TARIFF_CHARGE_BALANCE", "CHARGE_TARIFF", "true"));
                Database.ExecuteNonQuery(chargeBalanceSql.FormatUsing("DECISION_CHARGE_BALANCE", "OVERPLUS", "true"));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(chargeBalanceSql.FormatUsing("TARIFF_CHARGE_BALANCE", "CHARGE_TARIFF", "1"));
                Database.ExecuteNonQuery(chargeBalanceSql.FormatUsing("DECISION_CHARGE_BALANCE", "OVERPLUS", "1"));
            }
        }
    }
}
