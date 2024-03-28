namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014012100
{
    using System;
    using System.Data;
    using B4.Utils;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014011701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
                    "OVRHL_PAY_ACCOUNT",
                    new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("OVERDRAFT_LIMIT", DbType.Decimal));

            Database.AddForeignKey("FK_OVRHL_PAYACC_ACC", "OVRHL_PAY_ACCOUNT", "ID", "OVRHL_ACCOUNT", "ID");
            //Database.AddIndex("IND_OVRHL_PAYACC_ACC", true, "OVRHL_PAY_ACCOUNT", "ID");

            Database.AddRefColumn("OVRHL_PAY_ACCOUNT", new RefColumn("OWNER_ID", "OVRHL_PAYACC_OWN", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable(
                   "OVRHL_ACC_BANK_STATEMENT",
                   new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "OV_ACC_BANK_ST_ACC", "OVRHL_ACCOUNT", "ID"),
                   new RefColumn("STATE_ID", "OV_ACC_BANK_ST_STE", "B4_STATE", "ID"),
                   new Column("DOC_NUMBER", DbType.String, 50),
                   new Column("DOC_DATE", DbType.Date),
                   new Column("BALANCE_INCOME", DbType.Decimal),
                   new Column("BALANCE_OUT", DbType.Decimal),
                   new Column("LAST_OPERATION_DATE", DbType.Date));

            Database.AddEntityTable(
                    "OVRHL_ACCOUNT_OPERATION",
                    new RefColumn("ACC_BANK_STAT_ID", ColumnProperty.NotNull, "OV_ACC_OPER_BANK_ST", "OVRHL_ACC_BANK_STATEMENT", "ID"),
                    new RefColumn("OPERATION_ID", ColumnProperty.NotNull, "OV_OPERATION_RL_OPER", "OVRHL_DICT_ACCTOPERATION", "ID"),
                    new Column("OPERATION_DATE", DbType.Date, ColumnProperty.NotNull),
                    new Column("SUM", DbType.Decimal, ColumnProperty.NotNull),
                    new Column("RECEIVER", DbType.String, 128),
                    new Column("PAYER", DbType.String, 128),
                    new Column("PURPOSE", DbType.String, 128));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_ACCOUNT_OPERATION");
            Database.RemoveTable("OVRHL_ACC_BANK_STATEMENT");

            Database.RemoveColumn("OVRHL_PAY_ACCOUNT", "OWNER_ID");

            var idsForDelete = string.Empty;

            using (var dr = Database.ExecuteQuery("select id from OVRHL_PAY_ACCOUNT"))
            {
                while (dr.Read())
                {
                    if (dr[0] is DBNull)
                        break;

                    int id;
                    if (!int.TryParse(dr[0].ToString(), out id) || id == 0)
                        continue;

                    if (!idsForDelete.IsEmpty())
                    {
                        idsForDelete += ",";
                    }

                    idsForDelete += id;
                }
            }

            Database.RemoveConstraint("OVRHL_PAY_ACCOUNT", "FK_OVRHL_PAYACC_ACC");
            //Database.RemoveIndex("IND_OVRHL_PAYACC_ACC", "OVRHL_PAY_ACCOUNT");

            Database.RemoveTable("OVRHL_PAY_ACCOUNT");

            if (!idsForDelete.IsEmpty())
                Database.ExecuteNonQuery(string.Format("delete from OVRHL_ACCOUNT where id in ({0})", idsForDelete));
        }
    }
}