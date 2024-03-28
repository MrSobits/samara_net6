namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020121700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020121700")]
    [MigrationDependsOn(typeof(Version_2020121500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_SMEV_VALID_PASSPORT",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_PASSPORT_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("DOC_ISSUE_DATE", DbType.DateTime),
            new Column("DOC_SERIE", DbType.String, 10),
            new Column("DOC_NUMBER", DbType.String, 10),
            new Column("INVALIDITY_SINCE", DbType.DateTime),
            new Column("DOC_STATUS", DbType.String, 300),
            new Column("MESSAGEID", DbType.String, 300),
            new Column("INVALIDITY_REASON", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_VALID_PASSPORT_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_VP_ID", ColumnProperty.None, "GJI_CH_SMEV_PASSPORT_FILE_ID", "GJI_CH_SMEV_VALID_PASSPORT", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_PASSPORT_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
            "GJI_CH_SMEV_LIVING_PLACE",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_LIVING_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGEID", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("CITIZEN_LASTNAME", DbType.String, 100),
            new Column("CITIZEN_FIRSTNAME", DbType.String, 100),
            new Column("CITIZEN_GIVENNAME", DbType.String, 100),
            new Column("CITIZEN_BIRTHDAY", DbType.DateTime),
            new Column("CITIZEN_SNILS", DbType.String, 20),
            new Column("DOC_TYPE", DbType.String, 100),
            new Column("DOC_SERIE", DbType.String, 10),
            new Column("DOC_NUMBER", DbType.String, 10),
            new Column("DOC_ISSUEDATE", DbType.DateTime),
            new Column("REGION_CODE", DbType.String, 10),
            new Column("LPLACE_REGION", DbType.String, 10),
            new Column("LPLACE_DISTRICT", DbType.String, 50),
            new Column("LPLACE_CITY", DbType.String, 50),
            new Column("LPLACE_STREET", DbType.String, 50),
            new Column("LPLACE_HOUSE", DbType.String, 10),
            new Column("LPLACE_BUILDING", DbType.String, 10),
            new Column("LPLACE_FLAT", DbType.String, 10),
            new Column("REG_STATUS", DbType.String, 10)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_LIVING_PLACE_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_LP_ID", ColumnProperty.None, "GJI_CH_SMEV_LIVING_FILE_ID", "GJI_CH_SMEV_LIVING_PLACE", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_LIVING_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
            "GJI_CH_SMEV_STAYING_PLACE",
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_STAYING_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGEID", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("CITIZEN_LASTNAME", DbType.String, 100),
            new Column("CITIZEN_FIRSTNAME", DbType.String, 100),
            new Column("CITIZEN_GIVENNAME", DbType.String, 100),
            new Column("CITIZEN_BIRTHDAY", DbType.DateTime),
            new Column("CITIZEN_SNILS", DbType.String, 20),
            new Column("DOC_TYPE", DbType.String, 100),
            new Column("DOC_SERIE", DbType.String, 10),
            new Column("DOC_NUMBER", DbType.String, 10),
            new Column("DOC_ISSUEDATE", DbType.DateTime),
            new Column("REGION_CODE", DbType.String, 10),
            new Column("LPLACE_REGION", DbType.String, 10),
            new Column("LPLACE_DISTRICT", DbType.String, 50),
            new Column("LPLACE_CITY", DbType.String, 50),
            new Column("LPLACE_STREET", DbType.String, 50),
            new Column("LPLACE_HOUSE", DbType.String, 10),
            new Column("LPLACE_BUILDING", DbType.String, 10),
            new Column("LPLACE_FLAT", DbType.String, 10),
            new Column("REG_STATUS", DbType.String, 10)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_STAYING_PLACE_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_SP_ID", ColumnProperty.None, "GJI_CH_SMEV_STAYING_FILE_ID", "GJI_CH_SMEV_STAYING_PLACE", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_STAYING_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_STAYING_PLACE_FILE");
            Database.RemoveTable("GJI_CH_SMEV_LIVING_PLACE_FILE");
            Database.RemoveTable("GJI_CH_SMEV_VALID_PASSPORT_FILE");
            Database.RemoveTable("GJI_CH_SMEV_STAYING_PLACE");
            Database.RemoveTable("GJI_CH_SMEV_LIVING_PLACE");
            Database.RemoveTable("GJI_CH_SMEV_VALID_PASSPORT");
        }
    }
}
