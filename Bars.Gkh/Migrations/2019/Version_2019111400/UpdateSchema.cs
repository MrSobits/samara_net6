namespace Bars.Gkh.Migrations._2019.Version_2019111400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111400")]
    [MigrationDependsOn(typeof(Version_2019111300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "GIS_GKH_GUID");
        }
    }
}