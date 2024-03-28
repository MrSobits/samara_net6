namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120600")]
    [MigrationDependsOn(typeof(Version_2020120400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
          "GJI_CH_SMEV_PROPERTY_TYPE",
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_PROPERTY_TYPE_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
          new RefColumn("RO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_PROPERTY_TYPE_RO_ID", "GKH_REALITY_OBJECT", "ID"),
          new RefColumn("ROOM_ID", ColumnProperty.None, "GJI_CH_SMEV_PROPERTY_TYPE_ROOM_ID", "GKH_ROOM", "ID"),
          new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("PROP_LVL", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Null),
          new Column("CADASTRAL_NUMBER", DbType.String, 100),
          new Column("ATTR1", DbType.String, 500),
          new Column("ATTR2", DbType.String, 500),
          new Column("ATTR3", DbType.String, 500),
          new Column("ATTR4", DbType.String, 500),
          new Column("ATTR5", DbType.String, 500));

            Database.AddEntityTable(
       "GJI_CH_SMEV_PROPERTY_TYPE_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("SMEV_PT_ID", ColumnProperty.None, "GJI_CH_SMEV_PROPERTY_TYPE_FILE_REQ_ID", "GJI_CH_SMEV_PROPERTY_TYPE", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_PROPERTY_TYPE_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_PROPERTY_TYPE_FILE");
            Database.RemoveTable("GJI_CH_SMEV_PROPERTY_TYPE");
        }

    }
}
