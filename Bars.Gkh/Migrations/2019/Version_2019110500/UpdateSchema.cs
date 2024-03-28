namespace Bars.Gkh.Migrations._2019.Version_2019110500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019110500")]
    [MigrationDependsOn(typeof(Version_2019103100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("GIS_GKH_MATCH_DATE", DbType.DateTime));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("GIS_GKH_ACC_MATCH_DATE", DbType.DateTime));

            Database.AddColumn("GKH_CONTRAGENT", new Column("GIS_GKH_VERSION_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CONTRAGENT", new Column("GIS_GKH_ORGPPA_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "GIS_GKH_ORGPPA_GUID");
            Database.RemoveColumn("GKH_CONTRAGENT", "GIS_GKH_VERSION_GUID");

            Database.RemoveColumn("GKH_REALITY_OBJECT", "GIS_GKH_ACC_MATCH_DATE");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "GIS_GKH_MATCH_DATE");
        }
    }
}