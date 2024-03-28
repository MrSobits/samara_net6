using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014040400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014040100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.TableExists("OVRHL_REG_OPERATOR"))
            {
                Database.AddEntityTable("OVRHL_REG_OPERATOR",
                    new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "OVRHL_REG_OPER_CNTR", "GKH_CONTRAGENT", "ID"));
            }

            if (!Database.TableExists("OVRHL_REG_OP_CALC_ACC"))
            {
                Database.AddEntityTable(
                    "OVRHL_REG_OP_CALC_ACC",
                    new RefColumn("REG_OP_ID", ColumnProperty.NotNull, "OV_REG_OP_CA_REG_OP", "OVRHL_REG_OPERATOR", "ID"),
                    new Column("BALANCE_OUT", DbType.Decimal),
                    new Column("BALANCE_INCOME", DbType.Decimal));
            }

            if (!Database.ColumnExists("OVRHL_REG_OP_CALC_ACC", "IS_SPECIAL"))
            {
                Database.AddColumn("OVRHL_REG_OP_CALC_ACC", new Column("IS_SPECIAL", DbType.Boolean, false));
            }

            if (Database.ColumnExists("OVRHL_REG_OP_CALC_ACC", "CREDIT_ORG_ID"))
            {
                Database.RemoveColumn("OVRHL_REG_OP_CALC_ACC", "CREDIT_ORG_ID");
            }

            if (!Database.ColumnExists("OVRHL_REG_OP_CALC_ACC", "CA_BANK_ID"))
            {
                Database.AddRefColumn("OVRHL_REG_OP_CALC_ACC", new RefColumn("CA_BANK_ID", ColumnProperty.Null, "OVRHL_OVRHL_CACC_CAB", "GKH_CONTRAGENT_BANK", "ID"));
            }
            
            if (!Database.TableExists("OVRHL_REG_OPERATOR_MU"))
            {
                Database.AddEntityTable("OVRHL_REG_OPERATOR_MU",
                    new RefColumn("REG_OPERATOR_ID", ColumnProperty.NotNull, "OVRHL_REG_OPERATOR_MU_OPER",
                        "OVRHL_REG_OPERATOR", "ID"),
                    new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OVRHL_REG_OPERATOR_MU_MU",
                        "GKH_DICT_MUNICIPALITY", "ID"));
            }

            if (!Database.TableExists("OVRHL_REG_OP_PERS_ACC_MU"))
            {
                Database.AddEntityTable("OVRHL_REG_OP_PERS_ACC_MU",
                    new RefColumn("REG_OP_ID", ColumnProperty.NotNull, "OV_REG_OP_PA_MU_OP", "OVRHL_REG_OPERATOR", "ID"),
                    new RefColumn("MU_ID", ColumnProperty.NotNull, "OV_REG_OP_PA_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                    new Column("OWNER_FIO", DbType.String, 150),
                    new Column("PAID_CONTRIB", DbType.Decimal),
                    new Column("CREDIT_CONTRIB", DbType.Decimal),
                    new Column("CREDIT_PENALTY", DbType.Decimal),
                    new Column("PAID_PENALTY", DbType.Decimal),
                    new Column("SUM_LOCAL_BUD", DbType.Decimal),
                    new Column("SUM_SUBJ_BUD", DbType.Decimal),
                    new Column("SUM_FED_BUD", DbType.Decimal),
                    new Column("SUM_ADOPT", DbType.Decimal),
                    new Column("REPAY_SUM_ADOPT", DbType.Decimal)
                    );
            }

            if (!Database.ColumnExists("OVRHL_REG_OP_PERS_ACC_MU", "ACC_NUMBER"))
            {
                Database.AddColumn("OVRHL_REG_OP_PERS_ACC_MU", new Column("ACC_NUMBER", DbType.String, 50));
            }

            if (!Database.TableExists("OVRHL_FUND_FORMAT_CONTR"))
            {
                Database.AddEntityTable("OVRHL_FUND_FORMAT_CONTR",
                    new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_FND_CTR_LONG_OBJ",
                        "OVRHL_LONGTERM_PR_OBJECT", "ID"),
                    new RefColumn("REG_OPER_ID", ColumnProperty.NotNull, "OV_FND_CTR_REG_OP", "OVRHL_REG_OPERATOR", "ID"),
                    new Column("TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                    new Column("CONTRACT_NUMBER", DbType.String, 100),
                    new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                    new Column("DATE_END", DbType.DateTime),
                    new Column("CONTRACT_DATE", DbType.DateTime),
                    new RefColumn("FILE_ID", "OV_FND_CTR_FILE", "B4_FILE_INFO", "ID"));
            }

            Database.AddEntityTable(
                "REGOP_CALCACC_RO",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_RO_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn(
                    "REGOP_CALC_ACC_ID",
                    ColumnProperty.NotNull,
                    "REGOP_CALC_ACC_RO_RO",
                    "OVRHL_REG_OP_CALC_ACC",
                    "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_FUND_FORMAT_CONTR");
            Database.RemoveTable("REGOP_CALCACC_RO");
            Database.RemoveTable("OVRHL_REG_OP_PERS_ACC_MU");
            Database.RemoveTable("OVRHL_REG_OPERATOR_MU");
            Database.RemoveTable("OVRHL_REG_OP_CALC_ACC");
            Database.RemoveTable("OVRHL_REG_OPERATOR");
        }
    }
}
