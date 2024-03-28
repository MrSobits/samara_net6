namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2019082100
{
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2019082100")]
    [MigrationDependsOn(typeof(Version_2017052600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));

            Database.AddColumn("REGOP_DEC_NOTIF", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));

            Database.AddColumn("DEC_GOV_DECISION", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("DEC_GOV_DECISION", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("DEC_GOV_DECISION", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("DEC_GOV_DECISION", "GIS_GKH_ATTACHMENT_GUID");
            Database.RemoveColumn("DEC_GOV_DECISION", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("DEC_GOV_DECISION", "GIS_GKH_GUID");

            Database.RemoveColumn("REGOP_DEC_NOTIF", "GIS_GKH_ATTACHMENT_GUID");

            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "GIS_GKH_ATTACHMENT_GUID");
            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "GIS_GKH_GUID");
        }
    }
}