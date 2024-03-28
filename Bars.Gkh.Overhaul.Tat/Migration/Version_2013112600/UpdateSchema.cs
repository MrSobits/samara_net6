namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Добавляем колонку в 3 этап
            Database.AddColumn("OVRHL_VERSION_REC", new Column("TYPE_DPKR_RECORD", DbType.Int16, 4, ColumnProperty.NotNull, 10));

            // Добавляем во 2й этап
            Database.AddColumn("OVRHL_STAGE2_VERSION", new Column("TYPE_DPKR_RECORD", DbType.Int16, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("OVRHL_STAGE2_VERSION", new Column("TYPE_CORRECTION", DbType.Int16, 4, ColumnProperty.NotNull, 10));

            // Добавляем в 1 этап
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("TYPE_DPKR_RECORD", DbType.Int16, 4, ColumnProperty.NotNull, 10));


            Database.AddRefColumn("OVRHL_STAGE1_VERSION", new RefColumn("STR_EL_ID", "OVRHL_STAGE1_VERSION_ST", "OVRHL_STRUCT_EL", "ID"));

            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("SERVICE_SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0));

            // Добавляем колонки в Краткосрочную программу
            Database.AddColumn("OVRHL_SHORT_PROG_REC", new Column("TOTAL_COST", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SHORT_PROG_REC", new Column("SERVICE_COST", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SHORT_PROG_REC", new Column("TYPE_ACTUALITY", DbType.Int16, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("OVRHL_SHORT_PROG_REC", new Column("TYPE_DPKR_RECORD", DbType.Int16, 4, ColumnProperty.NotNull, 10));
            Database.AddRefColumn("OVRHL_SHORT_PROG_REC", new RefColumn("STAGE1_ID", "OVRHL_SHORT_PROG_REC_ST1", "OVRHL_STAGE1_VERSION", "ID"));

            // удаляем колонку из Краткосрочки
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "STAGE2_ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "TOTAL_COST");
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "SERVICE_COST");
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "TYPE_ACTUALITY");
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "TYPE_DPKR_RECORD");
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "STAGE1_ID");

            // Колонку Stage2_id Целенаправлено не восстанавливаю потому что это было бы грубо и неверно с моей стороны

            Database.RemoveColumn("OVRHL_VERSION_REC", "TYPE_DPKR_RECORD");
            Database.RemoveColumn("OVRHL_STAGE2_VERSION", "TYPE_DPKR_RECORD");
            Database.RemoveColumn("OVRHL_STAGE2_VERSION", "TYPE_CORRECTION");

            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "TYPE_DPKR_RECORD");
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "STR_EL_ID");
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "SERVICE_COST");
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "VOLUME");
        }
    }
}