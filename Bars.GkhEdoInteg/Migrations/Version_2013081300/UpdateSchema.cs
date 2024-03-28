namespace Bars.GkhEdoInteg.Migrations.Version_2013081300
{
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013081300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhEdoInteg.Migrations.Version_2013062700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            ViewManager.Drop(Database, "GkhEdoInteg");
            ViewManager.Create(Database, "GkhEdoInteg");
        }

        public override void Down()
        {
            ViewManager.Drop(Database, "GkhEdoInteg");
        }
    }
}