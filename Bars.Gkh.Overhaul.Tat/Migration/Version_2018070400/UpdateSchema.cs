namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2018070400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2018070400")]
    [MigrationDependsOn(typeof(Bars.Gkh.Overhaul.Tat.Migration.Version_2017052300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhOvrhlTat", "DeleteViewCrObject");
            ViewManager.Create(this.Database, "GkhOvrhlTat", "CreateViewCrObject");
        }
    }
}