namespace Bars.Gkh.Gis.Migrations._2015.Version_2015032400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015032400")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015020400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            // Таблицы, отображающие связь районов со схемами в базе для структуры КП 5.0.
            // Данные для таблиц получены от БЦ Биллинга и ЖКХ

            // GIS_KP50_DISTRICT = subs_rajon_gor (список районов)
            this.Database.AddPersistentObjectTable("GIS_KP50_DISTRICT",
                new Column("KP50_CODE", DbType.Int32, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull));

            // GIS_KP50_BASE = s_point (список банков)
            this.Database.AddPersistentObjectTable("GIS_KP50_BASE",
                new Column("KP50_CODE", DbType.Int32, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 200, ColumnProperty.NotNull),
                new Column("PREFIX", DbType.String, 20, ColumnProperty.NotNull),
                new RefColumn("DISTRICT_ID", "GIS_KP50_BASE_DIST_ID", "GIS_KP50_DISTRICT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_KP50_BASE");
            this.Database.RemoveTable("GIS_KP50_DISTRICT");
        }
    }
}