namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020071500
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2020071500")]
    [MigrationDependsOn(typeof(Version_2020062200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_NSO_ACTREMOVAL_ANNEX", new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_ACTREMOVAL_ANNEX", "GIS_GKH_ATTACHMENT_GUID");
        }
    }
}