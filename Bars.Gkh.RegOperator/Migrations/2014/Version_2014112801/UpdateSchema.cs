namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014112801
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014112800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PROGRAM_CR_TYPE",
                     new Column("NAME", DbType.String, 250),
                     new Column("CODE", DbType.String, 250));

            Database.AddRefColumn("RF_TRANSFER_CTR", new RefColumn("TYPE_WORK_ID", "RF_TR_CTR_TW", "CR_OBJ_TYPE_WORK", "ID"));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("PAY_PURPOSE_DESCR", DbType.String, 1000));
            Database.AddRefColumn("RF_TRANSFER_CTR", new RefColumn("REG_OPER_ID", "RF_TR_CTR_REGOP", "OVRHL_REG_OPERATOR", "ID"));
            Database.AddRefColumn("RF_TRANSFER_CTR", new RefColumn("CALC_ACCOUNT_ID", "RF_TR_CTR_CALC_ACC", "REGOP_CALC_ACC_REGOP", "ID"));
            Database.AddRefColumn("RF_TRANSFER_CTR", new RefColumn("PROG_CR_TYPE_ID", "RF_TR_CTR_PR_TYPE", "REGOP_PROGRAM_CR_TYPE", "ID"));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("COMMENT", DbType.String, 1000));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("KIND_PAYMENT", DbType.Int16, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_TRANSFER_CTR", "KIND_PAYMENT");
            Database.RemoveColumn("RF_TRANSFER_CTR", "COMMENT");
            Database.RemoveColumn("RF_TRANSFER_CTR", "PROG_CR_TYPE_ID");
            Database.RemoveColumn("RF_TRANSFER_CTR", "CALC_ACCOUNT_ID");
            Database.RemoveColumn("RF_TRANSFER_CTR", "REG_OPER_ID");
            Database.RemoveColumn("RF_TRANSFER_CTR", "PAY_PURPOSE_DESCR");
            Database.RemoveColumn("RF_TRANSFER_CTR", "TYPE_WORK_ID");

            Database.RemoveTable("REGOP_PROGRAM_CR_TYPE");
        }
    }
}
