namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120301
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120301")]
    [MigrationDependsOn(typeof(Version_2020120300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
          "GJI_CH_SMEV_EGRIP",
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRIP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
          new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("REQ_INN", DbType.String, ColumnProperty.NotNull, 15),
          new Column("REG_ORG_ADDRESS", DbType.String, 500),
          new Column("REG_ORG_CODE", DbType.String, 500),
          new Column("CREATE_WAY_NAME", DbType.String, 500),
          new Column("FIO", DbType.String, 500),
          new Column("ANSWER", DbType.String, 500),
          new Column("CITIZENSHIP", DbType.String, 500),
          new Column("IP_TYPE", DbType.String, 500),
          new Column("REGION_NAME", DbType.String, 500),
          new Column("REGION_TYPE", DbType.String, 500),
          new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("OGRN", DbType.String, 500),
          new Column("OGRN_DATE", DbType.DateTime),
          new Column("OKVED_CODES", DbType.String, 500),
          new Column("OKVED_NAMES", DbType.String, 500),
          new Column("REG_ORG_NAME", DbType.String, 500),
          new Column("RESPONCE_DATE", DbType.DateTime),
          new Column("MESSAGE_ID", DbType.String, 500),
          new Column("CITY_NAME", DbType.String, 500),
          new Column("CITY_TYPE", DbType.String, 500),
          new Column("INN_OGRN", DbType.String, 500));

          

            //Добавились позже
            //Property(x => x.CityName, "Наименование населённого пункта").Column("CITY_NAME");
            //Property(x => x.CityType, "Тип населённого пункта").Column("CITY_TYPE");
            //Property(x => x.InnOgrn, "InnOgrn").Column("INN_OGRN");

            Database.AddEntityTable(
       "GJI_CH_SMEV_EGRIP_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("SMEV_EGRIP_ID", ColumnProperty.None, "GJI_CH_SMEGRIP_SMEV_EGRIP_ID", "GJI_CH_SMEV_EGRIP", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEGRIP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_EGRIP_FILE");
            Database.RemoveTable("GJI_CH_SMEV_EGRIP");
        }

    }
}
