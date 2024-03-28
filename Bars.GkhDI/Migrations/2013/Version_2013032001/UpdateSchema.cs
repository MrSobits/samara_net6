namespace Bars.GkhDi.Migrations.Version_2013032001
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        // Часть 2 миграций
        public override void Up()
        {
            // Добавление столбца "Статус" к сущности
            Database.AddRefColumn("DI_DISINFO", new RefColumn("STATE_ID", "DI_DISINFO_STATE", "B4_STATE", "ID"));

            // Перенес документы из фин деятельности в отдельную сущность
            Database.RemoveColumn("DI_DISINFO_FIN_ACTIVITY", "BOOKKEEP_BALANCE");
            Database.RemoveColumn("DI_DISINFO_FIN_ACTIVITY", "BOOKKEEP_BALANCE_ANNEX");

            //-----Документы финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FINACT_DOCS",
                new RefColumn("DISINFO_ID", ColumnProperty.NotNull, "DI_DISINFO_FADOC_DI", "DI_DISINFO", "ID"),
                new RefColumn("BOOKKEEP_BALANCE", "DI_DISINFO_FADOC_BB", "B4_FILE_INFO", "ID"),
                new RefColumn("BOOKKEEP_BALANCE_ANNEX", "DI_DISINFO_FADOC_BBA", "B4_FILE_INFO", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));
            
            // Изменил тип колонки
            Database.ChangeColumn("DI_OTHER_SERVICE", new Column("CODE", DbType.String, 50));

            // Снял признак обязательности (not null) с колонки
            Database.ChangeColumn("DI_ADMIN_RESP", new RefColumn("SUPERVISORY_ORG_ID", "DI_ADMIN_RESP_SO", "DI_DICT_SUPERVISORY_ORG", "ID"));
        }

        public override void Down()
        {
            // Удаление столбца "Статус"
            Database.RemoveColumn("DI_DISINFO", "STATE_ID");

            Database.RemoveTable("DI_DISINFO_FINACT_DOCS");

            Database.AddRefColumn("DI_DISINFO_FIN_ACTIVITY", new RefColumn("BOOKKEEP_BALANCE", "FK_DI_DISINFO_FA_BB", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("DI_DISINFO_FIN_ACTIVITY", new RefColumn("BOOKKEEP_BALANCE_ANNEX", "FK_DI_DISINFO_FA_BBA", "B4_FILE_INFO", "ID"));
        }
    }
}