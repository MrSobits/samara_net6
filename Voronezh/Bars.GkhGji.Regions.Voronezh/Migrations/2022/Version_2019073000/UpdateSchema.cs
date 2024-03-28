namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019073000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019073000")]
    [MigrationDependsOn(typeof(Version_2019051300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_CH_GIS_ERP",
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

           new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_GIS_ERP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
           new RefColumn("DISPOSAL_ID", ColumnProperty.None, "GJI_CH_GIS_ERP_DISPOSAL_ID", "GJI_DOCUMENT", "ID"),
           new RefColumn("PROSECUTOR_ID", ColumnProperty.None, "GJI_CH_GIS_ERP_PROSECUTOR_ID", "GJI_DICT_PROSECUTOR_OFFICE", "ID"));

            Database.AddEntityTable(
       "GJI_CH_GIS_ERP_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("GIS_ERP_ID", ColumnProperty.None, "GJI_CH_GIS_ERP_ID", "GJI_CH_GIS_ERP", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_GIS_ERP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_GIS_ERP_FILE");
            Database.RemoveTable("GJI_CH_GIS_ERP");
        }
    }
}