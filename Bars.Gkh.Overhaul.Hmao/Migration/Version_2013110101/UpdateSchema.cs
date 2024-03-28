namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013110101
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013110100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            /*
            В данной таблице будет хранится только то что независит от черсий, то есть
            Плановые собираемости по домам, бюджеты, проценты
            */
            Database.AddEntityTable(
                "OVRHL_SUBSIDY_REC",
                new Column("SUBCIDY_YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_MU", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_FSR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_OTHER_SRC", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PLAN_OWN_COLLECTION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PLAN_OWN_PRC", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("NOT_REDUCE_SIZE_PRC", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OWNER_SUM_CR", DbType.Decimal, ColumnProperty.NotNull, 0));

            /*
            В этой таблице будет то что зависит от версии Поскольку вкаждой версии могут быть свои СУммы на КР 
            и соответсвенно РАспделение по Бюджетам будет уже другое (Нежели чем в другой версии) 
            */
            Database.AddEntityTable(
                "OVRHL_SUBSIDY_REC_VERSION",
                new RefColumn("SUBSIDY_REC_ID", ColumnProperty.NotNull, "OVRHL_SUBSIDY_RECVERS_S", "OVRHL_SUBSIDY_REC", "ID"),
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_SUBSIDY_RECVERS_V", "OVRHL_PRG_VERSION", "ID"),
                new Column("BUDGET_CR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("CORRECTION_FINANCES", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SUBSIDY_REC_VERSION");
            Database.RemoveTable("OVRHL_SUBSIDY_REC");
        }
    }
}