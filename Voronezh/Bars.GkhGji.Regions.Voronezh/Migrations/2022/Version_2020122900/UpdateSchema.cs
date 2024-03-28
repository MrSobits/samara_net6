namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020122900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020122900")]
    [MigrationDependsOn(typeof(Version_2020122800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_CH_SMEV_REDEVELOPMENT",
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_REDEV_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new RefColumn("RO_ID", ColumnProperty.Null, "GJI_CH_SMEV_REDEV_RO_ID", "GKH_REALITY_OBJECT", "ID"),
            new RefColumn("ROOM_ID", ColumnProperty.Null, "GJI_CH_SMEV_REDEV_ROOM_ID", "GKH_ROOM", "ID"),
            new RefColumn("ANS_FILE_ID", ColumnProperty.Null, "GJI_CH_SMEV_REDEV_ANS_FILE_ID", "B4_FILE_INFO", "ID"),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
           new Column("MESSAGE_ID", DbType.String, 300),
           new Column("ANSWER", DbType.String, 300),
           new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("GOV_NAME", DbType.String, 500),
           new Column("OBJECT_NAME", DbType.String, 500),
           new Column("CADASTRAL", DbType.String, 500),
           new Column("ACT_NUM", DbType.String, 500),
           new Column("ACT_DATE", DbType.DateTime, ColumnProperty.Null)
           );

            Database.AddEntityTable(
            "GJI_CH_SMEV_REDEVELOPMENT_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_REDEV_ID", ColumnProperty.None, "GJI_CH_SMEV_REDEV_FILE_ID", "GJI_CH_SMEV_REDEVELOPMENT", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_REDEV_FILE_INFO_ID", "B4_FILE_INFO", "ID"));


            Database.AddEntityTable(
            "GJI_SMEV_OW_PROPERTY",
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_OW_PROP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
             new RefColumn("RO_ID", ColumnProperty.Null, "GJI_CH_SMEV_OW_PROP_RO_ID", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("ROOM_ID", ColumnProperty.Null, "GJI_CH_SMEV_OW_PROP_ROOM_ID", "GKH_ROOM", "ID"),
             new RefColumn("ANS_FILE_ID", ColumnProperty.Null, "GJI_CH_SMEV_OW_PROP_FI_ID", "B4_FILE_INFO", "ID"),
             new Column("CADASTRAL", DbType.String, 500),
             new Column("QUERY_TYPE", DbType.Int32, ColumnProperty.Null),
             new Column("PROP_LEVEL", DbType.Int32, ColumnProperty.Null),
             new Column("REG_NUM", DbType.String, 500),
            new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("MESSAGE_ID", DbType.String, 300),
            new Column("ANSWER", DbType.String, 300),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10)
            );

            Database.AddEntityTable(
            "GJI_SMEV_OW_PROPERTY_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("SMEV_OW_PROP_ID", ColumnProperty.None, "GJI_CH_SMEV_OW_PROP_FILE_ID", "GJI_SMEV_OW_PROPERTY", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_OW_PROP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_SMEV_OW_PROPERTY_FILE");
            Database.RemoveTable("GJI_CH_SMEV_REDEVELOPMENT_FILE");
            Database.RemoveTable("GJI_SMEV_OW_PROPERTY");
            Database.RemoveTable("GJI_CH_SMEV_REDEVELOPMENT");
        }

    }
}
