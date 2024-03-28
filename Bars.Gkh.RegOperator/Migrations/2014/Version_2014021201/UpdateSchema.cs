namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021201
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021200.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "REGOP_RO_LOAN",
                new RefColumn("PROVIDER_ID", ColumnProperty.NotNull, "LOAN_RO_PROV", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("RECEIVER_ID", ColumnProperty.NotNull, "LOAN_RO_RCVR", "GKH_REALITY_OBJECT", "ID"),
                new Column("LOAN_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PLAN_END_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FACT_END_DATE", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("PROGRAM_CR_ID", ColumnProperty.NotNull, "LOAN_RO_PCR", "CR_DICT_PROGRAM", "ID"),
                new Column("LOAN_SUM", DbType.Decimal.WithSize(18, 2), ColumnProperty.NotNull, 0m),
                new Column("LOAN_DEBT", DbType.Decimal.WithSize(18, 2), ColumnProperty.NotNull, 0m),
                new Column("LOAN_RETURNED_SUM", DbType.Decimal.WithSize(18, 2), ColumnProperty.NotNull, 0m),
                new RefColumn("DOC_ID", ColumnProperty.NotNull, "LOAN_RO_DOC", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_RO_LOAN");
        }
    }
}
