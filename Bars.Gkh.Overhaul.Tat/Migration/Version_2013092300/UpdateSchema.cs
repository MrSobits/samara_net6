namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013092300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013091900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("OVHL_DPKR_CORRECT_ST1");
            Database.RemoveTable("OVHL_DPKR_CORRECT_ST2");

            Database.AddEntityTable(
                                    "OVHL_DPKR_CORRECT_ST2",
                new Column("OWNERS_MONEY_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PLAN_YEAR", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("YEAR_COLLECTION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("HAS_CREDIT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("BUDGET_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_MUNICIPALITY", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OTHER_BALANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("REALITYOBJECT_ID", ColumnProperty.NotNull, "OVHL_DPKR_CORR_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("CALC_COMPLETED", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "CALC_COMPLETED");

            Database.RemoveTable("OVHL_DPKR_CORRECT_ST2");
        }
    }
}