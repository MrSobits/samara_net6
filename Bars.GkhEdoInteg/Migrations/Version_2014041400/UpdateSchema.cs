namespace Bars.GkhEdoInteg.Migrations.Version_2014041400
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhEdoInteg.Migrations.Version_2013090400.UpdateSchema))]
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