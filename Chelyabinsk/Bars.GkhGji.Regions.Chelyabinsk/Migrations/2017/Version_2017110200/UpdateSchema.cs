namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017110200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017110200")]
    [MigrationDependsOn(typeof(Version_2017110100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Справочник кодов регионов
            Database.AddEntityTable(
                "GJI_CH_DICT_REGCODE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_CH_REGCODE_NAME", false, "GJI_CH_DICT_REGCODE", "NAME");
            Database.AddIndex("IND_GJI_CH_REGCODE__CODE", false, "GJI_CH_DICT_REGCODE", "CODE");

            Database.AddEntityTable(
          "GJI_CH_SMEV_MVD",
          new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("TYPE_ADDRESS_PRIMARY", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("TYPE_ADDRESS_ADDITIONAL", DbType.Int32, 4, ColumnProperty.None),
          new Column("ADDRESS_PRIMARY", DbType.String, 500),
          new Column("ADDRESS_ADDITIONAL", DbType.String, 500),
          new Column("BIRTH_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("SNILS", DbType.String, 20),
          new Column("SURNAME", DbType.String, 50),
          new Column("NAME", DbType.String, 50),
          new Column("MESSAGE_ID", DbType.String, 500),
          new Column("ANSWER", DbType.String, 500),
          new Column("PATRONYMIC", DbType.String, 50),
          new RefColumn("REG_CODE_PRIMARY_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_MVD_REGCODEPRIM_ID", "GJI_CH_DICT_REGCODE", "ID"),
          new RefColumn("REG_CODE_ADDITIONAL_ID", ColumnProperty.None, "GJI_CH_SMEV_MVD_REGCODEADDIT_ID", "GJI_CH_DICT_REGCODE", "ID"),
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_MVD_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
       "GJI_CH_SMEV_MVD_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("SMEV_MVD_ID", ColumnProperty.None, "GJI_CH_SMMVD_SMEV_MVD_ID", "GJI_CH_SMEV_MVD", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_MVD_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_MVD_FILE");
            Database.RemoveTable("GJI_CH_SMEV_MVD");
            Database.RemoveTable("GJI_CH_DICT_REGCODE");
        }
    }
}