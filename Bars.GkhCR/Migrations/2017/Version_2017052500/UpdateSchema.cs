namespace Bars.GkhCr.Migrations._2017.Version_2017052500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2017052500")]
    [MigrationDependsOn(typeof(Version_2017042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhCr", "CreateViewCrObject");
        }
    }
}