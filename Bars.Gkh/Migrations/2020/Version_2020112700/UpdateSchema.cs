namespace Bars.Gkh.Migrations._2020.Version_2020112700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2020112700")]
    [MigrationDependsOn(typeof(Version_2020102700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddSequence(FormatDataExportSequences.WorkKprTypeExportId);
            this.Database.AddExportId("OVRHL_STRUCT_EL", FormatDataExportSequences.WorkKprTypeExportId);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveExportId("OVRHL_STRUCT_EL");
        }
    }
}