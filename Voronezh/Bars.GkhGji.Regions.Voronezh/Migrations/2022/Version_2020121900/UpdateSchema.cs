namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020121900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020121900")]
    [MigrationDependsOn(typeof(Version_2020121800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_SMEV_EXP_RESOLUTION",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EXP_RESOLUTION_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGE_ID", DbType.String, 300),
            new Column("ANSWER", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("REQUEST_PARAM_TYPE", DbType.Int32),
            new Column("REQUEST_PARAM_TYPE_VALUE", DbType.String, 500),
            new Column("PERMIT_NUMBER", DbType.String, 50),
            new Column("PERMIT_DATE", DbType.DateTime, ColumnProperty.Null),
            new Column("ISSUE_ORGAN", DbType.String, 300),
            new Column("OBJECT_NAME", DbType.String, 1000),
            new Column("OKATO", DbType.String, 20),
            new Column("KLADR", DbType.String, 20),
            new Column("POSTAL_CODE", DbType.String, 20),
            new Column("REGION", DbType.String, 100),
            new Column("STREET", DbType.String, 300),
            new Column("HOUSE", DbType.String, 10),
            new Column("KORP", DbType.String, 10),
            new Column("CADASTRAL_ZU", DbType.String, 50)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_EXP_RESOLUTION_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_EXP_RESOLUTION_ID", ColumnProperty.None, "GJI_CH_SMEV_EXP_RESOLUTION_FILE_ID", "GJI_CH_SMEV_EXP_RESOLUTION", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EXP_RESOLUTION_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
            "GJI_CH_SMEV_CHANGE_PREM_STATE",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_CHANGE_PREM_STATE_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGE_ID", DbType.String, 300),
            new Column("ANSWER", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("CHANGE_PREMISES_TYPE", DbType.Int32),
            new RefColumn("RO_ID", ColumnProperty.Null, "GJI_CH_SMEV_CHANGE_PREM_STATE_RO_ID", "GKH_REALITY_OBJECT", "ID"),
            new RefColumn("ROOM_ID", ColumnProperty.Null, "GJI_CH_SMEV_CHANGE_PREM_STATE_ROOM_ID", "GKH_ROOM", "ID"),
            new Column("CADASTRAL_NUMBER", DbType.String, 50),
            new Column("DECLARANT_TYPE", DbType.String, 10),
            new Column("DECLARANT_NAME", DbType.String, 500),
            new Column("DECLARANT_ADDRESS", DbType.String, 1024),
            new Column("DEPARTMENT", DbType.String, 1024),
            new Column("AREA", DbType.String, 20),
            new Column("CITY", DbType.String, 100),
            new Column("STREET", DbType.String, 300),
            new Column("HOUSE", DbType.String, 10),
            new Column("BLOCK", DbType.String, 10),
            new Column("APARTMENT", DbType.String, 10),
            new Column("ROOM_TYPE", DbType.Int32),
            new Column("APPOINTMENT", DbType.String, 1024),
            new Column("ACT_NUMBER", DbType.String, 50),
            new Column("ACT_NAME", DbType.String, 300),
            new Column("ACT_DATE", DbType.DateTime, ColumnProperty.Null),
            new Column("OLD_PREM_TYPE", DbType.String, 10),
            new Column("NEW_PREM_TYPE", DbType.String, 10),
            new Column("CONDITION_TRANSFER", DbType.String, 1024),
            new Column("RESPONSIBLE_NAME", DbType.String, 300),
            new Column("RESPONSIBLE_POST", DbType.String, 300),
            new Column("RESPONSIBLE_DATE", DbType.DateTime, ColumnProperty.Null)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_CHANGE_PREM_STATE_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_CHANGE_PREM_STATE_ID", ColumnProperty.None, "GJI_CH_SMEV_CHANGE_PREM_STATE_FILE_ID", "GJI_CH_SMEV_CHANGE_PREM_STATE", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_CHANGE_PREM_STATE_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_EXP_RESOLUTION_FILE");
            Database.RemoveTable("GJI_CH_SMEV_CHANGE_PREM_STATE_FILE");
            Database.RemoveTable("GJI_CH_SMEV_EXP_RESOLUTION");
            Database.RemoveTable("GJI_CH_SMEV_CHANGE_PREM_STATE");
        }

    }
}
