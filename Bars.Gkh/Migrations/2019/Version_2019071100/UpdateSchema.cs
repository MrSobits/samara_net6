namespace Bars.Gkh.Migrations._2019.Version_2019071100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019071100")]
    [MigrationDependsOn(typeof(Version_2019071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GKH_ROOM", new Column("GIS_GKH_PREMISES_GUID", DbType.String, 36));
            Database.AddRefColumn("GKH_OPERATOR", new RefColumn("GIS_GKH_CONTRAGENT_ID", "OPERATOR_GIS_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OPERATOR", "GIS_GKH_CONTRAGENT_ID");
            Database.RemoveColumn("GKH_ROOM", "GIS_GKH_PREMISES_GUID");
            Database.RemoveColumn("GKH_CONTRAGENT", "GIS_GKH_GUID");
        }
    }
}