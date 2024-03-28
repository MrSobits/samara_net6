namespace Bars.GkhGji.Migrations._2017.Version_2017112900
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017112900")]
    [MigrationDependsOn(typeof(Version_2017112500.UpdateSchema))]
    public class UpdateSchema : Migration
    {

        public override void Up()
        {
            this.Database.ExecuteNonQuery("ALTER TABLE gji_appeal_citizens ALTER COLUMN appeal_uid TYPE uuid  USING appeal_uid::uuid");
            this.Database.AddIndex("IDX_GJI_APPEAL_CITIZENS_APPEAL_UID", true, "GJI_APPEAL_CITIZENS", "APPEAL_UID");
        }

        public override void Down()
        {
           this.Database.RemoveIndex("IDX_GJI_APPEAL_CITIZENS_APPEAL_UID", "GJI_APPEAL_CITIZENS");
        }
    }
}
