namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017052400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2017052400")]
    [MigrationDependsOn(typeof(Version_2017042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddSequence(FormatDataExportSequences.OplataPackExportId);
            this.Database.AddExportId("REGOP_BANK_DOC_IMPORT", FormatDataExportSequences.OplataPackExportId);
            this.Database.AddExportId("REGOP_BANK_ACC_STMNT", FormatDataExportSequences.OplataPackExportId);
        }

        public override void Down()
        {
            this.Database.RemoveExportId("REGOP_BANK_DOC_IMPORT");
            this.Database.RemoveExportId("REGOP_BANK_ACC_STMNT");
            this.Database.RemoveSequence(FormatDataExportSequences.OplataPackExportId);
        }
    }
}