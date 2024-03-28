namespace Bars.GkhGji.Migrations._2020.Version_2020052600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020052600")]
    [MigrationDependsOn(typeof(Version_2020051900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECK_ANNEX", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_DISPOSAL_ANNEX", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
            Database.AddColumn("GJI_PROTOCOL_ANNEX", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "GIS_GKH_ATTACHMENT_GUID");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "GIS_GKH_ATTACHMENT_GUID");
            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "GIS_GKH_ATTACHMENT_GUID");
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "GIS_GKH_ATTACHMENT_GUID");
        }
    }
}