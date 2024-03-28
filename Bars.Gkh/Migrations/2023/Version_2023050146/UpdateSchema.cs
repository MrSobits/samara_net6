namespace Bars.Gkh.Migrations._2023.Version_2023050146
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050146")]

    [MigrationDependsOn(typeof(Version_2023050145.UpdateSchema))]

    /// Является Version_2022040700 из ядра
    public class UpdateSchema : Migration
    {
        private const string InspectorPositionsTable = "GJI_DICT_INSPECTOR_POSITIONS";

        public override void Up()
        {
            this.Database.AddGkhDictTable(UpdateSchema.InspectorPositionsTable);
        }

        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.InspectorPositionsTable);
        }
    }
}