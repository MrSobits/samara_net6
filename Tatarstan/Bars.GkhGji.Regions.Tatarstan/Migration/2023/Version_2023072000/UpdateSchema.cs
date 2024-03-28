namespace Bars.GkhGji.Regions.Tatarstan.Migration._2023.Version_2023072000
{
	using Bars.B4.Modules.Ecm7.Framework;
	using Bars.Gkh;

	[Migration("2023072000")]
    [MigrationDependsOn(typeof(Version_2023013100.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2023.Version_2023021500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGjiTatarstan", "CreateViewDisposal");
        }
    }
}