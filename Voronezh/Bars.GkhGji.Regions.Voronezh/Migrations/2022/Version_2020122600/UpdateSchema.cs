namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020122600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020122600")]
    [MigrationDependsOn(typeof(Version_2020121900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_SMEV_SOCIAL_HIRE",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_SOC_HIRE_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
             new RefColumn("RO_ID", ColumnProperty.Null, "GJI_CH_SMEV_SOC_HIRE_RO_ID", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("ROOM_ID", ColumnProperty.Null, "GJI_CH_SMEV_SOC_HIRE_ROOM_ID", "GKH_ROOM", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGE_ID", DbType.String, 300),
            new Column("ANSWER", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new Column("CON_NUM", DbType.String, 500),
            new Column("CON_TYPE", DbType.Int32, ColumnProperty.Null),
            new Column("NAME", DbType.String, 500),
            new Column("PURPOSE", DbType.String, 500),
            new Column("TOTAL_AREA", DbType.String, 500),
            new Column("LIVE_AREA", DbType.String, 500),
            new Column("LAST_NAME", DbType.String, 500),
            new Column("FIRST_NAME", DbType.String, 500),
            new Column("MID_NAME", DbType.String, 500),
            new Column("BIRTHDAY", DbType.DateTime, ColumnProperty.Null),
            new Column("BIRTHPLACE", DbType.String, 500),
            new Column("CITIZENSHIP", DbType.String, 500),
            new Column("DOC_TYPE", DbType.Int32, ColumnProperty.Null),
            new Column("DOC_NUM", DbType.String, 500),
            new Column("DOC_SERIES", DbType.String, 500),
            new Column("DOC_DATE", DbType.DateTime, ColumnProperty.Null),
            new Column("IS_OWNER", DbType.Int32, ColumnProperty.Null),
            new Column("ANSWER_DISTRICT", DbType.String, 500),
            new Column("ANSWER_CITY", DbType.String, 500),
            new Column("ANSWER_STREET", DbType.String, 500),
            new Column("ANSWER_HOUSE", DbType.String, 500),
            new Column("ANSWER_FLAT", DbType.String, 500),
            new Column("ANSWER_REGION", DbType.String, 500)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_SOCIAL_HIRE_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_SOCIAL_HIRE_ID", ColumnProperty.None, "GJI_CH_SMEV_SOC_HIRE_FILE_ID", "GJI_CH_SMEV_SOCIAL_HIRE", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_SOC_HIRE_FILE_INFO_ID", "B4_FILE_INFO", "ID"));


            Database.AddEntityTable(
            "GJI_CH_SMEV_EMERGENCY_HOUSE",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EM_HOUSE_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
             new RefColumn("RO_ID", ColumnProperty.Null, "GJI_CH_SMEV_EM_HOUSE_RO_ID", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("ANS_FILE_ID", ColumnProperty.Null, "GJI_CH_SMEV_EM_HOUSE_FI_ID", "B4_FILE_INFO", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGE_ID", DbType.String, 300),
            new Column("ANSWER", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10)
            );

            Database.AddEntityTable(
            "GJI_CH_SMEV_EMERGENCY_HOUSE_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_EM_HOUSE_ID", ColumnProperty.None, "GJI_CH_SMEV_EM_HOUSE_FILE_ID", "GJI_CH_SMEV_EMERGENCY_HOUSE", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EM_HOUSE_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_SOCIAL_HIRE_FILE");
            Database.RemoveTable("GJI_CH_SMEV_EMERGENCY_HOUSE_FILE");
            Database.RemoveTable("GJI_CH_SMEV_EMERGENCY_HOUSE");
            Database.RemoveTable("GJI_CH_SMEV_SOCIAL_HIRE");
        }

    }
}
