namespace Bars.GkhGji.Regions.Tatarstan.Migration._2023.Version_2023042800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2023042800")]
    [MigrationDependsOn(typeof(Version_2023013100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGjiTatarstan", "CreateViewWarningDoc");
        }
    }
}