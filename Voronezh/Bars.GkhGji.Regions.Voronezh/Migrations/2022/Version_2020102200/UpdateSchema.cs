namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020102200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020102200")]
    [MigrationDependsOn(typeof(Version_2020101200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_CH_SMEV_EGRUL",
              new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
              new Column("REQ_INN", DbType.String, ColumnProperty.NotNull, 15),
              new Column("REG_ORG_ADDRESS", DbType.String, 500),
              new Column("ADDRESS_UL", DbType.String, 500),
              new Column("AUT_CAPITAL_AMMOUNT", DbType.Decimal),
              new Column("CAPITAL_TYPE", DbType.String, 500),
              new Column("REG_ORG_CODE", DbType.String, 500),
              new Column("CREATE_WAY_NAME", DbType.String, 500),
              new Column("FIO", DbType.String, 500),
              new Column("ANSWER", DbType.String, 500),
              new Column("INN", DbType.String, 500),
              new Column("KPP", DbType.String, 500),
              new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new Column("NAME", DbType.String, 500),
              new Column("OGRN", DbType.String, 500),
              new Column("OGRN_DATE", DbType.DateTime),
              new Column("OKVED_CODES", DbType.String, 500),
              new Column("OKVED_NAMES", DbType.String, 5000),
              new Column("OPF_NAME", DbType.String, 500),
              new Column("POZITION", DbType.String, 500),
              new Column("REG_ORG_NAME", DbType.String, 500),
              new Column("RESPONCE_DATE", DbType.DateTime),
              new Column("SHORT_NAME", DbType.String, 500),
              new Column("STATE_UL", DbType.String, 500),
              new Column("TYPE_POZITION_NAME", DbType.String, 500),
              new Column("MESSAGE_ID", DbType.String, 500),
              new Column("INN_OGRN", DbType.String, 500),
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_EGRUL_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
                "GJI_CH_SMEV_EGRUL_FILE",
                new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
                new RefColumn("SMEV_EGRUL_ID", ColumnProperty.None, "GJI_CH_SMEGRUL_SMEV_EGRUL_ID", "GJI_CH_SMEV_EGRUL", "ID"),
                new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEGRUL_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
                }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_EGRUL_FILE");
            Database.RemoveTable("GJI_CH_SMEV_EGRUL");
        }
    }
}


