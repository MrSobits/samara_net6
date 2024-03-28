namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017070400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2017070400")]
    [MigrationDependsOn(typeof(Version_2017012800.UpdateSchema))]
    [MigrationDependsOn(typeof(Gkh.Migrations._2017.Version_2017070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddSequence(FormatDataExportSequences.OuExportId);

            this.Database.AddExportId("GKH_MAN_ORG_ADD_SERVICE", FormatDataExportSequences.OuExportId);
            this.Database.AddExportId("GKH_MAN_ORG_AGR_SERVICE", FormatDataExportSequences.OuExportId);
            this.Database.AddExportId("GKH_MAN_ORG_COM_SERVICE", FormatDataExportSequences.OuExportId);
        }

        public override void Down()
        {
            this.Database.RemoveExportId("GKH_MAN_ORG_ADD_SERVICE");
            this.Database.RemoveExportId("GKH_MAN_ORG_AGR_SERVICE");
            this.Database.RemoveExportId("GKH_MAN_ORG_COM_SERVICE");

            this.Database.RemoveSequence(FormatDataExportSequences.OuExportId);
        }
    }
}