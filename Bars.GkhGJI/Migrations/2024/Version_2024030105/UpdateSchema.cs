namespace Bars.GkhGji.Migrations._2024.Version_2024030105
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030105")]
    [MigrationDependsOn(typeof(Version_2024030104.UpdateSchema))]
    /// Является Version_2018102200 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewFormatDataExportInspection");
        }

        public override void Down()
        {
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteViewFormatDataExportInspection");
        }
    }
}