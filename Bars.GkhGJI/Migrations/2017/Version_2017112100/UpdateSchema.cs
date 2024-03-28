namespace Bars.GkhGji.Migrations._2017.Version_2017112100
{
    using B4.Modules.Ecm7.Framework;
    using Gkh;

    [Migration("2017112100")]
    [MigrationDependsOn(typeof(Version_2017111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.DropAll(this.Database);
            ViewManager.CreateAll(this.Database);
        }
    }
}
