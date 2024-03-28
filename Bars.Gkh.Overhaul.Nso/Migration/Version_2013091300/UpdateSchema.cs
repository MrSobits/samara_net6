namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013091300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013091202.UpdateSchema))]
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



            //Удаялем колонки в Субсидировании
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "SET_TARIF");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "CALC_TARIF");

            //Добавляем новые колонки в Субсидировании
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("START_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("COEF_GROWTH_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("COEF_SUM_RISK", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("DATE_RETURN_LOAN", DbType.Int32, ColumnProperty.NotNull, 0));

            //Записи субсидирования
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "OWNER_FUND");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "DEFICIT");

            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("FINANCE_NEED_BEFORE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("FINANCE_NEED_AFTER", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("CALC_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("RECOMMEND_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("SHARE_BUDGET_FUND", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("SHARE_BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("SHARE_BUDGET_MU", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("SHARE_OTHER_SRC", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("SHARE_OWNER_FOUNDS", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("DEFICIT_BEFORE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("DEFICIT_AFTER", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            //Колонки субсидирования
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "START_TARIF");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "COEF_GROWTH_TARIF");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "COEF_SUM_RISK");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "DATE_RETURN_LOAN");
            
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("SET_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("CALC_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));

            //Колонки для Записи субсидирования
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "FINANCE_NEED_BEFORE");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "FINANCE_NEED_AFTER");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "CALC_TARIF");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "RECOMMEND_TARIF");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "SHARE_BUDGET_FUND");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "SHARE_BUDGET_REGION");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "SHARE_BUDGET_MU");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "SHARE_OTHER_SRC");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "SHARE_OWNER_FOUNDS");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "DEFICIT_BEFORE");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "DEFICIT_AFTER");

            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("OWNER_FUND", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("DEFICIT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }
    }
}