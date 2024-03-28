namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020121500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020121500")]
    [MigrationDependsOn(typeof(Version_2020121000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_SMEV_SNILS",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_SNILS_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("BIRTH_DATE", DbType.DateTime),
            new Column("COUNTRY", DbType.String, 100),
            new Column("DISTRICT", DbType.String, 100),
            new Column("ISSUE_DATE", DbType.DateTime),
            new Column("ISSUER", DbType.String, 300),
            new Column("SNILS", DbType.String, 30),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("SURNAME", DbType.String, 500),
            new Column("MESSAGE_ID", DbType.String, 500),
            new Column("ANSWER", DbType.String, 500),
            new Column("NUMBER", DbType.String, 50),
            new Column("REGION", DbType.String, 150),
            new Column("SERIES", DbType.String, 50),
            new Column("SETTELMENT", DbType.String, 150),
            new Column("GENDER", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("PLACE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("NAME", DbType.String, 150),
            new Column("PATRONYMIC", DbType.String, 150));

            Database.AddEntityTable(
            "GJI_CH_SMEV_SNILS_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_SNILS_ID", ColumnProperty.None, "GJI_CH_SMEV_SNILS_FILE_SNILS_ID", "GJI_CH_SMEV_SNILS", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_SNILS_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
          "GJI_CH_SMEV_NDFL",
          new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("BIRTH_DATE", DbType.DateTime),
          new Column("PERIOD_YEAR", DbType.String, 5),
          new Column("SERVICE_CODE", DbType.String, 50),
          new Column("REG_DATE", DbType.DateTime),
          new Column("SNILS", DbType.String, 20),
          new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("DOCUMENT_CODE", DbType.Int32),
          new Column("MESSAGE_ID", DbType.String, 500),
          new Column("ANSWER", DbType.String, 500),
          new Column("FAMILY_NAME", DbType.String, 50),
          new Column("FIRST_NAME", DbType.String, 50),
          new Column("PATRONYMIC", DbType.String, 50),
          new Column("REQUEST_ID", DbType.String, 50),
          new Column("REG_NUMBER", DbType.String, 50),
          new Column("SERIES_NUMBER", DbType.String, 50),
          new Column("INNUL", DbType.String, 50),
          new Column("KPP", DbType.String, 50),
          new Column("ORG_NAME", DbType.String, 500),
          new Column("RATE", DbType.Int32, ColumnProperty.Null),
          new Column("REVENUE_CODE", DbType.String, 50),
          new Column("MONTH", DbType.String, 50),
          new Column("REVENUE_SUM", DbType.Decimal, ColumnProperty.Null),
          new Column("RECOUPMENT_CODE", DbType.String, 50),
          new Column("RECOUPMENT_SUM", DbType.Decimal, ColumnProperty.Null),
          new Column("DUTY_BASE", DbType.Decimal, ColumnProperty.Null),
          new Column("DUTY_SUM", DbType.Decimal, ColumnProperty.Null),
          new Column("UNRETENTION_SUM", DbType.Decimal, ColumnProperty.Null),
          new Column("REVENUE_TOTAL_SUM", DbType.Decimal, ColumnProperty.Null),
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_SMEV_NDFL_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
            "GJI_CH_SMEV_NDFL_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_NDFL_ID", ColumnProperty.None, "GJI_SMEV_NDFL_ID", "GJI_CH_SMEV_NDFL", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_SMEV_NDFL_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_NDFL_FILE");
            Database.RemoveTable("GJI_CH_SMEV_NDFL");
            Database.RemoveTable("GJI_CH_SMEV_SNILS_FILE");
            Database.RemoveTable("GJI_CH_SMEV_SNILS");
        }
    }
}
