namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018070500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2018070500")]
    [MigrationDependsOn(typeof(Version_2018050300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhGjiTat", "DeleteViewHeatingSeason");
            ViewManager.Create(this.Database, "GkhGjiTat", "CreateViewHeatingSeason");
        }
    }
}