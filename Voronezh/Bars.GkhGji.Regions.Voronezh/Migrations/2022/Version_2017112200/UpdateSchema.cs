namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2017112200
{
    using B4.Modules.Ecm7.Framework;
    using Gkh;

    [Migration("2017112200")]
    [MigrationDependsOn(typeof(Version_2017051900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhGjiVoronezh");
            ViewManager.Create(this.Database, "GkhGjiVoronezh");
        }
    }
}