namespace Bars.GkhGji.Migrations._2024.Version_2024030106
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030106")]
    [MigrationDependsOn(typeof(Version_2024030105.UpdateSchema))]
    /// Является Version_2018102500 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewDisposal");
        }

        public override void Down()
        {
        }
    }
}