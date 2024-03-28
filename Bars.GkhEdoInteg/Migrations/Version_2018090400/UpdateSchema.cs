namespace Bars.GkhEdoInteg.Migrations.Version_2018090400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2018090400")]
    [MigrationDependsOn(typeof(Version_2014041400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhEdoInteg", "DeleteViewAppealCits");
            ViewManager.Create(this.Database, "GkhEdoInteg", "CreateViewAppealCits");
        }
    }
}