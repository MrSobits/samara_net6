namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016051800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.RegOperator.Enums;

    [Migration("2016051800")]
    [MigrationDependsOn(typeof(Version_2016051700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn(
                "REGOP_BANK_DOC_IMPORT",
                new Column("CHECK_STATE", DbType.Int16, ColumnProperty.NotNull, (int)BankDocumentImportCheckState.NotChecked));

            this.Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("ACCEPTED_SUM", DbType.Decimal, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("DISTRIBUTED_SUM", DbType.Decimal, ColumnProperty.Null));

            this.Database.ExecuteNonQuery("UPDATE REGOP_BANK_DOC_IMPORT SET CHECK_STATE = " + (int)BankDocumentImportCheckState.Checked);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "DISTRIBUTED_SUM");
            this.Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "ACCEPTED_SUM");
            this.Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "CHECK_STATE");
        }
    }
}
