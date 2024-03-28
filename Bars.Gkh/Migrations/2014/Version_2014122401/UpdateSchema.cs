namespace Bars.Gkh.Migrations.Version_2014122401
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2014122401")]
    [MigrationDependsOn(typeof(Version_2014122400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("DISTRICT", DbType.String, 300));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("UNOM_CODE", DbType.String, 100));
        }

        public override void Down()
        {
            ViewManager.Drop(Database, "Gkh");

            Database.RemoveColumn("GKH_REALITY_OBJECT", "DISTRICT");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "UNOM_CODE");
        }
    }
}