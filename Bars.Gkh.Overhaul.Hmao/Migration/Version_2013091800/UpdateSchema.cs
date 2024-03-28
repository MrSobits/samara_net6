namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                                    "OVHL_DPKR_CORRECT_ST4",
                new Column("FUND_BUDG_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("IS_FIN_FROM_FUND", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_FIN_FROM_MUN", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_FIN_FROM_OTH_SOURCE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_FIN_FROM_OWN_MONEY", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_FIN_FROM_REGION", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("WORK_JOBS_YEAR_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("MUN_BUDGET_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OTHSOURCE_BUDGET_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OWNERS_MONEY_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PLAN_YEAR_ST3", DbType.Int32, ColumnProperty.NotNull),
                new Column("REGION_BUDGET_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("SHARE_BUDGET_FUND", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("SHARE_BUDGET_MUN", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("SHARE_BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("SHARE_BUDGET_OTHSOURCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("SHARE_OWNERS_MONEY", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("YEAR_ST3", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("COMMON_ESTATE_ID", ColumnProperty.NotNull, "OVHL_DPKR_CORRECT_CE", "OVRHL_COMMON_ESTATE_OBJECT", "ID"),
                new RefColumn("REALITYOBJECT_ID", ColumnProperty.NotNull, "OVHL_DPKR_CORRECT_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OVHL_DPKR_CORRECT_W", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVHL_DPKR_CORRECT_ST4");
        }
    }
}