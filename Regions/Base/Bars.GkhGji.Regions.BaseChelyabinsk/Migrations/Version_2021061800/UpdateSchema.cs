namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021061800
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021061800")]
    [MigrationDependsOn(typeof(Version_2021060200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
            "SMEV_CH_CERT_INFO",
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "SMEV_CH_CERT_INFO_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
            new Column("REQUEST_DATE", DbType.DateTime, ColumnProperty.NotNull),
            new Column("ANSWER", DbType.String, 1000),
            new Column("MESSAGE_ID", DbType.String, 1000),
            new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "SMEV_CH_CERT_INFO_ID", "B4_FILE_INFO", "ID")
            );

            Database.AddEntityTable(
            "SMEV_CH_CERT_INFO_FILE",
            new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
            new RefColumn("CERT_INFO_ID", ColumnProperty.None, "SMEV_CH_CERT_INFO_FILE_ID", "SMEV_CH_CERT_INFO", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "SMEV_CH_CERT_INFO_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("SMEV_CH_CERT_INFO_FILE");
            Database.RemoveTable("SMEV_CH_CERT_INFO");
        }

    }
}