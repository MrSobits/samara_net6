namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019080500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019080500")]
    [MigrationDependsOn(typeof(Version_2019073000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_CH_EFFECTIVE_INDEX",
                new Column("TYPE_KND", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("YEAR_ENUM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CURRENT_INDEX", DbType.Decimal, ColumnProperty.None),
                new Column("TARGET_INDEX", DbType.Decimal, ColumnProperty.None),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300, ColumnProperty.None));

            Database.AddEntityTable("GJI_CH_TYPE_KND",
                new Column("TYPE_KND", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_TO", DbType.DateTime, ColumnProperty.None));

            Database.AddEntityTable("GJI_CH_TYPE_KND_ARTLAW",
                new RefColumn("TYPE_KND_ID", ColumnProperty.None, "KND_ARTLAW_TYPEKND_ID", "GJI_CH_TYPE_KND", "ID"),
                new Column("KOEFFICIENT", DbType.Int32, 4, ColumnProperty.None, 10),
                new RefColumn("ARTICLELAW_ID", ColumnProperty.None, "KND_ARTLAW_ARTLAW_ID", "GJI_DICT_ARTICLELAW", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_CATEGORY",
                new Column("CALC_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TYPE_KND", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("MKD_AREA_TOTAL", DbType.Decimal, ColumnProperty.None),
                new Column("MONTH_COUNT", DbType.Int32, 5, ColumnProperty.None),
                new Column("RESULT", DbType.Decimal, ColumnProperty.None),
                new Column("CATEGORY", DbType.Int32, 4, ColumnProperty.None),
                new RefColumn("STATE_ID", "GJI_ROM_CATEGORY_STATE", "B4_STATE", "ID"),
                new Column("VN_AMMOUNT", DbType.Int32, 5, ColumnProperty.None),
                new Column("VP_AMMOUNT", DbType.Int32, 5, ColumnProperty.None),
                new Column("VPR_AMMOUNT", DbType.Int32, 5, ColumnProperty.None),
                new Column("YEAR", DbType.Int32, 4, ColumnProperty.NotNull, 2018),
                new RefColumn("INSPECTOR_ID", ColumnProperty.None, "ROM_CATEGORY_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "ROM_CATEGORY_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_CATEGORY_ROBJECT",
                new Column("DATE_START", DbType.DateTime, ColumnProperty.None),
                new RefColumn("RO_ID", ColumnProperty.None, "ROM_CATEGORY_RO_REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_RO_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_VN_RESOLUTION",
                new Column("ART_LAW_NAMES", DbType.String, ColumnProperty.None),
                new RefColumn("RESOLUTION_ID", ColumnProperty.None, "ROM_CATEGORY_VN_RESOLUTION_ID", "GJI_RESOLUTION", "ID"),
                new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_VN_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_VP_RESOLUTION",
                new Column("ART_LAW_NAMES", DbType.String, ColumnProperty.None),
                new RefColumn("RESOLUTION_ID", ColumnProperty.None, "ROM_CATEGORY_VP_RESOLUTION_ID", "GJI_RESOLUTION", "ID"),
                new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_VP_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_VPR_PRESCRIPTION",
                new Column("STATE_NAME", DbType.String, ColumnProperty.None),
                new RefColumn("PRESCRIPTION_ID", ColumnProperty.None, "ROM_CATEGORY_VPR_PRESCRIPTION_ID", "GJI_PRESCRIPTION", "ID"),
                new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_VPR_PRESCR_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_VPR_RESOLUTION",
                new Column("ART_LAW_NAMES", DbType.String, ColumnProperty.None),
                new RefColumn("RESOLUTION_ID", ColumnProperty.None, "ROM_CATEGORY_VPR_RESOLUTION_ID", "GJI_RESOLUTION", "ID"),
                new RefColumn("ROM_CATEGORY_ID", ColumnProperty.None, "ROM_CATEGORY_VPR_CATEGORY_ID", "GJI_CH_ROM_CATEGORY", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_CALC_TASK",
                new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("YEAR", DbType.Int32, 4, ColumnProperty.NotNull, 2018),
                new Column("CALC_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CALC_STATE", DbType.String, 500),
                new RefColumn("FILE", ColumnProperty.None, "ROM_CALC_TASK_FILE_ID", "B4_FILE_INFO", "ID"),
                new RefColumn("INSPECTOR_ID", ColumnProperty.None, "ROM_CALC_TASK_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable("GJI_CH_ROM_CALC_TASK_CONTRAGENT",
                new RefColumn("ROM_CALC_TASK_ID", ColumnProperty.NotNull, "ROM_CALC_TASK_CONTRAGENT_TASK_ID", "GJI_CH_ROM_CALC_TASK", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "ROM_CALC_TASK_CONTRAGENT_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_EFFECTIVE_INDEX");
            Database.RemoveTable("GJI_CH_TYPE_KND_ARTLAW");
            Database.RemoveTable("GJI_CH_TYPE_KND");
            Database.RemoveTable("GJI_CH_ROM_CATEGORY_ROBJECT");
            Database.RemoveTable("GJI_CH_ROM_CATEGORY");
            Database.RemoveTable("GJI_CH_ROM_VN_RESOLUTION");
            Database.RemoveTable("GJI_CH_ROM_VP_RESOLUTION");
            Database.RemoveTable("GJI_CH_ROM_VPR_PRESCRIPTION");
            Database.RemoveTable("GJI_CH_ROM_VPR_RESOLUTION");
            Database.RemoveTable("GJI_CH_ROM_CALC_TASK_CONTRAGENT");
            Database.RemoveTable("GJI_CH_ROM_CALC_TASK");
        }
    }
}