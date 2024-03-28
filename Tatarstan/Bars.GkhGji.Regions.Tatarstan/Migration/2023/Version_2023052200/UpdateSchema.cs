namespace Bars.GkhGji.Regions.Tatarstan.Migration._2023.Version_2023052200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2023052200")]
    [MigrationDependsOn(typeof(_2022.Version_2022112300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2023042800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGjiTatarstan", "CreateViewWarningDoc");
        }
    }
}