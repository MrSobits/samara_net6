namespace Bars.GkhGji.Migrations._2020.Version_2020051900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020051900")]
    [MigrationDependsOn(typeof(Version_2020051800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_APPEAL_CITIZENS", new RefColumn("CORRESPONDENT_CONTRAGENT_ID", "FK_APPEAL_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("GIS_GKH_PARENT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("GIS_GKH_CONTRAGENT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("GIS_WORK", DbType.Boolean, false));

            Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("REDIRECT_CONTRAGENT_ID", "FK_APPEAL_ANSWER_REDIRECT_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("HASH", DbType.String));

            Database.AddColumn("GJI_APPEAL_CITIZENS_FILES", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GJI_APPEAL_CITIZENS_FILES", new Column("HASH", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS_FILES", "HASH");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS_FILES", "GIS_GKH_GUID");

            Database.RemoveColumn("GJI_APPCIT_ANSWER", "HASH");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "GIS_GKH_ATTACHMENT_GUID");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "GIS_GKH_GUID");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "REDIRECT_CONTRAGENT_ID");

            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "GIS_WORK");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "GIS_GKH_CONTRAGENT_GUID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "GIS_GKH_PARENT_GUID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "GIS_GKH_GUID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "CORRESPONDENT_CONTRAGENT_ID");
        }
    }
}