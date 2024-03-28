namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120200")]
    [MigrationDependsOn(typeof(Version_2020113001.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
             "GJI_FILE_REGISTER",
             new RefColumn("RO_ID", ColumnProperty.NotNull, "GJI_CH_REG_RO_ID", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("FILE_ID", ColumnProperty.NotNull, "GJI_CH_REG_FILE_ID", "B4_FILE_INFO", "ID"));

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
            Database.RemoveTable("GJI_FILE_REGISTER");
        }

    }
}
