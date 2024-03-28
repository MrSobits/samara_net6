namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013110600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013110401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            /*
            В данной сущности будет распределение дифицита по МО
            */
            Database.AddEntityTable(
                "OVRHL_SHORT_PROG_DIFITSIT",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_DIFITSIT_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_DIFITSIT_V", "OVRHL_PRG_VERSION", "ID"),
                new Column("YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("DIFITSIT", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("REGION_BUDGET_SHARE", DbType.Decimal, ColumnProperty.NotNull, 0));

            /*
            В данной таблице будут хранится Записи краткосрочной программе и разбиение на суммы откуда какие средства будут финансироватся
            (то есть из какиех бюджетов)
            */
            Database.AddEntityTable(
                "OVRHL_SHORT_PROG_REC",
                new RefColumn("DPKR_CORRECT_ID", ColumnProperty.NotNull, "OVRHL_SHORT_PROG_REC_DC", "OVHL_DPKR_CORRECT_ST2", "ID"),
                new Column("OWNER_SUM_CR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_MU", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_FSR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_OTHER_SRC", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("DIFITSIT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SHORT_PROG_DIFITSIT");
            Database.RemoveTable("OVRHL_SHORT_PROG_REC");
        }
    }
}