namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2017052600
{
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017052600")]
    [MigrationDependsOn(typeof(Version_2017050300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddSequence(FormatDataExportSequences.ProtocolossExportId);
            this.Database.AddExportId("GKH_OBJ_D_PROTOCOL", FormatDataExportSequences.ProtocolossExportId);
            this.Database.AddExportId("DEC_GOV_DECISION", FormatDataExportSequences.ProtocolossExportId);
        }

        public override void Down()
        {
            this.Database.RemoveExportId("GKH_OBJ_D_PROTOCOL");
            this.Database.RemoveExportId("DEC_GOV_DECISION");
            this.Database.RemoveSequence(FormatDataExportSequences.ProtocolossExportId);
        }
    }
}