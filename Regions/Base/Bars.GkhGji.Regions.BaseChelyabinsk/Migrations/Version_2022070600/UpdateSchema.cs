namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022070600
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using System.Data;

    [Migration("2022070600")]
    [MigrationDependsOn(typeof(Version_2022040500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "GJI_CH_SMEV_MVDPASSPORT",
              new Column("ANSWER", DbType.String, 1500),
              new Column("ANSWER_INFO", DbType.String, 5500),
              new Column("BIRTH_DATE", DbType.DateTime, ColumnProperty.None),
              new Column("BIRTH_PLACE", DbType.String, 500),
              new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
              new RefColumn("FILE_ID", ColumnProperty.None, "GJI_CH_SMEV_MVDPASSPORT_FILE_ID", "B4_FILE_INFO", "ID"),
              new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_MVDPASSPORT_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
              new Column("ISSUE_DATE", DbType.DateTime, ColumnProperty.None),
              new Column("MESSAGE_ID", DbType.String, 500),
              new Column("REQUEST_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new Column("NAME", DbType.String, 50),
              new Column("PASSPORT_NUM", DbType.String, 6),
              new Column("PASSPORT_SERIES", DbType.String, 4),
              new Column("PATRONYMIC", DbType.String, 50),
              new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new Column("SURNAME", DbType.String, 50));

            Database.AddEntityTable(
           "GJI_CH_SMEV_MVDPASSPORT_FILE",
           new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
           new RefColumn("MVDPASSPORT_ID", ColumnProperty.None, "GJI_CH_SMEV_MVDPASSPORT_FILE_MVD_ID", "GJI_CH_SMEV_MVDPASSPORT", "ID"),
           new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_MVDPASSPORT_FILE_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_MVDPASSPORT_FILE");
            Database.RemoveTable("GJI_CH_SMEV_MVDPASSPORT");
        }

    }
}