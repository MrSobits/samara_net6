namespace Bars.Gkh.Migrations.Version_2015020900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015020900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015020300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveConstraint("GKH_MANORG_LICENSE", "FK_GKH_MANORG_LICENSE_R");
            Database.AddForeignKey("FK_GKH_MANORG_LICENSE_R", "GKH_MANORG_LICENSE", "REQUEST_ID", "GKH_MANORG_LIC_REQUEST", "ID");
        }

        public override void Down()
        {
            //не требуется
        }
    }
}