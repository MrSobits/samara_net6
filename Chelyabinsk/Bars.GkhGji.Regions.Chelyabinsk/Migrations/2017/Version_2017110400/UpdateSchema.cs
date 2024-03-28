namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017110400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017110400")]
    [MigrationDependsOn(typeof(Version_2017110300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Справочник кодов регионов
            Database.AddEntityTable(
                "GJI_CH_DICT_FLDOC_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));
         

            Database.AddEntityTable(
          "GJI_CH_SMEV_EGRIP",
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
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRIP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

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
            Database.RemoveTable("GJI_CH_DICT_FLDOC_TYPE");

        }
    }
}