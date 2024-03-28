namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015052900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015052700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SUBSIDY_INCOME",
                new Column("DATE_RECEIPT", DbType.DateTime),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("REMAIN_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("MONEY_DIRECTION", DbType.Int16, ColumnProperty.NotNull, 0m),
                new Column("DCODE", DbType.String, 250),
                new Column("SUBSIDY_DISTR_TYPE", DbType.String, 250),
                new Column("C_GUID", DbType.String, 40),
                new Column("D_DATE", DbType.DateTime),
                new Column("SUBSIDY_INCOME_STATUS", DbType.Int32, ColumnProperty.NotNull),
                new Column("DETAILS_CNT", DbType.Int32, ColumnProperty.NotNull),
                new Column("DEFINE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("BANK_ACC_STMNT_ID", "REG_SI_BANK_ACC_ST", "REGOP_BANK_ACC_STMNT", "ID"),
                new FileColumn("DOCUMENT_ID", "REGOP_SUB_INC_DOC"));

            Database.AddEntityTable("REGOP_SUBSIDY_INC_DET",
                new Column("DATE_RECEIPT", DbType.DateTime),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("REAL_OBJ_ADR", DbType.String, 500),
                new Column("IMP_REAL_OBJ_ID", DbType.Int64),
                new Column("SUBSIDY_DISTR_TYPE", DbType.String, 250),
                new Column("IS_CONFIRMED", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("SUBSIDY_INCOME_ID", "REGOP_SI_DET_SI", "REGOP_SUBSIDY_INCOME", "ID"),
                new RefColumn("REAL_OBJ_ID", "REGOP_SI_DET_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SUBSIDY_INC_DET");
            Database.RemoveTable("REGOP_SUBSIDY_INCOME");
        }
    }
}
