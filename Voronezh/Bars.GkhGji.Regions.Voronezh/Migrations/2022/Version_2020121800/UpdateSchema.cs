namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020121800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020121800")]
    [MigrationDependsOn(typeof(Version_2020121700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_CMEV_GASU",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_CMEV_GASU_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
            new Column("DATE_TO", DbType.DateTime, ColumnProperty.NotNull),
            new Column("ANSWER", DbType.String, 1500),
            new Column("MESSAGEID", DbType.String, 50),
            new Column("MESSAGE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10)
            );

            Database.AddEntityTable(
            "GJI_CH_CMEV_GASU_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("GASU_ID", ColumnProperty.None, "GJI_CH_CMEV_GASU_FILE_GASU_ID", "GJI_CH_CMEV_GASU", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_CMEV_GASU_FILE_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
          

            Database.AddEntityTable(
            "GJI_CH_CMEV_GASU_DATA",
            new Column("INDEX_NAME", DbType.String, 500),
            new Column("INDEX_UID", DbType.String, 50),
            new Column("VALUE", DbType.Decimal,ColumnProperty.None),
            new RefColumn("UNIT_ID", ColumnProperty.None, "GJI_CH_CMEV_GASU_DATA_UNIT_ID", "GKH_DICT_UNITMEASURE", "ID"),
            new RefColumn("GASU_ID", ColumnProperty.None, "GJI_CH_CMEV_GASU_DATA_GASU_ID", "GJI_CH_CMEV_GASU", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_CMEV_GASU_DATA");
            Database.RemoveTable("GJI_CH_CMEV_GASU_FILE");
            Database.RemoveTable("GJI_CH_CMEV_GASU");
        }
    }
}
