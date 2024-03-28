namespace Bars.GkhGji.Migrations.Version_2013042500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013042500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_GJI_STAT_REQ_FILE", false, "GJI_STATEMENT_REQUEST", "FILE_INFO_ID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_GJI_STAT_REQ_FILE", "GJI_STATEMENT_REQUEST");
        }
    }
}