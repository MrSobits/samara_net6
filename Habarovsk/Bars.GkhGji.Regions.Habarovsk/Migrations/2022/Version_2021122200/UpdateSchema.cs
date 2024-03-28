namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2021122200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2021122200")]
    [MigrationDependsOn(typeof(Version_2021102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_CH_GIS_ERKNM",
           new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
           new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("ANSWER", DbType.String, 1000),
           new Column("MESSAGEID", DbType.String, 50),
           new Column("CARRYOUT_EVENTS", DbType.String, 5000),
           new Column("NAME", DbType.String, 500),
           new Column("CHECK_ID", DbType.String, 50),
           new Column("ADDRESS_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("INSPECTION_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
           new Column("NOTICE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 20),
           new Column("OBJECT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 40),
           new Column("REASON_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 40),
           new Column("RISK_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 60),
           new Column("REQUEST_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 1),
           new Column("OKATO", DbType.String, 20),
           new Column("GOALS", DbType.String),
           new Column("KND_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("SUBJ_ADDRESS", DbType.String, 500),
           new Column("ACT_DATE_CREATE", DbType.DateTime, ColumnProperty.None),
           new Column("DURATION_HOURS", DbType.Int16, ColumnProperty.None),
           new Column("HAS_VIOLATIONS", DbType.Int32, 4, ColumnProperty.NotNull, 30),
           new Column("NEED_TO_UPDATE", DbType.Int32, 4, ColumnProperty.NotNull, 20),
           new Column("REPRESENTATIVE_FULL_NAME", DbType.String),
           new Column("REPRESENTATIVE_POSITION", DbType.String),
           new Column("START_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("INSPECTION_GUID", DbType.String),
           new Column("INSPECTOR_GUID", DbType.String),
           new Column("OBJECT_GUID", DbType.String),
           new Column("RESULT_GUID", DbType.String),
           new Column("ERPID", DbType.String),

           new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_GIS_ERKNM_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
           new RefColumn("DISPOSAL_ID", ColumnProperty.None, "GJI_CH_GIS_ERKNM_DISPOSAL_ID", "GJI_DOCUMENT", "ID"),
           new RefColumn("PROSECUTOR_ID", ColumnProperty.None, "GJI_CH_GIS_ERKNM_PROSECUTOR_ID", "GJI_DICT_PROSECUTOR_OFFICE", "ID"));



            Database.AddEntityTable(
       "GJI_CH_GIS_ERKNM_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("GIS_ERKNM_ID", ColumnProperty.None, "GJI_CH_GIS_ERKNM_ID", "GJI_CH_GIS_ERKNM", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_GIS_ERKNM_FILE_INFO_ID", "B4_FILE_INFO", "ID"));


            Database.AddEntityTable("GJI_CH_GIS_ERKNM_VIOLATION",
               new Column("CODE", DbType.String, ColumnProperty.None),
               new Column("DATE_APPOINTMENT", DbType.DateTime, ColumnProperty.None),
               new Column("EXECUTION_DEADLINE", DbType.DateTime, ColumnProperty.None),
               new Column("EXECUTION_NOTE", DbType.String, ColumnProperty.None),
               new Column("NUM_GUID", DbType.String, ColumnProperty.None),
               new Column("TEXT", DbType.String, ColumnProperty.None),
               new Column("VIOLATION_ACT", DbType.String, ColumnProperty.None),
               new Column("VIOLATION_NOTE", DbType.String, ColumnProperty.None),
               new Column("VLAWSUIT_TYPE_ID", DbType.Int32, 4, ColumnProperty.NotNull, 30),
               new RefColumn("gis_erp_id", ColumnProperty.None, "GJI_CH_GIS_ERP_VIOLAT_ERKNM_ID", "GJI_CH_GIS_ERKNM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_GIS_ERKNM_VIOLATION");
            Database.RemoveTable("GJI_CH_GIS_ERKNM_FILE");
            Database.RemoveTable("GJI_CH_GIS_ERKNM");
        }
    }
}