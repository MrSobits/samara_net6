namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020111900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020111900")]
    [MigrationDependsOn(typeof(Version_2020111700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_CH_SMEV_FNS_LIC",
              new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
              new Column("REQ_TYPE", DbType.Int32),
              new Column("INN", DbType.String, 100),
              new Column("NAME_UL", DbType.String, 500),
              new Column("OGRN", DbType.String, 100),
              new Column("KIND_LIC", DbType.String, 100),
              new Column("DATE_LIC", DbType.DateTime),
              new Column("DATE_START_LIC", DbType.DateTime),
              new Column("DATE_END_LIC", DbType.DateTime),
              new Column("FIRST_NAME", DbType.String, 100),
              new Column("FAMILY_NAME", DbType.String, 100),
              new Column("NUM_LIC", DbType.String, 100),
              new Column("SER_LIC", DbType.String, 100),
              new Column("SLVD_CODE", DbType.String, 500),
              new Column("VD_NAME", DbType.String, 500),
              new Column("PR_ACTION", DbType.String, 10),
              new Column("ADDRESS", DbType.String, 500),
              new Column("DEC_KIND", DbType.String, 500),
              new Column("DEC_START_DATE", DbType.DateTime),
              new Column("DEC_END_DATE", DbType.DateTime),
              new Column("DEC_DATE", DbType.DateTime),
              new Column("DEC_NUM", DbType.String, 500),
              new Column("DEC_ORG_LIC", DbType.String, 500),
              new Column("LIC_ORG_INN", DbType.String, 100),
              new Column("LIC_ORG_FULL_NAME", DbType.String, 500),
              new Column("LIC_ORG_SHORT_NAME", DbType.String, 500),
              new Column("LIC_ORG_OGRN", DbType.String, 100),
              new Column("LIC_ORG_OKOGU", DbType.String, 100),
              new Column("LIC_ORG_REGION", DbType.String, 100),
              new Column("ANSWER", DbType.String, 500),
              new Column("REQUEST_STATE", DbType.Int32),
              new Column("MESSAGE_ID", DbType.String, 500),
              new Column("ID_DOC", DbType.String, 500),
              new Column("PERSON_TYPE", DbType.Int32),
              new Column("DELETE_ID_DOC", DbType.String, 500),
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_FNS_LIC_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new RefColumn("LICENSE_ID", "GJI_CH_SMEV_FNS_LIC_LIC_ID", "GKH_MANORG_LICENSE", "ID"));


            Database.AddEntityTable(
                "GJI_CH_SMEV_FNS_LIC_FILE",
                new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
                new RefColumn("SMEV_FNS_LIC_ID", ColumnProperty.None, "GJI_CH_SMFNSLIC_SMEV_FNSLIC_ID", "GJI_CH_SMEV_FNS_LIC", "ID"),
                new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMFNSLIC_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_FNS_LIC_FILE");
            Database.RemoveTable("GJI_CH_SMEV_FNS_LIC");
        }
    }
}