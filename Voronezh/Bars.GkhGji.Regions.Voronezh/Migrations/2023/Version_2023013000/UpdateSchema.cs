namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023013000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023013000")]
    [MigrationDependsOn(typeof(Version_2023011900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("PHONE", DbType.String, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("REQ_NUMBER", DbType.String));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("DOC_NUMBER", DbType.String));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("NOTE", DbType.String, 1500));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("DOC_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("PROTOCOL_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("APPLICANT_TYPE", DbType.Int16, ColumnProperty.NotNull, 5));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("PROTOCOL_NUMBER", DbType.String));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new RefColumn("ATTFILE_ID", "GJI_PROTOCOL_OSP_REQUEST_ATTFILE_ID", "B4_FILE_INFO", "ID"));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new RefColumn("PROTFILE_ID", "GJI_PROTOCOL_OSP_REQUEST_PROTFILE_ID", "B4_FILE_INFO", "ID"));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new RefColumn("STATE_ID", "GJI_PROTOCOL_OSP_REQUEST_STATE", "B4_STATE", "ID"));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new RefColumn("PROTOCOL_TYPE", "GJI_PROTOCOL_OSP_PROTOCOL_TYPE", "OVRHL_DICT_PROTTYPE_DECISION", "ID"));
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new RefColumn("INSPECTOR_ID", "GJI_PROTOCOL_OSP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "INSPECTOR_ID");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "PROTOCOL_TYPE");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "STATE_ID");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "PROTFILE_ID");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "ATTFILE_ID");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "PROTOCOL_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "APPLICANT_TYPE");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "PROTOCOL_DATE");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "DOC_DATE");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "NOTE");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "DOC_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "REQ_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "PHONE");
        }
    }
}