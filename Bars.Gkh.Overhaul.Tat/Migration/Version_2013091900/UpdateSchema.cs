namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013091900
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013091800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameTable("OVHL_DPKR_CORRECT_ST4", "OVHL_DPKR_CORRECT_ST1");

            Database.RemoveColumn("OVHL_DPKR_CORRECT_ST1", "YEAR_ST3");

            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "FUND_BUDG_SUM", "FUND_BUDG_NEED");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "MUN_BUDGET_SUM", "MUN_BUDGET_NEED");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "OTHSOURCE_BUDGET_SUM", "OTHSOURCE_BUDGET_NEED");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "REGION_BUDGET_SUM", "REGION_BUDGET_NEED");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "OWNERS_MONEY_SUM", "OWNERS_MONEY_NEED");

            Database.AddEntityTable(
                                    "OVHL_DPKR_CORRECT_ST2",
                new Column("CALC_DEFICIT", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OWNERS_MONEY_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PLAN_YEAR", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("REQUIRED_LOAN", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("LOAN_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("YEAR_COLLECTION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("YEAR_LOAN_LIMIT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVHL_DPKR_CORRECT_ST2");

            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "OWNERS_MONEY_NEED", "OWNERS_MONEY_SUM");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "FUND_BUDG_NEED", "FUND_BUDG_SUM");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "MUN_BUDGET_NEED", "MUN_BUDGET_SUM");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "OTHSOURCE_BUDGET_NEED", "OTHSOURCE_BUDGET_SUM");
            Database.RenameColumn("OVHL_DPKR_CORRECT_ST1", "REGION_BUDGET_NEED", "REGION_BUDGET_SUM");

            Database.AddColumn("OVHL_DPKR_CORRECT_ST1", new Column("YEAR_ST3", DbType.Int32, ColumnProperty.NotNull));

            Database.RenameTable("OVHL_DPKR_CORRECT_ST1", "OVHL_DPKR_CORRECT_ST4");
        }
    }
}