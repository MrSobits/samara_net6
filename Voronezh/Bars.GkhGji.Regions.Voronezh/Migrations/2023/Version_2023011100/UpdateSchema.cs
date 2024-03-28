namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023011100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023011100")]
    [MigrationDependsOn(typeof(Version_2022121200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("CADASTRAL_NUMBER", DbType.String, 50));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("ATTORNEY_NUMBER", DbType.String, 150));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("ATTORNEY_FIO", DbType.String, 250));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("DATE_FROM", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("DATE_TO", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("ATTORNEY_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new RefColumn("FILE_INFO_ID", "GJI_PROTOCOL_OSP_REQUEST_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "FILE_INFO_ID");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "ATTORNEY_DATE");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "DATE_TO");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "DATE_FROM");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "ATTORNEY_FIO");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "ATTORNEY_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "CADASTRAL_NUMBER");
        }
    }
}