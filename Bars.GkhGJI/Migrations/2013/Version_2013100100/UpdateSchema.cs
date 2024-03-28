namespace Bars.GkhGji.Migrations.Version_2013100100
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013092700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_GJI_APPCIT_FILE", false, "GJI_APPEAL_CITIZENS", "FILE_ID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_GJI_APPCIT_FILE", "GJI_APPEAL_CITIZENS");
        }
    }
}