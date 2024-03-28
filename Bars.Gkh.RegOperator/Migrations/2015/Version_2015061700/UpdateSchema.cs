namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015061700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015061700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015061100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECEIPT_DATE", DbType.DateTime));

            Database.ExecuteNonQuery(@"
                                UPDATE REGOP_BANK_ACC_STMNT SET RECEIPT_DATE = DOC_DATE;
                                UPDATE REGOP_BANK_ACC_STMNT SET STATE = 40 WHERE STATE = 30;
                           ");
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECEIPT_DATE");
        }
    }
}
