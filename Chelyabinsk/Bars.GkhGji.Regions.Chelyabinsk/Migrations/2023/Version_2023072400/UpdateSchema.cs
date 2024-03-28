namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023072400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023072400")]
    [MigrationDependsOn(typeof(Version_2023053100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable("GJI_CH_GIS_ERKNM_DICT",
                new Column("MESSAGE_ID", DbType.String, 36),
                new Column("DICT_GUID", DbType.String, 36),
                new Column("KNM_GUID", DbType.String, 36),
                new Column("KNO_GUID", DbType.String, 36),
                new Column("COMPARE_DATE", DbType.DateTime),
                new Column("ANSWER", DbType.String, 2000));

            Database.AddEntityTable("GJI_CH_GIS_ERKNM_DICT_FILE",
                new RefColumn("GIS_ERKNM_DICT_ID", "GJI_CH_GIS_ERKNM_DICT_FILE_DICT_ID", "GJI_CH_GIS_ERKNM_DICT", "ID"),
                new RefColumn("FILE_INFO_ID", "GJI_CH_GIS_ERKNM_DICT_FILE_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_GIS_ERKNM_DICT_FILE");
            Database.RemoveTable("GJI_CH_GIS_ERKNM_DICT");
        }
    }
}