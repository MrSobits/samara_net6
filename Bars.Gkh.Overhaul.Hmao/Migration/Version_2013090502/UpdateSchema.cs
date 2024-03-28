namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013090502
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090502")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013090501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "OVRHL_SUBSIDY_MU",
               new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OVRHL_SUBSIDY_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"),
               new Column("CALC_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0),
               new Column("SET_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddEntityTable(
                "OVRHL_SUBSIDY_MU_REC",
                new RefColumn("SUBSIDY_MU_ID", ColumnProperty.NotNull, "OVRHL_SUBSIDY_MU_REC_SMU", "OVRHL_SUBSIDY_MU", "ID"),
                new Column("SUBCIDY_YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("BUDGET_FUND", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_MU", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OTHER_SRC", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OWNER_FUND", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("CALC_COLLECTION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PLAN_COLLECTION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("DEFICIT", DbType.Decimal, ColumnProperty.NotNull, 0));

        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SUBSIDY_MU_REC");
            Database.RemoveTable("OVRHL_SUBSIDY_MU");
        }
    }
}