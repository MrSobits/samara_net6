namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //// Данной таблицы не было и не нужна
            //// Database.RemoveEntityTable("OVRHL_REG_OP_CALC_ACC_OPER");

            Database.RemoveTable("OVRHL_ACCOUNT_REAL_OPERATION");
            Database.RemoveTable("OVRHL_ACCOUNT_SPEC_OPERATION");
            //// Database.RemoveEntityTable("OVRHL_REG_OP_CALC_ACC");
            Database.RemoveTable("OVRHL_ACCOUNT_ACCR_OPERATION");
            Database.RemoveTable("OVRHL_ACCRUALS_ACCOUNT");
            Database.RemoveTable("OVRHL_SPECIAL_ACCOUNT");
            Database.RemoveTable("OVRHL_REAL_ACCOUNT");

            // перенесен в модуль overhaul
            //Database.RemoveEntityTable("OVRHL_ACCOUNT");

            //Database.AddEntityTable(
            //        "OVRHL_ACCOUNT",
            //        new Column("ACC_NUMBER", DbType.String, 50),
            //        new Column("OPEN_DATE", DbType.Date),
            //        new Column("CLOSE_DATE", DbType.Date),
            //        new Column("TOTAL_INCOME", DbType.Decimal),
            //        new Column("TOTAL_OUT", DbType.Decimal),
            //        new Column("BALANCE", DbType.Decimal),
            //        new Column("LAST_OPERATION_DATE", DbType.Date),
            //        new Column("ACCOUNT_TYPE", DbType.Int32, ColumnProperty.NotNull, 10));

            Database.AddTable(
                    "OVRHL_PAY_ACCOUNT",
                    new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("OVERDRAFT_LIMIT", DbType.Decimal));

            Database.AddRefColumn("OVRHL_PAY_ACCOUNT", new RefColumn("OWNER_ID", "OVRHL_REALACC_OWN", "GKH_CONTRAGENT", "ID"));
            Database.AddRefColumn("OVRHL_PAY_ACCOUNT", new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_PAY_ACC_LONG_TO", "OVRHL_LONGTERM_PR_OBJECT", "ID"));

            Database.AddTable(
                "OVRHL_SPECIAL_ACCOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("CREDIT_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull));

            Database.AddForeignKey("FK_OVRHL_SPEC_ACC_CRED_ORG", "OVRHL_SPECIAL_ACCOUNT", "CREDIT_ORG_ID", "OVRHL_CREDIT_ORG", "ID");
            Database.AddIndex("OVRHL_SPEC_ACC_CRED_ORG", false, "OVRHL_SPECIAL_ACCOUNT", "CREDIT_ORG_ID");

            Database.AddRefColumn("OVRHL_SPECIAL_ACCOUNT", new RefColumn("OWNER_ID", "OVRHL_SPECACC_OWN", "GKH_CONTRAGENT", "ID"));
            Database.AddRefColumn("OVRHL_SPECIAL_ACCOUNT", new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_SPEC_ACC_LONG_TO", "OVRHL_LONGTERM_PR_OBJECT", "ID"));

            Database.AddTable(
                "OVRHL_ACCRUALS_ACCOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OPENING_BALANCE", DbType.Decimal));

            Database.AddRefColumn("OVRHL_ACCRUALS_ACCOUNT", new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_ACC_ACC_LONG_TO", "OVRHL_LONGTERM_PR_OBJECT", "ID"));

            // перенесли на RegOperator
            //Database.AddTable(
            //    "OVRHL_REG_OP_CALC_ACC",
            //    new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
            //     new Column("BALANCE_OUT", DbType.Decimal),
            //     new Column("BALANCE_INCOME", DbType.Decimal));

            // Database.AddRefColumn("OVRHL_REG_OP_CALC_ACC", new RefColumn("CREDIT_ORG_ID", ColumnProperty.NotNull, "OV_REG_OP_CA_CR_ORG", "OVRHL_CREDIT_ORG", "ID"));
            // Database.AddRefColumn("OVRHL_REG_OP_CALC_ACC", new RefColumn("REG_OP_ID", ColumnProperty.NotNull, "OV_REG_OP_CA_REG_OP", "OVRHL_REG_OPERATOR", "ID"));

             Database.AddEntityTable(
                   "OVRHL_ACC_BANK_STATEMENT",
                   new Column("DOC_NUMBER", DbType.String, 50),
                   new Column("DOC_DATE", DbType.Date),
                   new Column("BALANCE_INCOME", DbType.Decimal),
                   new Column("BALANCE_OUT", DbType.Decimal),
                   new Column("LAST_OPERATION_DATE", DbType.Date),
                   new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "OV_ACC_BANK_ST_ACC", "OVRHL_ACCOUNT", "ID"));

             Database.AddEntityTable(
                     "OVRHL_ACCOUNT_OPERATION",
                     new RefColumn("ACC_BANK_STAT_ID", ColumnProperty.NotNull, "OV_ACC_OPER_BANK_ST", "OVRHL_ACC_BANK_STATEMENT", "ID"),
                     new RefColumn("OPERATION_ID", ColumnProperty.NotNull, "OV_OPERATION_RL_OPER", "OVRHL_DICT_ACCTOPERATION", "ID"),
                     new Column("OPERATION_DATE", DbType.Date, ColumnProperty.NotNull),
                     new Column("SUM", DbType.Decimal, ColumnProperty.NotNull),
                     new Column("RECEIVER", DbType.String, 128),
                     new Column("PAYER", DbType.String, 128),
                     new Column("PURPOSE", DbType.String, 128));

             Database.AddEntityTable(
                    "OVRHL_ACCOUNT_ACCR_OPERATION",
                     new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "OVRHL_ACCOUNT_ACCR", "OVRHL_ACCRUALS_ACCOUNT", "ID"),
                     new Column("ACCRUAL_DATE", DbType.Date),
                     new Column("TOTAL_DEBIT", DbType.Double),
                     new Column("TOTAL_CREDIT", DbType.Double),
                     new Column("OPENING_BALANCE", DbType.Double),
                     new Column("CLOSING_BALANCE", DbType.Double));               
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_ACCOUNT_OPERATION");
            Database.RemoveTable("OVRHL_ACC_BANK_STATEMENT");

            // перенесен в модуль overhaul
            // Database.AddRefColumn("OVRHL_ACCOUNT", new RefColumn("LONG_TERM_OBJ_ID", "OV_SPEC_AC_LONG_TO", "OVRHL_LONGTERM_PR_OBJECT", "ID"));
        }
    }
}