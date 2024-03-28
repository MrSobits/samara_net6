namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015031200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015030200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_DOCUMENT", DbType.String, 500));
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_DOC_NUMBER", DbType.String, 100));
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_DOC_DATE", DbType.Date));
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_SUM_PERCENT", DbType.Decimal));

            Database.AddRefColumn("CLW_DEBTOR_CLAIM_WORK", new RefColumn("RD_DOC_FILE_ID","CLW_DCW_DOC_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("CLW_DEBTOR_CLAIM_WORK", new RefColumn("RD_PAY_SCHDL_FILE_ID", "CLW_DCW_PAY_SCH_FILE", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable("CLW_RESTRUCT_SCHEDL",
                new Column("PAY_DEADLINE", DbType.Date, ColumnProperty.NotNull),
                new Column("RESTRUCT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("CLAIM_WORK_ID", ColumnProperty.NotNull,  "CLW_RESTR_SCHED_CLW", "CLW_DEBTOR_CLAIM_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_RESTRUCT_SCHEDL");

            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_PAY_SCHDL_FILE_ID");
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_DOC_FILE_ID");

            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_SUM_PERCENT");
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_DOC_DATE");
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_DOC_NUMBER");
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_DOCUMENT");
        }
    }
}
