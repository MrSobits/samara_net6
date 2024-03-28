namespace Bars.Gkh.Migrations._2017.Version_2017112301
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Utils;

    [Migration("2017112301")]
    [MigrationDependsOn(typeof(Version_2017071800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017082401.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017100600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable("CLW_RESTRUCT_DEBT",
                "CLW_DOCUMENT",
                "CLW_RESTRUCT_DEBT_CLW_DOCUMENT",
                new Column("BASE_TARIFF_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DECISION_TARIFF_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("RESTRUCT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PERCENT_SUM", DbType.Decimal, ColumnProperty.Null),
                new Column("DOCUMENT_STATE", DbType.Int32, ColumnProperty.NotNull, (int) RestructDebtDocumentState.Active),
                new Column("TERMINATION_DATE", DbType.DateTime),
                new Column("TERMINATION_NUMBER", DbType.String),
                new Column("TERMINATION_REASON", DbType.String),
                new FileColumn("DOC_FILE_ID", "CLW_RESTRUCT_DEBT_DOC_FILE"),
                new FileColumn("SCHEDULE_FILE_ID", "CLW_RESTRUCT_DEBT_SCHEDULE_FILE"),
                new FileColumn("TERMINATION_FILE_ID", "CLW_RESTRUCT_DEBT_TERMINATION_FILE")
            );
        }

        public override void Down()
        {
            this.Database.RemoveTable("CLW_RESTRUCT_DEBT");
        }
    }
}