namespace Bars.GkhGji.Migrations._2020.Version_2020050700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020050700")]
    [MigrationDependsOn(typeof(Version_2020050600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DOCUMENT", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GJI_DOCUMENT", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));

            Database.AddColumn("GJI_RESOLUTION", new Column("SURNAME", DbType.String));
            Database.AddColumn("GJI_RESOLUTION", new Column("FIRSTNAME", DbType.String));
            Database.AddColumn("GJI_RESOLUTION", new Column("PATRONYMIC", DbType.String));
            Database.AddColumn("GJI_RESOLUTION", new Column("POSITION", DbType.String));

            Database.AddColumn("GJI_RESOLUTION_ANNEX", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));

            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("GIS_GKH_CODE", DbType.String));
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "GIS_GKH_GUID");
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "GIS_GKH_CODE");

            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "GIS_GKH_ATTACHMENT_GUID");

            Database.RemoveColumn("GJI_RESOLUTION", "POSITION");
            Database.RemoveColumn("GJI_RESOLUTION", "PATRONYMIC");
            Database.RemoveColumn("GJI_RESOLUTION", "FIRSTNAME");
            Database.RemoveColumn("GJI_RESOLUTION", "SURNAME");

            Database.RemoveColumn("GJI_DOCUMENT", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GJI_DOCUMENT", "GIS_GKH_GUID");
        }
    }
}