namespace Bars.Gkh.Migrations._2023.Version_2023050115
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050115")]

    [MigrationDependsOn(typeof(Version_2023050114.UpdateSchema))]

    /// Является Version_2018100500 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveExportId("CR_OBJ_CONTRACT");
            this.Database.RemoveExportId("CR_OBJ_BUILD_CONTRACT");
        }

        public override void Down()
        {
        }
    }
}