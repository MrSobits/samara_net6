namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112202.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Полностью удаляем старые таблицы поскольку уже ненужны
            Database.RemoveTable("OVRHL_SM_RECORD_VERSION");
            Database.RemoveTable("OVRHL_SUBSIDY_MU_REC");
            Database.RemoveTable("OVRHL_SUBSIDY_MU");
            
            Database.AddEntityTable("OVRHL_SUBSIDY_MU",
                new RefColumn("MU_ID", ColumnProperty.NotNull, "OVRHL_SUBSIDY_MU_GKH_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable("OVRHL_SUBSIDY_MU_REC",
                new RefColumn("SUBSIDY_MU_ID", ColumnProperty.NotNull, "OVRHL_SUB_MU_REC_SUB", "OVRHL_SUBSIDY_MU", "ID"),
                new Column("SUBCIDY_YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("BUDGET_FCR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_MUNICIPALITY", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("OWNER_SOURCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("BUDGET_CR", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddEntityTable("OVRHL_SM_RECORD_VERSION",
                new RefColumn("SUBSIDY_REC_UNV_ID", ColumnProperty.NotNull, "SM_REC_VERSION_MUREC", "OVRHL_SUBSIDY_MU_REC", "ID"),
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "SM_REC_VERSION_VER", "OVRHL_PRG_VERSION", "ID"),
                new Column("NEED_FINANCE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("DEFICIT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            // Туту неделаем никаких откатов поскольку ненужно чтобы старые таблицы восстанавливались
        }
    }
}