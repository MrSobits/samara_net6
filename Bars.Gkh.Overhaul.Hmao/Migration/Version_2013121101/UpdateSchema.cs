namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121101
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли на RegOperator


            //Database.AddEntityTable(
            //    "OVRHL_REG_OP_CALC_ACC",
            //    new Column("ACC_NUMBER", DbType.Int32),
            //    new RefColumn("CREDIT_ORG_ID", ColumnProperty.NotNull, "OV_REG_OP_CA_CR_ORG", "OVRHL_CREDIT_ORG", "ID"),
            //     new RefColumn("REG_OP_ID", ColumnProperty.NotNull, "OV_REG_OP_CA_REG_OP", "OVRHL_CREDIT_ORG", "ID"),
            //     new Column("TOTAL_OUT", DbType.Decimal),
            //     new Column("TOTAL_INCOME", DbType.Decimal),
            //     new Column("BALANCE_OUT", DbType.Decimal),
            //     new Column("BALANCE_INCOME", DbType.Decimal),
            //     new Column("LAST_OP_DATE", DbType.DateTime),
            //     new Column("OPEN_DATE", DbType.DateTime),
            //     new Column("CLOSE_DATE", DbType.DateTime));

            //Database.AddEntityTable(
            //        "OVRHL_REG_OP_CALC_ACC_OPER",
            //        new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "OV_RO_CALC_ACC_OP_RO", "OVRHL_REG_OP_CALC_ACC", "ID"),
            //        new RefColumn("OPERATION_ID", ColumnProperty.NotNull, "OV_RO_CALC_ACC_OP_OP", "OVRHL_DICT_ACCTOPERATION", "ID"),
            //        new Column("OPERATION_DATE", DbType.Date),
            //        new Column("SUM", DbType.Decimal),
            //        new Column("RECEIVER", DbType.String, 250),
            //        new Column("PAYER", DbType.String, 250),
            //        new Column("PURPOSE", DbType.String, 500));

        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_REG_OP_CALC_ACC_OPER");
            // Database.RemoveEntityTable("OVRHL_REG_OP_CALC_ACC");
        }
    }
}