namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_LONG_PROG_OBJ_LOAN",
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "LONG_PROG_OBJ_LOAN_OBJ", "OVRHL_LONGTERM_PR_OBJECT", "ID"),
                new RefColumn("OBJECT_ISSUED_ID", ColumnProperty.NotNull, "LONG_PROG_OBJ_LOAN_OBJISS", "OVRHL_LONGTERM_PR_OBJECT", "ID"),
                new RefColumn("FILE_ID", "LONG_PROG_OBJ_LOAN_FILE", "B4_FILE_INFO", "ID"),
                new Column("DATE_ISSUE", DbType.Date),
                new Column("DATE_REPAYMENT", DbType.Date),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DOCUMENT_NUMBER", DbType.String, 300),
                new Column("LOAN_AMOUNT", DbType.Decimal),
                new Column("PERIOD_LOAN", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_LONG_PROG_OBJ_LOAN");
        }
    }
}