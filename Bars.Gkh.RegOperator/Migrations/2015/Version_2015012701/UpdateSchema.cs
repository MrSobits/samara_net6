namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015012701
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015012700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_DEBTOR", new Column("START_DATE", DbType.DateTime));
            Database.AddColumn("REGOP_DEBTOR", new Column("MONTH_COUNT", DbType.Int16));

            Database.AddTable("CLW_DEBTOR_CLAIM_WORK",
                    new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                    new Column("DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                    new Column("COUNT_MONTH_DELAY", DbType.Int16, ColumnProperty.NotNull, 0));

            Database.AddIndex("IND_CLW_DEBT_CLW_PA", false, "CLW_DEBTOR_CLAIM_WORK", "ID");
            Database.AddRefColumn("CLW_DEBTOR_CLAIM_WORK", new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "CLW_DEBT_CLW_PA", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "ACCOUNT_ID");
            Database.RemoveTable("CLW_DEBTOR_CLAIM_WORK");

            Database.RemoveColumn("REGOP_DEBTOR", "START_DATE");
            Database.RemoveColumn("REGOP_DEBTOR", "MONTH_COUNT");
        }
    }
}
